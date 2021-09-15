using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rechner
{
    class Term
    {
        public List<string> o = new List<string>();
        public List<string> z = new List<string>();
        public int pos = int.MaxValue;
        public bool active = true;

        public Term(List<string> o, List<string> z)
        {
            this.o = o;
            this.z = z;
        }
        public Term()
        {
            
        }
    }
}
