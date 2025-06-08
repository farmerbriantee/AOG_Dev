using AOG.Classes;
using AOG.Logic;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AOG.Forms.Field
{
    /// <summary>
    /// Form that allows the user to preview and download their own AgShare fields,
    /// with OpenGL rendering of boundaries and AB lines.
    /// </summary>
    public partial class FormAgShareDownloader : Form
    {
        private readonly FormGPS gps;
        private readonly CAgShareDownloader downloader;

        public FormAgShareDownloader(FormGPS gpsContext)
        {
            InitializeComponent();
            gps = gpsContext;
            downloader = new CAgShareDownloader();
        }

        // Initializes the form and loads field list
        private async void FormAgShareDownloader_Load(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();
            GL.ClearColor(Color.DarkSlateGray);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            glControl1.SwapBuffers();

            try
            {
                var fields = await downloader.GetOwnFieldsAsync();
                lbFields.BeginUpdate();
                foreach (var field in fields)
                {
                    var item = new ListViewItem(field.Name) { Tag = field };
                    lbFields.Items.Add(item);
                }
                lbFields.EndUpdate();

                if (lbFields.Items.Count > 0)
                    lbFields.Items[0].Selected = true;
            }
            catch
            {
                gps.TimedMessageBox(3000, "AgShare", "Failed to load field list.");
            }
        }

        // Called when the user selects a field from the list
        private async void lbFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFields.SelectedItems.Count == 0) return;

            var dto = lbFields.SelectedItems[0].Tag as AgShareGetOwnFieldDto;
            if (dto == null) return;

            lblSelectedField.Text = "Selected Field: " + dto.Name;
            lblSelectedField.ForeColor = Color.Red;

            var previewDto = await downloader.DownloadFieldPreviewAsync(dto.Id);
            var localModel = AgShareFieldParser.Parse(previewDto); // already converted to NE

            RenderField(localModel);
        }

        // Called when the user clicks the download/open button
        private async void btnOpen_Click(object sender, EventArgs e)
        {
            if (lbFields.SelectedItems.Count == 0)
            {
                gps.TimedMessageBox(2000, "AgShare", "No field selected.");
                return;
            }

            var selected = lbFields.SelectedItems[0].Tag as AgShareGetOwnFieldDto;
            if (selected == null)
            {
                gps.TimedMessageBox(2000, "AgShare", "Invalid selection.");
                return;
            }

            bool success = await downloader.DownloadAndSaveAsync(selected.Id);
            gps.TimedMessageBox(2000, "AgShare", success
                ? "Field downloaded and saved."
                : "Field download failed.");
        }

        // Closes the form
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region OpenGL

        // Renders the selected field using OpenGL
        private void RenderField(LocalFieldModel field)
        {
            glControl1.MakeCurrent();

            GL.ClearColor(0.12f, 0.12f, 0.12f, 1f); // anthracite background
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GetBounds(field.Boundaries, out double minX, out double minY, out double maxX, out double maxY);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            double marginX = (maxX - minX) * 0.05;
            double marginY = (maxY - minY) * 0.05;
            GL.Ortho(minX - marginX, maxX + marginX, minY - marginY, maxY + marginY, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw boundaries (lime green)
            GL.Color4(0f, 1f, 0f, 0.8f);
            foreach (var ring in field.Boundaries)
            {
                GL.Begin(PrimitiveType.LineLoop);
                foreach (var pt in ring)
                    GL.Vertex2(pt.Easting, pt.Northing);
                GL.End();
            }

            // Draw AB lines and Curves
            foreach (var ab in field.AbLines)
            {
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0F0F);
                GL.LineWidth(3.5f);

                if (ab.CurvePoints != null && ab.CurvePoints.Count > 0)
                {
                    // Curve → red dashed line
                    GL.Color4(1f, 0f, 0f, 0.9f);
                    GL.Begin(PrimitiveType.LineStrip);
                    foreach (var pt in ab.CurvePoints)
                        GL.Vertex2(pt.Easting, pt.Northing);
                    GL.End();
                }
                else
                {
                    // AB line → orange dashed line
                    GL.Color4(1f, 0.65f, 0f, 0.9f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(ab.PtA.Easting, ab.PtA.Northing);
                    GL.Vertex2(ab.PtB.Easting, ab.PtB.Northing);
                    GL.End();
                }

                GL.Disable(EnableCap.LineStipple);
            }

            glControl1.SwapBuffers();
        }

        // Computes min/max NE values for auto-scaling the viewport
        private void GetBounds(List<List<LocalPoint>> boundaries, out double minX, out double minY, out double maxX, out double maxY)
        {
            minX = minY = double.MaxValue;
            maxX = maxY = double.MinValue;

            foreach (var ring in boundaries)
            {
                foreach (var pt in ring)
                {
                    if (pt.Easting < minX) minX = pt.Easting;
                    if (pt.Easting > maxX) maxX = pt.Easting;
                    if (pt.Northing < minY) minY = pt.Northing;
                    if (pt.Northing > maxY) maxY = pt.Northing;
                }
            }
        }

        #endregion
    }
}
