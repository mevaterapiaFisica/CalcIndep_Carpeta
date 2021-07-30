using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;

namespace CalcIndep_Carpeta
{
    public static class Graficar
    {
        public static ChartArea Area(double dosisMaxima, double prescripcion)
        {
            ChartArea area = new ChartArea();
            area.AxisX.Title = "Dosis [cGy]";
            area.AxisX.TitleFont = new System.Drawing.Font("Arial", 15);
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = dosisMaxima;
            area.AxisX.LineColor = System.Drawing.Color.Black;
            //area.AxisX.MajorGrid.Interval = dosisMaxima / 10;
            area.AxisX.Interval = prescripcion / 10;
            area.AxisY.Title = "Volumen [%]";
            area.AxisY.TitleFont = new System.Drawing.Font("Arial", 15);
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 100;
            area.AxisY.LineColor = System.Drawing.Color.Black;

            return area;
        }

        public static System.Windows.Forms.DataVisualization.Charting.Series Curva(System.Drawing.Color color, double[] volumen, double[] dosis)
        {
            //FontFamily fuente = new FontFamily("Segoe UI");

            System.Windows.Forms.DataVisualization.Charting.Series serie = new System.Windows.Forms.DataVisualization.Charting.Series();
            serie.Points.DataBindXY(dosis, volumen);
            serie.ChartType = SeriesChartType.Line;
            serie.Color = color;
            //serie.ChartType = SeriesChartType.FastPoint;
            //serie.MarkerColor = color;
            //serie.MarkerSize = 4;
            //serie.MarkerStyle = MarkerStyle.Circle;

            return serie;
        }


        //public static Chart grafico(PlanningItem plan, List<Structure> estructuras, double dosisPrescripta)
        public static Chart grafico(Patient paciente, PlanningItem plan, List<Structure> estructuras,double dosisTotal, bool tienePrintScreen)
        {
            Chart grafico = new Chart();
            double dosisMaxima = plan.Dose.DoseMax3D.Dose;
            double dosisPrescripta = dosisTotal;
            if (plan.Dose.DoseMax3D.Unit == DoseValue.DoseUnit.Percent)
            {
                dosisMaxima = dosisMaxima * dosisPrescripta/100;
            }
            
            grafico.ChartAreas.Add(Area(dosisMaxima,dosisPrescripta));
            foreach (Structure estructura in estructuras)
            {
                DVHData dvh = plan.GetDVHCumulativeData(estructura, DoseValuePresentation.Absolute, VolumePresentation.Relative, 1);
                if (dvh==null)
                {
                    MessageBox.Show("La estructura " + estructura.Id + " no tiene información de dosis.\nSe excluirá del DVH");
                }
                else
                {
                    double[] volumen = new double[dvh.CurveData.Length];
                    double[] dosis = new double[dvh.CurveData.Length];
                    for (int i = 0; i < dvh.CurveData.Length; i++)
                    {
                        volumen[i] = dvh.CurveData[i].Volume;
                        dosis[i] = dvh.CurveData[i].DoseValue.Dose;
                    }
                    System.Drawing.Color colorD = System.Drawing.Color.FromArgb(estructura.Color.A, estructura.Color.R, estructura.Color.G, estructura.Color.B);
                    grafico.Series.Add(Curva(colorD, volumen, dosis));
                }
                
            }
            grafico.Width = 900;
            grafico.Height = 450;
            if (tienePrintScreen)
            {
                grafico.SaveImage("DVH.png", ChartImageFormat.Png);
            }
            else
            {
                grafico.SaveImage(paciente.Id + "_DVH.png", ChartImageFormat.Png);
            }
            
            return grafico;
            
        }
    }
}
