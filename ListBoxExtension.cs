#region license
/*
Public domain. Free to be used in any purpose.
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the ListBoxExtension control.
/// </summary>
namespace VPKSoft.VisualListBox
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// An extended list box control with alternating colors and a possibility to report it's scroll position.
    /// </summary>
    [DefaultEvent("VScrollChanged")]
    public partial class ListBoxExtension : ListBox
    {
        /// <summary>
        /// A constructor for the ListBoxExtension class.
        /// </summary>
        public ListBoxExtension() : base()
        {
            InitializeComponent();
            DoubleBuffered = true;
            DrawMode = DrawMode.OwnerDrawFixed; // the drawing of items is custom..

            DrawItem += ListBoxExtension_DrawItem; // subscribe to events..
            FontChanged += ListBoxExtension_FontChanged;
            MouseClick += ListBoxExtension_MouseClick;
            MouseMove += ListBoxExtension_MouseMove;
            PreviewKeyDown += ListBoxExtension_PreviewKeyDown;
            Disposed += ListBoxExtension_Disposed; // END: subscribe to events..

            // scroll bar is wanted as it's not wanted to make the drawing logic more difficult..
            ScrollAlwaysVisible = true;
        }

        #region InternalLogic
        /// <summary>
        /// A constant for a scroll message (WM_VSCROLL) for the WndProc override.
        /// </summary>
        internal const int WM_VSCROLL = 0x0115;

        /// <summary>
        /// A constant for a mouse wheel message (WM_MOUSEWHEEL) for the WndProc override.
        /// </summary>
        internal const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// A constant for a key down message (WM_KEYDOWN) for the WndProc override.
        /// </summary>
        internal const int WM_KEYDOWN = 0x0100;

        /// <summary>
        /// A string to measure font sizes.
        /// </summary>
        public const string MeasureText = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö£€%[]$@ÂÊÎÔÛâêîôûÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÁÉÍÓÚáéíóúÃÕãõ '|?+\\/{}½§01234567890+<>_-:;*&¤#\"!";

        /// <summary>
        /// The WndProc is overridden so some messages can be used to raise the VScrollChanged event.
        /// </summary>
        /// <param name="m">A reference to a Message class instance.</param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m); // first call the base WndProc..

            // .. and if the message indicates that the TopIndex property of the list box might have changed, raise the VScrollChanged event..
            if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL || m.Msg == WM_KEYDOWN)
            {
                // ..if not denied and the TopIndex property was actually changed..
                if (!noEvent && lastTopIndex != this.TopIndex)
                {
                    lastTopIndex = TopIndex; // save the top index so it's changes can be monitored..

                    // if the VScrollChanged event is subscribed the raise the event with (FromControl = true)..
                    VScrollChanged?.Invoke(this, new VScrollChangedEventArgs()
                    { Minimum = 0, Maximum = VScrollMaximum, Value = VScrollPosition, FromControl = true });
                }
            }
        }

        // watch the return key to raise ItemClicked event if subscribed..
        private void ListBoxExtension_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (SelectedIndex != -1 && e.KeyCode == Keys.Return)
            {
                // raise an event for the item click if subscribed..
                ItemClicked?.Invoke(this, new ListBoxButtonClickEventArgs() { ItemIndex = SelectedIndex, Item = Items[SelectedIndex] });
            }
        }

        // unsubscribe the code-subscribed events on disposing..
        private void ListBoxExtension_Disposed(object sender, EventArgs e)
        {
            DrawItem -= ListBoxExtension_DrawItem; // unsubscribe from events..
            FontChanged -= ListBoxExtension_FontChanged;
            MouseClick -= ListBoxExtension_MouseClick;
            MouseMove -= ListBoxExtension_MouseMove;
            PreviewKeyDown -= ListBoxExtension_PreviewKeyDown;
            Disposed -= ListBoxExtension_Disposed; // END: unsubscribe from events..
        }

        // handling if the "button" to the right of an item is clicked..
        private void ListBoxExtension_MouseClick(object sender, MouseEventArgs e)
        {
            // if an image is set, assume it will be wanted to be visible as well..
            if (_RightImage != null && _RightImageScaled != null)
            {
                // get the point where the mouse was clicked..
                Point point = new Point(e.X, e.Y);
                int clickIndex = IndexFromPoint(point); // ..and get an item's index on the point..
                if (clickIndex != -1) // if an index was found..
                {
                    Point p2 = new Point(e.X, 0); // check if the click hit the "button"..
                    if (RightButtonRectangle.Contains(p2))
                    {
                        // raise an event for the "button" click if subscribed..
                        ButtonClicked?.Invoke(this, new ListBoxButtonClickEventArgs() { ItemIndex = clickIndex, Item = Items[clickIndex] });
                    }
                    else
                    {
                        // raise an event for the item click if subscribed..
                        ItemClicked?.Invoke(this, new ListBoxButtonClickEventArgs() { ItemIndex = clickIndex, Item = Items[clickIndex] });
                    }
                }
            }
        }

        // save an index if mouse hovers over an item..
        private int _mouseHoverIndex = -1; // initialize with a "no index" value.. 
        private int _mouseHoverIndexPrevious = -1; // initialize with a "no index" value.. 
        private bool _mouseHoverIndexChanged = false; // an indicator if the mouse has moved to a different item..

        // handling if the mouse hovering over an item..
        private void ListBoxExtension_MouseMove(object sender, MouseEventArgs e)
        {
            // get the point where the mouse hovers..
            Point point = new Point(e.X, e.Y);
            int clickIndex = IndexFromPoint(point); // ..and get an item's index on the point..

            // save the hover index change indicator flag..
            _mouseHoverIndexChanged = _mouseHoverIndex == clickIndex ? false : true;

            _mouseHoverIndexPrevious = _mouseHoverIndex; // save the previous index value..
            _mouseHoverIndex = clickIndex; // save the hover index value..

            // invalidate the index the mouse is hovering on top of only if there is something to invalidate..
            if (_mouseHoverIndexChanged && clickIndex != -1)
            {
                Invalidate(GetItemRectangle(clickIndex));
            }

            // invalidate the previous mouse hover index only if there is something to invalidate
            if (_mouseHoverIndexPrevious != -1 && _mouseHoverIndexPrevious != _mouseHoverIndex && _mouseHoverIndexPrevious < Items.Count)
            {
                Invalidate(GetItemRectangle(_mouseHoverIndexPrevious));
            }
        }
        #endregion

        #region PublicEvents
        /// <summary>
        /// A delegate for the VScrollChanged event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">A VScrollChangedEventArgs class instance to indicate the scroll position.</param>
        public delegate void OnVScrollChanged(object sender, VScrollChangedEventArgs e);

        /// <summary>
        /// A delegate for the ButtonClicked event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">A ListBoxButtonClickEventArgs class instance to indicate the item's index and the item the button was clicked upon.</param>
        public delegate void OnButtonClicked(object sender, ListBoxButtonClickEventArgs e);

        /// <summary>
        /// An event that is raised when a user clicks the button of an item in the list box.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event OnButtonClicked ButtonClicked = null;

        /// <summary>
        /// An event that is raised when a user clicks an item in the list box with mouse or presses keyboard return button.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event OnButtonClicked ItemClicked = null;

        /// <summary>
        /// An event that is raised if the vertical scroll position was changed either by code or by the control.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event OnVScrollChanged VScrollChanged = null;
        #endregion

        #region Drawing
        /// <summary>
        /// Calculate a new height for an item in the list box if the Font property was changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">An EventArgs class instance.</param>
        private void ListBoxExtension_FontChanged(object sender, EventArgs e)
        {
            // measure a new height for an item with a long text constant..
            _ItemHeight = System.Windows.Forms.TextRenderer.MeasureText(MeasureText, Font).Height;
        }


        // a measured height for an item in the list box..
        internal int _ItemHeight = 0;

        /// <summary>
        /// Gets or sets the height of an item in the VPKSoft.VisualListBox.ListBoxExtension.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)] // this can't be modified by the user directly so hide it..
        [Browsable(false)] //.. and hide it from the designer too
        public override int ItemHeight
        {
            get
            {
                if (_ItemHeight == 0) // if the height is not already calculated, do calculate it..
                {
                    // measure a new height for an item with a long text constant..
                    _ItemHeight = System.Windows.Forms.TextRenderer.MeasureText(MeasureText, Font).Height;
                }

                // return the measured item height with the additional TextOffSet property's value multiplied by two..
                return _ItemHeight + TextOffset * 2;
            }

            set // the user can't interact with this value directly.. the interaction is done by changing the Font or TextOffset property value
            {
                // a cheat :-)
            }
        }

        // the left upper point where the right side "button" image was drawn if it is enabled..
        private int rightButtonX = int.MinValue; // initialize with a ridiculous value..

        /// <summary>
        /// Gets the right button rectangle.
        /// </summary>
        [Browsable(false)]
        public Rectangle RightButtonRectangle
        {
            get
            {
                if (_RightImage != null && _RightImageScaled != null)
                {
                    return new Rectangle(rightButtonX, 0, _RightImageScaled.Width, _RightImageScaled.Height);
                }
                else
                {
                    return new Rectangle();
                }                        
            }
        }

        /// <summary>
        /// This is a fixed owner drawn ListBox derivate.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">A DrawItemEventArgs class instance.</param>
        private void ListBoxExtension_DrawItem(object sender, DrawItemEventArgs e)
        {
            // draw nothing if there is nothing to draw..
            if (e.Index < 0 || e.Index >= Items.Count)
            {
                return; // .. so leave..
            }

            // if the mouse hover index has changed paint with different color..
            if (_mouseHoverIndex == e.Index && _mouseHoverIndexChanged)
            {
                e.Graphics.FillRectangle((e.Index % 2) == 0 ?
                    new SolidBrush(HoverBackColor) :
                    new SolidBrush(HoverBackColorAlternative),
                    e.Bounds);

            }
            else // ..otherwise the normal paint procedure..
            {
                // if an item is not selected or focused, fill a rectangle with a color indicating background..
                if ((!e.State.HasFlag(DrawItemState.Selected) && !e.State.HasFlag(DrawItemState.Focus)) || !_AllowItemSelection)
                {
                    // this ListBox derivate has alternating colors - so the (e.Index % 2) == 2 evaluation..
                    e.Graphics.FillRectangle((e.Index % 2) == 0 ?
                        new SolidBrush(BackColor) :
                        new SolidBrush(BackColorAlternative),
                        e.Bounds);
                }
                // if an item is selected or focused, fill a rectangle with a color indicating selection..
                else if (e.State.HasFlag(DrawItemState.Selected) || e.State.HasFlag(DrawItemState.Focus))
                {
                    // this ListBox derivate has alternating colors - so the (e.Index % 2) == 2 evaluation..
                    e.Graphics.FillRectangle((e.Index % 2) == 0 ?
                        new SolidBrush(SelectionColor) :
                        new SolidBrush(SelectionColorAlternative),
                        e.Bounds);
                }
            }

            // there is an item with an index at this point of execution of the method, so save the item's text so it can be drawn..            
            string itemText = GetItemText(Items[e.Index]);

            // construct a rectangle for the item's text based on the TextOffset property value..
            Rectangle rectangle = new Rectangle(e.Bounds.X + TextOffset, e.Bounds.Y + TextOffset, e.Bounds.Width - TextOffset * 2, e.Bounds.Height - TextOffset * 2);

            // if the mouse hover index has changed paint with different color..
            if (_mouseHoverIndex == e.Index && _mouseHoverIndexChanged)
            {
                // draw the item's text..this ListBox derivate has alternating colors - so the (e.Index % 2) == 2 evaluation..
                e.Graphics.DrawString(itemText, Font, (e.Index % 2) == 0 ?
                new SolidBrush(HoverForeColor) :
                new SolidBrush(HoverForeColorAlternative),
                rectangle);
            }
            else // ..otherwise the normal paint procedure..
            {
                // draw the item's text..this ListBox derivate has alternating colors - so the (e.Index % 2) == 2 evaluation..
                e.Graphics.DrawString(itemText, Font, (e.Index % 2) == 0 ?
                new SolidBrush(ForeColor) :
                new SolidBrush(ForeColorAlternative),
                rectangle);
            }

            if (_RightImage != null) // if an image is assigned then draw it to right corner of an item..
            {
                if (_RightImageScaled == null) // if the image hasn't been scaled to proper size then scale it..
                {
                    // .. scale the image..
                    _RightImageScaled = ScaleImageToFit(_RightImage, e.Bounds.Height);
                }

                // calculate the button X-coordinate..
                rightButtonX = e.Bounds.X + e.Bounds.Width - _RightImageScaled.Width;
           
                // calculate a rectangle for the image background so the item's text doesn't seem to go behind the button,
                // if the image is transparent..
                Rectangle imageBackRect = new Rectangle(rightButtonX - 4 < 0 ? 0 : rightButtonX - 4, e.Bounds.Y, _RightImageScaled.Width, _RightImageScaled.Height);

                // fill the image background area..
                if (_mouseHoverIndex == e.Index && _mouseHoverIndexChanged) // if the mouse hover index has changed paint with different color..

                {
                    e.Graphics.FillRectangle((e.Index % 2) == 0 ?
                        new SolidBrush(HoverBackColor) :
                        new SolidBrush(HoverBackColorAlternative),
                        imageBackRect);
                }
                else // ..otherwise the normal paint procedure..
                {
                    // if an item is not selected or focused, fill a rectangle with a color indicating background..
                    if ((!e.State.HasFlag(DrawItemState.Selected) && !e.State.HasFlag(DrawItemState.Focus)) || !_AllowItemSelection)
                    {
                        // this ListBox derivate has alternating colors - so the (e.Index % 2) == 2 evaluation..
                        e.Graphics.FillRectangle((e.Index % 2) == 0 ?
                            new SolidBrush(BackColor) :
                            new SolidBrush(BackColorAlternative),
                            imageBackRect);
                    }
                    // if an item is selected or focused, fill a rectangle with a color indicating selection..
                    else if (e.State.HasFlag(DrawItemState.Selected) || e.State.HasFlag(DrawItemState.Focus))
                    {
                        // this ListBox derivate has alternating colors - so the (e.Index % 2) == 2 evaluation..
                        e.Graphics.FillRectangle((e.Index % 2) == 0 ?
                            new SolidBrush(SelectionColor) :
                            new SolidBrush(SelectionColorAlternative),
                            imageBackRect);
                    }
                }
                // finally draw the image (button)..
                e.Graphics.DrawImage(_RightImageScaled, new Point(rightButtonX, e.Bounds.Y));
            }
        }

        private Image ScaleImageToFit(Image image, int maxHeight)
        {
            Size adjustSize = image.Size;

            double multiplier = (double)maxHeight / adjustSize.Height;

            adjustSize = new Size((int)(adjustSize.Width * multiplier), (int)(adjustSize.Height * multiplier));

            if (adjustSize.Width <= 0)
            {
                adjustSize.Width = 1;
            }

            if (adjustSize.Height <= 0)
            {
                adjustSize.Height = 1;
            }

            Bitmap retval = new Bitmap(adjustSize.Width, adjustSize.Height);

            using (Graphics g = Graphics.FromImage(retval))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, adjustSize.Width, adjustSize.Height);
            }
            return retval;
        }

        /// <summary>
        /// Causes an item with a given index to refresh.
        /// </summary>
        /// <param name="index">An index for an item to refresh.</param>
        public new void RefreshItem(int index)
        {
            int topIndex = TopIndex; // save the current TopIndex property value before the refresh..
            base.RefreshItem(index); // refresh the item..
            TopIndex = topIndex; // restore the TopIndex property value..
        }

        /// <summary>
        /// Just hide the DrawMode from the designer.
        /// </summary>
        [Browsable(false)] // hide from the designer..
        public override DrawMode DrawMode { get => base.DrawMode; set => base.DrawMode = value; }

        /// <summary>
        /// Refreshes all the items in the list box.
        /// </summary>
        public new void RefreshItems()
        {
            int topIndex = TopIndex; // save the current TopIndex property value before the refresh..
            base.RefreshItems(); // refresh all the items..
            TopIndex = topIndex; // restore the TopIndex property value..
        }
        #endregion

        #region AppearanceProperties
        private Image _RightImage = null; // a place holder for an unscaled button image..
        private Image _RightImageScaled = null; // a place holder for a scaled button image..

        /// <summary>
        /// An image to display at the right of the item's text.
        /// </summary>
        [Category("Appearance")]
        [Description("An image to display at the right of the item's text.")]
        [DefaultValue(null)]
        [Browsable(true)]
        public Image RightImage
        {
            get
            {
                return _RightImage;
            }

            set
            {
                _RightImage = value;
                _RightImageScaled = null;
                Refresh();
            }
        }

        // a margin around an item's text..
        internal int _TextOffSet = 6;

        /// <summary>
        /// Gets or sets the margin around an item's text.
        /// </summary>
        [Category("Appearance")]
        [Description("The margin around an item's text.")]
        [DefaultValue(6)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int TextOffset
        {
            get
            {
                return _TextOffSet; // return the value..
            }

            set
            {
                if (value >= 0) // only accept positive values..
                {
                    _TextOffSet = value;
                }
                else // .. otherwise complain via an exception..
                {
                    throw new ArgumentOutOfRangeException("TextOffset");
                }
            }
        }

        // an alternative foreground color..
        internal Color _ForeColorAlternative = SystemColors.ControlText;

        /// <summary>
        /// Gets or sets the alternative foreground color of the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The alternative foreground color of this component, which is used to display text.")]
        [DefaultValue(typeof(Color), "ControlText")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color ForeColorAlternative
        {
            get
            {                
                return _ForeColorAlternative;
            }

            set
            {
                _ForeColorAlternative = value;
            }
        }

        // an alternative background color..
        internal Color _BackColorAlternative = SystemColors.Control;

        /// <summary>
        /// Gets or sets the alternative background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The alternative background color for the component.")]
        [DefaultValue(typeof(Color), "Control")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color BackColorAlternative
        {
            get
            {
                return _BackColorAlternative;
            }

            set
            {
                _BackColorAlternative = value;
            }
        }

        // a selection color..
        internal Color _SelectionColor = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets the selection background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The background color for a selected item for the component.")]
        [DefaultValue(typeof(Color), "Highlight")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color SelectionColor
        {
            get
            {
                return _SelectionColor;
            }

            set
            {
                _SelectionColor = value;
            }
        }

        // an alternative selection color..
        internal Color _AlternativeSelectionColor = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets the selection background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The alternative background color for a selected item for the component.")]
        [DefaultValue(typeof(Color), "Highlight")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color SelectionColorAlternative
        {
            get
            {
                return _AlternativeSelectionColor;
            }

            set
            {
                _AlternativeSelectionColor = value;
            }
        }

        // a mouse hover background color..
        internal Color _HoverBackColor = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets the item mouse hover background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover background color for an item for the component.")]
        [DefaultValue(typeof(Color), "Highlight")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverBackColor
        {
            get
            {
                return _HoverBackColor;
            }

            set
            {
                _HoverBackColor = value;
            }
        }

        // a mouse hover alternative background color..
        internal Color _HoverBackColorAlternative = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets the item mouse hover alternative background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover alternative background color for an item for the component.")]
        [DefaultValue(typeof(Color), "Highlight")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverBackColorAlternative
        {
            get
            {
                return _HoverBackColorAlternative;
            }

            set
            {
                _HoverBackColorAlternative = value;
            }
        }

        // a mouse hover foreground color..
        internal Color _HoverForeColor= SystemColors.ControlText;

        /// <summary>
        /// Gets or sets the item mouse hover foreground color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover alternative foreground color for an item for the component.")]
        [DefaultValue(typeof(Color), "ControlText")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverForeColor
        {
            get
            {
                return _HoverForeColor;
            }

            set
            {
                _HoverForeColor = value;
            }
        }


        // a mouse hover alternative foreground color..
        internal Color _HoverForeColorAlternative = SystemColors.ControlText;

        /// <summary>
        /// Gets or sets the item mouse hover alternative foreground color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover alternative foreground color for an item for the component.")]
        [DefaultValue(typeof(Color), "ControlText")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverForeColorAlternative
        {
            get
            {
                return _HoverForeColorAlternative;
            }

            set
            {
                _HoverForeColorAlternative = value;
            }
        }

        private bool _AllowItemSelection = true;

        /// <summary>
        /// Gets or set a value if item selection is allowed.
        /// </summary>
        [Category("Behavior")]
        [Description("Indicates if a user can select an item from the list box.")]
        [DefaultValue(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool AllowItemSelection
        {
            get
            {
                return _AllowItemSelection;
            }

            set
            {
                if (value != _AllowItemSelection) // only if the value changed..
                {
                    if (!value) // if item selection was set to disabled, set the selected index to zero..
                    {
                        Invalidate();
                    }
                }
                _AllowItemSelection = value; // save the value..
            }
        }
        #endregion

        #region Indices and Values
        /// <summary>
        /// Sets the selected item index.
        /// </summary>
        /// <param name="index">An index for a selected item to set.</param>
        public void SetIndex(int index)
        {
            try // better safe than sorry..
            {
                // clear the selection if required..
                if (SelectedIndices.Count > 1 && SelectedIndex != index)
                {
                    ClearSelected();
                }
                // set the selected index..
                SelectedIndex = index;
            }
            catch // .. if sorry..
            {

            }
        }

        // a list for saving a selection..
        private List<int> pushedSelection = new List<int>();

        /// <summary>
        /// Saves the indices of currently selected items.
        /// </summary>
        /// <param name="clearSelection">Whether to clear the selection after saving or not.</param>
        public void PushSelection(bool clearSelection = false)
        {
            pushedSelection.Clear(); // clear the saved selection indices..

            // save the selected indices..
            for (int i = 0; i < SelectedIndices.Count; i++)
            {
                pushedSelection.Add(SelectedIndices[i]);
            }

            if (clearSelection) // if a request was made to clear the selection..
            {
                ClearSelected(); // .. then just clear it
            }
        }

        /// <summary>
        /// Restores a saved selection.
        /// </summary>
        public void PopSelection()
        {
            for (int i = 0; i < Items.Count; i++) // loop through the items..
            {
                // if the saved selection contains the item's index then select it..
                SetSelected(i, pushedSelection.IndexOf(i) != -1);
            }
        }

        /// <summary>
        /// Gets the vertical scroll bar minimum value.
        /// </summary>
        [Browsable(false)] // hide from the designer..
        public int VScrollMinimum
        {
            get
            {
                return 0; // which is always zero..
            }
        }

        /// <summary>
        /// Gets the vertical scroll bar maximum value.
        /// </summary>
        [Browsable(false)] // hide from the designer..
        public int VScrollMaximum
        {
            get
            {
                // if no items then 0, otherwise Items.Count - 1
                return Items.Count > 0 ? Items.Count - 1 : 0;
            }
        }

        // indicates if the next VScrollPosition value change should raise an event (VScrollChanged)..
        internal bool noEvent = false;

        /// <summary>
        /// Sets or sets the VScrollPosition property value without raising the VScrollChanged event.
        /// </summary>
        [Browsable(false)] // hide from the designer..
        public int VScrollPositionNoEvent
        {
            get
            {
                return VScrollPosition; // return the value..
            }

            set
            {
                noEvent = true; // indicate that no VScrollChanged event is raised..
                VScrollPosition = value; // set the VScrollPosition property value without raising the VScrollChanged..
                noEvent = false; // indicate that no VScrollChanged event can be raised again..
            }
        }

        /// <summary>
        /// Gets or sets the VScrollPosition property value.
        /// </summary>
        [Browsable(false)] // hide from the designer..
        public int VScrollPosition
        {
            get
            {
                if (Items.Count == 0) // do nothing if there are no items..
                {
                    return 0; // .. except return zero..
                }
                else // return the TopIndex (VScrollPosition)
                {
                    return TopIndex;
                }
            }

            set
            {
                if (value < 0) // do not handle stupid values..
                {
                    return;
                }

                if (Items.Count == 0) // do nothing if there are no items..
                {
                    TopIndex = 0; // .. except set the value to zero..
                }
                else if (value >= 0 && value < Items.Count) // set the TopIndex (VScrollPosition), if the value is valid..
                {
                    TopIndex = value;
                }
                else // ..otherwise complain via an exception..
                {
                    throw new ArgumentOutOfRangeException("VScrollPosition");
                }
            }
        }

        // a latest saved TopIndex property value to avoid unnecessary event raising. Initialize with a ridiculous value..
        internal int lastTopIndex = int.MinValue;

        /// <summary>
        /// Gets or set the value of the TopIndex property.
        /// </summary>
        internal new int TopIndex // declare as new for additional handling..
        {
            get
            {
                return base.TopIndex; // just return the base value..
            }

            set
            {
                if (lastTopIndex == value) // if there is nothing to change then return..
                {
                    return;
                }
                base.TopIndex = value; // set the value..

                if (lastTopIndex != base.TopIndex) // if the value changed then save the changed value..
                {
                    lastTopIndex = base.TopIndex;
                }
                else // if there is nothing to change then return..
                {
                    return;
                }

                // ..if not denied and the TopIndex property was actually changed..
                if (!noEvent && base.TopIndex != value)
                {
                    // if the VScrollChanged event is subscribed the raise the event with (FromControl = false)..
                    VScrollChanged?.Invoke(this, new VScrollChangedEventArgs() { Value = VScrollPosition, Minimum = 0, Maximum = VScrollMaximum, FromControl = false });
                }
            }
        }
        #endregion

        #region OtherProperties
        private bool _HoverEnabled = true; // indicates if the mouse hovering is enabled..

        /// <summary>
        /// Gets or sets a value indicating whether the mouse hover tracking for items in the list box is enabled.
        /// </summary>
        [Category("Behavior")]
        [Description("A value indicating whether the mouse hover tracking for items in the list box is enabled.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool HoverEnabled
        {
            get
            {
                return _HoverEnabled; // return the value..
            }

            set
            {
                if (_HoverEnabled != value && !value) // toggle the event subscriptions based on the new value..
                {
                    MouseMove -= ListBoxExtension_MouseMove;
                }
                else if (_HoverEnabled != value && !value) // .. and again..
                {
                    MouseMove += ListBoxExtension_MouseMove;
                }

                if (!value) // value was set to false so refresh the items that might have had a hover index..
                {
                    // invalidate the index the mouse is hovering on top of only if there is something to invalidate..
                    if (_mouseHoverIndex != -1)
                    {
                        // invalidate if there is a valid value assigned for the hover index..
                        Invalidate(GetItemRectangle(_mouseHoverIndex)); 
                        _mouseHoverIndex = -1; // invalidate the hover index value..
                    }

                    // invalidate the previous mouse hover index only if there is something to invalidate..
                    if (_mouseHoverIndexPrevious != -1)
                    {
                        // invalidate if there is a valid value assigned for the previous hover index..
                        Invalidate(GetItemRectangle(_mouseHoverIndexPrevious));
                        _mouseHoverIndexPrevious = -1; // invalidate the previous hover index value..
                    }
                }

                _HoverEnabled = value; // save the value..
            }
        }

        private bool _NoKeyboardClick = false;

        /// <summary>
        /// Gets or sets a value indicating whether a Return button press of a keyboard will raise an ItemClicked event.
        /// </summary>
        [Category("Behavior")]
        [Description("A value indicating whether a Return button press of a keyboard will raise an ItemClicked event.")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool NoKeyboardClick
        {
            get // return the value..
            {
                return _NoKeyboardClick;
            }

            set
            {
                // toggle the PreviewKeyDown event subscribtion based on the value changed and the new value..
                if (_NoKeyboardClick != value)
                {
                    if (!value) // unsubscribe the event as the value has changed and it is false..
                    {
                        PreviewKeyDown -= ListBoxExtension_PreviewKeyDown;
                    }
                    else // subscribe the event as the value has changed and it is true..
                    {
                        PreviewKeyDown += ListBoxExtension_PreviewKeyDown;
                    }
                }
                _NoKeyboardClick = value; // ..and finally save the value..
            }
        }
        #endregion
    }

    #region ListBoxButtonClickEventArgs
    /// <summary>
    /// Event arguments for the VPKSoft.VisualListBox control when a button on the item was clicked.
    /// </summary>
    public class ListBoxButtonClickEventArgs : EventArgs
    {
        /// <summary>
        /// An index of an item which button was clicked.
        /// </summary>
        public int ItemIndex { get; set; }

        /// <summary>
        /// The item which button was clicked.
        /// </summary>
        public object Item { get; set; }
    }
    #endregion

    #region VScrollChangedEventArgs
    /// <summary>
    /// A class that is passed to the ListBoxExtension's VScrollChanged event.
    /// </summary>
    public class VScrollChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The current minimum value of the scroll bar position. Always 0.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// The current maximum value for the scroll bar position.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// The current value for the scroll bar position.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Indicates if the event was raised internally from the control or the VScrollPosition property's value was changed by code.
        /// </summary>
        public bool FromControl { get; set; }
    }
    #endregion
}
