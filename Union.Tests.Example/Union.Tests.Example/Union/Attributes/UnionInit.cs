using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union.Attributes
{
    internal class UnionInit: Attribute
    {
        public string Xcss;
        public UnionInit(string xcss)
        {
            Xcss = xcss;
        }
    }
}
