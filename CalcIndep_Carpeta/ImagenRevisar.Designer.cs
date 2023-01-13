namespace CalcIndep_Carpeta
{
    partial class ImagenRevisar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PB_Imagen = new System.Windows.Forms.PictureBox();
            this.CB_IncluirImagen = new System.Windows.Forms.CheckBox();
            this.TB_ImagenNombre = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Imagen)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_Imagen
            // 
            this.PB_Imagen.Location = new System.Drawing.Point(30, 43);
            this.PB_Imagen.Name = "PB_Imagen";
            this.PB_Imagen.Size = new System.Drawing.Size(160, 120);
            this.PB_Imagen.TabIndex = 0;
            this.PB_Imagen.TabStop = false;
            // 
            // CB_IncluirImagen
            // 
            this.CB_IncluirImagen.AutoSize = true;
            this.CB_IncluirImagen.Checked = true;
            this.CB_IncluirImagen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_IncluirImagen.Location = new System.Drawing.Point(30, 20);
            this.CB_IncluirImagen.Name = "CB_IncluirImagen";
            this.CB_IncluirImagen.Size = new System.Drawing.Size(91, 17);
            this.CB_IncluirImagen.TabIndex = 1;
            this.CB_IncluirImagen.Text = "Incluir imagen";
            this.CB_IncluirImagen.UseVisualStyleBackColor = true;
            // 
            // TB_ImagenNombre
            // 
            this.TB_ImagenNombre.Location = new System.Drawing.Point(30, 178);
            this.TB_ImagenNombre.Name = "TB_ImagenNombre";
            this.TB_ImagenNombre.Size = new System.Drawing.Size(160, 20);
            this.TB_ImagenNombre.TabIndex = 2;
            // 
            // ImagenRevisar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TB_ImagenNombre);
            this.Controls.Add(this.CB_IncluirImagen);
            this.Controls.Add(this.PB_Imagen);
            this.Name = "ImagenRevisar";
            this.Size = new System.Drawing.Size(230, 230);
            ((System.ComponentModel.ISupportInitialize)(this.PB_Imagen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_Imagen;
        private System.Windows.Forms.CheckBox CB_IncluirImagen;
        private System.Windows.Forms.TextBox TB_ImagenNombre;
    }
}
