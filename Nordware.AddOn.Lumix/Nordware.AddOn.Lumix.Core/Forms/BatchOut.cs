using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.Forms;
using System;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    [FormAttribute("42")]
    public class BatchOut : SystemForm
    {
        public static bool AutoFill { get; set; } = false;

        public BatchOut(ItemEvent itemEventInfo)
        {
            ItemEventInfo = itemEventInfo;
        }

        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    Form.Items.Item("et_QrCode").Click();
                    Form.Items.Item("48").Visible = false;
                    Form.Items.Item("14").Visible = false;
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.ItemUID == "et_QrCode" && (ItemEventInfo.CharPressed == 9 || ItemEventInfo.CharPressed == 13))
                    {
                        this.SelectBatch();
                        //return false;
                    }
                }
            }

            return true;
        }

        private void SelectBatch()
        {
            EditText et_QrCode = (EditText)Form.Items.Item("et_QrCode").Specific;
            string qrCode = et_QrCode.Value;

            if (!String.IsNullOrEmpty(qrCode))
            {
                if (qrCode.Contains("|"))
                {
                    try
                    {
                        Form.Freeze(true);
                        Form.Items.Item("48").Visible = true;
                        string itemCode = qrCode.Split('|')[0];
                        string batch = qrCode.Split('|')[1];

                        DBDataSource dt_Item = Form.DataSources.DBDataSources.Item("SBDR");
                        Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
                        Matrix mt_Batch = Form.Items.Item("4").Specific as Matrix;
                        double quantity = 0;
                        double quantityNecessario = 0;
                        double quantitySelected = 0;

                        bool itemFound = false;

                        for (int i = 0; i < dt_Item.Size; i++)
                        {
                            quantity = GetDouble(dt_Item.GetValue("DocQuan", i));
                            quantityNecessario = GetDouble(dt_Item.GetValue("OpenQty", i));
                            quantitySelected = GetDouble(dt_Item.GetValue("TotalCreat", i));

                            if (quantity != quantitySelected)
                            {
                                if (dt_Item.GetValue("ItemCode", i).Trim() == itemCode.Trim())
                                {
                                    itemFound = true;
                                    if (!mt_Item.IsRowSelected(i + 1))
                                    {
                                        mt_Item.Columns.Item(0).Cells.Item(i + 1).Click();
                                    }

                                    SelectBatch(mt_Batch, batch, et_QrCode, quantityNecessario);
                                }
                            }
                        }
                        if (!itemFound)
                        {
                            SBOApp.Application.SetStatusBarMessage($"Item '{itemCode}' não encontrado!");
                            return;
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        SBOApp.Application.SetStatusBarMessage(ex.Message);
                    }
                    finally
                    {
                        Form.Items.Item("48").Visible = false;
                        Form.Freeze(false);
                    }
                }
            }
        }

        private void SelectBatch(Matrix mt_Batch, string batch, EditText et_QrCode, double quantity)
        {
            bool batchFound = false;

            for (int i = 1; i <= mt_Batch.VisualRowCount; i++)
            {
                if (((EditText)mt_Batch.GetCellSpecific("0", i)).Value == batch)
                {
                    batchFound = true;
                    if (!mt_Batch.IsRowSelected(i))
                    {
                        mt_Batch.Columns.Item(0).Cells.Item(i).Click();
                    }

                    double quantityAvailable = GetDouble(((EditText)mt_Batch.GetCellSpecific("3", i)).Value);
                    if (quantityAvailable < quantity)
                    {
                        SBOApp.Application.SetStatusBarMessage($"Quantidade disponível do lote insuficiente: [{quantity}|{quantityAvailable}]");
                        quantity = quantityAvailable;
                    }

                    ((EditText)mt_Batch.GetCellSpecific("234000059", i)).Value = quantity.ToString().Replace(",", ".");

                    Form.Items.Item("48").Click();
                    et_QrCode.Value = String.Empty;
                    break;
                }
            }
            if (!batchFound)
            {
                SBOApp.Application.SetStatusBarMessage($"Lote '{batch}' não encontrado!");
            }
        }

        private double GetDouble(string value)
        {
            if (value.Contains(".") && !value.Contains(","))
                return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);

            if (value.Contains(",") && !value.Contains("."))
                return double.Parse(value.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);

            else
                return double.Parse(value.Replace(".", "").Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
