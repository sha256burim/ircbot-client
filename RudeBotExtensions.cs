using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rudebot
{
    public static class RudeBotExtensions
    {
        public static void RespondBackup(this string _line, string _reply)
        {
            //Makes a text stream reader to read from the memory.txt file
            using (var File = new System.IO.StreamReader("C:\\Users\\Stockholm\\Desktop\\Rudebot\\Rudebot\\memory.txt", true))
            {
                while (!File.EndOfStream) //While not at the end of file
                {
                    string identifier = File.ReadLine(); //Set identifier to the Line

                    if (identifier == _line) //if Identifier is equal to the _line
                    {
                        Console.WriteLine("<< Reply found"); //Line is found, yay
                        string reply = File.ReadLine(); //Reply to the channel with the found response


                        _reply = reply; //set the reply to the function/method reply/response
                        //SayThis.rudebotsay = reply;
                        Console.WriteLine(reply); //this should be streamed out
                        return; //returns nothing. Not even null. Causes an exception from the serverside to be send
                    } //if

                } //while

            } //using

        }

        public static void Respond(this string _line, out string _reply)
        {
            _reply = "Got no idea ;)";
            try
            {
                //string toLowerLine = _line.ToLower();
                //string path = @"C:\Users\Stockholm\Desktop\Rudebot\Rudebot\memory.txt";
                var text = File.ReadAllText("interview.json");
                var lines = JsonConvert.DeserializeObject<List<Couple>>(text);

                _reply = lines.FirstOrDefault(x => x.Question.ToLower() == _line)?.Answer ?? "Got No Idea";

                //for (int i = 0; i < lines.Count; i++)
                //{
                //    if (lines[i].ToLower() == _line)
                //    {
                //        _reply = lines[i + 1];
                //        Console.WriteLine($"I Found it on {i}th");
                //        break;
                //    }
                //}

                Console.WriteLine(_reply); //this should be streamed out
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(_reply);
                _reply = "I'm Confused ;)";
            }
        }

        public static void RespondUsingLinqSynthax(this string _line, out string _reply)
        {
            try
            {
                _reply = File.ReadAllLines(@"C:\Users\Stockholm\Desktop\Rudebot\Rudebot\memory.txt").FirstOrDefault(l => l.ToLower() == _line.ToLower()) ?? "Got no idea ;)";

                Console.WriteLine(_reply);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _reply = ex.Message;
            }
        }
    }
}
