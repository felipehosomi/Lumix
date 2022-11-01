using Nordware.AddOn.Lumix.Core.BLL;
using SBO.Hub;
using SBO.Hub.Services;
using System;
using System.IO;
using System.Windows.Forms;

namespace Nordware.AddOn.Lumix
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.Exit();
                return;
            }

            try
            {
                SBOApp sboApp = new SBOApp(args[0], $"{Application.StartupPath}\\Nordware.AddOn.Lumix.Core.dll");

                sboApp.InitializeApplication();
                SBOApp.AutoTranslateHana = false;

                InitializeBLL.Initialize();

                var oListener = new Listener();
                var oThread = new System.Threading.Thread(oListener.startListener) { IsBackground = true };
                oThread.Start();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run();
            }
            catch (Exception ex)
            {
                if (!Directory.Exists("c:\\temp\\"))
                {
                    Directory.CreateDirectory("c:\\temp\\");
                }

                StreamWriter sw = new StreamWriter("c:\\temp\\Addon Lumix Log.txt");
                sw.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    sw.WriteLine(ex.InnerException.Message);
                }
                sw.Close();
                throw ex;
            }
        }
    }
}
