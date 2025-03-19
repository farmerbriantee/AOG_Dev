using AOG.Properties;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace AOG
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly Mutex Mutex = new Mutex(true, "{516-0AC5-B9A1-55fd-A8CE-72F04E6BDD8F}");

        [STAThread]
        private static void Main()
        {
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                Log.EventWriter("Program Started: "
                    + DateTime.Now.ToString("f", CultureInfo.CreateSpecificCulture(RegistrySettings.culture)));
                Log.EventWriter("AOG Version: " + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));

                RegistrySettings.Load();

                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormGPS());
            }
            else
            {
                MessageBox.Show("AOG is Already Running");
            }
        }
    }
}

////check for corrupt settings file
//try
//{
//    Settings.Default.setF_culture = regKey.GetValue("Language").ToString();
//}
//catch (System.Configuration.ConfigurationErrorsException ex)
//{
//    // Corrupted XML! Delete the file, the user can just reload when this fails to appear. No need to worry them
//    MessageBoxButtons btns = MessageBoxButtons.OK;
//    System.Windows.Forms.MessageBox.Show("Error detected in config file - fixing it now", "Problem!", btns);
//    string filename = ((ex.InnerException as System.Configuration.ConfigurationErrorsException)?.Filename) as string;
//    System.IO.File.Delete(filename);
//}