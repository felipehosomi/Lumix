using SAPbouiCOM;
using SBO.Hub;
using SBO.Hub.Attributes;
using SBO.Hub.Forms;
using System;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    [FormAttribute("133")]
    public class Invoice : SystemForm
    {
        public Invoice(ItemEvent itemEventInfo)
        {
            ItemEventInfo = itemEventInfo;
        }

        public Invoice(BusinessObjectInfo businessObjectInfo)
        {
            BusinessObjectInfo = businessObjectInfo;
        }


        public override bool ItemEvent()
        {
            base.ItemEvent();

            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_COMBO_SELECT)
                {
                    if (ItemEventInfo.ItemUID == "38" && ItemEventInfo.ColUID == "U_Checked")
                    {
                        this.FillCheckedField();
                    }
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    if (ItemEventInfo.ItemUID == "38")
                    {
                        this.FillCheckedField();
                    }
                }
            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_ITEM_PRESSED)
                {
                    if (ItemEventInfo.ItemUID == "1")
                    {
                        if (Form.Mode == BoFormMode.fm_ADD_MODE && Form.DataSources.UserDataSources.Item("ud_Checked").Value != "Y")
                        {
                            SBOApp.Application.SetStatusBarMessage("Todos os itens devem estar conferidos");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public override bool FormDataEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    this.FillCheckedField();
                }
            }
            return true;
        }

        private void FillCheckedField()
        {
            Form.Freeze(true);
            bool allChecked = true;
            Matrix mt_Item = Form.Items.Item("38").Specific as Matrix;

            for (int i = 1; i <= mt_Item.RowCount; i++)
            {
                if (!String.IsNullOrEmpty(((EditText)mt_Item.GetCellSpecific("1", i)).Value) && ((ComboBox)mt_Item.GetCellSpecific("U_Checked", i)).Value == "N")
                {
                    allChecked = false;
                    break;
                }
            }

            Form.DataSources.UserDataSources.Item("ud_Checked").Value = allChecked ? "Y" : "N";
            Form.Freeze(false);
        }
    }
}