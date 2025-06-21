using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Jazer.Game.Configuration;

public class JazerConfigManager(
    Storage storage,
    IDictionary<JazerSetting, object> defaultOverrides = null)
    : IniConfigManager<JazerSetting>(storage, defaultOverrides);

public enum JazerSetting
{
    AuthToken,
    Username,
    SavePassword,
    SaveUsername
}
