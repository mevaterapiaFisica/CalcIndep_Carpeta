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
    public partial class DatosPlanSuma : Form
    {
        public string tipoTratamiento;
        public double dosisTotal;
        public int? numeroFracciones;

        public DatosPlanSuma(string _tipoTratamiento, double _dosisTotal, int? _numeroFracciones)
        {
            InitializeComponent();

            if (_tipoTratamiento == "de intensidad modulada (IMRT)")
            {
                CB_TipoTratamiento.SelectedIndex = 0;
            }
            else if (_tipoTratamiento == "de intensidad modulada (IMRT) en modalidad VMAT")
            {
                CB_TipoTratamiento.SelectedIndex = 1;
            }
            else
            {
                CB_TipoTratamiento.SelectedIndex = 2;
            }
            
            TB_DosisTotal.Text = _dosisTotal.ToString();
            TB_NumeroFracciones.Text = _numeroFracciones.ToString();

        }

        private void BT_Aceptar_Click(object sender, EventArgs e)
        {
            if (CB_TipoTratamiento.SelectedIndex == 0)
            {
                tipoTratamiento = "de intensidad modulada (IMRT)";
            }
            else if (CB_TipoTratamiento.SelectedIndex == 1)
            {
                tipoTratamiento = "de intensidad modulada (IMRT) en modalidad VMAT";
            }
            else if (CB_TipoTratamiento.SelectedIndex == 2)
            {
                tipoTratamiento = "tridimensional conformado (3DC)";
            }
            else if (CB_TipoTratamiento.SelectedIndex == 3)
            {
                tipoTratamiento = "de radiocirugía estereotáxica craneal (SRS)";
            }
            else if (CB_TipoTratamiento.SelectedIndex == 4)
            {
                tipoTratamiento = "de radioterapia estereotáxica extracreaneal (SBRT)";
            }
            else
            {
                tipoTratamiento = "guiado por imágenes (IGRT)";
            }



                dosisTotal = Convert.ToDouble(TB_DosisTotal.Text);
            numeroFracciones = Convert.ToInt32(TB_NumeroFracciones.Text);
            Close();
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
 