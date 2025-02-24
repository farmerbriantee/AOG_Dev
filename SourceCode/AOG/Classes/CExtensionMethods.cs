﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public class ListViewItemSorter : IComparer
    {
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewItemSorter(ListView lv)
        {
            lv.ListViewItemSorter = this;
            lv.ColumnClick += new ColumnClickEventHandler(listView_ColumnClick);

            // Initialize the column to '0'
            SortColumn = 0;

            // Initialize the sort order to 'none'
            Order = SortOrder.Ascending;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        private int SortColumn { set; get; }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        private SortOrder Order { set; get; }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ReverseSortOrderAndSort(e.Column, (ListView)sender);
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            if (decimal.TryParse(listviewX.SubItems[SortColumn].Text, out decimal dx) && decimal.TryParse(listviewY.SubItems[SortColumn].Text, out decimal dy))
            {
                //compare the 2 items as doubles
                compareResult = decimal.Compare(dx, dy);
            }
            else if (DateTime.TryParse(listviewX.SubItems[SortColumn].Text, out DateTime dtx) && DateTime.TryParse(listviewY.SubItems[SortColumn].Text, out DateTime dty))
            {
                //compare the 2 items as doubles
                compareResult = -DateTime.Compare(dtx, dty);
            }
            // When one is a number and the other not, return -1 to have the numbers on top (or bottom)
            else if (decimal.TryParse(listviewX.SubItems[SortColumn].Text, out dx))
            {
                compareResult = -1;
            }
            // When one is a number and the other not, return 1 to have the numbers on top (or bottom)
            else if (decimal.TryParse(listviewY.SubItems[SortColumn].Text, out dy))
            {
                compareResult = 1;
            }
            else
            {
                // Compare the two items
                compareResult = ObjectCompare.Compare(listviewX.SubItems[SortColumn].Text, listviewY.SubItems[SortColumn].Text);
            }

            // Calculate correct return value based on object comparison
            if (Order == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (Order == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return -compareResult;
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        private void ReverseSortOrderAndSort(int column, ListView lv)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (column == SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (Order == SortOrder.Ascending)
                    Order = SortOrder.Descending;
                else
                    Order = SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                SortColumn = column;
                Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lv.Sort();
        }
    }

    public class RoundButton : Button
    {
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            GraphicsPath grPath = new GraphicsPath();
            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(e);
        }
    }

    public enum UnitMode
    {
        None,
        Large,
        Small,
        Speed,
        Area,
        Distance,
        Temperature
    }

    public class NumericUnitModeConverter : EnumConverter
    {
        public NumericUnitModeConverter(Type type) : base(type) { }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new UnitMode[]
            {
                UnitMode.None,
                UnitMode.Large,
                UnitMode.Small,
                UnitMode.Speed
            });
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true; // Prevents entering custom values
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true; // Enables dropdown in Designer
    }

    public class NudlessNumericUpDown : Button, ISupportInitialize
    {
        private double _value = double.NaN;
        private double minimum = 0;
        private double maximum = 100;
        private double increment = 1;
        private int decimalPlaces = 0;
        private bool initializing = true;
        private string format = "0";
        private EventHandler onValueChanged;
        private UnitMode mode;

        public NudlessNumericUpDown()
        {
            base.TextAlign = ContentAlignment.MiddleCenter;
            base.BackColor = Color.AliceBlue;
            base.ForeColor = Color.Black;
            base.UseVisualStyleBackColor = false;

            base.FlatStyle = FlatStyle.Flat;

            //base.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        public event EventHandler ValueChanged { add => onValueChanged = (EventHandler)Delegate.Combine(onValueChanged, value); remove => onValueChanged = (EventHandler)Delegate.Remove(onValueChanged, value); }

        protected override void OnClick(EventArgs e)
        {
            var localMin = minimum;
            var localMax = maximum;
            var localVal = _value;

            if (mode == UnitMode.Small)
            {
                localMin *= glm.m2InchOrCm;
                localMax *= glm.m2InchOrCm;
                localVal *= glm.m2InchOrCm;
            }
            else if (mode == UnitMode.Large)
            {
                localMin *= glm.m2FtOrM;
                localMax *= glm.m2FtOrM;
                localVal *= glm.m2FtOrM;
            }
            else if (mode == UnitMode.Speed)
            {
                localMin *= glm.kmhToMphOrKmh;
                localMax *= glm.kmhToMphOrKmh;
                localVal *= glm.kmhToMphOrKmh;
            }
            localVal = Math.Round(localVal, decimalPlaces);

            using (FormNumeric form = new FormNumeric(localMin, localMax, localVal))
            {
                DialogResult result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    var localReturn = Math.Round(form.ReturnValue, decimalPlaces);

                    if (mode == UnitMode.Small)
                        Value = localReturn * glm.inchOrCm2m;
                    else if (mode == UnitMode.Large)
                        Value = localReturn * glm.ftOrMtoM;
                    else if (mode == UnitMode.Speed)
                        Value = localReturn * glm.mphOrKmhToKmh;
                    else
                        Value = localReturn;
                }
            }
        }

        [DefaultValue(typeof(UnitMode), "None")]
        [TypeConverter(typeof(NumericUnitModeConverter))] // Restricts designer dropdown
        public UnitMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }


        [Bindable(false)]
        [Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (DesignMode)
                {
                    //base.AutoSize = false; // Ensures it fits in one line
                    base.Text = minimum.ToString(format) + "|" + maximum.ToString(format)+ "|" + mode.ToString();
                }
                else if (!initializing)
                {
                    if (value < minimum)
                    {
                        value = minimum;
                    }
                    else if (value > maximum)
                    {
                        value = maximum;
                    }
                    
                    if (value != _value)
                    {
                        bool isnan = double.IsNaN(_value);
                        _value = value;

                        if (!isnan && onValueChanged != null)
                        {
                            onValueChanged(this, EventArgs.Empty);
                        }
                        UpdateEditText();
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (DesignMode)
            {
                using (Graphics g = CreateGraphics())
                {
                    float fontSize = base.Font.Size;
                    SizeF textSize = g.MeasureString(base.Text, base.Font);

                    while ((textSize.Width > base.Width - 10 || textSize.Height > base.Height - 5) && fontSize > 5)
                    {
                        fontSize -= 0.5f;
                        textSize = g.MeasureString(base.Text, new Font(base.Font.FontFamily, fontSize, base.Font.Style));
                    }

                    base.Font = new Font(base.Font.FontFamily, fontSize, base.Font.Style);
                }
            }
            base.OnPaint(pevent);
        }

        [DefaultValue(typeof(double), "0")]
        public double Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
                if (minimum > maximum)
                {
                    maximum = value;
                }
                if (!initializing)
                    Value = _value;
            }
        }

        [DefaultValue(typeof(double), "100")]
        public double Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
                if (minimum > maximum)
                {
                    minimum = maximum;
                }

                if (!initializing)
                    Value = _value;
            }
        }

        [DefaultValue(typeof(double), "1")]
        public double Increment
        {
            get
            {
                return increment;
            }
            set
            {
                increment = value;
            }
        }


        [DefaultValue(typeof(int), "0")]
        public int DecimalPlaces
        {
            get
            {
                return decimalPlaces;
            }
            set
            {
                decimalPlaces = value;

                format = "0";

                if (decimalPlaces > 0)
                    format = "0.";

                for (int i = 0; i < decimalPlaces; i++)
                {
                    format += "0";
                }

                if (!initializing)
                    UpdateEditText();
            }
        }

        public void BeginInit()
        {
            initializing = true;
        }

        public void EndInit()
        {
            initializing = false;

            Value = _value;
        }

        public override string ToString()
        {
            string text = base.ToString();
            return text + ", Minimum = " + minimum.ToString("0.0") + ", Maximum = " + maximum.ToString("0.0");
        }

        protected void UpdateEditText()
        {
            if (mode == UnitMode.None)
                base.Text = _value.ToString(format);
            if (Mode == UnitMode.Small)
                base.Text = (_value * glm.m2InchOrCm).ToString(format);
            else if (Mode == UnitMode.Large)
                base.Text = (_value * glm.m2FtOrM).ToString(format);
            else
                base.Text = (_value * glm.kmhToMphOrKmh).ToString(format);
        }

        [Bindable(false)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor { get => base.BackColor; set => base.BackColor = value; }

        [Bindable(false)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new event EventHandler TextChanged { add => base.TextChanged += value; remove => base.TextChanged -= value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ContentAlignment TextAlign { get => base.TextAlign; set => base.TextAlign = value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override RightToLeft RightToLeft { get => base.RightToLeft; set => base.RightToLeft = value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool AutoSize { get => base.AutoSize; set => base.AutoSize = value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Cursor Cursor { get => base.Cursor; set => base.Cursor = value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool UseVisualStyleBackColor { get => base.UseVisualStyleBackColor; set => base.UseVisualStyleBackColor = value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor { get => base.ForeColor; set => base.ForeColor = value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new event EventHandler Click { add => base.Click += value; remove => base.Click -= value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new event EventHandler Enter { add => base.Enter += value; remove => base.Enter -= value; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle { get => base.FlatStyle; set => base.FlatStyle = value; }

        public override Font Font { get => base.Font; set => base.Font = value; }
    }

    public static class CExtensionMethods
    {
        /// <summary>
        /// Sets the progress bar value, without using 'Windows Aero' animation.
        /// This is to work around a known WinForms issue where the progress bar
        /// is slow to update.
        /// </summary>
        public static void SetProgressNoAnimation(this ProgressBar pb, int value)
        {
            // To get around the progressive animation, we need to move the
            // progress bar backwards.
            if (value == pb.Maximum)
            {
                // Special case as value can't be set greater than Maximum.
                pb.Maximum = value + 1;     // Temporarily Increase Maximum
                pb.Value = value + 1;       // Move past
                pb.Maximum = value;         // Reset maximum
            }
            else
            {
                pb.Value = value + 1;       // Move past
            }
            pb.Value = value;               // Move to correct value
        }

        public static Color CheckColorFor255(this Color color)
        {
            var currentR = color.R;
            var currentG = color.G;
            var currentB = color.B;

            if (currentR == 255) currentR = 254;
            if (currentG == 255) currentG = 254;
            if (currentB == 255) currentB = 254;

            return Color.FromArgb(color.A, currentR, currentG, currentB);
        }
    }

    //public class ExtendedPanel : Panel
    //{
    //    private const int WS_EX_TRANSPARENT = 0x20;
    //    public ExtendedPanel()
    //    {
    //        SetStyle(ControlStyles.Opaque, true);
    //    }

    //    private int opacity = 50;
    //    [DefaultValue(50)]
    //    public int Opacity
    //    {
    //        get
    //        {
    //            return this.opacity;
    //        }
    //        set
    //        {
    //            if (value < 0 || value > 100)
    //                throw new System.ArgumentException("value must be between 0 and 100");
    //            this.opacity = value;
    //        }
    //    }
    //    protected override CreateParams CreateParams
    //    {
    //        get
    //        {
    //            CreateParams cp = base.CreateParams;
    //            cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
    //            return cp;
    //        }
    //    }
    //    protected override void OnPaint(PaintEventArgs e)
    //    {
    //        using (var brush = new SolidBrush(Color.FromArgb(this.opacity * 255 / 100, this.BackColor)))
    //        {
    //            e.Graphics.FillRectangle(brush, this.ClientRectangle);
    //        }
    //        base.OnPaint(e);
    //    }
    //}
}