using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k2s.Cli.Helpers
{
    public static class Outputs
    {
        public static void Info(string prefix, string msg)
        {



            Generic(prefix, Color.Default.ToString(), Color.Default.ToString(), msg, Color.Cyan1.ToString(), Color.Default.ToString());

        }

        public static void Success(string prefix, string msg) {

           

            Generic(prefix, Color.Default.ToString(), Color.Default.ToString(), msg, Color.Green.ToString(), Color.Default.ToString());

        }
        public static void Warning(string prefix, string msg)
        {



            Generic(prefix, Color.Default.ToString(), Color.Default.ToString(), msg, Color.Yellow.ToString(), Color.Default.ToString());

        }
        public static void Error(string prefix, string msg)
        {



            Generic(prefix, Color.Default.ToString(), Color.Default.ToString(), msg, Color.Red.ToString(), Color.Default.ToString());

        }

        public static void Generic(string prefix,string prefixColor, string prefixBgColor, string msg, string msgColor, string msgBgColor, bool newlines=false)
        {

            if(newlines) AnsiConsole.WriteLine();
            AnsiConsole.Markup($"[{prefixColor} on {prefixBgColor}] {prefix} [/]");
            AnsiConsole.Markup($"[{msgColor} on {msgBgColor}] {msg} [/]");
            AnsiConsole.WriteLine();


        }


    }
}
