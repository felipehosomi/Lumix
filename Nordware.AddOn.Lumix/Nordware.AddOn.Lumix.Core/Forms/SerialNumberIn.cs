using Nordware.AddOn.Lumix.Core.DAO;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.DAO;
using SBO.Hub.Forms;
using System;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    [FormAttribute("21")]
    public class SerialNumberIn : SystemForm
    {
        public static bool AutoFill { get; set; } = true;

        public SerialNumberIn(ItemEvent itemEventInfo)
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
                    if (AutoFill)
                    {
                        DBDataSource dt_Item = Form.DataSources.DBDataSources.Item("SBDR");
                        Matrix mt_Item = Form.Items.Item("43").Specific as Matrix;
                        Matrix mt_Serial = Form.Items.Item("3").Specific as Matrix;

                        Int64 serial = Convert.ToInt64(CrudDAO.ExecuteScalar(Hana.Serial_GetMax).ToString()) + 1;

                        for (int i = 0; i < dt_Item.Size; i++)
                        {
                            double quantity = Convert.ToDouble(dt_Item.GetValue("OpenQty", i).Replace(".", ","));

                            if (quantity > 0)
                            {
                                if (!mt_Item.IsRowSelected(i + 1))
                                {
                                    mt_Item.Columns.Item(0).Cells.Item(i + 1).Click();
                                }
                                try
                                {
                                    int rowCount = mt_Serial.RowCount;

                                    while (quantity > 0)
                                    {
                                        SBOApp.Application.SetStatusBarMessage("AA" + serial.ToString().PadLeft(8, '0'));

                                        ((EditText)mt_Serial.GetCellSpecific("54", rowCount)).Value = "AA" + serial.ToString().PadLeft(8, '0');

                                        serial++;
                                        quantity--;
                                        rowCount++;
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        AutoFill = false;
                        if (Form.Mode == BoFormMode.fm_UPDATE_MODE)
                        {
                            Form.Items.Item("1").Click();
                        }
                        Form.Items.Item("1").Click();
                    }
                }
            }

            return true;
        }
    }
}
