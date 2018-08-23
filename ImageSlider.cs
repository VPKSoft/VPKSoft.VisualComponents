#region License
/*
VPKSoft.ImageSlider

A slider/track bar style control which displays images instead of just colors.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.ImageSlider.

VPKSoft.ImageSlider is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.ImageSlider is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.ImageSlider.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the ImageSlider control.
/// </summary>
namespace VPKSoft.ImageSlider
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A slider/track bar style control which displays images instead of just colors.
    /// </summary>
    [DefaultEvent("ValueChanged")]
    public partial class ImageSlider: UserControl
    {
        /// <summary>
        /// The constructor for the ImageSlider control.
        /// </summary>
        public ImageSlider()
        {
            // generated code..
            InitializeComponent();

            DoubleBuffered = true; // double-buffered is "preferred" for an owner drawn component..
        }

        /// <summary>
        /// The slider direction enumeration.
        /// </summary>
        public enum SliderDrawStyle
        {
            /// <summary>
            /// The slider's value increases from left to right.
            /// </summary>
            LeftToRight,

            /// <summary>
            /// The sliders value increases from bottom to top.
            /// </summary>
            BottomToTop,

            /// <summary>
            /// The slider's value increases from right to left.
            /// </summary>
            RightToLeft,

            /// <summary>
            /// The slider's value increases from top to bottom.
            /// </summary>
            TopToBottom
        }

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        [Category("Behavior")]
        [Description("Indicates whether the control is enabled.")]
        [DefaultValue(true)]
        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }

            set
            {
                base.Enabled = value;
                pnDrawArea.Enabled = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the value is changing.
        /// </summary>
        [Browsable(false)]
        public bool ValueChanging
        {
            get => isMouseDown;
        }

        // an image displayed at positions which are between the Minimum and the Value properties of the control..
        private Image _ImageOnValue = VisualComponents.Properties.Resources.teal_star;

        /// <summary>
        /// Gets or sets an image displayed at positions which are between the Minimum and the Value properties of the control.
        /// </summary>
        [Description("An image displayed at positions which are between the Minimum and the Value properties of the control")]
        [Category("ImageButton")]
        [DefaultValue(null)]
        public Image ImageOnValue
        {
            get
            {
                return _ImageOnValue;
            }

            set
            {
                _ImageOnValue = value;

                // refresh/repaint is needed if some value which would require a visualization change occurs..
                Refresh();
            }
        }

        // a tracker image which is displayed at the position of the Value property of the control..
        private Image _sliderImage = VisualComponents.Properties.Resources.slider;

        /// <summary>
        /// Gets or sets a tracker image which is displayed at the position of the Value property of the control.
        /// </summary>
        [Description("A tracker image which is displayed at the position of the Value property of the control")]
        [Category("ImageButton")]
        [DefaultValue(null)]
        public Image SliderImage
        {
            get
            {
                if (DrawStyle == SliderDrawStyle.LeftToRight || DrawStyle == SliderDrawStyle.RightToLeft)
                {
                    int w = _sliderImage.Width;
                    int h = (int)(ImageOnValue.Height * Multiplier);

                    w = w == 0 ? 1 : w; // one liner to avoid zero value..
                    h = h == 0 ? 1 : h; // one liner to avoid zero value..

                    return (Image)(new Bitmap(_sliderImage, w, h));
                }
                else 
                {
                    int w = (int)(ImageOnValue.Width * Multiplier);
                    int h = _sliderImage.Width;

                    w = w == 0 ? 1 : w; // one liner to avoid zero value..
                    h = h == 0 ? 1 : h; // one liner to avoid zero value..

                    return (Image)(new Bitmap(_sliderImage, w, h));
                }
            }

            set
            {
                _sliderImage = value;

                // refresh/repaint is needed if some value which would require a visualization change occurs..
                Refresh();
            }
        }

        internal int SliderImageWidth
        {
            get
            {
                if (_sliderImage == null)
                {
                    return 0;
                }

                if (DrawStyle == SliderDrawStyle.LeftToRight || DrawStyle == SliderDrawStyle.RightToLeft)
                {
                    return _sliderImage.Width;
                }
                else
                {
                    return (int)(_sliderImage.Height * Multiplier);
                }
            }
        }

        internal int SliderImageHeight
        {
            get
            {
                if (_sliderImage == null)
                {
                    return 0;
                }

                if (DrawStyle == SliderDrawStyle.LeftToRight || DrawStyle == SliderDrawStyle.RightToLeft)
                {
                    return (int)(ImageOnValue.Height * Multiplier);
                }
                else
                {
                    return (int)(_sliderImage.Height * Multiplier);
                }
            }
        }

        // image displayed at positions which are between the Value and the Maximum properties of the control..
        private Image _ImageOffValue = VisualComponents.Properties.Resources.gray_star;

        /// <summary>
        /// Gets or sets an image displayed at positions which are between the Value and the Maximum properties of the control.
        /// </summary>
        [Description("An image displayed at positions which are between the Value and the Maximum properties of the control")]
        [Category("ImageSlider")]
        [DefaultValue(null)]
        public Image ImageOffValue
        {
            get
            {
                return _ImageOffValue;
            }

            set
            {
                _ImageOffValue = value;

                // refresh/repaint is needed if some value which would require a visualization change occurs..
                Refresh();
            }
        }

        internal int _Minimum = 0; // a minimum value for the "slider"..
        internal int _Maximum = 100; // a maximum value for the "slider"..
        internal int _Value = 0; // a current value for the "slider"..

        internal int _previousMinimum = int.MinValue; // avoid refresh if a same minimum value is set.. with a ridiculous initialization value..


        /// <summary>
        /// Gets or set the minimum value of the slider.
        /// </summary>
        [Description("The minimum value of the slider")]
        [Category("ImageSlider")]
        [DefaultValue(0)]
        public int Minimum
        {
            get
            {
                return _Minimum;
            }

            set
            {
                if (_previousMinimum == value) // avoid refresh if a same value is set..
                {
                    return;
                }

                if (value < _Maximum)
                {
                    _Minimum = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Minimum", "The value must be lower than the Maximum value");
                }

                if (value > _Value)
                {
                    _Value = value;
                    _previousMinimum = value;
                }

                _previousMinimum = _Minimum; // save the previous value..

                // refresh/repaint is needed if some value which would require a visualization change occurs..
                Refresh();
            }
        }

        internal int _previousMaximum = int.MinValue; // avoid refresh if a same maximum value is set.. with a ridiculous initialization value..


        /// <summary>
        /// Gets or set the maximum value of the slider.
        /// </summary>
        [Description("The maximum value of the slider")]
        [Category("ImageSlider")]
        [DefaultValue(100)]
        public int Maximum
        {
            get
            {
                return _Maximum;
            }

            set
            {
                if (_previousMaximum == value) // avoid refresh if a same value is set..
                {
                     return;
                }

                if (value > _Minimum)
                {
                    _Maximum = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Maximum", "The value must be greater than the Minimum value");
                }

                if (value < _Value)
                {
                    _Value = value;
                }

                _previousMaximum = _Maximum; // save the previous value..

                // refresh/repaint is needed if some value which would require a visualization change occurs..
                Refresh();
            }
        }

        internal int _previousValue = int.MinValue; // avoid refresh if a same value is set.. with a ridiculous initialization value..

        /// <summary>
        /// Gets or set the current value of the slider.
        /// </summary>
        [Description("The current value of the slider")]
        [Category("ImageSlider")]
        [DefaultValue(0)]
        public int Value
        {
            get
            {
                return _Value;
            }

            set
            {
                if (value == _previousValue) // avoid refresh if a same value is set..
                {
                    return;
                }

                if (value < _Minimum || value > _Maximum)
                {
                    throw new ArgumentOutOfRangeException("Value", "The value must be between the Minimum and Maximum values");
                }
                else
                {
                    _Value = value;
                    _previousValue = value;

                    // .. only if subscribed..
                    ValueChanged?.Invoke(this, new EventArgs());

                    // refresh/repaint is needed if some value which would require a visualization change occurs..
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Sets the current value of the slider without raising an event.
        /// </summary>
        [Description("The current value of the slider")]
        [Category("ImageSlider")]
        [DefaultValue(0)]
        public int ValueNoEvent
        {
            set
            {
                if (value == _previousValue) // avoid refresh if a same value is set..
                {
                    return;
                }

                if (value < _Minimum || value > _Maximum)
                {
                    throw new ArgumentOutOfRangeException("Value", "The value must be between the Minimum and Maximum values");
                }
                else
                {
                    _Value = value;
                    _previousValue = value;

                    // refresh/repaint is needed if some value which would require a visualization change occurs..
                    Refresh();
                }
            }
        }

        // indicates the style of which direction the slider value would increase or decrease
        private SliderDrawStyle _DrawStyle = SliderDrawStyle.LeftToRight;

        /// <summary>
        /// Gets or sets the style of which direction the slider value would increase or decrease.
        /// </summary>
        [Description("A that indicates the style of which direction the slider value would increase or decrease")]
        [Category("ImageSlider")]
        [DefaultValue(SliderDrawStyle.LeftToRight)]
        public SliderDrawStyle DrawStyle
        {
            get
            {
                return _DrawStyle;
            }

            set
            {
                _DrawStyle = value;

                // refresh/repaint is needed if some value which would require a visualization change occurs..
                Refresh();
            }
        }

        // indicates if only whole images should be tile-drawn to the control..
        private bool _OnlyFullImages = true;

        /// <summary>
        /// Gets or sets the value that indicates if only whole images should be tile-drawn to the control.
        /// </summary>
        [Description("A value that indicates if only whole images should be tile-drawn to the control")]
        [Category("ImageSlider")]
        [DefaultValue(true)]
        public bool OnlyFullImages
        {
            get
            {
                return _OnlyFullImages;
            }

            set
            {
                _OnlyFullImages = value;

                // refresh/repaint is needed if some value which would require a visualization change occurs..
                Refresh(); 
            }
        }
        #endregion

        #region Paint
        /// <summary>
        /// Gets the multiplier to allow scaling of the images to fit the control.
        /// </summary>
        internal double Multiplier
        {
            get
            {
                // calculate the maximum size of an image..
                double sizeMax = ImageOnValue.Width > ImageOnValue.Height ?
                    ImageOnValue.Height : ImageOnValue.Width;

                // calculate the maximum size of the control..
                double sizeMaxThis = ClientSize.Width > ClientSize.Height ?
                    ClientSize.Height : ClientSize.Width;

                // return the multiplier..
                return sizeMaxThis / sizeMax;
            }
        }

        // a x-coordinate difference offset which used when a user interacts with the control to change the value property..
        internal int xOffset = 0;

        // a y-coordinate difference offset which used when a user interacts with the control to change the value property..
        internal int yOffset = 0;

        /// <summary>
        /// Draw the slider if the layout is defined as horizontal.
        /// </summary>
        /// <param name="r">A rectangle describing the slider's size.</param>
        /// <param name="w">A single image scale width.</param>
        /// <param name="h">A single image scale height.</param>
        /// <returns>A Bitmap with the full horizontal slider drawn. On failure null value is returned.</returns>
        private Bitmap DrawHorizontal(Rectangle r, int w, int h)
        {
            int loc = 0; // initialize drawing location..

            try
            {
                // save the rectangle for comparison (offset)
                Rectangle compareRect = r;
                // create a zero based rectangle of the given rectangle..
                // ..also limit the rectangle if only full images are allowed to "tile"..
                r = new Rectangle(0, 0, OnlyFullImages ? r.Width - (r.Width % w) : r.Width, r.Height);

                // these difference offsets are used when a user interacts with the control to change the value property..
                xOffset = (compareRect.Width - r.Width) / 2;
                yOffset = (compareRect.Height - r.Height) / 2;
            }
            catch
            {
                // prepare for an "unknown" error.. 
                r = new Rectangle(0, 0, r.Width, r.Height);
            }

            // .. this will cause a further exception so return null
            if (r.Width == 0 || r.Height == 0)
            {
                return null;
            }

            // This is the bitmap the whole slider will be drawn on to..
            Bitmap bm = new Bitmap(r.Width, r.Height);

            using (Graphics g = Graphics.FromImage(bm)) // .. graphics is IDisposable
            {
                g.FillRectangle(new SolidBrush(BackColor), r); // first draw the background color..

                // scale the "disable" image..
                Bitmap bitmap = new Bitmap(ImageOffValue, new Size(w, h));

                // tile the bitmap to the slider bitmap..
                using (bitmap)
                {
                    while (loc < r.Right) // while the tiled bitmap fits to the slider bitmap keep on drawing it..
                    {
                        g.DrawImage(bitmap, new Point(loc, 0));
                        loc += w; // increase the x coordinate..
                    }
                }

                if (!Enabled) // only the disabled image will show if disabled..
                {
                    if (DrawStyle == SliderDrawStyle.RightToLeft) // if RTL..
                    {
                        // .. flip the image horizontally..
                        bm.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }

                    // return the slider image..
                    return bm;
                }

                // scale the "enable" image..
                bitmap = new Bitmap(ImageOnValue, new Size(w, h));

                // tile the bitmap to the slider bitmap..
                using (bitmap)
                {
                    // calculate a maximum value for the tiled bitmap to indicate the value of the slider..
                    int maxValue = r.Width * Value / Maximum;

                    loc = 0;
                    // while the tiled bitmap fits to the slider bitmap's value area keep on drawing it..
                    while (loc + w < maxValue)
                    {
                        g.DrawImage(bitmap, new Point(loc, 0));
                        loc += w; // increase the x coordinate..
                    }

                    // clip the last bitmap to a limited size to indicate the real value of the slider..
                    g.DrawImageUnscaledAndClipped(bitmap, new Rectangle(loc, 0, maxValue - loc, r.Height));

                    // draw the slider position indicator bitmap..
                    g.DrawImage(SliderImage, maxValue - (SliderImage.Width / 2), 0);
                }
            }
            if (DrawStyle == SliderDrawStyle.RightToLeft) // if RTL..
            {
                // .. flip the image horizontally..
                bm.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            // return the slider image..
            return bm;
        }

        /// <summary>
        /// Draw the slider if the layout is defined as vertical.
        /// </summary>
        /// <param name="r">A rectangle describing the slider's size.</param>
        /// <param name="w">A single image scale width.</param>
        /// <param name="h">A single image scale height.</param>
        /// <returns>A Bitmap with the full vertical slider drawn. On failure null value is returned.</returns>
        private Bitmap DrawVertical(Rectangle r, int w, int h)
        {
            int loc = 0; // initialize drawing location..

            try
            {
                // save the rectangle for comparison (offset)
                Rectangle compareRect = r;

                // create a zero based rectangle of the given rectangle..
                // ..also limit the rectangle if only full images are allowed to "tile"..
                r = new Rectangle(0, 0, r.Width, OnlyFullImages ? r.Height - (r.Height % h) : r.Height);

                // these difference offsets are used when a user interacts with the control to change the value property..
                xOffset = (compareRect.Width - r.Width) / 2;
                yOffset = (compareRect.Height - r.Height) / 2;
            }
            catch
            {
                // prepare for an "unknown" error.. 
                r = new Rectangle(0, 0, r.Width, r.Height);
            }

            // .. this will cause a further exception so return null
            if (r.Width == 0 || r.Height == 0)
            {
                return null;
            }

            // This is the bitmap the whole slider will be drawn on to..
            Bitmap bm = new Bitmap(r.Width, r.Height);

            using (Graphics g = Graphics.FromImage(bm)) // .. graphics is IDisposable
            {
                g.FillRectangle(new SolidBrush(BackColor), r); // first draw the background color..

                // scale the "enable" image..
                Bitmap bitmap = new Bitmap(ImageOffValue, new Size(w, h));

                // tile the bitmap to the slider bitmap..
                using (bitmap)
                {
                    while (loc < r.Bottom) // while the tiled bitmap fits to the slider bitmap keep on drawing it..
                    {
                        g.DrawImage(bitmap, new Point(0, loc));
                        loc += h; // increase the y coordinate..
                    }
                }

                if (!Enabled) // only the disabled image will show if disabled..
                {
                    if (DrawStyle == SliderDrawStyle.TopToBottom) // if top to bottom..
                    {
                        // .. flip the image vertically..
                        bm.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }

                    // return the slider image..
                    return bm;
                }

                // scale the "disable" image..
                bitmap = new Bitmap(ImageOnValue, new Size(w, h));

                // tile the bitmap to the slider bitmap..
                using (bitmap)
                {
                    // calculate a maximum value for the tiled bitmap to indicate the value of the slider..
                    int maxValue = (r.Height * Value / Maximum);

                    loc = 0;

                    // while the tiled bitmap fits to the slider bitmap's value area keep on drawing it..
                    while (loc + h < maxValue)
                    {
                        g.DrawImage(bitmap, new Point(0, loc));
                        loc += h; // increase the y coordinate..
                    }

                    // clip the last bitmap to a limited size to indicate the real value of the slider..
                    g.DrawImageUnscaledAndClipped(bitmap, new Rectangle(0, loc, r.Right, maxValue - loc));

                    // draw the slider position indicator bitmap..
                    g.DrawImage(SliderImage, 0, maxValue - (SliderImage.Height / 2));
                }

                if (DrawStyle == SliderDrawStyle.TopToBottom) // if top to bottom..
                {
                    // .. flip the image vertically..
                    bm.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                // return the slider image..
                return bm;
            }
        }

        // The actual paint procedure
        private void pnDrawArea_Paint(object sender, PaintEventArgs e)
        {
            // fill the draw are with the control's background color..
            e.Graphics.FillRectangle(new SolidBrush(BackColor), e.ClipRectangle);

            // get the image size multiplier..
            double multiplier = Multiplier;

            // calculate the image width and height based on the multiplier..
            int w = (int)(ImageOnValue.Width * multiplier);
            int h = (int)(ImageOnValue.Height * multiplier);


            // a horizontal image is requested..
            if (DrawStyle == SliderDrawStyle.LeftToRight ||
                DrawStyle == SliderDrawStyle.RightToLeft)
            {
                // .. so create a horizontal slider image
                Bitmap bm = DrawHorizontal(e.ClipRectangle, w, h);

                if (bm == null) // failure, so don't resume
                {
                    return;
                }

                using (bm) // draw the image to the center of the paint area..
                {
                    e.Graphics.DrawImage(bm, ((e.ClipRectangle.Width - bm.Width) / 2), ((e.ClipRectangle.Height - bm.Height) / 2));
                }
            }
            // a vertical image is requested..
            else if (DrawStyle == SliderDrawStyle.BottomToTop ||
                     DrawStyle == SliderDrawStyle.TopToBottom)
            {
                // .. so create a vertical slider image
                Bitmap bm = DrawVertical(e.ClipRectangle, w, h);

                if (bm == null) // failure, so don't resume
                {
                    return;
                }

                using (bm) // draw the image to the center of the paint area..
                {                    
                    e.Graphics.DrawImage(bm, ((e.ClipRectangle.Width - bm.Width) / 2), ((e.ClipRectangle.Height - bm.Height) / 2));
                }
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Tries to set the value for the Value property of the control.
        /// </summary>
        /// <param name="value">A new value for the Value property.</param>
        /// <returns>True if the value was within acceptable range, otherwise false.</returns>
        public bool TrySetValue(int value)
        {
            if (value >= Minimum && value <= Maximum)
            {
                Value = value;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region InternalLogic
        // indicates if the mouse is down on the control or not..
        internal bool isMouseDown = false;

        // a mouse down needs to be "recorded" for the slider/track bar to work..
        private void pnDrawArea_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true; // the mouse is down..
            base.OnMouseDown(e); // re-route the event to the lower lever..
            pnDrawArea_MouseMove(sender, e); // cause a virtual mouse move as well..
        }

        // with the design of this control many events will assign the mouse not to be down..
        private void pnDrawArea_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            base.OnMouseUp(e); // re-route the event to the lower lever..
        }

        // this is just for re-routing the event to base level..
        private void pnDrawArea_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e); // re-route the event to the lower lever..
        }

        // this is just for re-routing the event to base level..
        private void pnDrawArea_Leave(object sender, EventArgs e)
        {
            isMouseDown = false;
            base.OnLeave(e); // re-route the event to the lower lever..
        }

        // with the design of this control many events will assign the mouse not to be down..
        private void pnDrawArea_MouseLeave(object sender, EventArgs e)
        {
            isMouseDown = false;
            base.OnMouseLeave(e); // re-route the event to the lower lever..
        }

        // this will do the Value property change when the mouse is down and it's in correct coordinates..
        private void pnDrawArea_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e); // re-route the event to the lower lever..

            // there is only a one Panel on this control - discard all else ( - where there should be none)..
            if (!(sender is Panel))
            {
                return;
            }

            // do an explicit cast as now the sender can't be anything else than an instance of an Panel
            Panel pn = (Panel)sender;

            // only do something if the mouse is down..
            if (isMouseDown)
            {
                // save the ClientRectangle of the Panel to "manipulate" it..
                Rectangle r = pn.ClientRectangle;

                // do the manipulation.. for the rectangle..
                r = new Rectangle(r.X + xOffset, r.Y + yOffset, r.Width - xOffset * 2, r.Height - yOffset * 2);

                // save to mouse location into a Point
                Point p = e.Location;

                // if the "manipulated" rectangle contains the mouse position, an action is needed..
                if (r.Contains(p))
                {
                    // each drawing style requires a bit different approach..

                    // ..left to right seems to be easiest to understand..
                    if (DrawStyle == SliderDrawStyle.LeftToRight)
                    {
                        // calculate a value for the slider/track bar..
                        double d = (double)(p.X - xOffset) / (r.Width - SliderImageWidth / 2);
                        d *= (double)Maximum - Minimum;
                        TrySetValue((int)d); // try to set it..
                    }
                    // ..difficulty grows as thought backwards from the previous calculation..
                    else if (DrawStyle == SliderDrawStyle.RightToLeft)
                    {
                        // calculate a value for the slider/track bar..
                        double d = (double)(p.X - xOffset) / (r.Width - SliderImageWidth / 2);
                        d *= (double)Maximum - Minimum;
                        d = Maximum - d; // .. but the only thing required is to invert the value..
                        TrySetValue((int)d); // ..try to set the value..
                    }
                    // ..bottom to top seems to be the next easiest to understand..
                    else if (DrawStyle == SliderDrawStyle.BottomToTop)
                    {
                        // calculate a value for the slider/track bar..
                        double d = (double)(p.Y - yOffset) / (r.Height - SliderImageHeight / 2);
                        d *= (double)Maximum - Minimum;
                        TrySetValue((int)d); // ..try to set the value..
                    }
                    // AGAIN ..difficulty grows as thought backwards from the previous calculation..
                    else if (DrawStyle == SliderDrawStyle.TopToBottom)
                    {
                        // calculate a value for the slider/track bar..
                        double d = (double)(p.Y - yOffset) / (r.Height - SliderImageHeight / 2);
                        d *= (double)Maximum - Minimum;
                        d = Maximum - d; // .. but the only thing required is to invert the value..
                        TrySetValue((int)d); // ..try to set the value..
                    }
                }
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Gets or sets the cursor that is displayed when the mouse pointer is over the control.
        /// </summary>
        [Description("The cursor that appears when the pointer moves over the control.")]
        [Category("Appearance")]
        public override Cursor Cursor
        {
            get => base.Cursor;

            set
            {
                base.Cursor = value;
                pnDrawArea.Cursor = value;
            }
        }
        #endregion

        #region PublicEvents
        /// <summary>
        /// Occurs when the Value property of a ImageSlider changes, either by mouse interaction or by manipulation in code.
        /// </summary>
        [Category("Action")]
        [Description("An event that occurs when the Value property of a ImageSlider changes, either by mouse interaction or by manipulation in code")]
        public event EventHandler ValueChanged = null;
        #endregion
    }
}
