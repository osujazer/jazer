using System.Collections.Generic;
using osu.Framework.Input.Bindings;

namespace Jazer.Game;

public partial class GlobalActionContainer : KeyBindingContainer<GlobalAction>
{
    public override IEnumerable<IKeyBinding> DefaultKeyBindings =>
    [
        new KeyBinding(new[] { InputKey.Control, InputKey.T }, GlobalAction.ToggleToolbar),
    ];
}

public enum GlobalAction
{
    ToggleToolbar,
}
