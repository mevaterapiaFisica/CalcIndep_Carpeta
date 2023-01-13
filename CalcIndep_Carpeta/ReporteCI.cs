using System;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Forms;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;

namespace CalcIndep_Carpeta
{
    public static class ReporteCI
    {
        public struct CampoCI
        {
            public string ID { get; set; }
            public string energia { get; set; }
            public double UMeclipse { get; set; }
            public double UMindep { get; set; }
            [DisplayName("Sesgo Rel [%]")]
            public double sesgoRel { get; set; }
            [DisplayName("Sesgo Abs [cGy]")]
            public double sesgoAbs { get; set; }
            [Browsable(false)]
            public Bitmap imagen { get; set; }
        }

        public static Document informe(Patient paciente, PlanSetup plan, string usuario, List<CampoCI> camposCI)
        {
            Document informe = new Document();
            Estilos.definirEstilos(informe);
            Section seccion = new Section();
            Estilos.formatearSeccion(seccion);
            seccion.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            crearEncabezado(seccion, paciente, plan, usuario);
            for (int k = 0; k < Math.Ceiling((double)camposCI.Count / 8); k++)
            {
                seccion.Add(tablaCampos(camposCI));
            }
            informe.Add(seccion);
            return informe;
        }

        public static void vistaPrevia(Patient paciente, PlanSetup plan, string usuario, List<CampoCI> camposCI)
        {
            Document document = informe(paciente, plan, usuario, camposCI);
            VistaPrevia vistaPrevia = new VistaPrevia(document,paciente,plan,usuario,camposCI);
            vistaPrevia.ShowDialog();
        }
        public static void exportarAPdf(Document report, Patient paciente, PlanSetup plan, string usuario, List<CampoCI> camposCI)
        {
            string nombreMasID = paciente.LastName.ToUpper() + ", " + paciente.FirstName.ToUpper() + "-" + paciente.Id;
            string pathDirectorio = IO.crearCarpetaPaciente(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, plan).Id, plan.Id);


            string path =  IO.GetUniqueFilename(pathDirectorio, @"\" + nombreMasID + "_CI", "pdf");
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = report;
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(path);
            MessageBox.Show("Se ha guaradado el reporte con el nombre: " + Path.GetFileName(path));
            crearTxt(paciente, plan, camposCI);

        }

        public static void crearTxt(Patient paciente, PlanSetup plan, List<CampoCI> camposCI)
        {
            List<string> output = new List<string>();
            foreach (CampoCI campoCI in camposCI)
            {
                output.Add(campoCI.ID + ";" + campoCI.sesgoRel + ";" + campoCI.sesgoAbs);
            }
            string nombreMasID = paciente.LastName.ToUpper() + ", " + paciente.FirstName.ToUpper() + "-" + paciente.Id;
            string pathDirectorio = IO.crearCarpetaPaciente(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, plan).Id, plan.Id);


            string path = pathDirectorio + @"\" + nombreMasID + "_CI.txt";
            File.WriteAllLines(path, output);
        }

