#region License
/*
VPKSoft.VisualFileBrowser

A component for selecting a file or a directory.
Copyright © 2018 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.VisualFileBrowser.

VPKSoft.VisualFileBrowser is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.VisualFileBrowser is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.VisualFileBrowser.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
/// <summary>
/// A name space holding the VisualFileBrowser control.
/// </summary>
namespace VPKSoft.VisualFileBrowser
#pragma warning restore CS1587 // XML comment is not placed on a valid language element
{
    /// <summary>
    /// A control that shows a file browser, which is suitable for larger screens, i.e. television.
    /// </summary>
    public partial class VisualFileBrowser : UserControl, IMessageFilter
    {
        /// <summary>
        /// The constructor of the VisualFileBrowser class.
        /// </summary>
        public VisualFileBrowser()
        {
            InitializeComponent();

            DoNoHScroll(); // hide horizontal scrolling..

            BackColor = Color.SteelBlue;
            this.DoubleBuffered = true; // just to be "safe"..

            Application.AddMessageFilter(this);

            ListDirectory(); // List the directory contents..
            this.Disposed += VisualFileBrowser_Disposed; // Leave no event handlers hanging..            
        }

        /// <summary>
        /// String containing multiple characters which can be used to measure maximum text height.
        /// </summary>
        internal const string measureText = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖabcdefghijklmnopqrstuvwxyzåäö£€%[]$@ÂÊÎÔÛâêîôûÄËÏÖÜäëïöüÀÈÌÒÙàèìòùÁÉÍÓÚáéíóúÃÕãõ '|?+\\/{}½§01234567890+<>_-:;*&¤#\"!";

        /// <summary>
        /// Seems to be a good way to capture mouse wheel scroll "event" as it does not raise the Scroll event.. of course somewhat "fishy"..
        /// </summary>
        /// <param name="m">A reference to Message class instance.</param>
        /// <returns>Always false, it is not wanted to seem that the message was handled.</returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x20a) // this is the (hexadecimal) code for the mouse wheel.. it is just followed blindly (no fancy things)..
            {
                vsbVertical.Maximum = pnFileList.VerticalScroll.Maximum; // exception might occur otherwise..
                vsbVertical.Value = pnFileList.VerticalScroll.Value; // do set the value even if nothing was changed..
            }
            return false; // as int the description, it is not wanted to seem that the message was handled..
        }

        #region Events
        /// <summary>
        /// An class that is given as a parameter to FileSelected and DirectorySelected events.
        /// </summary>
        public class FileOrDirectorySelectedEventArgs: EventArgs
        {
            /// <summary>
            /// Gets or set a value indicating if the Path is a file or not.
            /// </summary>
            public bool IsFile { get; set; }

            /// <summary>
            /// Gets or set a value indicating if the Path is a directory or not.
            /// </summary>
            public bool IsDirectory { get; set; }

            /// <summary>
            /// Gets or sets the value of the file or a directory. This is a full path.
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// Gets or sets the value if the item is accepted to the list. 
            /// </summary>
            public bool Accept { get; set; }

            /// <summary>
            /// Gets or sets the value of a custom image which would be displayed on the right side of the file/directory item.
            /// </summary>
            public Image Image { get; set; }

            /// <summary>
            /// Gets or set a value indicating if the event was handled by the subscriber or not.
            /// </summary>
            public bool Cancel { get; set; }
        }

        /// <summary>
        /// A delegate for the FileSelected event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">FileOrDirectorySelectedEventArgs class instance.</param>
        public delegate void OnFileSelected(object sender, FileOrDirectorySelectedEventArgs e);

        /// <summary>
        /// An event that is raised when the user selects a file from the directory listing.
        /// </summary>
        [Description("An event that is raised when the user selects a file from the directory listing")]
        [Category("VisualFileBrowser")]
        public event OnFileSelected FileSelected = null;

        /// <summary>
        /// A delegate for the DirectorySelected event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">FileOrDirectorySelectedEventArgs class instance.</param>
        public delegate void OnDirectorySelected(object sender, FileOrDirectorySelectedEventArgs e);

        /// <summary>
        /// An event that is raised when the user selects a directory from the directory listing.
        /// </summary>
        [Description("An event that is raised when the user selects a directory from the directory listing")]
        [Category("VisualFileBrowser")]
        public event OnDirectorySelected DirectorySelected = null;

        /// <summary>
        /// A delegate for the ContextRequested event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">FileOrDirectorySelectedEventArgs class instance.</param>
        public delegate void OnContextRequested(object sender, FileOrDirectorySelectedEventArgs e);

        /// <summary>
        /// An event that is raised when the clicks a file or a directory with right mouse button on an item in the file/directory listing.
        /// </summary>
        [Description("An event that is raised when the clicks a file or a directory with right mouse button on an item in the file/directory listing")]
        [Category("VisualFileBrowser")]
        public event OnDirectorySelected ContextRequested = null;

        /// <summary>
        /// A delegate for the AcceptItem event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">FileOrDirectorySelectedEventArgs class instance.</param>
        public delegate void OnAcceptItem(object sender, FileOrDirectorySelectedEventArgs e);

        /// <summary>
        /// An event that is raised when the user selects a file from the directory listing.
        /// </summary>
        [Description("An event that is raised an item's acceptance to the is asked when the item listing is being created")]
        [Category("VisualFileBrowser")]
        public event OnAcceptItem AcceptItem = null;

        /// <summary>
        /// A delegate for the GetCustomItemImage event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">FileOrDirectorySelectedEventArgs class instance.</param>
        public delegate void OnGetCustomItemImage(object sender, FileOrDirectorySelectedEventArgs e);

        /// <summary>
        /// An event that is raised when a custom image is asked for a specific file or directory item.
        /// </summary>
        [Description("An event that is raised when a custom image is asked for a specific file or directory item")]
        [Category("VisualFileBrowser")]
        public event OnGetCustomItemImage GetCustomItemImage = null;
        #endregion

        #region ColorProperties
        // Item's background color..
        private Color _ItemBackgroundColor = Color.SteelBlue;

        /// <summary>
        /// The background color of an item in the file/directory listing.
        /// </summary>
        [Description("The background color of a directory or a file entry")]
        [Category("VisualFileBrowser")]
        [DefaultValue(typeof(Color), "SteelBlue")]
        public Color ItemBackgroundColor
        {
            get
            {
                return _ItemBackgroundColor;
            }

            set
            {
                _ItemBackgroundColor = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // Item's foreground color..
        private Color _ItemForeColor = Color.White;

        /// <summary>
        /// The foreground color of an item in the file/directory listing.
        /// </summary>
        [Description("The color of a directory or a file entry")]
        [Category("VisualFileBrowser")]
        [DefaultValue(typeof(Color), "White")]
        public Color ItemForeColor
        {
            get
            {
                return _ItemForeColor;
            }

            set
            {
                _ItemForeColor = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // Foreground color of a current directory item..
        private Color _CurrentDirectoryForegroundColor = Color.White;

        /// <summary>
        /// The foreground color of a current directory item.
        /// </summary>
        [Description("The color of a directory or a file entry")]
        [Category("VisualFileBrowser")]
        [DefaultValue(typeof(Color), "White")]
        public Color CurrentDirectoryForegroundColor
        {
            get
            {
                return _CurrentDirectoryForegroundColor;
            }

            set
            {
                _CurrentDirectoryForegroundColor = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // Background color of a current directory item..
        private Color _CurrentDirectoryBackgroundColor = Color.MidnightBlue;

        /// <summary>
        /// The background color of a current directory item.
        /// </summary>
        [Description("The background color of a current directory item")]
        [Category("VisualFileBrowser")]
        [DefaultValue(typeof(Color), "MidnightBlue")]
        public Color CurrentDirectoryBackgroundColor
        {
            get
            {
                return _CurrentDirectoryBackgroundColor;
            }

            set
            {
                _CurrentDirectoryBackgroundColor = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // Item's background color when mouse is on top of an item.
        private Color _HoverBackgroundColor = Color.LightSkyBlue;

        /// <summary>
        /// The background color of an item in the file/directory listing when mouse hovers over the item. 
        /// </summary>
        [Description("The background color of an item the mouse is currently hovering over")]
        [Category("VisualFileBrowser")]
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
        /// The foreground color of an item in the file/directory listing when mouse hovers over the item. 
        /// </summary>
        [Description("The foreground color of an item the mouse is currently hovering over")]
        [Category("VisualFileBrowser")]
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

        #region MiscProperties
        // The text to display when in the root of the file/directory listing..
        private string _DriveListText = "Root";

        /// <summary>
        /// The text which is displayed when the in root of the file/directory listing. This is for localization purpose.
        /// </summary>
        [Description("The text to display on a drive selection mode")]
        [Category("VisualFileBrowser")]
        [DefaultValue("Root")]
        public string DriveListText
        {
            get
            {
                return _DriveListText;
            }

            set
            {
                if (value != null && value.Trim() != string.Empty) // We don't accept nothing..
                {
                    _DriveListText = value;
                }
            }
        }
        #endregion

        #region CustomizationProperties
        // A value indicating whether to use custom images on the right side of the item..
        private bool _UseCustomImage = false;

        /// <summary>
        /// Gets or sets a value indicating whether to use custom images on the right side of the item.
        /// </summary>
        [Description("A value indicating whether to use custom images on the right side of the item")]
        [Category("VisualFileBrowser")]
        [DefaultValue(false)]
        public bool UseCustomImage
        {
            get
            {
                return _UseCustomImage;
            }

            set
            {
                _UseCustomImage = value;
                ListDirectory(); // List the directory contents..
            }
        }
        #endregion

        #region NavigationProperties
        // The base path..
        private string _BasePath = ":DRIVES:";

        private string _previousBasePath = ":DRIVES:";

        /// <summary>
        /// Base path from where to list directories and/or files.
        /// <para/>The special value ":DRIVES: can be used to list system's drives and start the navigation from there.
        /// </summary>
        [Description("Set the path to list files and directories from")]
        [Category("VisualFileBrowser")]
        [DefaultValue(":DRIVES:")]
        public string BasePath
        {
            get
            {
                return _BasePath;
            }

            set
            {
                if (Directory.Exists(value) || value == ":DRIVES:")
                {
                    _BasePath = value;
                    _previousBasePath = value; // save the user given value if navigation is prevented from this point backwards..
                    ListDirectory(); // List the directory contents..
                }
            }
        }

        // File extensions to list..
        private string[] _VisibleFileExtensions = new string[] { "*.*" };

        /// <summary>
        /// An array of strings that determines which types of files (extension) to list.
        /// </summary>
        [Description("Set the enumerable search masks for files")]
        [Category("VisualFileBrowser")]
        [DefaultValue("*.*")]
        public string[] VisibleFileExtensions
        {
            get
            {
                return _VisibleFileExtensions;
            }

            set
            {
                _VisibleFileExtensions = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // If only directories should be listed..
        private bool _OnlySelectDirectories = false;

        /// <summary>
        /// Indicates if the component only lists directories.
        /// </summary>
        [Description("If only the directories can be browsed")]
        [Category("VisualFileBrowser")]
        [DefaultValue(false)]
        public bool OnlySelectDirectories
        {
            get
            {
                return _OnlySelectDirectories;
            }

            set
            {
                _OnlySelectDirectories = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // If directories which do not contain requested files should be shown?
        private bool _FileModeListUselessDirs = false;

        /// <summary>
        /// If the directories which does not contain requested file types should be shown.
        /// </summary>
        [Description("If the directories which does not contain requested file types should be shown")]
        [Category("VisualFileBrowser")]
        [DefaultValue(false)]
        public bool FileModeListUselessDirs
        {
            get
            {
                return _FileModeListUselessDirs;
            }

            set
            {
                _FileModeListUselessDirs = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // Can the user navigate backwards in the directory tree?
        private bool _AllowBackwardNavigation = true;

        /// <summary>
        /// Indicates if the user can navigate back in the directory tree or not. This means upper from the BasePath property value.
        /// </summary>
        [Description("Indicates if the user can navigate back in the directory tree or not. This means upper from the BasePath property value")]
        [Category("VisualFileBrowser")]
        [DefaultValue(true)]
        public bool AllowBackwardNavigation
        {
            get
            {
                return _AllowBackwardNavigation;
            }

            set
            {
                _AllowBackwardNavigation = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // Can the user navigate in the directory tree at all?
        private bool _AllowNavigation = true;

        /// <summary>
        /// Indicates if the user can navigate anywhere in the directory tree or not.
        /// </summary>
        [Description("Indicates if the user can navigate anywhere in the directory tree or not")]
        [Category("VisualFileBrowser")]
        [DefaultValue(true)]
        public bool AllowNavigation
        {
            get
            {
                return _AllowNavigation;
            }

            set
            {
                _AllowNavigation = value;
                ListDirectory(); // List the directory contents..
            }
        }
        #endregion

        #region ImageProperties
        // An image to show before the folder name
        private Image _folderImage = VisualComponents.Properties.Resources.folder;

        /// <summary>
        /// An image to show before the folder name.
        /// </summary>
        [Description("An image to show before the folder name")]
        [Category("VisualFileBrowser")]
        public Image FolderImage
        {
            get
            {
                return _folderImage;
            }

            set
            {
                _folderImage = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // An image to show before the file name
        private Image _fileImage = VisualComponents.Properties.Resources.file;

        /// <summary>
        /// An image to show before the file name.
        /// </summary>
        [Description("An image to show before the file name")]
        [Category("VisualFileBrowser")]
        public Image FileImage
        {
            get
            {
                return _fileImage;
            }

            set
            {
                _fileImage = value;
                ListDirectory(); // List the directory contents..
            }
        }

        // An image to for the current directory item..
        private Image _selectedFolderImage = VisualComponents.Properties.Resources.folders;

        /// <summary>
        /// An image to show before the currently selected folder.
        /// </summary>
        [Description("An image to show before the currently selected folder")]
        [Category("VisualFileBrowser")]
        public Image SelectedFolderImage
        {
            get
            {
                return _selectedFolderImage;
            }

            set
            {
                _selectedFolderImage = value;
                ListDirectory(); // List the directory contents..
            }
        }
        #endregion

        #region CreateControls

        /// <summary>
        /// Removes the existing control event handlers and clears the list items.
        /// </summary>
        private void DetachEventHandlersAndClear()
        {
            // Remove existing controls event handles (the list items)..
            foreach (Control ctrl in pnFileList.Controls)
            {
                foreach (Control subCtrl in ctrl.Controls)
                {
                    subCtrl.MouseDown -= PathMouseDown;
                    subCtrl.MouseEnter -= PathMouseEnter;
                    subCtrl.MouseLeave -= PathMouseLeave;
                }
            }

            SuspendLayout();
            // Remove existing controls (the list items)..
            pnFileList.Controls.Clear();
            pnTop.Controls.Clear();
            ResumeLayout();
        }

        /// <summary>
        /// Returns a value indicating if the specified Path is a file.
        /// </summary>
        /// <param name="path">A path indication a file or a directory.</param>
        /// <param name="fileOrDirectory">A reference string to check if the there are some special meanings to the path.</param>
        /// <param name="isDirectoryItem">A value indicating if the item is the current directory item.</param>
        /// <returns>True if the path is a file, otherwise false.</returns>
        private bool IsFile(string path, string fileOrDirectory, bool isDirectoryItem)
        {
            if (fileOrDirectory != ".." && fileOrDirectory != ":DRIVES:" && !isDirectoryItem)
            {
                if (File.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a value indicating if the specified Path is a directory.
        /// </summary>
        /// <param name="path">A path indication a file or a directory.</param>
        /// <param name="fileOrDirectory">A reference string to check if the there are some special meanings to the path.</param>
        /// <param name="isDirectoryItem">A value indicating if the item is the current directory item.</param>
        /// <returns>True if the path is a directory, otherwise false.</returns>
        private bool IsDirectory(string path, string fileOrDirectory, bool isDirectoryItem)
        {
            if (fileOrDirectory != ".." && fileOrDirectory != ":DRIVES:" && !isDirectoryItem)
            {
                if (Directory.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }

        // List the directory contents..
        void ListDirectory()
        {
            
            SuspendLayout();
            List<string> dirs = new List<string>(); // a list for directories..
            List<string> files = new List<string>(); // a list for files..
            if (BasePath == ":DRIVES:") // if the "special" directory is the BasePath the list only drives with their lables..
            {
                DriveInfo[] drives = DriveInfo.GetDrives(); // Get the system's drive listing..
                foreach(DriveInfo di in drives)
                {
                    dirs.Add(di.RootDirectory.FullName + " [" + di.VolumeLabel + "]"); // Add the drives's full root directory name and it's volume label..
                }
            }
            else // No special path so the basic listing..
            {
                // Select only directories that match are not hidden or system files
                dirs.AddRange( // Get the directories..
                    Directory.GetDirectories(BasePath).Where(f => !(new DirectoryInfo(f).Attributes.HasFlag(FileAttributes.Hidden) ||
                                                                    new DirectoryInfo(f).Attributes.HasFlag(FileAttributes.System))));

                if (!OnlySelectDirectories) // Should we list the files?
                {
                    List<string> tempFiles = new List<string>();
                    tempFiles.AddRange(Directory.GetFiles(BasePath, "*").Where(f => !(File.GetAttributes(f).HasFlag(FileAttributes.Hidden) ||
                                                                                  File.GetAttributes(f).HasFlag(FileAttributes.System))));

                    for (int i = tempFiles.Count - 1; i >= 0; i--)
                    {
                        bool match = false;

                        foreach (string ext in VisibleFileExtensions) // get only the files which extension matches the extension array (VisibleFileExtensions)..
                        {
                            if (ext == "*.*")
                            {
                                match = true;
                                break;
                            }

                            if (ext == "*")
                            {
                                match = true;
                                break;
                            }

                            if (tempFiles[i].EndsWith(ext.TrimStart('*'), StringComparison.InvariantCultureIgnoreCase))
                            {
                                match = true;
                                break;
                            }
                        }

                        if (!match)
                        {
                            tempFiles.RemoveAt(i);
                        }
                    }

                    files.AddRange(tempFiles);
                }
            }

            dirs.Sort(); // sort the directories to a alphabetic order..
            files.Sort(); // sort the files to a alphabetic order..

            DetachEventHandlersAndClear(); // Remove existing controls event handles (the list items)..

            // if the user is not allowed to navigate to upper level in the directory tree..
            bool cantGoUpper = !AllowBackwardNavigation && BasePath.Equals(_previousBasePath, StringComparison.InvariantCultureIgnoreCase);
            cantGoUpper |= !AllowBackwardNavigation && BasePath.Equals(_previousBasePath.TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase);


            if (BasePath != ":DRIVES:" && !cantGoUpper && AllowNavigation) // If navigation is possible to upper level in the directory tree..
            {
                files.Insert(0, ".."); // .. add the ".." special directory..
                files.InsertRange(1, dirs); // Insert the directories to the file list so we don't have to iterate through them both..
            }
            else
            {
                files.InsertRange(0, dirs); // Insert the directories to the file list so we don't have to iterate through them both..
            }

            // Measure the maximum text height using the long list of different characters
            int textHeight = TextRenderer.MeasureText(measureText, Font).Height;
            int pnTop = 0; // Set the top value for an item to list..

            files.Insert(0, BasePath == ":DRIVES:" ? DriveListText : BasePath);

            bool isDirectoryItem = true; // The first one in the loop is the current directory item..

            for (int i = 0; i < files.Count; i++) // Create the item list..
            {
                string fileOrDirectory = files[i];
                string tagStr = string.Empty;

                if (isDirectoryItem) // The directory item..
                {
                    tagStr = "<diritem>";
                }
                else
                {
                    if (BasePath == ":DRIVES:") // If the "root" is visible, then..
                    {
                        tagStr = fileOrDirectory.Substring(0, fileOrDirectory.IndexOf('[') - 1); // .. the tag should point to the root directory of the name
                    }
                    else // .. otherwise the fileOrDirectory variable is only that is needed.
                    {
                        tagStr = fileOrDirectory;
                    }
                }

                bool isFile = IsFile(tagStr, fileOrDirectory, isDirectoryItem); // save these values, so no more calls the IsFile() or IsDirectory() methods are required..
                bool isDirectory = IsDirectory(tagStr, fileOrDirectory, isDirectoryItem);

                if (AcceptItem != null) // .. and the event is actually subscribed.
                {
                    // Raise an event to query if a file/directory should be accepted to the file/directory listing..
                    FileOrDirectorySelectedEventArgs eventArgs = new FileOrDirectorySelectedEventArgs() { Accept = true, IsDirectory = isDirectory, IsFile = isFile, Path = tagStr, Image = null, Cancel = false };
                    AcceptItem(this, eventArgs); // Raise the event.
                    if (!eventArgs.Accept) // .. if not accepted then simply continue.
                    {
                        continue;
                    }
                }

                TableLayoutPanel tableLayout = new TableLayoutPanel(); // We need a TableLayoutPanel, Panel and a Label for each item.
                Panel panelImage = new Panel(); // First the panel..
                Panel panelImageCustom = new Panel(); // A custom image to the right side of the item..
                tableLayout.RowCount = 1; // Set the row count to 1
                tableLayout.ColumnCount =  UseCustomImage ? 3 : 2; // Set the column count to correct value..
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10)); // give the rows and columns their sizes..
                tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, UseCustomImage ? 80 : 90));
                if (UseCustomImage) // Only add the third column if the right-side custom image is enabled..
                {
                    tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10)); // .. at size of 10 percentage of the item's space.
                }
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // END: give the rows and columns their sizes..
                Label label = new Label(); // Create the label

                if (isDirectoryItem) // The directory item..
                {
                    label.Text = BasePath == ":DRIVES:" ? DriveListText : BasePath; // Check if the path can be shown or are we in the "root"..
                    label.Click += Label_Click;
                }
                else
                {
                    if (BasePath == ":DRIVES:") // set the label's text depending if we are int the "root" or not..
                    {
                        label.Text = fileOrDirectory;
                    }
                    /* A useless if now..
                    else if (Regex.IsMatch(fileOrDirectory, @"^[A-Za-z][:][\\]$"))
                    {
                        label.Text = fileOrDirectory;
                    }
                    */
                    else // END: set the label's text depending if we are int the "root" or not..
                    {
                        label.Text = fileOrDirectory.Split(Path.DirectorySeparatorChar).Last(); // We only want the last file or directory
                    }
                }

                tableLayout.Controls.Add(label, 1, 0); // Add the Label to the base TableLayoutPanel..
                label.Parent = tableLayout; // ..make it as it's parent.

                label.Tag = tagStr;
                panelImage.Tag = tagStr;
                panelImageCustom.Tag = tagStr;

                if (isDirectoryItem) // The directory item..
                {
                    panelImage.BackgroundImage = SelectedFolderImage;
                }
                else if (Directory.Exists(panelImage.Tag.ToString())) // select the image based on whether the item is a file or a folder
                {
                    panelImage.BackgroundImage = FolderImage;
                }
                else if (File.Exists(panelImage.Tag.ToString()))
                {
                    panelImage.BackgroundImage = FileImage;
                }  // END: select the image based on whether the item is a file or a folder

                if (UseCustomImage) // Only if the use of custom images is defined..
                {
                    if (GetCustomItemImage != null) // And if the event is subscribed..
                    {
                        FileOrDirectorySelectedEventArgs eventArgs = new FileOrDirectorySelectedEventArgs() { Accept = true, IsDirectory = isDirectory, IsFile = isFile, Path = tagStr, Image = null, Cancel = false };
                        GetCustomItemImage(this, eventArgs); // Get the image (no matter if null)..
                        panelImageCustom.BackgroundImage = eventArgs.Image; // Assign the image..
                    }
                }

                panelImage.BackgroundImageLayout = ImageLayout.Zoom; // It's needed that the Panel holding the image of a file or a directory is laid out properly..
                panelImageCustom.BackgroundImageLayout = ImageLayout.Zoom; // It's needed that the Panel holding the image of a file or a directory is laid out properly..

                tableLayout.Controls.Add(panelImage, 0, 0);
                if (UseCustomImage) // Only if the use of custom images is defined..
                {
                    tableLayout.Controls.Add(panelImageCustom, 2, 0);
                }

                panelImage.Dock = DockStyle.Fill; // .. first just do fill.. (see the end of the method..)
                panelImageCustom.Dock = DockStyle.Fill; // .. first just do fill.. (see the end of the method..)

                label.AutoEllipsis = true; // the Label must not wrap, so this makes it to add three dots and cut the text if wrapping should occur
                if (isDirectoryItem) // The directory item..
                {
                    tableLayout.BackColor = CurrentDirectoryBackgroundColor; // Again, assign colors..
                    label.BackColor = CurrentDirectoryBackgroundColor;
                    label.ForeColor = CurrentDirectoryForegroundColor;
                    panelImageCustom.BackColor = CurrentDirectoryBackgroundColor;
                    panelImage.BackColor = CurrentDirectoryBackgroundColor; // Assign the background color
                }
                else
                {
                    tableLayout.BackColor = ItemBackgroundColor; // Again, assign colors..
                    label.BackColor = ItemBackgroundColor;
                    label.ForeColor = ItemForeColor;
                    panelImageCustom.BackColor = ItemBackgroundColor;
                    panelImage.BackColor = ItemBackgroundColor; // Assign the background color
                }
                tableLayout.Height = textHeight; // The height of the long string was measured before, so do set it..
                tableLayout.Top = pnTop; // this is the top value of the item's base control - increasing with this loop..
                label.AutoSize = false; // disable the stupid auto-sizing..
                label.Dock = DockStyle.Fill; // all space to the label..
                label.TextAlign = ContentAlignment.MiddleLeft; // text is aligned to the middle left - for now, sorry for the cultures who use RTL :-(
                pnTop += isDirectoryItem ? 0 : textHeight; // increase the top counter by an item's height

                if (isDirectoryItem) // The directory item..
                {
                    this.pnTop.Controls.Add(tableLayout); // add the TableLayoutPanel to the item list..
                    tableLayout.Parent = this.pnTop; // .. set the parent tor the item (TableLayoutPanel)
                }
                else
                {
                    pnFileList.Controls.Add(tableLayout); // add the TableLayoutPanel to the item list..
                    tableLayout.Parent = pnFileList; // .. set the parent tor the item (TableLayoutPanel)
                }

                if (!isDirectoryItem) // Only subscribe the event handles to actually click-able items, not the current directory..
                {
                    label.MouseDown += PathMouseDown; // Subscribe to event handlers
                    label.MouseEnter += PathMouseEnter;
                    label.MouseLeave += PathMouseLeave;

                    panelImage.MouseDown += PathMouseDown;
                    panelImage.MouseEnter += PathMouseEnter;
                    panelImage.MouseLeave += PathMouseLeave; // END: Subscribe to event handlers

                    if (UseCustomImage)  // Only if the use of custom images is defined..
                    {
                        panelImageCustom.MouseDown += PathMouseDown;
                        panelImageCustom.MouseEnter += PathMouseEnter;
                        panelImageCustom.MouseLeave += PathMouseLeave; // END: Subscribe to event handlers
                    }
                }

                // (C): https://stackoverflow.com/questions/13725721/get-height-and-width-of-tablelayoutpanel-cell-in-windows-forms
                TableLayoutPanelCellPosition pos = tableLayout.GetCellPosition(panelImage);
                int height = tableLayout.GetRowHeights()[pos.Row]; // measure the cell height so the image can be aligned as square..
                tableLayout.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, height);

                if (UseCustomImage)  // Only if the use of custom images is defined..
                {
                    tableLayout.ColumnStyles[2] = new ColumnStyle(SizeType.Absolute, height);
                }

                isDirectoryItem = false; // Only the first one in the loop is the current directory item..
            }

            foreach (Control ctrl in pnFileList.Controls)
            {
                ctrl.Width = pnFileList.ClientSize.Width; // Resize to fit..
            }

            foreach (Control ctrl in this.pnTop.Controls)
            {
                ctrl.Width = this.pnTop.ClientSize.Width; // Resize to fit..
            }
            ResumeLayout();
        }

        private void Label_Click(object sender, EventArgs e)
        {
            string dirOrFile = (sender as Control).Text;
            if (Directory.Exists(dirOrFile)) // An item holding a directory was clicked.
            {
                // Launch the event..
                DirectorySelected?.Invoke(this, new FileOrDirectorySelectedEventArgs() { IsFile = false, Path = dirOrFile, IsDirectory = true, Cancel = false });
            }
        }
        #endregion

        #region InternalEvents
        // Mouse cursor has left the area of a click-able item..
        private void PathMouseLeave(object sender, EventArgs e)
        {
            Control parent = ((Control)sender).Parent; // Find the sender's (Control's) parent..

            SuspendLayout();
            foreach (Control ctrl in parent.Controls) // ..restore normal color to it's children..
            {
                ctrl.ForeColor = ItemForeColor;
                ctrl.BackColor = ItemBackgroundColor;
            }

            parent.BackColor = ItemBackgroundColor; // ..restore normal color to the parent control.
            ResumeLayout();
        }

        // Mouse cursor has left the area of a click-able item..
        private void PathMouseEnter(object sender, EventArgs e)
        {
            Control parent = ((Control)sender).Parent; // Find the sender's (Control's) parent..
            SuspendLayout();
            foreach (Control ctrl in parent.Controls) // ..set the mouse hover colors to it's children..
            {
                ctrl.ForeColor = HoverItemColor;
                ctrl.BackColor = HoverBackgroundColor;
            }
            ResumeLayout();
            parent.BackColor = HoverBackgroundColor; // ..set the mouse hover color to the parent control.
        }

        // A click-able item (file or a directory) was clicked..
        private void PathMouseDown(object sender, MouseEventArgs e)
        {
            string dirOrFile = (sender as Control).Tag.ToString();

            // accept only  left mouse button as the right one is for an action requesting user to interact with the context..
            if (e.Button == MouseButtons.Left)
            {

                // Is navigation allowed on the control?
                if (dirOrFile == ".." && AllowNavigation) // An item holding a ".." (go back in the directory tree) directory was clicked.
                {
                    string root = Path.GetPathRoot(BasePath);

                    if (root.Equals(BasePath, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _BasePath = ":DRIVES:";
                        ListDirectory(); // List the directory contents..
                    }
                    else
                    {
                        _BasePath = Directory.GetParent(BasePath).FullName;
                        ListDirectory(); // List the directory contents..
                    }
                }                                       // Is navigation allowed on the control?
                else if (Directory.Exists(dirOrFile) && AllowNavigation) // An item holding a directory was clicked.
                {
                    _BasePath = dirOrFile;
                    ListDirectory(); // List the directory contents..
                }
                /* NOT NEEDED?
                else if (Directory.Exists(dirOrFile)) // Launch an event that a user has selected a directory..
                {
                    if (DirectorySelected != null) // .. if the event was subscribed.
                    {
                        // Launch the event..
                        DirectorySelected(this, new FileOrDirectorySelectedEventArgs() { IsFile = false, Path = dirOrFile, IsDirectory = true, Cancel = false });
                    }
                }
                */
                else if (File.Exists(dirOrFile)) // Launch an event that a user has selected a file..
                {
                    // Launch the event..
                    FileSelected?.Invoke(this, new FileOrDirectorySelectedEventArgs() { IsFile = true, Path = dirOrFile, IsDirectory = false, Cancel = false });
                }
            }
            else if (e.Button == MouseButtons.Right) // some action for an item (file or directory) was requested by the user..
            {
                if (ContextRequested != null) // .. if the event was subscribed.
                {
                    // the clicked item is a file so raise the event with proper FileOrDirectorySelectedEventArgs instance..
                    if (File.Exists(dirOrFile))
                    {
                        // create a reference to the FileOrDirectorySelectedEventArgs class instance as the Cancel property's value is needed..
                        FileOrDirectorySelectedEventArgs fileOrDirectorySelectedEventArgs = new FileOrDirectorySelectedEventArgs() { IsFile = true, Path = dirOrFile, IsDirectory = false, Cancel = false };

                        // raise the event..
                        ContextRequested(this, fileOrDirectorySelectedEventArgs);

                        // .. if not canceled, something might have been done with the file..
                        if (!fileOrDirectorySelectedEventArgs.Cancel)
                        {
                            ListDirectory(); // List the directory contents..
                        }
                    }
                    // the clicked item is a directory so raise the event with proper FileOrDirectorySelectedEventArgs instance..
                    else if (dirOrFile != ".." && Directory.Exists(dirOrFile))
                    {
                        // create a reference to the FileOrDirectorySelectedEventArgs class instance as the Cancel property's value is needed..
                        FileOrDirectorySelectedEventArgs fileOrDirectorySelectedEventArgs = new FileOrDirectorySelectedEventArgs() { IsFile = false, Path = dirOrFile, IsDirectory = true, Cancel = false };

                        // raise the event..
                        ContextRequested(this, fileOrDirectorySelectedEventArgs);

                        // .. if not canceled, something might have been done with the directory..
                        if (!fileOrDirectorySelectedEventArgs.Cancel)
                        {
                            ListDirectory(); // List the directory contents..
                        }
                    }
                }
            }
        }

        // When the client size of a control has changed the child controls need some resizing..
        private void VisualFileBrowser_ClientSizeChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            if (sender.Equals(pnFileList)) // Check which is the calling control..
            {
                foreach (Control ctrl in pnFileList.Controls) // this is to prevent the horizontal scrollbar to appear..
                {
                    ctrl.Width = pnFileList.ClientSize.Width;// - SystemInformation.VerticalScrollBarWidth;
                }
            }
            else if (sender.Equals(pnTop)) // Check which is the calling control..
            {
                foreach (Control ctrl in pnTop.Controls) // this is to prevent the horizontal scrollbar to appear..
                {
                    ctrl.Width = pnTop.ClientSize.Width; // - SystemInformation.VerticalScrollBarWidth;
                }
            }
            DoNoHScroll(); // hide horizontal scrolling..
            vsbVertical.Width = SystemInformation.VerticalScrollBarWidth;
            ResumeLayout();
        }

        // The value of the VPKSoft.VisualScrollBar was changed..
        private void vsbVertical_ValueChanged(object sender, ScrollEventArgs e)
        {
            // .. so do set the new value of the actually hidden scroll bar :-) ..
            if (e.NewValue < pnFileList.VerticalScroll.Maximum)
            {
                pnFileList.VerticalScroll.Value = e.NewValue;
            }
        }

        // (C): https://stackoverflow.com/questions/5489273/how-do-i-disable-the-horizontal-scrollbar-in-a-panel
        // try to visually override the control's automatic scroll bar and disable horizontal scroll "fully"..
        private void DoNoHScroll()
        {
            SuspendLayout();
            pnFileList.AutoScroll = true; // scrolling is wanted..
            pnFileList.HorizontalScroll.Enabled = false; // ..but no horizontal one..
            pnFileList.HorizontalScroll.Visible = false; // ..continuing to: but no horizontal one..
            vsbVertical.Maximum = pnFileList.VerticalScroll.Maximum; // .. set the maximum value to avoid self-caused exceptions..
            vsbVertical.Visible = pnFileList.VerticalScroll.Visible; // we don't want the vsbVertical to "go hiding" behind the standard scroll bar..
            if (vsbVertical.Visible) // .. so if visible..
            {
                vsbVertical.BringToFront(); // ..bring it to front
            }
            ResumeLayout();
        }

        // Leave no event handlers hanging..
        private void VisualFileBrowser_Disposed(object sender, EventArgs e)
        {
            DetachEventHandlersAndClear(); // Remove existing controls event handlers (the list items)..
            Application.RemoveMessageFilter(this);
        }
        #endregion
    }
}
