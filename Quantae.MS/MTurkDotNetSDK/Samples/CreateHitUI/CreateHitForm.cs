#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amazon.WebServices.MechanicalTurk;
using Amazon.WebServices.MechanicalTurk.Domain;

namespace Samples.CreateHitUI
{
    public partial class CreateHitForm : Form
    {
        private SimpleClient client = new SimpleClient();

        public CreateHitForm()
        {
            InitializeComponent();
        }

        private void CreateHitForm_Load(object sender, EventArgs e)
        {
            // show login form first to configure settings
            LoginForm form = new LoginForm(client.Config);
            if (DialogResult.OK != form.ShowDialog(this))
            {
                this.Close();
            }

            this.dateTimePickerExpiration.Value = DateTime.Now.AddDays(1);
            this.dateTimePickerExpiration.MinDate = DateTime.Now;

            this.textBoxTitle.Focus();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            HIT hit = client.CreateHIT(
                null,
                this.textBoxTitle.Text,
                this.textBoxDescription.Text,
                null,
                this.textBoxQuestion.Text,
                decimal.Parse(this.maskedTextBox1.Text),
                (long)(this.numericUpDownTime.Value * 60),
                null,
                3600,
                (int)this.numericUpDownAssignements.Value,
                null,
                null,
                null);

            // put hit ID in clipboard (to e.g. be used in Reviewer sample)
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Text, hit.HITId);

            if (DialogResult.Yes == MessageBox.Show(this, 
                "Created HIT " + hit.HITId + "\n\nWould you like to view this hit?", 
                "Success", 
                MessageBoxButtons.YesNo))
            {
                ShowWorkerURL(hit);
            }
        }

        private void ShowWorkerURL(HIT h)
        {
            Process.Start(client.GetPreviewURL(h.HITTypeId));
        }
    }
}