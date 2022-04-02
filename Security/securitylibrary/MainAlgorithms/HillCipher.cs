using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plainText = new List<int>();
            if (key.Count == 4)
            {
                List<int> inv = new List<int>();
                inv.Add(key[3]);
                inv.Add(-key[1]);
                inv.Add(-key[2]);
                inv.Add(key[0]);
                for (int i = 0; i < plainText.Count; i += 2)
                {
                    int val = (inv[0] * cipherText[i] + inv[1] * cipherText[i + 1]) % 26;
                    plainText.Add(val);
                    val = (inv[2] * cipherText[i] + inv[3] * cipherText[i + 1]) % 26;
                    plainText.Add(val);
                }
            }
            else
            {
                int det = key[0] * key[3] - key[1] * key[2];
                det %= 26;
                int x = 26 - det;
                int b;
                for (int c = 1; c < 100000000; c++)
                {
                    if ((x * c) % 26 == 1)
                    {
                        b = 26 - c;
                        break;
                    }
                }
              
            }
            return plainText;
            //throw new NotImplementedException();
        }


		public List<int> Encrypt(List<int> plainText, List<int> key)
		{
			List<int> cipherText = new List<int>();
            if(key.Count==4)
			{
				for (int i = 0; i < plainText.Count; i+=2)
				{
                    int val = (key[0] * plainText[i] + key[1] * plainText[i + 1]) % 26;
                    cipherText.Add(val);
                    val = (key[2] * plainText[i] + key[3] * plainText[i + 1]) % 26;
                    cipherText.Add(val);
                }
			}
            else
			{
                for (int i = 0; i < plainText.Count; i += 3)
                {
                    int val = (key[0] * plainText[i] + key[1] * plainText[i + 1] + key[2] * plainText[i + 2]) % 26;
                    cipherText.Add(val);
                    val = (key[3] * plainText[i] + key[4] * plainText[i + 1] + key[5] * plainText[i + 2]) % 26;
                    cipherText.Add(val);
                    val = (key[6] * plainText[i] + key[7] * plainText[i + 1] + key[8] * plainText[i + 2]) % 26;
                    cipherText.Add(val);
                }
            }
            return cipherText;
			//throw new NotImplementedException();
		}


		public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

    }
}
