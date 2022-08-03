namespace CalcIndep_Carpeta
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Informe = new System.Windows.Forms.Button();
            this.BT_GuardarImagenes = new System.Windows.Forms.Button();
            this.button_PPF = new System.Windows.Forms.Button();
            this.button_PatMove = new System.Windows.Forms.Button();
            this.button_CI = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_Close = new System.Windows.Forms.Button();
            this.textBox_HC = new System.Windows.Forms.TextBox();
            this.bton_Open = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label_Paciente = new System.Windows.Forms.Label();
            this.listBox_Plans = new System.Windows.Forms.ListBox();
            this.listBox_Course = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Button_Configurar = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_Informe);
            this.panel1.Controls.Add(this.BT_GuardarImagenes);
            this.panel1.Controls.Add(this.button_PPF);
            this.panel1.Controls.Add(this.button_PatMove);
            this.panel1.Controls.Add(this.button_CI);
            this.panel1.Location = new System.Drawing.Point(153, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(153, 158);
            this.panel1.TabIndex = 0;
            // 
            // button_Informe
            // 
            this.button_Informe.Location = new System.Drawing.Point(3, 118);
            this.button_Informe.Name = "button_Informe";
            this.button_Informe.Size = new System.Drawing.Size(141, 23);
            this.button_Informe.TabIndex = 5;
            this.button_Informe.Text = "Generar Informe";
            this.button_Informe.UseVisualStyleBackColor = true;
            this.button_Informe.Click += new System.EventHandler(this.button_Informe_Click);
            // 
            // BT_GuardarImagenes
            // 
            this.BT_GuardarImagenes.Location = new System.Drawing.Point(3, 89);
            this.BT_GuardarImagenes.Name = "BT_GuardarImagenes";
            this.BT_GuardarImagenes.Size = new System.Drawing.Size(141, 23);
            this.BT_GuardarImagenes.TabIndex = 4;
            this.BT_GuardarImagenes.Text = "Guardar Imagenes";
            this.BT_GuardarImagenes.UseVisualStyleBackColor = true;
            this.BT_GuardarImagenes.Click += new System.EventHandler(this.BT_GuardarImagenes_Click);
            // 
            // button_PPF
            // 
            this.button_PPF.Location = new System.Drawing.Point(3, 60);
            this.button_PPF.Name = "button_PPF";
            this.button_PPF.Size = new System.Drawing.Size(141, 23);
            this.button_PPF.TabIndex = 3;
            this.button_PPF.Text = "Generar .PPF";
            this.button_PPF.UseVisualStyleBackColor = true;
            this.button_PPF.Click += new System.EventHandler(this.button_PPF_Click);
            // 
            // button_PatMove
            // 
            this.button_PatMove.Location = new System.Drawing.Point(3, 32);
            this.button_PatMove.Name = "button_PatMove";
            this.button_PatMove.Size = new System.Drawing.Size(141, 23);
            this.button_PatMove.TabIndex = 2;
            this.button_PatMove.Text = "Generar PatMove";
            this.button_PatMove.UseVisualStyleBackColor = true;
            this.button_PatMove.Click += new System.EventHandler(this.button_PatMove_Click);
            // 
            // button_CI
            // 
            this.button_CI.Location = new System.Drawing.Point(3, 3);
            this.button_CI.Name = "button_CI";
            this.button_CI.Size = new System.Drawing.Size(141, 23);
            this.button_CI.TabIndex = 1;
            this.button_CI.Text = "Cálculo Independiente";
            this.button_CI.UseVisualStyleBackColor = true;
            this.button_CI.Click += new System.EventHandler(this.button_CI_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_Close);
            this.panel2.Controls.Add(this.textBox_HC);
            this.panel2.Controls.Add(this.bton_Open);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(135, 94);
            this.panel2.TabIndex = 1;
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(3, 60);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(129, 23);
            this.button_Close.TabIndex = 2;
            this.button_Close.Text = "Salir";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // textBox_HC
            // 
            this.textBox_HC.Location = new System.Drawing.Point(3, 6);
            this.textBox_HC.Name = "textBox_HC";
            this.textBox_HC.Size = new System.Drawing.Size(129, 20);
            this.textBox_HC.TabIndex = 2;
            // 
            // bton_Open
            // 
            this.bton_Open.Location = new System.Drawing.Point(3, 31);
            this.bton_Open.Name = "bton_Open";
            this.bton_Open.Size = new System.Drawing.Size(129, 23);
            this.bton_Open.TabIndex = 2;
            this.bton_Open.Text = "Abrir Paciente";
            this.bton_Open.UseVisualStyleBackColor = true;
            this.bton_Open.Click += new System.EventHandler(this.bton_Open_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label_Paciente);
            this.panel3.Controls.Add(this.listBox_Plans);
            this.panel3.Controls.Add(this.listBox_Course);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(12, 112);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(135, 332);
            this.panel3.TabIndex = 2;
            // 
            // label_Paciente
            // 
            this.label_Paciente.AutoSize = true;
            this.label_Paciente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Paciente.Location = new System.Drawing.Point(8, 11);
            this.label_Paciente.Name = "label_Paciente";
            this.label_Paciente.Size = new System.Drawing.Size(31, 13);
            this.label_Paciente.TabIndex = 3;
            this.label_Paciente.Text = "--------";
            // 
            // listBox_Plans
            // 
            this.listBox_Plans.FormattingEnabled = true;
            this.listBox_Plans.Location = new System.Drawing.Point(3, 204);
            this.listBox_Plans.Name = "listBox_Plans";
            this.listBox_Plans.Size = new System.Drawing.Size(129, 121);
            this.listBox_Plans.TabIndex = 5;
            this.listBox_Plans.SelectedIndexChanged += new System.EventHandler(this.listBox_Plans_SelectedIndexChanged);
            // 
            // listBox_Course
            // 
            this.listBox_Course.FormattingEnabled = true;
            this.listBox_Course.Location = new System.Drawing.Point(3, 53);
            this.listBox_Course.Name = "listBox_Course";
            this.listBox_Course.Size = new System.Drawing.Size(129, 121);
            this.listBox_Course.TabIndex = 3;
            this.listBox_Course.SelectedIndexChanged += new System.EventHandler(this.listBox_Course_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Planes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Courses";
            // 
            // Button_Configurar
            // 
            this.Button_Configurar.Location = new System.Drawing.Point(156, 414);
            this.Button_Configurar.Name = "Button_Configurar";
            this.Button_Configurar.Size = new System.Drawing.Size(141, 23);
            this.Button_Configurar.TabIndex = 6;
            this.Button_Configurar.Text = "Configurar";
            this.Button_Configurar.UseVisualStyleBackColor = true;
            this.Button_Configurar.Click += new System.EventHandler(this.Button_Configurar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 450);
            this.Controls.Add(this.Button_Configurar);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_CI;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox_HC;
        private System.Windows.Forms.Button bton_Open;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox listBox_Plans;
        private System.Windows.Forms.ListBox listBox_Course;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Paciente;
        private System.Windows.Forms.Button button_PPF;
        private System.Windows.Forms.Button button_PatMove;
        private System.Windows.Forms.Button button_Informe;
        private System.Windows.Forms.Button BT_GuardarImagenes;
        private System.Windows.Forms.Button Button_Configurar;
    }
}

