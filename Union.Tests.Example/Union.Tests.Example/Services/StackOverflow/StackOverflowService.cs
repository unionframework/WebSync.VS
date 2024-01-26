using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Union.Tests.Example.Services.StackOverflow
{
    internal class StackOverflowService : UnionService<StackOverflowPage>
    {
        public override string BaseUrl => "http://www.stackoverflow.com";
    }
}
