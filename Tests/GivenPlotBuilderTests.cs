using NUnit.Framework;

namespace TellIt
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
            var builder = new PlotTapBuilder();

            var story = builder.GenerateStory();

            Assert.That(story, Is.Not.Null);
        }

        [Test]
        public void WhenEnrolledListener()
        {
            var builder = new PlotTapBuilder();

            builder.Listen<TestClassA>((a, actor) => { });
        }
    }
}