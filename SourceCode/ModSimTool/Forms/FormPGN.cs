﻿using System;
using System.Windows.Forms;

namespace ModSimTool
{
    public partial class FormPGN: Form
    {
        public FormPGN()
        {
            InitializeComponent();
        }

        private void btnSerialOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}