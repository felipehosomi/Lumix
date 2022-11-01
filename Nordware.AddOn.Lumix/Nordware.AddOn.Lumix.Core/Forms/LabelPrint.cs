using Nordware.AddOn.Lumix.Core.BLL;
using Nordware.AddOn.Lumix.Core.Model;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Forms;
using SBO.Hub.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    public class LabelPrint : BaseForm
    {
        private static string Printer = "";

        public LabelPrint()
        {
            this.ItemEventInfo = ItemEventInfo;
        }


        public LabelPrint(ItemEvent itemEventInfo)
        {
            this.ItemEventInfo = itemEventInfo;
        }

        public LabelPrint(MenuEvent menuEventInfo)
        {
            this.MenuEventInfo = menuEventInfo;
        }

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (ItemEventInfo.ItemUID == "bt_Print")
                    {
                        this.Print();
                    }
                    if (ItemEventInfo.ItemUID == "bt_Printer")
                    {
                        Printer = PrintUtil.GetPrinter();
                    }
                }
                else if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    this.OnChooseFromList();
                }
            }

            return true;
        }

        private void Print()
        {
            if (String.IsNullOrEmpty(Printer))
            {
                Printer = PrintUtil.GetPrinter();
            }

            string itemCode = Form.DataSources.UserDataSources.Item("ud_Item").Value;
            string serialBatch = Form.DataSources.UserDataSources.Item("ud_DistNum").Value;
            string binCode = Form.DataSources.UserDataSources.Item("ud_IdLoc").Value;

            //if (String.IsNullOrEmpty(itemCode))
            //{
            //    SBOApp.Application.SetStatusBarMessage("Informe o item");
            //    return;
            //}

            int quantity;
            if (!Int32.TryParse(Form.DataSources.UserDataSources.Item("ud_Qty").Value, out quantity))
            {
                SBOApp.Application.SetStatusBarMessage("Informe a quantidade da(s) etiqueta(s) a serem impressas");
                return;
            }

            if (String.IsNullOrEmpty(itemCode) && !String.IsNullOrEmpty(binCode))
            {
                int result = SBOApp.Application.MessageBox("Nenhum item informado - Todas as etiquetas dos itens na localização informada serão impressas, deseja continuar?", 2, "Sim", "Não");
                if (result == 2)
                {
                    return;
                }
            }
            else if (String.IsNullOrEmpty(serialBatch))
            {
                int result = SBOApp.Application.MessageBox("Nenhum lote/série informado - Todas as etiquetas pendentes do item serão impressas, deseja continuar?", 2, "Sim", "Não");
                if (result == 2)
                {
                    return;
                }
            }
            else if (String.IsNullOrEmpty(binCode))
            {
                int result = SBOApp.Application.MessageBox("Nenhuma localização informada - Todas as etiquetas pendentes do item serão impressas, deseja continuar?", 2, "Sim", "Não");
                if (result == 2)
                {
                    return;
                }
            }

            string error;
            QrCodeBLL qrCodeBLL = new QrCodeBLL();
            List<LabelModel> list = LabelBLL.GetLabels(itemCode, serialBatch, binCode);
            if (list.Count == 0)
            {
                if (!String.IsNullOrEmpty(serialBatch))
                {
                    int result = SBOApp.Application.MessageBox("Item e lote/série informados não possui quantidade em estoque, deseja continuar?", 1, "Sim", "Não");
                    if (result == 1)
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            list.Add(new LabelModel() { ItemCode = itemCode, DistNumber = serialBatch });
                        }
                        
                        error = qrCodeBLL.GenerateEPL(list, Printer);
                        if (!String.IsNullOrEmpty(error))
                        {
                            SBOApp.Application.SetStatusBarMessage(error);
                        }
                    }
                }
            }

            if (list.Count == 0)
            {
                SBOApp.Application.SetStatusBarMessage("Nenhuma etiqueta encontrada para impressão para os dados informados");
            }

            for (int i = 1; i < quantity; i++)
            {
                list.AddRange(list);
            }

            error = qrCodeBLL.GenerateEPL(list, Printer);
            if (!String.IsNullOrEmpty(error))
            {
                SBOApp.Application.SetStatusBarMessage(error);
                return;
            }

            SBOApp.Application.SetStatusBarMessage("Impressão finalizada com sucesso!", BoMessageTime.bmt_Medium, false);
        }

        private void OnChooseFromList()
        {
            IChooseFromListEvent oCFLEvent = ((IChooseFromListEvent)ItemEventInfo);
            ChooseFromList oCFL = Form.ChooseFromLists.Item(oCFLEvent.ChooseFromListUID);
            DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable != null && oDataTable.Rows.Count > 0)
            {
                try
                {
                    if (ItemEventInfo.ItemUID == "et_Item")
                    {
                        Form.DataSources.UserDataSources.Item("ud_Item").Value = oDataTable.GetValue("ItemCode", 0).ToString();
                    }
                    else
                    {
                        Form.DataSources.UserDataSources.Item("ud_IdLoc").Value = oDataTable.GetValue("AbsEntry", 0).ToString();
                        Form.DataSources.UserDataSources.Item("ud_CodLoc").Value = oDataTable.GetValue("BinCode", 0).ToString();
                    }
                }
                catch { }
            }
        }
    }
}
