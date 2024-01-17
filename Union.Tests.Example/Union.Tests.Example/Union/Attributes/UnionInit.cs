using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union.Attributes
{
    internal class UnionInitAttribute: Attribute
    {
        public string Xcss;
        public UnionInitAttribute(string xcss)
        {
            Xcss = xcss;
        }
    }
}
