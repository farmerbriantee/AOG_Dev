namespace AOG.Forms.Field
{
    partial class FormAgShareDownloader
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbFields = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.glControl1 = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // lbFields
            // 
            this.lbFields.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chArea});
            this.lbFields.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFields.FullRowSelect = true;
            this.lbFields.GridLines = true;
            this.lbFields.HideSelection = false;
            this.lbFields.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lbFields.Location = new System.Drawing.Point(12, 12);
            this.lbFields.MultiSelect = false;
            this.lbFields.Name = "lbFields";
            this.lbFields.Size = new System.Drawing.Size(477, 495);
            this.lbFields.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lbFields.TabIndex = 1;
            this.lbFields.UseCompatibleStateImageBehavior = false;
            this.lbFields.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Field";
            this.chName.Width = 375;
            // 
            // chArea
            // 
            this.chArea.Text = "Area";
            this.chArea.Width = 100;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.Location = new System.Drawing.Point(841, 581);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 57);
            this.button1.TabIndex = 2;
            this.button1.Text = "Download and Open";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.DarkGray;
            this.glControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.glControl1.Location = new System.Drawing.Point(510, 12);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(480, 494);
            this.glControl1.TabIndex = 3;
            this.glControl1.VSync = false;
            // 
            // FormAgShareDownloader
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(1004, 661);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbFields);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAgShareDownloader";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormAgShareDownloader";
            this.Load += new System.EventHandler(this.FormAgShareDownloader_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lbFields;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chArea;
        private System.Windows.Forms.Button button1;
        private OpenTK.GLControl glControl1;
    }
}