namespace VPKSoft.ImageButton
{
    partial class ImageButton
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
            this.lbButtonText = new System.Windows.Forms.Label();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnButtonImage = new System.Windows.Forms.Panel();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbButtonText
            // 
            this.lbButtonText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbButtonText.Location = new System.Drawing.Point(100, 0);
            this.lbButtonText.Margin = new System.Windows.Forms.Padding(0);
            this.lbButtonText.Name = "lbButtonText";
            this.lbButtonText.Size = new System.Drawing.Size(406, 90);
            this.lbButtonText.TabIndex = 1;
            this.lbButtonText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbButtonText.Click += new System.EventHandler(this.baseClick);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.lbButtonText, 1, 0);
            this.tlpMain.Controls.Add(this.pnButtonImage, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(1, 1);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(506, 90);
            this.tlpMain.TabIndex = 1;
            // 
            // pnButtonImage
            // 
            this.pnButtonImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnButtonImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnButtonImage.Location = new System.Drawing.Point(1, 1);
            this.pnButtonImage.Margin = new System.Windows.Forms.Padding(1);
            this.pnButtonImage.Name = "pnButtonImage";
            this.pnButtonImage.Size = new System.Drawing.Size(98, 88);
            this.pnButtonImage.TabIndex = 2;
            this.pnButtonImage.Click += new System.EventHandler(this.baseClick);
            // 
            // ImageButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ImageButton";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(508, 92);
            this.ClientSizeChanged += new System.EventHandler(this.ImageButton_ClientSizeChanged);
            this.tlpMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbButtonText;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnButtonImage;
    }
}
