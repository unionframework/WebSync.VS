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
        [UnionInit(".updated")]
        public UnionElement Title;

        [UnionInit(".question-body")]
        public UnionElement Body;
    }
}
