namespace CalcIndep_Carpeta
{
    partial class DatosPlanSuma
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
            this.label3 = new System.Windows.Forms.Label();
            this.TB_DosisTotal = new System.Windows.Forms.TextBox();
            this.TB_NumeroFracciones = new System.Windows.Forms.TextBox();
            this.BT_Aceptar = new System.Windows.Forms.Button();
            this.BT_Cancelar = new System.Windows.Forms.Button();
            this.CB_TipoTratamiento = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tipo de tratamiento";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Dosis Total [Gy]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nómero de fracciones";
            // 
            // TB_DosisTotal
            // 
            this.TB_DosisTotal.Location = new System.Drawing.Point(139, 47);
            this.TB_DosisTotal.Name = "TB_DosisTotal";
            this.TB_DosisTotal.Size = new System.Drawing.Size(122, 20);
            this.TB_DosisTotal.TabIndex = 4;
            // 
            // TB_NumeroFracciones
            // 
            this.TB_NumeroFracciones.Location = new System.Drawing.Point(139, 81);
            this.TB_NumeroFracciones.Name = "TB_NumeroFracciones";
            this.TB_NumeroFracciones.Size = new System.Drawing.Size(122, 20);
            this.TB_NumeroFracciones.TabIndex = 5;
            // 
            // BT_Aceptar
            // 
            this.BT_Aceptar.Location = new System.Drawing.Point(186, 144);
            this.BT_Aceptar.Name = "BT_Aceptar";
            this.BT_Aceptar.Size = new System.Drawing.Size(75, 23);
            this.BT_Aceptar.TabIndex = 6;
            this.BT_Aceptar.Text = "Aceptar";
            this.BT_Aceptar.UseVisualStyleBackColor = true;
            this.BT_Aceptar.Click += new System.EventHandler(this.BT_Aceptar_Click);
            // 
            // BT_Cancelar
            // 
            this.BT_Cancelar.Location = new System.Drawing.Point(15, 144);
            this.BT_Cancelar.Name = "BT_Cancelar";
            this.BT_Cancelar.Size = new System.Drawing.Size(75, 23);
            this.BT_Cancelar.TabIndex = 7;
            this.BT_Cancelar.Text = "Cancelar";
            this.BT_Cancelar.UseVisualStyleBackColor = true;
            this.BT_Cancelar.Click += new System.EventHandler(this.BT_Cancelar_Click);
            // 
            // CB_TipoTratamiento
            // 
            this.CB_TipoTratamiento.FormattingEnabled = true;
            this.CB_TipoTratamiento.Items.AddRange(new object[] {
            "IMRT",
            "VMAT",
            "3D",
            "SRS",
            "SBRT",
            "IGRT"});
            this.CB_TipoTratamiento.Location = new System.Drawing.Point(139, 15);
            this.CB_TipoTratamiento.Name = "CB_TipoTratamiento";
            this.CB_TipoTratamiento.Size = new System.Drawing.Size(122, 21);
            this.CB_TipoTratamiento.TabIndex = 8;
            // 
            // DatosPlanSuma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 193);
            this.Controls.Add(this.CB_TipoTratamiento);
            this.Controls.Add(this.BT_Cancelar);
            this.Controls.Add(this.BT_Aceptar);
            this.Controls.Add(this.TB_NumeroFracciones);
            this.Controls.Add(this.TB_DosisTotal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DatosPlanSuma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datos para informe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_DosisTotal;
        private System.Windows.Forms.TextBox TB_NumeroFracciones;
        private System.Windows.Forms.Button BT_Aceptar;
        private System.Windows.Forms.Button BT_Cancelar;
        private System.Windows.Forms.ComboBox CB_TipoTratamiento;
    }
}