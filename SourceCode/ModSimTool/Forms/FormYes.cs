using System;
using System.Windows.Forms;

namespace ModSimTool
{
    public partial class FormYes : Form
    {
        public FormYes(string messageStr)
        {
            InitializeComponent();

            lblMessage2.Text = messageStr;

            int messWidth = messageStr.Length;
            Width = messWidth * 15 + 180;
        }
    }
}