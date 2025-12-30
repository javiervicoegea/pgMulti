using PgMulti.AppData;
using PgMulti.Forms;
using System.Globalization;

namespace PgMulti
{
    internal static class Program
    {
        private const string MutexGuid = "{B6FA7AF9-6317-4BD9-A8DE-8C8BC9E42A57}";
        static Mutex mutex = new Mutex(true, MutexGuid);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo cu = AppLanguage.CurrentLanguage.CultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cu;
            CultureInfo.DefaultThreadCurrentUICulture = cu;

            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
            else
            {
                Win32Messages.PostMessage((IntPtr)Win32Messages.HWND_BROADCAST, Win32Messages.WM_SHOWME, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}