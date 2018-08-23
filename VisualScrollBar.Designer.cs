namespace VPKSoft.VisualScrollBar
{
    partial class VisualScrollBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualScrollBar));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnScrollBar = new System.Windows.Forms.Panel();
            this.pnScrollDown = new System.Windows.Forms.Panel();
            this.pnScrollUp = new System.Windows.Forms.Panel();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pnScrollDown, 0, 2);
            this.tlpMain.Controls.Add(this.pnScrollBar, 0, 1);
            this.tlpMain.Controls.Add(this.pnScrollUp, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(28, 242);
            this.tlpMain.TabIndex = 0;
            // 
            // pnScrollBar
            // 
            this.pnScrollBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnScrollBar.Location = new System.Drawing.Point(0, 20);
            this.pnScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.pnScrollBar.Name = "pnScrollBar";
            this.pnScrollBar.Size = new System.Drawing.Size(28, 202);
            this.pnScrollBar.TabIndex = 1;
            this.pnScrollBar.Paint += new System.Windows.Forms.PaintEventHandler(this.pnScrollBar_Paint);
            this.pnScrollBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnScrollBar_MouseDown);
            this.pnScrollBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnScrollBar_MouseMove);
            // 
            // pnScrollDown
            // 
            this.pnScrollDown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnScrollDown.BackgroundImage")));
            this.pnScrollDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnScrollDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnScrollDown.Location = new System.Drawing.Point(0, 222);
            this.pnScrollDown.Margin = new System.Windows.Forms.Padding(0);
            this.pnScrollDown.Name = "pnScrollDown";
            this.pnScrollDown.Size = new System.Drawing.Size(28, 20);
            this.pnScrollDown.TabIndex = 3;
            this.pnScrollDown.Click += new System.EventHandler(this.pnScrollWHClick);
            // 
            // pnScrollUp
            // 
            this.pnScrollUp.BackgroundImage = global::VPKSoft.VisualComponents.Properties.Resources.scroll_up;
            this.pnScrollUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnScrollUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnScrollUp.Location = new System.Drawing.Point(0, 0);
            this.pnScrollUp.Margin = new System.Windows.Forms.Padding(0);
            this.pnScrollUp.Name = "pnScrollUp";
            this.pnScrollUp.Size = new System.Drawing.Size(28, 20);
            this.pnScrollUp.TabIndex = 2;
            this.pnScrollUp.Click += new System.EventHandler(this.pnScrollWHClick);
            // 
            // VisualScrollBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VisualScrollBar";
            this.Size = new System.Drawing.Size(28, 242);
            this.Resize += new System.EventHandler(this.VisualScrollBar_Resize);
            this.tlpMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnScrollBar;
        private System.Windows.Forms.Panel pnScrollUp;
        private System.Windows.Forms.Panel pnScrollDown;
    }
}
