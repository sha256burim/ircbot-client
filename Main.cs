using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//What we need for the client to work

using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace Rudebot
{
    class Client 
    {
        static void Main(string[] args)
        {
            //Consider writing this to a config file so you won't have to come back and hardcode everything everytime
            var Connection = new ServerConnect
                (
                server: "input server address here",
                port: 6667, //default irc port
                user: "USER user1234 0 * :user1234", //change user1234 to your username or whatever, consult RFC
                nick: "yournick",
                pass: "yourpass",
                channel: "#channel"
                );

            Connection.Start(); //Starts the connection to the IRC server. 



        }
    }
}
