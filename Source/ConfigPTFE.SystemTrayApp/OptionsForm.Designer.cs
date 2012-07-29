namespace ConfigPTFE.SystemTrayApp
{
    partial class OptionsForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblEnvironmentConfigFileName = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dgvEnvironmentNames = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpEnvironmentNames = new System.Windows.Forms.TabPage();
            this.tpEnvironmentRoles = new System.Windows.Forms.TabPage();
            this.dgvEnvironmentRoles = new System.Windows.Forms.DataGridView();
            this.gbTargetEnvironmentConfig = new System.Windows.Forms.GroupBox();
            this.linkBrowseToConfigDirectory = new System.Windows.Forms.LinkLabel();
            this.linkCreateNewEnvironmentConfigFile = new System.Windows.Forms.LinkLabel();
            this.ddlEnvironmentConfigFileNames = new System.Windows.Forms.ComboBox();
            this.btnEditEnvironmentConfigFile = new System.Windows.Forms.Button();
            this.gbTrayMenuManagment = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentNames)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpEnvironmentNames.SuspendLayout();
            this.tpEnvironmentRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentRoles)).BeginInit();
            this.gbTargetEnvironmentConfig.SuspendLayout();
            this.gbTrayMenuManagment.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(464, 425);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(545, 425);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblEnvironmentConfigFileName
            // 
            this.lblEnvironmentConfigFileName.AutoSize = true;
            this.lblEnvironmentConfigFileName.Location = new System.Drawing.Point(24, 24);
            this.lblEnvironmentConfigFileName.Name = "lblEnvironmentConfigFileName";
            this.lblEnvironmentConfigFileName.Size = new System.Drawing.Size(229, 17);
            this.lblEnvironmentConfigFileName.TabIndex = 3;
            this.lblEnvironmentConfigFileName.Text = "Menu selections will modify this file:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dgvEnvironmentNames
            // 
            this.dgvEnvironmentNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEnvironmentNames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEnvironmentNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnvironmentNames.Location = new System.Drawing.Point(6, 6);
            this.dgvEnvironmentNames.Name = "dgvEnvironmentNames";
            this.dgvEnvironmentNames.RowTemplate.Height = 24;
            this.dgvEnvironmentNames.Size = new System.Drawing.Size(568, 180);
            this.dgvEnvironmentNames.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpEnvironmentNames);
            this.tabControl1.Controls.Add(this.tpEnvironmentRoles);
            this.tabControl1.Location = new System.Drawing.Point(6, 31);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(588, 221);
            this.tabControl1.TabIndex = 5;
            // 
            // tpEnvironmentNames
            // 
            this.tpEnvironmentNames.Controls.Add(this.dgvEnvironmentNames);
            this.tpEnvironmentNames.Location = new System.Drawing.Point(4, 25);
            this.tpEnvironmentNames.Name = "tpEnvironmentNames";
            this.tpEnvironmentNames.Padding = new System.Windows.Forms.Padding(3);
            this.tpEnvironmentNames.Size = new System.Drawing.Size(580, 192);
            this.tpEnvironmentNames.TabIndex = 0;
            this.tpEnvironmentNames.Text = "Environment Names";
            this.tpEnvironmentNames.UseVisualStyleBackColor = true;
            // 
            // tpEnvironmentRoles
            // 
            this.tpEnvironmentRoles.Controls.Add(this.dgvEnvironmentRoles);
            this.tpEnvironmentRoles.Location = new System.Drawing.Point(4, 25);
            this.tpEnvironmentRoles.Name = "tpEnvironmentRoles";
            this.tpEnvironmentRoles.Padding = new System.Windows.Forms.Padding(3);
            this.tpEnvironmentRoles.Size = new System.Drawing.Size(580, 192);
            this.tpEnvironmentRoles.TabIndex = 1;
            this.tpEnvironmentRoles.Text = "Environment Roles";
            this.tpEnvironmentRoles.UseVisualStyleBackColor = true;
            // 
            // dgvEnvironmentRoles
            // 
            this.dgvEnvironmentRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEnvironmentRoles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEnvironmentRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnvironmentRoles.Location = new System.Drawing.Point(6, 6);
            this.dgvEnvironmentRoles.Name = "dgvEnvironmentRoles";
            this.dgvEnvironmentRoles.RowTemplate.Height = 24;
            this.dgvEnvironmentRoles.Size = new System.Drawing.Size(580, 253);
            this.dgvEnvironmentRoles.TabIndex = 4;
            // 
            // gbTargetEnvironmentConfig
            // 
            this.gbTargetEnvironmentConfig.Controls.Add(this.linkBrowseToConfigDirectory);
            this.gbTargetEnvironmentConfig.Controls.Add(this.linkCreateNewEnvironmentConfigFile);
            this.gbTargetEnvironmentConfig.Controls.Add(this.ddlEnvironmentConfigFileNames);
            this.gbTargetEnvironmentConfig.Controls.Add(this.btnEditEnvironmentConfigFile);
            this.gbTargetEnvironmentConfig.Controls.Add(this.lblEnvironmentConfigFileName);
            this.gbTargetEnvironmentConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTargetEnvironmentConfig.Location = new System.Drawing.Point(16, 17);
            this.gbTargetEnvironmentConfig.Name = "gbTargetEnvironmentConfig";
            this.gbTargetEnvironmentConfig.Size = new System.Drawing.Size(600, 111);
            this.gbTargetEnvironmentConfig.TabIndex = 6;
            this.gbTargetEnvironmentConfig.TabStop = false;
            this.gbTargetEnvironmentConfig.Text = "Application Environment Config Managment";
            // 
            // linkBrowseToConfigDirectory
            // 
            this.linkBrowseToConfigDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkBrowseToConfigDirectory.AutoSize = true;
            this.linkBrowseToConfigDirectory.Location = new System.Drawing.Point(198, 82);
            this.linkBrowseToConfigDirectory.Name = "linkBrowseToConfigDirectory";
            this.linkBrowseToConfigDirectory.Size = new System.Drawing.Size(171, 17);
            this.linkBrowseToConfigDirectory.TabIndex = 6;
            this.linkBrowseToConfigDirectory.TabStop = true;
            this.linkBrowseToConfigDirectory.Text = "Browse to config directory";
            this.linkBrowseToConfigDirectory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBrowseToConfigDirectory_LinkClicked);
            // 
            // linkCreateNewEnvironmentConfigFile
            // 
            this.linkCreateNewEnvironmentConfigFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkCreateNewEnvironmentConfigFile.AutoSize = true;
            this.linkCreateNewEnvironmentConfigFile.Location = new System.Drawing.Point(25, 82);
            this.linkCreateNewEnvironmentConfigFile.Name = "linkCreateNewEnvironmentConfigFile";
            this.linkCreateNewEnvironmentConfigFile.Size = new System.Drawing.Size(155, 17);
            this.linkCreateNewEnvironmentConfigFile.TabIndex = 5;
            this.linkCreateNewEnvironmentConfigFile.TabStop = true;
            this.linkCreateNewEnvironmentConfigFile.Text = "Create a new config file";
            this.linkCreateNewEnvironmentConfigFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCreateNewEnvironmentConfigFile_LinkClicked);
            // 
            // ddlEnvironmentConfigFileNames
            // 
            this.ddlEnvironmentConfigFileNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlEnvironmentConfigFileNames.FormattingEnabled = true;
            this.ddlEnvironmentConfigFileNames.Location = new System.Drawing.Point(27, 44);
            this.ddlEnvironmentConfigFileNames.Name = "ddlEnvironmentConfigFileNames";
            this.ddlEnvironmentConfigFileNames.Size = new System.Drawing.Size(342, 24);
            this.ddlEnvironmentConfigFileNames.TabIndex = 4;
            // 
            // btnEditEnvironmentConfigFile
            // 
            this.btnEditEnvironmentConfigFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditEnvironmentConfigFile.Location = new System.Drawing.Point(375, 45);
            this.btnEditEnvironmentConfigFile.Name = "btnEditEnvironmentConfigFile";
            this.btnEditEnvironmentConfigFile.Size = new System.Drawing.Size(75, 23);
            this.btnEditEnvironmentConfigFile.TabIndex = 2;
            this.btnEditEnvironmentConfigFile.Text = "Edit";
            this.btnEditEnvironmentConfigFile.UseVisualStyleBackColor = true;
            this.btnEditEnvironmentConfigFile.Click += new System.EventHandler(this.btnEditEnvironmentConfigFile_Click);
            // 
            // gbTrayMenuManagment
            // 
            this.gbTrayMenuManagment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTrayMenuManagment.Controls.Add(this.tabControl1);
            this.gbTrayMenuManagment.Location = new System.Drawing.Point(16, 147);
            this.gbTrayMenuManagment.Name = "gbTrayMenuManagment";
            this.gbTrayMenuManagment.Size = new System.Drawing.Size(600, 258);
            this.gbTrayMenuManagment.TabIndex = 6;
            this.gbTrayMenuManagment.TabStop = false;
            this.gbTrayMenuManagment.Text = "Tray Menu Management";
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(632, 460);
            this.Controls.Add(this.gbTrayMenuManagment);
            this.Controls.Add(this.gbTargetEnvironmentConfig);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(650, 500);
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentNames)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpEnvironmentNames.ResumeLayout(false);
            this.tpEnvironmentRoles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentRoles)).EndInit();
            this.gbTargetEnvironmentConfig.ResumeLayout(false);
            this.gbTargetEnvironmentConfig.PerformLayout();
            this.gbTrayMenuManagment.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblEnvironmentConfigFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dgvEnvironmentNames;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpEnvironmentNames;
        private System.Windows.Forms.TabPage tpEnvironmentRoles;
        private System.Windows.Forms.DataGridView dgvEnvironmentRoles;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox gbTargetEnvironmentConfig;
        private System.Windows.Forms.GroupBox gbTrayMenuManagment;
        private System.Windows.Forms.ComboBox ddlEnvironmentConfigFileNames;
        private System.Windows.Forms.LinkLabel linkCreateNewEnvironmentConfigFile;
        private System.Windows.Forms.Button btnEditEnvironmentConfigFile;
        private System.Windows.Forms.LinkLabel linkBrowseToConfigDirectory;
    }
}