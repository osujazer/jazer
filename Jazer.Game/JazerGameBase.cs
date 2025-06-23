using Jazer.Game.Configuration;
using Jazer.Game.Online.API;
using Jazer.Resources;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osuTK;

namespace Jazer.Game
{
    public partial class JazerGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        protected override Container<Drawable> Content { get; }

        protected Storage Storage { get; set; }

        protected JazerConfigManager LocalConfig { get; set; }

        protected IAPIAccess API { get; private set; }

        private DependencyContainer dependencies;

        protected JazerGameBase()
        {
            // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(Content = new DrawSizePreservingFillContainer
            {
                // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                TargetDrawSize = new Vector2(1366, 768)
            });
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Storage ??= host.Storage;

            LocalConfig ??= new JazerConfigManager(Storage);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(typeof(JazerResources).Assembly));

            AddFont(Resources, "Fonts/NunitoSans/NunitoSans-Regular");
            AddFont(Resources, "Fonts/NunitoSans/NunitoSans-Medium");
            AddFont(Resources, "Fonts/NunitoSans/NunitoSans-SemiBold");
            AddFont(Resources, "Fonts/NunitoSans/NunitoSans-Bold");

            dependencies.CacheAs(LocalConfig);

            dependencies.CacheAs(API ??= new APIAccess(LocalConfig));

            if (API is APIAccess apiAccess)
                base.Content.Add(apiAccess);
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
    }
}
