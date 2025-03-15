using AOG.Classes;
using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormNumeric : Form
    {
        private readonly double max;
        private readonly double min;
        private bool isFirstKey;

        public double ReturnValue { get; set; }

        public FormNumeric(double _min, double _max, double currentValue)
        {
            max = _max;
            min = _min;
            InitializeComponent();

            this.Text = "Enter Value";
            //fill in the display
            tboxNumber.Text = currentValue.ToString();

            isFirstKey = true;
        }

        private void FormNumeric_Load(object sender, EventArgs e)
        {
            lblMax.Text = max.ToString();
            lblMin.Text = min.ToString();
            tboxNumber.SelectionStart = tboxNumber.Text.Length;
            tboxNumber.SelectionLength = 0;
        }
        
        private void RaiseButtonPressed(char keyChar)
        {
            if (isFirstKey)
            {
                tboxNumber.Text = "";
                isFirstKey = false;
            }

            //clear the error as user entered new values
            if (tboxNumber.Text == gStr.Get(gs.gsError))
            {
                tboxNumber.Text = "";
                lblMin.ForeColor = SystemColors.ControlText;
                lblMax.ForeColor = SystemColors.ControlText;
            }

            //if its a number just add it
            if (Char.IsNumber(keyChar))
            {
                tboxNumber.Text += keyChar;
            }

            //Backspace key, remove 1 char
            else if (keyChar == 'B')
            {
                if (tboxNumber.Text.Length > 0)
                {
                    tboxNumber.Text = tboxNumber.Text.Remove(tboxNumber.Text.Length - 1);
                }
            }

            //decimal point
            else if (keyChar == '.')
            {
                //does it already have a decimal?
                if (!tboxNumber.Text.Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                {
                    tboxNumber.Text += Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                    //if decimal is first char, prefix with a zero
                    if (tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 0)
                    {
                        tboxNumber.Text = "0" + tboxNumber.Text;
                    }

                    //neg sign then added a decimal, insert a 0
                    if (tboxNumber.Text.IndexOf("-") == 0 && tboxNumber.Text.IndexOf(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator) == 1)
                    {
                        tboxNumber.Text = "-0" + Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    }
                }
            }

            //negative sign
            else if (keyChar == '-')
            {
                //If already has a negative don't add again
                if (!tboxNumber.Text.Contains("-"))
                {
                    //prefix the negative sign
                    tboxNumber.Text = "-" + tboxNumber.Text;
                }
                else
                {
                    //if already has one, take it away = +/- does that
                    if (tboxNumber.Text.StartsWith("-"))
                    {
                        tboxNumber.Text = tboxNumber.Text.Substring(1);
                    }
                }
            }

            //Exit or cancel
            else if (keyChar == 'X')
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }

            //clear whole display
            else if (keyChar == 'C')
            {
                tboxNumber.Text = "";
            }

            //ok button
            else if (keyChar == 'K')
            {
                //not ok if empty - just return
                if (tboxNumber.Text == "") return;

                //culture invariant parse to double
                double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

                //test if above or below min/max
                if (tryNumber < min)
                {
                    tboxNumber.Text = gStr.Get(gs.gsError);
                    lblMin.ForeColor = System.Drawing.Color.Red;
                }
                else if (tryNumber > max)
                {
                    tboxNumber.Text = gStr.Get(gs.gsError);
                    lblMax.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    //all good, return the value
                    this.ReturnValue = tryNumber;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }

            //Show the cursor
            tboxNumber.SelectionStart = tboxNumber.Text.Length;
            tboxNumber.SelectionLength = 0;
            tboxNumber.Focus();
        }

        private void BtnDistanceUPGN_MouseDown(object sender, MouseEventArgs e)
        {
            if (tboxNumber.Text == "" || tboxNumber.Text == "-" || tboxNumber.Text == gStr.Get(gs.gsError)) tboxNumber.Text = "0";
            double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

            tryNumber++;

            if (tryNumber > max) tryNumber = max;

            tboxNumber.Text = tryNumber.ToString();

            isFirstKey = false;
        }

        private void BtnDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            if (tboxNumber.Text == "" || tboxNumber.Text == "-" || tboxNumber.Text == gStr.Get(gs.gsError)) tboxNumber.Text = "0";
            double tryNumber = double.Parse(tboxNumber.Text, CultureInfo.CurrentCulture);

            tryNumber--;
            if (tryNumber < min) tryNumber = min;

            tboxNumber.Text = tryNumber.ToString();

            isFirstKey = false;
        }

        private void tboxNumber_Click(object sender, EventArgs e)
        {
            if (isFirstKey)
            {
                isFirstKey = false;
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('1');
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('2');
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('3');
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('4');
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('5');
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('6');
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('7');
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('8');
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('9');
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('0');
        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('-');
        }

        private void btnDecimal_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('.');
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('C');
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('B');
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('X');
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RaiseButtonPressed('K');
        }
    }
}