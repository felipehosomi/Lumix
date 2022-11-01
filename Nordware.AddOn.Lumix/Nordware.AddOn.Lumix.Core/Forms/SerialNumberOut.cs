using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.Forms;
using System;
using Nordware.AddOn.Lumix.Core.Utils;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    [FormAttribute("25")]
    public class SerialNumberOut : SystemForm
    {
        public static bool AutoFill { get; set; } = false;

        public SerialNumberOut(ItemEvent itemEventInfo)
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

                    Form.Items.Item("480000061").Visible = false;
                    Form.Items.Item("480000060").Visible = false;
                    Form.Items.Item("10000058").Visible = false;
                    Form.Items.Item("10000059").Visible = false;
                    Form.Items.Item("21").Visible = false;
                    Form.Items.Item("22").Visible = false;

                    Form.Items.Item("8").Visible = false;
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN)
                {
                    if (ItemEventInfo.ItemUID == "et_QrCode" && (ItemEventInfo.CharPressed == 9 || ItemEventInfo.CharPressed == 13))
                    {
                        this.SelectSerialNum();
                        //return false;
                    }
                }
            }

            return true;
        }

        private void SelectSerialNum()
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
                        Form.Items.Item("8").Visible = true;
                        string itemCode = qrCode.Split('|')[0];
                        string serial = qrCode.Split('|')[1];

                        DBDataSource dt_Item = Form.DataSources.DBDataSources.Item("SBDR");
                        Matrix mt_Item = Form.Items.Item("3").Specific as Matrix;
                        Matrix mt_Serial = Form.Items.Item("5").Specific as Matrix;

                        bool itemFound = false;

                        for (int i = 0; i < dt_Item.Size; i++)
                        {
                            if (dt_Item.GetValue("ItemCode", i).Trim() == itemCode.Trim())
                            {
                                itemFound = true;
                                if (!mt_Item.IsRowSelected(i + 1))
                                {
                                    mt_Item.Columns.Item(0).Cells.Item(i + 1).Click();
                                }
                                break;
                            }
                        }
                        if (!itemFound)
                        {
                            SBOApp.Application.SetStatusBarMessage($"Item '{itemCode}' não encontrado!");
                            et_QrCode.Value = "";
                            return;
                        }

                        bool batchFound = false;

                        for (int i = 1; i <= mt_Serial.VisualRowCount; i++)
                        {
                            if (((EditText)mt_Serial.GetCellSpecific("19", i)).Value == serial)
                            {
                                batchFound = true;
                                if (!mt_Serial.IsRowSelected(i))
                                {
                                    mt_Serial.Columns.Item(0).Cells.Item(i).Click();
                                }

                                Form.Items.Item("8").Click();
                                et_QrCode.Value = String.Empty;
                                break;
                            }
                        }
                        if (!batchFound)
                        {
                            SBOApp.Application.SetStatusBarMessage($"Nº série '{serial}' não encontrado!");
                            et_QrCode.Value = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogFileException(ex);

                        SBOApp.Application.SetStatusBarMessage($"Erro ao processar Formulário: {ex.Message}");
                    }
                    finally
                    {
                        Form.Items.Item("8").Visible = false;
                        Form.Freeze(false);
                    }
                }

            }
        }
    }
}
