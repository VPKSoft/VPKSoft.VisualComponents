namespace VPKSoft.VisualFileBrowser
{
    partial class VisualFileBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualFileBrowser));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnTop = new System.Windows.Forms.Panel();
            this.pnFileListHolder = new System.Windows.Forms.Panel();
            this.pnFileList = new System.Windows.Forms.Panel();
            this.vsbVertical = new VPKSoft.VisualScrollBar.VisualScrollBar();
            this.tlpMain.SuspendLayout();
            this.pnFileListHolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pnTop, 0, 0);
            this.tlpMain.Controls.Add(this.pnFileListHolder, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(351, 299);
            this.tlpMain.TabIndex = 2;
            // 
            // pnTop
            // 
            this.pnTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnTop.AutoSize = true;
            this.pnTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnTop.Location = new System.Drawing.Point(3, 3);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(345, 0);
            this.pnTop.TabIndex = 4;
            this.pnTop.ClientSizeChanged += new System.EventHandler(this.VisualFileBrowser_ClientSizeChanged);
            // 
            // pnFileListHolder
            // 
            this.pnFileListHolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnFileListHolder.Controls.Add(this.vsbVertical);
            this.pnFileListHolder.Controls.Add(this.pnFileList);
            this.pnFileListHolder.Location = new System.Drawing.Point(0, 6);
            this.pnFileListHolder.Margin = new System.Windows.Forms.Padding(0);
            this.pnFileListHolder.Name = "pnFileListHolder";
            this.pnFileListHolder.Size = new System.Drawing.Size(351, 273);
            this.pnFileListHolder.TabIndex = 5;
            // 
            // pnFileList
            // 
            this.pnFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnFileList.AutoScroll = true;
            this.pnFileList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnFileList.Location = new System.Drawing.Point(0, 0);
            this.pnFileList.Margin = new System.Windows.Forms.Padding(0);
            this.pnFileList.Name = "pnFileList";
            this.pnFileList.Size = new System.Drawing.Size(351, 273);
            this.pnFileList.TabIndex = 3;
            this.pnFileList.ClientSizeChanged += new System.EventHandler(this.VisualFileBrowser_ClientSizeChanged);
            // 
            // vsbVertical
            // 
            this.vsbVertical.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsbVertical.Location = new System.Drawing.Point(323, 0);
            this.vsbVertical.Margin = new System.Windows.Forms.Padding(0);
            this.vsbVertical.Name = "vsbVertical";
            this.vsbVertical.ScrollDownImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollDownImage")));
            this.vsbVertical.ScrollLeftImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollLeftImage")));
            this.vsbVertical.ScrollRightImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollRightImage")));
            this.vsbVertical.ScrollUpImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollUpImage")));
            this.vsbVertical.Size = new System.Drawing.Size(28, 273);
            this.vsbVertical.TabIndex = 4;
            this.vsbVertical.ValueChanged += new VPKSoft.VisualScrollBar.VisualScrollBar.OnValueChanged(this.vsbVertical_ValueChanged);
            // 
            // VisualFileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "VisualFileBrowser";
            this.Size = new System.Drawing.Size(351, 299);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.pnFileListHolder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnFileList;
        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.Panel pnFileListHolder;
        private VisualScrollBar.VisualScrollBar vsbVertical;
    }
}
