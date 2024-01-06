using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Union.Attributes;

namespace Union.Tests.Example.Services.StackOverflow
{
    internal class QuestionPage:UnionPage
    {
        [UnionInit(".title")]
        public UnionElement Title;

        [UnionInit(".title")]
        public UnionElement Body;
    }
}
