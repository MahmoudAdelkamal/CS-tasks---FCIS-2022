using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        static string HexStringToString(string hexString)
        {
            var sb = new StringBuilder();
            for (var i = 2; i < hexString.Length; i += 2)
            {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }

        static string StringToHexString(string str)
        {
            byte[] ba = Encoding.Default.GetBytes(str);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return "0x" + hexString;
        }

        public override string Decrypt(string cipherText, string key)
        {
            return Encrypt(cipherText, key);
            //throw new NotImplementedException();
        }

        public override  string Encrypt(string plainText, string key)
        {
            bool hex = false;
            if (plainText.Substring(0,2) == "0x")
            {
                hex = true;
                plainText = HexStringToString(plainText);
            }
            if (key.Substring(0,2) == "0x")
                key = HexStringToString(key);
            // init of S and T
            int[] s = new int[256], t = new int[256];
            for (int i = 0; i < 256; i++)
            {
                s[i] = i;
                
                int j = (i % key.Length);
                t[i] = key[j];
            }

            // initial permutation
            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + s[i] + t[i]) % 256;
                
                // swapping s[i] and s[j]
                s[i] += s[j];
                s[j] = s[i] - s[j];
                s[i] -= s[j];
            }

            string cipherText = "";
            for (int indx = 0, i = 0, j = 0; indx < plainText.Length; indx++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;

                // swapping s[i] and s[j]
                s[i] += s[j];
                s[j] = s[i] - s[j];
                s[i] -= s[j];

                int x = (s[i] + s[j]) % 256;
                cipherText += (char) (plainText[indx] ^ s[x]);
                
            }
            
            if (hex)
            {
                cipherText = StringToHexString(cipherText);
            }

            return cipherText;
        }
    }
}
