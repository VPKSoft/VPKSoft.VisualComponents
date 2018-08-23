namespace VPKSoft.VisualLocationBrowser
{
    partial class VisualLocationBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualLocationBrowser));
            this.tlpBackForward = new System.Windows.Forms.TableLayoutPanel();
            this.pnForward = new System.Windows.Forms.Panel();
            this.pnBack = new System.Windows.Forms.Panel();
            this.pnAddSomething = new System.Windows.Forms.Panel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnMain = new System.Windows.Forms.Panel();
            this.pnLocationList = new System.Windows.Forms.Panel();
            this.vsbVertical = new VPKSoft.VisualScrollBar.VisualScrollBar();
            this.tlpBackForward.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpBackForward
            // 
            this.tlpBackForward.BackColor = System.Drawing.Color.SteelBlue;
            this.tlpBackForward.ColumnCount = 3;
            this.tlpBackForward.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpBackForward.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBackForward.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpBackForward.Controls.Add(this.pnForward, 2, 0);
            this.tlpBackForward.Controls.Add(this.pnBack, 0, 0);
            this.tlpBackForward.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBackForward.Location = new System.Drawing.Point(0, 0);
            this.tlpBackForward.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBackForward.Name = "tlpBackForward";
            this.tlpBackForward.RowCount = 1;
            this.tlpBackForward.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBackForward.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBackForward.Size = new System.Drawing.Size(416, 20);
            this.tlpBackForward.TabIndex = 2;
            // 
            // pnForward
            // 
            this.pnForward.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnForward.BackColor = System.Drawing.Color.SteelBlue;
            this.pnForward.BackgroundImage = global::VPKSoft.VisualComponents.Properties.Resources.forward_misc;
            this.pnForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnForward.Location = new System.Drawing.Point(312, 0);
            this.pnForward.Margin = new System.Windows.Forms.Padding(0);
            this.pnForward.Name = "pnForward";
            this.pnForward.Size = new System.Drawing.Size(104, 20);
            this.pnForward.TabIndex = 1;
            this.pnForward.MouseEnter += new System.EventHandler(this.ItemMouseEnter);
            this.pnForward.MouseLeave += new System.EventHandler(this.ItemMouseLeave);
            // 
            // pnBack
            // 
            this.pnBack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnBack.BackColor = System.Drawing.Color.SteelBlue;
            this.pnBack.BackgroundImage = global::VPKSoft.VisualComponents.Properties.Resources.back_misc;
            this.pnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnBack.Location = new System.Drawing.Point(0, 0);
            this.pnBack.Margin = new System.Windows.Forms.Padding(0);
            this.pnBack.Name = "pnBack";
            this.pnBack.Size = new System.Drawing.Size(104, 20);
            this.pnBack.TabIndex = 0;
            this.pnBack.MouseEnter += new System.EventHandler(this.ItemMouseEnter);
            this.pnBack.MouseLeave += new System.EventHandler(this.ItemMouseLeave);
            // 
            // pnAddSomething
            // 
            this.pnAddSomething.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnAddSomething.BackColor = System.Drawing.Color.SteelBlue;
            this.pnAddSomething.BackgroundImage = global::VPKSoft.VisualComponents.Properties.Resources.add_something;
            this.pnAddSomething.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnAddSomething.Location = new System.Drawing.Point(0, 290);
            this.pnAddSomething.Margin = new System.Windows.Forms.Padding(0);
            this.pnAddSomething.Name = "pnAddSomething";
            this.pnAddSomething.Size = new System.Drawing.Size(416, 20);
            this.pnAddSomething.TabIndex = 1;
            this.pnAddSomething.Click += new System.EventHandler(this.ItemClickHandler);
            this.pnAddSomething.MouseEnter += new System.EventHandler(this.ItemMouseEnter);
            this.pnAddSomething.MouseLeave += new System.EventHandler(this.ItemMouseLeave);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Controls.Add(this.pnAddSomething, 0, 2);
            this.tlpMain.Controls.Add(this.tlpBackForward, 0, 0);
            this.tlpMain.Controls.Add(this.pnMain, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(416, 310);
            this.tlpMain.TabIndex = 0;
            // 
            // pnMain
            // 
            this.pnMain.AutoScroll = true;
            this.pnMain.Controls.Add(this.vsbVertical);
            this.pnMain.Controls.Add(this.pnLocationList);
            this.pnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMain.Location = new System.Drawing.Point(0, 20);
            this.pnMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(416, 270);
            this.pnMain.TabIndex = 3;
            // 
            // pnLocationList
            // 
            this.pnLocationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnLocationList.AutoScroll = true;
            this.pnLocationList.Location = new System.Drawing.Point(0, 0);
            this.pnLocationList.Margin = new System.Windows.Forms.Padding(0);
            this.pnLocationList.Name = "pnLocationList";
            this.pnLocationList.Size = new System.Drawing.Size(416, 270);
            this.pnLocationList.TabIndex = 2;
            this.pnLocationList.Scroll += new System.Windows.Forms.ScrollEventHandler(this.pnLocationList_Scroll);
            this.pnLocationList.ClientSizeChanged += new System.EventHandler(this.VisualLocationBrowser_ClientSizeChanged);
            // 
            // vsbVertical
            // 
            this.vsbVertical.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsbVertical.Location = new System.Drawing.Point(388, 0);
            this.vsbVertical.Margin = new System.Windows.Forms.Padding(0);
            this.vsbVertical.Name = "vsbVertical";
            this.vsbVertical.ScrollDownImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollDownImage")));
            this.vsbVertical.ScrollLeftImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollLeftImage")));
            this.vsbVertical.ScrollRightImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollRightImage")));
            this.vsbVertical.ScrollUpImage = ((System.Drawing.Image)(resources.GetObject("vsbVertical.ScrollUpImage")));
            this.vsbVertical.Size = new System.Drawing.Size(28, 270);
            this.vsbVertical.TabIndex = 3;
            this.vsbVertical.ValueChanged += new VPKSoft.VisualScrollBar.VisualScrollBar.OnValueChanged(this.pnScroll_ValueChanged);
            // 
            // VisualLocationBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "VisualLocationBrowser";
            this.Size = new System.Drawing.Size(416, 310);
            this.Resize += new System.EventHandler(this.VisualLocationBrowser_Resize);
            this.tlpBackForward.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBackForward;
        private System.Windows.Forms.Panel pnForward;
        private System.Windows.Forms.Panel pnBack;
        private System.Windows.Forms.Panel pnAddSomething;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnMain;
        private System.Windows.Forms.Panel pnLocationList;
        private VPKSoft.VisualScrollBar.VisualScrollBar vsbVertical;
    }
}
