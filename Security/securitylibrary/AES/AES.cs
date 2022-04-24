using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        byte[,] sBox = new byte[16, 16] {   {0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76},
                                            {0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0},
                                            {0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15},
                                            {0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75},
                                            {0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84},
                                            {0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF},
                                            {0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8},
                                            {0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2},
                                            {0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73},
                                            {0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB},
                                            {0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79},
                                            {0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08},
                                            {0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A},
                                            {0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E},
                                            {0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF},
                                            {0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16} };

        static byte[,] sBoxInverse = new byte[16, 16] { { 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb },
                                                        { 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb },
                                                        { 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e },
                                                        { 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25 },
                                                        { 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92 },
                                                        { 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84 },
                                                        { 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06 },
                                                        { 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b },
                                                        { 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73 },
                                                        { 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e },
                                                        { 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b },
                                                        { 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4 },
                                                        { 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f },
                                                        { 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef },
                                                        { 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61 },
                                                        { 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d } };

        int RconIndex = 0;
        byte[,] Rcon = new byte[4, 10] { {0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36 },
                                         {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                                         {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                                         {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }};

        byte[,] galoisField = new byte[4, 4] {  {0x02, 0x03, 0x01, 0x01},
                                                {0x01, 0x02, 0x03, 0x01},
                                                {0x01, 0x01, 0x02, 0x03},
                                                {0x03, 0x01, 0x01, 0x02}};

        byte[,] galoisFieldInverse = new byte[4, 4] {   {0x0e, 0x0b, 0x0d, 0x09},
                                                        {0x09, 0x0e, 0x0b, 0x0d},
                                                        {0x0d, 0x09, 0x0e, 0x0b},
                                                        {0x0b, 0x0d, 0x09, 0x0e}};
        byte[,] keyExpansion = new byte[44, 4]; // ((n+1)*4,4)
        byte[] rotateWord(byte[] word)
        {
            byte tmp = word[0];
            for (int i = 0; i < 3; i++)
                word[i] = word[i + 1];
            word[3] = tmp;
            return word;
        }
        byte[] subByte(byte[] word)
        {
            byte[] res = new byte[4];
            // new i , new j 
            int ii,jj;
            for (int i = 0; i < 4; i++)
            {
                string tmp = Convert.ToString(word[i], 16);
                if (tmp.Length == 1)
                {
                    ii = 0;
                    jj = Convert.ToInt32(tmp[0].ToString(), 16);
                }
                else
                {
                    ii = Convert.ToInt32(tmp[0].ToString(), 16);
                    jj = Convert.ToInt32(tmp[1].ToString(), 16);
                }
                res[i] = sBox[ii, jj];
            }
            return res;
        }
        byte[] xor(byte[] first, byte[] second, byte[] third, int isMultipleOf_4)
        {
            byte[] res = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                string tmp;
                if (isMultipleOf_4 == 0)
                    tmp = Convert.ToString(first[i] ^ second[i], 16);
                else
                    tmp = Convert.ToString(first[i] ^ second[i] ^ third[i], 16);

                res[i] = Convert.ToByte(tmp, 16);

            }


            return res;
        }

        string convertMatrixToString(byte[,] matrix)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var temp = Convert.ToString(matrix[j, i], 16);
                    if (temp.Length < 2)
                    {
                        str.Append("0" + temp);
                    }
                    else str.Append(temp);
                }
            }
            return str.ToString().ToUpper().Insert(0, "0x");
        }

        byte[,] convertToByteMatrixPlainTextVersion(string str)
        {
            byte[,] matrix = new byte[4, 4];
            // start idx is 2 cuz every string start with 0x at begining 
            int idx = 2;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    // convert the current two chars to hexadecimal
                    string tmp = "0x" + str[idx] + str[idx + 1];
                    // fill the byte matrix column wise 
                    matrix[i, j] = Convert.ToByte(tmp, 16);
                    idx += 2;
                }
            }
            return matrix;
        }

        byte[,] convertToByteMatrixKeyVersion(string str)
        {
            byte[,] matrix = new byte[4, 4];

            int idx = 2;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    string tmp = "0x" + str[idx] + str[idx + 1];
                    // fill the byte matrix row wise (normal fill)
                    matrix[j, i] = Convert.ToByte(tmp, 16);
                    idx += 2;
                }
            }
            return matrix;
        }

        void setKey(string key)
        {
            byte[,] keyMatrix = new byte[4, 4];
            keyMatrix = convertToByteMatrixKeyVersion(key);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    keyExpansion[i, j] = keyMatrix[i, j];

        }
        byte[,] getKeyMatrix(int index)
        {
            byte[,] keyMat = new byte[4, 4];
            int row = 0, col = 0;
            for (int i = index * 4; i < index * 4 + 4; i++)
            {
                col = 0;
                for (int j = 0; j < 4; j++)
                {
                    // filling key matix column wise 
                    keyMat[col, row] = keyExpansion[i, j];
                    col++;
                }
                row++;
            }
            return keyMat;
        }
        byte[,] RoundKey(byte[,] matrix, int roundIndex)
        {
            byte[,] RoundKeyMatrix;
            RoundKeyMatrix = getKeyMatrix(roundIndex);


            string tmp;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tmp = Convert.ToString(RoundKeyMatrix[i, j] ^ matrix[i, j], 16);
                    RoundKeyMatrix[i, j] = Convert.ToByte(tmp, 16);
                }
            }
            return RoundKeyMatrix;
        }
        void implementKeyExpansion()
        {
            byte[] first = new byte[4];
            byte[] second = new byte[4];
            byte[] third = new byte[4];
            byte[] final = new byte[4];
            for (int i = 4; i < 44; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    first[j] = keyExpansion[i - 1, j];
                    second[j] = keyExpansion[i - 4, j];
                    if (RconIndex < 10)
                        third[j] = Rcon[j, RconIndex];
                }
                if (i % 4 == 0)
                {
                    // Console.WriteLine(i);
                    RconIndex++;
                    first = rotateWord(first);
                    first = subByte(first);
                    final = xor(first, second, third, 1);
                }
                else
                    final = xor(first, second, third, 0);

                for (int j = 0; j < 4; j++)
                {
                    // Console.Write(keyExpansion[i,j]);
                    keyExpansion[i, j] = final[j];
                }

            }
        }
       
        byte[,] mixCols(byte[,] matrix)
        {
            byte[] XOR_array = new byte[4];
            byte[,] mixedColsMatrix = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (galoisField[j, k] == 2)
                        {
                            XOR_array[k] = multiplyByTwo(matrix[k, i]);
                        }
                        if (galoisField[j, k] == 3)
                        {
                            XOR_array[k] = Convert.ToByte(multiplyByTwo(matrix[k, i]) ^ matrix[k, i]);
                        }

                        if (galoisField[j, k] == 1)
                        {
                            XOR_array[k] = matrix[k, i];
                        }
                    }
                    byte cell = Convert.ToByte(XOR_array[0] ^ XOR_array[1] ^ XOR_array[2] ^ XOR_array[3]);
                    mixedColsMatrix[j, i] = cell;
                }
            }
            return mixedColsMatrix;
        }

        byte multiplyByTwo(byte x)
        {
            byte res; 
            // mul by 2 
            UInt32 temp = Convert.ToUInt32(x << 1);
            // anding with 0xFF returns the last 8 bytes when performaing shift on integer 
            res = (byte)(temp & 0xFF);
            // byte overflow
            if (x > 127)
                res = Convert.ToByte(res ^ 27); // 0x1b == 27 
            return res;
        }

        byte[,] mixColsInverse(byte[,] matrix)
        {
            byte[] XOR_array = new byte[4];
            byte[,] mixedColsMatrix = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (galoisFieldInverse[j, k] == 0x9)
                        {
                            byte x0 = matrix[k, i];
                            byte x1 = multiplyByTwo(x0);
                            byte x2 = multiplyByTwo(x1);
                            byte x3 = multiplyByTwo(x2);
                            XOR_array[k] = Convert.ToByte(x3 ^ x0);
                        }
                        if (galoisFieldInverse[j, k] == 0xB)
                        {
                            byte x0 = matrix[k, i];
                            byte x1 = multiplyByTwo(x0);
                            byte x2 = multiplyByTwo(x1);
                            byte x3 = multiplyByTwo(x2);
                            XOR_array[k] = Convert.ToByte(x3 ^ x0 ^ x1);
                        }
                        if (galoisFieldInverse[j, k] == 0xD)
                        {
                            byte x0 = matrix[k, i];
                            byte x1 = multiplyByTwo(x0);
                            byte x2 = multiplyByTwo(x1);
                            byte x3 = multiplyByTwo(x2);
                            XOR_array[k] = Convert.ToByte(x3 ^ x2 ^ x0);
                        }

                        if (galoisFieldInverse[j, k] == 0xE)
                        {
                            byte x0 = matrix[k, i];
                            byte x1 = multiplyByTwo(x0);
                            byte x2 = multiplyByTwo(x1);
                            byte x3 = multiplyByTwo(x2);
                            XOR_array[k] = Convert.ToByte(x3 ^ x2 ^ x1);
                        }
                    }
                    byte cell = Convert.ToByte(XOR_array[0] ^ XOR_array[1] ^ XOR_array[2] ^ XOR_array[3]);
                    mixedColsMatrix[j, i] = cell;
                }
            }
            return mixedColsMatrix;
        }

        byte[,] firstRound(byte[,] state)
        {
            string tmp;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // Xoring state with K0
                    tmp = Convert.ToString(state[j, i] ^ keyExpansion[i, j], 16);

                    state[j, i] = Convert.ToByte(tmp, 16);
                }
            }
            return state;
        }
        byte[,] subStateMatrixWithS_BoxMatrix(byte[,] matrix)
        {
            byte[,] newMatrix = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string tmp = Convert.ToString(matrix[i, j], 16);
                    // new i , new j
                    int ii, jj;
                    if (tmp.Length == 1)
                    {
                        ii = 0;
                        jj = Convert.ToInt32(tmp[0].ToString(), 16);
                    }
                    else
                    {
                        ii = Convert.ToInt32(tmp[0].ToString(), 16);
                        jj = Convert.ToInt32(tmp[1].ToString(), 16);
                    }


                    newMatrix[i, j] = sBox[ii, jj];
                }
            }
            return newMatrix;
        }
        byte[,] subStateMatrixWithS_BoxMatrixInverseVersion(byte[,] matrix)
        {
            byte[,] newMatrix = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string tmp = Convert.ToString(matrix[i, j], 16);
                    // new i , new j 
                    int ii, jj;
                    if (tmp.Length == 1)
                    {
                        ii = 0;
                        jj = Convert.ToInt32(tmp[0].ToString(), 16);
                    }
                    else
                    {
                        ii = Convert.ToInt32(tmp[0].ToString(), 16);
                        jj = Convert.ToInt32(tmp[1].ToString(), 16);
                    }


                    newMatrix[i, j] = sBoxInverse[ii, jj];
                }
            }
            return newMatrix;
        }

        byte[,] shiftMatrix(byte[,] matrix)
        {
            
            byte[,] newMatrix = matrix;
            for (int i = 1; i < 4; i++)
            {
                int moves = i;
                while (moves > 0)
                {
                    // holding first element to be shifted to back
                    byte tmp = newMatrix[i, 0];
                    for (int j = 0; j < 3; j++)
                    {
                        newMatrix[i, j] = newMatrix[i, j + 1];
                    }
                    // puting the shifted element in it's right place
                    newMatrix[i, 3] = tmp;
                    moves--;
                }
            }
            return newMatrix;
        }
       
        byte[,] shiftMatrixInverse(byte[,] matrix)
        {
            byte[,] newMatrix = matrix;
            for (int i = 1; i < 4; i++)
            {
                int moves = i;
                while (moves > 0)
                {
                    byte tmp = newMatrix[i, 3];
                    for (int j = 3; j > 0; j--)
                    {
                        newMatrix[i, j] = newMatrix[i, j - 1];
                    }
                    newMatrix[i, 0] = tmp;
                    moves--;
                }
            }
            return newMatrix;
        }

        
        byte[,] lastRound(byte[,] state)
        {
            state = subStateMatrixWithS_BoxMatrix(state);
            state = shiftMatrix(state);
            state = RoundKey(state, 10);
            return state;
        }
        byte[,] finalRoundDecrypt(byte[,] state)
        {
            state = RoundKey(state, 10);
            state = shiftMatrixInverse(state);
            state = subStateMatrixWithS_BoxMatrixInverseVersion(state);
            return state;
        }
        byte[,] mainRoundsEncryption(byte[,] state, int round)
        {
            //subtitude state matrix with S-Box matrix
            state = subStateMatrixWithS_BoxMatrix(state);
            state = shiftMatrix(state);
            state = mixCols(state);
            state = RoundKey(state, round);
            return state;
        }
        byte[,] mainRoundsDecryption(byte[,] state, int round)
        {
            state = RoundKey(state, round);
            state = mixColsInverse(state);
            state = shiftMatrixInverse(state);
            state = subStateMatrixWithS_BoxMatrixInverseVersion(state);
            return state;
        }
        byte[,] firstRoundDecryption(byte[,] state)
        {
            state = RoundKey(state, 0);
            return state;
        }
        public override string Decrypt(string cipherText, string key)
        {

            byte[,] state = convertToByteMatrixPlainTextVersion(cipherText);
            setKey(key);
            implementKeyExpansion();
            state = finalRoundDecrypt(state);

            for (int i = 9; i > 0; i--)
            {
                state = mainRoundsDecryption(state, i);
            }

            state = firstRoundDecryption(state);
            return convertMatrixToString(state);
        }

        public override string Encrypt(string plainText, string key)
        {
            byte[,] state = convertToByteMatrixPlainTextVersion(plainText);
            setKey(key);
            implementKeyExpansion();
            // only Xoring with K0
            state = firstRound(state);

            for (int i = 1; i < 10; i++)
            {
                // 1-sub with S-Box
                // 2-shift matrix
                // 3-mix cols
                // 4-round key
                state = mainRoundsEncryption(state, i);
            }
            // no mixed cols
            state = lastRound(state);
            return convertMatrixToString(state);
        }
    }
}
