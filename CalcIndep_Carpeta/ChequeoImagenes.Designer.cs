namespace CalcIndep_Carpeta
{
    partial class ChequeoImagenes
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
            this.PB_Axial = new System.Windows.Forms.PictureBox();
            this.PB_Sagital = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PB_Coronal = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.BT_No = new System.Windows.Forms.Button();
            this.BT_Si = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.PB_Nombre = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Axial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Sagital)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Coronal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nombre)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "¿Las imagenes obtenidas son correctas?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Axial";
            // 
            // PB_Axial
            // 
            this.PB_Axial.Location = new System.Drawing.Point(38, 122);
            this.PB_Axial.Name = "PB_Axial";
            this.PB_Axial.Size = new System.Drawing.Size(162, 122);
            this.PB_Axial.TabIndex = 2;
            this.PB_Axial.TabStop = false;
            // 
            // PB_Sagital
            // 
            this.PB_Sagital.Location = new System.Drawing.Point(259, 122);
            this.PB_Sagital.Name = "PB_Sagital";
            this.PB_Sagital.Size = new System.Drawing.Size(162, 122);
            this.PB_Sagital.TabIndex = 4;
            this.PB_Sagital.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(323, 260);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Sagital";
            // 
            // PB_Coronal
            // 
            this.PB_Coronal.Location = new System.Drawing.Point(478, 122);
            this.PB_Coronal.Name = "PB_Coronal";
            this.PB_Coronal.Size = new System.Drawing.Size(162, 122);
            this.PB_Coronal.TabIndex = 6;
            this.PB_Coronal.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(536, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Coronal";
            // 
            // BT_No
            // 
            this.BT_No.Location = new System.Drawing.Point(452, 324);
            this.BT_No.Name = "BT_No";
            this.BT_No.Size = new System.Drawing.Size(75, 23);
            this.BT_No.TabIndex = 7;
            this.BT_No.Text = "No";
            this.BT_No.UseVisualStyleBackColor = true;
            this.BT_No.Click += new System.EventHandler(this.BT_No_Click);
            // 
            // BT_Si
            // 
            this.BT_Si.Location = new System.Drawing.Point(565, 324);
            this.BT_Si.Name = "BT_Si";
            this.BT_Si.Size = new System.Drawing.Size(75, 23);
            this.BT_Si.TabIndex = 8;
            this.BT_Si.Text = "Si";
            this.BT_Si.UseVisualStyleBackColor = true;
            this.BT_Si.Click += new System.EventHandler(this.BT_Si_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(35, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "¿El paciente es el correcto?";
            // 
            // PB_Nombre
            // 
            this.PB_Nombre.Location = new System.Drawing.Point(38, 37);
            this.PB_Nombre.Name = "PB_Nombre";
            this.PB_Nombre.Size = new System.Drawing.Size(450, 20);
            this.PB_Nombre.TabIndex = 10;
            this.PB_Nombre.TabStop = false;
            // 
            // ChequeoImagenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 355);
            this.Controls.Add(this.PB_Nombre);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BT_Si);
            this.Controls.Add(this.BT_No);
            this.Controls.Add(this.PB_Coronal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PB_Sagital);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PB_Axial);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ChequeoImagenes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chequeo de imagenes";
            ((System.ComponentModel.ISupportInitialize)(this.PB_Axial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Sagital)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Coronal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Nombre)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox PB_Axial;
        private System.Windows.Forms.PictureBox PB_Sagital;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox PB_Coronal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BT_No;
        private System.Windows.Forms.Button BT_Si;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox PB_Nombre;
    }
}