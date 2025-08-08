using System.Windows.Forms;

namespace AOG
{
    public partial class FormSaving : Form
    {
        private Label lastlbl;
        int idx = 0;
        public FormSaving()
        {
            InitializeComponent();
        }

        public void UpdateStep(string text)
        {
            if (lastlbl != null)
                lastlbl.Text = text;
        }

        public void AddStep(string text)
        {
            if (idx == 0)
                lastlbl = label1;
            else if (idx == 1)
                lastlbl = label2;
            else if (idx == 2)
                lastlbl = label3;
            else if (idx == 3)
                lastlbl = label4;
            else if (idx == 4)
                lastlbl = label5;
            else if (idx == 5)
                lastlbl = label6;

            idx++;
            lastlbl.Text = text;
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
