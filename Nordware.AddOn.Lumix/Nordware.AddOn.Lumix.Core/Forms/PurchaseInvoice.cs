using Nordware.AddOn.Lumix.Core.BLL;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.Forms;
using SBO.Hub.Util;
using System;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    [FormAttribute("141")]
    public class PurchaseInvoice : SystemForm
    {
        private static int Retry = 0;
        private static string CurrentDocNum = "";
        private static string Printer = "";

        public PurchaseInvoice(ItemEvent itemEventInfo)
        {
            ItemEventInfo = itemEventInfo;
        }

        public PurchaseInvoice(BusinessObjectInfo businessObjectInfo)
        {
            BusinessObjectInfo = businessObjectInfo;
        }

        public override bool ItemEvent()
        {
            base.ItemEvent();

            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Printer = String.Empty;
                    Form.Items.Item("bt_Label").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);
                    Form.Items.Item("bt_Label").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_Ok, BoModeVisualBehavior.mvb_True);
                    Form.Items.Item("bt_Label").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, (int)BoAutoFormMode.afm_View, BoModeVisualBehavior.mvb_True);
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    if (ItemEventInfo.ItemUID == "1")
                    {
                        if (Form.Mode == BoFormMode.fm_ADD_MODE && Retry < 2 && ((EditText)Form.Items.Item("8").Specific).Value == CurrentDocNum)
                        {
                            Retry++;
                            Form.Items.Item("1").Click();
                        }
                        else
                        {
                            Retry = 0;
                            CurrentDocNum = "";
                        }
                    }
                    else if (ItemEventInfo.ItemUID == "bt_Label")
                    {
                        DBDataSource dataSource = Form.DataSources.DBDataSources.Item("OPCH");
                        string docEntry = dataSource.GetValue("DocEntry", 0);

                        if (String.IsNullOrEmpty(Printer))
                        {
                            Printer = PrintUtil.GetPrinter();
                        }
                        string error = PurchaseInvoiceBLL.PrintLabels(Convert.ToInt32(docEntry), Printer);
                        if (!String.IsNullOrEmpty(error))
                        {
                            SBOApp.Application.MessageBox(error);
                        }
                        else
                        {
                            SBOApp.Application.SetStatusBarMessage("Impressão finalizada com sucesso!", BoMessageTime.bmt_Medium, false);
                        }
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    if (ItemEventInfo.ItemUID == "1")
                    {
                        CurrentDocNum = ((EditText)Form.Items.Item("8").Specific).Value;

                        BatchIn.AutoFill = true;
                        SerialNumberIn.AutoFill = true;

                        #region Logica Preenchimento manual
                        //Form.Items.Item("112").Click();
                        //try
                        //{                        
                        //    Matrix mt_Item = Form.Items.Item("38").Specific as Matrix;
                        //    bool batchDone = false;
                        //    bool serialDone = false;

                        //    String matrixSchemaXML = (Form.Items.Item("38").Specific as Matrix).SerializeAsXML(BoMatrixXmlSelect.mxs_All);
                        //    int indexOfText = matrixSchemaXML.IndexOf("<UniqueID>11</UniqueID>");
                        //    string subs = matrixSchemaXML.Substring(0, indexOfText);
                        //    int quantityPosition = Regex.Matches(subs, "<ColumnInfo>").Count - 1;

                        //    for (int i = 0; i < mt_Item.Columns.Count; i++)
                        //    {
                        //        if (mt_Item.Columns.Item(i).UniqueID == "11")
                        //        {
                        //            quantityPosition = i;
                        //            break;
                        //        }
                        //    }

                        //    for (int i = 0; i < mt_Item.RowCount; i++)
                        //    {
                        //        try
                        //        {
                        //            string itemCode = ((EditText)mt_Item.GetCellSpecific("1", i + 1)).Value;
                        //            mt_Item.SetCellFocus(i + 1, quantityPosition);
                        //            ((EditText)mt_Item.GetCellSpecific("11", i + 1)).Item.Click();

                        //            if (!String.IsNullOrEmpty(itemCode))
                        //            {
                        //                string itemManagement = CrudDAO.ExecuteScalar(String.Format(SQL.Item_GetManagement, itemCode)).ToString();
                        //                if (itemManagement == "B" && !batchDone)
                        //                {
                        //                    Batch.AutoFill = true;
                        //                    SBOApp.Application.ActivateMenuItem("5896");
                        //                    batchDone = true;
                        //                }
                        //                else if (itemManagement == "S" && !serialDone)
                        //                {
                        //                    SerialNumber.AutoFill = true;
                        //                    SBOApp.Application.ActivateMenuItem("5896");
                        //                    serialDone = true;
                        //                }
                        //            }
                        //        }
                        //        catch (Exception ex) { }
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    SBOApp.Application.SetStatusBarMessage(ex.Message);
                        //}
                        //finally
                        //{

                        //}
                        #endregion
                    }
                }
            }
            return true;
        }
    }
}