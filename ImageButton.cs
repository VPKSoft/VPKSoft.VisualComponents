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
using VPKSoft.VisualUtils; // (C): http://www.vpksoft.net/, GNU Lesser General Public License Version 3

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the ImageButton class.
/// </summary>
namespace VPKSoft.ImageButton
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A button control which shows an image on the left site of the text.
    /// </summary>
    [DefaultEvent("Click")] // the default event
    public partial class ImageButton : UserControl
    {
        /// <summary>
        /// The image button constructor.
        /// </summary>
        public ImageButton()
        {
            InitializeComponent();
            lbButtonText.Text = base.Name;
        }

        // do note that this region was going to be named as GUILogic - but a "typo" occurred so I decided to leave it as it is..
        // This is NOT meant to be an offense to anyone/anything (PS. I'm gay too.. FOR REAL).. 
        // Have fun as you are coding and as you surely have read the license - NO LAW SUITS AGAINS ME..
        #region GAYLogic
        // if the control's text property was set via a property
        private bool textSetFromProperty = false;

        /// <summary>
        /// Resize the button's image to a square.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">An EventArgs class instance.</param>
        private void ImageButton_ClientSizeChanged(object sender, EventArgs e)
        {
            ScaleThis(); // resize the button's image to a square..
        }

        /// <summary>
        /// Resize the button's image to a square.
        /// </summary>
        private void ScaleThis()
        {
            int colSize0 = Math.Min(tlpMain.Height, tlpMain.Width) * 70 / 100; // do use the same scaling to the image as to the text..
            tlpMain.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, colSize0);
            tlpMain.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 100.0f);

            int fh = lbButtonText.ClientSize.Height * 70 / 100; // calculate the font size to fit the control to be "suitable.."

            lbButtonText.Font = new Font(Font.FontFamily, fh, Font.Style, GraphicsUnit.Pixel); // assign a ne font.
            SetEnabledState(); // set the image and label color depending on the Enabled property value..
        }
        #endregion

        #region Properties
        // An image to show before the button's text..
        private Image _buttonImage = null;

        /// <summary>
        /// A button's image.
        /// </summary>
        [Description("An image to show before the button's text")]
        [Category("ImageButton")]
        [DefaultValue(null)]
        public Image ButtonImage
        {
            get
            {
                return _buttonImage;
            }

            set
            {
                _buttonImage = value;
                pnButtonImage.BackgroundImage = value;
                ScaleThis(); // resize the button's image to a square..
            }
        }

        // a value indicating if the control is enabled or not.. the base.Enable is dropped with a new keyword..
        private bool _enabled = true;

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        [Description("Gets or sets a value indicating whether the control can respond to user interaction")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public new bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                SetEnabledState(); // set the image and label color depending on the Enabled property value..
            }
        }

        // The name of the control..
        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new string Name
        {
            get
            {
                return base.Name;
            }

            set
            {
                base.Name = value;
                if (!textSetFromProperty) // Set the button's text to the control's name if not set by the Text property..
                {
                    lbButtonText.Text = value;
                    ScaleThis(); // resize the button's image to a square..
                }
            }
        }

        // Set the ForeColor for the overlaid controls as well..


        /// <summary>
        ///  Gets or sets the foreground color of the control.
        /// </summary>
        [Description("Gets or sets the foreground color of the control")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ControlText")]
        public override Color ForeColor
        {
            get => base.ForeColor;

            set
            {
                base.ForeColor = value;
                lbButtonText.ForeColor = value;
                pnButtonImage.ForeColor = value;
            }                
        }

        // Set the BackColor for the overlaid controls as well..

        /// <summary>
        /// Gets or sets a value indicating whether control will raise click events. Meaning this control can act as a label as well.
        /// </summary>
        [Description("Gets or sets a value indicating whether control will raise click events. Meaning this control can act as a label as well")]
        [Category("ImageButton")]
        [DefaultValue(false)]
        public override Color BackColor
        {
            get => base.BackColor;

            set
            {
                base.BackColor = value;
                lbButtonText.BackColor = value;
                pnButtonImage.BackColor = value;
            }
        }

        // Set the "button's" text as well


        /// <summary>
        /// Gets or sets the text shown in the ImageButton.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => base.Text;
            set
            {
                textSetFromProperty = true; // indicate that the button's text was set by a property so the Name property setting don't override the text..
                lbButtonText.Text = value;
                base.Text = value;
                ScaleThis(); // resize the button's image to a square..
            }
        }

        // A value that is returned to the parent form when the button is clicked..
        private DialogResult _dialogResult = DialogResult.None;

        /// <summary>
        /// Gets or sets a value that is returned to the parent form when the button is clicked.
        /// </summary>
        [Description("Gets or sets a value that is returned to the parent form when the button is clicked")]
        [Category("Behavior")]
        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult
        {
            get
            {
                return _dialogResult;
            }

            set
            {
                _dialogResult = value;
            }
        }

        private bool _Label = false;

        /// <summary>
        /// Gets or sets a value indicating whether control will raise click events. Meaning this control can act as a label as well.
        /// </summary>
        [Description("Gets or sets a value indicating whether control will raise click events. Meaning this control can act as a label as well")]
        [Category("ImageButton")]
        [DefaultValue(false)]
        public bool Label
        {
            get
            {
                return _Label;
            }

            set
            {
                _Label = value;
            }
        }
        #endregion

        #region InternalLogic
        /// <summary>
        /// Cause the overlaid components to generate a Click event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Event arguments of the event.</param>
        private void baseClick(object sender, EventArgs e)
        {
            // if not enabled or if the button's property Label is set to true, the button can't be clicked..
            if (!Enabled || Label)
            {
                return; // .. so just return..
            }

            if (DialogResult != DialogResult.None) // we even have a dialog result!
            {
                if (this.ParentForm != null) // if a parent form exists..
                {
                    this.ParentForm.DialogResult = DialogResult; // ..set it's DialogResult if != DialogResult.None
                }
            }
            base.OnClick(e); // cause a Click event..
        }

        /// <summary>
        /// Toggle the "enabled" state for this ImageButton control.
        /// </summary>
        internal void SetEnabledState()
        {
            pnButtonImage.BackgroundImage = _enabled ? ButtonImage : UtilsMisc.MakeGrayscale3(ButtonImage);
            lbButtonText.ForeColor = _enabled ? ForeColor : Color.Gray;
        }
        #endregion
    }
}
