using System.Collections.Generic;

namespace JASON_Compiler
{
    public static class Tiny_Compiler
    {
        public static Scanner Jason_Scanner = new Scanner();

        public static List<string> Lexemes = new List<string>();
        public static List<Token> TokenStream = new List<Token>();


        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner

            Jason_Scanner.StartScanning(SourceCode);
            //Parser
            //Sematic Analysis
        }





    }
}
