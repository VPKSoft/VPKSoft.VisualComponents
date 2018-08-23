namespace VPKSoft.ImageSlider
{
    partial class ImageSlider
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
            this.pnDrawArea = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnDrawArea
            // 
            this.pnDrawArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDrawArea.Location = new System.Drawing.Point(0, 0);
            this.pnDrawArea.Margin = new System.Windows.Forms.Padding(0);
            this.pnDrawArea.Name = "pnDrawArea";
            this.pnDrawArea.Size = new System.Drawing.Size(597, 190);
            this.pnDrawArea.TabIndex = 0;
            this.pnDrawArea.Paint += new System.Windows.Forms.PaintEventHandler(this.pnDrawArea_Paint);
            this.pnDrawArea.Leave += new System.EventHandler(this.pnDrawArea_Leave);
            this.pnDrawArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnDrawArea_MouseDown);
            this.pnDrawArea.MouseEnter += new System.EventHandler(this.pnDrawArea_MouseEnter);
            this.pnDrawArea.MouseLeave += new System.EventHandler(this.pnDrawArea_MouseLeave);
            this.pnDrawArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnDrawArea_MouseMove);
            this.pnDrawArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnDrawArea_MouseUp);
            // 
            // ImageSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnDrawArea);
            this.Name = "ImageSlider";
            this.Size = new System.Drawing.Size(597, 190);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnDrawArea;
    }
}
