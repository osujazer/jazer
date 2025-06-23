// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in https://github.com/ppy/osu/blob/master/LICENCE for full licence text.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jazer.Game.Graphics.Carousel;

/// <summary>
/// An interface representing a filter operation which can be run on a <see cref="Carousel{T}"/>.
/// </summary>
public interface ICarouselFilter
{
    /// <summary>
    /// Execute the filter operation.
    /// </summary>
    /// <param name="items">The items to be filtered.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The post-filtered items.</returns>
    Task<List<CarouselItem>> Run(IEnumerable<CarouselItem> items, CancellationToken cancellationToken);
}
