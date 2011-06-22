namespace Quantae
{
    partial class GrammarInterfaceForm
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
            this.lstTopics = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.wordUserControl21 = new Quantae.WordUserControl2();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvSentences = new System.Windows.Forms.DataGridView();
            this.SentenceText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Translation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddTopic = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDocument2 = new System.Drawing.Printing.PrintDocument();
            this.btnEditTopic = new System.Windows.Forms.Button();
            this.btnAddWords = new System.Windows.Forms.Button();
            this.btnEditWord = new System.Windows.Forms.Button();
            this.btnAddSentence = new System.Windows.Forms.Button();
            this.btnEditSentence = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSentences)).BeginInit();
            this.SuspendLayout();
            // 
            // lstTopics
            // 
            this.lstTopics.FormattingEnabled = true;
            this.lstTopics.Location = new System.Drawing.Point(6, 19);
            this.lstTopics.Name = "lstTopics";
            this.lstTopics.Size = new System.Drawing.Size(506, 199);
            this.lstTopics.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.lstTopics);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 232);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Topics";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(759, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(731, 573);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sentence";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.textBox3);
            this.groupBox4.Location = new System.Drawing.Point(21, 177);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(677, 379);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 204);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(287, 271);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(21, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(677, 150);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(560, 103);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Tokenize";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sentence Translation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sentence Text";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(121, 59);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(515, 20);
            this.textBox4.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(121, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(515, 20);
            this.textBox1.TabIndex = 1;
            // 
            // wordUserControl21
            // 
            this.wordUserControl21.Location = new System.Drawing.Point(18, 250);
            this.wordUserControl21.Name = "wordUserControl21";
            this.wordUserControl21.Size = new System.Drawing.Size(522, 362);
            this.wordUserControl21.TabIndex = 5;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgvSentences);
            this.groupBox5.Location = new System.Drawing.Point(18, 615);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(522, 203);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sentences";
            // 
            // dgvSentences
            // 
            this.dgvSentences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSentences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SentenceText,
            this.Translation,
            this.Type});
            this.dgvSentences.Location = new System.Drawing.Point(7, 20);
            this.dgvSentences.Name = "dgvSentences";
            this.dgvSentences.Size = new System.Drawing.Size(499, 177);
            this.dgvSentences.TabIndex = 0;
            // 
            // SentenceText
            // 
            this.SentenceText.HeaderText = "Text";
            this.SentenceText.Name = "SentenceText";
            // 
            // Translation
            // 
            this.Translation.HeaderText = "Translation";
            this.Translation.Name = "Translation";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // btnAddTopic
            // 
            this.btnAddTopic.Location = new System.Drawing.Point(547, 31);
            this.btnAddTopic.Name = "btnAddTopic";
            this.btnAddTopic.Size = new System.Drawing.Size(142, 23);
            this.btnAddTopic.TabIndex = 7;
            this.btnAddTopic.Text = "Add Topic";
            this.btnAddTopic.UseVisualStyleBackColor = true;
            // 
            // btnEditTopic
            // 
            this.btnEditTopic.Location = new System.Drawing.Point(546, 60);
            this.btnEditTopic.Name = "btnEditTopic";
            this.btnEditTopic.Size = new System.Drawing.Size(142, 23);
            this.btnEditTopic.TabIndex = 7;
            this.btnEditTopic.Text = "Edit Topic";
            this.btnEditTopic.UseVisualStyleBackColor = true;
            // 
            // btnAddWords
            // 
            this.btnAddWords.Location = new System.Drawing.Point(546, 269);
            this.btnAddWords.Name = "btnAddWords";
            this.btnAddWords.Size = new System.Drawing.Size(142, 23);
            this.btnAddWords.TabIndex = 7;
            this.btnAddWords.Text = "Add Words";
            this.btnAddWords.UseVisualStyleBackColor = true;
            // 
            // btnEditWord
            // 
            this.btnEditWord.Location = new System.Drawing.Point(546, 298);
            this.btnEditWord.Name = "btnEditWord";
            this.btnEditWord.Size = new System.Drawing.Size(142, 23);
            this.btnEditWord.TabIndex = 7;
            this.btnEditWord.Text = "Edit Word";
            this.btnEditWord.UseVisualStyleBackColor = true;
            // 
            // btnAddSentence
            // 
            this.btnAddSentence.Location = new System.Drawing.Point(547, 635);
            this.btnAddSentence.Name = "btnAddSentence";
            this.btnAddSentence.Size = new System.Drawing.Size(142, 23);
            this.btnAddSentence.TabIndex = 7;
            this.btnAddSentence.Text = "Add Sentence";
            this.btnAddSentence.UseVisualStyleBackColor = true;
            // 
            // btnEditSentence
            // 
            this.btnEditSentence.Location = new System.Drawing.Point(546, 664);
            this.btnEditSentence.Name = "btnEditSentence";
            this.btnEditSentence.Size = new System.Drawing.Size(142, 23);
            this.btnEditSentence.TabIndex = 7;
            this.btnEditSentence.Text = "Edit Sentence";
            this.btnEditSentence.UseVisualStyleBackColor = true;
            // 
            // GrammarInterfaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(701, 830);
            this.Controls.Add(this.btnEditSentence);
            this.Controls.Add(this.btnAddSentence);
            this.Controls.Add(this.btnEditWord);
            this.Controls.Add(this.btnAddWords);
            this.Controls.Add(this.btnEditTopic);
            this.Controls.Add(this.btnAddTopic);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.wordUserControl21);
            this.Controls.Add(this.groupBox1);
            this.Name = "GrammarInterfaceForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSentences)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstTopics;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox1;
        private WordUserControl2 wordUserControl21;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvSentences;
        private System.Windows.Forms.DataGridViewTextBoxColumn SentenceText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Translation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.Button btnAddTopic;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Drawing.Printing.PrintDocument printDocument2;
        private System.Windows.Forms.Button btnEditTopic;
        private System.Windows.Forms.Button btnAddWords;
        private System.Windows.Forms.Button btnEditWord;
        private System.Windows.Forms.Button btnAddSentence;
        private System.Windows.Forms.Button btnEditSentence;
    }
}