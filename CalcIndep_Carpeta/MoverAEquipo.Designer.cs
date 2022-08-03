namespace CalcIndep_Carpeta
{
    partial class MoverAEquipo
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
            this.BT_EnviarAEq2 = new System.Windows.Forms.Button();
            this.BT_EnviarAEq3 = new System.Windows.Forms.Button();
            this.BT_NoEnviar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Se ha encontrado un archivo dcm que coincide con el plan seleccionado\r\n¿Desea env" +
    "iarlo al equipo 2 o al equipo 3?";
            // 
            // BT_EnviarAEq2
            // 
            this.BT_EnviarAEq2.Location = new System.Drawing.Point(16, 60);
            this.BT_EnviarAEq2.Name = "BT_EnviarAEq2";
            this.BT_EnviarAEq2.Size = new System.Drawing.Size(82, 23);
            this.BT_EnviarAEq2.TabIndex = 1;
            this.BT_EnviarAEq2.Text = "Enviar a Eq2";
            this.BT_EnviarAEq2.UseVisualStyleBackColor = true;
            this.BT_EnviarAEq2.Click += new System.EventHandler(this.BT_EnviarAEq2_Click);
            // 
            // BT_EnviarAEq3
            // 
            this.BT_EnviarAEq3.Location = new System.Drawing.Point(149, 60);
            this.BT_EnviarAEq3.Name = "BT_EnviarAEq3";
            this.BT_EnviarAEq3.Size = new System.Drawing.Size(82, 23);
            this.BT_EnviarAEq3.TabIndex = 2;
            this.BT_EnviarAEq3.Text = "Enviar a Eq3";
            this.BT_EnviarAEq3.UseVisualStyleBackColor = true;
            this.BT_EnviarAEq3.Click += new System.EventHandler(this.BT_EnviarAEq3_Click);
            // 
            // BT_NoEnviar
            // 
            this.BT_NoEnviar.Location = new System.Drawing.Point(284, 60);
            this.BT_NoEnviar.Name = "BT_NoEnviar";
            this.BT_NoEnviar.Size = new System.Drawing.Size(82, 23);
            this.BT_NoEnviar.TabIndex = 3;
            this.BT_NoEnviar.Text = "No enviar";
            this.BT_NoEnviar.UseVisualStyleBackColor = true;
            this.BT_NoEnviar.Click += new System.EventHandler(this.BT_NoEnviar_Click);
            // 
            // MoverAEquipo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 95);
            this.Controls.Add(this.BT_NoEnviar);
            this.Controls.Add(this.BT_EnviarAEq3);
            this.Controls.Add(this.BT_EnviarAEq2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MoverAEquipo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mover a otro equipo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BT_EnviarAEq2;
        private System.Windows.Forms.Button BT_EnviarAEq3;
        private System.Windows.Forms.Button BT_NoEnviar;
    }
}