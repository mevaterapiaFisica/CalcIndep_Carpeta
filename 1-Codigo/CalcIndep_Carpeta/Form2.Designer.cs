namespace CalcIndep_Carpeta
{
    partial class Form_CI
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
            this.listBox_RefPoints = new System.Windows.Forms.ListBox();
            this.button_Calc = new System.Windows.Forms.Button();
            this.button_Next = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_SesgoD = new System.Windows.Forms.Label();
            this.label_SesgoPerc = new System.Windows.Forms.Label();
            this.label_UMindep = new System.Windows.Forms.Label();
            this.label_UMeclipse = new System.Windows.Forms.Label();
            this.label_Energia = new System.Windows.Forms.Label();
            this.label_Equipo = new System.Windows.Forms.Label();
            this.label_IDcampo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.textBox_Bolus = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxAng = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox_RefPoints
            // 
            this.listBox_RefPoints.FormattingEnabled = true;
            this.listBox_RefPoints.Location = new System.Drawing.Point(12, 12);
            this.listBox_RefPoints.Name = "listBox_RefPoints";
            this.listBox_RefPoints.Size = new System.Drawing.Size(120, 134);
            this.listBox_RefPoints.TabIndex = 0;
            // 
            // button_Calc
            // 
            this.button_Calc.Location = new System.Drawing.Point(12, 284);
            this.button_Calc.Name = "button_Calc";
            this.button_Calc.Size = new System.Drawing.Size(120, 23);
            this.button_Calc.TabIndex = 1;
            this.button_Calc.Text = "Calcular";
            this.button_Calc.UseVisualStyleBackColor = true;
            this.button_Calc.Click += new System.EventHandler(this.button_Calc_Click);
            // 
            // button_Next
            // 
            this.button_Next.Location = new System.Drawing.Point(12, 313);
            this.button_Next.Name = "button_Next";
            this.button_Next.Size = new System.Drawing.Size(120, 23);
            this.button_Next.TabIndex = 2;
            this.button_Next.Text = "Próximo";
            this.button_Next.UseVisualStyleBackColor = true;
            this.button_Next.Click += new System.EventHandler(this.button_Next_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_SesgoD);
            this.panel1.Controls.Add(this.label_SesgoPerc);
            this.panel1.Controls.Add(this.label_UMindep);
            this.panel1.Controls.Add(this.label_UMeclipse);
            this.panel1.Controls.Add(this.label_Energia);
            this.panel1.Controls.Add(this.label_Equipo);
            this.panel1.Controls.Add(this.label_IDcampo);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(151, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(307, 324);
            this.panel1.TabIndex = 3;
            // 
            // label_SesgoD
            // 
            this.label_SesgoD.AutoSize = true;
            this.label_SesgoD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SesgoD.Location = new System.Drawing.Point(158, 241);
            this.label_SesgoD.Name = "label_SesgoD";
            this.label_SesgoD.Size = new System.Drawing.Size(97, 20);
            this.label_SesgoD.TabIndex = 13;
            this.label_SesgoD.Text = "Sesgo (cGy)";
            // 
            // label_SesgoPerc
            // 
            this.label_SesgoPerc.AutoSize = true;
            this.label_SesgoPerc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SesgoPerc.ForeColor = System.Drawing.Color.Green;
            this.label_SesgoPerc.Location = new System.Drawing.Point(158, 208);
            this.label_SesgoPerc.Name = "label_SesgoPerc";
            this.label_SesgoPerc.Size = new System.Drawing.Size(83, 20);
            this.label_SesgoPerc.TabIndex = 12;
            this.label_SesgoPerc.Text = "Sesgo (%)";
            // 
            // label_UMindep
            // 
            this.label_UMindep.AutoSize = true;
            this.label_UMindep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_UMindep.Location = new System.Drawing.Point(158, 156);
            this.label_UMindep.Name = "label_UMindep";
            this.label_UMindep.Size = new System.Drawing.Size(83, 20);
            this.label_UMindep.TabIndex = 11;
            this.label_UMindep.Text = "UM Indep.";
            // 
            // label_UMeclipse
            // 
            this.label_UMeclipse.AutoSize = true;
            this.label_UMeclipse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_UMeclipse.Location = new System.Drawing.Point(158, 124);
            this.label_UMeclipse.Name = "label_UMeclipse";
            this.label_UMeclipse.Size = new System.Drawing.Size(89, 20);
            this.label_UMeclipse.TabIndex = 10;
            this.label_UMeclipse.Text = "UM Eclipse";
            // 
            // label_Energia
            // 
            this.label_Energia.AutoSize = true;
            this.label_Energia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Energia.Location = new System.Drawing.Point(158, 74);
            this.label_Energia.Name = "label_Energia";
            this.label_Energia.Size = new System.Drawing.Size(64, 20);
            this.label_Energia.TabIndex = 9;
            this.label_Energia.Text = "Energia";
            // 
            // label_Equipo
            // 
            this.label_Equipo.AutoSize = true;
            this.label_Equipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Equipo.Location = new System.Drawing.Point(158, 42);
            this.label_Equipo.Name = "label_Equipo";
            this.label_Equipo.Size = new System.Drawing.Size(59, 20);
            this.label_Equipo.TabIndex = 8;
            this.label_Equipo.Text = "Equipo";
            // 
            // label_IDcampo
            // 
            this.label_IDcampo.AutoSize = true;
            this.label_IDcampo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_IDcampo.Location = new System.Drawing.Point(158, 14);
            this.label_IDcampo.Name = "label_IDcampo";
            this.label_IDcampo.Size = new System.Drawing.Size(86, 20);
            this.label_IDcampo.TabIndex = 7;
            this.label_IDcampo.Text = "ID_Campo";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(14, 241);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Sesgo (cGy)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Sesgo (%)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "UM Indep.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "UM Eclipse";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Energia";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Equipo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID_Campo";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(491, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(430, 430);
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // textBox_Bolus
            // 
            this.textBox_Bolus.Location = new System.Drawing.Point(12, 169);
            this.textBox_Bolus.Name = "textBox_Bolus";
            this.textBox_Bolus.Size = new System.Drawing.Size(120, 20);
            this.textBox_Bolus.TabIndex = 5;
            this.textBox_Bolus.Text = "0";
            this.textBox_Bolus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Espesor Bolus (mm)";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 207);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Angulo de incidencia(º)";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // textBoxAng
            // 
            this.textBoxAng.Location = new System.Drawing.Point(12, 223);
            this.textBoxAng.Name = "textBoxAng";
            this.textBoxAng.Size = new System.Drawing.Size(120, 20);
            this.textBoxAng.TabIndex = 7;
            this.textBoxAng.Text = "0";
            this.textBoxAng.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form_CI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 450);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxAng);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_Bolus);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_Next);
            this.Controls.Add(this.button_Calc);
            this.Controls.Add(this.listBox_RefPoints);
            this.Name = "Form_CI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calculo independiente";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_RefPoints;
        private System.Windows.Forms.Button button_Calc;
        private System.Windows.Forms.Button button_Next;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_SesgoD;
        private System.Windows.Forms.Label label_SesgoPerc;
        private System.Windows.Forms.Label label_UMindep;
        private System.Windows.Forms.Label label_UMeclipse;
        private System.Windows.Forms.Label label_Energia;
        private System.Windows.Forms.Label label_Equipo;
        private System.Windows.Forms.Label label_IDcampo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TextBox textBox_Bolus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxAng;
    }
}