using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            
            int row = 0,col=0,idx=0;
            if (cipherText[0] == plainText[0])
            {
                for (int i = 1; i < plainText.Length; i++)
                {
                    if (cipherText[1] == plainText[i])
                    {
                        col = i;
                        row = ((int)Math.Ceiling(Convert.ToDouble(plainText.Length) / col));
                        break;
                    }
                }
            }
			else
			{
                for (int i = 2; i < plainText.Length; i++)
                {
                    if (cipherText[0] == plainText[i])
                    {
						for (int j = i+2; j < plainText.Length; j++)
						{
                            if(cipherText[1]==plainText[j])
							{
                                col = j-i;
                                row = ((int)Math.Ceiling(Convert.ToDouble(plainText.Length) / col));
                                break;
                            }
						}
                        break;
                    }
                }
            }
            /*for (int i = 0; i < cipherText.Length; i++)
			{
				while (idx<plainText.Length)
				{
                    if (plainText[idx] == cipherText[i])
                    {
                        row++;
                        idx++;
                        break;
                    }
                    idx++;
				}
                if (idx == plainText.Length) break;
			}
            col = ((int)Math.Ceiling(Convert.ToDouble(plainText.Length) / row));
            */
            char[,] ch = new char[row, col];
            idx = 0;
			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < col; j++)
				{
                    if(idx==plainText.Length)
					{
                        ch[i, j] = '*';
                        continue;
					}
                    ch[i, j] = plainText[idx];
                    idx++;
				}
			}
            int groups = 1;
            List<int> Keys = new List<int>(col);
			for (int i = 0; i < col; i++)
			{
                Keys.Add(0);
			}
			for (int i = 0; i < cipherText.Length; i++)
			{
				for (int j = 0; j < col; j++)
				{
                    if (ch[0, j] != cipherText[i]) continue;
                    Boolean ok = true;
                    idx = i;
                    for (int k = 0; k < row; k++)
                    {
                        if (ch[k, j] != cipherText[i])
                        {
                            ok = false;
                            break;
                        }
                        i++;
                        if (i == cipherText.Length) break;
                    }
                    if(ok)
					{
                        i--;
                        Keys[j] = groups;
                        groups++;
                        break;
					}
					else
					{
                        i = idx;
					}
				}
			}

            return Keys;

         //   throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            double val = Math.Ceiling(Convert.ToDouble(cipherText.Length) / key.Count);
            char[,] ch = new char[((int)val), key.Count];
            int x = 1,idx=0;
            for (int i = 0; x<= key.Count; i++)
			{
                if (i == key.Count) i = 0;
                if (key[i] != x) continue;
                x++;
				for (int j = 0; j < val; j++)
				{
					if (idx == cipherText.Length)
					{
                        ch[j, i] = '*';
                        continue;
					}
                    ch[j, i] = cipherText[idx];
                    idx++;
                   
				}
            }
            string plainText = "";

            for (int i = 0; i < val; i++)
			{
				for (int j = 0; j < key.Count; j++)
				{
                    if (ch[i, j] == '*') continue;
                    plainText += ch[i, j];
				}
			}
            return plainText;
            // throw new NotImplementedException();
        }

        public string Encrypt(string plainText, List<int> key)
        {
            double val = Math.Ceiling(Convert.ToDouble( plainText.Length )/ key.Count);
            char[,] ch = new char[((int)val), key.Count];
            int idx = 0;
            for (int i = 0; i < val; i++)
			{
				for (int j = 0; j < key.Count; j++)
                {
                    if (idx == plainText.Length)
                    {
                        ch[i, j] = '*';
                        continue;
                    }
					ch[i, j] = plainText[idx];
                    idx++;
                  
				}
            }
            string cipherText="";
            int x = 1;
			for (int i = 0; x<=key.Count; i++)
			{   if (i == key.Count) i = 0;
                if (key[i] != x) continue;
                x++;
				for (int j = 0; j < val; j++)
				{
                    if (ch[j, i] == '*') continue;

                    cipherText += ch[j, i];
				}
			}
            return cipherText;
           // throw new NotImplementedException();
        }
    }
}
