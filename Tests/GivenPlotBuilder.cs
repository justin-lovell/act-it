using NUnit.Framework;

namespace TellIt
{
    [TestFixture]
    public class GivenPlotBuilder
    {
        [Test]
        public void WhenCreatedNewInstanceItShouldBeAbleToGenerateStoryInstance()
        {
            var builder = new PlotBuilder();

            var story = builder.GenerateStory();

            Assert.That(story, Is.Not.Null);
        }
    }
}