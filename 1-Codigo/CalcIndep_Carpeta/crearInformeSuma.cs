using System;
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
    public static class crearInformeSuma
    {
        public static System.Drawing.Image nombrePaciente;
        public static System.Drawing.Image axial;
        public static System.Drawing.Image coronal;
        public static System.Drawing.Image sagital;
        public static Bitmap imagen;
        public static string textoTipoTratamiento = "";
        public static double dosisTotal = 0;
        public static int? numeroFracciones = 0;

        public static void Informe(Patient paciente, PlanningItem plan, List<Structure> estructuras, bool _hayContext)
        {
            int imagenesPaciente = obtenerYContarImagenes(paciente);
            if (imagenesPaciente == 3)
            {
                exportarAPdf(paciente, plan, estructuras);
            }
            else if (imagenesPaciente > 0)
            {
                MessageBox.Show("Se encontraron " + imagenesPaciente.ToString() + " imágenes del paciente (se esperaban 3).\nPor favor repetir el procedimiento desde el inicio");
                foreach (string imagen in Directory.GetFiles(Properties.Settings.Default.PathPrograma + @"\Imagenes").Where(f => f.Contains(paciente.Id)))
                {
                    File.Delete(imagen);
                }
            }
            else if (Clipboard.ContainsImage())
            {
                MessageBox.Show("No se encuentran las capturas.\nSe procederá con la impresión de pantalla");
                imagen = new Bitmap(Clipboard.GetImage());
                exportarAPdf(paciente, plan, imagen, estructuras);
            }
            else
            {
                MessageBox.Show("No se encuentran las capturas ni la impresión de pantalla");
            }
        }

        public static bool coloresIguales(System.Drawing.Color color1, System.Drawing.Color color2)
        {
            if (color1.R == color2.R && color1.G == color2.G && color1.B == color2.B)
            {
                return true;
            }
            return false;
        }

        public static bool casiNegro(System.Drawing.Color color)
        {
            if (color.R < 10 && color.G < 10 && color.B < 10)
            {
                return true;
            }
            return false;
        }

        public static bool casiBlanco(System.Drawing.Color color)
        {
            if (color.R > 252 && color.G > 252 && color.B > 252)
            {
                return true;
            }
            return false;
        }

        public static bool casiNegroROI(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            //int tamanoROI = 10;
            for (int i = 0; i < tamanoROI; i++)
            {
                for (int j = 0; j < tamanoROI; j++)
                {
                    if (!casiNegro(imagen.GetPixel(x + i, y + j)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool casiBlancoROI(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            //int tamanoROI = 10;
            for (int i = 0; i < tamanoROI; i++)
            {
                for (int j = 0; j < tamanoROI; j++)
                {
                    if (!casiBlanco(imagen.GetPixel(x + i, y + j)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool casiTodoBlanco(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {

            //int tamanoROI = 10;
            int blanco = 0;
            for (int i = 0; i < tamanoROI; i++)
            {
                for (int j = 0; j < tamanoROI; j++)
                {
                    if (casiBlanco(imagen.GetPixel(x + i, y + j)))
                    {
                        blanco++;
                    }
                }
            }
            if (blanco > tamanoROI * (tamanoROI - 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool casiNadaBlanco(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {

            //int tamanoROI = 10;
            int noBlanco = 0;
            for (int i = 0; i < tamanoROI; i++)
            {
                for (int j = 0; j < tamanoROI; j++)
                {
                    if (!casiBlanco(imagen.GetPixel(x + i, y + j)))
                    {
                        noBlanco++;
                    }
                }
            }
            if (noBlanco > tamanoROI * tamanoROI / 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool pocoNegro(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {

            //int tamanoROI = 10;
            int noNegro = 0;
            for (int i = 0; i < tamanoROI; i++)
            {
                for (int j = 0; j < tamanoROI; j++)
                {
                    if (!casiNegro(imagen.GetPixel(x + i, y + j)))
                    {
                        noNegro++;
                    }
                }
            }
            if (noNegro > 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool nadaBlanco(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {

            //int tamanoROI = 10;
            int noBlanco = 0;
            for (int i = 0; i < tamanoROI; i++)
            {
                for (int j = 0; j < tamanoROI; j++)
                {
                    if (!casiBlanco(imagen.GetPixel(x + i, y + j)))
                    {
                        noBlanco++;
                    }
                }
            }
            if (noBlanco > tamanoROI * (tamanoROI - 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool blancoNegroHorizontal(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            if (casiBlancoROI(imagen, x, y, tamanoROI) && casiNegroROI(imagen, x + 10, y, tamanoROI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool negroBlancoHorizontal(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            if (casiNegroROI(imagen, x, y, tamanoROI) && casiTodoBlanco(imagen, x + 10, y, tamanoROI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool negroBlancoVertical(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            if (nadaBlanco(imagen, x, y, tamanoROI) && casiBlancoROI(imagen, x, y + tamanoROI, tamanoROI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool negroNoNegroVertical(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            if (nadaBlanco(imagen, x, y, tamanoROI) && pocoNegro(imagen, x, y + tamanoROI, tamanoROI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ahoraBlancoVertical(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            if (casiNadaBlanco(imagen, x, y, tamanoROI) && casiBlancoROI(imagen, x, y + tamanoROI, tamanoROI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool yaNoBlancoVertical(Bitmap imagen, int x, int y, int tamanoROI = 10)
        {
            if (casiBlancoROI(imagen, x, y, tamanoROI) && casiNadaBlanco(imagen, x, y + tamanoROI, tamanoROI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Imagen1
        public static Tuple<int, int> hallarPrimerNegroArriba(Bitmap imagen)
        {
            for (int j = 0; j < imagen.Height - 15; j++)
            {
                for (int i = 0; i < imagen.Width - 15; i++)
                {
                    if (casiNegroROI(imagen, i, j))
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }
            return new Tuple<int, int>(0, 0);
        }

        public static Tuple<int, int> hallarPrimerBlanco1(Bitmap imagen, Tuple<int, int> primerNegroArriba)
        {
            for (int j = primerNegroArriba.Item2; j < imagen.Height / 3; j++)
            {
                for (int i = primerNegroArriba.Item1 + 1; i < imagen.Width / 3; i++)
                {
                    if (negroBlancoHorizontal(imagen, i, j))
                    {
                        return new Tuple<int, int>(i + 10, j);
                    }
                }
            }
            return new Tuple<int, int>(0, 0);
        }

        public static Tuple<int, int> hallarUltimoBlanco1(Bitmap imagen, Tuple<int, int> primerBlanco1)
        {
            for (int i = primerBlanco1.Item1; i < imagen.Width - 15; i++)
            {
                if (blancoNegroHorizontal(imagen, i, primerBlanco1.Item2))
                {
                    return new Tuple<int, int>(i + 10, primerBlanco1.Item2);
                }
            }
            return new Tuple<int, int>(0, 0);
        }

        public static Tuple<int, int> hallarPrimerBlanco1Abajo(Tuple<int, int> primerBlanco1)
        {
            int x = primerBlanco1.Item1;
            int yInicial = primerBlanco1.Item2;
            for (int j = yInicial; j < imagen.Height - 15; j++)
            {
                if (yaNoBlancoVertical(imagen, x, j, 7))
                {
                    return new Tuple<int, int>(x, j);
                }
            }
            return new Tuple<int, int>(0, 0);

        }

        public static int anchoImagen1(Tuple<int, int> primerBlanco1, Tuple<int, int> ultimoBlanco1)
        {
            return ultimoBlanco1.Item1 - primerBlanco1.Item1;
        }

        public static int altoImagen1(Tuple<int, int> primerBlanco1, Tuple<int, int> primerBlanco1Abajo)
        {
            return primerBlanco1Abajo.Item2 - primerBlanco1.Item2;
        }

        #endregion
        #region Imagen2

        public static Tuple<int, int> hallarPrimerNegroAbajo(Bitmap imagen, Tuple<int, int> primerNegroArriba)
        {
            int yInicial = primerNegroArriba.Item2 + imagen.Height / 2;
            int x = primerNegroArriba.Item1;
            for (int j = yInicial; j > 0; j--)
            {
                if (!casiNegroROI(imagen, x, j) && !casiNegroROI(imagen, x, j - 10) && !casiNegroROI(imagen, x, j - 20) && !casiNegroROI(imagen, x, j - 30) && !casiNegroROI(imagen, x, j - 40))
                {
                    return new Tuple<int, int>(x, j);
                }

            }
            return new Tuple<int, int>(0, 0);
        }

        public static Tuple<int, int> hallarPrimerBlanco2(Bitmap imagen, Tuple<int, int> primerNegroAbajo, Tuple<int, int> primerBlanco1, int anchoImagen)
        {
            int x = primerBlanco1.Item1 + anchoImagen * 1 / 3 - 20;
            for (int j = primerNegroAbajo.Item2; j < imagen.Height - 15; j++)
            {
                if (negroNoNegroVertical(imagen, x, j, 5))
                {
                    return new Tuple<int, int>(primerBlanco1.Item1, j + 5);
                }
            }


            return new Tuple<int, int>(0, 0);
        }

        public static Tuple<int, int> hallarUltimoBlanco2(Bitmap imagen, Tuple<int, int> primerBlanco2)
        {
            int x = primerBlanco2.Item1;
            int yInicial = primerBlanco2.Item2;
            for (int j = yInicial; j < imagen.Height - 15; j++)
            {
                if (yaNoBlancoVertical(imagen, x, j, 7))
                {
                    return new Tuple<int, int>(x, j);
                }
            }
            return new Tuple<int, int>(0, 0);
        }

        #endregion

        #region Imagen3

        public static Tuple<int, int> hallarPrimerBlanco3(Bitmap imagen, Tuple<int, int> primerNegroAbajo, Tuple<int, int> primerBlanco2, int anchoImagen)
        {
            int y = primerBlanco2.Item2 + 1;
            int xInicial = primerBlanco2.Item1 + anchoImagen;
            for (int i = xInicial; i < imagen.Width - 15; i++)
            {
                if (casiTodoBlanco(imagen, i, y))
                {
                    return new Tuple<int, int>(i + 10, y);
                }
            }
            return new Tuple<int, int>(0, 0);
        }


        #endregion
        public static System.Drawing.Image cropImage(System.Drawing.Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (System.Drawing.Image)(bmpCrop);
        }

        public static bool obtenerImagenes(Patient paciente, Bitmap imagen)
        {
            Tuple<int, int> p0 = hallarPrimerNegroArriba(imagen);
            if (esCero(p0))
            {
                return false;
            }
            Tuple<int, int> p1 = hallarPrimerBlanco1(imagen, p0);
            if (esCero(p1))
            {
                return false;
            }
            Tuple<int, int> p2 = hallarUltimoBlanco1(imagen, p1);
            if (esCero(p2))
            {
                return false;
            }
            Tuple<int, int> p3 = hallarPrimerNegroAbajo(imagen, p0);
            if (esCero(p3))
            {
                return false;
            }
            Tuple<int, int> p4 = hallarPrimerBlanco1Abajo(p1);
            if (esCero(p4))
            {
                return false;
            }
            int altoImagenAxial = altoImagen1(p1, p4);
            if (altoImagenAxial == 0)
            {
                return false;
            }
            int anchoImagen = anchoImagen1(p1, p2);
            if (anchoImagen == 0)
            {
                return false;
            }
            Tuple<int, int> p5 = hallarPrimerBlanco2(imagen, p3, p1, anchoImagen);
            if (esCero(p5))
            {
                return false;
            }
            Tuple<int, int> p6 = hallarPrimerBlanco3(imagen, p3, p5, anchoImagen);
            if (esCero(p6))
            {
                return false;
            }
            Tuple<int, int> p7 = hallarUltimoBlanco2(imagen, p5);
            if (esCero(p7))
            {
                return false;
            }
            int altoImagenCoroSagital = altoImagen1(p5, p7);
            if (altoImagenCoroSagital == 0)
            {
                return false;
            }
            nombrePaciente = cropImage(imagen, new Rectangle(0, 0, 450, 20));
            axial = cropImage(imagen, new Rectangle(p1.Item1, p1.Item2, anchoImagen, altoImagenAxial));
            coronal = cropImage(imagen, new Rectangle(p5.Item1, p5.Item2, anchoImagen, altoImagenCoroSagital));
            sagital = cropImage(imagen, new Rectangle(p6.Item1, p6.Item2, anchoImagen, altoImagenCoroSagital));

            ChequeoImagenes chequeoImagenes = new ChequeoImagenes(axial, sagital, coronal, nombrePaciente);
            chequeoImagenes.ShowDialog();
            if (chequeoImagenes.imagenesOK)
            {
                axial.Save("axial" + ".png", System.Drawing.Imaging.ImageFormat.Png);
                coronal.Save("coronal" + ".png", System.Drawing.Imaging.ImageFormat.Png);
                sagital.Save("sagital" + ".png", System.Drawing.Imaging.ImageFormat.Png);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int obtenerYContarImagenes(Patient paciente)
        {
            List<string> imagenes = Directory.GetFiles(Properties.Settings.Default.PathPrograma + @"\Imagenes").Where(f => f.Contains(paciente.Id)).ToList();
            if (imagenes.Count == 3)
            {
                File.Move(imagenes[0], Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_axial.png");
                File.Move(imagenes[1], Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_sagital.png");
                File.Move(imagenes[2], Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_coronal.png");
            }
            return imagenes.Count;
        }

        public static List<string> obtenerYListarImagenes(Patient paciente)
        {
            return Directory.GetFiles(Properties.Settings.Default.PathPrograma + @"\Imagenes").Where(f => f.Contains(paciente.Id)).ToList();
        }

        public static bool esCero(Tuple<int, int> par)
        {
            if (par.Item1 != 0 || par.Item2 != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region textos

        public static Paragraph PrimerParrafo(Patient paciente, PlanningItem plan)
        {

            if (plan is PlanSetup)
            {
                Beam campo = ((PlanSetup)plan).Beams.First();
                dosisTotal = ((PlanSetup)plan).TotalPrescribedDose.Dose;
                numeroFracciones = ((PlanSetup)plan).UniqueFractionation.NumberOfFractions;
                if (campo.MLCPlanType == MLCPlanType.DoseDynamic && campo.ControlPoints.Count() > 30)
                {
                    textoTipoTratamiento = "de intensidad modulada (IMRT)";
                }

                else if (campo.MLCPlanType == MLCPlanType.VMAT)
                {
                    textoTipoTratamiento = "de intensidad modulada (IMRT) en modalidad VMAT";
                }
                else
                {
                    textoTipoTratamiento = "tridimensional conformado (3DC)";
                }
                DatosPlanSuma datosPlanSuma = new DatosPlanSuma(textoTipoTratamiento, dosisTotal, numeroFracciones);
                datosPlanSuma.ShowDialog();
                dosisTotal = datosPlanSuma.dosisTotal;
                numeroFracciones = datosPlanSuma.numeroFracciones;
                textoTipoTratamiento = datosPlanSuma.tipoTratamiento;
            }
            else
            {
                foreach (PlanSetup planSetup in ((PlanSum)plan).PlanSetups)
                {
                    dosisTotal += planSetup.TotalPrescribedDose.Dose;
                    numeroFracciones += planSetup.UniqueFractionation.NumberOfFractions;
                }
                Beam campo = ((PlanSum)plan).PlanSetups.First().Beams.First();
                if (campo.MLCPlanType == MLCPlanType.DoseDynamic && campo.ControlPoints.Count() > 30)
                {
                    textoTipoTratamiento = "de intensidad modulada (IMRT)";
                }

                else if (campo.MLCPlanType == MLCPlanType.VMAT)
                {
                    textoTipoTratamiento = "de intensidad modulada (IMRT) en modalidad VMAT";
                }
                else
                {
                    textoTipoTratamiento = "tridimensional conformado (3DC)";
                }

                DatosPlanSuma datosPlanSuma = new DatosPlanSuma(textoTipoTratamiento, dosisTotal, numeroFracciones);
                datosPlanSuma.ShowDialog();
                dosisTotal = datosPlanSuma.dosisTotal;
                numeroFracciones = datosPlanSuma.numeroFracciones;
                textoTipoTratamiento = datosPlanSuma.tipoTratamiento;
            }
            Paragraph primerParrafo = new Paragraph();
            primerParrafo.Style = "Texto";
            primerParrafo.AddText("El paciente ");
            primerParrafo.AddFormattedText(paciente.LastName + ", " + paciente.FirstName + " HC: " + paciente.Id, TextFormat.Bold);
            primerParrafo.AddText(" realizó tratamiento radiante " + textoTipoTratamiento + " con una dosis de prescripción de " + dosisTotal.ToString() + "cGy en " + numeroFracciones.ToString() + " fracciones. En el presente informe se adjunta corte axial en isocentro de la lesión y planificación del tratamiento (Figura 1). Las curvas representan niveles de dosis en Gy, indicados con diferentes colores.");
            return primerParrafo;
        }

        public static Paragraph SegundoParrafo()
        {
            Paragraph segundoParrafo = new Paragraph();
            segundoParrafo.Style = "Texto";
            segundoParrafo.AddText("En la Figura 2, se muestran las vistas coronal y sagital reconstruidas a partir de los cortes tomográficos axiales.");
            return segundoParrafo;
        }

        public static Paragraph TercerParrafo()
        {
            Paragraph tercerParrafo = new Paragraph();
            tercerParrafo.Style = "Texto";
            tercerParrafo.AddText("En la Figura 3 se presenta la distribución de dosis en los diferentes volúmenes de interés en forma de histograma dosis-volumen acumulado, con los valores de dosis máxima, media y mínima reportados en la Tabla 1.");
            return tercerParrafo;
        }

        public static Paragraph CuartoParrafo()
        {
            Paragraph cuartoParrafo = new Paragraph();
            cuartoParrafo.Style = "Texto";
            cuartoParrafo.AddText("En la Tabla 2 se enumeran los campos utilizados con las caracteristicas de los mismos.");
            return cuartoParrafo;
        }

        public static MigraDoc.DocumentObjectModel.Tables.Table TablaDVH(PlanningItem plan, List<Structure> estructuras)
        {
            MigraDoc.DocumentObjectModel.Tables.Table tabla = new MigraDoc.DocumentObjectModel.Tables.Table();
            for (int i = 0; i < 6; i++)
            {
                tabla.AddColumn();
            }
            MigraDoc.DocumentObjectModel.Tables.Row header = tabla.AddRow();
            header.HeadingFormat = true;
            header.Format.Font.Bold = true;
            header.Cells[0].AddParagraph("Color");
            header.Cells[1].AddParagraph("Estructura");
            header.Cells[2].AddParagraph("Volumen[cm3]");
            header.Cells[3].AddParagraph("Dosis Min[cGy]");
            header.Cells[4].AddParagraph("Dosis Max[cGy]");
            header.Cells[5].AddParagraph("Dosis Media[cGy]");
            foreach (Structure estructura in estructuras)
            {
                DVHData dvh = plan.GetDVHCumulativeData(estructura, DoseValuePresentation.Absolute, VolumePresentation.AbsoluteCm3, 0.1);
                if (dvh == null)
                {

                }
                else
                {
                    MigraDoc.DocumentObjectModel.Tables.Row fila = tabla.AddRow();
                    fila.Cells[0].Shading.Color = new MigraDoc.DocumentObjectModel.Color(estructura.Color.R, estructura.Color.G, estructura.Color.B);
                    fila.Cells[1].AddParagraph(estructura.Id);
                    fila.Cells[2].AddParagraph(Math.Round(estructura.Volume, 1).ToString());
                    fila.Cells[3].AddParagraph(Math.Round(dvh.MinDose.Dose, 1).ToString());
                    fila.Cells[4].AddParagraph(Math.Round(dvh.MaxDose.Dose, 1).ToString());
                    fila.Cells[5].AddParagraph(Math.Round(dvh.MeanDose.Dose, 1).ToString());
                }
            }
            Estilos.formatearTabla(tabla);
            tabla.Columns.Width = 85;
            tabla.Columns[0].Width = 40;
            //tabla.Format.Alignment = ParagraphAlignment.Center;
            tabla.Rows.LeftIndent = "1cm";
            return tabla;
        }





        public static MigraDoc.DocumentObjectModel.Tables.Table TablaCampos(PlanSetup plan)
        {
            MigraDoc.DocumentObjectModel.Tables.Table tabla = new MigraDoc.DocumentObjectModel.Tables.Table();
            for (int i = 0; i < 9; i++)
            {
                tabla.AddColumn();
            }
            MigraDoc.DocumentObjectModel.Tables.Row header = tabla.AddRow();
            header.HeadingFormat = true;
            header.Format.Font.Bold = true;
            header.Cells[0].AddParagraph("Campo");
            header.Cells[1].AddParagraph("Técnica");
            header.Cells[2].AddParagraph("Gantry [º]");
            header.Cells[3].AddParagraph("Col [º]");
            header.Cells[4].AddParagraph("Camilla [º]");
            header.Cells[5].AddParagraph("X (X1/X2)");
            header.Cells[6].AddParagraph("Y (Y1/Y2)");
            header.Cells[7].AddParagraph("DFS [cm]");
            header.Cells[8].AddParagraph("UM");

            foreach (Beam campo in plan.Beams)
            {
                string tecnica = "";
                if (campo.IsSetupField)
                {
                    tecnica = "Setup";
                }
                else if (campo.MLCPlanType == MLCPlanType.DoseDynamic && plan.Beams.First().ControlPoints.Count() > 30)
                {
                    tecnica = "IMRT";
                }
                else if (campo.MLCPlanType == MLCPlanType.VMAT)
                {
                    tecnica = "VMAT";
                }
                else
                {
                    tecnica = "3D";
                }

                string gantry = "";
                if (campo.ControlPoints.First().GantryAngle == campo.ControlPoints.Last().GantryAngle)
                {
                    gantry = crearPPF.IECaVarian(campo.ControlPoints.First().GantryAngle, crearPPF.EquipoEsIEC(campo)).ToString();
                }
                else
                {
                    gantry = crearPPF.IECaVarian(campo.ControlPoints.First().GantryAngle, crearPPF.EquipoEsIEC(campo)).ToString() + "->" + crearPPF.IECaVarian(campo.ControlPoints.Last().GantryAngle, crearPPF.EquipoEsIEC(campo)).ToString();
                }

                double y2 = Math.Round((campo.ControlPoints.First().JawPositions.Y2 / 10), 1);
                double y1 = Math.Round((-campo.ControlPoints.First().JawPositions.Y1 / 10), 1);
                double x2 = Math.Round((campo.ControlPoints.First().JawPositions.X2 / 10), 1);
                double x1 = Math.Round((-campo.ControlPoints.First().JawPositions.X1 / 10), 1);
                double tamX = Math.Round(x2 + x1, 1);
                double tamY = Math.Round(y2 + y1, 1);
                string textoX = tamX.ToString() + " (" + x1.ToString() + "/" + x2.ToString() + ")";
                string textoY = tamY.ToString() + " (" + y1.ToString() + "/" + y2.ToString() + ")";
                string UM = "";
                if (!campo.IsSetupField)
                {
                    UM = Math.Round(campo.Meterset.Value, 0).ToString();
                }
                MigraDoc.DocumentObjectModel.Tables.Row fila = tabla.AddRow();
                fila.Cells[0].AddParagraph(campo.Id);
                fila.Cells[1].AddParagraph(tecnica);
                fila.Cells[2].AddParagraph(gantry);
                fila.Cells[3].AddParagraph(crearPPF.IECaVarian(campo.ControlPoints.First().CollimatorAngle, crearPPF.EquipoEsIEC(campo)).ToString());
                fila.Cells[4].AddParagraph(crearPPF.IECaVarian(campo.ControlPoints.First().PatientSupportAngle, crearPPF.EquipoEsIEC(campo)).ToString());
                fila.Cells[5].AddParagraph(textoX);
                fila.Cells[6].AddParagraph(textoY);
                fila.Cells[7].AddParagraph((Math.Round(campo.SSD, 0) / 10).ToString());
                fila.Cells[8].AddParagraph(UM);
            }
            Estilos.formatearTabla(tabla);
            tabla.Columns.Width = 60;
            tabla.Columns[5].Width = 65;
            tabla.Columns[6].Width = 65;
            tabla.Columns[8].Width = 40;
            tabla.Columns[1].Format.Alignment = ParagraphAlignment.Center;
            tabla.Rows.LeftIndent = "0.5cm";
            return tabla;
        }
        public static Document informe(Patient paciente, PlanningItem plan, Bitmap imagen, List<Structure> estructuras)
        {
            Document informe = new Document();
            Estilos.definirEstilos(informe);
            Section seccion = new Section();
            Estilos.formatearSeccion(seccion);
            var parrafoLogo = seccion.AddParagraph();
            parrafoLogo.Format.Alignment = ParagraphAlignment.Right;
            var Logo = parrafoLogo.AddImage(Properties.Settings.Default.PathPrograma + @"\" + "LogoMeva.png");
            Logo.Height = 70;
            seccion.AddParagraph("Mevaterapia Oncología Radiante", "Titulo");


            seccion.AddParagraph("Informe Físico del tratamiento realizado", "Titulo");
            seccion.Add(PrimerParrafo(paciente, plan));
            if (obtenerImagenes(paciente, imagen))
            {

                var axial = seccion.AddImage("axial.png");
                axial.Width = new Unit(14, UnitType.Centimeter);
                axial.Left = MigraDoc.DocumentObjectModel.Shapes.ShapePosition.Center;

                seccion.AddParagraph("Figura 1. Corte axial de la lesión", "Texto Centro Cursiva");
                seccion.Add(SegundoParrafo());

                MigraDoc.DocumentObjectModel.Tables.Table imagenes = new MigraDoc.DocumentObjectModel.Tables.Table();
                imagenes.AddColumn(new Unit(9, UnitType.Centimeter));
                imagenes.AddColumn(new Unit(9, UnitType.Centimeter));
                imagenes.AddRow();
                imagenes.Rows.LeftIndent = "1cm";
                var coronal = imagenes.Rows[0].Cells[0].AddImage("coronal.png");
                coronal.Width = new Unit(8.5, UnitType.Centimeter);
                var sagital = imagenes.Rows[0].Cells[1].AddImage("sagital.png");
                sagital.Width = new Unit(8.5, UnitType.Centimeter);
                seccion.Add(imagenes);
                seccion.AddParagraph("Figura 2. Vista coronal y sagital reconstruidas en el plano isocentrico", "Texto Centro Cursiva");
                seccion.Add(TercerParrafo());

                Graficar.grafico(paciente, plan, estructuras, dosisTotal, true);

                var DVH = seccion.AddImage("DVH.png");
                DVH.Width = new Unit(14, UnitType.Centimeter);
                DVH.Height = new Unit(7, UnitType.Centimeter);
                DVH.Left = MigraDoc.DocumentObjectModel.Shapes.ShapePosition.Center;
                seccion.AddParagraph("Figura 3. Histograma Dosis-Volumen acumulado", "Texto Centro Cursiva");
                seccion.Add(TablaDVH(plan, estructuras));
                seccion.AddParagraph("Tabla 1. Referencia de volúmenes en histograma dosis-volumen, valores absolutos de dosis máxima, media  y mínima", "Texto Centro Cursiva");

                seccion.Add(CuartoParrafo());
                if (plan is PlanSetup)
                {
                    seccion.Add(TablaCampos((PlanSetup)plan));
                }
                else
                {
                    foreach (PlanSetup planSetup in ((PlanSum)plan).PlanSetups)
                    {
                        seccion.AddParagraph(planSetup.Id + " (dosis: " + planSetup.TotalPrescribedDose.Dose + "cGy)", "Texto");
                        seccion.Add(TablaCampos(planSetup));
                    }
                }
                seccion.AddParagraph("Tabla 2. Campos de tratamiento y características de los mismos.", "Texto Centro Cursiva");

                informe.Add(seccion);
                return informe;
            }
            else
            {
                MessageBox.Show("No se pudieron obtener las imágenes.\nRealizar de nuevo la impresión de pantalla");
                return null;
            }

        }

        public static Document informe(Patient paciente, PlanningItem plan, List<Structure> estructuras)
        {
            Document informe = new Document();
            Estilos.definirEstilos(informe);
            Section seccion = new Section();
            Estilos.formatearSeccion(seccion);
            var parrafoLogo = seccion.AddParagraph();
            parrafoLogo.Format.Alignment = ParagraphAlignment.Right;
            var Logo = parrafoLogo.AddImage(Properties.Settings.Default.PathPrograma + @"\" + "LogoMeva.png");
            Logo.Height = 70;
            seccion.AddParagraph("Mevaterapia Oncología Radiante", "Titulo");


            seccion.AddParagraph("Informe Físico del tratamiento realizado", "Titulo");
            seccion.Add(PrimerParrafo(paciente, plan));
            var axial = seccion.AddImage(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_axial.png");
            axial.Width = new Unit(14, UnitType.Centimeter);
            axial.Left = MigraDoc.DocumentObjectModel.Shapes.ShapePosition.Center;

            seccion.AddParagraph("Figura 1. Corte axial de la lesión", "Texto Centro Cursiva");
            seccion.Add(SegundoParrafo());

            MigraDoc.DocumentObjectModel.Tables.Table imagenes = new MigraDoc.DocumentObjectModel.Tables.Table();
            imagenes.AddColumn(new Unit(9, UnitType.Centimeter));
            imagenes.AddColumn(new Unit(9, UnitType.Centimeter));
            imagenes.AddRow();
            imagenes.Rows.LeftIndent = "1cm";
            var coronal = imagenes.Rows[0].Cells[0].AddImage(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_coronal.png");
            coronal.Width = new Unit(8.5, UnitType.Centimeter);
            var sagital = imagenes.Rows[0].Cells[1].AddImage(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_sagital.png");
            sagital.Width = new Unit(8.5, UnitType.Centimeter);
            seccion.Add(imagenes);
            seccion.AddParagraph("Figura 2. Vista coronal y sagital reconstruidas en el plano isocentrico", "Texto Centro Cursiva");
            seccion.Add(TercerParrafo());

            Graficar.grafico(paciente, plan, estructuras, dosisTotal, false);

            var DVH = seccion.AddImage(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_DVH.png");
            DVH.Width = new Unit(14, UnitType.Centimeter);
            DVH.Height = new Unit(7, UnitType.Centimeter);
            DVH.Left = MigraDoc.DocumentObjectModel.Shapes.ShapePosition.Center;
            seccion.AddParagraph("Figura 3. Histograma Dosis-Volumen acumulado", "Texto Centro Cursiva");
            seccion.Add(TablaDVH(plan, estructuras));
            seccion.AddParagraph("Tabla 1. Referencia de volúmenes en histograma dosis-volumen, valores absolutos de dosis máxima, media  y mínima", "Texto Centro Cursiva");

            seccion.Add(CuartoParrafo());
            if (plan is PlanSetup)
            {
                seccion.Add(TablaCampos((PlanSetup)plan));
            }
            else
            {
                foreach (PlanSetup planSetup in ((PlanSum)plan).PlanSetups)
                {
                    seccion.AddParagraph(planSetup.Id + " (dosis: " + planSetup.TotalPrescribedDose.Dose + "cGy)", "Texto");
                    seccion.Add(TablaCampos(planSetup));
                }
            }
            seccion.AddParagraph("Tabla 2. Campos de tratamiento y características de los mismos.", "Texto Centro Cursiva");

            informe.Add(seccion);
            return informe;
        }

        public static void exportarAPdf(Patient paciente, PlanningItem plan, Bitmap imagen, List<Structure> estructuras)
        {
            string nombreMasID = paciente.LastName.ToUpper() + ", " + paciente.FirstName.ToUpper() + "-" + paciente.Id;
            string pathDirectorio = IO.crearCarpetaPaciente(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, plan).Id, plan.Id);
            string path = IO.GetUniqueFilename("", nombreMasID + "_Informe", "pdf");
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            Document Informe = informe(paciente, plan, imagen, estructuras);
            if (Informe != null)
            {
                pdfRenderer.Document = Informe;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(pathDirectorio + @"\" + path);
                MessageBox.Show("Se ha generado el informe del paciente correctamente");
                File.Delete("DVH.png");
                File.Delete("sagital.png");
                File.Delete("coronal.png");
                File.Delete("axial.png");
            }

        }

        public static void exportarAPdf(Patient paciente, PlanningItem plan, List<Structure> estructuras)
        {
            string nombreMasID = paciente.LastName.ToUpper() + ", " + paciente.FirstName.ToUpper() + "-" + paciente.Id;
            string pathDirectorio = IO.crearCarpetaPaciente(paciente.LastName, paciente.FirstName, paciente.Id, crearInforme.Curso(paciente, plan).Id, plan.Id);
            string path = IO.GetUniqueFilename("", nombreMasID + "_Informe", "pdf");
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            Document Informe = informe(paciente, plan, estructuras);
            if (Informe != null)
            {
                pdfRenderer.Document = Informe;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(pathDirectorio + @"\" + path);
                MessageBox.Show("Se ha generado el informe del paciente correctamente");
                File.Delete(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_DVH.png");
                File.Delete(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_sagital.png");
                File.Delete(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_coronal.png");
                File.Delete(Properties.Settings.Default.PathPrograma + @"\Imagenes\" + paciente.Id + "_axial.png");
                Clipboard.Clear(); //Probar. Para que no se use en otro paciente después
            }

        }
        #endregion
    }
}
