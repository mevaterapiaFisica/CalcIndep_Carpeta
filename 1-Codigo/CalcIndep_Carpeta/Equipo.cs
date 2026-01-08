using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcIndep_Carpeta
{
    public class Equipo
    {
        public static string pathEquipos = @"\\ARIAMEVADB-SVR\va_data$\Calculo Independiente\Equipos.csv";

        public string Nombre { get; set; }
        public string SN { get; set; }
        public string DicomID { get; set; }
        public string Centro { get; set; }
        public string PrefijoSitra { get; set; }
        public string PathPacientes { get; set; }
        public string PathDRRs { get; set; }
        public bool EsNormaIEC { get; set; }
        public bool EsDicomRT { get; set; }
        public string PathDICOM { get; set; }

        public List<string> Energias { get; set; }

        public string Camilla {get;set;}
        public string HojaRefToIso {get;set;}
        public bool DeltaCouch { get; set; }
        public bool Imagenes {get;set;}




    public override string ToString()
        {
            return Nombre + "," + SN +"," + DicomID + "," + Centro + "," + PrefijoSitra + "," + PathPacientes + "," + PathDRRs + "," + EsNormaIEC.ToString() + "," + EsDicomRT.ToString() + "," + PathDICOM + "," + String.Join("-", this.Energias) + 
                "," + Camilla + "," + HojaRefToIso + "," + DeltaCouch.ToString() + "," + Imagenes.ToString();
        }

        public Equipo(string linea)
        {
            string[] aux = linea.Split(',');
            Nombre = aux[0];
            SN = aux[1];
            DicomID = aux[2];
            Centro = aux[3];
            PrefijoSitra = aux[4];
            PathPacientes = aux[5];
            PathDRRs = aux[6];
            EsNormaIEC = aux[7].ToLower() == "true";
            EsDicomRT = aux[8].ToLower() == "true";
            PathDICOM = aux[9];
            string[] aux_Energias = aux[10].Split('-');
            Energias = new List<string>();
            foreach (string s in aux_Energias)
            {
                Energias.Add(s);
            }
            Camilla = aux[10];
            HojaRefToIso = aux[11];
            DeltaCouch = aux[12].ToLower() == "true";
            Imagenes = aux[13].ToLower() == "true";
        }

        public static List<Equipo> LeerArchivo()
        {
            List<Equipo> list = new List<Equipo>();
            string[] archivo = File.ReadAllLines(pathEquipos);
            foreach (string linea in archivo.Skip(1))
            {
                list.Add(new Equipo(linea));
            }
            return list;
        }

        public static void EscribirArchivo(List<Equipo> equipos)
        {
            List<string> salida = new List<string>();
            salida.Add("Nombre,SN,DicomID,Centro,PrefijoSitra,PathPaciente,PathDRRs,EsNormaIEC,EsDicomRT,PathDICOM,Energias;Camilla;Hoja RefToIso;DeltaCouch;Imagenes");
            foreach (Equipo equipo in equipos)
            {
                salida.Add(equipo.ToString());
            }
            File.WriteAllLines(pathEquipos, salida.ToArray());
        }

        public static Equipo seleccionarEquipo(string DicomRT)
        {
            List<Equipo> equipos = LeerArchivo();
            return equipos.First(s => s.DicomID == DicomRT);
        }


    }

    /*public class Haz
    {
        public static string pathEnergias = @"\\ARIAMEVADB-SVR\va_data$\Calculo Independiente\Energias.csv";
        public int ID_Equipo { get; set; }
        public string Modalidad { get; set; }
        public int Energia { get; set; }
        public double Dmax { get; set; }
        public double tr_MLC { get; set; }
        public double TPR_20_10 { get; set; }
        public double Dose { get; set; }

        public override string ToString()
        {
            return ID_Equipo.ToString() + ";" + Modalidad + ";" + Energia.ToString() + ";" + Dmax.ToString() + ";" + tr_MLC.ToString() + ";" + TPR_20_10.ToString() + ";" + Dose.ToString();
        }

        public void EscribirArchivo(List<Equipo> equipos)
        {
            List<string> salida = new List<string>();
            salida.Add("ID_Equipo;Modalidad;Energia;Dmax;tr_MLC;TPR_20_10");
            foreach (Equipo equipo in equipos)
            {
                foreach (Haz haz in equipo.Haces)
                {
                    salida.Add(haz.ToString());
                }
            }
            File.WriteAllLines(pathEnergias, salida.ToArray());
        }
        public haz(string linea)
        {
            string[] aux = linea.Split(',');
            SN = aux[0];
            DicomID = aux[1];
            Dmax = aux[2];
            tr_MLC = aux[3];
            TPR_20_10 = aux[4];
            Dose = aux[5];
            Energia = aux[6];
        }
    }
    */




}



