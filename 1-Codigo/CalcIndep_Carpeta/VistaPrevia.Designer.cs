namespace CalcIndep_Carpeta
{
    partial class VistaPrevia
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
            this.BT_Imprimir = new System.Windows.Forms.Button();
            this.DGV_Resultado = new System.Windows.Forms.DataGridView();
            this.BT_GuardarPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Resultado)).BeginInit();
            this.SuspendLayout();
            // 
            // BT_Imprimir
            // 
            this.BT_Imprimir.Location = new System.Drawing.Point(12, 257);
            this.BT_Imprimir.Name = "BT_Imprimir";
            this.BT_Imprimir.Size = new System.Drawing.Size(113, 30);
            this.BT_Imprimir.TabIndex = 1;
            this.BT_Imprimir.Text = "Imprimir";
            this.BT_Imprimir.UseVisualStyleBackColor = true;
            this.BT_Imprimir.Click += new System.EventHandler(this.BT_Imprimir_Click);
            // 
            // DGV_Resultado
            // 
            this.DGV_Resultado.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Resultado.Location = new System.Drawing.Point(12, 12);
            this.DGV_Resultado.Name = "DGV_Resultado";
            this.DGV_Resultado.Size = new System.Drawing.Size(484, 224);
            this.DGV_Resultado.TabIndex = 2;
            // 
            // BT_GuardarPDF
            // 
            this.BT_GuardarPDF.Location = new System.Drawing.Point(383, 257);
            this.BT_GuardarPDF.Name = "BT_GuardarPDF";
            this.BT_GuardarPDF.Size = new System.Drawing.Size(113, 30);
            this.BT_GuardarPDF.TabIndex = 3;
            this.BT_GuardarPDF.Text = "Guardar y cerrar";
            this.BT_GuardarPDF.UseVisualStyleBackColor = true;
            this.BT_GuardarPDF.Click += new System.EventHandler(this.BT_GuardarPDF_Click);
            // 
            // VistaPrevia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 299);
            this.Controls.Add(this.BT_GuardarPDF);
            this.Controls.Add(this.DGV_Resultado);
            this.Controls.Add(this.BT_Imprimir);
            this.Name = "VistaPrevia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resultado";
            this.Load += new System.EventHandler(this.VistaPrevia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Resultado)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BT_Imprimir;
        private System.Windows.Forms.DataGridView DGV_Resultado;
        private System.Windows.Forms.Button BT_GuardarPDF;
    }
}