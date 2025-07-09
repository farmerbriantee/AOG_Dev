using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOG
{
    public partial class FormSaving : Form
    {
        public FormSaving()
        {
            InitializeComponent();
        }

        public void InitializeSteps(bool isJobStarted)
        {
            lstSteps.Items.Clear();

            if (isJobStarted)
            {
                lstSteps.Items.Add(ShutdownSteps.SaveParams);
                lstSteps.Items.Add(ShutdownSteps.SaveField);
                lstSteps.Items.Add(ShutdownSteps.SaveSettings);
                lstSteps.Items.Add(ShutdownSteps.Finalizing);
            }
            else
            {
                lstSteps.Items.Add(ShutdownSteps.SaveSettings);
                lstSteps.Items.Add(ShutdownSteps.Finalizing);
            }
        }

        public void UpdateStep(int index, string text)
        {
            if (index >= 0 && index < lstSteps.Items.Count)
                lstSteps.Items[index] = text;
        }

        public void InsertStep(int index, string text)
        {
            if (index >= 0 && index <= lstSteps.Items.Count)
                lstSteps.Items.Insert(index, text);
        }

        public void AddFinalMessage()
        {
            lstSteps.Items.Add("");
            lstSteps.Items.Add(ShutdownSteps.Beer);
        }
    }
}
public static class ShutdownSteps
{
    public const string SaveParams = "• Saving field parameters...";
    public const string SaveField = "• Saving field...";
    public const string SaveSettings = "• Saving settings...";
    public const string Finalizing = "• Finalizing shutdown...";

    public const string UploadAgShare = "• Uploading field to AgShare...";
    public const string UploadDone = "✓ Upload complete.";
    public const string UploadFailed = "✗ Upload failed.";

    public const string ParamsDone = "✓ Field parameters saved.";
    public const string FieldSaved = "✓ Field saved locally.";
    public const string SettingsSaved = "✓ Settings saved.";
    public const string AllDone = "✔ All done. Closing now...";
    public const string Beer = "🍺 Time for a Beer! Goodbye!";
}
