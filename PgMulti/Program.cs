using PgMulti.AppData;
using System.Globalization;

namespace PgMulti
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo cu = AppLanguage.CurrentLanguage.CultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cu;
            CultureInfo.DefaultThreadCurrentUICulture = cu;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}