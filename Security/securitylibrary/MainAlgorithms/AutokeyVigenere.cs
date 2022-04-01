using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {

            string keyStream = "";
            cipherText = cipherText.ToUpper();
            plainText = plainText.ToUpper();

            int idx = 0;

            char[,] matrix = new char[26, 26];

            char c = 'A';
            for (int i = 0; i < 26; i++)
            {
                char cn = c;
                for (int j = 0; j < 26; j++)
                {
                    matrix[i, j] = cn;
                    if (cn == 'Z')
                        cn = 'A';
                    else
                        cn++;
                }
                c++;
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (matrix[plainText[i] - 'A', j] == cipherText[i])
                    {
                        keyStream += Convert.ToChar(j + 'A');
                        break;
                    }
                }
            }
            string key = "";
            for (int i = 0; i < keyStream.Length; i++)
            {
                if (keyStream[i]== plainText[0] && keyStream[i+1]==plainText[1]) break;
                key += keyStream[i];
            }
            
            return key;
            //   throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            string keyStream = key;
           

            char[,] matrix = new char[26, 26];

            char c = 'A';
            for (int i = 0; i < 26; i++)
            {
                char cn = c;
                for (int j = 0; j < 26; j++)
                {
                    matrix[i, j] = cn;
                    if (cn == 'Z')
                        cn = 'A';
                    else
                        cn++;
                }
                c++;
            }
            int idx = 0;
            for (int i = 0; i < keyStream.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (matrix[keyStream[i] - 'A', j] == cipherText[i])
                    {
                        plainText += Convert.ToChar(j + 'A');
                        break;
                    }
                }
                idx = i;
            }
            idx++;
            for (int i = 0; plainText.Length<cipherText.Length; i++,idx++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (matrix[plainText[i] - 'A', j] == cipherText[idx])
                    {
                        plainText += Convert.ToChar(j + 'A');
                        break;
                    }
                }
            }
            return plainText;
            // throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            int idx = 0;
            string cipherText = "";
            plainText = plainText.ToUpper();
            key = key.ToUpper();
            while (key.Length < plainText.Length)
            {
                key += plainText[idx];
                idx++;
            }
            char[,] matrix = new char[26, 26];

            char c = 'A';
            for (int i = 0; i < 26; i++)
            {
                char cn = c;
                for (int j = 0; j < 26; j++)
                {
                    matrix[i, j] = cn;
                    if (cn == 'Z')
                        cn = 'A';
                    else
                        cn++;
                }
                c++;
            }

            for (int i = 0; i < plainText.Length; i++)
            {
                cipherText += matrix[plainText[i] - 'A', key[i] - 'A'];
            }
            return cipherText;
            // throw new NotImplementedException();
        }
    }
}
