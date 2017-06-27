using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//what we need for the client to work, down below.

using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Rudebot
{
    class ServerConnect
    {
        //Server and port to connect to.
        private readonly string _server; //ip or url
        private readonly int _port;  //server port default: 6667

        //User Info defined in RFC 2812, see Documentation.
        //Add links to the documentation later.
        private readonly string _user;
        private readonly string _nick;
        private readonly string _pass;

        private readonly string _channel;
        private readonly int _maxRetries;

        //ServerConnect Constructor
        public ServerConnect
            (
            string server,
            int port,
            string user,
            string nick,
            string pass,
            string channel,
            int maxRetries = 3 //times to retry the connection
            )
        {
            _server = server;
            _port = port;
            _user = user;
            _nick = nick;
            _pass = pass;
            _channel = channel;
            _maxRetries = maxRetries;
        }

        //Starts the connection to the server
        public void Start()
        {
            var retry = false;
            var retryCount = 0;

            do
            {
                try
                {
                    using (var irc = new TcpClient(_server, _port))
                    using (var stream = irc.GetStream())
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream))

                    {
                        writer.WriteLine("PASS " + _pass); //According to the IRC RFC 2812 the password has to be streamed first
                        writer.WriteLine("NICK " + _nick); //Then the the users nick.
                        writer.Flush(); 
                        writer.WriteLine(_user);
                        writer.Flush();

                        while (true)
                        {
                            
                            string inputLine; //Where what is coming from the server will be stored.

                            while ((inputLine = reader.ReadLine()) != null) //While inputline from the stream reader is empty
                            {
                            
                                Console.WriteLine(">>" + inputLine); //write >> to the beginning of the console

                                //Split the lines sent from the server by spaces (Parsing by delimiter ' ' //Whitespace)
                                string[] splitInput = inputLine.Split(new Char[] { ' ' });
                                
                                //This handles the ping response from the server which pings the client every so often to check connection. 
                                //If the ping doesn't respond the connection times out.
                                if (splitInput[0] == "PING")
                                {
                                    string PongReply = splitInput[1];
                                    //Console.WriteLine(">>PONG " + PongReply);
                                    writer.WriteLine("PONG " + PongReply);
                                    writer.Flush();
                                    //continue
                                }
                                
                                //Checks if rudebot is called
                                if (inputLine.Contains("!rudebot"))
                                {
                                    string stringsend = inputLine; //This is the string to send to the RudeChat class for analysis

                                    //*******TODO: Figure out a way so that when you implement a while loop nothing braks the connection this time
                                    //Also ******TODO: Put this abomination inside a while loop or something.

                                    //These three if statements removes everythign leading up to the final "!"
                                    if (inputLine.Contains("!"))
                                    {
                                        int count = stringsend.IndexOf("!");
                                        stringsend = stringsend.Remove(0, count+1); //Note: Because the index of the word starts at 0, you have to add +1 to the count
                                    }
                                    if (inputLine.Contains("!"))
                                    {
                                        int count = stringsend.IndexOf("!");
                                        stringsend = stringsend.Remove(0, count+1);
                                    }
                                    if (inputLine.Contains("!"))
                                    {
                                        int count = stringsend.IndexOf("!");
                                        stringsend = stringsend.Remove(0, count+1);
                                    }

                                    //After everything up to the last "!" is removed, we need to remove the remaining letters "rudebot " Which is 7 chars + a whitespace.
                                    stringsend = stringsend.Remove(0, 8); 
                                    Console.WriteLine(stringsend); //Confirms what it will send to the RudeChat class.

                                    var Chatreply = new RudeChat(); //Creates an object from Rudechat. insinde this scope. 
                                    //*********TODO: ^ think about wraping this in a using and putting it as a global var so it wont create
                                    //the object each time, because that's unecessary and takes up response time. 

                                    Chatreply.respond(stringsend); //sends stringsend to RudeChat.respond.
                                    string thisreply = Chatreply._reply; //Gets the repsonse from RudeChat
                                    
                                    writer.WriteLine("PRIVMSG " + _channel + " " + thisreply);  //Streams Response to the Server. 
                                    writer.Flush();
                                }




                                //*******TODO: Make own class for this
                                switch (splitInput[1])
                                {
                                    //see numeric reply in IRC, see documentation
                                    case "001":
                                        writer.WriteLine("JOIN " + _channel);
                                        writer.Flush();
                                        //See case "353"
                                        break;
                                    
                                    default:
                                        break;
                                } //switch

                            } //while
                        } //while (true), Is this an infinite loop that checks each time the code executes?
                        //This ^ is really inneficient and should be considered for fixing.
                    } //using (((())))

                } //tryr
                catch (Exception e) //Exception handling duh. 
                {
                    //Shows the exception, sleeps for a little while and then tries
                    // establish a new connection to the IRC server
                    Console.WriteLine(e.ToString());
                    Thread.Sleep(5000);
                    retry = ++retryCount <= _maxRetries;
                    //throw;
                }

            } while (retry);
        }

    }
}
