using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


public enum Token_Class
{
    Int, Float, String, Read, Write,
    Repeat, Until, If, Elseif, Else, Then, Return, Endl, End,
    Semicolon, Comma, LParanthesis, RParanthesis,
    EqualOp, NotEqualOp, LessThanOp, GreaterThanOp, AndOp, OrOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp, AssignOp, Idenifier, Number,Comment, LogicalAndOp, LogicalOrOp, LCurlyBraces, RCurlyBraces
}
namespace JASON_Compiler
{

    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);

            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("&&", Token_Class.LogicalAndOp);
            Operators.Add("||", Token_Class.LogicalOrOp);
            Operators.Add(":=", Token_Class.AssignOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("{", Token_Class.LCurlyBraces);
            Operators.Add("}", Token_Class.RCurlyBraces);
        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                    continue;

                j++;

                if (char.IsLetter(CurrentChar))
                {
                    for (; j < SourceCode.Length && char.IsLetter(SourceCode[j]); j++)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                    }
                    FindTokenClass(CurrentLexeme);
                }
                else if (char.IsDigit(CurrentChar))
                {
                    for (; j < SourceCode.Length && char.IsDigit(SourceCode[j]); j++)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                    }
                    if (SourceCode[j] == '.')
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                        j++;

                        for (; j < SourceCode.Length && char.IsDigit(SourceCode[j]); j++)
                        {
                            CurrentChar = SourceCode[j];
                            CurrentLexeme += CurrentChar;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '/' && SourceCode[j] == '*')
                {
                    for (; j < SourceCode.Length && !(j > 1 && SourceCode[j - 2] == '*' && SourceCode[j - 1] == '/'); j++)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                    }
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '\"')
                {
                    for (; j < SourceCode.Length; j++)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar;
                        if (CurrentChar == '\"')
                        {
                            j++;
                            break;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }
                else
                {
                    if (Operators.ContainsKey(CurrentLexeme + SourceCode[j]))
                    {
                        CurrentLexeme += SourceCode[j];
                        j++;
                    }
                    FindTokenClass(CurrentLexeme);
                }
                i = j - 1;
            }
            Tiny_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;

            if (IsComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }
            else if (IsString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            else if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            else if (IsIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            else if (IsNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else
                Errors.Error_List.Add("Error at " + Lex);

        }

        public bool IsIdentifier(string lex)
        {
            Regex reg = new Regex(@"^([a-zA-Z])([0-9a-zA-Z])*$", RegexOptions.Compiled);
            return reg.IsMatch(lex);
        }
        public bool IsNumber(string lex)
        {
            Regex reg = new Regex(@"^(\+|-)?[0-9]+(\.([0-9])+)?$", RegexOptions.Compiled);
            return reg.IsMatch(lex);
        }
        public bool IsComment(string lex)
        {
            return (lex.Length >= 4 && lex.StartsWith("/*") && lex.EndsWith("*/"));
        }
        public bool IsString(string lex)
        {
            return (lex.Length >= 4 && lex.StartsWith("\"") && lex.EndsWith("\""));
        }
    }
}