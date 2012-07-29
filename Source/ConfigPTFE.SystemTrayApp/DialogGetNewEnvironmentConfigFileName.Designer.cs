namespace ConfigPTFE.SystemTrayApp
{
    partial class DialogGetNewEnvironmentConfigFileName
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbNewEnvConfigFilename = new System.Windows.Forms.TextBox();
            this.lblNewEnvConfigFilename = new System.Windows.Forms.Label();
            this.lblNewEnvConfigFilenameExample = new System.Windows.Forms.Label();
            this.lblNewEnvConfigFilenameExtension = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(209, 89);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(290, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbNewEnvConfigFilename
            // 
            this.tbNewEnvConfigFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorProvider1.SetIconAlignment(this.tbNewEnvConfigFilename, System.Windows.Forms.ErrorIconAlignment.BottomLeft);
            this.tbNewEnvConfigFilename.Location = new System.Drawing.Point(16, 33);
            this.tbNewEnvConfigFilename.Name = "tbNewEnvConfigFilename";
            this.tbNewEnvConfigFilename.Size = new System.Drawing.Size(218, 22);
            this.tbNewEnvConfigFilename.TabIndex = 0;
            this.tbNewEnvConfigFilename.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbNewEnvConfigFilename.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewEnvConfigFilename_Validating);
            this.tbNewEnvConfigFilename.Validated += new System.EventHandler(this.tbNewEnvConfigFilename_Validated);
            // 
            // lblNewEnvConfigFilename
            // 
            this.lblNewEnvConfigFilename.AutoSize = true;
            this.lblNewEnvConfigFilename.Location = new System.Drawing.Point(13, 13);
            this.lblNewEnvConfigFilename.Name = "lblNewEnvConfigFilename";
            this.lblNewEnvConfigFilename.Size = new System.Drawing.Size(296, 17);
            this.lblNewEnvConfigFilename.TabIndex = 2;
            this.lblNewEnvConfigFilename.Text = "New Application Environment Config filename:";
            // 
            // lblNewEnvConfigFilenameExample
            // 
            this.lblNewEnvConfigFilenameExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNewEnvConfigFilenameExample.AutoSize = true;
            this.lblNewEnvConfigFilenameExample.Location = new System.Drawing.Point(148, 58);
            this.lblNewEnvConfigFilenameExample.Name = "lblNewEnvConfigFilenameExample";
            this.lblNewEnvConfigFilenameExample.Size = new System.Drawing.Size(217, 17);
            this.lblNewEnvConfigFilenameExample.TabIndex = 2;
            this.lblNewEnvConfigFilenameExample.Text = "(ex. common.environment.config)";
            // 
            // lblNewEnvConfigFilenameExtension
            // 
            this.lblNewEnvConfigFilenameExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNewEnvConfigFilenameExtension.AutoSize = true;
            this.lblNewEnvConfigFilenameExtension.Location = new System.Drawing.Point(233, 36);
            this.lblNewEnvConfigFilenameExtension.Name = "lblNewEnvConfigFilenameExtension";
            this.lblNewEnvConfigFilenameExtension.Size = new System.Drawing.Size(132, 17);
            this.lblNewEnvConfigFilenameExtension.TabIndex = 3;
            this.lblNewEnvConfigFilenameExtension.Text = ".environment.config";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // DialogGetNewEnvironmentConfigFileName
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(377, 124);
            this.Controls.Add(this.lblNewEnvConfigFilenameExtension);
            this.Controls.Add(this.lblNewEnvConfigFilenameExample);
            this.Controls.Add(this.lblNewEnvConfigFilename);
            this.Controls.Add(this.tbNewEnvConfigFilename);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DialogGetNewEnvironmentConfigFileName";
            this.Text = "Filename";
            this.Load += new System.EventHandler(this.DialogGetNewEnvironmentConfigFileName_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblNewEnvConfigFilename;
        private System.Windows.Forms.Label lblNewEnvConfigFilenameExample;
        private System.Windows.Forms.Label lblNewEnvConfigFilenameExtension;
        protected internal System.Windows.Forms.TextBox tbNewEnvConfigFilename;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}