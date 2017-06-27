using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO;
using System.Collections;
using System.Reflection;



namespace Rudebot
{

    class RudeChat
    {

        public string _reply; //Think of maybe making this a get set function

        public void respond(string line)
        {

            string _line = line; //Set's the line function/method parameter to the string line inside the function
            try
            {
                line.ToLower().Respond(out _reply);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _reply = ex.Message;
            }

        }

        //This method is not being used yet
        public void Log(string line)
        {
            string _line = line; //Same functionality as above, depending on when it is called. 

            using (var File = new System.IO.StreamWriter("input path to file here", true))
            {
                File.WriteLine(_line + "\n"); //Does something idk
            }

        }


    }
}
