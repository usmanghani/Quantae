namespace Quantae
{
    partial class WordUserControl2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvWords = new System.Windows.Forms.DataGridView();
            this.Text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Translation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WordType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.WordSubtype = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DefinitenessRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NounGenderRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NounNumberRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VerbNumberRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VerbGenderRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VerbTenseRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VerbPersonRule = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWords)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvWords);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(509, 351);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Words";
            // 
            // dgvWords
            // 
            this.dgvWords.AllowUserToOrderColumns = true;
            this.dgvWords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Text,
            this.Translation,
            this.WordType,
            this.WordSubtype,
            this.DefinitenessRule,
            this.NounGenderRule,
            this.NounNumberRule,
            this.VerbNumberRule,
            this.VerbGenderRule,
            this.VerbTenseRule,
            this.VerbPersonRule});
            this.dgvWords.Location = new System.Drawing.Point(7, 20);
            this.dgvWords.Name = "dgvWords";
            this.dgvWords.Size = new System.Drawing.Size(490, 325);
            this.dgvWords.TabIndex = 0;
            // 
            // Text
            // 
            this.Text.HeaderText = "Text";
            this.Text.Name = "Text";
            // 
            // Translation
            // 
            this.Translation.HeaderText = "Translation";
            this.Translation.Name = "Translation";
            // 
            // WordType
            // 
            this.WordType.HeaderText = "WordType";
            this.WordType.Name = "WordType";
            // 
            // WordSubtype
            // 
            this.WordSubtype.HeaderText = "Subtype";
            this.WordSubtype.Name = "WordSubtype";
            // 
            // DefinitenessRule
            // 
            this.DefinitenessRule.HeaderText = "Definitess";
            this.DefinitenessRule.Name = "DefinitenessRule";
            // 
            // NounGenderRule
            // 
            this.NounGenderRule.HeaderText = "NounGender";
            this.NounGenderRule.Name = "NounGenderRule";
            this.NounGenderRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NounGenderRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // NounNumberRule
            // 
            this.NounNumberRule.HeaderText = "NounNumber";
            this.NounNumberRule.Name = "NounNumberRule";
            this.NounNumberRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.NounNumberRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // VerbNumberRule
            // 
            this.VerbNumberRule.HeaderText = "VerbNumber";
            this.VerbNumberRule.Name = "VerbNumberRule";
            this.VerbNumberRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VerbNumberRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // VerbGenderRule
            // 
            this.VerbGenderRule.HeaderText = "VerbGender";
            this.VerbGenderRule.Name = "VerbGenderRule";
            this.VerbGenderRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VerbGenderRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // VerbTenseRule
            // 
            this.VerbTenseRule.HeaderText = "VerbTense";
            this.VerbTenseRule.Name = "VerbTenseRule";
            this.VerbTenseRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VerbTenseRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // VerbPersonRule
            // 
            this.VerbPersonRule.HeaderText = "VerbPerson";
            this.VerbPersonRule.Name = "VerbPersonRule";
            this.VerbPersonRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.VerbPersonRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // WordUserControl2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "WordUserControl2";
            this.Size = new System.Drawing.Size(522, 362);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWords)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvWords;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text;
        private System.Windows.Forms.DataGridViewTextBoxColumn Translation;
        private System.Windows.Forms.DataGridViewComboBoxColumn WordType;
        private System.Windows.Forms.DataGridViewComboBoxColumn WordSubtype;
        private System.Windows.Forms.DataGridViewComboBoxColumn DefinitenessRule;
        private System.Windows.Forms.DataGridViewComboBoxColumn NounGenderRule;
        private System.Windows.Forms.DataGridViewComboBoxColumn NounNumberRule;
        private System.Windows.Forms.DataGridViewComboBoxColumn VerbNumberRule;
        private System.Windows.Forms.DataGridViewComboBoxColumn VerbGenderRule;
        private System.Windows.Forms.DataGridViewComboBoxColumn VerbTenseRule;
        private System.Windows.Forms.DataGridViewComboBoxColumn VerbPersonRule;
    }
}
