using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union.Attributes
{
    internal class UnionInitAttribute: Attribute
    {
        public object[] Args { get; }
        public string ComponentName { get; set; }
        public string FrameXcss { get; set; }

        public UnionInitAttribute()
        {
        }

        public UnionInitAttribute(params object[] args)
        {
            Args = args;
        }
    }
}
