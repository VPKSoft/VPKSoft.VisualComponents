namespace VPKSoft.VisualListBox
{
    partial class VisualListBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualListBox));
            this.vsbMain = new VPKSoft.VisualScrollBar.VisualScrollBar();
            this.lbeMain = new VPKSoft.VisualListBox.ListBoxExtension();
            this.SuspendLayout();
            // 
            // vsbMain
            // 
            this.vsbMain.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsbMain.Location = new System.Drawing.Point(653, 0);
            this.vsbMain.Margin = new System.Windows.Forms.Padding(0);
            this.vsbMain.Name = "vsbMain";
            this.vsbMain.ScrollDownImage = ((System.Drawing.Image)(resources.GetObject("vsbMain.ScrollDownImage")));
            this.vsbMain.ScrollLeftImage = ((System.Drawing.Image)(resources.GetObject("vsbMain.ScrollLeftImage")));
            this.vsbMain.ScrollRightImage = ((System.Drawing.Image)(resources.GetObject("vsbMain.ScrollRightImage")));
            this.vsbMain.ScrollUpImage = ((System.Drawing.Image)(resources.GetObject("vsbMain.ScrollUpImage")));
            this.vsbMain.Size = new System.Drawing.Size(28, 404);
            this.vsbMain.TabIndex = 1;
            this.vsbMain.ValueNoEvent = 0;
            this.vsbMain.ValueChanged += new VPKSoft.VisualScrollBar.VisualScrollBar.OnValueChanged(this.vsbMain_ValueChanged);
            // 
            // lbeMain
            // 
            this.lbeMain.BackColor = System.Drawing.Color.Black;
            this.lbeMain.BackColorAlternative = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbeMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbeMain.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbeMain.ForeColor = System.Drawing.Color.White;
            this.lbeMain.ForeColorAlternative = System.Drawing.Color.Gainsboro;
            this.lbeMain.FormattingEnabled = true;
            this.lbeMain.HoverBackColor = System.Drawing.Color.Black;
            this.lbeMain.HoverBackColorAlternative = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.lbeMain.HoverForeColor = System.Drawing.Color.White;
            this.lbeMain.HoverForeColorAlternative = System.Drawing.Color.Gainsboro;
            this.lbeMain.ItemHeight = 25;
            this.lbeMain.Items.AddRange(new object[] {
            "testing 0...",
            "testing 1...",
            "testing 2...",
            "testing 3...",
            "testing 4...",
            "testing 5...",
            "testing 6...",
            "testing 7...",
            "testing 8...",
            "testing 9..."});
            this.lbeMain.Location = new System.Drawing.Point(0, 0);
            this.lbeMain.Margin = new System.Windows.Forms.Padding(0);
            this.lbeMain.Name = "lbeMain";
            this.lbeMain.ScrollAlwaysVisible = true;
            this.lbeMain.Size = new System.Drawing.Size(681, 404);
            this.lbeMain.TabIndex = 0;
            this.lbeMain.VScrollPosition = 0;
            this.lbeMain.VScrollPositionNoEvent = 0;
            this.lbeMain.ButtonClicked += new VPKSoft.VisualListBox.ListBoxExtension.OnButtonClicked(this.lbeMain_ButtonClicked);
            this.lbeMain.ItemClicked += new VPKSoft.VisualListBox.ListBoxExtension.OnButtonClicked(this.lbeMain_ItemClicked);
            this.lbeMain.VScrollChanged += new VPKSoft.VisualListBox.ListBoxExtension.OnVScrollChanged(this.lbeMain_VScrollChanged);
            this.lbeMain.DoubleClick += new System.EventHandler(this.lbeMain_DoubleClick);
            // 
            // VisualListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.vsbMain);
            this.Controls.Add(this.lbeMain);
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VisualListBox";
            this.Size = new System.Drawing.Size(681, 404);
            this.ResumeLayout(false);

        }

        #endregion

        private VisualScrollBar.VisualScrollBar vsbMain;
        private ListBoxExtension lbeMain;
    }
}