        public static void imprimir(Document report, Patient paciente, PlanSetup plan, string usuario, List<CampoCI> camposCI)
        {
            
            MigraDoc.Rendering.Printing.MigraDocPrintDocument pd = new MigraDoc.Rendering.Printing.MigraDocPrintDocument();
            var rendered = new DocumentRenderer(report);
            rendered.PrepareDocument();
            pd.Renderer = rendered;
            PrintDialog printDialog1 = new PrintDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                pd.PrinterSettings = printDialog1.PrinterSettings;
                pd.Print();
            }
            exportarAPdf(report, paciente, plan, usuario, camposCI);
        }


        public static void crearEncabezado (Section seccion, Patient paciente, PlanSetup plan, string usuario)
        {
            seccion.AddParagraph("Cálculo independiente", "Titulo");
            seccion.AddParagraph("");
            MigraDoc.DocumentObjectModel.Tables.Table tabla = new MigraDoc.DocumentObjectModel.Tables.Table();
            tabla.AddColumn(270);
            tabla.Borders.Visible = false;
            for (int i = 0; i < 6; i++)
            {
                tabla.AddRow();
            }
            tabla.Rows[0].Cells[0].Add(Estilos.etiquetaYValor("Paciente", paciente.LastName + ", " + paciente.FirstName));
            tabla.Rows[1].Cells[0].Add(Estilos.etiquetaYValor("HC", paciente.Id));
            tabla.Rows[2].Cells[0].Add(Estilos.etiquetaYValor("Equipo", equipo(plan.Beams.First())));
            tabla.Rows[3].Cells[0].Add(Estilos.etiquetaYValor("Realizado por", usuario));
            tabla.Rows[4].Cells[0].Add(Estilos.etiquetaYValor("Fecha", DateTime.Today.ToShortDateString()));
            tabla.Rows[5].Cells[0].Add(Estilos.etiquetaYValor("Nombre del Plan", plan.Id));
            seccion.Add(tabla);
            seccion.AddParagraph("");
        }

        public static MigraDoc.DocumentObjectModel.Tables.Table tablaCampos(List<CampoCI> camposCI)
        {
            double tol_porciento = 4; // 4 % de tolerancia en dosis por campo
            double tol_dosis = 2;     //2 cGy de tolerancia en dosis por campo

                MigraDoc.DocumentObjectModel.Tables.Table tabla = new MigraDoc.DocumentObjectModel.Tables.Table();
                for (int i = 0; i < camposCI.Count + 1; i++)
                {
                    tabla.AddColumn();
                    tabla.Columns[i].Format.Alignment = ParagraphAlignment.Center;
                }

                for (int j = 0; j < 7; j++)
                {
                    tabla.AddRow();
                }
                tabla.Rows[0].Cells[0].AddParagraph("Campo");
                tabla.Rows[1].Cells[0].AddParagraph("Energía");
                tabla.Rows[2].Cells[0].AddParagraph("UM eclipse");
                tabla.Rows[3].Cells[0].AddParagraph("UM CI");
                tabla.Rows[4].Cells[0].AddParagraph("Sesgo [%]");
                tabla.Rows[5].Cells[0].AddParagraph("Sesgo [cGy]");

                for (int i = 0; i < camposCI.Count; i++)
                {
                    CampoCI campo = camposCI[i];
                    tabla.Rows[0].Cells[i + 1].AddParagraph(campo.ID);
                    tabla.Rows[1].Cells[i + 1].AddParagraph(campo.energia);
                    tabla.Rows[2].Cells[i + 1].AddParagraph(Math.Round(campo.UMeclipse, 1).ToString());
                    tabla.Rows[3].Cells[i + 1].AddParagraph(Math.Round(campo.UMindep, 1).ToString());
                    tabla.Rows[4].Cells[i + 1].AddParagraph(Math.Round(campo.sesgoRel, 1).ToString());
                    tabla.Rows[5].Cells[i + 1].AddParagraph(Math.Round(campo.sesgoAbs, 1).ToString());

                    if (Math.Abs(campo.sesgoRel) <= tol_porciento)
                    {
                        tabla.Rows[4].Cells[i + 1].Format.Font.Color = Colors.Green;
                        tabla.Rows[5].Cells[i + 1].Format.Font.Color = Colors.Green;

                    }
                    else if (Math.Abs(campo.sesgoRel) > tol_porciento && Math.Abs(campo.sesgoAbs) <= tol_dosis)
                    {
                        tabla.Rows[4].Cells[i + 1].Format.Font.Color = Colors.Yellow;
                        tabla.Rows[5].Cells[i + 1].Format.Font.Color = Colors.Yellow;
                    }
                    else
                    {
                        tabla.Rows[4].Cells[i + 1].Format.Font.Color = Colors.Red;
                        tabla.Rows[5].Cells[i + 1].Format.Font.Color = Colors.Red;
                    }
                    campo.imagen.Save(campo.ID + ".png", ImageFormat.Png);
                    MigraDoc.DocumentObjectModel.Shapes.Image imagen = tabla.Rows[6].Cells[i + 1].AddImage(campo.ID + ".png");
                    imagen.Width = 70;
                    //imagen.Height = 40;

                }
                tabla.Style = "Tabla";
                tabla.Columns[0].Format.Font.Bold = true;
                tabla.Columns[0].Format.Alignment = ParagraphAlignment.Left;
                tabla.Columns.Width = new Unit(2.5, UnitType.Centimeter);
                //tabla.Columns[0].Format.Shading.Color = new MigraDoc.DocumentObjectModel.Color(238, 236, 225);
                tabla.Borders.Visible = true;
                tabla.TopPadding = 4;
                tabla.BottomPadding = 4;
                tabla.Format.Font.Size = 10;
                //tabla.Rows[6].Height = 40;
                tabla.Columns.Width = 80;
                return tabla;
            }
        public static string equipo(Beam campo)
        {
            string equipoString = "";
            if (campo.TreatmentUnit.Id == "Equipo1")
            {
                equipoString += "Equipo 1";
            }
            else if (campo.TreatmentUnit.Id == "2100CMLC")
            {
                equipoString = "Equipo 3";
            }
            else if (campo.TreatmentUnit.Id == "Equipo 2 6EX")
            {
                equipoString += "Equipo 2";
            }
            else if (campo.TreatmentUnit.Id == "D-2300CD")
            {
                equipoString = "Equipo 4";
            }
            else if (campo.TreatmentUnit.Id == "PBA_6EX_730")
            {
                equipoString += "CETRO_Fotones_06MV";
            }
            return equipoString;
        }
    }



}
