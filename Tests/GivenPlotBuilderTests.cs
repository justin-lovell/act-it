using NUnit.Framework;

namespace ActIt
{
    [TestFixture]
    public class GivenPlotBuilderTests
    {
        public class TestClassA
        {
        }

        [Test]
        public void WhenCreatedNewInstanceItShouldBeAbleToGenerateStoryInstance()
        {
            var builder = new PlotBuilder();

            var story = builder.GenerateStory();

            Assert.That(story, Is.Not.Null);
        }

        [Test]
        public void WhenEnrolledListener()
        {
            var builder = new PlotBuilder();

            builder.Listen<TestClassA>((a, actor) => { });
        }
    }
}