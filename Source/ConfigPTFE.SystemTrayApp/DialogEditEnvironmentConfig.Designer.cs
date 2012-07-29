namespace ConfigPTFE.SystemTrayApp
{
    partial class DialogEditEnvironmentConfig
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
            this.components = new System.ComponentModel.Container();
            this.lblEnvironmentName = new System.Windows.Forms.Label();
            this.lblEnvironmentRole = new System.Windows.Forms.Label();
            this.lblConfigSectionLocations = new System.Windows.Forms.Label();
            this.dgvConfigSectionLocations = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ddlEnvironmentName = new System.Windows.Forms.ComboBox();
            this.ddlEnvironmentRole = new System.Windows.Forms.ComboBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfigSectionLocations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEnvironmentName
            // 
            this.lblEnvironmentName.AutoSize = true;
            this.lblEnvironmentName.Location = new System.Drawing.Point(13, 13);
            this.lblEnvironmentName.Name = "lblEnvironmentName";
            this.lblEnvironmentName.Size = new System.Drawing.Size(128, 17);
            this.lblEnvironmentName.TabIndex = 0;
            this.lblEnvironmentName.Text = "Environment Name";
            // 
            // lblEnvironmentRole
            // 
            this.lblEnvironmentRole.AutoSize = true;
            this.lblEnvironmentRole.Location = new System.Drawing.Point(12, 43);
            this.lblEnvironmentRole.Name = "lblEnvironmentRole";
            this.lblEnvironmentRole.Size = new System.Drawing.Size(120, 17);
            this.lblEnvironmentRole.TabIndex = 0;
            this.lblEnvironmentRole.Text = "Environment Role";
            // 
            // lblConfigSectionLocations
            // 
            this.lblConfigSectionLocations.AutoSize = true;
            this.lblConfigSectionLocations.Location = new System.Drawing.Point(12, 95);
            this.lblConfigSectionLocations.Name = "lblConfigSectionLocations";
            this.lblConfigSectionLocations.Size = new System.Drawing.Size(160, 17);
            this.lblConfigSectionLocations.TabIndex = 0;
            this.lblConfigSectionLocations.Text = "ConfigSection Locations";
            // 
            // dgvConfigSectionLocations
            // 
            this.dgvConfigSectionLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConfigSectionLocations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConfigSectionLocations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.errorProvider1.SetIconAlignment(this.dgvConfigSectionLocations, System.Windows.Forms.ErrorIconAlignment.TopRight);
            this.dgvConfigSectionLocations.Location = new System.Drawing.Point(16, 115);
            this.dgvConfigSectionLocations.Name = "dgvConfigSectionLocations";
            this.dgvConfigSectionLocations.RowTemplate.Height = 24;
            this.dgvConfigSectionLocations.Size = new System.Drawing.Size(617, 209);
            this.dgvConfigSectionLocations.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(558, 343);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(477, 343);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ddlEnvironmentName
            // 
            this.ddlEnvironmentName.FormattingEnabled = true;
            this.ddlEnvironmentName.Location = new System.Drawing.Point(147, 13);
            this.ddlEnvironmentName.Name = "ddlEnvironmentName";
            this.ddlEnvironmentName.Size = new System.Drawing.Size(323, 24);
            this.ddlEnvironmentName.TabIndex = 5;
            this.ddlEnvironmentName.TextChanged += new System.EventHandler(this.ddlEnvironmentName_TextChanged);
            this.ddlEnvironmentName.Validating += new System.ComponentModel.CancelEventHandler(this.ddlEnvironmentName_Validating);
            // 
            // ddlEnvironmentRole
            // 
            this.ddlEnvironmentRole.FormattingEnabled = true;
            this.ddlEnvironmentRole.Location = new System.Drawing.Point(147, 43);
            this.ddlEnvironmentRole.Name = "ddlEnvironmentRole";
            this.ddlEnvironmentRole.Size = new System.Drawing.Size(323, 24);
            this.ddlEnvironmentRole.TabIndex = 5;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // DialogEditEnvironmentConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 378);
            this.Controls.Add(this.ddlEnvironmentRole);
            this.Controls.Add(this.ddlEnvironmentName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvConfigSectionLocations);
            this.Controls.Add(this.lblConfigSectionLocations);
            this.Controls.Add(this.lblEnvironmentRole);
            this.Controls.Add(this.lblEnvironmentName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DialogEditEnvironmentConfig";
            this.Text = "Edit Application Environment Config";
            this.Load += new System.EventHandler(this.dialogEditEnvironmentConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfigSectionLocations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEnvironmentName;
        private System.Windows.Forms.Label lblEnvironmentRole;
        private System.Windows.Forms.Label lblConfigSectionLocations;
        private System.Windows.Forms.DataGridView dgvConfigSectionLocations;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox ddlEnvironmentName;
        private System.Windows.Forms.ComboBox ddlEnvironmentRole;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}