namespace Nordware.AddOn.LabelTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxItem = new System.Windows.Forms.TextBox();
            this.tbxLote = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.tbxPrinter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.btnPrinter = new System.Windows.Forms.Button();
            this.tbxLabel = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Lote";
            // 
            // tbxItem
            // 
            this.tbxItem.Location = new System.Drawing.Point(126, 18);
            this.tbxItem.Name = "tbxItem";
            this.tbxItem.Size = new System.Drawing.Size(100, 20);
            this.tbxItem.TabIndex = 2;
            this.tbxItem.Text = "TESTE123TESTE123";
            // 
            // tbxLote
            // 
            this.tbxLote.Location = new System.Drawing.Point(126, 44);
            this.tbxLote.Name = "tbxLote";
            this.tbxLote.Size = new System.Drawing.Size(100, 20);
            this.tbxLote.TabIndex = 3;
            this.tbxLote.Text = "1234567890";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(40, 257);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Imprimir";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // tbxPrinter
            // 
            this.tbxPrinter.Location = new System.Drawing.Point(126, 70);
            this.tbxPrinter.Name = "tbxPrinter";
            this.tbxPrinter.Size = new System.Drawing.Size(100, 20);
            this.tbxPrinter.TabIndex = 6;
            this.tbxPrinter.Text = "ZDesigner TLP 2844 (Copiar 1)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Impressora";
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // btnPrinter
            // 
            this.btnPrinter.Location = new System.Drawing.Point(232, 67);
            this.btnPrinter.Name = "btnPrinter";
            this.btnPrinter.Size = new System.Drawing.Size(27, 23);
            this.btnPrinter.TabIndex = 7;
            this.btnPrinter.Text = "...";
            this.btnPrinter.UseVisualStyleBackColor = true;
            this.btnPrinter.Click += new System.EventHandler(this.btnPrinter_Click);
            // 
            // tbxLabel
            // 
            this.tbxLabel.Location = new System.Drawing.Point(40, 112);
            this.tbxLabel.Multiline = true;
            this.tbxLabel.Name = "tbxLabel";
            this.tbxLabel.Size = new System.Drawing.Size(354, 139);
            this.tbxLabel.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Etiqueta";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 292);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbxLabel);
            this.Controls.Add(this.btnPrinter);
            this.Controls.Add(this.tbxPrinter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.tbxLote);
            this.Controls.Add(this.tbxItem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Lumix - Teste Impressão de Etiqueta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxItem;
        private System.Windows.Forms.TextBox tbxLote;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox tbxPrinter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.Button btnPrinter;
        private System.Windows.Forms.TextBox tbxLabel;
        private System.Windows.Forms.Label label4;
    }
}

