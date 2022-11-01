using Nordware.AddOn.Lumix.Core.Model;
using SBO.Hub.DAO;
using System;
using System.Collections.Generic;

namespace Nordware.AddOn.Lumix.Core.BLL
{
    public class LabelBLL
    {
        public static List<LabelModel> GetLabels(string itemCode, string serialBatch, string binCode)
        {
            if (String.IsNullOrEmpty(itemCode))
            {
                itemCode = "NULL";
            }
            else
            {
                itemCode = $"'{itemCode}'";
            }

            if (String.IsNullOrEmpty(serialBatch))
            {
                serialBatch = "NULL";
            }
            else
            {
                serialBatch = $"'{serialBatch}'";
            }
            if (String.IsNullOrEmpty(binCode))
            {
                binCode = "NULL";
            }

            string sql = $"CALL SP_SERIALBATCH_GET ({itemCode}, {serialBatch}, {binCode})";
            List<LabelModel> list = new CrudDAO().FillModelListFromSql<LabelModel>(sql);
            return list;
        }
    }
}
