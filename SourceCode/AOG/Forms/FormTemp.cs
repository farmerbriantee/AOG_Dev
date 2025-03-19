using System;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormTemp : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormTemp(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void FormToolPivot_Load(object sender, EventArgs e)
        {
        }
    }
}