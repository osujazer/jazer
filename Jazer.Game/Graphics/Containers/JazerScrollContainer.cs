// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in https://github.com/ppy/osu/blob/master/LICENCE for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace Jazer.Game.Graphics.Containers;

public partial class JazerScrollContainer<T> : BasicScrollContainer<T>
    where T : Drawable
{
    public JazerScrollContainer(Direction scrollDirection = Direction.Vertical)
        : base(scrollDirection)
    {
    }

    /// <summary>
    /// Scrolls a <see cref="Drawable"/> into view.
    /// </summary>
    /// <param name="d">The <see cref="Drawable"/> to scroll into view.</param>
    /// <param name="animated">Whether to animate the movement.</param>
    /// <param name="extraScroll">An added amount to scroll beyond the requirement to bring the target into view.</param>
    public void ScrollIntoView(Drawable d, bool animated = true, float extraScroll = 0)
    {
        double childPos0 = GetChildPosInContent(d);
        double childPos1 = GetChildPosInContent(d, d.DrawSize);

        double minPos = Math.Min(childPos0, childPos1);
        double maxPos = Math.Max(childPos0, childPos1);

        if (minPos < Current || (minPos > Current && d.DrawSize[ScrollDim] > DisplayableContent))
            ScrollTo(minPos - extraScroll, animated);
        else if (maxPos > Current + DisplayableContent)
            ScrollTo(maxPos - DisplayableContent + extraScroll, animated);
    }

    protected override bool OnScroll(ScrollEvent e)
    {
        // allow for controlling volume when alt is held.
        // mostly for compatibility with osu-stable.
        if (e.AltPressed) return false;

        return base.OnScroll(e);
    }

    #region Absolute scrolling

    /// <summary>
    /// Controls the rate with which the target position is approached when performing a relative drag. Default is 0.02.
    /// </summary>
    public double DistanceDecayOnAbsoluteScroll = 0.02;

    protected virtual void ScrollToAbsolutePosition(Vector2 screenSpacePosition)
    {
        float fromScrollbarPosition = FromScrollbarPosition(ToLocalSpace(screenSpacePosition)[ScrollDim]);
        float scrollbarCentreOffset = FromScrollbarPosition(Scrollbar.DrawHeight) * 0.5f;

        ScrollTo(Clamp(fromScrollbarPosition - scrollbarCentreOffset), true, DistanceDecayOnAbsoluteScroll);
    }

    #endregion
}

public partial class JazerScrollContainer : JazerScrollContainer<Drawable>
{
    public JazerScrollContainer(Direction scrollDirection = Direction.Vertical)
        : base(scrollDirection)
    {
    }
}
