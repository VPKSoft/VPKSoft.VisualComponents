namespace VPKSoft.VideoBrowser
{
    partial class VideoBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnPrevious = new System.Windows.Forms.Panel();
            this.pnNext = new System.Windows.Forms.Panel();
            this.pnLargeContainer = new System.Windows.Forms.Panel();
            this.tlpPreviewLarge = new System.Windows.Forms.TableLayoutPanel();
            this.pnPreviewImage = new System.Windows.Forms.Panel();
            this.lbPreviewImage = new System.Windows.Forms.Label();
            this.pnPreviewDescription = new System.Windows.Forms.Panel();
            this.tlpDetailHolder = new System.Windows.Forms.TableLayoutPanel();
            this.lbDescriptionCaption = new System.Windows.Forms.Label();
            this.lbDescriptionValue = new System.Windows.Forms.Label();
            this.lbLengthCaptionAndValue = new System.Windows.Forms.Label();
            this.pnSplit1 = new System.Windows.Forms.Panel();
            this.pnSplit2 = new System.Windows.Forms.Panel();
            this.tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            this.pbDeleteMovie = new System.Windows.Forms.PictureBox();
            this.pbCloseParentForm = new System.Windows.Forms.PictureBox();
            this.pbAddMovieRequest = new System.Windows.Forms.PictureBox();
            this.tlpMain.SuspendLayout();
            this.pnLargeContainer.SuspendLayout();
            this.tlpPreviewLarge.SuspendLayout();
            this.pnPreviewDescription.SuspendLayout();
            this.tlpDetailHolder.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeleteMovie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCloseParentForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddMovieRequest)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.BackColor = System.Drawing.Color.Black;
            this.tlpMain.ColumnCount = 7;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tlpMain.Controls.Add(this.pnPrevious, 0, 2);
            this.tlpMain.Controls.Add(this.pnNext, 6, 2);
            this.tlpMain.Controls.Add(this.pnLargeContainer, 1, 1);
            this.tlpMain.Controls.Add(this.pnPreviewDescription, 4, 1);
            this.tlpMain.Controls.Add(this.tlpButtons, 6, 1);
            this.tlpMain.Location = new System.Drawing.Point(12, 12);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tlpMain.Size = new System.Drawing.Size(951, 582);
            this.tlpMain.TabIndex = 0;
            // 
            // pnPrevious
            // 
            this.pnPrevious.BackgroundImage = global::VPKSoft.VisualComponents.Properties.Resources.backward_steelblue;
            this.pnPrevious.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnPrevious.Location = new System.Drawing.Point(6, 406);
            this.pnPrevious.Margin = new System.Windows.Forms.Padding(6);
            this.pnPrevious.Name = "pnPrevious";
            this.pnPrevious.Size = new System.Drawing.Size(35, 162);
            this.pnPrevious.TabIndex = 0;
            this.pnPrevious.Click += new System.EventHandler(this.pnPrevious_Click);
            // 
            // pnNext
            // 
            this.pnNext.BackgroundImage = global::VPKSoft.VisualComponents.Properties.Resources.forward_steelblue;
            this.pnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnNext.Location = new System.Drawing.Point(908, 406);
            this.pnNext.Margin = new System.Windows.Forms.Padding(6);
            this.pnNext.Name = "pnNext";
            this.pnNext.Size = new System.Drawing.Size(37, 162);
            this.pnNext.TabIndex = 1;
            this.pnNext.Click += new System.EventHandler(this.pnNext_Click);
            // 
            // pnLargeContainer
            // 
            this.pnLargeContainer.BackColor = System.Drawing.Color.SteelBlue;
            this.tlpMain.SetColumnSpan(this.pnLargeContainer, 3);
            this.pnLargeContainer.Controls.Add(this.tlpPreviewLarge);
            this.pnLargeContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnLargeContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLargeContainer.Location = new System.Drawing.Point(50, 8);
            this.pnLargeContainer.Name = "pnLargeContainer";
            this.pnLargeContainer.Padding = new System.Windows.Forms.Padding(6);
            this.pnLargeContainer.Size = new System.Drawing.Size(507, 389);
            this.pnLargeContainer.TabIndex = 4;
            this.pnLargeContainer.Click += new System.EventHandler(this.playVideoClick);
            // 
            // tlpPreviewLarge
            // 
            this.tlpPreviewLarge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpPreviewLarge.BackColor = System.Drawing.Color.Black;
            this.tlpPreviewLarge.ColumnCount = 1;
            this.tlpPreviewLarge.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPreviewLarge.Controls.Add(this.pnPreviewImage, 0, 0);
            this.tlpPreviewLarge.Controls.Add(this.lbPreviewImage, 0, 1);
            this.tlpPreviewLarge.Location = new System.Drawing.Point(6, 6);
            this.tlpPreviewLarge.Margin = new System.Windows.Forms.Padding(6);
            this.tlpPreviewLarge.Name = "tlpPreviewLarge";
            this.tlpPreviewLarge.RowCount = 2;
            this.tlpPreviewLarge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPreviewLarge.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPreviewLarge.Size = new System.Drawing.Size(495, 377);
            this.tlpPreviewLarge.TabIndex = 3;
            // 
            // pnPreviewImage
            // 
            this.pnPreviewImage.AutoSize = true;
            this.pnPreviewImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnPreviewImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnPreviewImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnPreviewImage.Location = new System.Drawing.Point(6, 6);
            this.pnPreviewImage.Margin = new System.Windows.Forms.Padding(6);
            this.pnPreviewImage.Name = "pnPreviewImage";
            this.pnPreviewImage.Size = new System.Drawing.Size(483, 345);
            this.pnPreviewImage.TabIndex = 0;
            this.pnPreviewImage.Click += new System.EventHandler(this.playVideoClick);
            // 
            // lbPreviewImage
            // 
            this.lbPreviewImage.AutoEllipsis = true;
            this.lbPreviewImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbPreviewImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPreviewImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPreviewImage.ForeColor = System.Drawing.Color.White;
            this.lbPreviewImage.Location = new System.Drawing.Point(0, 357);
            this.lbPreviewImage.Margin = new System.Windows.Forms.Padding(0);
            this.lbPreviewImage.Name = "lbPreviewImage";
            this.lbPreviewImage.Padding = new System.Windows.Forms.Padding(6);
            this.lbPreviewImage.Size = new System.Drawing.Size(495, 20);
            this.lbPreviewImage.TabIndex = 1;
            this.lbPreviewImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbPreviewImage.Click += new System.EventHandler(this.playVideoClick);
            // 
            // pnPreviewDescription
            // 
            this.pnPreviewDescription.BackColor = System.Drawing.Color.SteelBlue;
            this.tlpMain.SetColumnSpan(this.pnPreviewDescription, 2);
            this.pnPreviewDescription.Controls.Add(this.tlpDetailHolder);
            this.pnPreviewDescription.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnPreviewDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnPreviewDescription.Location = new System.Drawing.Point(563, 8);
            this.pnPreviewDescription.Name = "pnPreviewDescription";
            this.pnPreviewDescription.Padding = new System.Windows.Forms.Padding(6);
            this.pnPreviewDescription.Size = new System.Drawing.Size(336, 389);
            this.pnPreviewDescription.TabIndex = 5;
            this.pnPreviewDescription.Click += new System.EventHandler(this.playVideoClick);
            // 
            // tlpDetailHolder
            // 
            this.tlpDetailHolder.BackColor = System.Drawing.Color.Black;
            this.tlpDetailHolder.ColumnCount = 1;
            this.tlpDetailHolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetailHolder.Controls.Add(this.lbDescriptionCaption, 0, 0);
            this.tlpDetailHolder.Controls.Add(this.lbDescriptionValue, 0, 2);
            this.tlpDetailHolder.Controls.Add(this.lbLengthCaptionAndValue, 0, 9);
            this.tlpDetailHolder.Controls.Add(this.pnSplit1, 0, 1);
            this.tlpDetailHolder.Controls.Add(this.pnSplit2, 0, 8);
            this.tlpDetailHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetailHolder.Location = new System.Drawing.Point(6, 6);
            this.tlpDetailHolder.Margin = new System.Windows.Forms.Padding(6);
            this.tlpDetailHolder.Name = "tlpDetailHolder";
            this.tlpDetailHolder.RowCount = 10;
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tlpDetailHolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpDetailHolder.Size = new System.Drawing.Size(324, 377);
            this.tlpDetailHolder.TabIndex = 4;
            // 
            // lbDescriptionCaption
            // 
            this.lbDescriptionCaption.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbDescriptionCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDescriptionCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDescriptionCaption.ForeColor = System.Drawing.Color.White;
            this.lbDescriptionCaption.Location = new System.Drawing.Point(6, 0);
            this.lbDescriptionCaption.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbDescriptionCaption.Name = "lbDescriptionCaption";
            this.lbDescriptionCaption.Size = new System.Drawing.Size(312, 45);
            this.lbDescriptionCaption.TabIndex = 4;
            this.lbDescriptionCaption.Text = "Description";
            this.lbDescriptionCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDescriptionValue
            // 
            this.lbDescriptionValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbDescriptionValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDescriptionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDescriptionValue.ForeColor = System.Drawing.Color.White;
            this.lbDescriptionValue.Location = new System.Drawing.Point(6, 51);
            this.lbDescriptionValue.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbDescriptionValue.Name = "lbDescriptionValue";
            this.tlpDetailHolder.SetRowSpan(this.lbDescriptionValue, 6);
            this.lbDescriptionValue.Size = new System.Drawing.Size(312, 270);
            this.lbDescriptionValue.TabIndex = 7;
            this.lbDescriptionValue.Click += new System.EventHandler(this.playVideoClick);
            // 
            // lbLengthCaptionAndValue
            // 
            this.lbLengthCaptionAndValue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbLengthCaptionAndValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLengthCaptionAndValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLengthCaptionAndValue.ForeColor = System.Drawing.Color.White;
            this.lbLengthCaptionAndValue.Location = new System.Drawing.Point(6, 327);
            this.lbLengthCaptionAndValue.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbLengthCaptionAndValue.Name = "lbLengthCaptionAndValue";
            this.lbLengthCaptionAndValue.Size = new System.Drawing.Size(312, 50);
            this.lbLengthCaptionAndValue.TabIndex = 3;
            this.lbLengthCaptionAndValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbLengthCaptionAndValue.Click += new System.EventHandler(this.playVideoClick);
            // 
            // pnSplit1
            // 
            this.pnSplit1.BackColor = System.Drawing.Color.SteelBlue;
            this.pnSplit1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSplit1.Location = new System.Drawing.Point(0, 45);
            this.pnSplit1.Margin = new System.Windows.Forms.Padding(0);
            this.pnSplit1.Name = "pnSplit1";
            this.pnSplit1.Size = new System.Drawing.Size(324, 6);
            this.pnSplit1.TabIndex = 8;
            this.pnSplit1.Click += new System.EventHandler(this.playVideoClick);
            // 
            // pnSplit2
            // 
            this.pnSplit2.BackColor = System.Drawing.Color.SteelBlue;
            this.pnSplit2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnSplit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSplit2.Location = new System.Drawing.Point(0, 321);
            this.pnSplit2.Margin = new System.Windows.Forms.Padding(0);
            this.pnSplit2.Name = "pnSplit2";
            this.pnSplit2.Size = new System.Drawing.Size(324, 6);
            this.pnSplit2.TabIndex = 9;
            this.pnSplit2.Click += new System.EventHandler(this.playVideoClick);
            // 
            // tlpButtons
            // 
            this.tlpButtons.ColumnCount = 1;
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButtons.Controls.Add(this.pbDeleteMovie, 0, 2);
            this.tlpButtons.Controls.Add(this.pbCloseParentForm, 0, 0);
            this.tlpButtons.Controls.Add(this.pbAddMovieRequest, 0, 1);
            this.tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButtons.Location = new System.Drawing.Point(902, 5);
            this.tlpButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.RowCount = 4;
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButtons.Size = new System.Drawing.Size(49, 395);
            this.tlpButtons.TabIndex = 6;
            // 
            // pbDeleteMovie
            // 
            this.pbDeleteMovie.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDeleteMovie.Image = global::VPKSoft.VisualComponents.Properties.Resources.Cancel;
            this.pbDeleteMovie.Location = new System.Drawing.Point(0, 189);
            this.pbDeleteMovie.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pbDeleteMovie.Name = "pbDeleteMovie";
            this.pbDeleteMovie.Size = new System.Drawing.Size(49, 90);
            this.pbDeleteMovie.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDeleteMovie.TabIndex = 3;
            this.pbDeleteMovie.TabStop = false;
            this.pbDeleteMovie.Click += new System.EventHandler(this.pbDeleteMovie_Click);
            // 
            // pbCloseParentForm
            // 
            this.pbCloseParentForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbCloseParentForm.Image = global::VPKSoft.VisualComponents.Properties.Resources.exit;
            this.pbCloseParentForm.Location = new System.Drawing.Point(0, 3);
            this.pbCloseParentForm.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pbCloseParentForm.Name = "pbCloseParentForm";
            this.pbCloseParentForm.Size = new System.Drawing.Size(49, 90);
            this.pbCloseParentForm.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCloseParentForm.TabIndex = 2;
            this.pbCloseParentForm.TabStop = false;
            this.pbCloseParentForm.Click += new System.EventHandler(this.pbCloseParentForm_Click);
            // 
            // pbAddMovieRequest
            // 
            this.pbAddMovieRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbAddMovieRequest.Image = global::VPKSoft.VisualComponents.Properties.Resources.add_something_steelblue;
            this.pbAddMovieRequest.Location = new System.Drawing.Point(0, 96);
            this.pbAddMovieRequest.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pbAddMovieRequest.Name = "pbAddMovieRequest";
            this.pbAddMovieRequest.Size = new System.Drawing.Size(49, 90);
            this.pbAddMovieRequest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAddMovieRequest.TabIndex = 1;
            this.pbAddMovieRequest.TabStop = false;
            this.pbAddMovieRequest.Click += new System.EventHandler(this.pbAddMovieRequest_Click);
            // 
            // VideoBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VideoBrowser";
            this.Size = new System.Drawing.Size(975, 606);
            this.Resize += new System.EventHandler(this.VideoBrowser_Resize);
            this.tlpMain.ResumeLayout(false);
            this.pnLargeContainer.ResumeLayout(false);
            this.tlpPreviewLarge.ResumeLayout(false);
            this.tlpPreviewLarge.PerformLayout();
            this.pnPreviewDescription.ResumeLayout(false);
            this.tlpDetailHolder.ResumeLayout(false);
            this.tlpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbDeleteMovie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCloseParentForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddMovieRequest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnPrevious;
        private System.Windows.Forms.Panel pnNext;
        private System.Windows.Forms.Panel pnLargeContainer;
        private System.Windows.Forms.TableLayoutPanel tlpPreviewLarge;
        private System.Windows.Forms.Panel pnPreviewImage;
        private System.Windows.Forms.Label lbPreviewImage;
        private System.Windows.Forms.Panel pnPreviewDescription;
        private System.Windows.Forms.TableLayoutPanel tlpDetailHolder;
        private System.Windows.Forms.Label lbDescriptionCaption;
        private System.Windows.Forms.Label lbDescriptionValue;
        private System.Windows.Forms.Label lbLengthCaptionAndValue;
        private System.Windows.Forms.Panel pnSplit1;
        private System.Windows.Forms.Panel pnSplit2;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private System.Windows.Forms.PictureBox pbCloseParentForm;
        private System.Windows.Forms.PictureBox pbAddMovieRequest;
        private System.Windows.Forms.PictureBox pbDeleteMovie;
    }
}
