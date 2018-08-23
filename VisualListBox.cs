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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Design;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the ListBoxExtension and VisualListBox controls.
/// </summary>
namespace VPKSoft.VisualListBox
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// An extended list box control with alternating colors and a possibility to report it's scroll position.
    /// </summary>
    public partial class VisualListBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualListBox"/> class.
        /// </summary>
        public VisualListBox()
        {
            InitializeComponent(); // allow the generated code to run..

            // subscribe to a few events..
            lbeMain.SelectedIndexChanged += LbeMain_SelectedIndexChanged;
            lbeMain.SelectedValueChanged += LbeMain_SelectedValueChanged;
            Disposed += VisualListBox_Disposed;
            ClientSizeChanged += VisualListBox_ClientSizeChanged;

            // set the width of the "overridden" scroll bar..
            vsbMain.Width = SystemInformation.VerticalScrollBarWidth;

            // construct a timer to watch if the ListBox.Items.Count property has been changed..
            listCountChangeTimer = new System.Timers.Timer(500);
            listCountChangeTimer.Elapsed += ListCountChangeTimer_Elapsed;

            // clear the items from the ListBox..
            Items.Clear();
        }

        #region Dispose
        // unsubscribe from the self subscribed events..
        private void VisualListBox_Disposed(object sender, EventArgs e)
        {
            lbeMain.SelectedIndexChanged -= LbeMain_SelectedIndexChanged;
            lbeMain.SelectedValueChanged -= LbeMain_SelectedValueChanged;
            ClientSizeChanged -= VisualListBox_ClientSizeChanged;

            using (listCountChangeTimer) // dispose of the disposable..
            {
                listCountChangeTimer.Stop(); // stop the Items.Count watch timer..
                // ..unsubscribe from it's event..
                listCountChangeTimer.Elapsed -= ListCountChangeTimer_Elapsed;
            }

            // unsubscribe the Disposed event (!)..
            Disposed -= VisualListBox_Disposed;
        }
        #endregion

        #region InternalEvents
        // resize the "overridden" scroll bar to hide the actual one
        private void VisualListBox_ClientSizeChanged(object sender, EventArgs e)
        {
            vsbMain.Width = SystemInformation.VerticalScrollBarWidth;
            vsbMain.BringToFront(); // ensure that the "overridden" scroll bar is on the top..
        }

        // invoke the ListBox events downwards..
        private void LbeMain_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedValueChanged?.Invoke(sender, e); // only if subscribed..
        }

        // invoke the ListBox events downwards..
        private void LbeMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged?.Invoke(sender, e); // only if subscribed..
        }

        // invoke the ListBox events downwards..
        private void lbeMain_ButtonClicked(object sender, ListBoxButtonClickEventArgs e)
        {
            ButtonClicked?.Invoke(this, e); // only if subscribed..
        }

        /// <summary>
        /// Handles the DoubleClick event of the lbeMain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void lbeMain_DoubleClick(object sender, EventArgs e)
        {
            base.OnDoubleClick(e);
        }
        #endregion

        #region BaseColors
        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return lbeMain.ForeColor;
            }

            set
            {
                base.ForeColor = value;
                lbeMain.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return lbeMain.BackColor;
            }

            set
            {
                base.BackColor = value;
                lbeMain.BackColor = value;
            }
        }
        #endregion

        #region ExposeListBox
        /// <summary>
        ///  Gets or sets the zero-based index of the currently selected item in a System.Windows.Forms.ListBox.
        /// </summary>
        [Browsable(false)] // no one should want the browse this in the visual designer..
        public int SelectedIndex { get => lbeMain.SelectedIndex; set => lbeMain.SelectedIndex = value; }

        /// <summary>
        /// Gets a collection that contains the zero-based indices of all currently selected items in the System.Windows.Forms.ListBox.
        /// </summary>
        public ListBox.SelectedIndexCollection SelectedIndices { get => lbeMain.SelectedIndices; }

        /// <summary>
        ///  Gets or sets the currently selected item in the System.Windows.Forms.ListBox.
        /// </summary>
        [Browsable(false)] // no one should want the browse this in the visual designer..
        public object SelectedItem { get => lbeMain.SelectedItem; set => lbeMain.SelectedItem = value; }

        /// <summary>
        /// Gets a collection containing the currently selected items in the System.Windows.Forms.ListBox.
        /// </summary>
        [Browsable(false)] // no one should want the browse this in the visual designer..
        public ListBox.SelectedObjectCollection SelectedItems { get => lbeMain.SelectedItems; }

        /// <summary>
        ///  Gets or sets the method in which items are selected in the System.Windows.Forms.ListBox.
        /// </summary>
        public SelectionMode SelectionMode { get => lbeMain.SelectionMode; set => lbeMain.SelectionMode = value; }

        /// <summary>
        ///  Selects or clears the selection for the specified item in a System.Windows.Forms.ListBox.
        /// </summary>
        /// <param name="index">The zero-based index of the item in a System.Windows.Forms.ListBox to select or clear the selection for</param>
        /// <param name="value">true to select the specified item; otherwise, false.</param>
        public void SetSelected(int index, bool value)
        {
            lbeMain.SetSelected(index, value);
        }

        /// <summary>
        /// Gets or sets the path of the property to use as the actual value for the items in the System.Windows.Forms.ListControl.
        /// </summary>
        [Category("Data")]
        public string ValueMember { get => lbeMain.ValueMember; set => lbeMain.ValueMember = value; }

        /// <summary>
        /// Gets or sets the property to display for this System.Windows.Forms.ListControl.
        /// </summary>
        [Category("Data")]
        public string DisplayMember { get => lbeMain.DisplayMember; set => lbeMain.DisplayMember = value; }

        /// <summary>
        /// Saves the indices of currently selected items.
        /// </summary>
        /// <param name="clearSelection">Whether to clear the selection after saving or not.</param>
        public void PushSelection(bool clearSelection = false)
        {
            lbeMain.PushSelection(clearSelection);
        }

        /// <summary>
        /// Restores a saved selection.
        /// </summary>
        public void PopSelection()
        {
            lbeMain.PopSelection();
        }

        /// <summary>
        ///  Returns a value indicating whether the specified item is selected.
        /// </summary>
        /// <param name="index">The zero-based index of the item that determines whether it is selected.</param>
        /// <returns>true if the specified item is currently selected in the System.Windows.Forms.ListBox; otherwise, false.</returns>
        public bool GetSelected(int index)
        {
            return lbeMain.GetSelected(index);
        }

        /// <summary>
        /// Occurs when the System.Windows.Forms.ListBox.SelectedIndex property or the System.Windows.Forms.ListBox.SelectedIndices collection has changed.
        /// </summary>
        public EventHandler SelectedIndexChanged;

        /// <summary>
        /// Occurs when the System.Windows.Forms.ListControl.SelectedValue property changes.
        /// </summary>
        public EventHandler SelectedValueChanged;

        /// <summary>
        /// Unselects all items in the VPKSoft.VisualListBox.ListBoxExtension.
        /// </summary>
        public void ClearSelected()
        {
            lbeMain.ClearSelected();
        }

        /// <summary>
        /// Gets or sets the back color alternative.
        /// </summary>
        /// <value>
        /// The back color alternative.
        /// </value>
        public Color BackColorAlternative { get => lbeMain.BackColorAlternative; set => lbeMain.BackColorAlternative = value; }

        /// <summary>
        /// Gets or sets the color of the selection.
        /// </summary>
        /// <value>
        /// The color of the selection.
        /// </value>
        public Color SelectionColor { get => lbeMain.SelectionColor; set => lbeMain.SelectionColor = value; }

        /// <summary>
        /// Gets or sets the selection color alternative.
        /// </summary>
        /// <value>
        /// The selection color alternative.
        /// </value>
        public Color SelectionColorAlternative { get => lbeMain.SelectionColorAlternative; set => lbeMain.SelectionColorAlternative = value; }

        /// <summary>
        /// Gets or sets the fore color alternative.
        /// </summary>
        /// <value>
        /// The fore color alternative.
        /// </value>
        public Color ForeColorAlternative { get => lbeMain.ForeColorAlternative; set => lbeMain.ForeColorAlternative = value; }

        /// <summary>
        /// Gets or sets the item mouse hover background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover background color for an item for the component.")]
        [DefaultValue(typeof(Color), "Highlight")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverBackColor { get => lbeMain.HoverBackColor; set => lbeMain.HoverBackColor = value; }

        /// <summary>
        /// Gets or sets the item mouse hover alternative background color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover alternative background color for an item for the component.")]
        [DefaultValue(typeof(Color), "Highlight")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverBackColorAlternative { get => lbeMain.HoverBackColorAlternative; set => lbeMain.HoverBackColorAlternative = value; }

        /// <summary>
        /// Gets or sets the item mouse hover foreground color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover alternative foreground color for an item for the component.")]
        [DefaultValue(typeof(Color), "ControlText")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverForeColor { get => lbeMain.HoverForeColor; set => lbeMain.HoverForeColor = value; }

        /// <summary>
        /// Gets or sets the item mouse hover alternative foreground color for the control.
        /// </summary>
        [Category("Appearance")]
        [Description("The item mouse hover alternative foreground color for an item for the component.")]
        [DefaultValue(typeof(Color), "ControlText")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color HoverForeColorAlternative { get => lbeMain.HoverForeColorAlternative; set => lbeMain.HoverForeColorAlternative = value; }

        /// <summary>
        /// Gets the items of the System.Windows.Forms.ListBox.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Localizable(true)]
        [MergableProperty(false)]
        [Category("Data")]
        public ListBox.ObjectCollection Items
        {
            get
            {
                return lbeMain.Items; // return the items..
            }
        }

        /// <summary>
        /// An image to display at the right of the item's text.
        /// </summary>
        [Category("Appearance")]
        [Description("An image to display at the right of the item's text.")]
        [DefaultValue(null)]
        [Browsable(true)]
        public Image RightImage { get => lbeMain.RightImage; set => lbeMain.RightImage = value; }

        /// <summary>
        /// An event that is raised when a user clicks the button of an item in the list box.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event ListBoxExtension.OnButtonClicked ButtonClicked = null;

        /// <summary>
        /// Gets or set a value if item selection is allowed.
        /// </summary>
        [Category("Behavior")]
        [Description("Indicates if a user can select an item from the list box.")]
        [DefaultValue(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool AllowItemSelection { get => lbeMain.AllowItemSelection; set => lbeMain.AllowItemSelection = value; }
        #endregion

        #region OtherProperties
        /// <summary>
        /// Gets or sets a value indicating whether the mouse hover tracking for items in the list box is enabled.
        /// </summary>
        [Category("Behavior")]
        [Description("A value indicating whether the mouse hover tracking for items in the list box is enabled.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool HoverEnabled { get => lbeMain.HoverEnabled; set => lbeMain.HoverEnabled = value; }
        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether a Return button press of a keyboard will raise an ItemClicked event.
        /// </summary>
        [Category("Behavior")]
        [Description("A value indicating whether a Return button press of a keyboard will raise an ItemClicked event.")]
        [DefaultValue(false)]
        [Browsable(true)]
        public bool NoKeyboardClick { get => lbeMain.NoKeyboardClick; set => lbeMain.NoKeyboardClick = value; }

        #region Scroll
        // this is used to measure if the item count of the list was changed..
        internal int lastItemCount = int.MinValue; // initialize with a ridiculous value..

        // ..and this refreshes the VisualScrollBar to match the Items.Count property if the value was changed..
        internal System.Timers.Timer listCountChangeTimer = null;

        // .. and here is the refresh code for the Items.Count property value changed
        internal void ListCountChangeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // ..refresh the VisualScrollBar..
            Invoke(new MethodInvoker(delegate { RefreshScroll(); }));
        }

        /// <summary>
        /// Refreshes the scroll bar (layout) if the item count was changed.
        /// <note type="note">A call to this method after every insert or deletion might be resource consuming.</note>
        /// </summary>
        public void RefreshScroll()
        {
            if (lastItemCount != lbeMain.Items.Count) // only if changed..
            {
                lastItemCount = lbeMain.Items.Count;
                vsbMain.SetMinMaxValueSafe(lbeMain.VScrollMinimum, lbeMain.VScrollMaximum, lbeMain.VScrollPosition);
            }
        }

        // this event is raised via the ListBoxExtension class instance when the scroll value changes..
        private void lbeMain_VScrollChanged(object sender, VScrollChangedEventArgs e)
        {
            try // it is not recommended that a user control crashes - so just to be safe..
            {
                if (e.FromControl) // if the value wasn't changed via code..
                {
                    vsbMain.SetMinMaxValueSafe(e.Minimum, e.Maximum, e.Value);
                    return;
                }

                // the value was changed via code..
                if (e.Maximum > vsbMain.Maximum)
                {
                    vsbMain.Maximum = e.Maximum; // set the value..
                }

                // avoid raising a circular event..
                vsbMain.ValueNoEvent = e.Value; // set the value without raising an event..

                vsbMain.BringToFront(); // bring the VisualScrollBar to the to just in case..
            }
            catch
            {
                // do nothing..
            }
        }

        // this event is raised via the VisualScrollBar class instance if it's value changes..
        private void vsbMain_ValueChanged(object sender, ScrollEventArgs e)
        {
            try // it is not recommended that a user control crashes - so just to be safe..
            {
                if (e.NewValue >= lbeMain.VScrollMaximum) // set the values "safely"..
                {
                    // avoid raising a circular event..
                    lbeMain.VScrollPositionNoEvent = lbeMain.VScrollMaximum - 1;
                }
                else
                {
                    // avoid raising a circular event..
                    lbeMain.VScrollPositionNoEvent = e.NewValue;
                }
            }
            catch
            {
                // do nothing..
            }
        }
        #endregion

        /// <summary>
        /// An event that is raised when a user clicks an item in the list box with mouse or presses keyboard return button.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event ListBoxExtension.OnButtonClicked ItemClicked = null;


        private void lbeMain_ItemClicked(object sender, ListBoxButtonClickEventArgs e)
        {
            ItemClicked?.Invoke(this, e);
        }
    }
}
