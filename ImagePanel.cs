#region License
/*
VPKSoft.ImagePanel

A simple panel component which scales a given image to the center of the panel.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.ImagePanel.

VPKSoft.ImagePanel is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.ImagePanel is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.ImagePanel.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using VPKSoft.Hashes;

// A summary for name space will give warnings, but some help file generator may need it..
#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the ImagePanel control.
/// </summary>
namespace VPKSoft.ImagePanel
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A simple panel component which scales a given image to the center of the panel.
    /// </summary>
    public partial class ScaleImagePanel : UserControl
    {
        /// <summary>
        /// The class constructor.
        /// </summary>
        public ScaleImagePanel()
        {
            InitializeComponent();
            DoubleBuffered = true; // Just to avoid flickering..
        }

        internal static List<KeyValuePair<Image, string>> globalCache = new List<KeyValuePair<Image, string>>();

        // The original sized image of the _BackgroundImage value..
        private Image _BackgroundImageOriginal = null;

        // An MD5 hash of the previous image given to the control's background..
        private string lastImageMD5 = string.Empty;

        // A resized version scaled to fit the control..
        private Image _BackgroundImage = null;

        // The last size of the background image so it won't be resized without a reason..
        private Size lastBackroundImageSize = new Size(0, 0);

        /// <summary>
        /// Generates a MD5 hexadecimal representation of a given image as PNG.
        /// </summary>
        /// <param name="image">An image of which hash to generate.</param>
        /// <returns>A MD5 hexadecimal representation of a given image as PNG.</returns>
        public static string MD5HashImagePNG(Image image)
        {
            MD5 mD5 = MD5.Create(); // Create an MD5 hash algorithm..

            // the image will be saved to the memory stream for the MD5 hash generation.. 
            // .. it is expected that the image not be of gigabytes in size.
            using (MemoryStream ms = new MemoryStream()) 
            {
                image.Save(ms, ImageFormat.Png); // save the image to the memory stream..
                IOHash.MD5AppenStream(ms, ref mD5); // .. hash the memory stream..
                string retval = IOHash.MD5GetHashString(ref mD5); // get the MD5 hexadecimal representation..
                mD5.Dispose(); // dispose the IDisposable..
                return retval; // return the MD5 hexadecimal representation..
            }
        }

        /// <summary>
        /// Gets or sets the background image displayed in the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The background image used for the control")]
        public override Image BackgroundImage
        {
            get
            {
                return _BackgroundImageOriginal; // return the original sized image of the BackgroundImage property..
            }
            
            set
            {
                if (value == null) // if a null value was given, there is nothing to do..
                {
                    return; // ..and then do nothing.
                }

                string md5HashVerify = MD5HashImagePNG(value); // hash the given "new" value..
                if (globalCache.Exists(f => f.Value == md5HashVerify))
                {
                    value = globalCache.First(f => f.Value == md5HashVerify).Key;
                }
                else
                {
                    globalCache.Add(new KeyValuePair<Image, string>(value, md5HashVerify));
                }

                if (_BackgroundImage == null) // If the background image is null..
                {
                    _BackgroundImageOriginal = value; // save the image..
                    lastImageMD5 = MD5HashImagePNG(value); // hash the image so a change will be remembered..
                    _BackgroundImage = ImageResizer.ScaleToFitControl(value, pnImage); // scale the image to fit the control..
                    pnImage.BackgroundImage = _BackgroundImage; // assign the image to the actual "holder" panel..
                    lastBackroundImageSize = _BackgroundImage.Size; // assign the size of the image so it won't be uselessly resized..
                }
                else // a background image is already assigned..
                {
                    string md5Hash = MD5HashImagePNG(value); // hash the given "new" value..
                    if (lastImageMD5 != md5Hash) // ..only change the image if the new image is different..
                    {
                        _BackgroundImageOriginal = value; // save the unscaled image..
                        _BackgroundImage = ImageResizer.ScaleToFitControl(value, pnImage); // save scaled image..
                        pnImage.BackgroundImage = _BackgroundImage; // assign the scaled image..
                        lastBackroundImageSize = _BackgroundImage.Size; // save the last size of the scaled image..
                        lastImageMD5 = md5Hash;
                    }
                    else // nothing was changed - if not the scaled size..
                    {
                        if (_BackgroundImage.Size.Equals(lastBackroundImageSize)) // if the scaled size wasn't changed..
                        {
                            return; // .. do nothing.
                        }
                        // the scaled size was changed, however the image wasn't so just scale it to fit..
                        _BackgroundImage = ImageResizer.ScaleToFitControl(_BackgroundImageOriginal, pnImage);
                        pnImage.BackgroundImage = _BackgroundImage; // assign the scaled image..
                        lastBackroundImageSize = _BackgroundImage.Size; // save the last size of the scaled image..
                    }
                }
            }
        }

        // this one is for the background image scaling..
        private void pnImage_ClientSizeChanged(object sender, EventArgs e)
        {
            if (_BackgroundImageOriginal != null) // .. if assigned..
            {
                // compare the client size to the scaled image size do determine if scaling is required..
                if (ImageResizer.NewSize(_BackgroundImageOriginal, pnImage).Equals(lastBackroundImageSize))
                {
                    return; // .. not required!
                }

                _BackgroundImage = ImageResizer.ScaleToFitControl(_BackgroundImageOriginal, pnImage); // ..save scaled image..
                pnImage.BackgroundImage = _BackgroundImage; // assign the scaled image..
                lastBackroundImageSize = _BackgroundImage.Size; // save the last size of the scaled image..
            }
        }

        // Set the ForeColor for the overlaid controls as well..
        /// <summary>
        ///  Gets or sets the foreground color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The foreground color of the component, which is used to display text.")]
        [DefaultValue(typeof(Color), "ControlText")]
        public override Color ForeColor
        {
            get => base.ForeColor;

            set
            {
                base.ForeColor = value;
                pnImage.ForeColor = value;
            }
        }

        // Set the BackColor for the overlaid controls as well..
        /// <summary>
        ///  Gets or sets the background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The background color of the component.")]
        [DefaultValue(typeof(Color), "Control")]
        public override Color BackColor
        {
            get => base.BackColor;

            set
            {
                base.BackColor = value;
                pnImage.BackColor = value;
            }
        }

    }
}
