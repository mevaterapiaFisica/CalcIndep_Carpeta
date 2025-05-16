using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model;

namespace CalcIndep_Carpeta
{
    public static class crearPPF
    {
        public static List<string> escribirHeader(Patient paciente, PlanSetup plan)
        {
            int numCampos = 0;
            foreach (Beam campo in plan.Beams)
            {
                if (!campo.IsSetupField)
                {
                    numCampos++;
                }
            }
            string medico = "";
            if (plan.ApprovalStatus == PlanSetupApprovalStatus.PlanningApproved)
            {
                medico = plan.PlanningApprover;
            }
            List<string> header = new List<string>();
            header = File.ReadAllLines(Properties.Settings.Default.PathPrograma + @"\" + "PPFheader.txt").ToList();
            header[5] += numCampos.ToString();
            header[6] += Math.Round(plan.UniqueFractionation.PrescribedDosePerFraction.Dose, 0).ToString();
            header[7] += "100.0";
            header[8] += plan.UniqueFractionation.NumberOfFractions.ToString();
            header[9] += Math.Round(plan.PlanNormalizationValue,1).ToString();
            //header[9] += ""; //completar
            //header[10] += ""; //completar
            header[12] += paciente.LastName + "^" + paciente.FirstName;
            header[13] += paciente.Id;
            header[15] += DateTime.Today.ToString("ddMMyyyy");
            header[16] += medico;
            header[19] += equipo(plan.Beams.Where(b=>!b.IsSetupField).First());
            return header;
        }

        public static List<string> escribirCampo(Beam campo, PlanSetup plan, List<string> campoPPF)
        {
            double sumaPesos = 0;
            foreach (Beam c in plan.Beams)
            {
                sumaPesos += c.WeightFactor;
            }
            double factorPesos = plan.UniqueFractionation.PrescribedDosePerFraction.Dose / (100 * sumaPesos);
            double y2 = Math.Round((campo.ControlPoints.First().JawPositions.Y2), 0);
            double y1 = Math.Round((-campo.ControlPoints.First().JawPositions.Y1), 0);
            double x2 = Math.Round((campo.ControlPoints.First().JawPositions.X2), 0);
            double x1 = Math.Round((-campo.ControlPoints.First().JawPositions.X1), 0);
            double tamX = Math.Round(x2 + x1, 0);
            double tamY = Math.Round(y2 + y1, 0);
            string haz = "";
            if (campo.EnergyModeDisplayName == "6X" || campo.EnergyModeDisplayName == "10X" || campo.EnergyModeDisplayName == "6X-SRS")    //FALTA SRS!!!!!!!!!!!!!!!!!!
            {
                haz = "PHOTON";
            }
            else
            {
                haz = "ELECTRON";
            }
            if (y2 == y1) //simetrico en Y
            {
                y1 = 0;
                y2 = 0;
            }

            if (x2 == x1) //simetrico en X
            {
                x1 = 0;
                x2 = 0;
            }
            //campo.
            string gantry = IECaVarian(campo.ControlPoints[0].GantryAngle,EquipoEsIEC(campo)).ToString();
            if (campo.ControlPoints.Last().GantryAngle != campo.ControlPoints.First().GantryAngle)
            {
                gantry += "->" + IECaVarian(campo.ControlPoints.Last().GantryAngle, EquipoEsIEC(campo)).ToString();
            }
            string cuna = "NONE";
            string orientacionCuna = "----";
            if (campo.Wedges.Count() > 0)
            {
                cuna = campo.Wedges.First().Id.Substring(0, 5);
                orientacionCuna = campo.Wedges.First().Id.Substring(5);
            }
            campoPPF.Add("[FIELD_" + campo.BeamNumber.ToString() + "]");
            campoPPF.Add(campo.Id);
            campoPPF.Add("ISO" + (posicionIsoEnLista(campo.IsocenterPosition, plan) + 1).ToString());
            campoPPF.Add(tamY.ToString());
            campoPPF.Add(tamX.ToString());
            campoPPF.Add(IECaVarian(campo.ControlPoints[0].PatientSupportAngle, EquipoEsIEC(campo)).ToString());
            campoPPF.Add(gantry);
            campoPPF.Add("0");
            campoPPF.Add(IECaVarian(campo.ControlPoints[0].CollimatorAngle, EquipoEsIEC(campo)).ToString());
            //campoPPF.Add()
            campoPPF.Add(Math.Round((campo.WeightFactor * factorPesos), 5).ToString());
            campoPPF.Add(cuna);
            campoPPF.Add(orientacionCuna);
            campoPPF.Add("Si");// falta ver si tiene MLC campoPPF.Add(campo.ControlPoints.First().LeafPositions)
            campoPPF.Add("1");
            campoPPF.Add(y2.ToString());
            campoPPF.Add(y1.ToString());
            campoPPF.Add(x1.ToString());
            campoPPF.Add(x2.ToString());
            campoPPF.Add(Math.Round(campo.SSD, 0).ToString());
            campoPPF.Add("0");
            campoPPF.Add(haz);
            campoPPF.Add(Math.Round(campo.Meterset.Value, 4).ToString());

            return campoPPF;
        }

        public static void escribirPPFcompleto(Patient paciente, PlanSetup plan,List<Beam> camposOrdenados)
        {
            string nombreMasID = paciente.LastName.ToUpper() + ", " + paciente.FirstName.ToUpper() + "-" + paciente.Id;
            string pathDirectorio = IO.crearCarpetaPaciente(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, plan).Id, plan.Id, plan.Beams.First().TreatmentUnit.Id);
            List<string> ppf = new List<string>();
            ppf = escribirHeader(paciente, plan);
            foreach (Beam campo in camposOrdenados)
            {
                ppf = escribirCampo(campo, plan, ppf);
            }
            ppf.Add("[Footer]");
            ppf.Add("");
            ppf.Add("");
            ppf.Add("Density correction ON");
            File.WriteAllLines(pathDirectorio + @"\" + nombreMasID + ".PPF", ppf);
        }

        public static List<VVector> listaIsos(PlanSetup plan)
        {
            List<VVector> isos = new List<VVector>();
            foreach (Beam campo in plan.Beams)
            {
                bool agregar = true;
                VVector isoToRef = restaVectores(campo.IsocenterPosition, plan.StructureSet.Image.UserOrigin);
                foreach (VVector iso in isos)
                {

                    if (distanciaMaxima(isoToRef, iso) < 0.0001) //son el mismo punto
                    {
                        agregar = false;
                        break;
                    }
                }
                if (agregar)
                {
                    isos.Add(isoToRef);
                }
            }
            return isos;
        }

        public static double distanciaMaxima(VVector punto1, VVector punto2)
        {
            double difX = Math.Abs(punto1.x - punto2.x);
            double difY = Math.Abs(punto1.y - punto2.y);
            double difZ = Math.Abs(punto1.z - punto2.z);
            return Math.Max(difX, Math.Max(difY, difZ));
        }

        public static int posicionIsoEnLista(VVector iso, PlanSetup plan)
        {
            List<VVector> isosPlan = listaIsos(plan);
            VVector isoToRef = restaVectores(iso, plan.StructureSet.Image.UserOrigin);
            for (int i = 0; i < isosPlan.Count(); i++)
            {
                if (distanciaMaxima(isoToRef, isosPlan[i]) < 0.0001) //son el mismo punto
                {
                    return i;
                }
            }
            return -1;
        }

        public static double IECaVarian(double valorIEC, bool esIEC)
        {
            if (esIEC)
            {
                return valorIEC;
            }
            if (valorIEC <= 180)
            {
                return 180 - valorIEC;
            }
            else
            {
                return 540 - valorIEC;
            }
        }

        public static string equipo(Beam campo)
        {
            string equipoString = "";
            if (campo.TreatmentUnit.Id == "Equipo1")
            {
                equipoString += "MEVA_EQ1_Fotones_06MV";
            }
            else if (campo.TreatmentUnit.Id == "Equipo 2 6EX")
            {
                equipoString += "MEVA_EQ2_Fotones_06MV";
            }
            else if (campo.TreatmentUnit.Id == "Equipo3")
            {
                equipoString += "MEVA_EQ3_";
                if (campo.EnergyModeDisplayName == "6X")
                {
                    equipoString += "Fotones_06MV";
                }
                else if (campo.EnergyModeDisplayName == "10X")
                {
                    equipoString += "Fotones_10MV";
                }
                else if (campo.EnergyModeDisplayName == "6E")
                {
                    equipoString += "Electrones_06MeV";
                }
                else if (campo.EnergyModeDisplayName == "9E")
                {
                    equipoString += "Electrones_09MeV";
                }
                else if (campo.EnergyModeDisplayName == "12E")
                {
                    equipoString += "Electrones_12MeV";
                }
                /*else if (campo.EnergyModeDisplayName == "16E")
                {
                    equipoString += "Electrones_16MeV";
                }
                else if (campo.EnergyModeDisplayName == "20E")
                {
                    equipoString += "Electrones_20MeV";
                }*/

            }
            else if (campo.TreatmentUnit.Id == "D-2300CD")
            {
                equipoString += "MEVA_EQ4";
                {
                    if (campo.EnergyModeDisplayName == "6X" || campo.EnergyModeDisplayName == "6X-SRS")
                    {
                        equipoString += "-Trilogy_Fotones_06MV";
                    }
                    else if (campo.EnergyModeDisplayName == "10X")
                    {
                        equipoString += "-Trilogy_Fotones_10MV";
                    }
                    else if (campo.EnergyModeDisplayName == "6E")
                    {
                        equipoString += "_Electrones_06MeV";
                    }
                    else if (campo.EnergyModeDisplayName == "9E")
                    {
                        equipoString += "_Electrones_09MeV";
                    }
                    else if (campo.EnergyModeDisplayName == "12E")
                    {
                        equipoString += "_Electrones_12MeV";
                    }
                    else if (campo.EnergyModeDisplayName == "15E")
                    {
                        equipoString += "_Electrones_15MeV";
                    }
                    else if (campo.EnergyModeDisplayName == "18E")
                    {
                        equipoString += "_Electrones_18MeV";
                    }
                    else if (campo.EnergyModeDisplayName == "22E")
                    {
                        equipoString += "_Electrones_22MeV";
                    }
                }
            }
            else if (campo.TreatmentUnit.Id == "PBA_6EX_730")
            {
                equipoString += "CETRO_Fotones_06MV";
            }
            else if (campo.TreatmentUnit.Id == "CL21EX")
            {
                equipoString += "MEDRANO_Fotones_06MV";
            }
            else if (campo.TreatmentUnit.Id == "EQ2_iX_827")
            {
                equipoString += "QUILMES_EQ2";
                {
                    if (campo.EnergyModeDisplayName == "6X")
                    {
                        equipoString += "_Fotones_06MV";
                    }
                    else if (campo.EnergyModeDisplayName == "15X")
                    {
                        equipoString += "_Fotones_15MV";
                    }
                    else if (campo.EnergyModeDisplayName == "6E")
                    {
                        equipoString += "_Electrones_06MeV";
                    }
                    else if (campo.EnergyModeDisplayName == "9E")
                    {
                        equipoString += "_Electrones_09MeV";
                    }
                    else if (campo.EnergyModeDisplayName == "12E")
                    {
                        equipoString += "_Electrones_12MeV";
                    }                    
                }
            }
            else if (campo.TreatmentUnit.Id == "QBA_600CD_523")
            {
                equipoString += "QUILMES_Fotones_06MV";
            }
            return equipoString;

        }
        public static bool EquipoEsIEC(Beam campo)
        {
            return equipo(campo).Contains("MEDRANO") || equipo(campo).Contains("EQ3") || equipo(campo).Contains("QUILMES");
        }

        public static VVector restaVectores(VVector v1, VVector v2)
        {
            VVector resta = new VVector();
            resta.x = v1.x - v2.x;
            resta.y = v1.y - v2.y;
            resta.z = v1.z - v2.z;
            return resta;
        }
    }


}
