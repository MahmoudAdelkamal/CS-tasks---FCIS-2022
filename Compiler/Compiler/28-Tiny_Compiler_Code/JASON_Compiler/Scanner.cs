using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    //Reserved
    Integer, Float, String, Read, Write, Repeat, Until, If, Elseif, Else, Then, Return, Endl, End, Main,

    //Operators
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, LBrace, RBrace, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp, Or, And, Assignment,

    Idenifier, Constant, StringLiteral, Comment
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
            ReservedWords.Add("int", Token_Class.Integer);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("main", Token_Class.Main);

            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LBrace);
            Operators.Add("}", Token_Class.RBrace);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("||", Token_Class.Or);
            Operators.Add("&&", Token_Class.And);
            Operators.Add(":=", Token_Class.Assignment);
        }

        public void StartScanning(string SourceCode)
        {
            Tokens.Clear();
            Errors.Error_List.Clear();

            int lastIndex = -1;

            for (int i = 0; i < SourceCode.Length - 1;)
            {
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();
                int j = i + 1;

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                {
                    i = j;
                    lastIndex = j;
                    continue;
                }

                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                    CurrentChar = SourceCode[j];
                    while (isLetter(CurrentChar) || isDigit(CurrentChar))
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j < SourceCode.Length)
                            CurrentChar = SourceCode[j];
                        else
                            break;
                    }
                }
                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    CurrentChar = SourceCode[j];
                    while (isDigit(CurrentChar) || CurrentChar == '.')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j < SourceCode.Length)
                            CurrentChar = SourceCode[j];
                        else
                            break;
                    }
                }
                else if (CurrentChar == '/')
                {
                    CurrentChar = SourceCode[j];
                    if (CurrentChar == '*')
                    { //Comment beginning
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        CurrentChar = SourceCode[j];
                        int k = j + 1;
                        char NextChar;
                        while (j < SourceCode.Length && k < SourceCode.Length)
                        {
                            NextChar = SourceCode[k];
                            if (CurrentChar == '*' && NextChar == '/')
                            {
                                j += 2;
                                CurrentLexeme += "*/";
                                break;
                            }
                            else
                            {
                                CurrentLexeme += CurrentChar.ToString();
                                j++;
                                k++;
                                CurrentChar = NextChar;
                            }
                        }
                    }
                }
                else if (CurrentChar == '"')
                { //String literal
                    CurrentChar = SourceCode[j];
                    while (CurrentChar != '"')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j < SourceCode.Length)
                            CurrentChar = SourceCode[j];
                        else
                            break;
                    }
                    if (j < SourceCode.Length && CurrentChar == '"')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                    }
                }
                else
                { //Bi-Operators
                    char NextChar = SourceCode[j];
                    if (CurrentChar == '&' && NextChar == '&' ||
                       CurrentChar == '|' && NextChar == '|' ||
                       CurrentChar == '<' && NextChar == '>' ||
                       CurrentChar == ':' && NextChar == '=')
                    {
                        CurrentLexeme += NextChar.ToString();
                        j++;
                    }

                }

                FindTokenClass(CurrentLexeme);
                i = j;
                lastIndex = j;
            }

            if (lastIndex == SourceCode.Length - 1)
            {
                FindTokenClass(SourceCode[lastIndex].ToString());
            }

            Tiny_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex)
        {
            if (Lex.Length == 0)
                return;

            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;

            //Is it a reserved word?
            if (ReservedWords.ContainsKey(Tok.lex))
            {
                TC = ReservedWords[Tok.lex];
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it an identifier?
            else if (isIdentifier(Tok.lex))
            {
                TC = Token_Class.Idenifier;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it a Constant?
            else if (isConstant(Tok.lex))
            {
                TC = Token_Class.Constant;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it an operator?
            else if (Operators.ContainsKey(Tok.lex))
            {
                TC = Operators[Tok.lex];
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it a string literal?
            else if (isStringLiteral(Tok.lex))
            {
                TC = Token_Class.StringLiteral;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }

            //Is it a comment?
            else if (isComment(Tok.lex))
            {
                TC = Token_Class.Comment;
                Tok.token_type = TC;
                Tokens.Add(Tok);
            }
            else
                Errors.Error_List.Add(Lex);
        }


        bool isIdentifier(string lex)
        {
            bool isValid = true;

            if (isLetter(lex[0]))
            {
                for (int i = 1; i < lex.Length; i++)
                {
                    if (!isLetter(lex[i]) && !isDigit(lex[i]))
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            else
                isValid = false;
            return isValid;
        }

        bool isConstant(string lex)
        {
            bool isValid = true;
            bool isDecimal = false;

            int i;
            for (i = 0; i < lex.Length && isDigit(lex[i]); i++) ;
            if (i != lex.Length)
            {
                if (lex[i] != '.')
                    isValid = false;
                else
                {
                    i++;
                    isDecimal = true;
                }
            }
            if (isDecimal)
            {
                if (i == lex.Length)
                    isValid = false;
                else
                    for (; i < lex.Length && isDigit(lex[i]); i++) ;
            }

            if (i != lex.Length)
                isValid = false;

            return isValid;
        }

        bool isStringLiteral(string lex)
        {
            bool isValid = true;
            int len = lex.Length;
            if (!(lex[0] == '"' && lex[len - 1] == '"'))
                isValid = false;
            return isValid;
        }

        bool isComment(string lex)
        {
            bool isValid = true;
            int len = lex.Length;
            if (!(lex[0] == '/' && lex[1] == '*' && lex[len - 2] == '*' && lex[len - 1] == '/'))
                isValid = false;
            return isValid;
        }

        bool isLetter(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }

        bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}