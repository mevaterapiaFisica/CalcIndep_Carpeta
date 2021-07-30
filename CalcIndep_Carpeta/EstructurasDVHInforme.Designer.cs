namespace CalcIndep_Carpeta
{
    partial class EstructurasDVHInforme
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
            this.BT_Continuar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chlb_Estructuras = new System.Windows.Forms.CheckedListBox();
            this.BT_SeleccionarTodas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BT_Continuar
            // 
            this.BT_Continuar.Location = new System.Drawing.Point(165, 328);
            this.BT_Continuar.Name = "BT_Continuar";
            this.BT_Continuar.Size = new System.Drawing.Size(94, 25);
            this.BT_Continuar.TabIndex = 1;
            this.BT_Continuar.Text = "Continuar";
            this.BT_Continuar.UseVisualStyleBackColor = true;
            this.BT_Continuar.Click += new System.EventHandler(this.BT_Continuar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "Seleccionar las estructuras que aparecerán\r\nen el DVH";
            // 
            // chlb_Estructuras
            // 
            this.chlb_Estructuras.CheckOnClick = true;
            this.chlb_Estructuras.FormattingEnabled = true;
            this.chlb_Estructuras.Location = new System.Drawing.Point(15, 49);
            this.chlb_Estructuras.Name = "chlb_Estructuras";
            this.chlb_Estructuras.Size = new System.Drawing.Size(244, 259);
            this.chlb_Estructuras.TabIndex = 3;
            // 
            // BT_SeleccionarTodas
            // 
            this.BT_SeleccionarTodas.Location = new System.Drawing.Point(15, 328);
            this.BT_SeleccionarTodas.Name = "BT_SeleccionarTodas";
            this.BT_SeleccionarTodas.Size = new System.Drawing.Size(111, 25);
            this.BT_SeleccionarTodas.TabIndex = 4;
            this.BT_SeleccionarTodas.Text = "Seleccionar todas";
            this.BT_SeleccionarTodas.UseVisualStyleBackColor = true;
            this.BT_SeleccionarTodas.Click += new System.EventHandler(this.BT_SeleccionarTodas_Click);
            // 
            // EstructurasDVHInforme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 363);
            this.Controls.Add(this.BT_SeleccionarTodas);
            this.Controls.Add(this.chlb_Estructuras);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BT_Continuar);
            this.Name = "EstructurasDVHInforme";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estructuras";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BT_Continuar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox chlb_Estructuras;
        private System.Windows.Forms.Button BT_SeleccionarTodas;
    }
}