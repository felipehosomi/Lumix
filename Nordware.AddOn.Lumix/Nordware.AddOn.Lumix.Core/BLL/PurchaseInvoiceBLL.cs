using Nordware.AddOn.Lumix.Core.DAO;
using Nordware.AddOn.Lumix.Core.Model;
using Nordware.AddOn.Lumix.Core.Utils;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.DAO;
using System;
//using System;
using System.Collections.Generic;
using System.Linq;

namespace Nordware.AddOn.Lumix.Core.BLL
{
    public class PurchaseInvoiceBLL
    {
        public static string PrintLabels(int docEntry, string printerName)
        {
            try
            {
                List<LabelModel> labelList = new CrudDAO().FillModelListFromSql<LabelModel>(String.Format(Hana.PurchaseInvoice_GetItems, docEntry));

                labelList = labelList.Where(l => !string.IsNullOrEmpty(l.DistNumber)).ToList();

                if (labelList.Count == 0)
                {
                    return "Nenhuma etiqueta encontrada para impressão";
                }
                else
                {
                    SBOApp.Application.SetStatusBarMessage($"{labelList.Count} etiquetas encontradas...", BoMessageTime.bmt_Medium, false);
                }

                QrCodeBLL qrCodeBLL = new QrCodeBLL();
                string error = qrCodeBLL.GenerateEPL(labelList, printerName);
                return error;
            }
            catch (Exception ex)
            {
                LogHelper.LogFileException(ex);
                return ex.Message;
            }
        }
    }
}
