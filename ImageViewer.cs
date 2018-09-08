#region License
/*
VPKSoft.ImageViewer

A control that allows an image to be zoomed and padded via mouse or keyboard input.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.ImageViewer.

VPKSoft.ImageViewer is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.ImageViewer is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.ImageViewer.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space for the VPKSoft.ImageViewer control.
/// </summary>
namespace VPKSoft.ImageViewer
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A control that allows an image to be zoomed and padded via mouse or keyboard input.
    /// </summary>
    public partial class ImageViewer : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageViewer"/> class.
        /// </summary>
        public ImageViewer()
        {
            InitializeComponent();
            DoubleBuffered = true; // just to avoid flickering..
            // to capture keys such as left or right arrow the message must somewhat be manipulated..
            this.PreviewKeyDown += ImageViewer_PreviewKeyDown;

            // subscribe the Disposed event to unsubscribe from the non-generated event subscriptions such as this subscription ;-) ..
            this.Disposed += ImageViewer_Disposed;

            // enabled drag and drop operations on the control..
            pbMain.DragEnter += PbMain_DragEnter;
            pbMain.DragOver += PbMain_DragOver;
            pbMain.DragLeave += PbMain_DragLeave;
            pbMain.DragDrop += PbMain_DragDrop;
            // END: enabled drag and drop operations on the control..
        }

        #region DragDrop        
        /// <summary>
        /// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
        /// </summary>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("Gets or sets a value indicating whether the control can accept data that the user drags onto it.")]
        public override bool AllowDrop
        {
            get => base.AllowDrop;

            set
            {
                base.AllowDrop = value;
                pbMain.AllowDrop = value;
            }
        }

        /// <summary>
        /// Delegates the DragDrop event from the main picture box to the control.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="DragEventArgs"/> instance containing the event data.</param>
        private void PbMain_DragDrop(object sender, DragEventArgs e)
        {
            OnDragDrop(e);
        }

        /// <summary>
        /// Delegates the DragLeave event from the main picture box to the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PbMain_DragLeave(object sender, EventArgs e)
        {
            OnDragLeave(e);
        }

        /// <summary>
        /// Delegates the DragOver event from the main picture box to the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PbMain_DragOver(object sender, DragEventArgs e)
        {
            OnDragOver(e);
        }

        /// <summary>
        /// Delegates the DragEnter event from the main picture box to the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PbMain_DragEnter(object sender, DragEventArgs e)
        {
            OnDragEnter(e);
        }
        #endregion

        #region DelegateMouse
        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseMove event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        public void RaiseMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(CalcControlPoint(e));
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseDown event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        public void RaiseMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(CalcControlPoint(e));
        }

        /// <summary>
        /// Raises the System.Windows.Forms.Control.MouseUp event.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        public void RaiseMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(CalcControlPoint(e));
        }

        /// <summary>
        /// Computes the location of the specified screen point into client coordinates returning a new MouseEventArgs class instance.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <returns>A new <see cref="MouseEventArgs"/> instance with corrected coordinates.</returns>
        internal MouseEventArgs CalcControlPoint(MouseEventArgs e)
        {
            Point point = PointToClient(e.Location);
            return new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta);
        }

        /// <summary>
        /// Computes the location of the specified client point into screen coordinates.
        /// </summary>
        /// <param name="sender">The Control of which point is to be calculated to the screen coordinates.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        /// <returns>A new <see cref="MouseEventArgs"/> instance with corrected coordinates.</returns>
        public static MouseEventArgs CalcPointFromControl(Control sender, MouseEventArgs e)
        {
            Point point = sender.PointToScreen(e.Location);
            return new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta);
        }
        #endregion

        #region PrivateBehaviorMembers
        // an area where the image can be located in the control (zoomed or not zoomed)..
        private Rectangle imageRect = Rectangle.Empty;

        // a size of an image in the control's image area when zoomed to fit maintaining the aspect ratio of the image..
        private Size imageScaledSize = Size.Empty;

        // the actual size of an image assigned to the control..
        private Size imageSize = Size.Empty;

        // the previous mouse position over the control when the mouse is moving..
        private Point lastMousePos = Point.Empty;

        // the last "adjusted" point of the location of the image on the control.
        // this is used while zooming the image to avoid flickering to not to reposition the image to the a
        // same location over and over again..
        private Point lastAdjustedPoint = Point.Empty;

        // indicates if the zoomed panning of the image assigned to this control..
        private bool zoomPanOn = false;

        // indicates if the mouse move zoom / panning should be ignored..
        private bool ignoreMouse = false;
        #endregion

        #region Properties        
        /// <summary>
        /// Gets a value indicating whether the mouse zooming / panning is currently enabled.
        /// </summary>
        [Browsable(false)]
        public bool MouseZooming
        {
            get
            {
                return zoomPanOn && !ignoreMouse;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the keyboard zooming / panning is currently enabled.
        /// </summary>
        [Browsable(false)]
        public bool KeyboardZooming
        {
            get
            {
                return zoomPanOn && ignoreMouse;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the foreground color of the control.")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ControlText")]
        public override Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the background color for the control.")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Control")]
        public override Color BackColor
        {
            get => base.BackColor;

            set
            {
                base.BackColor = value;
                pbMain.BackColor = value; // set the picture box background color as well..
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a zoom function may leave the borders of the control.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a zoom function may leave the borders of the control; otherwise, <c>false</c>.
        /// </value>
        [Browsable(true)]
        [Description("Gets or sets padding within the control.")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool AllowLeaveArea { get; set; } = false;

        /// <summary>
        /// Gets or sets padding within the control.
        /// </summary>
        [Localizable(true)]
        [Category("Layout")]
        [Description("Gets or sets padding within the control.")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding { get => new Padding(0); set => Padding = new Padding(0); } // there will be no Padding (!)..

        /// <summary>
        /// Gets or sets the overlap margin which combined with the <see cref="AllowLeaveArea"/> property defines a constraint of how much the image can be allowed to pan beyond the control's area.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the overlap margin of how much the image can be allowed to pan beyond the control's area.")]
        [Category("Behavior")]
        [DefaultValue(20)]
        public int OverlapMargin { get; set; } = 20;

        /// <summary>
        /// Gets or sets the stepping of how much the image pans in the control's area when zooming with a keyboard.
        /// </summary>
        /// <value>
        /// The keyboard stepping.
        /// </value>
        [Browsable(true)]
        [Description("The stepping of how much the image pans in the control's area when zooming with a keyboard.")]
        [Category("Behavior")]
        [DefaultValue(10)]
        public int KeyboardStepping { get; set; } = 10;
        #endregion

        #region Calculations
        /// <summary>
        /// Gets a value indicating whether an image assigned to the picture box can be zoomed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an image assigned to the picture box can be zoomed or is already zoomed; otherwise, <c>false</c>.
        /// </value>
        private bool CanZoomImage
        {
            get
            {
                // can't zoom if there is no image..
                if (pbMain.Image == null)
                {
                    return false; // .. so just return false..
                }

                // return the value based on if the image is already zoomed or it can be zoomed..
                return Zoomed ? true : imageRect.Contains(lastMousePos) && zoomPanOn;
            }
        }

        /// <summary>
        /// Reset all the flags regarding to the image calculations.
        /// </summary>
        private void ResetFlags()
        {
            imageRect = Rectangle.Empty;
            imageScaledSize = Size.Empty;
            imageSize = Size.Empty;
            lastMousePos = Point.Empty;
            lastAdjustedPoint = Point.Empty;
            zoomPanOn = false;
            ignoreMouse = false;
        }

        /// <summary>
        /// Adjusts the mouse location so that a user may pan the image assigned to the control while zoomed..
        /// </summary>
        /// <param name="point">A Point to adjust for panning the image.</param>
        /// <returns>An adjusted point for the image's pan location.</returns>
        private Point AdjustInsideArea(Point point)
        {
            if (!Zoomed) // if not zoomed, there is nothing to adjust..
            {
                return point;
            }

            // if the image can be panned outside of the boundaries of this control no adjusting is required..
            if (AllowLeaveArea)
            {
                if (OverlapMargin > 0)
                {
                    // check for the leftmost coordinate with the overlap margin..
                    point.X = point.X + OverlapMargin > 0 ? OverlapMargin : point.X;

                    // check for the topmost coordinate with the overlap margin..
                    point.Y = point.Y + OverlapMargin > 0 ? OverlapMargin : point.Y; 

                    // check for the rightmost coordinate with the overlap margin..
                    if (point.X + pbMain.Width - OverlapMargin < ClientRectangle.Right)
                    {
                        // prevent a boundary overlap while panning more than the overlap margin defines..
                        point.X = -(pbMain.Width - ClientRectangle.Width) - OverlapMargin;
                    }

                    // check for the bottommost coordinate with overlap margin..
                    if (point.Y + pbMain.Height - OverlapMargin < ClientRectangle.Bottom)
                    {
                        // prevent a boundary overlap while panning more than the overlap margin defines..
                        point.Y = -(pbMain.Height - ClientRectangle.Height) - OverlapMargin;
                    }

                    // if the X boundary doesn't allow panning..
                    if (pbMain.Width < ClientRectangle.Width)
                    {
                        // ..recalculate the X point..
                        point.X = (ClientRectangle.Width - pbMain.Width) / 2;
                    }

                    // if the Y boundary doesn't allow panning..
                    if (pbMain.Height < ClientRectangle.Height)
                    {
                        // ..recalculate the Y point..
                        point.Y = (ClientRectangle.Height - pbMain.Height) / 2;
                    }
                }
                return point;
            }
            else // adjust the point so the image assigned to this control won't leave the control's boundaries..
            {
                point.X = point.X > 0 ? 0 : point.X; // check for the leftmost coordinate..
                point.Y = point.Y > 0 ? 0 : point.Y; // check for the topmost coordinate..

                // check for the rightmost coordinate..
                if (point.X + pbMain.Width < ClientRectangle.Right)
                {
                    // prevent a boundary overlap while panning..
                    point.X = -(pbMain.Width - ClientRectangle.Width);
                }

                // check for the bottommost coordinate..
                if (point.Y + pbMain.Height < ClientRectangle.Bottom)
                {
                    // prevent a boundary overlap while panning..
                    point.Y = -(pbMain.Height - ClientRectangle.Height);
                }

                // if the X boundary doesn't allow panning..
                if (pbMain.Width < ClientRectangle.Width)
                {
                    // ..recalculate the X point..
                    point.X = (ClientRectangle.Width - pbMain.Width) / 2;
                }

                // if the Y boundary doesn't allow panning..
                if (pbMain.Height < ClientRectangle.Height)
                {
                    // ..recalculate the Y point..
                    point.Y = (ClientRectangle.Height - pbMain.Height) / 2;
                }

                return point; // return the adjusted point..
            }
        }

        /// <summary>
        /// Gets the zoom ratio of the image assigned to the control.
        /// </summary>
        private double Zoom
        {
            get
            {
                // return one if there is no image..
                if (pbMain.Image == null)
                {
                    return 1;
                }

                // the zoom ratio can be either client size width divided by the image size width or
                // the client height divided by the image width as the aspect ratio will be kept for the image..
                return (double)ClientSize.Width / pbMain.Image.Width;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ImageViewer"/> is zoomed (with a mouse or a keyboard).
        /// </summary>
        /// <value>
        ///   <c>true</c> if zoomed; otherwise, <c>false</c>.
        /// </value>
        private bool Zoomed
        {
            get
            {
                // if the internal picture box is not docked and it's auto size mode is set to auto and zooming is initialized,
                // then true, otherwise false..
                return pbMain.Dock == DockStyle.None &&
                    pbMain.SizeMode == PictureBoxSizeMode.AutoSize &&
                    zoomPanOn;
            }

            set
            {
                // the zoomed value will be set: the flag indicating that zooming is either enabled or disabled is set and
                // the internal picture box dock style and size mode is set according to the value..
                zoomPanOn = value;
                pbMain.Dock = value ? DockStyle.None : DockStyle.Fill;
                pbMain.SizeMode = value ? PictureBoxSizeMode.AutoSize : PictureBoxSizeMode.Zoom;

                ignoreMouse = false;
            }
        }

        /// <summary>
        /// Gets an adjusted zoom point (location) for the internal picture box.
        /// </summary>
        private Point ImageZoomPoint
        {
            get
            {
                // if the image can be zoomed, calculate a Point for the position..
                if (CanZoomImage)
                {
                    // latest mouse coordinates are required to calculate a new location
                    // for the internal picture box..
                    double x = lastMousePos.X;
                    double y = lastMousePos.Y;

                    // divide the client area of the control by two..
                    double clientX = ClientSize.Width / 2;
                    double clientY = ClientSize.Height / 2;

                    // multiply the coordinates to scale according with the image's actual size
                    // compared to the control's client size..
                    x *= ((double)imageSize.Width / ClientSize.Width);
                    y *= ((double)imageSize.Height / ClientSize.Height);

                    // make the coordinates negative..
                    x *= -1;
                    y *= -1;

                    // adjust the coordinates to the relate to the center of the control..
                    x += clientX;
                    y += clientY;

                    // cast the coordinates to integers..
                    int returnX = (int)x;
                    int returnY = (int)y;

                    // adjust the calculated coordinates based on the AllowLeaveArea property value..
                    return AdjustInsideArea(new Point(returnX, returnY));
                }
                else
                {
                    return Point.Empty; // if zooming isn't possible then return an empty point..
                }
            }
        }

        /// <summary>
        /// Saves the dimension values of the image assigned to the control to help panning and zooming calculations.
        /// </summary>
        private void CalculateImageRect()
        {
            // only reset the calculations if there is no assigned image..
            if (pbMain.Image == null)
            {
                ResetFlags();
                return; // .. and return..
            }

            // calculate the zoomed dimensions of the image..
            int height = (int)(pbMain.Image.Height * Zoom);
            int width = (int)(pbMain.Image.Width * Zoom);

            // ..and save them..
            imageScaledSize = new Size(width, height);

            // save the actual size of the image..
            imageSize = pbMain.Image.Size;

            // calculate the image's location on the control's client area while not in zoom/pan mode..
            imageRect = new Rectangle((ClientSize.Width - width) / 2, (ClientSize.Height - height) / 2, width, height);
        }

        // when the control is resized the image's dimensions are required to be recalculated..
        private void ImageViewer_Resize(object sender, EventArgs e)
        {
            CalculateImageRect();
        }
        #endregion

        #region ReactToMouse
        // mouse down handling for the zooming and panning for the image assigned to this control..
        private void ImageViewer_MouseDown(object sender, MouseEventArgs e)
        {
            // set the flag that the zooming is requested..
            if (e.Button == MouseButtons.Right)
            {
                zoomPanOn = true;
            }

            // save the last mouse position..
            lastMousePos = e.Location;

            // if the image can be zoomed (is large enough)..
            if (CanZoomImage)
            {
                Zoomed = true; // set the Zoomed property value..
                pbMain.Location = ImageZoomPoint; // relocate the image => give it new coordinates..
            }
        }

        // set the Zoomed flag to false..
        private void ImageViewer_MouseUp(object sender, MouseEventArgs e)
        {
            Zoomed = false;
        }

        // the mouse is moving so if the image is "zoomed", relocate the image according to the mouse coordinates..
        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (ignoreMouse)
            {
                return;
            }

            // to avoid flickering, do nothing if the mouse position hasn't changed..
            if (lastMousePos.Equals(e.Location))
            {
                return;
            }
            lastMousePos = e.Location; // save the previous mouse location..

            if (CanZoomImage)
            {
                // to avoid flickering, prevent setting the same value to the picture box location..
                if (lastAdjustedPoint.Equals(ImageZoomPoint))
                {
                    return;
                }
                pbMain.Location = ImageZoomPoint; // get the new location..
                lastAdjustedPoint = pbMain.Location; // save the previous location..
            }
        }
        #endregion

        #region ExposePictureBox
        /// <summary>
        /// Gets or sets the path or URL for the image to display in the System.Windows.Forms.PictureBox.
        /// </summary>
        [DefaultValue(null)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [Category("Asynchronous")]
        [Description("Gets or sets the path or URL for the image to display in the System.Windows.Forms.PictureBox.")]
        [Browsable(true)]
        public string ImageLocation
        {
            get => pbMain.ImageLocation;
            set
            {
                pbMain.ImageLocation = value;
                Zoomed = false; // the image changed, so no zoom / pan..
                CalculateImageRect(); //  the image changed so the image's dimensions are required to be recalculated..
            }
        }

        /// <summary>
        /// Gets or sets the image that is displayed by System.Windows.Forms.PictureBox.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the image that is displayed by System.Windows.Forms.PictureBox.")]
        [Browsable(true)]
        public Image Image
        {
            get => pbMain.Image;

            set  
            {
                pbMain.Image = value;
                Zoomed = false; // the image changed, so no zoom / pan..
                CalculateImageRect(); //  the image changed so the image's dimensions are required to be recalculated..
            }
        }

        /// <summary>
        /// Indicates how the image is displayed.
        /// </summary>
        [Description("Indicates how the image is displayed.")]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Category("Appearance")]
        [Browsable(true)]
        public PictureBoxSizeMode SizeMode { get => pbMain.SizeMode; set => pbMain.SizeMode = value; }
        #endregion

        #region DelegateMouseToControl
        private void pbMain_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(pbMain.PointToScreen(e.Location));
            OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
        }

        private void pbMain_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        private void pbMain_MouseUp(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(pbMain.PointToScreen(e.Location));
            OnMouseUp(new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
        }

        private void pbMain_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = PointToClient(pbMain.PointToScreen(e.Location));
            OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, point.X, point.Y, e.Delta));
        }

        private void pbMain_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }
        #endregion

        #region ReactToKeyboard
        private void ImageViewer_KeyDown(object sender, KeyEventArgs e)
        {
            bool zoomPan = false; // an inside indicator if a key to pan the image was pressed..

            // cancel the keyboard zoom / pan mode..
            if (e.KeyCode == Keys.Space)
            {
                zoomPanOn = false; // indicate that the zooming / panning has been disabled..
                Zoomed = false; // disable the zoom..
                ignoreMouse = false; // indicate that reacting to mouse moving no longer is disabled..
                e.SuppressKeyPress = true; // suppress the key event to delegate further..
                e.Handled = true; // indicate that the key down was handled..
                return; // leave..
            }
            else if (e.KeyCode == Keys.Return) // set the flag that the zooming is requested..
            {
                zoomPanOn = true; // indicate that the zooming / panning has been enabled..
                lastMousePos = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
                // if the image can be zoomed (is large enough)..
                if (CanZoomImage)
                {
                    Zoomed = true; // set the Zoomed property value..
                    ignoreMouse = true; // indicate that the mouse movement on the control should be discarded..
                    pbMain.Location = ImageZoomPoint; // relocate the image => give it new coordinates..
                    zoomPan = true; // set the inside indicator if a key to pan the image was pressed..
                }
            }
            else if (e.KeyCode == Keys.Left) // the left key..
            {
                lastMousePos = new Point(lastMousePos.X - KeyboardStepping, lastMousePos.Y);
                zoomPan = true; // set the inside indicator if a key to pan the image was pressed..
            }
            else if (e.KeyCode == Keys.Right) // the right key..
            {
                lastMousePos = new Point(lastMousePos.X + KeyboardStepping, lastMousePos.Y);
                zoomPan = true; // set the inside indicator if a key to pan the image was pressed..
            }
            else if (e.KeyCode == Keys.Up) // the up key..
            {
                lastMousePos = new Point(lastMousePos.X, lastMousePos.Y - KeyboardStepping);
                zoomPan = true; // set the inside indicator if a key to pan the image was pressed..
            }
            else if (e.KeyCode == Keys.Down) // the down key..
            {
                lastMousePos = new Point(lastMousePos.X, lastMousePos.Y + KeyboardStepping);
                zoomPan = true; // set the inside indicator if a key to pan the image was pressed..
            }

            if (CanZoomImage && zoomPan) // if a key to pan or zoom was pressed and the image can be zoomed / panned..
            {
                // to avoid flickering, prevent setting the same value to the picture box location..
                if (lastAdjustedPoint.Equals(ImageZoomPoint))
                {
                    return;
                }
                pbMain.Location = ImageZoomPoint; // get the new location..
                lastAdjustedPoint = pbMain.Location; // save the previous location..
                e.SuppressKeyPress = true; // suppress the key event to delegate further..
                e.Handled = true; // indicate that the key down was handled..
            }
        }

        // allow the arrow keys to interact with the control..
        private void ImageViewer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up ||
                e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Left ||
                e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true; // .. by setting the IsInputKey property to true..
            }
        }
        #endregion

        #region InternalLogic
        // unsubscribe the events subscribed in the non-generated code..
        private void ImageViewer_Disposed(object sender, EventArgs e)
        {
            this.PreviewKeyDown -= ImageViewer_PreviewKeyDown;
            this.Disposed -= ImageViewer_Disposed;

            // unsubscribe the enabled drag and drop operations on the control..
            pbMain.DragEnter -= PbMain_DragEnter;
            pbMain.DragOver -= PbMain_DragOver;
            pbMain.DragLeave -= PbMain_DragLeave;
            pbMain.DragDrop -= PbMain_DragDrop;
            // END: unsubscribe enabled drag and drop operations on the control..

        }
        #endregion

        #region PublicMethods        
        /// <summary>
        /// Rotates the image assigned to this control 90 degrees clockwise.
        /// </summary>
        public void RotateImageRight()
        {
            if (Image != null)
            {
                Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                Zoomed = false; // the image changed, so no zoom / pan..
                CalculateImageRect(); //  the image changed so the image's dimensions are required to be recalculated..
                pbMain.Invalidate();
            }
        }

        /// <summary>
        /// Rotates the image assigned to this control 90 degrees counterclockwise.
        /// </summary>
        public void RotateImageLeft()
        {
            if (Image != null)
            {
                Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                Zoomed = false; // the image changed, so no zoom / pan..
                CalculateImageRect(); //  the image changed so the image's dimensions are required to be recalculated..
                pbMain.Invalidate();
            }
        }
        #endregion
    }
}
