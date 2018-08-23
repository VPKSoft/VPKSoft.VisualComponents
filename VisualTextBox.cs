#region License
/*
VPKSoft.VisualTextBox

A control which is less visually challenged System.Windows.Forms.TextBox control.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.VisualTextBox.

VPKSoft.VisualTextBox is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.VisualTextBox is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.VisualTextBox.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the VisualTextBox control.
/// </summary>
namespace VPKSoft.VisualTextBox
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A control which is less visually challenged System.Windows.Forms.TextBox control.
    /// </summary>
    [DefaultEvent("TextChanged")]
    public partial class VisualTextBox : UserControl
    {
        /// <summary>
        /// The constructor of the VPKSoft.VisualTextBox.VisualTextBox control.
        /// </summary>
        public VisualTextBox()
        {
            InitializeComponent(); // let the '*.Designer.cs' do most of the work..
            dgvTextBoxPretend.RowCount = 1; // only one row and the user can't add them..
            SetColors(); // give the one cell a decent look..

            // keep the single cell's size always so that it fills the control..
            dgvTextBoxPretend.Rows[0].Height = dgvTextBoxPretend.Height;
        }

        /// <summary>
        ///  Gets a value indicating whether the control has input focus.
        /// </summary>
        public override bool Focused
        {
            get
            {
                return base.Focused || dgvTextBoxPretend.Focused || dgvTextBoxPretend.ContainsFocus;                
            }
        }

        /// <summary>
        ///  Sets input focus to the control.
        /// </summary>
        /// <returns>true if the input focus request was successful; otherwise, false.</returns>
        public new bool Focus()
        {            
            return dgvTextBoxPretend.Focus();
        }

        /// <summary>
        /// Selects all text in the text box.
        /// </summary>
        public void SelectAll()
        {
            if (!Focused)
            {
                Focus();
            }
            dgvTextBoxPretend.BeginEdit(true);
        }
        

        #region Properties
        /// <summary>
        /// Gets or sets the current text in the VPKSoft.VisualTextBox.VisualTextBox.
        /// </summary>
        [Description("Gets or sets the current text in the VPKSoft.VisualTextBox.VisualTextBox")]
        [Category("Appearance")]
        [Browsable(true)] // must ensure this is shows in the auto-complete, designer and is brow-sable..
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)] // .. END: must ensure this is shows in the auto-complete, designer and is brow-sable.
        public override string Text
        {
            get
            {
                if (base.Text == null) // don't upset the user by returning a null value..
                {
                    return string.Empty; // ..so just return an empty string..
                }
                return base.Text; // return the base class Text property value..
            }

            set
            {
                base.Text = value; // set the base class Text property value..
                if (!textChangedInternally) // if the Text property was changed internally, avoid causing the cell's value to get the same value again..
                {
                    dgvTextBoxPretend.Rows[0].Cells[0].Value = Text; // set the one cell's text as the Text property value..
                }
                textChangedInternally = false; // .. set to flag to false..

                TextChanged?.Invoke(this, new EventArgs()); // .. raise the event..
            }
        }

        // the alignment of text in the VPKSoft.VisualTextBox.VisualTextBox..
        private ContentAlignment _TextAlign = ContentAlignment.TopLeft; // this seems to be the default value everywhere..

        /// <summary>
        /// Gets or sets the alignment of text in the VPKSoft.VisualTextBox.VisualTextBox.
        /// </summary>
        [Description("Gets or sets the alignment of text in the VPKSoft.VisualTextBox.VisualTextBox")]
        [DefaultValue(ContentAlignment.TopLeft)] // AGAIN: this seems to be the default value everywhere..
        [Category("Appearance")]
        public ContentAlignment TextAlign
        {
            get
            {
                return _TextAlign; // return the current text alignment..
            }

            set
            {
                _TextAlign = value; // set the text alignment..

                // these enumerations seem to be the same, but an explicit cast is required..
                dgvTextBoxPretend.DefaultCellStyle.Alignment = (DataGridViewContentAlignment)value;
            }
        }

        // the border width of the VPKSoft.VisualTextBox.VisualTextBox..
        private int _BorderWidth = 0; // no border by default..

        /// <summary>
        /// Gets or sets the border width of the VPKSoft.VisualTextBox.VisualTextBox.
        /// </summary>
        [Description("Gets or sets the border width of the VPKSoft.VisualTextBox.VisualTextBox")]
        [DefaultValue(0)] // no border by default..
        [Category("Appearance")]
        public int BorderWidth
        {
            get
            {
                return _BorderWidth; // return the current border width..
            }

            set
            {
                if (value < 0) // negative borders are not accepted in this time-space..
                {
                    throw new ArgumentOutOfRangeException("The value must be positive."); // .. so complain via an exception..
                }
                _BorderWidth = value; // set the new border width..
                Padding = new Padding(value); // set the hidden Padding property value so the border can actually show..
                Invalidate(); // .. setting the border requires a repaint..
            }
        }

        // the border color of the VPKSoft.VisualTextBox.VisualTextBox..
        private Color _BorderColor = SystemColors.AppWorkspace; // this seems to be the default value everywhere..

        /// <summary>
        /// Gets or sets the border color of the VPKSoft.VisualTextBox.VisualTextBox.
        /// </summary>
        [Description("Gets or sets the border color of the VPKSoft.VisualTextBox.VisualTextBox")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "AppWorkspace")] // AGAIN: this seems to be the default value everywhere..
        public Color BorderColor
        {
            get
            {
                return _BorderColor; // return the border's color..
            }

            set
            {
                _BorderColor = value; // set the border's color..
                Invalidate(); // .. setting the border requires a repaint.. 
            }
        }
        #endregion

        #region InternalLogic
        /// <summary>
        /// The OnPaint event must be overridden as the border can't be drawn otherwise.
        /// </summary>
        /// <param name="e">A PaintEventArgs class instance.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); // the base class can do most of the work..

            if (BorderWidth < 1) // there is now possible border to draw, so leave..
            {
                return;
            }

            ControlPaint.DrawBorder( // draw a border with the given properties for it..
                e.Graphics, e.ClipRectangle, BorderColor, BorderWidth,
                ButtonBorderStyle.Solid, BorderColor, BorderWidth,
                ButtonBorderStyle.Solid, BorderColor, BorderWidth,
                ButtonBorderStyle.Solid, BorderColor, BorderWidth,
                ButtonBorderStyle.Solid);
        }
        #endregion

        #region PublicEvents
        /// <summary>
        /// Occurs when the VPKSoft.VisualTextBox.VisualTextBox property value changes.
        /// </summary>
        [Category("PropertyChanged")]
        [Description("Occurs when the VPKSoft.VisualTextBox.VisualTextBox property value changes")]
        [Browsable(true)] // must ensure this is shows in the auto-complete, designer and is brow-sable..
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [EditorBrowsable(EditorBrowsableState.Always)] // .. END: must ensure this is shows in the auto-complete, designer and is brow-sable.
        public new event EventHandler TextChanged = null; // make the event a new one as the UserControl's version is somewhat difficult..
        #endregion

        #region HiddenProperties
        /// <summary>
        /// Gets or sets the border style of the user control.
        /// </summary>
        [Bindable(false)] // the user can't be allowed to access this BorderStyle property as the border is drawn "manually"..
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BorderStyle BorderStyle { get => base.BorderStyle; set => base.BorderStyle = value; }

        /// <summary>
        /// Gets or sets padding within the control.
        /// </summary>
        [Bindable(false)] // the user can't be allowed to access this Padding property as the border is drawn "manually"..
        [Browsable(false)] // .. and the Padding property value is required to do the manual drawing possible.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding { get => base.Padding; set => base.Padding = value; }
        #endregion

        #region InternalEvents
        // indicates if the text from changed with the control's internal logic to avoid 
        internal bool textChangedInternally = false;

        // the size of the control changed so resize the one cell..
        private void VisualTextBox_SizeChanged(object sender, EventArgs e)
        {
            if (dgvTextBoxPretend.RowCount > 0) // on a slower computer this caused an exception (!?)..
            {
                dgvTextBoxPretend.Rows[0].Height = dgvTextBoxPretend.Height;
            }
        }

        // the single cell's value changed so assign it to the Text property..
        private void dgvTextBoxPretend_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            textChangedInternally = true;
            // prepare that a null value may occur..
            this.Text = dgvTextBoxPretend.Rows[0].Cells[0].Value == null ?
                string.Empty : dgvTextBoxPretend.Rows[0].Cells[0].Value.ToString();
        }

        // the single cell's value "changed" so assign it to the Text property..
        private void dgvTextBoxPretend_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            textChangedInternally = true;
            this.Text = e.Value.ToString();
        }

        // a color value changed so re-set all the four colors that matter..
        private void VisualTextBox_ForeColorChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        // a color value changed so re-set all the four colors that matter..
        private void VisualTextBox_BackColorChanged(object sender, EventArgs e)
        {
            SetColors();
        }

        
        /// <summary>
        /// To simulate a "normal" text box the TextChanged will happen on each key press (the content of the cell changed)
        /// </summary>
        protected override bool ProcessKeyPreview(ref Message m)
        {
            dgvTextBoxPretend.CommitEdit(DataGridViewDataErrorContexts.Commit); // "commit" the changed edit value to the single and only cell..
            return base.ProcessKeyPreview(ref m);
        }
        


        const int WM_KEYUP = 0x0101;
        const int WM_SYSKEYUP = 0x0105;

        const int WM_KEYDOWN = 0x0100;
        const int WM_SYSKEYDOWN = 0x0104;

        /// <summary>
        /// To simulate a "normal" text box the TextChanged will happen on each key press (the content of the cell changed).
        /// </summary>
        /// <param name="msg">A System.Windows.Forms.Message, passed by reference, that represents the window message to process.</param>
        /// <param name="keyData"> One of the System.Windows.Forms.Keys values that represents the key to process.</param>
        /// <returns>True if the character was processed by the control; otherwise, false.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (char.IsLetterOrDigit((char)keyData) || keyData == Keys.Back)
            {               
                dgvTextBoxPretend.CommitEdit(DataGridViewDataErrorContexts.Commit); // "commit" the changed edit value to the single and only cell..
                return false; // for some reason in this case false value should be returned..
            }

            // raise events if the right message was gotten..
            if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
            {
                // raise a KeyDown event..
                base.OnKeyDown(new KeyEventArgs(keyData));
            }
            else if (msg.Msg == WM_KEYUP || msg.Msg == WM_SYSKEYUP)
            {
                // raise a KeyUp event..
                base.OnKeyUp(new KeyEventArgs(keyData));
            }

            // let the "base" control to process the key..
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // set the single cell's default style to match the control's color properties..
        private void SetColors()
        {
            dgvTextBoxPretend.DefaultCellStyle.BackColor = BackColor;
            dgvTextBoxPretend.DefaultCellStyle.SelectionBackColor = BackColor;
            dgvTextBoxPretend.DefaultCellStyle.SelectionForeColor = ForeColor;
            dgvTextBoxPretend.DefaultCellStyle.ForeColor = ForeColor;
        }
        #endregion

        private void dgvTextBoxPretend_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }
    }
}
