#region Copyright & license notice
/*
 * Copyright: Copyright (c) 2007 Amazon Technologies, Inc.
 * License:   Apache License, Version 2.0
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Amazon.WebServices.MechanicalTurk;

namespace Samples.CreateHitUI
{
    public partial class LoginForm : Form
    {
        private MTurkConfig cfg;

        public LoginForm(MTurkConfig config)
        {
            InitializeComponent();

            cfg = config;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.textBoxID.Text = cfg.AccessKeyId;
            this.textBoxKey.Text = cfg.SecretAccessKey;
            this.textBoxURL.Text = cfg.ServiceEndpoint;
        }

        private void buttonContinue_Click(object sender, EventArgs e)
        {
            cfg.ServiceEndpoint = this.textBoxURL.Text;
            cfg.SecretAccessKey = this.textBoxKey.Text;
            cfg.AccessKeyId = this.textBoxID.Text;

            this.Close();
        }
    }
}