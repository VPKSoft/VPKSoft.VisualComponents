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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using VPKSoft.TMDbFileUtils; // (C): https://www.vpksoft.net/2015-03-31-13-33-28/libraries/vpksoft-tmdbfileutils, GNU General Public License Version 3

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the VPKSoft.VideoBrowser control.
/// </summary>
namespace VPKSoft.VideoBrowser
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// An extension class for the TMDbDetail class.
    /// </summary>
    /// <seealso cref="VPKSoft.TMDbFileUtils.TMDbDetail" />
    /// <seealso cref="System.IDisposable" />
    public class TMDbDetailExt : TMDbDetail, IDisposable
    {
        /// <summary>
        /// Gets or sets the duration of a video file this class represents.
        /// </summary>
        public TimeSpan Duration { get; set; } = new TimeSpan();

        /// <summary>
        /// A still/poster image describing the video.
        /// </summary>
        private Image _Image = null;

        /// <summary>
        /// Gets or sets the state of the playback with the video before.
        /// </summary>
        public VideoPlaybackState VideoPlaybackState { get; set; } = VideoPlaybackState.New;

        /// <summary>
        /// Gets or sets a still/poster image describing the video.
        /// </summary>
        public Image Image
        {
            get
            {
                // load the image from the file system cache directory if "conditions" are met..
                if (ImageDumbed && ImageFileCache)
                {
                    // construct a full file name for the image..
                    string imageFileName = Path.Combine(ImageFileCacheDir, ImageFileName);
                    if (File.Exists(imageFileName)) // load the image if the file exists..
                    {
                        return Image.FromFile(imageFileName); // load the image..
                    }

                    LoadDumpedImage();                    
                }
                return _Image ?? VideoBrowser.ImageNoVideoImage;
            }

            set
            {
                _Image = value; // set the image..
            }
        }

        /// <summary>
        /// Gets or sets the file size of the video file.
        /// </summary>
        public long FileSize { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether the image is to be cached to the file system.
        /// </summary>
        public bool ImageFileCache { get; set; } = false;

        /// <summary>
        /// Gets or sets the image file cache path if the <see cref="ImageFileCache"/> is enabled.
        /// </summary>
        public string ImageFileCacheDir { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the image is dumped in the file system cache.
        /// </summary>
        private bool ImageDumbed { get; set; } = false;

        /// <summary>
        /// Dumps the image to the file cache if the <see cref="ImageFileCache"/> is enabled.
        /// </summary>
        public void DumpImage()
        {
            // do nothing if the file cache isn't enabled or the image is already saved to the cache..
            if (!ImageFileCache || ImageDumbed) 
            {
                return;
            }

            try // avoid an exception..
            {
                if (_Image != null)
                {
                    // save the image if it's not already saved..
                    if (!File.Exists(Path.Combine(ImageFileCacheDir, ImageFileName)))
                    {
                        _Image.Save(Path.Combine(ImageFileCacheDir, ImageFileName));
                    }
                }

                using (_Image) // dispose of the image..
                {
                    _Image = null;
                }
                ImageDumbed = true; // indicate that the image was dumped..
            }
            catch
            {

            }
        }

        /// <summary>
        /// Loads the dumped image from the file system cache.
        /// </summary>
        public void LoadDumpedImage()
        {
            // if the file system cache is enabled and the image was dumbed..
            if (ImageFileCache && ImageDumbed)
            {
                // construct a full file name for the image..
                string imageFileName = Path.Combine(ImageFileCacheDir, ImageFileName);
                if (File.Exists(imageFileName)) // load the image if the file exists..
                {
                    _Image = Image.FromFile(imageFileName); // load the image..
                    ImageDumbed = false; // set the dumbed flag..
                }
            }
        }

        /// <summary>
        /// Gets the name of the still/poster image file.
        /// </summary>
        public string ImageFileName
        {
            get
            {
                try // avoid an exception..
                {
                    // return an file name from Uri..
                    return PosterOrStillURL.Segments.Last();
                }
                catch
                {
                    // error, so return nothing..
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// A copy method for a TMDbDetail class instance.
        /// </summary>
        /// <param name="detail">A TMDbDetail class instance to construct a TMDbDetailExt class instance.</param>
        /// <param name="duration">A video duration in milliseconds.</param>
        /// <param name="fileNameFull">A full file name for a video this class would represent of.</param>
        /// <param name="imageFileCache">if set to <c>true</c> the class uses file system for caching image files.</param>
        /// <param name="imageFileCacheDir">A full path to use for the image file cache.</param>
        /// <param name="image">A poster/still image which represents the video file. If set to null a web download is tried.</param>
        /// <param name="videoPlaybackState">Indicates the state of how the video was interacted with before.</param>
        /// <returns>A TMDbDetailExt class instance containing the data from the given TMDbDetail class instance.</returns>
        public static TMDbDetailExt FromTMDbDetail(TMDbDetail detail, int duration, 
            string fileNameFull, bool imageFileCache, string imageFileCacheDir, Image image, VideoPlaybackState videoPlaybackState)
        {
            // create a new TMDbDetailExt class instance..
            TMDbDetailExt result = new TMDbDetailExt
            {
                // copy the data..
                Description = detail.Description,
                Episode = detail.Episode,
                EpisodeID = detail.EpisodeID,
                DetailDescription = detail.DetailDescription,
                FileName = detail.FileName,
                ID = detail.ID,
                PosterOrStillURL = detail.PosterOrStillURL,
                Season = detail.Season,
                SeasonID = detail.SeasonID,
                Title = detail.Title
                // END: copy the data..
            };

            if (image == null) // if the given image parameter was null try to download the image from the web..
            {
                try
                {
                    // download the image data from the web..
                    using (WebClient webClient = new WebClient())
                    {
                        // download the data as byte array..
                        byte[] imageData = webClient.DownloadData(result.PosterOrStillURL);
                        // using (MemoryStream ms = new MemoryStream(imageData)) Exception: A generic error occurred in GDI+

                        // construct a MemoryStream instance of the downloaded image data..
                        MemoryStream ms = new MemoryStream(imageData);

                        // set the image..
                        result.Image = Image.FromStream(ms);
                    }
                }
                catch
                {

                }
            }
            else // an image was given..
            {
                result.Image = image; // ..so set the image..
            }

            // if there is no TMDb ID for the video and the given image was null the image will be null..
            if (detail.ID == -1 && image == null) 
            {
                result.Image = null;
            }

            // set the previous playback state of the video..
            result.VideoPlaybackState = videoPlaybackState;
            
            // set the duration..
            result.Duration = TimeSpan.FromMilliseconds(duration);

            try // try to set the file size of the video..
            {
                result.FileSize = new FileInfo(fileNameFull).Length;
            }
            catch
            {
                result.FileSize = 0;
            }

            // set the file system caching value..
            result.ImageFileCache = imageFileCache;

            // set the file system cache path..
            result.ImageFileCacheDir = imageFileCacheDir;

            // dump the image to the file system cache path if "conditions" are met..
            result.DumpImage();

            return result; // return the TMDbDetailExt class instance
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object (almost).
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// true if the specified object is equal to the current object; otherwise, false.
        /// </returns>
        public bool AlmostEquals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return base.Equals(obj);
            }
            else
            {
                TMDbDetailExt detailExt = (TMDbDetailExt)obj;
                return
                    Description == detailExt.Description &&
                    DetailDescription == detailExt.DetailDescription &&
                    Duration == detailExt.Duration &&
                    Episode == detailExt.Episode &&
                    EpisodeID == detailExt.EpisodeID &&
                    FileName == detailExt.FileName &&
                    FileSize == detailExt.FileSize &&
                    ID == detailExt.ID &&
                    ImageFileCache == detailExt.ImageFileCache &&
                    ImageFileCacheDir == detailExt.ImageFileCacheDir &&
                    ImageFileName == detailExt.ImageFileName &&
                    PosterOrStillURL == detailExt.PosterOrStillURL &&
                    Season == detailExt.Season &&
                    SeasonID == detailExt.SeasonID &&
                    VideoPlaybackState == detailExt.VideoPlaybackState;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_Image != null) // if the image is assigned..
            {
                using (_Image) // ..then dispose of the image..
                {
                    _Image = null;
                }
            }
        }
    }

    /// <summary>
    /// An enumeration to describe the state of how the video was interacted with before.
    /// </summary>
    public enum VideoPlaybackState
    {
        /// <summary>
        /// The video was never played.
        /// </summary>
        New = 0,

        /// <summary>
        /// The video playback was stopped somewhere.
        /// </summary>
        Somewhere = 1,

        /// <summary>
        /// The video was played to the end.
        /// </summary>
        Played = 2
    }
}
