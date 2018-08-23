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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A VPKSoft.VisualUtils name space which holds the UtilsMisc class.
/// </summary>
namespace VPKSoft.VisualUtils
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// Some utilities for mostly resizing controls / (and) or fonts, etc..
    /// </summary>
    public static class UtilsMisc
    {
        /// <summary>
        /// A string to measure font sizes
        /// </summary>
        public const string MeasureText = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö£€%[]$@ÂÊÎÔÛâêîôûÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÁÉÍÓÚáéíóúÃÕãõ '|?+\\/{}½§01234567890+<>_-:;*&¤#\"!";

        /// <summary>
        /// Resizes a font so that it fit in a label by its height
        /// </summary>
        /// <param name="control">A control which font to resize</param>
        /// <param name="useRefString">Whether to use the UtilsMisc.MeasureText constant or the controls own Text contents.</param>
        /// <param name="stepping">How much to change the font size in the resize loop.</param>
        /// <param name="height">A height to reference to if the controls height value is invalid (e.g. docked controls).</param>
        public static void ResizeFontHeight(Control control, bool useRefString = false, float stepping = 0.5f, int height = 0)
        {
            height = height == 0 ? control.Height : height; // select one of the height(s)

            // Do font size measuring until its height is "correct": https://stackoverflow.com/questions/9527721/resize-text-size-of-a-label-when-the-text-got-longer-than-the-label-size

            int fHeight = System.Windows.Forms.TextRenderer.MeasureText(useRefString ? MeasureText : control.Text, new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style)).Height;

            bool decrease = fHeight > height;
            float changeValue = decrease ? -stepping : stepping; // Decrease or increase ?..

            if (fHeight == 0) // Something is wrong (?!), return so an infinite loop does not occur..
            {
                return;
            }

            while ((fHeight > height && decrease) ||
                   (fHeight < height && !decrease))
            {
                if (control.Font.Size + changeValue < 0) // No less than zero effects..
                {
                    break;
                }

                control.Font = new Font(control.Font.FontFamily, control.Font.Size + changeValue, control.Font.Style);
                fHeight = System.Windows.Forms.TextRenderer.MeasureText(useRefString ? MeasureText : control.Text, new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style)).Height;
            }

            // Ensure that no oversizing happened..
            if (control.Font.Size - changeValue > 0 && !decrease)
            {
                control.Font = new Font(control.Font.FontFamily, control.Font.Size - changeValue, control.Font.Style);
            }
        }

        /// <summary>
        /// Resizes a font so that it fit in a label by its width
        /// </summary>
        /// <param name="control">A control which font to resize</param>
        /// <param name="refString">An alternative string to measure the text width instead the Control's Text property.</param>
        /// <param name="stepping">How much to change the font size in the resize loop.</param>
        /// <param name="width">A width to reference to if the controls width value is invalid (e.g. docked controls).</param>
        public static void ResizeFontWidth(Control control, string refString = "", float stepping = 0.5f, int width = 0)
        {
            width = width == 0 ? control.Width : width; // select one of the height(s)

            // Do font size measuring until its height is "correct": https://stackoverflow.com/questions/9527721/resize-text-size-of-a-label-when-the-text-got-longer-than-the-label-size

            refString = refString == string.Empty ? control.Text : refString;

            int fWidth = System.Windows.Forms.TextRenderer.MeasureText(refString, new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style)).Width;

            bool decrease = fWidth > width;
            float changeValue = decrease ? -stepping : stepping; // Decrease or increase ?..

            if (fWidth == 0) // Something is wrong (?!), return so an infinite loop does not occur..
            {
                return;
            }

            while ((fWidth > width && decrease) ||
                   (fWidth < width && !decrease))
            {
                if (control.Font.Size + changeValue < 0) // No less than zero effects..
                {
                    break;
                }

                control.Font = new Font(control.Font.FontFamily, control.Font.Size + changeValue, control.Font.Style);
                fWidth = System.Windows.Forms.TextRenderer.MeasureText(refString, new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style)).Width;
            }

            // Ensure that no oversizing happened..
            if (control.Font.Size - changeValue > 0 && !decrease)
            {
                control.Font = new Font(control.Font.FontFamily, control.Font.Size - changeValue, control.Font.Style);
            }
        }

        /// <summary>
        /// Resizes a font so that it fit in a label by its width
        /// </summary>
        /// <param name="control">A Control which font to resize</param>
        /// <param name="refString">An alternative string to measure the text width instead the Control's Text property.</param>
        /// <param name="stepping">How much to change the font size in the resize loop.</param>
        /// <param name="width">A width to reference to if the controls width value is invalid (e.g. docked controls).</param>
        /// <param name="height">A height to reference to if the controls height value is invalid (e.g. docked controls).</param>         
        public static void ResizeFontWidthHeight(Control control, string refString = "", float stepping = 0.5f, int width = 0, int height = 0)
        {
            width = width == 0 ? control.Width : width; // select one of the height(s)
            height = height == 0 ? control.Height : height; // select one of the height(s)

            // Do font size measuring until its height is "correct": https://stackoverflow.com/questions/9527721/resize-text-size-of-a-label-when-the-text-got-longer-than-the-label-size

            refString = refString == string.Empty ? control.Text : refString;

            Size fontSize = System.Windows.Forms.TextRenderer.MeasureText(refString, new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style));

            int fWidth = fontSize.Width;
            int fHeigh = fontSize.Height;

            bool decrease = fWidth > width || fHeigh > height;
            float changeValue = decrease ? -stepping : stepping; // Decrease or increase ?..

            if (fWidth == 0 || fHeigh == 0) // Something is wrong (?!), return so an infinite loop does not occur..
            {
                return;
            }

            while (((fWidth > width && decrease) ||
                    (fWidth < width && !decrease)) &&
                   ((fHeigh > height && decrease) ||
                   (fHeigh < height && !decrease)))
            {
                if (control.Font.Size + changeValue < 0) // No less than zero effects..
                {
                    break;
                }

                control.Font = new Font(control.Font.FontFamily, control.Font.Size + changeValue, control.Font.Style);
                fontSize = System.Windows.Forms.TextRenderer.MeasureText(refString, new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style));
                fWidth = fontSize.Width;
                fHeigh = fontSize.Height;
            }

            // Ensure that no oversizing happened..
            if (control.Font.Size - changeValue > 0 && !decrease)
            {
                control.Font = new Font(control.Font.FontFamily, control.Font.Size - changeValue, control.Font.Style);
            }
        }

        /// <summary>
        /// Scales a Form to fit to the screen it's on and centers it.
        /// </summary>
        /// <param name="form">A Form class instance which size to change.</param>
        /// <param name="percentageStep">A value in percentage of how much the form's size should be increased or decreased with one iteration.</param>
        public static void ScaleToFitScreen(Form form, double percentageStep = 10.0)
        {
            Screen screen = Screen.FromRectangle(form.Bounds); // Get the screen the form is mostly on..

            percentageStep /= 100;

            form.SuspendLayout(); // suspend the layout "engine" of the form to avoid layout on every scaling iteration..
            form.AutoScaleMode = AutoScaleMode.None; // can't scale if AutoScaleMode is set..
            form.StartPosition = FormStartPosition.Manual; // .. and can't set the position if it's "automatic"..

            // determine tho which direction the form should be scaled..
            bool increaseScaling = form.Size.Height < screen.WorkingArea.Size.Height && form.Size.Width < screen.WorkingArea.Size.Width;


            // Scale if scaling downward is required..
            while (form.Size.Height > screen.WorkingArea.Size.Height || form.Size.Width > screen.WorkingArea.Size.Width && !increaseScaling)
            {
                int dec = Math.Max((int)((double)form.Size.Height * percentageStep), (int)((double)form.Size.Width * percentageStep)); // try to keep the scaling 
                form.Size = new Size(form.Size.Width - dec, form.Size.Height - dec);
            }

            // Scale if scaling upward is required..
            int inc = Math.Max((int)((double)form.Size.Height * percentageStep), (int)((double)form.Size.Width * percentageStep)); // try to keep the scaling 
            while (form.Size.Height + inc < screen.WorkingArea.Size.Height && form.Size.Width + inc < screen.WorkingArea.Size.Width && increaseScaling)
            {
                form.Size = new Size(form.Size.Width + inc, form.Size.Height + inc);
                inc = Math.Max((int)((double)form.Size.Height * percentageStep), (int)((double)form.Size.Width * percentageStep)); // try to keep the scaling 
            }

            // align the form and resume layout..
            form.Left = (screen.Bounds.Width / 2) - (form.Size.Width / 2);
            form.Top = (screen.Bounds.Height / 2) - (form.Size.Height / 2);
            form.ResumeLayout();
        }

        // (C): https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /* License:
         Copyright (c) 2006-2010 Paranoid Ferret Productions.  All rights reserved.

          Developed by: Paranoid Ferret Productions
                        http://www.paranoidferret.com

          Permission is hereby granted, free of charge, to any person obtaining a copy
          of this software and associated documentation files (the "Software"), to
          deal with the Software without restriction, including without limitation the
          rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
          sell copies of the Software, and to permit persons to whom the Software is
          furnished to do so, subject to the following conditions:
            1. Redistributions of source code must retain the above copyright notice,
               this list of conditions and the following disclaimers.
            2. Redistributions in binary form must reproduce the above copyright
               notice, this list of conditions and the following disclaimers in the
               documentation and/or other materials provided with the distribution.
            3. Neither the name Paranoid Ferret Productions, nor the names of its 
               contributors may be used to endorse or promote products derived 
               from this Software without specific prior written permission.

          THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
          IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
          FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
          CONTRIBUTORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
          LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
          FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
          WITH THE SOFTWARE.
        */

        // NOTE: This is slightly modified version of the original code, calling a dispose method is changed using clause.. 
        // and the bitmap parameter is changed to 

        /// <summary>
        /// Makes a given Image to a gray-scaled Image.
        /// </summary>
        /// <param name="image">The image to convert into a gray-scaled image.</param>
        /// <returns>An image converted into a gray-scaled version of the original image.</returns>
        public static Image MakeGrayscale3(Image image)
        {
            Bitmap newBitmap = new Bitmap(image);
            //create a blank bitmap the same size as original

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                //create the gray-scale ColorMatrix
               ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
               });

                //create some image attributes
                ImageAttributes attributes = new ImageAttributes();

                //set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                //draw the original image on the new image
                //using the gray-scale color matrix
                using (Bitmap original = new Bitmap(image))
                {
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                       0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
        }
    }
}
