using SAPbouiCOM;
using SBO.Hub.Attributes;
using SBO.Hub.Forms;

namespace Nordware.AddOn.Lumix.Core.Forms
{
    [FormAttribute("0")]
    class MessageBox : SystemForm
    {
        public MessageBox(ItemEvent itemEventInfo)
        {
            ItemEventInfo = itemEventInfo;
        }

        public override bool ItemEvent()
        {
            base.ItemEvent();
            if (!ItemEventInfo.BeforeAction)
            {
                try
                {
                    if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                    {
                        StaticText st_Message = Form.Items.Item("7").Specific as StaticText;
                        if (st_Message.Caption.ToLower().Contains("o lote") && st_Message.Caption.ToLower().Contains("já existe. continuar"))
                        {
                            Form.Items.Item("1").Click();
                        }
                        if (Form.Title.ToLower() == "nota fiscal de entrada" && st_Message.Caption.ToLower().Contains("impossível"))
                        {
                            Form.Items.Item("1").Click();
                        }
                    }
                }
                catch { }
            }

            return true;
        }
    }
}
