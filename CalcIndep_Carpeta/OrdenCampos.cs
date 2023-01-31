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
using static CalcIndep_Carpeta.ReporteCI;


namespace CalcIndep_Carpeta
{
    public partial class OrdenCampos : Form
    {
        Patient paciente;
        PlanSetup plan;
        List<Beam> camposList = new List<Beam>();
        BindingList<Beam> campos = new BindingList<Beam>();
        bool esPlanSuma = false;
        public OrdenCampos(Patient _paciente, PlanSetup _plan, bool _esPlanSuma)
        {
            InitializeComponent();
            paciente = _paciente;
            plan = _plan;
            esPlanSuma = _esPlanSuma;
            Label_Plan.Text = plan.Id;
            camposList = crearListaDeCampos(plan);
            campos = new BindingList<Beam>(camposList);
            LB_Campos.DataSource = campos;
            LB_Campos.DisplayMember = "Id";
            Text = plan.Id + " - orden campos";
        }

        public List<Beam> extraerLista()
        {
            return campos.ToList();
            //return LB_Campos.Items.Cast<Beam>().ToList();
        }

        public List<Beam> crearListaDeCampos(PlanSetup plan)
        {
            List<Beam> campos = new List<Beam>();
            foreach (Beam campo in plan.Beams)
            {
                if (!campo.IsSetupField)
                {
                    campos.Add(campo);
                }
            }
            return campos;
        }



        private void BT_Subir_Click(object sender, EventArgs e)
        {
            int indice = LB_Campos.SelectedIndex;
            Beam item = (Beam)LB_Campos.SelectedItem;
            campos.Remove(item);
            campos.Insert(indice - 1, item);
            LB_Campos.ClearSelected();
            LB_Campos.SelectedIndex = indice - 1;
        }

        private void BT_Bajar_Click(object sender, EventArgs e)
        {
            int indice = LB_Campos.SelectedIndex;
            Beam item = (Beam)LB_Campos.SelectedItem;
            campos.Remove(item);
            campos.Insert(indice + 1, item);
            LB_Campos.ClearSelected();
            LB_Campos.SelectedIndex = indice + 1;
        }

        private void BT_Horario_Click(object sender, EventArgs e)
        {
            //camposList.Sort((x, y) => x.ControlPoints.First().GantryAngle.CompareTo(y.ControlPoints.First().GantryAngle));
            campos.ResetBindings();
            campos = new BindingList<Beam>(camposList.OrderByDescending(b => crearPPF.IECaVarian(b.ControlPoints.First().GantryAngle, crearPPF.EquipoEsIEC(b))).ThenBy(b => b.Id).ToList());
            LB_Campos.DataSource = campos;
            //LB_Campos.DataSource = LB_Campos.Items.Cast<Beam>().OrderBy(b => b.ControlPoints.First().GantryAngle).ToList();
        }

        private void BT_Antihorario_Click(object sender, EventArgs e)
        {
            campos.ResetBindings();
            campos = new BindingList<Beam>(camposList.OrderBy(b => crearPPF.IECaVarian(b.ControlPoints.First().GantryAngle, crearPPF.EquipoEsIEC(b))).ThenBy(b => b.Id).ToList());
            LB_Campos.DataSource = campos;
        }

        private void BT_Aceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = "Se creó el archivo .PPF correctamente";
                crearPPF.escribirPPFcompleto(paciente, plan, extraerLista());
                //if (plan.Beams.First().TreatmentUnit.Id== "6EX Viamonte" && Dcm.moverDCM(paciente,plan))
                if ((plan.Beams.First().TreatmentUnit.Id == "2100CMLC" || plan.Beams.First().TreatmentUnit.Id == "Equipo 2 6EX") && plan.ApprovalStatus == PlanSetupApprovalStatus.TreatmentApproved && Dcm.moverDCM(paciente, plan, esPlanSuma))
                {
                    texto += " y se movió el archivo dcm";
                }
                else if ((plan.Beams.First().TreatmentUnit.Id == "D-2300CD" || plan.Beams.First().TreatmentUnit.Id == "Equipo1"))
                {
                    string equipoOrigen = "Equipo 1";
                    if (plan.Beams.First().TreatmentUnit.Id == "D-2300CD")
                    {
                        equipoOrigen = "Equipo 4";
                    }
                    if (Dcm.obtenerDCM(paciente,plan)!= "No se encontró coincidencia")
                    {
                        if ((plan.Beams.First().ControlPoints.Count() > 30 && plan.Beams.First().MLCPlanType == MLCPlanType.DoseDynamic) ||plan.Beams.First().MLCPlanType == MLCPlanType.VMAT)
                        {

                        }
                        else
                        {
                            MoverAEquipo moverAEquipo = new MoverAEquipo(plan.Beams.Any(b => b.EnergyModeDisplayName == "10X"));
                            moverAEquipo.ShowDialog();
                            if (Dcm.moverDCM(paciente, plan, esPlanSuma, true, equipoOrigen, moverAEquipo.equipoAEnviar))
                            {
                                texto += " y se movió el archivo dcm";
                            }
                        }
                        
                    }
                }
                MessageBox.Show(texto);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error:\n" + e.ToString());
                throw;

            }
            Close();
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LB_Campos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LB_Campos.SelectedIndex == 0)
            {
                BT_Subir.Enabled = false;
            }
            else
            {
                BT_Subir.Enabled = true;
            }
            if (LB_Campos.SelectedIndex == LB_Campos.Items.Count - 1)
            {
                BT_Bajar.Enabled = false;
            }
            else
            {
                BT_Bajar.Enabled = true;
            }
        }
    }
}
