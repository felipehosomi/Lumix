using Nordware.AddOn.Lumix.Core.DAO;
using SBO.Hub;
using SBO.Hub.Helpers;
using SBO.Hub.SBOHelpers;
using System;
using System.Windows.Forms;

namespace Nordware.AddOn.Lumix.Core.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            EventFiltersBLL.SetDefaultEvents();
            UserFieldsBLL.CreateUserFields();

            try
            {
                MenuHelper.LoadFromXML($"{Application.StartupPath}\\Menu\\Menu.xml");
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage($"Erro ao criar menu: {ex.Message}");
            }

            //FormattedSearch formattedSearch = new FormattedSearch();
            //formattedSearch.AssignFormattedSearch("Lotes/Séries disponíveis", Hana.BatchSerial_GetAvailable, "LabelPrint", "et_DistNum");

            //UserFieldsBLL.CreateUserFields();
        }
    }
}
