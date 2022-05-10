using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
        public long FastPower(long Base, long exponential, long mod)
        {
            if (exponential == 0)
                return 1;
            long ret = FastPower(Base, exponential / 2, mod);
            ret = (ret * ret) % mod;
            if (exponential % 2 != 0)
                ret = (ret * (Base % mod)) % mod;
            return ret;
        }
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            long ya = FastPower(alpha, xa, q);
            long yb = FastPower(alpha, xb, q);
            List<int> Keys = new List<int>{

                (int)FastPower(ya, xb, q),
                (int)FastPower(yb, xa, q)
            };
            return Keys;
        }
    }
}
