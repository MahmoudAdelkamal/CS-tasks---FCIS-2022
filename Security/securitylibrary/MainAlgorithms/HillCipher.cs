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
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        static int modInverse(int a, int m)
        {
            int m0 = m;
            int y = 0, x = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                int q = a / m;

                int t = m;

                m = a % m;
                a = t;
                t = y;

                y = x - q * y;
                x = t;
            }

            if (x < 0)
                x += m0;

            return x;
        }

        static int gcd(int a, int m)
        {
            if (a == 0)
                return m;
            return gcd(m % a, a);
        }

        static int posMod(int val, int mod)
        {
            return ((val % mod) + mod) % mod;
        }
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            for (int i = 0; i + 3 < plainText.Count; i += 2)
            {
                for (int j = i + 2; j < plainText.Count; j += 2)
                {

                    List<int> plainInv = new List<int>(), adj = new List<int>();
                    adj.Add(plainText[j + 1]);
                    adj.Add(-plainText[j]);
                    adj.Add(-plainText[i + 1]);
                    adj.Add(plainText[i]);

                    int det = plainText[i] * plainText[j + 1] - plainText[i + 1] * plainText[j];

                    bool valid = true;
                    
                    for (int x = 0; valid && x < 2; x ++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            int val = cipherText[i + x] * adj[y] + cipherText[j + x] * adj[y + 2];
                            int denominator = det;
                            int g = gcd(val, denominator);
                            val /= g;
                            denominator /= g;
                            if (gcd(denominator, 26) == 1)
                            {
                                val *= modInverse(denominator, 26);
                                plainInv.Add(posMod(val, 26));
                            }
                            else
                            {
                                valid = false;
                                break;
                            }
                        }
                    }
                    if (valid)
                        return plainInv;
                }
            }
            throw new InvalidAnlysisException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plainText = new List<int>();
            if (key.Count == 4)
            {
                List<int> inv = new List<int>();
                int det = modInverse(posMod(key[0] * key[3] - key[1] * key[2], 26), 26);
                // calc det to calc inverse
                inv.Add((det * key[3]) % 26);
                inv.Add(posMod((det * -key[1]), 26));
                inv.Add(posMod((det * -key[2]), 26));
                inv.Add((det * key[0]) % 26);
                for (int i = 0; i < cipherText.Count; i += 2)
                {
                    int val = (inv[0] * cipherText[i] + inv[1] * cipherText[i + 1]) % 26;
                    plainText.Add(val);
                    val = (inv[2] * cipherText[i] + inv[3] * cipherText[i + 1]) % 26;
                    plainText.Add(val);
                }
            }
            else
            {
                int det = 0;
                // calc det
                for (int i = 0; i < 3; i++)
                {
                    int[] indx = { 3 + (i + 1)%3, 3 + (i + 2)%3,    
                                   6 + (i + 1)%3, 6 + (i + 2)%3};   

                    det += key[i] * ((key[indx[0]] * key[indx[3]]) - (key[indx[1]] * key[indx[2]]));
                }
                det = posMod(det, 26);
                // calc inv of det
                int b = modInverse(det, 26);
                List<int> keyInverse = new List<int>();

                for (int col = 0; col < 3; col++)
                {
                    for (int row = 0; row < 3; row++)
                    {
                        int[] indx = { 3*((row+1)%3) + (col+1)%3, 3*((row+1)%3) + (col+2)%3,
                                       3*((row+2)%3) + (col+1)%3, 3*((row+2)%3) + (col+2)%3};
                        keyInverse.Add(posMod(b * ((key[indx[0]] * key[indx[3]]) - (key[indx[1]] * key[indx[2]])), 26));
                    }
                }
                plainText = Encrypt(cipherText, keyInverse);
            }
            return plainText;
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> cipherText = new List<int>();
            if (key.Count == 4)
            {
                for (int i = 0; i < plainText.Count; i += 2)
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
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            for (int i = 0; i + 2 < plainText.Count; i++)
            {
                for (int j = i + 3; j + 2 < plainText.Count; j++)
                {
                    for (int k = j + 3; k + 2 < plainText.Count; k++)
                    {

                        int det = plainText[i] * (plainText[j + 1] * plainText[k + 2] - plainText[k + 1] * plainText[j + 2]);
                        det -= plainText[j] * (plainText[i + 1] * plainText[k + 2] - plainText[k + 1] * plainText[i + 2]);
                        det += plainText[k] * (plainText[i + 1] * plainText[j + 2] - plainText[j + 1] * plainText[i + 2]);
                        int[] col = { i, j, k };
                        List<int> plainInv = new List<int>();
                        if (gcd(det, 26) != 1)
                            continue;
                        for (int x = 0; x < 3; x++)
                        {
                            for (int row = 0; row < 3; row++)
                            {
                                int[] indx = { (row+1)%3 + col[(x+1) % 3], (row+1)%3 + col[(x+2) % 3],
                                       (row+2)%3 + col[(x+1)%3], (row+2)%3 + col[(x+2)%3]};
                                int val =  plainText[indx[0]] * plainText[indx[3]] - plainText[indx[1]] * plainText[indx[2]];
                                val = posMod(posMod(val, 26) * modInverse(det, 26), 26);
                                plainInv.Add(val);
                            }
                        }
                        List<int> d = new List<int>(), rtn = new List<int>();
                        for (int x = 0; x < 3; x++)
                            for (int y = 0; y < 3; y++)
                                d.Add(cipherText[x + col[y]]);
                        for (int x = 0; x < 3; x++)
                        {
                            for (int y = 0; y < 3; y++)
                            {
                                int val = d[3*x] * plainInv[y] 
                                    + d[3*x + 1] * plainInv[y + 3] + d[3*x + 2] * plainInv[y + 6];
                                rtn.Add(posMod(val, 26));
                            }
                        }
                        return rtn;
                    }
                }
            }
            throw new InvalidAnlysisException();
        }

    }
}