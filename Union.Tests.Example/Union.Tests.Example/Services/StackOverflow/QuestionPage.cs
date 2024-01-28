using Union.Attributes;

namespace Union.Tests.Example.Services.StackOverflow
{
    internal class QuestionPage : UnionPage
    {
        [UnionInit("Tlkkkkk")]
        public UnionElement Title;

        [UnionInit(".question-body")]
        public UnionElement Body;

        [UnionInitAttribute(".home-page")]
        public Union.UnionElement Element1;

        public override string AbsolutePath => "absoluteUrl";
    }
}