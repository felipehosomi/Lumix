using Nordware.AddOn.Lumix.Core.BLL;
using Nordware.AddOn.Lumix.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nordware.AddOn.LabelTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            QrCodeBLL bll = new QrCodeBLL();

            List<LabelModel> list = new List<LabelModel>();
            list.Add(new LabelModel() { ItemCode = "CROMACONTR103", DistNumber = "0001462493" });
            list.Add(new LabelModel() { ItemCode = "Item2Item2Item2", DistNumber = "LoteTeste2", UnitCompra = "un", UnitVenda="pc" });
            list.Add(new LabelModel() { ItemCode = "Item3Item3Item3", DistNumber = "LoteTeste3", UnitCompra = "un", UnitVenda = "pc" });
            list.Add(new LabelModel() { ItemCode = "Item4Item4Item4", DistNumber = "LoteTeste4", UnitCompra = "un", UnitVenda = "pc" });
            list.Add(new LabelModel() { ItemCode = "Item5Item5Item5", DistNumber = "LoteTeste5", UnitCompra = "un", UnitVenda = "pc" });
            list.Add(new LabelModel() { ItemCode = "Item6Item6Item6", DistNumber = "LoteTeste6", UnitCompra = "un", UnitVenda = "pc" });
            list.Add(new LabelModel() { ItemCode = "Item7Item7Item7", DistNumber = "LoteTeste7", UnitCompra = "un", UnitVenda = "pc" });
            list.Add(new LabelModel() { ItemCode = "Item8Item8Item8", DistNumber = "LoteTeste8", UnitCompra = "un", UnitVenda = "pc" });
            list.Add(new LabelModel() { ItemCode = "Item9Item9Item9", DistNumber = "LoteTeste9", UnitCompra = "un", UnitVenda = "pc" });
            list.Add(new LabelModel() { ItemCode = "Item10Item10Ite", DistNumber = "LoteTeste1", UnitCompra = "un", UnitVenda = "pc" });


            string error = bll.GenerateEPL(list, tbxPrinter.Text);

            //string error = bll.GenerateZPL(list.Select(i => i.ItemCode).ToList(), list.Select(i => i.DistNumber).ToList(), tbxPrinter.Text);
            if (!String.IsNullOrEmpty(error))
            {
                MessageBox.Show(error);
            }
            else
            {
                MessageBox.Show("Etiqueta enviada para a impressora!");
            }
        }

        private void btnPrinter_Click(object sender, EventArgs e)
        {
            printDialog1.ShowDialog();
            tbxPrinter.Text = printDialog1.PrinterSettings.PrinterName;
        }
    }
}
