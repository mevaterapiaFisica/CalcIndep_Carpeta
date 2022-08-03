using MathNet.Numerics;
using System;
//using System.DirectoryServices;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using EvilDICOM;
using EvilDICOM.Core.Helpers;
using EvilDICOM.Core.IO.Writing;
using EvilDICOM.Core.IO.Data;
using EvilDICOM.Core.Selection;
using System.Windows.Forms;
using System.Net;


namespace CalcIndep_Carpeta
{
    public class Dcm
    {
        public string path { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string ID { get; set; }
        public string planID { get; set; }
        public List<campoDCM> camposDCM { get; set; }

        public bool coincide(Patient paciente, PlanSetup plan)
        {
            List<Beam> camposECL = plan.Beams.Where(b => b.IsSetupField == false).ToList();
            if (Apellido != paciente.LastName || Nombre != paciente.FirstName || ID != paciente.Id || planID != plan.Id || camposDCM.Count != camposECL.Count())
            {
                //MessageBox.Show(Apellido + " " + Nombre + " " + ID + " " + camposDCM.Count.ToString());
                //MessageBox.Show(Path.GetFileName(path) + " no coincide 1");
                return false;
            }
            else
            {
                foreach (campoDCM _campoDCM in camposDCM)
                {
                    Beam campoECL = camposECL.Where(c => c.Id == _campoDCM.ID).First();
                    if (camposECL == null || _campoDCM.UM != Convert.ToInt32(Math.Round(campoECL.Meterset.Value)))
                    {
                        //MessageBox.Show(Path.GetFileName(path) + " no coincide 2");
                        return false;
                    }
                    //chequeo = _campoDCM.gantry == campoECL.ControlPoints.First().GantryAngle;
                    //chequeo = _campoDCM.colimador == campoECL.ControlPoints.First().CollimatorAngle;
                    //chequeo = _campoDCM.camilla == campoECL.ControlPoints.First().PatientSupportAngle;

                }
            }
            return true;


        }

        public static List<string> listaDCM()
        {
            return Directory.GetFiles(Properties.Settings.Default.PathDCMRP, "*.dcm").ToList();
        }

        public void crear(string archivo)
        {
            var objeto = EvilDICOM.Core.DICOMObject.Read(archivo);
            try
            {
                //DATOS
                string nombre = objeto.FindFirst("00100010").DData.ToString();
                nombre = nombre.Replace("??", "Ñ");
                string[] aux = nombre.Split('^');
                //Dcm dcm = new Dcm();
                Apellido = aux[0];
                Nombre = aux[1];
                ID = objeto.FindFirst("00100020").DData.ToString();
                planID = objeto.FindFirst("300A0002").DData.ToString();
                path = archivo;

                camposDCM = new List<campoDCM>();
                var Beams = objeto.FindFirst("300A00B0").DData_;
                var RefBeams = objeto.FindFirst("300C0004").DData_;
                foreach (var beam in Beams)
                {
                    campoDCM CampoDCM = new campoDCM();
                    CampoDCM.ID = ((EvilDICOM.Core.DICOMObject)beam).FindFirst("300A00C2").DData;
                    foreach (var refBeam in RefBeams)
                    {
                        if (((EvilDICOM.Core.DICOMObject)refBeam).FindFirst("300C0006").DData == ((EvilDICOM.Core.DICOMObject)beam).FindFirst("300A00C0").DData)
                        {
                            CampoDCM.UM = Convert.ToInt32(Math.Round((double)((EvilDICOM.Core.DICOMObject)refBeam).FindFirst("300A0086").DData));
                            break;
                        }
                    }
                    camposDCM.Add(CampoDCM);
                }
            }
            catch (Exception)
            {
            }
            
        }
        public static string obtenerDCM(Patient paciente, PlanSetup plan)
        {
            foreach (string dcmPath in listaDCM())
            {
                Dcm dcm = new Dcm();
                dcm.crear(dcmPath);
                if (dcm.coincide(paciente, plan))
                {
                    return dcmPath;
                }
            }
            return "No se encontró coincidencia";
        }

        public static bool moverDCM(Patient paciente, PlanSetup plan, bool esPlanSuma, bool vieneDeEq1oEq4=false, string equipoOrigen=null, string equipoDestino=null)
        {
            string path = obtenerDCM(paciente, plan);
            if (path != "No se encontró coincidencia")
            {
                string reingresoCurso = plan.Course.Id[1].ToString();
                string reingresoID = paciente.Id.Last().ToString();
                string IdCorregida = paciente.Id;
                if (reingresoCurso!=reingresoID)
                {
                    MessageBox.Show("El dígito de reingreso en el curso es " + reingresoCurso + " y difiere del hallado en la HC del paciente en Eclipse. Se toma el del curso para el nombre de la carpeta en DicomRT");
                    IdCorregida = paciente.Id.Remove(paciente.Id.Length - 1, 1) + reingresoCurso;
                }
                string pathPaciente="";
                if (plan.Beams.First().TreatmentUnit.Id=="2100CMLC")
                {
                    pathPaciente = Properties.Settings.Default.PathDCMEquipo + @"\" + paciente.LastName.ToUpper() + ", " + paciente.FirstName + " " + IdCorregida;
                }
                else if (plan.Beams.First().TreatmentUnit.Id== "Equipo 2 6EX")
                {
                    pathPaciente = Properties.Settings.Default.PathDCMEquipo2 + @"\" + paciente.LastName.ToUpper() + ", " + paciente.FirstName + " " + IdCorregida;
                }
                else if (vieneDeEq1oEq4)
                {
                    if (equipoDestino=="2100CMLC")
                    {
                        pathPaciente = Properties.Settings.Default.PathDCMEquipo + @"\" + paciente.LastName.ToUpper() + ", " + paciente.FirstName + " " + IdCorregida + " (" + equipoOrigen + ")";
                    }
                    else if (equipoDestino == "Equipo 2 6EX")
                    {
                        pathPaciente = Properties.Settings.Default.PathDCMEquipo2 + @"\" + paciente.LastName.ToUpper() + ", " + paciente.FirstName + " " + IdCorregida + " (" + equipoOrigen + ")";
                    }
                }
                IO.crearCarpeta(pathPaciente);
                if (esPlanSuma)
                {
                    try
                    {
                        string pathPlan = pathPaciente + @"\" + plan.Id;
                        IO.crearCarpeta(pathPlan);
                        IO.crearCarpeta(pathPlan + @"\BACKUP");
                        IO.moverArchivo(path, pathPlan + @"\" + plan.Id + ".dcm");
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show("No se puede acceder a la carpeta\n" + e.ToString());
                    }
                    
                    
                }
                else
                {
                    try
                    {
                        IO.crearCarpeta(pathPaciente + @"\BACKUP");
                        IO.moverArchivo(path, pathPaciente + @"\" + plan.Id + ".dcm");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("No se puede acceder a la carpeta\n" + e.ToString());
                    }
                }
                


                return true;
            }
            else
            {
                return false;
            }
        }


    }

    public struct campoDCM
    {
        public string ID { get; set; }
        //public double gantry { get; set; }
        //public double colimador { get; set; }
        //public double camilla { get; set; }
        public int UM { get; set; }
    }


}
