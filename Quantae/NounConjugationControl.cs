using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Quantae
{
    public partial class NounConjugationControl : UserControl
    {
        public NounConjugationControl()
        {
            InitializeComponent();
        }

        private void NounConjugationControl_Load(object sender, EventArgs e)
        {
            cmbGender.DataSource = Enum.GetValues(typeof(GenderRule));
            cmbNumber.DataSource = Enum.GetValues(typeof(NumberRule));
        }
    }
}
