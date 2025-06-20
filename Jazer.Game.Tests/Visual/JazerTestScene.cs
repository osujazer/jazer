using osu.Framework.Testing;

namespace Jazer.Game.Tests.Visual
{
    public abstract partial class JazerTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new JazerTestSceneTestRunner();

        private partial class JazerTestSceneTestRunner : JazerGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
