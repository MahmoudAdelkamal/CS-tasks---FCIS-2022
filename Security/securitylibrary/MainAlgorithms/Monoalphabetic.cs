using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            char[] keyArr = new char[26];
            for (int i = 0; i < keyArr.Length; i++)
            {
                keyArr[i] = '@';
            }

            int[] MatchedCharsIdx = new int[26];

            for (int i = 0; i < plainText.Length; i++)
            {
                keyArr[plainText[i] - 'a'] = cipherText[i];
                MatchedCharsIdx[cipherText[i] - 'a'] = 1;
            }

            for (int i = 0; i < 26; i++)
            {
                if (keyArr[i] == '@')
                {
                    for (int j = 0; j < 26; j++)
                    {
                        if (MatchedCharsIdx[j] != 1)
                        {
                            keyArr[i] = (char)(j + 'a');
                            MatchedCharsIdx[j] = 1;
                            break;
                        }
                    }
                }
            }
            string Key = new string(keyArr);
            return Key;
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            string DecryptText = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                int curCharIdx = key.IndexOf(cipherText[i]);
                DecryptText += (char)(curCharIdx + 'a');
            }

            return DecryptText;
        }

        public string Encrypt(string plainText, string key)
        {
            string EncryptedText = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                int curCharIdx = plainText[i] - 'a';
                EncryptedText += key[curCharIdx];
            }
            return EncryptedText;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            string alphabetFreq = "etaoinsrhldcumfpgwybvkxjqz";
            cipher = cipher.ToLower();
            string PlainText = "";
            Dictionary<char, int> alphaFreq = new Dictionary<char, int>();
            SortedDictionary<char, char> keyTable = new SortedDictionary<char, char>();

            for (int i = 0; i < cipher.Length; i++)
            {
                if (!alphaFreq.ContainsKey(cipher[i]))
                {
                    alphaFreq.Add(cipher[i], 0);
                }
                else
                {
                    alphaFreq[cipher[i]]++;
                }
            }
            // sort by freq (greater)
            alphaFreq = alphaFreq.OrderBy(x => x.Value).Reverse().ToDictionary(x => x.Key, x => x.Value);
            int curIdx = 0;
            foreach (var charcter in alphaFreq)
            {
                keyTable.Add(charcter.Key, alphabetFreq[curIdx]);
                curIdx++;
            }

            for (int i = 0; i < cipher.Length; i++)
            {
                PlainText += keyTable[cipher[i]];
            }

            return PlainText;

        }
    }
}