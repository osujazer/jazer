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
        protected override Container<Drawable> Content { get; }

        protected Storage Storage { get; set; } = null!;

        protected JazerConfigManager LocalConfig { get; set; } = null!;

        protected IAPIAccess API { get; private set; } = null!;

        private DependencyContainer dependencies = null!;

        protected JazerGameBase()
        {
            base.Content.Add(new DrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1366, 768),
                Child = Content = new GlobalActionContainer
                {
                    RelativeSizeAxes = Axes.Both,
                }
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
