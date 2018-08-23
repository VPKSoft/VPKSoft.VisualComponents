#region License
/*
VisualLocationBrowser

A list control for selecting various given items.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VisualLocationBrowser.

VisualLocationBrowser is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VisualLocationBrowser is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VisualLocationBrowser.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VPKSoft.VisualUtils; // (C): http://www.vpksoft.net/, GNU Lesser General Public License Version 3

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space holding the VisualLocationBrowser control.
/// </summary>
namespace VPKSoft.VisualLocationBrowser
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A control that shows a list of custom location(s) or URL(s), which is suitable for larger screens, i.e. television.
    /// </summary>
    public partial class VisualLocationBrowser : UserControl, IMessageFilter
    {
        /// <summary>
        /// A constructor for the VisualLocationBrowser class.
        /// </summary>
        public VisualLocationBrowser()
        {
            InitializeComponent();

            DoubleBuffered = true;

            // there is no reason to localize THIS..

            DoNoHScroll(); // "disable" the control's own scrollbar..

            RaiseGetItemListEvent(); // raise event to subscriber(s) that items are required to the list..
            this.Disposed += VisualLocationBrowser_Disposed; // add an event handler to unsubscribe the other event handlers..
            pnMain.MouseWheel += MouseWheelEvent; // subscribe to the mouse wheel event..
            this.MouseWheel += MouseWheelEvent; // .. and just in case keep subscribing to the mouse wheel event..
            pnLocationList.MouseWheel += MouseWheelEvent; // .. and again just in case keep subscribing to the mouse wheel event..
        }

        #region MiscAndUtils
        /// <summary>
        /// Seems to be a good way to capture mouse wheel scroll "event" as it does not raise the Scroll event.. of course somewhat "fishy"..
        /// </summary>
        /// <param name="m">A reference to Message class instance.</param>
        /// <returns>Always false, it is not wanted to seem that the message was handled.</returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x20a) // this is the (hexadecimal) code for the mouse wheel.. it is just followed blindly (no fancy things)..
            {
                vsbVertical.Maximum = pnLocationList.VerticalScroll.Maximum; // exception might occur otherwise..
                vsbVertical.Value = pnLocationList.VerticalScroll.Value; // do set the value even if nothing was changed..
            }
            return false; // as int the description, it is not wanted to seem that the message was handled..
        }

        // unsubscribe the event handlers of the child controls and to THIS event..
        private void VisualLocationBrowser_Disposed(object sender, EventArgs e)
        {
            ClearControls(); // mostly event handler unsubscribtion code..
            this.Disposed -= VisualLocationBrowser_Disposed; // this is not required any more..
        }

        // (C): https://stackoverflow.com/questions/5489273/how-do-i-disable-the-horizontal-scrollbar-in-a-panel
        // try to visually override the control's automatic scroll bar and disable horizontal scroll "fully"..
        private void DoNoHScroll(bool doSuspend = true)
        {
            if (doSuspend) // false if already suspended..
            {
                SuspendLayout();
            }

            pnLocationList.AutoScroll = true; // scrolling is wanted..
            pnLocationList.HorizontalScroll.Enabled = false; // ..but no horizontal one..
            pnLocationList.HorizontalScroll.Visible = false; // ..continuing to: but no horizontal one..
            vsbVertical.Maximum = pnLocationList.VerticalScroll.Maximum; // .. set the maximum value to avoid self-caused exceptions..
            vsbVertical.Visible = pnLocationList.VerticalScroll.Visible; // we don't want the vsbVertical to "go hiding" behind the standard scroll bar..
            if (vsbVertical.Visible) // .. so if visible..
            {
                vsbVertical.BringToFront(); // ..bring it to front
            }

            if (doSuspend) // false if already suspended..
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Causes the ItemList event to be raised if subscribed
        /// </summary>
        public void UpdateItemList()
        {
            RaiseGetItemListEvent(); //.. as described before.
        }

        /// <summary>
        /// The items (location(s) or URL(s)) and their descriptions to show on the control
        /// </summary>
        private List<KeyValuePair<string, string>> locationList = new List<KeyValuePair<string, string>>();

        // unsubscribe the event handlers from the child controls and then let them be destroyed..
        private void ClearControls(bool doSuspend = true)
        {
            if (doSuspend) // false if already suspended..
            {
                SuspendLayout();
            }

            foreach (Control control in pnLocationList.Controls) // loop through the child controls..
            {
                if (control is Label) // only the controls which were created dynamically by the class..
                {
                    control.MouseEnter -= ItemMouseEnter; // detach the event handlers..
                    control.MouseLeave -= ItemMouseLeave;
                    control.Click -= ItemClickHandler; // END: detach the event handlers..
                }
                else if (control is Panel) // .. AGAIN: only the controls which were created dynamically by the class..
                {
                    control.MouseEnter -= ItemMouseEnter; // detach the event handlers..
                    control.MouseLeave -= ItemMouseLeave;
                    control.Click -= ItemClickHandler; // END: detach the event handlers..
                }
            }
            pnLocationList.Controls.Clear(); // event handlers are no unsubscribed, so let them "free"..

            if (doSuspend) // false if already suspended..
            {
                ResumeLayout();
            }
        }
        #endregion

        #region Layout
        // list the location(s) or URL(s) given to the control via an event subscribtion..
        private void ListLocations(string debugWhoCalled)
        {
            SuspendLayout(); // do not "excessively" stress the (G)UI..
            pnLocationList.SuspendLayout(); // AGAIN: do not "excessively" stress the (G)UI..

            vsbVertical.Width = SystemInformation.VerticalScrollBarWidth; // make the visual scroll bar to the size of the automatic scroll bar..
            // .. so it can be "hidden"..

            // clear the previously constructed controls and unsubscribe their event handlers..
            ClearControls(false);

            // do show the backward and forward buttons only if a property requires a visibility..
            tlpBackForward.Visible = BackForwardButtonsVisible;
            // do show the add button only if a property requires a visibility..
            pnAddSomething.Visible = AddLocationButtonVisible;

            pnAddSomething.BackgroundImage = AddLocationButtonVisible ? AddItemButton : null; // assign the add "button" image if required (visible)..
            pnBack.BackgroundImage = BackForwardButtonsVisible ? BackButtonImage : null; // assign the backward "button" image if required (visible)..
            pnForward.BackgroundImage = BackForwardButtonsVisible ? ForwardButtonImage : null; // assign the forward "button" image if required (visible)..

            // if visibility is required set their sizes accordingly..
            tlpMain.RowStyles[0] = BackForwardButtonsVisible ?
                new RowStyle(SizeType.Absolute, ItemMaxHeight) : // visibility - so the size must be of some value..
                new RowStyle(SizeType.Absolute, 0.0f); // no visibility - so the size must of value of zero..

            tlpMain.RowStyles[2] = AddLocationButtonVisible ?
                new RowStyle(SizeType.Absolute, ItemMaxHeight) : // visibility - so the size must be of some value..
                new RowStyle(SizeType.Absolute, 0.0f); // no visibility - so the size must of value of zero..

            int topPosition = 0; // this would be the top position of an item in the list of location(s) or URL(s).. do remember to increase!
            foreach (KeyValuePair<string, string> location in locationList)
            {
                // A TableLayoutPanel for an item
                TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
                {
                    RowCount = 1, // only one row is needed..
                    ColumnCount = 2 // two column are required..
                }; // create the "base" control for an item in the list of (you know)..
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f)); // as there is only one row is's size is 100%..

                // if a delete item button is required the give it a reasonable size..
                if (ShowDeleteButton)
                {
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
                }
                else // ..otherwise give it a useless size..
                {
                    tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f));
                }


                // create a base for an item (TableLayoutPanel) based on the property of ItemMaxHeight's value..
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, ItemMaxHeight)); // ..do create a column style for it..
                tableLayoutPanel.Margin = new Padding(0); // no margins are required as the control is designed to serve in a full screen mode..
                tableLayoutPanel.MaximumSize = new Size(pnLocationList.ClientSize.Width, ItemMaxHeight); // .. for layout safety, do add it a constraint..
                tableLayoutPanel.Size = new Size(pnLocationList.ClientSize.Width, ItemMaxHeight); // do set the size for an item..
                tableLayoutPanel.Location = new Point(0, topPosition); // .. and give it a "reasonable" position..
                topPosition += ItemMaxHeight; // .. and align it to it's parent..
                tableLayoutPanel.Tag = location.Value; // save an actual location to the control's tag..
                tableLayoutPanel.BackColor = ItemBackgroundColor; // and now give them their given colors..
                tableLayoutPanel.ForeColor = ItemForeColor;

                // A Label for an item..
                Label label = new Label
                {
                    Text = location.Value, // text should differ from it's actual location..
                    Tag = location.Key, // AGAIN: save an actual location to the control's tag..
                    BackColor = ItemBackgroundColor, // and now give them their given colors..
                    ForeColor = ItemForeColor,
                    Dock = DockStyle.Fill, // dock the control to fill its given location..
                    Margin = new Padding(0) // no padding..
                }; // .. so create a label..

                // measure the font to fit the label using a long string of Unicode characters..
                UtilsMisc.ResizeFontHeight(label, true, 0.5f, ItemMaxHeight);

                // add the label to it's parent
                tableLayoutPanel.Controls.Add(label, 0, 0);
                label.Parent = tableLayoutPanel; // subscribe to event handlers so the control can react to user input correctly..
                label.MouseEnter += ItemMouseEnter;
                label.MouseLeave += ItemMouseLeave;
                label.Click += ItemClickHandler; // END: subscribe to event handlers so the control can react to user input correctly..

                // A Panel for to act as a button for item's deletion
                if (ShowDeleteButton) // no need to create this if it's not visible
                {
                    Panel panel = new Panel(); // create a panel to act as a button..
                    panel.SuspendLayout(); // do not "excessively" stress the (G)UI..
                    panel.Tag = location.Key; // just add the Tag and "Text" properties to the item's actual location..
                    panel.Text = location.Key;
                    panel.BackColor = ItemBackgroundColor; // and now give "button" their given colors..
                    panel.ForeColor = ItemForeColor;
                    panel.Dock = DockStyle.Fill; // dock can be set to fill, the column style should handle the rest..
                    panel.Margin = new Padding(0); // no padding..
                    panel.BackgroundImageLayout = ImageLayout.Zoom; // make the image to fit the panel..
                    panel.BackgroundImage = DeleteItemImage; // assign the image to the panel acting as a button..
                    tableLayoutPanel.Controls.Add(panel, 1, 0); // add the panel to it's parent (the item holder)..
                    panel.Parent = tableLayoutPanel; // assign the correct parent for the panel..
                    panel.MouseEnter += ItemMouseEnter; // subscribe to event handlers so the control can react to user input correctly..
                    panel.MouseLeave += ItemMouseLeave;
                    panel.Click += ItemClickHandler; // END: subscribe to event handlers so the control can react to user input correctly..
                    panel.ResumeLayout(); // do not "excessively" stress the (G)UI..
                }

                pnLocationList.Controls.Add(tableLayoutPanel); // a location item is no ready to be added to the parent control..
                tableLayoutPanel.Parent = pnLocationList; // .. give the item a parent..
            }

            pnLocationList.ResumeLayout(); // no "excessively" (G)UI stressing isn't coming by the layout anymore..
            ResumeLayout();
            DoNoHScroll(); // hide horizontal scrolling..
        }
        #endregion

        #region ColorProperties
        // Item's background color..
        private Color _ItemBackgroundColor = Color.SteelBlue;

        /// <summary>
        /// The background color of an item in the location listing.
        /// </summary>
        [Description("The background color of a location entry")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(typeof(Color), "SteelBlue")]
        public Color ItemBackgroundColor
        {
            get
            {
                return _ItemBackgroundColor;
            }

            set
            {
                if (value == _ItemBackgroundColor) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _ItemBackgroundColor = value;
                ListLocations("ItemBackgroundColor"); // List the directory contents..
            }
        }

        // Item's foreground color..
        private Color _ItemForeColor = Color.White;

        /// <summary>
        /// The foreground color of an item in the location listing.
        /// </summary>
        [Description("The color of a location entry")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(typeof(Color), "White")]
        public Color ItemForeColor
        {
            get
            {
                return _ItemForeColor;
            }

            set
            {
                if (value == _ItemForeColor) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _ItemForeColor = value;
                ListLocations("ItemForeColor"); // List the location list contents..
            }
        }

        // Item's background color when mouse is on top of an item.
        private Color _HoverBackgroundColor = Color.LightSkyBlue;

        /// <summary>
        /// The background color of an item in the location listing when mouse hovers over the item. 
        /// </summary>
        [Description("The background color of an item the mouse is currently hovering over")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(typeof(Color), "LightSkyBlue")]
        public Color HoverBackgroundColor
        {
            get
            {
                return _HoverBackgroundColor;
            }

            set
            {
                _HoverBackgroundColor = value;
            }
        }

        // Item's foreground color when mouse is on top of an item.
        private Color _HoverItemColor = Color.Moccasin;

        /// <summary>
        /// The foreground color of an item in the location listing when mouse hovers over the item. 
        /// </summary>
        [Description("The foreground color of an item the mouse is currently hovering over")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(typeof(Color), "Moccasin")]
        public Color HoverItemColor
        {
            get
            {
                return _HoverItemColor;
            }

            set
            {
                _HoverItemColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the background color for the control")]
        [DefaultValue(typeof(Color), "SteelBlue")]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }
        #endregion

        #region VisualConstraints

        private int _ItemMaxHeight = 50;

        /// <summary>
        /// The foreground color of an item in the location listing.
        /// </summary>
        [Description("A maximum height in pixels of a location entry")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(50)]
        public int ItemMaxHeight
        {
            get
            {
                return _ItemMaxHeight;
            }

            set
            {
                if (value == _ItemMaxHeight) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _ItemMaxHeight = value;
                ListLocations("ItemMaxHeight"); // List the location list contents..
            }
        }

        #endregion

        #region ClassProperties
        // A value indicating if the add new location button is visible..
        private bool _AddLocationButtonVisible = true;

        /// <summary>
        /// Gets or sets the value indicating if the add new location button is visible.
        /// </summary>
        [Description("Gets or sets the value indicating if the add new location button is visible")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(true)]
        public bool AddLocationButtonVisible
        {
            get
            {
                return _AddLocationButtonVisible;
            }

            set
            {
                if (value == _AddLocationButtonVisible) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _AddLocationButtonVisible = value;
                ListLocations("AddLocationButtonVisible"); // List the location list contents..
            }
        }

        // A value indicating if the back / forward buttons are visible..
        private bool _BackForwardButtonsVisible = true;

        /// <summary>
        /// Gets or sets the value indicating if the backward and forward navigation buttons are visible.
        /// </summary>
        [Description("Gets or sets the value indicating if the backward and forward navigation buttons are visible")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(true)]
        public bool BackForwardButtonsVisible
        {
            get
            {
                return _BackForwardButtonsVisible;
            }

            set
            {
                if (value == _BackForwardButtonsVisible) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _BackForwardButtonsVisible = value;
                ListLocations("BackForwardButtonsVisible"); // List the location list contents..
            }
        }

        // A value indicating if an item can be deleted from the list and an event is raised for that action..
        private bool _ShowDeleteButton = true;

        /// <summary>
        /// Gets or sets the indicating if an item can be deleted from the list and an event is raised for that action.
        /// </summary>
        [Description("Gets or sets the indicating if an item can be deleted from the list and an event is raised for that action")]
        [Category("VisualLocationBrowser")]
        [DefaultValue(true)]
        public bool ShowDeleteButton
        {
            get
            {
                return _ShowDeleteButton;
            }

            set
            {
                if (value == _ShowDeleteButton) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _ShowDeleteButton = value;
                ListLocations("ShowDeleteButton"); // List the location list contents..
            }
        }
        #endregion

        #region ImageProperties
        // An image to show as a delete item button
        private Image _DeleteItemImage = VisualComponents.Properties.Resources.remove;

        /// <summary>
        /// An image to show as a delete item button image.
        /// </summary>
        [Description("An image to show as a delete item button image")]
        [Category("VisualLocationBrowser")]
        public Image DeleteItemImage
        {
            get
            {
                return _DeleteItemImage;
            }

            set
            {
                if (value == _DeleteItemImage) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _DeleteItemImage = value;
                ListLocations("DeleteItemImage"); // List the location list contents..
            }
        }

        // An image to show as the add location button image
        private Image _AddItemButton = VisualComponents.Properties.Resources.add_something;

        /// <summary>
        /// An image to show as the add location button image.
        /// </summary>
        [Description("An image to show as the add location button image")]
        [Category("VisualLocationBrowser")]
        public Image AddItemButton
        {
            get
            {
                return _AddItemButton;
            }

            set
            {
                if (value == _AddItemButton) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _AddItemButton = value;
                ListLocations("AddItemButton"); // List the location list contents..
            }
        }

        // An image to show as the navigate backward button image
        private Image _BackButtonImage = VPKSoft.VisualComponents.Properties.Resources.back_misc;

        /// <summary>
        ///  An image to show as the navigate backward button image.
        /// </summary>
        [Description("An image to show as the navigate backward button image")]
        [Category("VisualLocationBrowser")]
        public Image BackButtonImage
        {
            get
            {
                return _BackButtonImage;
            }

            set
            {
                if (value == _BackButtonImage) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _BackButtonImage = value;
                ListLocations("BackButtonImage"); // List the location list contents..
            }
        }

        // An image to show as the navigate forward button image
        private Image _ForwardButtonImage = VisualComponents.Properties.Resources.forward_misc;

        /// <summary>
        ///  An image to show as the navigate forward button image.
        /// </summary>
        [Description("An image to show as the navigate forward button image")]
        [Category("VisualLocationBrowser")]
        public Image ForwardButtonImage
        {
            get
            {
                return _ForwardButtonImage;
            }

            set
            {
                if (value == _ForwardButtonImage) // avoid excess refresh amount..
                {
                    return; // .. so just return..
                }
                _ForwardButtonImage = value;
                ListLocations("ForwardButtonImage"); // List the location list contents..
            }
        }
        #endregion

        #region InternalEvents
        // Mouse cursor has left the area of a click-able item..
        private void ItemMouseLeave(object sender, EventArgs e)
        {
            Control control = ((Control)sender); // Cast the sender as control..

            foreach (Control ctrl in control.Controls) // ..restore normal color to the control's children..
            {
                ctrl.ForeColor = ItemForeColor;
                ctrl.BackColor = ItemBackgroundColor;
            }

            control.BackColor = ItemBackgroundColor; // ..restore normal color to the control.
        }

        // Mouse cursor has left the area of a click-able item..
        private void ItemMouseEnter(object sender, EventArgs e)
        {
            Control control = ((Control)sender); // Cast the sender as control..

            foreach (Control ctrl in control.Controls) // ..set the mouse hover colors to the control's children..
            {
                ctrl.ForeColor = HoverItemColor;
                ctrl.BackColor = HoverBackgroundColor;
            }

            control.BackColor = HoverBackgroundColor; // ..set the mouse hover color to the control.
        }

        // re-align/resize the controls to fit the control..
        private void VisualLocationBrowser_Resize(object sender, EventArgs e)
        {
            // .. so a new row style is required every time the size changes..
            tlpMain.RowStyles[0] = new RowStyle(SizeType.Absolute, (float)ItemMaxHeight);

            // .. and a repeat of the previous..
            tlpMain.RowStyles[2] = new RowStyle(SizeType.Absolute, (float)ItemMaxHeight);

            // calculate the minimum width/height for the buttons..
            int hw = Math.Min(pnBack.Height, pnBack.Width); // .. so using of math is expected to give a proper answer hopefully..
            pnBack.Size = new Size(new Point(hw, hw)); // do resize..
            pnForward.Size = new Size(new Point(hw, hw)); // ..do resize..
        }

        // When the client size of a control has changed the child controls need some resizing..
        private void VisualLocationBrowser_ClientSizeChanged(object sender, EventArgs e)
        {
            if (sender.Equals(pnLocationList)) // Check which is the calling control..
            {
                SuspendLayout();
                pnLocationList.SuspendLayout();

                vsbVertical.Width = SystemInformation.VerticalScrollBarWidth;

                foreach (Control ctrl in pnLocationList.Controls) // this is to prevent the horizontal scrollbar to appear..
                {
                    ctrl.Width = pnLocationList.ClientSize.Width;// - SystemInformation.VerticalScrollBarWidth;
                }
                pnLocationList.ResumeLayout();
                DoNoHScroll(false);
                ResumeLayout();
            }
        }

        // If the Item (Label) or a Button (Panel) was clicked, raise and event (if subscribed) accordingly. 
        private void ItemClickHandler(object sender, EventArgs e)
        {
            if (sender.Equals(pnAddSomething)) // a request was made to add some item to the list..
            {
                if (ItemAddRequest != null) // .. cant raise event if not subscribed..
                {
                    // let the "user" code handle the logic of adding an item to the list..
                    ItemSelectedEventArgs args = new ItemSelectedEventArgs() { ItemLocation = string.Empty, ItemDescription = string.Empty, ItemParentLocation = string.Empty, Cancel = false };
                    ItemAddRequest(this, args); // raise the event with "empty" ItemSelectedEventArgs class instance
                    if (!args.Cancel) // if no cancellation was requested in the event, then a new item might have been added..
                    {
                        RaiseGetItemListEvent(); // .. so get a possible new list another event needs to be raised.
                    }
                }
            }
            else if (sender.GetType() == typeof(Panel)) // to only other valid Panel should be one of the delete item buttons..
            {
                Panel panel = (Panel)sender; // just do a cast to shorten the forthcoming code..
                if (ItemDeleted != null) // .. cant raise event if not subscribed..
                {
                    // let the "user" code handle the logic of deleting an item from the list..
                    ItemSelectedEventArgs args = new ItemSelectedEventArgs() { ItemLocation = panel.Tag.ToString(), ItemDescription = panel.Text, Cancel = false };
                    // raise the event with values describing the item to be deleted/removed with an ItemSelectedEventArgs class instance..
                    ItemDeleted(this, args);
                    if (!args.Cancel) // if no cancellation was requested in the event, then item is requested to be deleted..
                    {
                        RaiseGetItemListEvent(); // so the "user" code has handled the deletion --> a possible new list should be refreshed..
                    }
                }
            }
            else if (ItemSelected != null) // an item was selected from the list..
            {
                Label label = (Label)sender; // ..which means that a label describing the item was clicked..
                // ..so allow the "user" code to handle the item click.. the event cancellation doesn't mean anything at this point..
                ItemSelected(this, new ItemSelectedEventArgs() { ItemLocation = label.Tag.ToString(), ItemDescription = label.Text, Cancel = false });
            }
        }

        // this is possible a useless subscribtion to an event handler as the "hidden" actual vertical scroll bar is "buried" under a different control
        private void pnLocationList_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll) // accept only vertical scroll bar values..
            {
                vsbVertical.Value = e.NewValue; // give a new value to the actual visible scroll bar..
            }
        }

        // this is possible a useless subscribtion to an event handler as the mouse wheel event doesn't seem to fire..
        private void MouseWheelEvent(object sender, MouseEventArgs e)
        {
            vsbVertical.Value = pnLocationList.VerticalScroll.Value; // give a new value to the actual visible scroll bar..
        }

        // if the "buried" scroll bar fires an event set the visible scroll bar's value to math this one..
        private void pnScroll_ValueChanged(object sender, ScrollEventArgs e)
        {
            pnLocationList.VerticalScroll.Value = e.NewValue; // give a new value to the actual visible scroll bar..
        }
        #endregion

        #region Events
        /// <summary>
        /// Raises the GetItemList event if subscribed.
        /// </summary>
        private void RaiseGetItemListEvent()
        {
            if (GetItemList != null) // Can't raise an event of not subscribed..
            {
                GetItemListEventArgs getItemListEventArgs = new GetItemListEventArgs
                {
                    ItemList = new List<KeyValuePair<string, string>>(locationList) // Tell the current location list's contents to the event's subscribers..
                };
                GetItemList(this, getItemListEventArgs);

                // check if the list after the event was actually changed before redraw..
                if (CompareItemListSame(getItemListEventArgs.ItemList))
                {
                    return;
                }

                locationList = getItemListEventArgs.ItemList; // ..update the current location list's contents give from to the event's subscribers..

                ListLocations("RaiseGetItemListEvent()");
            }
        }

        private bool CompareItemListSame(List<KeyValuePair<string, string>> compareTo)
        {
            if (locationList.Count != compareTo.Count)
            {
                return false;
            }

            List<KeyValuePair<string, string>> comp1 = new List<KeyValuePair<string, string>>(locationList);
            List<KeyValuePair<string, string>> comp2 = new List<KeyValuePair<string, string>>(compareTo);

            comp1.Sort((x, y) => x.Key.CompareTo(y.Key));
            comp2.Sort((x, y) => x.Key.CompareTo(y.Key));

            for (int i = 0; i < comp1.Count; i++)
            {
                if (comp1[i].Key != comp2[i].Key)
                {
                    return false;
                }
                if (comp1[i].Value != comp2[i].Value)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// An class that is given as a parameter to ItemSelected and ItemDeleted events.
        /// </summary>
        public class ItemSelectedEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the value of the item's location.
            /// </summary>
            public string ItemLocation { get; set; }

            /// <summary>
            /// Gets or sets the description of the item's location.
            /// </summary>
            public string ItemDescription { get; set; }

            /// <summary>
            /// Gets or sets the parent location of an item's location.
            /// </summary>
            public string ItemParentLocation { get; set; }

            /// <summary>
            /// Gets or sets the value if an item can be deleted.
            /// </summary>
            public bool Cancel { get; set; }
        }

        /// <summary>
        /// An class that is given as a parameter to GetItemList event.
        /// </summary>
        public class GetItemListEventArgs : EventArgs
        {
            /// <summary>
            /// Gets or set the value of the items and their descriptions.
            /// </summary>
            public List<KeyValuePair<string, string>> ItemList = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// A delegate for the ItemSelected event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">ItemSelectedEventArgs class instance.</param>
        public delegate void OnGetItemList(object sender, GetItemListEventArgs e);

        /// <summary>
        /// An event that is raised when the user selects a file from the directory listing.
        /// </summary>
        [Description("An event that is raised when the control requests an item listing")]
        [Category("VisualFileBrowser")]
        public event OnGetItemList GetItemList = null;

        /// <summary>
        /// A delegate for the ItemSelected event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">ItemSelectedEventArgs class instance.</param>
        public delegate void OnItemSelected(object sender, ItemSelectedEventArgs e);

        /// <summary>
        /// An event that is raised when the user selects a file from the directory listing.
        /// </summary>
        [Description("An event that is raised when the user selects an item from the listing")]
        [Category("VisualFileBrowser")]
        public event OnItemSelected ItemSelected = null;

        /// <summary>
        /// A delegate for the ItemDeleted event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">ItemSelectedEventArgs class instance.</param>
        public delegate void OnItemDeleted(object sender, ItemSelectedEventArgs e);

        /// <summary>
        /// An event that is raised when the user selects a file from the directory listing.
        /// </summary>
        [Description("An event that is raised when the user deletes an item from the listing")]
        [Category("VisualFileBrowser")]
        public event OnItemDeleted ItemDeleted = null;

        /// <summary>
        /// A delegate for the ItemAddRequest event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">ItemSelectedEventArgs class instance.</param>
        public delegate void OnItemAddRequest(object sender, ItemSelectedEventArgs e);

        /// <summary>
        /// An event that is raised when the user selects a file from the directory listing.
        /// </summary>
        [Description("An event that is raised when the user clicks the add item button on the control")]
        [Category("VisualFileBrowser")]
        public event OnItemAddRequest ItemAddRequest = null;
        #endregion
    }
}
