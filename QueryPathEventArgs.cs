#region License
/*
VPKSoft.VisualComponents

Windows.Forms component collection to be used with a HTPC software.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.VisualComponents.

VPKSoft.VisualComponents is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.VisualComponents is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with VPKSoft.VisualComponents.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the VPKSoft.VideoBrowser control.
/// </summary>
namespace VPKSoft.VideoBrowser
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// Event arguments passed to the VPKSoft.VideoBrowser.OnQueryPath event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class QueryPathEventArgs: EventArgs
    {
        /// <summary>
        /// A voluntary context string for the event.
        /// </summary>
        public string Context { get; set; } = string.Empty;

        /// <summary>
        /// This collection should contain the TMDbDetailExt class instances queried by the event.
        /// </summary>
        public IEnumerable<TMDbDetailExt> TMDbDetails { get; set; } = new List<TMDbDetailExt>();

        /// <summary>
        /// Gets or sets a value indicating whether the event was canceled.
        /// </summary>
        public bool Cancel { get; set; } = false;
    }

    /// <summary>
    /// Event arguments passed to the VPKSoft.VideoBrowser.DeleteRequested event or to the VPKSoft.VideoBrowser.PlaybackRequested event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TBDbDetailExtArgs: EventArgs
    {
        /// <summary>
        /// An instance of the TMDbDetailExt class for the event.
        /// </summary>
        public TMDbDetailExt TMDbDetailExt { get; set; } = new TMDbDetailExt();

        /// <summary>
        /// Gets or sets a value indicating whether the event was canceled.
        /// </summary>
        public bool Cancel { get; set; } = false;
    }
}
