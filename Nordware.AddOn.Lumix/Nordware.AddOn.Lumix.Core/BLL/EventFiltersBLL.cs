using SAPbouiCOM;
using SBO.Hub.Helpers;

namespace Nordware.AddOn.Lumix.Core.BLL
{
    public class EventFiltersBLL
    {
        public static void SetDefaultEvents()
        {
            EventFilterHelper.SetFormEvent("133", BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent("133", BoEventTypes.et_COMBO_SELECT);
            EventFilterHelper.SetFormEvent("133", BoEventTypes.et_ITEM_PRESSED);
            EventFilterHelper.SetFormEvent("133", BoEventTypes.et_CHOOSE_FROM_LIST);

            EventFilterHelper.SetFormEvent("141", BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent("141", BoEventTypes.et_ITEM_PRESSED);
            EventFilterHelper.SetFormEvent("141", BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent("141", BoEventTypes.et_FORM_DATA_ADD);

            EventFilterHelper.SetFormEvent("41", BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent("42", BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent("42", BoEventTypes.et_KEY_DOWN);

            EventFilterHelper.SetFormEvent("21", BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent("25", BoEventTypes.et_FORM_LOAD);
            EventFilterHelper.SetFormEvent("25", BoEventTypes.et_KEY_DOWN);


            EventFilterHelper.SetFormEvent("0", BoEventTypes.et_FORM_LOAD);

            EventFilterHelper.SetFormEvent("LabelPrint", BoEventTypes.et_CLICK);
            EventFilterHelper.SetFormEvent("LabelPrint", BoEventTypes.et_CHOOSE_FROM_LIST);

            EventFilterHelper.EnableEvents();
        }
    }
}
