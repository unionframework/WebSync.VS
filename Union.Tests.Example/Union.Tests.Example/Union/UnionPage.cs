using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Union.Interfaces;

namespace Union
{
    internal abstract class UnionPage:IUnionPage
    {
        public abstract string AbsolutePath { get; }
    }
}
