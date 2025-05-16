using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;
using EvilDICOM.Core.Modules;

namespace CalcIndep_Carpeta
{
    public partial class Form1 : Form
    {
        VMS.TPS.Common.Model.API.Application app;
        VMS.TPS.Common.Model.API.Patient paciente;
        Course course;
        PlanningItem plan;
        int PlannedFractions;
        List<Course> Cursos = new List<Course>();
        List<PlanningItem> Planes = new List<PlanningItem>();


        bool hayContext = false;
        User usuarioContext = null;
        IEnumerable<PlanSum> planSumsContext = null;

        string texto = "";


        public Form1(bool _hayContext = false, VMS.TPS.Common.Model.API.Patient _pacienteContext = null, PlanSetup _planContext = null, User _usuarioContext = null, IEnumerable<PlanSum> _planSumsContext = null)
        {
            InitializeComponent();
            usuarioContext = _usuarioContext;
            hayContext = _hayContext;
            plan = _planContext;
            planSumsContext = _planSumsContext;

            if (hayContext && plan != null)
            {
                panel2.Enabled = false;
                panel3.Enabled = false;
                paciente = _pacienteContext;
                plan = _planContext;
                label_Paciente.Text = (paciente.LastName + ", " + paciente.FirstName);
                listBox_Plans.Items.Add(plan);
                texto += Chequeos.chequeos(plan, false);
                if (texto != "")
                {
                    if (MessageBox.Show(texto, "Chequeos en plan actual") == DialogResult.OK)
                    {

                    }
                }
                else
                {
                    if (MessageBox.Show("Todo bien", "Chequeos en plan actual") == DialogResult.OK)
                    {

                    }
                }
            }
            else if (hayContext && plan == null)
            {
                if (_planSumsContext != null)
                {
                    PlanesSumaContext planesSumaContext = new PlanesSumaContext(planSumsContext);
                    planesSumaContext.ShowDialog();
                    plan = planesSumaContext.PlanSuma;
                    panel2.Enabled = false;
                    panel3.Enabled = false;
                    paciente = _pacienteContext;
                    label_Paciente.Text = (paciente.LastName + ", " + paciente.FirstName);
                    listBox_Plans.Items.Add(plan);
                    texto += Chequeos.chequeos(plan, true);
                    if (texto != "")
                    {
                        if (MessageBox.Show(texto, "Chequeos en plan actual") == DialogResult.OK)
                        {

                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Todo bien", "Chequeos en plan actual") == DialogResult.OK)
                        {

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un plan");
                    this.Close();
                }

            }
            else
            {

                try
                {
                    app = VMS.TPS.Common.Model.API.Application.CreateApplication("paberbuj", "123qwe");
                }
                catch (Exception)
                {
                    MessageBox.Show("No se puede acceder a Eclipse.\n Compruebe que está en una PC con acceso al TPS");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void bton_Open_Click(object sender, EventArgs e)
        {
            abrirPaciente(textBox_HC.Text);
            if (paciente != null)
            {
                label_Paciente.Text = (paciente.LastName + ", " + paciente.FirstName);
                Cursos = listaCursos(paciente);
                listBox_Course.DataSource = Cursos;
            }
            //listBox_Course.Items = Cursos;
            //plan = paciente.Courses.First().PlanSetups.First();
            //MessageBox.Show(texto);

        }


        public Course abrirCurso(VMS.TPS.Common.Model.API.Patient paciente, string nombreCurso)
        {
            return paciente.Courses.Where(c => c.Id == nombreCurso).FirstOrDefault();
        }

        public PlanningItem abrirPlan(Course curso, string nombrePlan)
        {
            return curso.PlanSetups.Where(p => p.Id == nombrePlan).FirstOrDefault();
        }

        /* public Course cursoSeleccionado()
         {
             if (LB_Cursos.SelectedItems.Count == 1)
             {
                 return (Course)LB_Cursos.SelectedItems[0];
             }
             else
             {
                 return curso;
             }
         }*/

        /* public PlanningItem planSeleccionado()
         {
             if (hayContext)
             {
                 return plan;
             }
             else if (LB_Planes.SelectedItems.Count == 1)
             {
                 return (PlanningItem)LB_Planes.SelectedItems[0];
             }
             else
             {
                 return plan;
             }
         }*/


        public List<Course> listaCursos(VMS.TPS.Common.Model.API.Patient paciente)
        {
            return paciente.Courses.ToList<Course>();
        }

        public List<PlanningItem> listaPlanes(Course curso)
        {
            List<PlanningItem> lista = new List<PlanningItem>();
            lista.Clear();
            foreach (PlanSetup planSetup in curso.PlanSetups)
            {
                lista.Add(planSetup);
            }
            foreach (PlanSum planSum in curso.PlanSums)
            {
                lista.Add(planSum);
            }
            return lista;
        }




        public bool abrirPaciente(string ID)
        {
            if (paciente != null)
            {
                cerrarPaciente();
            }
            if (app.PatientSummaries.Any(p => p.Id == ID))
            {
                paciente = app.OpenPatientById(ID);
                //L_NombrePaciente.Text = paciente.LastName + ", " + paciente.FirstName;
                //L_NombrePaciente.Visible = true;
                this.Text += " - " + paciente.LastName + ", " + paciente.FirstName;
                return true;
            }
            else
            {
                MessageBox.Show("El paciente no existe");
                //L_NombrePaciente.Visible = false;
                return false;
            }
        }

        public void cerrarPaciente()
        {
            app.ClosePatient();
        }

        private void listBox_Course_SelectedIndexChanged(object sender, EventArgs e)
        {
            Course Curso_actual = (Course)listBox_Course.SelectedItem;
            Planes = listaPlanes(Curso_actual);
            //listBox_Plans.Items.Clear();
            listBox_Plans.DataSource = Planes;
        }

        private void button_CI_Click(object sender, EventArgs e)
        {
            if (plan is PlanSetup)
            {
                if (((PlanSetup)plan).ApprovalStatus != PlanSetupApprovalStatus.Rejected)
                {
                    MessageBox.Show("El plan " + plan.Id + " no está rechazado\nLos planes CI deben quedar rechazados para evitar errores");
                }
                calculoIndependiente((PlanSetup)plan);
            }
            else
            {
                foreach (PlanSetup planSetup in ((PlanSum)plan).PlanSetups)
                {
                    if (planSetup.ApprovalStatus != PlanSetupApprovalStatus.Rejected)
                    {
                        MessageBox.Show("El plan " + plan.Id + " no está rechazado\nLos planes CI deben quedar rechazados para evitar errores");
                    }
                    calculoIndependiente(planSetup);
                }
            }

        }

        private void listBox_Plans_SelectedIndexChanged(object sender, EventArgs e)
        {
            plan = (PlanningItem)listBox_Plans.SelectedItem;
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if (paciente != null)
            {
                cerrarPaciente();
            }
            System.Environment.Exit(1);
        }

        private void button_PPF_Click(object sender, EventArgs e)
        {
            if (plan is PlanSetup)
            {
                OrdenCampos ordenCampos = new OrdenCampos(paciente, (PlanSetup)plan, false);
                ordenCampos.ShowDialog();
            }
            else
            {
                foreach (PlanSetup planSetup in ((PlanSum)plan).PlanSetups)
                {
                    OrdenCampos ordenCampos = new OrdenCampos(paciente, planSetup,true);
                    ordenCampos.ShowDialog();
                }
            }

        }

        private void button_PatMove_Click(object sender, EventArgs e)
        {
            if (plan is PlanSetup)
            {
                try
                {
                    //crearPatMove.generarTodosLosPatMove(paciente, (PlanSetup)plan);
                    crearPatMove.enviarTodosLosCorrimientos(paciente, (PlanSetup)plan);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocurrió un error:\n" + e.ToString());
                    throw;
                }
            }
            else
            {
                try
                {
                    crearPatMove.enviarTodosLosCorrimientos(paciente, (PlanSum)plan);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocurrió un error:\n" + e.ToString());
                    throw;
                }
            }
        }

        private void button_Informe_Click(object sender, EventArgs e)
        {
            StructureSet structureSet;
            if (plan is PlanSetup)
            {
                structureSet = ((PlanSetup)plan).StructureSet;
            }
            else
            {
                structureSet = ((PlanSum)plan).PlanSetups.First().StructureSet;
            }
            EstructurasDVHInforme edvhInforme = new EstructurasDVHInforme(paciente, plan, structureSet, hayContext);
            edvhInforme.ShowDialog();
            edvhInforme.Close();

        }

        private void Button_Configurar_Click(object sender, EventArgs e)
        {
            Configuracion configuracion = new Configuracion();
            configuracion.ShowDialog();
            configuracion.Close();
        }

        private void calculoIndependiente(PlanSetup plan)
        {
            PlannedFractions = (int)plan.UniqueFractionation.NumberOfFractions;
            List<ReporteCI.CampoCI> camposCI = new List<ReporteCI.CampoCI>();
            int campoNumero = 1;
            int numeroCampos = plan.Beams.Where(b => b.IsSetupField == false && b.Technique.Id.Equals("STATIC")).Count();
            foreach (Beam campo in plan.Beams)
            {
                if (!campo.IsSetupField && campo.Technique.Id.Equals("STATIC"))
                {
                    {
                        //ReporteCI.CampoCI campoCI = new ReporteCI.CampoCI();
                        using (Form_CI formCI = new Form_CI(plan, campo, PlannedFractions, plan.TreatmentOrientation.ToString(), campoNumero, numeroCampos))
                        {
                            formCI.StartPosition = FormStartPosition.CenterParent;
                            if (formCI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                camposCI.Add(formCI.campoCI);
                            }
                            else
                            {
                                break;
                            }

                        }
                    }



                }
                campoNumero++;
            }
            string usarioId = "";
            if (usuarioContext != null)
            {
                usarioId = usuarioContext.Id;
            }
            if (numeroCampos == camposCI.Count())
            {
                VistaPrevia vistaPrevia = new VistaPrevia(ReporteCI.informe(paciente, plan, usarioId, camposCI), paciente, plan, usarioId, camposCI);
                vistaPrevia.ShowDialog();
                foreach (ReporteCI.CampoCI campo in camposCI)
                {
                    File.Delete(campo.ID + ".png");
                }
                vistaPrevia.Close();
            }
            else
            {

            }
        }

        private void BT_GuardarImagenes_Click(object sender, EventArgs e)
        {
            
            if (plan is PlanSetup)
            {
                string Equipo = NombreEquipo((PlanSetup)plan);
                if (Equipo == null)
                {
                    MessageBox.Show("No se pueden enviar imágenes a ese equipo");
                }
                if (((PlanSetup)plan).Beams.Any(b => b.ReferenceImage == null))
                {
                    MessageBox.Show("Una o más imágenes no tiene generadas las DRR. Corregir");
                }
                else
                {
                    string path = IO.crearCarpetaPacienteImagenes(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, plan).Id, plan.Id, Equipo);
                    int numero = DRR.GenerarImagenes((PlanSetup)plan, path, crearInforme.Curso(paciente, plan), paciente);
                    List<string> imagenesExtra = ObtenerImagenesPaciente(paciente);
                    if (imagenesExtra.Count>0)
                    {
                        AgregarImagenes agregarImagenes = new AgregarImagenes(ObtenerImagenesPaciente(paciente), path);
                        agregarImagenes.ShowDialog();
                        numero += agregarImagenes.NumeroDeImagenes;
                    }
                    MessageBox.Show("Se generaron y enviaron " + numero.ToString() + " imágenes del plan " + plan.Id);
                }
            }
            else
            {
                foreach (PlanSetup planSetup in ((PlanSum)plan).PlanSetups)
                {
                    string Equipo = NombreEquipo(planSetup);
                    if (Equipo == null)
                    {
                        MessageBox.Show("No se pueden enviar imágenes a ese equipo");
                    }
                    
                    if (((PlanSetup)planSetup).Beams.Any(b=>b.ReferenceImage==null))
                    {
                        MessageBox.Show("Una o más imágenes no tiene generadas las DRR. Corregir");
                    }
                    else
                    {
                        string path = IO.crearCarpetaPacienteImagenes(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, planSetup).Id, planSetup.Id, Equipo);
                        int numero = DRR.GenerarImagenes(planSetup, path, crearInforme.Curso(paciente, planSetup), paciente);
                        List<string> imagenesExtra = ObtenerImagenesPaciente(paciente);                        
                        if (imagenesExtra.Count > 0)
                        {
                            AgregarImagenes agregarImagenes = new AgregarImagenes(ObtenerImagenesPaciente(paciente), path);
                            agregarImagenes.ShowDialog();
                            numero += agregarImagenes.NumeroDeImagenes;
                        }                        
                        MessageBox.Show("Se generaron y enviaron " + numero.ToString() + " imágenes del plan " + planSetup.Id);
                    }
                }
            }
            /*else if (MessageBox.Show("Se encontraron " + imagenes.Count + " imagenes.\n¿Guardar en carpeta de equipo?","Guardar imágenes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string path = IO.crearCarpetaPacienteImagenes(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, plan).Id, plan.Id, Equipo);
                foreach (string imagen in imagenes)
                {
                    File.Move(imagen, path + @"\Imagen_" + imagenes.IndexOf(imagen) + ".png");
                }
            }*/
        }

        private List<string> ObtenerImagenesPaciente(VMS.TPS.Common.Model.API.Patient paciente)
        {
            return Directory.GetFiles(Properties.Settings.Default.PathPrograma + @"\Imagenes").Where(f => f.Contains(paciente.Id)).ToList();
        }
        private string NombreEquipo(PlanSetup plan)
        {
            if (plan.Beams.First().TreatmentUnit.Id == "Equipo1")
            {
                return "Equipo1";
            }
            else if (plan.Beams.First().TreatmentUnit.Id == "Equipo 2 6EX")
            {
                return "Equipo2";
            }
            else if (plan.Beams.First().TreatmentUnit.Id == "Equipo3")
            {
                return "Equipo3";
            }
            else if (plan.Beams.First().TreatmentUnit.Id == "D-2300CD")
            {
                return "Equipo4";
            }
            else if (plan.Beams.First().TreatmentUnit.Id == "CL21EX")
            {
                return "EquipoMedrano";
            }
            else if (plan.Beams.First().TreatmentUnit.Id == "PBA_6EX_730")
            {
                return "EquipoCetro";
            }
            else if (plan.Beams.First().TreatmentUnit.Id == "QBA_600CD_523")
            {
                return "Q_Equipo1";
            }
            else if (plan.Beams.First().TreatmentUnit.Id == "EQ2_iX_827")
            {
                return "Q_Equipo2";
            }
            else
            {
                return null;
            }

        }
    }
}

