using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        
        public string modifyPlainText(string plainText)
        {
            for (int i = 0; i < plainText.Length - 1; i += 2)
            {
                if (plainText[i] == plainText[i + 1])
                    plainText = plainText.Substring(0, i + 1) + 'x' + plainText.Substring(i + 1);
            }
            if (plainText.Length % 2 != 0)
                plainText += 'x';
            return plainText;
        }
        public void fillMatrix(IList<char> key, Dictionary<char, string> charToPos, Dictionary<string, char> posToChar)
        {
            char[,] matrix = new char[5, 5];
            int keyIdx = 0, charIdx = 0;
            List<char> alphabet = new List<char>();
            for (char i = 'a'; i <= 'z'; i++)
            {
                if (i == 'j') continue;
                alphabet.Add(i);
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    // fill bel key l7ad ma y5ls ba3d kda fill bel alphabet
                    if (charIdx < key.Count)
                    {
                        matrix[i, j] = key[charIdx];
                        alphabet.Remove(key[charIdx]);
                        charIdx++;
                    }

                    else
                    {
                        matrix[i, j] = alphabet[keyIdx];
                        keyIdx++;
                    }

                    string charPos = i.ToString() + j.ToString();

                    charToPos.Add(matrix[i, j], charPos);
                    posToChar.Add(charPos, matrix[i, j]);
                }
            }
        }

        public string Decrypt(string cipherText, string key)
        {

            string decryptedString = "";
            cipherText = cipherText.ToLower();
           
            Dictionary<char, string> charToPos = new Dictionary<char, string>();
            Dictionary<string, char> posToChar = new Dictionary<string, char>();
            fillMatrix(key.Distinct().ToArray(), charToPos, posToChar);
         
            for (int i = 0; i < cipherText.Length; i += 2)
            {
               
                string curTwoChars = cipherText.Substring(i, 2);
                string firstCharPos = charToPos[curTwoChars[0]];
                string secondCharPos = charToPos[curTwoChars[1]];

                // same Row: inc colums
                if (firstCharPos[0] == secondCharPos[0])
                {
                    int newFirstCharcolumn = (int.Parse(firstCharPos[1].ToString()) - 1) % 5;
                    int newSecondCharcoluumn = (int.Parse(secondCharPos[1].ToString()) - 1) % 5;
                    newFirstCharcolumn = (newFirstCharcolumn + 5) % 5;
                    newSecondCharcoluumn = (newSecondCharcoluumn + 5) % 5;
                    decryptedString += posToChar[firstCharPos[0].ToString() + newFirstCharcolumn.ToString()];
                    decryptedString += posToChar[secondCharPos[0].ToString() + newSecondCharcoluumn.ToString()];
                }

                // same Column : inc Rows
                else if (firstCharPos[1] == secondCharPos[1])
                {
                    int newFirstCharRow = (int.Parse(firstCharPos[0].ToString()) - 1) % 5;
                    int newSecondCharRow = (int.Parse(secondCharPos[0].ToString()) - 1) % 5;
                    newFirstCharRow = (newFirstCharRow + 5) % 5;
                    newSecondCharRow = (newSecondCharRow + 5) % 5;
                    decryptedString += posToChar[newFirstCharRow.ToString() + firstCharPos[1].ToString()];
                    decryptedString += posToChar[newSecondCharRow.ToString() + secondCharPos[1].ToString()];

                }
                // different row , column
                else
                {
                    decryptedString += posToChar[firstCharPos[0].ToString() + secondCharPos[1].ToString()];
                    decryptedString += posToChar[secondCharPos[0].ToString() + firstCharPos[1].ToString()];

                }
              

            }
            // removing x from decrypted string
            string decryptedStringWithoutXs = decryptedString;
            if (decryptedString[decryptedString.Length - 1] == 'x')
            {
                decryptedStringWithoutXs = decryptedStringWithoutXs.Remove(decryptedString.Length - 1);
            }

            int prev = 0;
            for (int i = 0; i < decryptedStringWithoutXs.Length; i++)
            {
                if (decryptedString[i] == 'x' && (i%2)==1)
                {
       
                    if (decryptedString[i - 1] == decryptedString[i + 1])
                    {
                        decryptedStringWithoutXs = decryptedStringWithoutXs.Remove(i+ prev, 1);
                        prev--;
                    }

                }
            }

            return decryptedStringWithoutXs;
        }

        public string Encrypt(string plainText, string key)
        {
            string encryptedString = "";
            Dictionary<char, string> charToPos = new Dictionary<char, string>();
            Dictionary<string, char> posToChar = new Dictionary<string, char>();
            fillMatrix(key.Distinct().ToArray(), charToPos, posToChar);
            plainText = modifyPlainText(plainText);
            for (int i = 0; i < plainText.Length; i += 2)
            {
                //dlfdsdndihbddtntuebluoimcvbserulyo
                string curTwoChars = plainText.Substring(i, 2);
                string firstCharPos = charToPos[curTwoChars[0]];
                string secondCharPos = charToPos[curTwoChars[1]];

                // same Row: inc colums
                if (firstCharPos[0] == secondCharPos[0])
                {
                    int newFirstCharcolumn = (int.Parse(firstCharPos[1].ToString()) + 1) % 5;
                    int newSecondCharcoluumn = (int.Parse(secondCharPos[1].ToString()) + 1) % 5;
                    encryptedString += posToChar[firstCharPos[0].ToString() + newFirstCharcolumn.ToString()];
                    encryptedString += posToChar[secondCharPos[0].ToString() + newSecondCharcoluumn.ToString()];
                }

                // same Column : inc Rows
                else if (firstCharPos[1] == secondCharPos[1])
                {
                    int newFirstCharRow = (int.Parse(firstCharPos[0].ToString()) + 1) % 5;
                    int newSecondCharRow = (int.Parse(secondCharPos[0].ToString()) + 1) % 5;
                    encryptedString += posToChar[newFirstCharRow.ToString() + firstCharPos[1].ToString()];
                    encryptedString += posToChar[newSecondCharRow.ToString() + secondCharPos[1].ToString()];

                }
                // different row , column
                else
                {
                    encryptedString += posToChar[firstCharPos[0].ToString() + secondCharPos[1].ToString()];
                    encryptedString += posToChar[secondCharPos[0].ToString() + firstCharPos[1].ToString()];

                }

            }

            return encryptedString;
        }
    }
}
