using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VMS.TPS.Common.Model.API;

namespace CalcIndep_Carpeta
{
    public partial class AgregarImagenes : Form
    {
        List<string> ImagenesPaciente;
        Patient paciente;
        public int NumeroDeImagenes = 0;
        string path;
        public AgregarImagenes(List<string> _imagenesPaciente, string _path)
        {
            InitializeComponent();
            ImagenesPaciente = _imagenesPaciente;
            CargarTodasLasImagenes();
            this.AutoSize = true;
            //paciente = _paciente;
            path = _path;
        }

        public void AgregarImagen(string imagen, int indice)
        {
            ImagenRevisar imagenRevisar = new ImagenRevisar(indice,imagen);
            imagenRevisar.Name = imagen;
            this.Controls.Add(imagenRevisar);
            imagenRevisar.Location = new Point(40 + indice * 240, 50);
        }
        public void CargarTodasLasImagenes()
        {
            int indice = 0;
            foreach (string imagen in ImagenesPaciente)
            {
                AgregarImagen(imagen, indice);
                indice++;
            }
        }

        private void BT_Guardar_Click(object sender, EventArgs e)
        {
            this.Close();
            foreach (ImagenRevisar ir in Controls.OfType<ImagenRevisar>())
            {
                if (ir.Seleccionada())
                {
                    File.Move(ir.Name, IO.GetUniqueFilename(path + @"\", ir.Nombre(),"png"));
                    NumeroDeImagenes++;
                    //File.Delete(ir.PathImagen());
                }
            }
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
