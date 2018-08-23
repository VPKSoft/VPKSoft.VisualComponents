#region License
/*
VPKSoft.VisualScrollBar

A custom scroll bar control with more visualization possibilities that the basic one.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.VisualScrollBar.

VPKSoft.VisualScrollBar is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.VisualScrollBar is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.VisualScrollBar.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the VPKSoft.VisualScrollBar class.
/// </summary>
namespace VPKSoft.VisualScrollBar
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A custom scroll bar control with more visualization possibilities that the basic one.
    /// </summary>
    public partial class VisualScrollBar: UserControl
    {
        /// <summary>
        /// The VPKSoft.VisualScrollBar class constructor. DoubleBuffered is set to true because part of the control is owner-drawn.
        /// </summary>
        public VisualScrollBar()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        #region Paint
        private void pnScrollBar_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)(sender); // Get the Panel to be painted..
            e.Graphics.FillRectangle(_BackgroundBrush, e.ClipRectangle); // fill the panels's background..

            if (_Horizontal) // if the scroll bar is set to be horizontal, paint accordingly..
            {
                // from the position between Minimum and Maximum value, draw the scroll bar's slider..
                if (_ScrollerWH + Convert.ToInt32(panel.Tag) >= panel.Width)  // ..and try to keep within the area's boundaries
                {
                    e.Graphics.FillRectangle(_SliderBrush, new Rectangle(e.ClipRectangle.Width - _ScrollerWH, 0, _ScrollerWH, e.ClipRectangle.Height));
                }
                else if (_ScrollerWH + Convert.ToInt32(panel.Tag) < 0) // a negative position would occur..
                {
                    e.Graphics.FillRectangle(_SliderBrush, new Rectangle(0, 0, _ScrollerWH, e.ClipRectangle.Height));
                }
                else // no boundaries were exceeded so the normal case
                {
                    e.Graphics.FillRectangle(_SliderBrush, new Rectangle(Convert.ToInt32(panel.Tag), 0, _ScrollerWH, e.ClipRectangle.Height));
                }
            }
            else // if the scroll bar is set to be vertical, paint accordingly..
            {
                // from the position between Minimum and Maximum value, draw the scroll bar's slider..
                if (_ScrollerWH + Convert.ToInt32(panel.Tag) >= panel.Height) // ..and try to keep within the area's boundaries
                {
                    e.Graphics.FillRectangle(_SliderBrush, new Rectangle(0, e.ClipRectangle.Height - _ScrollerWH, e.ClipRectangle.Width, _ScrollerWH));
                }
                else if (_ScrollerWH + Convert.ToInt32(panel.Tag) < 0) // a negative position would occur..
                {
                    e.Graphics.FillRectangle(_SliderBrush, new Rectangle(0, 0, e.ClipRectangle.Width, _ScrollerWH));
                }
                else // no boundaries were exceeded so the normal case
                {
                    e.Graphics.FillRectangle(_SliderBrush, new Rectangle(0, Convert.ToInt32(panel.Tag), e.ClipRectangle.Width, _ScrollerWH));
                }
            }
            ScrollerPosition = Convert.ToInt32(pnScrollBar.Tag); // Inform the possible value change to the scroll bar's "internal logic"..
        }

        private void pnScrollBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Only left button's (NOTE: this does not discriminate the left handed as the OS handles the clicks so)
            {
                Panel panel = (Panel)(sender); // Get the Panel which was "clicked"..
                panel.Tag = _Horizontal ? e.X : e.Y; // Save the mouse position to the panel's Tag property..
                panel.Invalidate(); // make the panel to re-paint..
            }
        }

        private void pnScrollBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Horizontal) // If the scroll bar is horizontal
            {
                if (e.Button == MouseButtons.Left && e.X >= 0) // And left button is down and the X-coordinate is valid..
                {
                    Panel panel = (Panel)(sender); // Get the sending panel..
                    panel.Tag = e.X; // ..save the mouse position to the panel's Tag property
                    panel.Invalidate(); // make the panel to re-paint..
                }
            }
            else // If the scroll bar is vertical
            {
                if (e.Button == MouseButtons.Left && e.Y >= 0) // And left button is down and the Y-coordinate is valid..
                {
                    Panel panel = (Panel)(sender); // Get the sending panel..
                    panel.Tag = e.Y; // ..save the mouse position to the panel's Tag property
                    panel.Invalidate(); // make the panel to re-paint..
                }
            }
        }

        // On of the buttons (up-down or left-right was clicked)..
        private void pnScrollWHClick(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender; // Get the sending panel..
            if (panel.Equals(pnScrollUp)) // Left or Up button was clicked depending of the Horizontal property value..
            {
                pnScrollBar.Tag = (Convert.ToInt32(pnScrollBar.Tag) - LargeChange >= 0) ?
                    Convert.ToInt32(pnScrollBar.Tag) - LargeChange : 0; // this is the decrementing button so the logic is easy..
                pnScrollBar.Invalidate(); // Redraw/repaint..
            }
            else if (panel.Equals(pnScrollDown)) // Right or Down button was clicked depending of the Horizontal property value.. this is a bit more complex case..
            {
                int currentValue = Convert.ToInt32(pnScrollBar.Tag); // put the current position into a variable to avoid excess amount of code writing..

                // calculate the maximum value for the Tag property..
                int maxValue = _Horizontal ? //.. depending whether the scroll bar is horizontal or vertical..
                    pnScrollBar.Width - LargeChange : pnScrollBar.Height - LargeChange;

                if (_Horizontal) // dealing with a horizontal scroll bar here..
                {
                    // a simple calculation (?) - just avoid a value which makes the scroll box not to fit the scroll bar's area..
                    pnScrollBar.Tag = (currentValue + LargeChange < maxValue) ?
                        currentValue + LargeChange : pnScrollBar.Tag = maxValue;
                }
                else // dealing with a vertical scroll bar here..
                {
                    // a simple calculation (?) - just avoid a value which makes the scroll box not to fit the scroll bar's area..
                    pnScrollBar.Tag = (currentValue + LargeChange < maxValue) ?
                        pnScrollBar.Tag = currentValue + LargeChange : maxValue;
                }
                pnScrollBar.Invalidate(); // Redraw/repaint..
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// The VPKSoft.VisualScrollBar.Value property is changed..
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">ScrollEventArgs class instance.</param>
        public delegate void OnValueChanged(object sender, ScrollEventArgs e);

        /// <summary>
        /// Occurs when the VPKSoft.VisualScrollBar.Value property is changed, either by a VPKSoft.VisualScrollBar.Scroll event or programmatically.
        /// </summary>
        [Description("Occurs when the VPKSoft.VisualScrollBar.Value property is changed, either by a VPKSoft.VisualScrollBar.Scroll event or programmatically")]
        [Category("Action")]
        public event OnValueChanged ValueChanged = null;
        #endregion

        #region ScrollBarProperties
        // A value to be added to or subtracted from the VPKSoft.VisualScrollBar.Value property when a button is clicked..
        private int _LargeChange = 10;

        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the VPKSoft.VisualScrollBar.Value property.
        /// </summary>
        [DefaultValue(10)]
        [Description("Gets or sets a value to be added to or subtracted from the VPKSoft.VisualScrollBar.Value property")]
        [Category("Behavior")]
        public int LargeChange
        {
            get
            {
                return _LargeChange; // return the value..
            }
            set
            {
                if (value < 1 || value >= Maximum || value > _ScrollerWH) // if the given change is "invalid", do complain via an exception..
                {
                    throw new ArgumentOutOfRangeException("LargeChange");
                }
                _LargeChange = value; // set the value..                
            }
        }

        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the VPKSoft.VisualScrollBar.Value property.
        /// <para/>This is a synonym for the LargeChange property as this is not a "traditional" scroll bar.
        /// </summary>
        [DefaultValue(10)]
        [Description("Gets or sets a value to be added to or subtracted from the VPKSoft.VisualScrollBar.Value property")]
        [Category("Behavior")]
        public int SmallChange
        {
            get
            {
                return LargeChange; // return the value..
            }
            set
            {
                LargeChange = value; // set the value..LargeChange setter can process a possible invalid value in it's own logic..
            }
        }

        /// <summary>
        /// Gets or sets the value of the scroll bar. The value is between Minimum and Maximum property values.
        /// <para/>This is an internal value property and it is responsible for raising the ValueChanged event if required.
        /// </summary>
        private int ScrollerPosition
        {
            set // Calculate the value based on the scroll box's position..
            {
                // this may look complex - at least for me..
                double tag = Convert.ToInt32(pnScrollBar.Tag); // first take the Tag in which the scroll box's position (X or Y) is saved..
                // take the maximum width or height depending if the scroll bar is horizontal or vertical..
                double wh = (_Horizontal ? pnScrollBar.Width - _ScrollerWH : pnScrollBar.Height - _ScrollerWH); 

                // construct a new value for the Value property..
                int newValue = (int)((double)(Maximum - Minimum) * (tag / wh));

                // prevent "this" to cause exceptions.. invalid control..
                if (newValue > Maximum)
                {
                    newValue = Maximum;
                }
                else if (newValue < Minimum) // .. again prevent "this" to cause exceptions.. invalid control
                {
                    newValue = Minimum;
                }

                // .. and raise ValueChanged event if subscribed and no denial to raise event was defined in the case that the value did change..
                if (newValue != _value && ValueChanged != null && !noEvent)
                {
                    ValueChanged(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, newValue));
                }

                _value = newValue; // save the new value to be accessible from the Value property..
            }

            get // Calculate a new value for the scroll box's position..
            {
                // the value depends on the state whether the scrollbar is horizontal or vertical..
                double wh = (double)(_Horizontal ? pnScrollBar.Width - _ScrollerWH : pnScrollBar.Height - _ScrollerWH);

                // do some multiplication based on the Value property between Minimum and Maximum properties..
                return (int)(wh                    
                     * ((double)_value / ((double)Maximum - (double)Minimum)));
            }
        }

        // The scroll bar's location indicator's width or height in pixels..
        private int _ScrollerWH = 20;

        /// <summary>
        /// Gets or sets the scroll bar's location indicator's width or height in pixels.
        /// </summary>
        [Description("Gets or sets the scroll bar's location indicator's width or height in pixels")]
        [DefaultValue(20)]
        [Category("Behavior")]
        public int ScrollerWH
        {
            get
            {
                return _ScrollerWH; // return the scroll box size (width or height)
            }

            set
            {
                if (_Horizontal) // Assign a new value the scroll box..
                {
                    if (value < pnScrollBar.Width) // don't accept values that are larger than the actual scroll bar..
                    {
                        _ScrollerWH = value;
                        pnScrollBar.Invalidate(); // Redraw/repaint..
                    }
                }
                else
                {
                    if (value < pnScrollBar.Height) // don't accept values that are larger than the actual scroll bar..
                    {
                        _ScrollerWH = value;
                        pnScrollBar.Invalidate(); // Redraw/repaint..
                    }
                }
            }
        }

        // A lower limit of values of the scrollable range..
        private int _Minimum = 0;

        /// <summary>
        /// Gets or sets the lower limit of values of the scrollable range.
        /// </summary>
        [Description("Gets or sets the lower limit of values of the scrollable range")]
        [DefaultValue(0)]
        [Category("Behavior")]
        public int Minimum
        {
            get
            {
                return _Minimum; // return the minimum value..
            }

            set
            {
                if (value == Maximum) // don't accept same values..
                {
                    return;
                }

                if (value < 0 || value >= Maximum || value > _ScrollerWH) // if the given change is "invalid", do complain via an exception..
                {
                    throw new ArgumentOutOfRangeException("Minimum");
                }
                _Minimum = value; // no invalid's, so set the value..
            }
        }

        // An upper limit of values of the scrollable range..
        private int _Maximum = 100;

        /// <summary>
        /// Gets or sets the upper limit of values of the scrollable range.
        /// </summary>
        [Description("Gets or sets the upper limit of values of the scrollable range")]
        [DefaultValue(100)]
        [Category("Behavior")]
        public int Maximum
        {
            get
            {
                return _Maximum; // return the maximum value..
            }

            set
            {
                if (value == Minimum) // don't accept same values..
                {
                    return;
                }

                if (value < 1 || value <= Minimum || value < _ScrollerWH) // if the given change is "invalid", do complain via an exception..
                {
                    throw new ArgumentOutOfRangeException("Maximum");
                }
                _Maximum = value;
            }
        }

        // indicates if no event should be raised when the Value property is changed..
        private bool noEvent = false;

        // A numeric value that represents the current position of the scroll box on the scroll bar control..
        private int _value = 0;

        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the scroll box on the scroll bar control.
        /// </summary>
        [Description("Gets or sets a numeric value that represents the current position of the scroll box on the scroll bar control")]
        [DefaultValue(0)]
        [Category("Behavior")]
        public int Value
        {
            get
            {
                return _value; // return the value (Value)..
            }

            set
            {
                if (value < Minimum || value > Maximum) // if the given change is "invalid", do complain via an exception..
                {
                    noEvent = false;
                    throw new ArgumentOutOfRangeException("Value");
                }

                _value = value; // set the value..

                pnScrollBar.Tag = ScrollerPosition; // calculate a visual value for the scroll box..
                pnScrollBar.Invalidate(); // Redraw/repaint..

                // if the ValueChanged event was subscribed and no denial to raise event was defined, then raise the event..
                if (ValueChanged != null && !noEvent)
                {
                    ValueChanged(this, new ScrollEventArgs(ScrollEventType.ThumbPosition, _value));
                }
            }
        }

        /// <summary>
        /// Does the setting of the Minimum, Maximum and Value properties in a "safe" way. Only throws an exception if the given minimum value is larger than the maximum value. No events will be raised.
        /// </summary>
        /// <param name="minimum">A value for the Maximum property.</param>
        /// <param name="maximum">A value for the Minimum property.</param>
        /// <param name="value">A value for the Value property</param>
        public void SetMinMaxValueSafe(int minimum, int maximum, int value)
        {
            if (minimum > maximum) // this is the only exception case so throw an exception..
            {
                throw new ArgumentOutOfRangeException("Minimum");
            }

            if (value > maximum) // adjust the value so it doesn't exceed the maximum value
            {
                value = maximum;
            }

            _Minimum = minimum; // assign the value for the Minimum property..
            _Maximum = maximum; // assign the value for the Maximum property..
            _value = value; // assign the value for the Value property..

            noEvent = true; // indicate that this change will no raise an event..
            pnScrollBar.Tag = ScrollerPosition; // calculate a visual value for the scroll box..

            // Redraw/repaint.. a call to Refresh method does the repaint immediately, so the noEvent has no change to change..
            pnScrollBar.Refresh();

            noEvent = false; // indicate that this events can be raised again..
        }


        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the scroll box on the scroll bar control without raising an event.
        /// </summary>
        [Browsable(false)]
        public int ValueNoEvent
        {
            get
            {
                return Value; // no special handling is required so just return the Value property.
            }

            set
            {
                noEvent = true; // indicate that no event should be raised..
                Value = value;
                noEvent = false; // indicate that events can be raised again..
            }
        }

        // if true the scroll bar is horizontal, otherwise it is vertical..
        private bool _Horizontal = false;

        /// <summary>
        /// Gets or sets the value indicating if the scroll bar is vertical or not.
        /// </summary>
        [Description("Gets or sets the value indicating if the scroll bar is vertical or not")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Horizontal
        {
            get
            {
                return _Horizontal; // the getter is simple.. return the value!
            }

            set // a complete remake is required if the Horizontal property is changed..
            {
                _Horizontal = value;

                if (value) // horizontal layout..
                {
                    tlpMain.Controls.Clear(); // clear the controls.. they are referenced in the *.Designer.cs..
                    tlpMain.ColumnCount = 3; // set the amount of rows and columns..
                    tlpMain.RowCount = 1;

                    tlpMain.RowStyles.Clear(); // clear the possibly previously set column and row styles..
                    tlpMain.ColumnStyles.Clear();

                    // .. create new column and row styles..
                    tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
                    tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20.0f));
                    tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
                    tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20.0f));

                    // re-add the controls back..
                    tlpMain.Controls.Add(pnScrollUp, 0, 0);
                    tlpMain.Controls.Add(pnScrollBar, 1, 0);
                    tlpMain.Controls.Add(pnScrollDown, 2, 0);
                    pnScrollUp.Parent = tlpMain;
                    pnScrollBar.Parent = tlpMain;
                    pnScrollDown.Parent = tlpMain;

                    // dock the controls..
                    pnScrollUp.Dock = DockStyle.Fill;
                    pnScrollBar.Dock = DockStyle.Fill;
                    pnScrollDown.Dock = DockStyle.Fill;
                }
                else // vertical layout..
                {
                    tlpMain.Controls.Clear(); // clear the controls.. they are referenced in the *.Designer.cs..
                    tlpMain.ColumnCount = 1; // set the amount of rows and columns..
                    tlpMain.RowCount = 3;

                    tlpMain.RowStyles.Clear(); // clear the possibly previously set column and row styles..
                    tlpMain.ColumnStyles.Clear();

                    // .. create new column and row styles..
                    tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20.0f));
                    tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
                    tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20.0f));
                    tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));

                    // re-add the controls back..
                    tlpMain.Controls.Add(pnScrollUp, 0, 0);
                    tlpMain.Controls.Add(pnScrollBar, 0, 1);
                    tlpMain.Controls.Add(pnScrollDown, 0, 2);
                    pnScrollUp.Parent = tlpMain;
                    pnScrollBar.Parent = tlpMain;
                    pnScrollDown.Parent = tlpMain;

                    // dock the controls..
                    pnScrollUp.Dock = DockStyle.Fill;
                    pnScrollBar.Dock = DockStyle.Fill;
                    pnScrollDown.Dock = DockStyle.Fill;
                }

                SetImages(); // set the images depending if the scroll bar was set to horizontal or to vertical value..
                Value = Minimum; // reset the scroll bar's slider's (or scroll box's) position
            }
        }
        #endregion

        #region ColorProperties
        // The background color of the control..
        private Color _BackColor = Color.SteelBlue;
        private Brush _BackgroundBrush = new SolidBrush(Color.SteelBlue);

        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        [Description("Gets or sets the background color of the control")]
        [Category("VisualScrollBar")]
        [DefaultValue(typeof(Color), "SteelBlue")]
        public override Color BackColor
        {
            get
            {
                return _BackColor; // return the current background color..
            }

            set
            {
                base.BackColor = value; // just in case, set the value to all the child controls..
                tlpMain.BackColor = value;
                pnScrollBar.BackColor = value;
                _BackgroundBrush = new SolidBrush(Color.SteelBlue); // Generate the brushes so no regeneration is required when the actual painting starts..
                pnScrollBar.Invalidate(); // Redraw/repaint..
            }
        }

        private Color _ButtonBackColor = SystemColors.Control;

        /// <summary>
        /// Gets or sets the background color of the control.
        /// </summary>
        [Description("Gets or sets the background color of the buttons on the control")]
        [Category("VisualScrollBar")]
        [DefaultValue(typeof(Color), "Control")]
        public Color ButtonBackColor
        {
            get
            {
                return _ButtonBackColor; // return the current background color..
            }

            set
            {
                _ButtonBackColor = value;
                pnScrollDown.BackColor = value; // set the buttons background colors..
                pnScrollUp.BackColor = value;
            }
        }

        // The scroll bar's slider foreground color..
        private Color _SliderForeColor = Color.LightSteelBlue;
        private Brush _SliderBrush = new SolidBrush(Color.LightSteelBlue);        

        /// <summary>
        /// The color of a scroll bar's slider.
        /// </summary>
        [Description("The color of a scroll bar's slider")]
        [Category("VisualScrollBar")]
        [DefaultValue(typeof(Color), "LightSteelBlue")]
        public Color SliderForeColor
        {
            get
            {
                return _SliderForeColor; // return the current slider background color.. (which is named as *ForeColor ;-) )
            }

            set
            {
                _SliderForeColor = value; // Set the value..
                _SliderBrush = new SolidBrush(value); // Generate the brushes so no regeneration is required when the actual painting starts..
                pnScrollBar.Invalidate(); // Redraw/repaint.. // Redraw/repaint..
            }
        }
        #endregion

        #region ImageProperties
        // An image of the scroll up button..
        private Image _ScrollUpImage = VisualComponents.Properties.Resources.scroll_up;

        /// <summary>
        /// An image of the scroll up button.
        /// </summary>
        [Description("An image of the scroll up button")]
        [Category("ArrowImages")]
        public Image ScrollUpImage
        {
            get
            {
                return _ScrollUpImage; // return the current image..
            }

            set
            {
                _ScrollUpImage = value; // set the current image..
                SetImages(); // assign the images to their "buttons" depending on the Horizontal property value..
            }
        }

        // An image of the scroll down button..
        private Image _ScrollDownImage = VisualComponents.Properties.Resources.scroll_down;

        /// <summary>
        /// An image of the scroll down button.
        /// </summary>
        [Description("An image of the scroll down button")]
        [Category("ArrowImages")]
        public Image ScrollDownImage
        {
            get
            {
                return _ScrollDownImage; // return the current image..
            }

            set
            {
                _ScrollDownImage = value; // set the current image..
                SetImages(); // assign the images to their "buttons" depending on the Horizontal property value..
            }
        }

        // An image of the scroll left button..
        private Image _ScrollLeftImage = VisualComponents.Properties.Resources.scroll_left;

        /// <summary>
        /// An image of the scroll left button.
        /// </summary>
        [Description("An image of the scroll left button")]
        [Category("ArrowImages")]
        public Image ScrollLeftImage
        {
            get
            {
                return _ScrollLeftImage; // return the current image..
            }

            set
            {
                _ScrollLeftImage = value; // set the current image..
                SetImages(); // assign the images to their "buttons" depending on the Horizontal property value..
            }
        }

        // An image of the scroll right button..
        private Image _ScrollRightImage = VisualComponents.Properties.Resources.scroll_right;

        /// <summary>
        /// An image of the scroll right button.
        /// </summary>
        [Description("An image of the scroll right button")]
        [Category("ArrowImages")]
        public Image ScrollRightImage
        {
            get
            {
                return _ScrollRightImage; // return the current image..
            }

            set
            {
                _ScrollRightImage = value; // set the current image..
                SetImages(); // assign the images to their "buttons" depending on the Horizontal property value..
            }
        }
        #endregion

        #region ImageLayout
        /// <summary>
        ///  Assigns the images to their "buttons" depending on the Horizontal property value.
        /// </summary>
        private void SetImages()
        {
            if (Horizontal) // if horizontal..
            {
                pnScrollUp.BackgroundImage = _ScrollLeftImage; // ..the scrolling happens to the left..
                pnScrollDown.BackgroundImage = _ScrollRightImage; // ..and to the right.. (SEE: https://en.wikipedia.org/wiki/The_Rocky_Horror_Picture_Show)
                int picSize = Math.Max(pnScrollUp.Width, pnScrollUp.Height); // .. the image is expected to be a square --> width == height.. 
                tlpMain.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, picSize); // assign new column styles..
                tlpMain.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, picSize); //..nothing to say anymore..
            }
            else // if vertical..
            {
                pnScrollUp.BackgroundImage = _ScrollUpImage; // ..the scrolling happens to the up..
                pnScrollDown.BackgroundImage = _ScrollDownImage; // ..and to the down.. (SEE: https://en.wikipedia.org/wiki/The_Rocky_Horror_Picture_Show)
                int picSize = Math.Max(pnScrollUp.Width, pnScrollUp.Height); // .. the image is expected to be a square --> width == height.. 
                tlpMain.RowStyles[0] = new RowStyle(SizeType.Absolute, picSize); // assign new column styles..
                tlpMain.RowStyles[2] = new RowStyle(SizeType.Absolute, picSize); //..nothing to say anymore..
            }
            pnScrollBar.Invalidate(); // Redraw/repaint..
        }

        // When the control is resized the arrow images need some resizing too and possible layout changes are required..
        private void VisualScrollBar_Resize(object sender, EventArgs e)
        {
            Horizontal = _Horizontal; // .. so re-layout the control - with this cunning trick..
        }
        #endregion
    }
}
