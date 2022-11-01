using Nordware.AddOn.Lumix.Core.DAO;
using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.DAO;
using SBO.Hub.Forms;
using System;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    [FormAttribute("41")]
    public class BatchIn : SystemForm
    {
        public static bool AutoFill { get; set; } = false;

        public BatchIn(ItemEvent itemEventInfo)
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
                    AutoFillBatches();
                }
            }
            return true;
        }

        private void AutoFillBatches()
        {
            if (AutoFill)
            {
                DBDataSource dt_Item = Form.DataSources.DBDataSources.Item("SBDR");
                Matrix mt_Item = Form.Items.Item("35").Specific as Matrix;
                Matrix mt_Batch = Form.Items.Item("3").Specific as Matrix;

                

                for (int i = 0; i < dt_Item.Size; i++)
                {
                    double quantity = Convert.ToDouble(dt_Item.GetValue("DocQuan", i).Replace(".", ","));
                    double quantityCreated = Convert.ToDouble(dt_Item.GetValue("TotalCreat", i).Replace(".", ","));

                    if (quantity != quantityCreated)
                    {
                        if (!mt_Item.IsRowSelected(i + 1))
                        {
                            mt_Item.Columns.Item(0).Cells.Item(i + 1).Click();
                        }
                        try
                        {
                            ((EditText)mt_Batch.GetCellSpecific("2", 1)).Value = CrudDAO.ExecuteScalar(String.Format(Hana.Item_GetCodeBar, dt_Item.GetValue("ItemCode", i))).ToString();
                            ((EditText)mt_Batch.GetCellSpecific("5", 1)).Value = quantity.ToString().Replace(",", ".");

                            //((EditText)mt_Batch.GetCellSpecific("234000024", 1)).Value = quantity.ToString().Replace(",", ".");
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
}
