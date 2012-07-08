using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Amazon.WebServices.MechanicalTurk;

namespace PhotoCollector
{
    public partial class LoginForm : Form
    {
        MTurkConfig config;
        public LoginForm(MTurkConfig config)
        {
            this.config = config;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAccessKeyId.Text))
            {
                MessageBox.Show("Access Key Id is empty.");
                return;
            }

            if (string.IsNullOrEmpty(txtSecretAccessKey.Text))
            {
                MessageBox.Show("Secret Access Key is empty.");
                return;
            }

            config.AccessKeyId = txtAccessKeyId.Text;
            config.SecretAccessKey = txtSecretAccessKey.Text;

            config.ServiceEndpoint = string.Format("https://mechanicalturk{0}.amazonaws.com?Service=AWSMechanicalTurkRequester", chkSandbox.Checked ? ".sandbox" : string.Empty);

            this.Close();
        }
    }
}
