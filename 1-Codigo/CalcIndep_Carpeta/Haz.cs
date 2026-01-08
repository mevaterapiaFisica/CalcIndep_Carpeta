using System;
using VMS.TPS.Common.Model.API;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcIndep_Carpeta
{
    public class Haz
    {
        public static string pathEnergias = @"\\ARIAMEVADB-SVR\va_data$\Calculo Independiente\Haces.csv";
        public string SN { get; set; }
        public string DicomID { get; set; }
        public string Etiqueta { get; set; }
        public double Dmax { get; set; }
        public double tr_MLC { get; set; }
        public double TPR_20_10 { get; set; }
        public double Dose { get; set; }
        public double Energia { get; set; }

        public Haz(string linea)
        {
            string[] aux = linea.Split(',');
            SN = aux[0];
            DicomID = aux[1];
            Etiqueta = aux[2];
            Dmax = Convert.ToDouble(aux[3]);
            tr_MLC = Convert.ToDouble(aux[4]);
            TPR_20_10 = Convert.ToDouble(aux[5]);
            Dose = Convert.ToDouble(aux[6]);
            Energia = Convert.ToDouble(aux[7]);
        }
        public static List<Haz> LeerArchivo()
        {
            List<Haz> list = new List<Haz>();
            string[] archivo = File.ReadAllLines(pathEnergias);
            foreach (string linea in archivo.Skip(1))
            {
                list.Add(new Haz(linea));
            }
            return list;
        }

        public static Haz seleccionarHaz(Beam campo)
        {
            List<Haz> haces = LeerArchivo();
            if (haces.Any(h => h.DicomID == campo.TreatmentUnit.Id && h.Etiqueta == campo.EnergyModeDisplayName))
            {
                return haces.First(h => h.DicomID == campo.TreatmentUnit.Id && h.Etiqueta == campo.EnergyModeDisplayName);
            }
            return null;
                
        }
    }
}
