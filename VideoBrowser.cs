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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VPKSoft.MessageHelper;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
                              /// <summary>
                              /// A name space for the VPKSoft.VideoBrowser control.
                              /// </summary>
namespace VPKSoft.VideoBrowser
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A component which displays a list of videos with their still images and descriptions.
    /// </summary>
    [DefaultEvent("PlaybackRequested")]
    public partial class VideoBrowser : UserControl
    {
        /// <summary>
        /// A constructor for the VPKSoft.VideoBrowser control.
        /// </summary>
        public VideoBrowser()
        {
            InitializeComponent();
            base.Padding = new Padding(0);

            // set the fonts..
            FontLarge = FontLarge;
            FontMedium = FontMedium;
            FontSmall = FontSmall;
            // END: set the fonts..

            ResizeButtons(); // scale the buttons..
            DoubleBuffered = true; // try to avoid flickering..
            UpdateHighlight(); // update the highlighted video details..
        }

        #region PrivateMembers
        /// <summary>
        /// A string to measure font sizes.
        /// </summary>
        private const string MeasureText = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö£€%[]$@ÂÊÎÔÛâêîôûÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÁÉÍÓÚáéíóúÃÕãõ '|?+\\/{}½§01234567890+<>_-:;*&¤#\"!";

        // a list of TableLayoutPanel class instances which indicates a video in the video list..
        private List<TableLayoutPanel> panels = new List<TableLayoutPanel>();
        #endregion

        #region PublicEvents
        /// <summary>
        /// A delegate for the QueryPath event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">QueryPathEventArgs class instance passed to the event.</param>
        public delegate void OnQueryPath(object sender, QueryPathEventArgs e);

        /// <summary>
        /// An event that is raised if a user requests for more videos to be added to the collection.
        /// </summary>
        [Category("VideoBrowser")]
        [Browsable(true)]
        [Description("An event that is raised if a user requests for more videos to be added to the collection.")]
        public event OnQueryPath QueryPath = null;

        /// <summary>
        /// A delegate for the DeleteRequested event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">TBDbDetailExtArgs class instance passed to the event.</param>
        public delegate void OnDeleteRequested(object sender, TBDbDetailExtArgs e);

        /// <summary>
        /// An event that is raised when a user requests an item to be deleted from the video collection.
        /// </summary>
        [Category("VideoBrowser")]
        [Browsable(true)]
        [Description("An event that is raised when a user requests an item to be deleted from the video collection.")]
        public event OnDeleteRequested DeleteRequested = null;

        /// <summary>
        /// A delegate for the PlaybackRequested event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">TBDbDetailExtArgs class instance passed to the event.</param>
        public delegate void OnPlaybackRequested(object sender, TBDbDetailExtArgs e);

        /// <summary>
        /// An event that is raised if a user requests a selected video to be played.
        /// </summary>
        [Category("VideoBrowser")]
        [Browsable(true)]
        [Description("An event that is raised if a user requests a selected video to be played.")]
        public event OnPlaybackRequested PlaybackRequested = null;
        #endregion

        #region HiddenProperties     
        /// <summary>
        /// Gets or sets padding within the control.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding // no padding is required/accepted..
        {
            get
            {
                return new Padding(0); // ..so it will always be read-only and tries to be hidden..
            }
        }
        #endregion

        #region GUILogic
        /// <summary>
        /// List the videos on the Videos collection visually.
        /// </summary>
        private void ListVideos()
        {
            ClearVideoControls(); // first clear the previous controls from the main TableLayout panel..

            DisposeList(); // cleanup.. 


            // loop though the Videos list..
            foreach (TMDbDetailExt video in Videos)
            {
                // create a main TableLayoutPanel for an item in the list..
                TableLayoutPanel moviePanel = new TableLayoutPanel
                {
                    RowCount = 2, // two rows..
                    ColumnCount = 1, // one column..
                    Dock = DockStyle.Fill, // set to fill the area..
                    Cursor = Cursors.Hand, // indicate that the item is click-able..
                    Tag = video, // tag the control with the "meta data"..
                };

                // create row and column styles for the main TableLayoutPanel..
                moviePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // the first row will scale..

                // measure the absolute height for the second row..
                int fHeight = TextRenderer.MeasureText(MeasureText, FontSmall).Height;

                moviePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, fHeight)); // set the second row style..
                moviePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); // set the only column style..

                //tlpPreviewLarge.RowStyles[1] = new RowStyle(SizeType.Absolute, fHeight);

                // this is the border panel for the item..
                Panel moviePanelImageBorder = new Panel
                {
                    Dock = DockStyle.Fill, // dock to fill..
                    Padding = new Padding(6), // no border without padding..
                    BackColor = ItemBorderColor, // set the color for the border..
                    Margin = new Padding(0), // no margin..
                    Name = "Border", // indicate the border panel by giving it a name to help highlight it if it's selected..
                    Cursor = Cursors.Hand, // indicate that the item is click-able..
                    Tag = video, // tag the control with the "meta data"..                    
                };

                // this is the panel for the video's still image..
                Panel moviePanelImage = new Panel
                {
                    BackgroundImageLayout = ImageLayout.Zoom, // set the image to zoom..
                    Dock = DockStyle.Fill, // dock to fill..
                    BackgroundImage = video.Image, // set the image..
                    BackColor = Color.Black, // this one is black..
                    Cursor = Cursors.Hand, // indicate that the item is click-able..
                    Tag = video // tag the control with the "meta data"..
                };

                // add the video's still image panel to the border panel..
                moviePanelImageBorder.Controls.Add(moviePanelImage);

                // create a label for the video's title..
                Label moviePanelLabel = new Label
                {
                    Dock = DockStyle.Fill, // dock to fill..
                    TextAlign = ContentAlignment.MiddleCenter, // set the text's align..
                    Text = video.Title, // set the text..
                    AutoSize = false, // no auto size as the text's align is on the middle of the label..
                    AutoEllipsis = true, // set the auto ellipsis to true..
                    ForeColor = ColorSmallFont, // Set the font's color..
                    Font = FontSmall, // set the font..
                    Cursor = Cursors.Hand, // indicate that the item is click-able..
                    Tag = video // tag the control with the "meta data"..
                };

                // add the border panel to the main TableLayoutPanel..
                moviePanel.Controls.Add(moviePanelImageBorder, 0, 0);

                // add the title label to the main TableLayoutPanel..
                moviePanel.Controls.Add(moviePanelLabel, 0, 1);

                // add the main TableLayoutPanel control to the panels list..
                panels.Add(moviePanel);

                moviePanel.Click += MoviePanel_Click; // subscribe to mouse click event..
                moviePanelImageBorder.Click += MoviePanel_Click;
                moviePanelImage.Click += MoviePanel_Click;
                moviePanelLabel.Click += MoviePanel_Click; // END: subscribe to mouse click event..
            }

            // list the panels which should be shown based on the indices..
            UpdateList();
        }

        /// <summary>
        /// Gets controls and sub-controls owned by a given control.
        /// </summary>
        /// <param name="mainControl">The control of which sub-controls to get.</param>
        /// <returns>A list of Controls containing the main control and it's sub-controls.</returns>
        private List<Control> GetControls(Control mainControl)
        {
            List<Control> result = new List<Control>
            {
                mainControl // the main control will also be in the list..
            }; // the return value..
            foreach (Control control in mainControl.Controls)
            {
                result.AddRange(GetControls(control)); // some recursion..
            }
            return result; // return the result..
        }

        /// <summary>
        /// Detaches mouse click event handlers of given TableLayoutPanel control and it's sub-controls.
        /// </summary>
        /// <param name="panel">A TableLayoutPanel control to detach the event handlers from.</param>
        private void DetachDetailClickHandlers(TableLayoutPanel panel)
        {
            List<Control> controls = GetControls(panel); // get all the controls on the TableLayoutPanel..
            foreach (Control control in controls) // loop through the controls..
            {
                control.Click -= MoviePanel_Click; // detach mouse click event handlers..
            }
        }

        // this is called when a video item in the list is clicked..
        private void MoviePanel_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            TMDbDetailExt detailExt = (TMDbDetailExt)control.Tag;
            UpdateHighlight(detailExt); // highlight the clicked item and display it's details..
        }

        private int lastVideoIndex = -1; // the previous starting index of an item to display..
        private int lastVideoDetailIndex = -1; // the previous item index of which details to display..

        /// <summary>
        /// Updates the list of visible videos on the control.
        /// </summary>
        private void UpdateList()
        {
            if (VideoIndex == -1) // nothing to do if the index is negative..
            {
                lastVideoIndex = VideoIndex;
                return; // .. so just return..
            }

            if (VideoDetailIndex <= 0 || VideoDetailIndex <= 0)
            {
                pnPrevious.BackgroundImage = VisualUtils.UtilsMisc.MakeGrayscale3(_ImagePreviousButton);
            }
            else
            {
                pnPrevious.BackgroundImage = _ImagePreviousButton;
            }

            if (VideoDetailIndex >= Videos.Count)
            {
                pnNext.BackgroundImage = VisualUtils.UtilsMisc.MakeGrayscale3(_ImageNextButton);
            }
            else
            {
                pnNext.BackgroundImage = _ImageNextButton;
            }


            // if the video index was changed from the previous time..
            if (lastVideoIndex != VideoIndex)
            {
                SuspendLayout(); // avoid flickering..
                int layOutIndex = 1; // the index to start to layout the video item panels..
                ClearVideoControls(); // clear the previous detail panels..

                for (int i = VideoIndex; i < VideoIndex + 5; i++) // only five video item panels will be visible at a time..
                {
                    // don't allow "overflow" to happen..
                    if (i >= Videos.Count || i >= panels.Count || panels.Count == 0)
                    {
                        break;
                    }

                    // add the video item panel to the list..
                    tlpMain.Controls.Add(panels[i], layOutIndex++, 2);
                }
                ResumeLayout(); // END: avoid flickering..
            }

            // if the last video detail index was changed from the previous time..
            if (lastVideoDetailIndex != VideoDetailIndex)
            {
                UpdateHighlight(); // update the highlighted video and display it's details..
            }

            lastVideoDetailIndex = VideoDetailIndex; // save the previous detail index..
            lastVideoIndex = VideoIndex; // save the previous index..
        }

        /// <summary>
        /// Clears the highlighted (selected) video item details.
        /// </summary>
        private void ClearHighlight()
        {
            // just clear all..
            lbDescriptionValue.Text = string.Empty; 
            pnPreviewImage.BackgroundImage = null;
            lbPreviewImage.Text = string.Empty;
            lbLengthCaptionAndValue.Text = string.Empty;
        }

        /// <summary>
        /// Updates the highlighted (selected) video item and displays it's details.
        /// </summary>
        /// <param name="detailExt">An optional TMDbDetailExt class instance of which index to use.</param>
        private void UpdateHighlight(TMDbDetailExt detailExt = null)
        {
            if (detailExt != null) // if the optional parameter was given..
            {
                VideoDetailIndex = Videos.IndexOf(detailExt); // get it's index..
            }

            // loop through the video detail panels on the control's video list..
            for (int i = VideoIndex; i < VideoIndex + 5; i++)
            {
                // don't allow "overflow" to happen..
                if (i >= Videos.Count || i >= panels.Count || panels.Count == 0)
                {
                    break;
                }

                // find the border control..
                Control[] controls = panels[i].Controls.Find("Border", false);
                if (controls.Length > 0) // ..and if the border control was found..
                {
                    Panel panel = (Panel)controls[0];
                    // set it's background color to match either highlight or normal border color.. 
                    panel.BackColor = i == VideoDetailIndex ? HighLilightColor : ItemBorderColor;
                }

                // if the highlighted index is in the collection..
                if (VideoDetailIndex == i)
                {
                    // display the details of the highlighted video..
                    pnPreviewImage.BackgroundImage = Videos[VideoDetailIndex].Image; // set the larger image..
                    lbPreviewImage.Text = Videos[VideoDetailIndex].Title; // set the title..

                    // set the description..
                    lbDescriptionValue.Text =
                        string.IsNullOrEmpty(Videos[VideoDetailIndex].DetailDescription) ?
                            Videos[VideoDetailIndex].Description : Videos[VideoDetailIndex].DetailDescription;

                    // display the video's length in hours and minutes..
                    lbLengthCaptionAndValue.Text = LengthCaption + " " + Videos[VideoDetailIndex].Duration.ToString(@"hh\:mm");
                }
            }
        }

        /// <summary>
        /// Clears the controls (the small video file images from the lower list).
        /// </summary>
        private void ClearVideoControls(bool suspend = false)
        {
            SuspendLayout(); // to avoid flickering..
            foreach (TableLayoutPanel panel in panels)
            {
                foreach (Control control in tlpMain.Controls) // clear the controls..
                {
                    if (control.Equals(panel)) // ..that actually resides on the main TableLayoutPanel..
                    {
                        tlpMain.Controls.Remove(panel);
                    }
                }
            }
            ResumeLayout(); // resume the layout again..
        }

        /// <summary>
        /// If the control size is changed the do some re-layout..
        /// </summary>
        private void ResizeButtons()
        {
            pbCloseParentForm.Height = pbCloseParentForm.Width;
            pbAddMovieRequest.Height = pbAddMovieRequest.Width;
            pbDeleteMovie.Height = pbDeleteMovie.Width;
        }

        // the control was resized..
        private void VideoBrowser_Resize(object sender, EventArgs e)
        {
            ResizeButtons(); // ..so re-layout some child controls..
        }

        /// <summary>
        /// Unsubscribes the event handlers of the video list and clears the video list afterwards.
        /// </summary>
        public void DisposeList()
        {
            try
            {
                foreach (TableLayoutPanel panel in panels)
                {
                    DetachDetailClickHandlers(panel);
                }
                panels.Clear(); // clear the panels list..   
            }
            catch // just to be sure..
            {

            }
        }

        /// <summary>
        /// A method to close the underlying form the control is on.
        /// </summary>
        private void CloseForm()
        {
            try
            {
                FindForm()?.Close(); // if found (!= null), then close..
                DisposeList(); // cleanup.. 
            }
            catch // just to be sure..
            {

            }
        }

        /// <summary>
        /// Override the WndProc to capture mouse wheel movement. 
        /// </summary>
        /// <param name="m">A reference to a Message class instance.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == MessageHelper.MessageHelper.WM_MOUSEWHEEL) // only respond to mouse wheel movement..
            {
                if (m.WParamHiWord() > 0)
                {
                    VideoDetailIndex++; // user requested the next video's details to be shown..
                }
                else
                {
                    VideoDetailIndex--; // user requested the previous video's details to be shown..
                }
            }
            base.WndProc(ref m); // direct to base class..
        }

        /// <summary>
        /// Override the OnPreviewKeyDown to capture keyboard events.
        /// </summary>
        /// <param name="e">A PreviewKeyDownEventArgs class instance.</param>
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                VideoDetailIndex--; // user requested the previous video's details to be shown..
            }
            else if (e.KeyCode == Keys.Right)
            {
                VideoDetailIndex++; // user requested the next video's details to be shown..
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CloseForm(); // user requested the form to be closed..
            }
            else if (e.KeyCode == Keys.Add || e.KeyValue == 187)
            {
                RaiseQueryPath(); // a user requested for a directory to be enumerated for video files..
            }
            else if (e.KeyCode == Keys.Delete)
            {
                RaiseDeleteRequested(); // a user requested for a deletion of a video file from the list..
            }
            else if (e.KeyCode == Keys.Return)
            {
                RaisePlaybackRequested(); // a user requested for a playback of the selected video file..
            }
            else
            {
                base.OnPreviewKeyDown(e);
            }
        }

        private void pbCloseParentForm_Click(object sender, EventArgs e)
        {
            CloseForm(); // user requested the form to be closed..
        }

        private void pnNext_Click(object sender, EventArgs e)
        {
            VideoDetailIndex++; // user requested the next video's details to be shown..
        }

        private void pnPrevious_Click(object sender, EventArgs e)
        {
            VideoDetailIndex--; // user requested the previous video's details to be shown..
        }

        private void pbAddMovieRequest_Click(object sender, EventArgs e)
        {
            RaiseQueryPath(); // a user requested for a directory to be enumerated for video files..
        }

        private void pbDeleteMovie_Click(object sender, EventArgs e)
        {
            RaiseDeleteRequested(); // a user requested for a deletion of a video file from the list..
        }

        private void playVideoClick(object sender, EventArgs e)
        {
            RaisePlaybackRequested(); // a user requested for a playback of the selected video file..
        }

        /// <summary>
        /// Raises the QueryPath event.
        /// </summary>
        private void RaiseQueryPath()
        {
            QueryPathEventArgs e = new QueryPathEventArgs()
            {
                Context = this.Context
            };
            QueryPath?.Invoke(this, e);

            if (!e.Cancel)
            {
                Videos.AddRange(e.TMDbDetails);
            }
        }

        /// <summary>
        /// Raises the DeleteRequested event.
        /// </summary>
        private void RaiseDeleteRequested()
        {
            if (VideoDetailIndex >= 0 && VideoDetailIndex < Videos.Count)
            {
                TBDbDetailExtArgs e = new TBDbDetailExtArgs()
                {
                    TMDbDetailExt = Videos[VideoDetailIndex]
                };

                if (DeleteRequested != null)
                {
                    DeleteRequested(this, e);
                    if (!e.Cancel)
                    {
                        Videos.Remove(e.TMDbDetailExt);
                    }
                }
            }
        }

        /// <summary>
        /// Raises the PlaybackRequested event.
        /// </summary>
        private void RaisePlaybackRequested()
        {            
            if (VideoDetailIndex >= 0 && VideoDetailIndex < Videos.Count)
            {
                TBDbDetailExtArgs e = new TBDbDetailExtArgs()
                {
                    TMDbDetailExt = Videos[VideoDetailIndex],
                    Cancel = true
                };

                PlaybackRequested?.Invoke(this, e);
                if (!e.Cancel) // the form the control is an was requested to be closed..
                {
                    CloseForm(); // ..so close the form..
                }
            }
        }
        #endregion

        #region PublicProperties        
        // a video detail index which detailed properties are shown..
        private int _VideoDetailIndex = 2;

        /// <summary>
        /// Gets or sets the video detail index which detailed properties are shown.
        /// </summary>
        [Browsable(false)]
        public int VideoDetailIndex
        {
            get
            {
                // some comparison is required so an invalid value won't be returned..
                return _VideoDetailIndex >= Videos.Count ? Videos.Count - 1 : (_VideoDetailIndex == -1 && Videos.Count > 0 ? 0 : _VideoDetailIndex);
            }

            set
            {
                // save the previous value..
                int lastValue = _VideoDetailIndex;

                // if no change has occurred then return..
                if (value == _VideoDetailIndex)
                {
                    return;
                }

                // the value can't be larger than the amount of total video details in the list..
                if (value >= Videos.Count)
                {
                    _VideoDetailIndex = Videos.Count - 1;
                }
                else // ..otherwise the value will be accepted..
                {
                    _VideoDetailIndex = value;
                }

                // don't allow negative values if there are videos in the list..
                if (value < 0 && Videos.Count > 0)
                {
                    return;
                }

                // don't allow larger values than there are videos in the list..
                if (value >= Videos.Count)
                {
                    return;
                }

                // indicate if the value is increasing or decreasing..
                bool dec = lastValue > value;

                // if decreasing and the display list's start index is larger than the detail index..
                if (dec && VideoIndex > value)
                {
                    VideoIndex--; // ..then decrease the list's start index..
                }
                // if increasing and the display list's end index is smaller than the detail index..
                else if (!dec && value >= VideoIndex + 5)
                {
                    VideoIndex++; // ..then increase the list's start index..
                }

                // if the value was actually changed, update the list..
                if (lastValue != value)
                {
                    UpdateList();
                }

                if (_VideoDetailIndex == -1)
                {
                    ClearHighlight(); // nothing to see with a negative index..
                }
            }
        }

        private int _VideoIndex = 0;

        /// <summary>
        /// Gets or sets a start index from where the video list is shown.
        /// </summary>
        [Browsable(false)]
        public int VideoIndex
        {
            get
            {
                // some comparison is required so an invalid value won't be returned..
                return _VideoIndex >= Videos.Count ? Videos.Count - 1 : (_VideoIndex == -1 && Videos.Count > 0 ? 0 : _VideoIndex);
            }

            set
            {
                // save the previous value..
                int lastValue = _VideoIndex;

                // if no change has occurred then return..
                if (value == _VideoIndex)
                {
                    return;
                }

                // don't allow values larger than the amount of videos + five in the list..
                if (value + 5 > Videos.Count)
                {
                    return;
                }

                // don't allow negative values if there are videos in the list..
                if (value < 0 && Videos.Count > 0)
                {
                    return;
                }

                // don't allow larger values than there are videos in the list..
                if (value >= Videos.Count)
                {
                    _VideoIndex = Videos.Count - 1;
                }
                // set the video index..
                else
                {
                    _VideoIndex = value;
                }

                // if the value was actually changed, update the list..
                if (lastValue != value)
                {
                    UpdateList();
                }
            }
        }

        // a list of TMDbDetailExt class instances to act as a video list..
        private List<TMDbDetailExt> _Videos = new List<TMDbDetailExt>();

        /// <summary>
        /// A list of TMDbDetailExt class instances for items to be shown on the control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public List<TMDbDetailExt> Videos
        {
            get
            {
                return _Videos;
            }

            set
            {
                _Videos = value;
                ListVideos();
            }
        }

        /// <summary>
        /// Gets or sets a voluntary context string which is passed as property QueryPathEventArgs.Context value on the QueryPath event.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets a voluntary context string which is passed as property QueryPathEventArgs.Context value on the QueryPath event.")]
        [Category("Behavior")]
        public string Context { get; set; } = string.Empty; // nothing is the default..

        // a caption used to display the length of a video in hours and minutes..
        private string _LengthCaption = "Length:";

        /// <summary>
        /// Gets or sets the caption used to display the length of a video in hours and minutes.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the caption used to display the length of a video in hours and minutes.")]
        [Category("Appearance")]
        [DefaultValue("Length:")]
        public string LengthCaption
        {
            get => _LengthCaption;

            set
            {
                if (value != _LengthCaption)
                {
                    _LengthCaption = value;
                    UpdateHighlight();
                }
            }
        }

        // the caption used for a video description..
        private string _DescriptionCaption = "Description";

        /// <summary>
        /// Gets or sets the caption used for a video description.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the caption used for a video's description.")]
        [Category("Appearance")]
        [DefaultValue("Description")]
        public string DescriptionCaption
        {
            get => _DescriptionCaption;

            set
            {
                if (value != _DescriptionCaption)
                {
                    _DescriptionCaption = value;
                    lbDescriptionCaption.Text = value;
                }
            }
        }

        // the font used on the control for small text..
        private Font fontSmall = null;

        /// <summary>
        /// Gets or sets the font used on the control for small text, such as the small name description in the video file list.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the font used on the control for small text, such as the small name description in the video file list.")]
        [Category("Appearance")]
        public Font FontSmall
        {
            get
            {
                if (fontSmall == null)
                {
                    fontSmall = base.Font;
                }

                return fontSmall;
            }

            set
            {
                base.Font = value;
                if (value != fontSmall)
                {
                    ListVideos();
                }
                fontSmall = value;
            }
        }

        private Color _ColorSmallFont = Color.White;

        /// <summary>
        /// Gets or sets the color of the SmallFont property.
        /// </summary>
        [Description("Gets or sets the border color of items in on the control.")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")] // the default value..
        [Browsable(true)]
        public Color ColorSmallFont
        {
            get => _ColorSmallFont;

            set
            {
                if (value != _ColorSmallFont)
                {
                    _ColorSmallFont = value;

                    ListVideos();
                }
            }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control. This is the synonym for the FontSmall property.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the font of the text displayed by the control.")]
        [Category("Appearance")]
        public override Font Font { get => base.Font; set => FontSmall = value; }

        // the font used on the control for large text..
        private Font fontLarge = null;

        /// <summary>
        /// Gets or sets the font used on the control for large text, the length of the video, the description text caption and the title caption.
        /// </summary>
        [Description("Gets or sets the font used on the control for large text.")]
        [Category("Appearance")]
        [Browsable(true)]
        public Font FontLarge
        {
            get
            {
                if (fontLarge == null)
                {
                    fontLarge = new Font(base.Font.FontFamily, Font.SizeInPoints * 2, Font.Style, GraphicsUnit.Point);
                }

                return fontLarge;
            }

            set
            {
                if (value != fontLarge)
                {
                    fontLarge = value;
                    int fHeight = System.Windows.Forms.TextRenderer.MeasureText(MeasureText, value).Height;

                    tlpPreviewLarge.RowStyles[1] = new RowStyle(SizeType.Absolute, fHeight + lbPreviewImage.Padding.Size.Height);
                    tlpDetailHolder.RowStyles[0] = new RowStyle(SizeType.Absolute, fHeight);
                    tlpDetailHolder.RowStyles[9] = new RowStyle(SizeType.Absolute, fHeight);

                    lbPreviewImage.Font = value;
                    lbDescriptionCaption.Font = value;
                    lbLengthCaptionAndValue.Font = value;
                }
            }
        }

        // the font used on the control for medium text..
        private Font fontMedium = null;

        /// <summary>
        /// Gets or sets the font used on the control for medium text, the video's description.
        /// </summary>
        [Description("Gets or sets the font used on the control for medium text, the video's description.")]
        [Category("Appearance")]
        [Browsable(true)]
        public Font FontMedium
        {
            get
            {
                if (fontMedium == null)
                {
                    fontMedium = new Font(base.Font.FontFamily, Font.SizeInPoints * 1.5f, Font.Style, GraphicsUnit.Point);
                }

                return fontMedium;
            }

            set
            {
                if (value != fontMedium)
                {
                    fontMedium = value;
                    lbDescriptionValue.Font = value;
                }
            }
        }

        // the highlight color of as selected video in the list..
        private Color _HighLilightColor = Color.White;

        /// <summary>
        /// Gets or sets the highlight color of as selected video in the list.
        /// </summary>
        [Description("Gets or sets the highlight color of as selected video in the list.")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")] // the default value..
        [Browsable(true)]
        public Color HighLilightColor
        {
            get => _HighLilightColor;

            set
            {
                if (value != _HighLilightColor)
                {
                    _HighLilightColor = value;
                    UpdateHighlight();
                }
            }
        }

        // the border color of items in on the control..
        private Color _ItemBorderColor = Color.SteelBlue;

        /// <summary>
        /// Gets or sets the border color of items in on the control.
        /// </summary>
        [Description("Gets or sets the border color of items in on the control.")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "SteelBlue")] // the default value..
        [Browsable(true)]
        public Color ItemBorderColor
        {
            get => _ItemBorderColor;

            set
            {
                if (value != _ItemBorderColor)
                {
                    _ItemBorderColor = value;

                    ListVideos();
                    pnLargeContainer.BackColor = value;
                    pnPreviewDescription.BackColor = value;
                    pnSplit1.BackColor = value;
                    pnSplit2.BackColor = value;
                }
            }
        }
        #endregion

        #region PublicImageProperties
        // the image for the close form button..
        private Image _ImageCloseButton = VisualComponents.Properties.Resources.exit;

        /// <summary>
        /// Gets or sets the image for the close form button.
        /// </summary>
        [Description("Gets or sets the image for the close form button.")]
        [Category("Appearance")]
        [Browsable(true)]
        public Image ImageCloseButton
        {
            get
            {
                return _ImageCloseButton;
            }

            set
            {
                _ImageCloseButton = value;
                pbCloseParentForm.Image = value;
            }
        }

        // the image for the add videos button..
        private Image _ImageAddButton = VisualComponents.Properties.Resources.add_something_steelblue;

        /// <summary>
        /// Gets or sets the image for the add videos button.
        /// </summary>
        [Description("Gets or sets the image for the add videos button.")]
        [Category("Appearance")]
        [Browsable(true)]
        public Image ImageAddButton
        {
            get
            {
                return _ImageAddButton;
            }

            set
            {
                _ImageAddButton = value;
                pbAddMovieRequest.Image = value;
            }
        }

        // the image for the delete video button..
        private Image _ImageDeleteButton = VisualComponents.Properties.Resources.Cancel;

        /// <summary>
        /// Gets or sets the image for the delete video button.
        /// </summary>
        [Description("Gets or sets the image for the delete video button.")]
        [Category("Appearance")]
        [Browsable(true)]
        public Image ImageDeleteButton
        {
            get
            {
                return _ImageDeleteButton;
            }

            set
            {
                _ImageDeleteButton = value;
                pbDeleteMovie.Image = value;
            }
        }

        // the image for the next video button..
        private Image _ImageNextButton = VisualComponents.Properties.Resources.forward_steelblue;

        /// <summary>
        /// Gets or sets the image for the next video button.
        /// </summary>
        [Description("Gets or sets the image for the next video button.")]
        [Category("Appearance")]
        [Browsable(true)]
        public Image ImageNextButton
        {
            get
            {
                return _ImageNextButton;
            }

            set
            {
                _ImageNextButton = value;
                pnNext.BackgroundImage = value;
            }
        }

        // the image for the previous video button..
        private Image _ImagePreviousButton = VisualComponents.Properties.Resources.backward_steelblue;

        /// <summary>
        /// Gets or sets the image for the previous video button.
        /// </summary>
        [Description("Gets or sets the image for the previous video button.")]
        [Category("Appearance")]
        [Browsable(true)]
        public Image ImagePreviousButton
        {
            get
            {
                return _ImagePreviousButton;
            }

            set
            {
                _ImagePreviousButton = value;
                pnPrevious.BackgroundImage = value;
            }
        }

        /// <summary>
        /// Gets or sets the image for a video with no poster/still image.
        /// </summary>
        [Description("Gets or sets the image for a video with no poster/still image.")]
        [Category("Appearance")]
        [Browsable(true)]
        public static Image ImageNoVideoImage { get; set; } = VisualComponents.Properties.Resources.matt_icons_video_x_generic;
        #endregion
    }
}
