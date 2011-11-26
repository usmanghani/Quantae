namespace DataValidator
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTopics = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTopicParsingLog = new System.Windows.Forms.TextBox();
            this.btnLoadTopicInputFile = new System.Windows.Forms.Button();
            this.txtTopicFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabSentences = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSentenceParsingLog = new System.Windows.Forms.TextBox();
            this.btnOpenSentencesFolder = new System.Windows.Forms.Button();
            this.txtSentenceFolderName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabTopics.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabSentences.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTopics);
            this.tabControl1.Controls.Add(this.tabSentences);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(465, 349);
            this.tabControl1.TabIndex = 0;
            // 
            // tabTopics
            // 
            this.tabTopics.Controls.Add(this.groupBox1);
            this.tabTopics.Controls.Add(this.btnLoadTopicInputFile);
            this.tabTopics.Controls.Add(this.txtTopicFileName);
            this.tabTopics.Controls.Add(this.label1);
            this.tabTopics.Location = new System.Drawing.Point(4, 22);
            this.tabTopics.Name = "tabTopics";
            this.tabTopics.Size = new System.Drawing.Size(457, 323);
            this.tabTopics.TabIndex = 1;
            this.tabTopics.Text = "Topics";
            this.tabTopics.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTopicParsingLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 271);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // txtTopicParsingLog
            // 
            this.txtTopicParsingLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTopicParsingLog.Location = new System.Drawing.Point(6, 19);
            this.txtTopicParsingLog.Multiline = true;
            this.txtTopicParsingLog.Name = "txtTopicParsingLog";
            this.txtTopicParsingLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTopicParsingLog.Size = new System.Drawing.Size(416, 246);
            this.txtTopicParsingLog.TabIndex = 4;
            // 
            // btnLoadTopicInputFile
            // 
            this.btnLoadTopicInputFile.Location = new System.Drawing.Point(407, 4);
            this.btnLoadTopicInputFile.Name = "btnLoadTopicInputFile";
            this.btnLoadTopicInputFile.Size = new System.Drawing.Size(33, 23);
            this.btnLoadTopicInputFile.TabIndex = 2;
            this.btnLoadTopicInputFile.Text = "...";
            this.btnLoadTopicInputFile.UseVisualStyleBackColor = true;
            this.btnLoadTopicInputFile.Click += new System.EventHandler(this.btnLoadTopicInputFile_Click);
            // 
            // txtTopicFileName
            // 
            this.txtTopicFileName.Enabled = false;
            this.txtTopicFileName.Location = new System.Drawing.Point(95, 4);
            this.txtTopicFileName.Name = "txtTopicFileName";
            this.txtTopicFileName.Size = new System.Drawing.Size(306, 20);
            this.txtTopicFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Topic Input File";
            // 
            // tabSentences
            // 
            this.tabSentences.Controls.Add(this.groupBox2);
            this.tabSentences.Controls.Add(this.btnOpenSentencesFolder);
            this.tabSentences.Controls.Add(this.txtSentenceFolderName);
            this.tabSentences.Controls.Add(this.label2);
            this.tabSentences.Location = new System.Drawing.Point(4, 22);
            this.tabSentences.Name = "tabSentences";
            this.tabSentences.Padding = new System.Windows.Forms.Padding(3);
            this.tabSentences.Size = new System.Drawing.Size(457, 323);
            this.tabSentences.TabIndex = 0;
            this.tabSentences.Text = "Sentences";
            this.tabSentences.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSentenceParsingLog);
            this.groupBox2.Location = new System.Drawing.Point(15, 44);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(428, 271);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // txtSentenceParsingLog
            // 
            this.txtSentenceParsingLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSentenceParsingLog.Location = new System.Drawing.Point(6, 19);
            this.txtSentenceParsingLog.Multiline = true;
            this.txtSentenceParsingLog.Name = "txtSentenceParsingLog";
            this.txtSentenceParsingLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSentenceParsingLog.Size = new System.Drawing.Size(416, 246);
            this.txtSentenceParsingLog.TabIndex = 4;
            // 
            // btnOpenSentencesFolder
            // 
            this.btnOpenSentencesFolder.Location = new System.Drawing.Point(410, 15);
            this.btnOpenSentencesFolder.Name = "btnOpenSentencesFolder";
            this.btnOpenSentencesFolder.Size = new System.Drawing.Size(33, 23);
            this.btnOpenSentencesFolder.TabIndex = 5;
            this.btnOpenSentencesFolder.Text = "...";
            this.btnOpenSentencesFolder.UseVisualStyleBackColor = true;
            this.btnOpenSentencesFolder.Click += new System.EventHandler(this.btnOpenSentencesFolder_Click);
            // 
            // txtSentenceFolderName
            // 
            this.txtSentenceFolderName.Enabled = false;
            this.txtSentenceFolderName.Location = new System.Drawing.Point(98, 15);
            this.txtSentenceFolderName.Name = "txtSentenceFolderName";
            this.txtSentenceFolderName.Size = new System.Drawing.Size(306, 20);
            this.txtSentenceFolderName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Topic Input File";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 349);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Data Validator for Quantae";
            this.tabControl1.ResumeLayout(false);
            this.tabTopics.ResumeLayout(false);
            this.tabTopics.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabSentences.ResumeLayout(false);
            this.tabSentences.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTopics;
        private System.Windows.Forms.TabPage tabSentences;
        private System.Windows.Forms.Button btnLoadTopicInputFile;
        private System.Windows.Forms.TextBox txtTopicFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtTopicParsingLog;
        private System.Windows.Forms.Button btnOpenSentencesFolder;
        private System.Windows.Forms.TextBox txtSentenceFolderName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSentenceParsingLog;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

