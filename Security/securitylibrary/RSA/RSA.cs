using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public long FastPower(long Base, long exponential, long mod)
        {
            if(exponential==0)
                return 1;
            long ret = FastPower(Base,exponential/2,mod);
            ret = (ret * ret) % mod;
            if(exponential%2!=0)
                ret = (ret*(Base%mod))%mod;
            return ret;
        }
        public long MultiplicativeInverse(long number, long Base)
        {
            List<long> L = new List<long>() {
                -1, 0, 0, 0,
            };
            List<long> N = new List<long>() {
                -1, 0, 1, number,
            };
            List<long> B = new List<long>() {
                -1, 1, 0, Base,
            };
            while (true)
            {
                if (N[3] == 0)
                    return -1;
                else if (N[3] == 1)
                    return ((N[2] % Base) + Base) % Base;
                long Q = B[3] / N[3];
                L[1] = B[1] - (Q * N[1]);
                L[2] = B[2] - (Q * N[2]);
                L[3] = B[3] - (Q * N[3]);
                B[1] = N[1]; B[2] = N[2]; B[3] = N[3];
                N[1] = L[1]; N[2] = L[2]; N[3] = L[3];
            }
        }
        public int Encrypt(int p, int q, int M, int e)
        {
            long n = p*q;
            int EncryptedNumber = (int)FastPower(M,e,n);
            return EncryptedNumber;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            long d = MultiplicativeInverse(e, (p - 1) * (q - 1));
            int DecryptedNumber = (int)FastPower(C, d, p*q);
            return DecryptedNumber;
        }
    }
}
