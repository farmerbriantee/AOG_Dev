using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Globalization;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;


namespace AgIO
{
    public partial class FormLoop
    {
        // GPSOut BackgroundWorker
        public static BackgroundWorker bgGPSOut = new BackgroundWorker();

        private void bgGPSOut_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            worker.ReportProgress(GPSOut.BuildSentences((int)(FormLoop.gpsHz * 0.1 + 0.3)));

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void bgGPSOut_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > -1)
            {
                traffic.cntrGPS_OutSerial += e.ProgressPercentage;
            }
            else
            {
                lblSlowGPSOut.Text = "Baud Too Low";
                if (FormLoop.spGPSOut.IsOpen) FormLoop.spGPSOut.DiscardOutBuffer();
            }
        }

        public void StartBgGPSOutWorker()
        {
            if (!bgGPSOut.IsBusy)
            {
                bgGPSOut.RunWorkerAsync();  //this will call the DoWork
            }
            else
            {
                FormLoop.spGPSOut.DiscardOutBuffer();
                return;
            }
        }

        public void StartATimer()
        {
            algoTimer.Restart();
        }

        private double aTime;

        public void StopAtimer()
        {
            double newTime = ((double)(algoTimer.ElapsedTicks * 1000) / (double)System.Diagnostics.Stopwatch.Frequency);
            aTime = newTime * 0.1 + aTime * 0.9;
            lblSlowGPSOut.Text = aTime.ToString("N3");
        }
    }
}