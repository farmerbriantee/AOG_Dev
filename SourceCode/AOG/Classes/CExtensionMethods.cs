using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace AOG
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

    public class RepeatButton : Button
    {
        private Timer m_timerRepeater;

        private IContainer m_components;

        private bool m_disposed = false;

        private MouseEventArgs m_mouseDownArgs = null;

        public int InitialDelay = 400;

        public int RepeatInterval = 62;

        public RepeatButton()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.m_components = new System.ComponentModel.Container();
            this.m_timerRepeater = new System.Windows.Forms.Timer(this.m_components);
            base.SuspendLayout();
            this.m_timerRepeater.Tick += new System.EventHandler(timerRepeater_Tick);
            base.ResumeLayout(false);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            m_mouseDownArgs = mevent;
            m_timerRepeater.Enabled = false;
            timerRepeater_Tick(null, EventArgs.Empty);
        }

        private void timerRepeater_Tick(object sender, EventArgs e)
        {
            base.OnMouseDown(m_mouseDownArgs);
            if (m_timerRepeater.Enabled)
            {
                m_timerRepeater.Interval = RepeatInterval;
            }
            else
            {
                m_timerRepeater.Interval = InitialDelay;
            }

            m_timerRepeater.Enabled = true;
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            m_timerRepeater.Enabled = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (m_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (m_components != null)
                {
                    m_components.Dispose();
                }

                m_timerRepeater.Dispose();
            }

            m_disposed = true;
            base.Dispose(disposing);
        }

        /*
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            GraphicsPath grPath = new GraphicsPath();
            grPath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(grPath);
            base.OnPaint(e);
        }
        */
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

    public class NudlessNumericUpDown : Button
    {
        private double _value = double.NaN;
        private double minimum = 0;
        private double maximum = 100;
        private int decimalPlaces = 0;
        private bool initializing = true;
        private string format = "0";
        private EventHandler onValueChanged;
        private UnitMode mode;

        public NudlessNumericUpDown()
        {
            base.TextAlign = ContentAlignment.MiddleCenter;
            base.BackColor = SystemColors.Control;
            base.ForeColor = Color.Black;
            base.UseVisualStyleBackColor = false;

            base.FlatStyle = FlatStyle.Flat;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            base.Font = new System.Drawing.Font("Tahoma", this.Height / 2, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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

                    onValueChanged?.Invoke(this, EventArgs.Empty);
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
                RefreshDesigner();
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
                if (value < minimum)
                {
                    value = minimum;
                }
                else if (value > maximum)
                {
                    value = maximum;
                }

                _value = value;
                initializing = false;
                UpdateEditText();
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

                RefreshDesigner();
            }
        }

        // Force Visual Studio Designer to update when the property changes
        private void RefreshDesigner()
        {
            if (DesignMode)
            {
                //base.AutoSize = false; // Ensures it fits in one line
                if (decimalPlaces > 0)
                    base.Text = minimum.ToString("0.0#######") + "|" + maximum.ToString("0.0#######") + "|" + mode.ToString();
                else
                    base.Text = minimum.ToString("0") + "|" + maximum.ToString("0") + "|" + mode.ToString();

                var host = (IComponentChangeService)GetService(typeof(IComponentChangeService));
                host?.OnComponentChanged(this, null, null, null);
                Parent?.Invalidate(); // Force parent container to redraw
                Parent?.Update();
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

                RefreshDesigner();
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

                for (int i = 0; i < decimalPlaces; i++)
                {
                    if (i == 0)
                        format = "0.0";
                    else
                        format += "0";
                }

                if (!initializing)
                    UpdateEditText();
                
                RefreshDesigner();
            }
        }

        public override string ToString()
        {
            return base.ToString() + ", Minimum = " + minimum.ToString("0.0") + ", Maximum = " + maximum.ToString("0.0");
        }

        protected void UpdateEditText()
        {
            if (mode == UnitMode.None)
                base.Text = _value.ToString(format);
            else
            {
                if (Mode == UnitMode.Small)
                    base.Text = (_value * glm.m2InchOrCm).ToString(format);
                else if (Mode == UnitMode.Large)
                    base.Text = (_value * glm.m2FtOrM).ToString(format);
                else
                    base.Text = (_value * glm.kmhToMphOrKmh).ToString(format);
            }
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

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        public static List<vec3> OffsetLine(this List<vec3> points, double distance, double minDist, bool loop)
        {
            points.CalculateHeadings(loop);

            var result = new List<vec3>();
            //countExit the points from the boundary
            int count = points.Count;

            double distSq = distance * distance - 0.0001;

            //make the boundary tram outer array
            for (int i = 0; i < count; i++)
            {
                //calculate the point inside the boundary
                var easting = points[i].easting + (Math.Cos(points[i].heading) * distance);
                var northing = points[i].northing - (Math.Sin(points[i].heading) * distance);

                bool Add = true;

                for (int j = 0; j < count; j++)
                {
                    double check = glm.DistanceSquared(northing, easting, points[j].northing, points[j].easting);
                    if (check < distSq)
                    {
                        Add = false;
                        break;
                    }
                }

                if (Add)
                {
                    if (result.Count > 0)
                    {
                        double dist = glm.DistanceSquared(northing, easting, result[result.Count - 1].northing, result[result.Count - 1].easting);
                        if (dist > minDist)
                            result.Add(new vec3(easting, northing));
                    }
                    else
                        result.Add(new vec3(easting, northing));
                }
            }

            return result;
        }

        public static void CalculateHeadings(this List<vec3> points, bool loop)
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = points.Count;

            if (cnt > 1)
            {
                vec3[] arr = new vec3[cnt];
                cnt--;
                points.CopyTo(arr);
                points.Clear();

                //first point needs last, first, second points
                vec3 pt3 = arr[0];
                if (loop)
                    pt3.heading = Math.Atan2(arr[1].easting - arr[cnt].easting, arr[1].northing - arr[cnt].northing);
                else
                    pt3.heading = Math.Atan2(arr[1].easting - arr[0].easting, arr[1].northing - arr[0].northing);

                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                points.Add(pt3);

                //middle points
                for (int i = 1; i < cnt; i++)
                {
                    pt3 = arr[i];
                    pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                    if (pt3.heading < 0) pt3.heading += glm.twoPI;
                    points.Add(pt3);
                }

                //last and first point
                pt3 = arr[cnt];
                if (loop)
                    pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 1].easting, arr[0].northing - arr[cnt - 1].northing);
                else
                    pt3.heading = Math.Atan2(arr[cnt].easting - arr[cnt - 1].easting, arr[cnt].northing - arr[cnt - 1].northing);

                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                points.Add(pt3);
            }
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