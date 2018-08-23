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
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

// A summary for name space will give warnings, but some help file generator may need it..
#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the ImagePanel control.
/// </summary>
namespace VPKSoft.ImagePanel
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A class which resizes an image to fit a Control's client size.
    /// <para/>Stack Overflow Question: https://stackoverflow.com/questions/1940581/c-sharp-image-resizing-to-different-size-while-preserving-aspect-ratio
    /// <para/>The accepted answer by user: https://stackoverflow.com/users/43603/sf
    /// </summary>
    public static class ImageResizer
    {
        /// <summary>
        /// Scales an image to fit to a client size of the given control.
        /// </summary>
        /// <param name="image">An image to scale.</param>
        /// <param name="control">A control which dimensions are used as a reference.</param>
        /// <returns>A scaled image to fit the control's client area with transparent background.</returns>
        public static Image ScaleToFitControl(Image image, Control control)
        {
            if (image == null) // can't do anything to a null object..
            {
                return null;
            }

            // Slightly modified version of:
            // (C): https://stackoverflow.com/questions/1940581/c-sharp-image-resizing-to-different-size-while-preserving-aspect-ratio

            // Take the control's client size in to mind. The image is supposed to be centered..
            double controlMin = Math.Min(control.ClientSize.Width, control.ClientSize.Height); // min from the control..

            // Get the larger value of the image's width and height
            double imageMax = Math.Max(image.Width, image.Height); // ..max from the image

            double width = image.Width * controlMin / imageMax; // scale the destination width to fit the control..
            double height = image.Height * controlMin / imageMax; // scale the destination height to fit the control..

            double imageWidth = image.Width; // the actual size of the image (width) --> to double..
            double imageHeight = image.Height; // the actual size of the image (height) --> to double..

            double scalePercentage =  // calculate the relative scale percentage..
                width / imageWidth > height / imageHeight ?
                width / imageWidth : height / imageHeight;

            // Create a Bitmap that holds a new image with and alpha-channel (R=8-bit = byte, G=8-bit = byte, B=8-bit = byte, A=8-bit = byte)..
            // ..complex (?) - haha..

            if (width < 1.0) // to prevent zero sized images..
            {
                width = 1.0;
            }

            if (height < 1.0) // .. again to prevent zero sized images..
            {
                height = 1.0;
            }

            Bitmap bmImage = new Bitmap((int)width, (int)height, PixelFormat.Format32bppPArgb);

            // set the resolution to match the original image..
            bmImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics grImage = Graphics.FromImage(bmImage)) // always use [using] on an IDisposable object..
            {
                // We need high quality icons / clip-art..
                grImage.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grImage.DrawImage(image, new Rectangle((int)((width - (imageWidth * scalePercentage)) / 2.0),
                    (int)((height - (imageHeight * scalePercentage)) / 2.0),
                    (int)(imageWidth * scalePercentage), (int)(imageHeight * scalePercentage)),
                    new Rectangle(0, 0, (int)imageWidth, (int)imageHeight), GraphicsUnit.Pixel);
            }
            return bmImage; // return the new image..
        }

        /// <summary>
        /// Gets a size of an image scaled to a given control's client size.
        /// </summary>
        /// <param name="image">An Image to compare to control's client size.</param>
        /// <param name="control">A control to compare with an image.</param>
        /// <returns>A size for the given image if it were to be scaled to fit the control's client area.</returns>
        public static Size NewSize(Image image, Control control)
        {
            // Take the control's client size in to mind. The image is supposed to be centered..
            double controlMin = Math.Min(control.ClientSize.Width, control.ClientSize.Height); // min from the control..

            // Get the larger value of the image's width and height
            double imageMax = Math.Max(image.Width, image.Height); // ..max from the image

            double width = image.Width * controlMin / imageMax; // scale the destination width to fit the control..
            double height = image.Height * controlMin / imageMax; // scale the destination height to fit the control..

            if (width < 1.0) // to prevent zero sized images..
            {
                width = 1.0;
            }

            if (height < 1.0) // .. again to prevent zero sized images..
            {
                height = 1.0;
            }

            // return the new size..
            return new Size((int)width, (int)height);
        }
    }
}
