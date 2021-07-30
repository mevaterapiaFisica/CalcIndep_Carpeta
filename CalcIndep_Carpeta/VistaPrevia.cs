using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Forms;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;

namespace CalcIndep_Carpeta
{
    public partial class VistaPrevia : Form
    {
        Document report;
        Patient paciente;
        PlanSetup plan;
        string usuario;
        List<ReporteCI.CampoCI> camposCI;
        public VistaPrevia(Document _report, Patient _paciente, PlanSetup _plan, string _usuario, List<ReporteCI.CampoCI> _camposCI)
        {
            InitializeComponent();
            report = _report;
            paciente = _paciente;
            plan = _plan;
            usuario = _usuario;
            camposCI = _camposCI;
        }

        private void BT_Imprimir_Click(object sender, EventArgs e)
        {
            ReporteCI.imprimir(report, paciente, plan, usuario, camposCI);
            Close();
        }

        private void llenarDGV(List<ReporteCI.CampoCI> camposCI)
        {
            DGV_Resultado.DataSource = camposCI;
            DGV_Resultado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void colorCelda(DataGridView dgv)
        {
            foreach (DataGridViewRow fila in dgv.Rows)
            {
                double tol_porciento = 4; // 4 % de tolerancia en dosis por campo
                double tol_dosis = 2;     //2 cGy de tolerancia en dosis por campo
                if (Math.Abs(Convert.ToDouble(fila.Cells[4].Value)) <= tol_porciento)
                {
                    //fila.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    fila.Cells[4].Style.BackColor = System.Drawing.Color.LightGreen;
                    fila.Cells[5].Style.BackColor = System.Drawing.Color.LightGreen;
                }
                else if (Math.Abs(Convert.ToDouble(fila.Cells[4].Value)) > tol_porciento && Math.Abs(Convert.ToDouble(fila.Cells[5].Value)) <= tol_dosis)
                {
                    fila.Cells[4].Style.BackColor = System.Drawing.Color.LightYellow;
                    fila.Cells[5].Style.BackColor = System.Drawing.Color.LightYellow;
                }
                else
                {
                    fila.Cells[4].Style.BackColor = System.Drawing.Color.Red;
                    fila.Cells[5].Style.BackColor = System.Drawing.Color.Red;
                }
            }
            
        }

        private void VistaPrevia_Load(object sender, EventArgs e)
        {
            llenarDGV(camposCI);
            colorCelda(DGV_Resultado);
        }
    }
}

