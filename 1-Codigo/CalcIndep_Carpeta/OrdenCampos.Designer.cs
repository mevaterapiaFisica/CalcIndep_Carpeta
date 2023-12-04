namespace CalcIndep_Carpeta
{
    partial class OrdenCampos
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
            this.LB_Campos = new System.Windows.Forms.ListBox();
            this.BT_Subir = new System.Windows.Forms.Button();
            this.BT_Bajar = new System.Windows.Forms.Button();
            this.BT_Horario = new System.Windows.Forms.Button();
            this.BT_Antihorario = new System.Windows.Forms.Button();
            this.BT_Aceptar = new System.Windows.Forms.Button();
            this.BT_Cancelar = new System.Windows.Forms.Button();
            this.Label_Plan = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ordenar los campos según aparecerán el PPF";
            // 
            // LB_Campos
            // 
            this.LB_Campos.FormattingEnabled = true;
            this.LB_Campos.Location = new System.Drawing.Point(15, 61);
            this.LB_Campos.Name = "LB_Campos";
            this.LB_Campos.Size = new System.Drawing.Size(114, 173);
            this.LB_Campos.TabIndex = 1;
            this.LB_Campos.SelectedIndexChanged += new System.EventHandler(this.LB_Campos_SelectedIndexChanged);
            // 
            // BT_Subir
            // 
            this.BT_Subir.Location = new System.Drawing.Point(144, 61);
            this.BT_Subir.Name = "BT_Subir";
            this.BT_Subir.Size = new System.Drawing.Size(75, 23);
            this.BT_Subir.TabIndex = 2;
            this.BT_Subir.Text = "Subir";
            this.BT_Subir.UseVisualStyleBackColor = true;
            this.BT_Subir.Click += new System.EventHandler(this.BT_Subir_Click);
            // 
            // BT_Bajar
            // 
            this.BT_Bajar.Location = new System.Drawing.Point(144, 90);
            this.BT_Bajar.Name = "BT_Bajar";
            this.BT_Bajar.Size = new System.Drawing.Size(75, 23);
            this.BT_Bajar.TabIndex = 3;
            this.BT_Bajar.Text = "Bajar";
            this.BT_Bajar.UseVisualStyleBackColor = true;
            this.BT_Bajar.Click += new System.EventHandler(this.BT_Bajar_Click);
            // 
            // BT_Horario
            // 
            this.BT_Horario.Location = new System.Drawing.Point(144, 150);
            this.BT_Horario.Name = "BT_Horario";
            this.BT_Horario.Size = new System.Drawing.Size(75, 38);
            this.BT_Horario.TabIndex = 4;
            this.BT_Horario.Text = "Horario (359->1)";
            this.BT_Horario.UseVisualStyleBackColor = true;
            this.BT_Horario.Click += new System.EventHandler(this.BT_Horario_Click);
            // 
            // BT_Antihorario
            // 
            this.BT_Antihorario.Location = new System.Drawing.Point(144, 194);
            this.BT_Antihorario.Name = "BT_Antihorario";
            this.BT_Antihorario.Size = new System.Drawing.Size(75, 40);
            this.BT_Antihorario.TabIndex = 5;
            this.BT_Antihorario.Text = "Antihorario (1->359)";
            this.BT_Antihorario.UseVisualStyleBackColor = true;
            this.BT_Antihorario.Click += new System.EventHandler(this.BT_Antihorario_Click);
            // 
            // BT_Aceptar
            // 
            this.BT_Aceptar.Location = new System.Drawing.Point(40, 264);
            this.BT_Aceptar.Name = "BT_Aceptar";
            this.BT_Aceptar.Size = new System.Drawing.Size(75, 23);
            this.BT_Aceptar.TabIndex = 6;
            this.BT_Aceptar.Text = "Aceptar";
            this.BT_Aceptar.UseVisualStyleBackColor = true;
            this.BT_Aceptar.Click += new System.EventHandler(this.BT_Aceptar_Click);
            // 
            // BT_Cancelar
            // 
            this.BT_Cancelar.Location = new System.Drawing.Point(144, 264);
            this.BT_Cancelar.Name = "BT_Cancelar";
            this.BT_Cancelar.Size = new System.Drawing.Size(75, 23);
            this.BT_Cancelar.TabIndex = 7;
            this.BT_Cancelar.Text = "Cancelar";
            this.BT_Cancelar.UseVisualStyleBackColor = true;
            this.BT_Cancelar.Click += new System.EventHandler(this.BT_Cancelar_Click);
            // 
            // Label_Plan
            // 
            this.Label_Plan.AutoSize = true;
            this.Label_Plan.Location = new System.Drawing.Point(15, 35);
            this.Label_Plan.Name = "Label_Plan";
            this.Label_Plan.Size = new System.Drawing.Size(60, 13);
            this.Label_Plan.TabIndex = 8;
            this.Label_Plan.Text = "Label_Plan";
            // 
            // OrdenCampos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 311);
            this.Controls.Add(this.Label_Plan);
            this.Controls.Add(this.BT_Cancelar);
            this.Controls.Add(this.BT_Aceptar);
            this.Controls.Add(this.BT_Antihorario);
            this.Controls.Add(this.BT_Horario);
            this.Controls.Add(this.BT_Bajar);
            this.Controls.Add(this.BT_Subir);
            this.Controls.Add(this.LB_Campos);
            this.Controls.Add(this.label1);
            this.Name = "OrdenCampos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrdenCampos";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox LB_Campos;
        private System.Windows.Forms.Button BT_Subir;
        private System.Windows.Forms.Button BT_Bajar;
        private System.Windows.Forms.Button BT_Horario;
        private System.Windows.Forms.Button BT_Antihorario;
        private System.Windows.Forms.Button BT_Aceptar;
        private System.Windows.Forms.Button BT_Cancelar;
        private System.Windows.Forms.Label Label_Plan;
    }
}