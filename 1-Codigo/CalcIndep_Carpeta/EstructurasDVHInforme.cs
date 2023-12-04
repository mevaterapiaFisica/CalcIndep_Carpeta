using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;

namespace CalcIndep_Carpeta
{
    public partial class EstructurasDVHInforme : Form
    {
        Patient paciente;
        PlanningItem plan;
        StructureSet estructuras;
        bool hayContext;
        public EstructurasDVHInforme(Patient _paciente, PlanningItem _plan, StructureSet _estructuras, bool _hayContext)
        {
            InitializeComponent();
            paciente = _paciente;
            plan = _plan;
            estructuras = _estructuras;
            cargarTabla(estructuras, chlb_Estructuras);
            hayContext = _hayContext;
        }

        public static void cargarTabla(StructureSet estructuras, CheckedListBox lista)
        {
            
            foreach (Structure estructura in estructuras.Structures)
            {
                if (estructura.DicomType != "SUPPORT")
                {
                    lista.Items.Add(estructura);
                }
            }
        }

        public static List<Structure> estructurasSeleccionadas(CheckedListBox lista)
        {
            return lista.CheckedItems.OfType<Structure>().ToList();
        }

        private void BT_Continuar_Click(object sender, EventArgs e)
        {
            if (estructurasSeleccionadas(chlb_Estructuras).Count>0)
            {
                crearInformeSuma.Informe(paciente, plan, estructurasSeleccionadas(chlb_Estructuras), hayContext);
                this.Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar al menos una estructura");
            }
            
            
        }

        private void BT_SeleccionarTodas_Click(object sender, EventArgs e)
        {
            for (int i=0;i<chlb_Estructuras.Items.Count;i++)
            {
                chlb_Estructuras.SetItemChecked(i, true);
            }
        }
    }

    
}
