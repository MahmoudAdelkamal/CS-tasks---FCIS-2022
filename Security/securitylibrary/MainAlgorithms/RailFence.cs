using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            int ans=0;
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            char c = cipherText[1];
            for (int i = 0; i < plainText.Length; i++)
			{
                if(c==plainText[i])
				{
                    ans = i;
                    if (plainText[i] == plainText[i + 1]) ans++;
                    break;
				}
			}
            return ans;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, int key)
        {
            string plainText = "";
            double val = Math.Ceiling(Convert.ToDouble(cipherText.Length) / key);
            char[,] ch = new char[key, ((int)val) ];
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < ((int)Math.Ceiling(val)); j++)
                {
                    ch[i, j] = '*';
                }
            }
            int x = 0;
			for (int i = 0; i < key; i++)
			{
				for (int j = 0; j < ((int)Math.Ceiling(val)); j++)
				{

                    ch[i, j] = cipherText[x];
                    x++;
                    if (x == cipherText.Length) break;
				}
                if (x == cipherText.Length) break;
            }
            for (int i = 0; i <  ((int)Math.Ceiling(val)); i++)
            {
                Boolean ok = false;
                for (int j = 0; j < key; j++)
                {

                    if (ch[j, i] == '*')
                    {
                        ok = true;
                        break;
                    }
                    plainText += ch[j, i];
                }

                if (ok) break;
            }
            return plainText;
            //  throw new NotImplementedException();
        }

        public string Encrypt(string plainText, int key)
        {
            string cipherText="";
            char[,] ch = new char[key, plainText.Length / key + 1];
			for (int i = 0; i < key; i++)
			{
				for (int j = 0; j < plainText.Length / key + 1; j++)
				{
                    ch[i, j] = '*';
				}
			}
            int col = 0;
            for (int i = 0; i < plainText.Length; i++)
			{

				for (int j = 0; j < key; j++)
				{
                    ch[j, col] = plainText[i];
                    i++;
                    if (i == plainText.Length) break;
				}
                i--;
                col++;
			}

			for (int i = 0; i < key; i++)
			{
				for (int j = 0; j < plainText.Length / key + 1; j++)
				{
                    if (ch[i, j] == '*')
                        continue;
                    cipherText += ch[i, j];
				}
            }
			return cipherText;
            //throw new NotImplementedException();
        }
    }
}
