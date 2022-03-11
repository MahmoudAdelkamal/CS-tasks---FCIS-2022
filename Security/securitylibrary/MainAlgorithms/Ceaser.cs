using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            string EncryptedString = "";
            for(int i=0;i<plainText.Length;i++)
            {
                char StartIndexingCharacter;
                if (char.IsUpper(plainText[i]))
                    StartIndexingCharacter = 'A';
                else
                    StartIndexingCharacter = 'a';
                char EncryptedCharacter = (char)(StartIndexingCharacter+((plainText[i] - StartIndexingCharacter) + key) % 26);
                EncryptedString += EncryptedCharacter;
            }
            return EncryptedString;
        }
        public string Decrypt(string cipherText, int key)
        {
            string DecryptedString = "";
            for(int i=0;i<cipherText.Length;i++)
            {
                char StartIndexingCharacter;
                if (char.IsUpper(cipherText[i]))
                    StartIndexingCharacter = 'A';
                else
                    StartIndexingCharacter = 'a';
                char DecryptedCharacter = (char)(StartIndexingCharacter + ((cipherText[i] - StartIndexingCharacter) - key + 26) % 26);
                DecryptedString += DecryptedCharacter;
            }
            return DecryptedString;
        }
        public int Analyse(string plainText, string cipherText)
        {
            for(int key=0;key<26;key++)
            {
                string EncryptedString = Encrypt(plainText,key);
                if (cipherText.Equals(EncryptedString, StringComparison.OrdinalIgnoreCase))
                    return key;
            }
            return -1;
        }
    }
}
