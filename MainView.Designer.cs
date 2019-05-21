namespace GrandoLib
{
    partial class MainView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.flpContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCounter = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pbDelete = new System.Windows.Forms.PictureBox();
            this.pbAddNew = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddNew)).BeginInit();
            this.SuspendLayout();
            // 
            // flpContainer
            // 
            this.flpContainer.AutoScroll = true;
            this.flpContainer.Location = new System.Drawing.Point(12, 50);
            this.flpContainer.Name = "flpContainer";
            this.flpContainer.Size = new System.Drawing.Size(522, 557);
            this.flpContainer.TabIndex = 4;
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold);
            this.lblCounter.ForeColor = System.Drawing.SystemColors.Control;
            this.lblCounter.Location = new System.Drawing.Point(13, 10);
            this.lblCounter.MaximumSize = new System.Drawing.Size(360, 0);
            this.lblCounter.MinimumSize = new System.Drawing.Size(360, 0);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(360, 17);
            this.lblCounter.TabIndex = 6;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(380, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(154, 20);
            this.txtSearch.TabIndex = 7;
            this.txtSearch.TextChanged += new System.EventHandler(this.Search_Click);
            // 
            // pbDelete
            // 
            this.pbDelete.BackColor = System.Drawing.Color.Transparent;
            this.pbDelete.Image = global::GrandoLib.Properties.Resources.delete;
            this.pbDelete.Location = new System.Drawing.Point(12, 606);
            this.pbDelete.Name = "pbDelete";
            this.pbDelete.Size = new System.Drawing.Size(50, 50);
            this.pbDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDelete.TabIndex = 8;
            this.pbDelete.TabStop = false;
            this.pbDelete.Visible = false;
            this.pbDelete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // pbAddNew
            // 
            this.pbAddNew.BackColor = System.Drawing.Color.Transparent;
            this.pbAddNew.Image = global::GrandoLib.Properties.Resources.plus1;
            this.pbAddNew.Location = new System.Drawing.Point(483, 606);
            this.pbAddNew.Name = "pbAddNew";
            this.pbAddNew.Size = new System.Drawing.Size(50, 50);
            this.pbAddNew.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbAddNew.TabIndex = 5;
            this.pbAddNew.TabStop = false;
            this.pbAddNew.Click += new System.EventHandler(this.Add_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(545, 658);
            this.Controls.Add(this.pbDelete);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.pbAddNew);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.flpContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grando Movie Library";
            this.Activated += new System.EventHandler(this.MainView_Active);
            this.Load += new System.EventHandler(this.MainView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAddNew)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpContainer;
        private System.Windows.Forms.PictureBox pbAddNew;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.PictureBox pbDelete;
    }
}

