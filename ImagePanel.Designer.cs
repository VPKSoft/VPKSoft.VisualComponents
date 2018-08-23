namespace VPKSoft.ImagePanel
{
    partial class ScaleImagePanel
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
            this.pnImage = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnImage
            // 
            this.pnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnImage.Location = new System.Drawing.Point(0, 0);
            this.pnImage.Margin = new System.Windows.Forms.Padding(0);
            this.pnImage.Name = "pnImage";
            this.pnImage.Size = new System.Drawing.Size(150, 150);
            this.pnImage.TabIndex = 0;
            this.pnImage.ClientSizeChanged += new System.EventHandler(this.pnImage_ClientSizeChanged);
            // 
            // ImagePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnImage);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ImagePanel";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnImage;
    }
}
