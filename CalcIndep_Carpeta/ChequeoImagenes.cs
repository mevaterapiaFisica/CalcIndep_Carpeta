using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CalcIndep_Carpeta
{
    public partial class ChequeoImagenes : Form
    {
        public bool imagenesOK = false;
        public ChequeoImagenes(Image axial, Image sagital, Image coronal, Image nombrePaciente)
        {
            InitializeComponent();
            PB_Nombre.Image = nombrePaciente;
            PB_Axial.Image = axial;
            PB_Coronal.Image = coronal;
            PB_Sagital.Image = sagital;
            PB_Axial.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_Coronal.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_Sagital.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void BT_No_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BT_Si_Click(object sender, EventArgs e)
        {
            imagenesOK = true;
            Close();
        }
    }
}
