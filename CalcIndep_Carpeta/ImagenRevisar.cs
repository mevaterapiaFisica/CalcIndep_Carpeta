using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalcIndep_Carpeta
{
    public partial class ImagenRevisar : UserControl
    {
        public static string pathImagen;
        public ImagenRevisar(int indice, string _Imagen)
        {
            InitializeComponent();
            TB_ImagenNombre.Text = "Imagen" + indice.ToString();
            Image imagen = Image.FromStream(new MemoryStream(File.ReadAllBytes(_Imagen)));
            PB_Imagen.Image = imagen;
            PB_Imagen.SizeMode = PictureBoxSizeMode.StretchImage;
            pathImagen = _Imagen;
        }
        public bool Seleccionada()
        {
            return CB_IncluirImagen.Checked;
        }
        public string PathImagen()
        {
            return pathImagen;
        }
        public string Nombre()
        {
            return TB_ImagenNombre.Text;
        }
    }
}
