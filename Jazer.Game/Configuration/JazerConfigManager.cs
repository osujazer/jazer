using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Jazer.Game.Configuration;

public class JazerConfigManager(Storage storage)
    : IniConfigManager<JazerSetting>(storage)
{
    protected override void InitialiseDefaults()
    {
        SetDefault<string?>(JazerSetting.AuthToken, null);
        SetDefault(JazerSetting.Username, string.Empty);
        SetDefault(JazerSetting.SavePassword, false);
        SetDefault(JazerSetting.SaveUsername, true);
    }
}

public enum JazerSetting
{
    AuthToken,
    Username,
    SavePassword,
    SaveUsername
}
