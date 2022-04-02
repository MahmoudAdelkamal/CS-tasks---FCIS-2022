using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
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
                key += keyStream[i];
                if (keyStream.Substring(i + 1, key.Length) == key) break;
			}
            return key;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            string keyStream = key;
            int idx = 0;
            while (keyStream.Length < cipherText.Length)
            {
                keyStream += key[idx];
                idx++;
                if (idx == key.Length) idx = 0;
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
            string keyStream = key;
            while (keyStream.Length<plainText.Length)
			{
                keyStream += key[idx];
                idx++;
                if (idx == key.Length) idx = 0;
			}
            char[,] matrix = new char[26, 26];

            char c = 'A';
			for (int i = 0; i < 26; i++)
			{
                char cn = c;
				for (int j = 0; j < 26; j++)
				{
                    matrix[i,j] = cn;
                    if (cn == 'Z')
                        cn = 'A';
                    else
                        cn++;
				}
                c++;
			}
			
			for (int i = 0; i < plainText.Length; i++)
			{
                cipherText += matrix[plainText[i] - 'A', keyStream[i] - 'A'];
			}
            return cipherText;
          //  throw new NotImplementedException();
        }
    }
}