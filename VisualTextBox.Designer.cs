namespace VPKSoft.VisualTextBox
{
    partial class VisualTextBox
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvTextBoxPretend = new System.Windows.Forms.DataGridView();
            this.colText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTextBoxPretend)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTextBoxPretend
            // 
            this.dgvTextBoxPretend.AllowUserToAddRows = false;
            this.dgvTextBoxPretend.AllowUserToDeleteRows = false;
            this.dgvTextBoxPretend.AllowUserToResizeColumns = false;
            this.dgvTextBoxPretend.AllowUserToResizeRows = false;
            this.dgvTextBoxPretend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTextBoxPretend.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTextBoxPretend.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTextBoxPretend.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTextBoxPretend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTextBoxPretend.ColumnHeadersVisible = false;
            this.dgvTextBoxPretend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colText});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Maroon;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTextBoxPretend.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTextBoxPretend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTextBoxPretend.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvTextBoxPretend.Location = new System.Drawing.Point(1, 1);
            this.dgvTextBoxPretend.MultiSelect = false;
            this.dgvTextBoxPretend.Name = "dgvTextBoxPretend";
            this.dgvTextBoxPretend.RowHeadersVisible = false;
            this.dgvTextBoxPretend.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTextBoxPretend.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvTextBoxPretend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTextBoxPretend.ShowCellErrors = false;
            this.dgvTextBoxPretend.ShowCellToolTips = false;
            this.dgvTextBoxPretend.ShowEditingIcon = false;
            this.dgvTextBoxPretend.ShowRowErrors = false;
            this.dgvTextBoxPretend.Size = new System.Drawing.Size(389, 148);
            this.dgvTextBoxPretend.TabIndex = 0;
            this.dgvTextBoxPretend.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTextBoxPretend_CellValueChanged);
            this.dgvTextBoxPretend.SizeChanged += new System.EventHandler(this.VisualTextBox_SizeChanged);
            this.dgvTextBoxPretend.Click += new System.EventHandler(this.dgvTextBoxPretend_Click);
            // 
            // colText
            // 
            this.colText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.colText.DefaultCellStyle = dataGridViewCellStyle2;
            this.colText.HeaderText = "";
            this.colText.Name = "colText";
            // 
            // VisualTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvTextBoxPretend);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VisualTextBox";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(391, 150);
            this.BackColorChanged += new System.EventHandler(this.VisualTextBox_BackColorChanged);
            this.ForeColorChanged += new System.EventHandler(this.VisualTextBox_ForeColorChanged);
            this.SizeChanged += new System.EventHandler(this.VisualTextBox_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTextBoxPretend)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTextBoxPretend;
        private System.Windows.Forms.DataGridViewTextBoxColumn colText;
    }
}
