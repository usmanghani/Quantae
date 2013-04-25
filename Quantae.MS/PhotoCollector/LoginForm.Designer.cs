namespace PhotoCollector
{
    partial class LoginForm
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
            this.lblAccessKeyId = new System.Windows.Forms.Label();
            this.txtAccessKeyId = new System.Windows.Forms.TextBox();
            this.txtSecretAccessKey = new System.Windows.Forms.TextBox();
            this.lblSecretAccessKey = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.chkSandbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblAccessKeyId
            // 
            this.lblAccessKeyId.AutoSize = true;
            this.lblAccessKeyId.Location = new System.Drawing.Point(13, 13);
            this.lblAccessKeyId.Name = "lblAccessKeyId";
            this.lblAccessKeyId.Size = new System.Drawing.Size(103, 13);
            this.lblAccessKeyId.TabIndex = 0;
            this.lblAccessKeyId.Text = "AWS Access Key Id";
            // 
            // txtAccessKeyId
            // 
            this.txtAccessKeyId.Location = new System.Drawing.Point(16, 29);
            this.txtAccessKeyId.Name = "txtAccessKeyId";
            this.txtAccessKeyId.Size = new System.Drawing.Size(299, 20);
            this.txtAccessKeyId.TabIndex = 1;
            // 
            // txtSecretAccessKey
            // 
            this.txtSecretAccessKey.Location = new System.Drawing.Point(16, 85);
            this.txtSecretAccessKey.Name = "txtSecretAccessKey";
            this.txtSecretAccessKey.Size = new System.Drawing.Size(299, 20);
            this.txtSecretAccessKey.TabIndex = 2;
            // 
            // lblSecretAccessKey
            // 
            this.lblSecretAccessKey.AutoSize = true;
            this.lblSecretAccessKey.Location = new System.Drawing.Point(13, 69);
            this.lblSecretAccessKey.Name = "lblSecretAccessKey";
            this.lblSecretAccessKey.Size = new System.Drawing.Size(125, 13);
            this.lblSecretAccessKey.TabIndex = 3;
            this.lblSecretAccessKey.Text = "AWS Secret Access Key";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(239, 125);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // chkSandbox
            // 
            this.chkSandbox.AutoSize = true;
            this.chkSandbox.Location = new System.Drawing.Point(16, 125);
            this.chkSandbox.Name = "chkSandbox";
            this.chkSandbox.Size = new System.Drawing.Size(74, 17);
            this.chkSandbox.TabIndex = 5;
            this.chkSandbox.Text = "Sandbox?";
            this.chkSandbox.UseVisualStyleBackColor = true;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 173);
            this.Controls.Add(this.chkSandbox);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblSecretAccessKey);
            this.Controls.Add(this.txtSecretAccessKey);
            this.Controls.Add(this.txtAccessKeyId);
            this.Controls.Add(this.lblAccessKeyId);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAccessKeyId;
        private System.Windows.Forms.TextBox txtAccessKeyId;
        private System.Windows.Forms.TextBox txtSecretAccessKey;
        private System.Windows.Forms.Label lblSecretAccessKey;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.CheckBox chkSandbox;
    }
}