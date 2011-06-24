using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Quantae.DataModel;

namespace Quantae
{
    public partial class VerbConjugationControl : UserControl
    {
        public VerbConjugationControl()
        {
            InitializeComponent();
        }

        public PersonRule PersonRule { get; set; }
        public TenseRule TenseRule { get; set; }
        public NumberRule NumberRule { get; set; }
        public GenderRule GenderRule { get; set; }

        private void VerbConjugationControl_Load(object sender, EventArgs e)
        {
            cmbGender.DataSource = Enum.GetValues(typeof(GenderRule));
            cmbNumber.DataSource = Enum.GetValues(typeof(NumberRule));
            cmbTenseRule.DataSource = Enum.GetValues(typeof(TenseRule));
            cmbPersonRule.DataSource = Enum.GetValues(typeof(PersonRule));
        }

        private void cmbPersonRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            PersonRule = (PersonRule)Enum.Parse(typeof(PersonRule), cmbPersonRule.SelectedItem.ToString());
        }

        private void cmbTenseRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            TenseRule = (TenseRule)Enum.Parse(typeof(TenseRule), cmbTenseRule.SelectedItem.ToString());
        }

        private void cmbNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            NumberRule = (NumberRule)Enum.Parse(typeof(NumberRule), cmbNumber.SelectedItem.ToString());
        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenderRule = (GenderRule)Enum.Parse(typeof(GenderRule), cmbGender.SelectedItem.ToString());
        }
    }
}
