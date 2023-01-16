using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMS.TPS.Common.Model.Types;
using VMS.TPS.Common.Model.API;


namespace CalcIndep_Carpeta
{
    public class DRR
    {
        public static Bitmap dibujarCampoEnImagen(Beam campo)
        {
            Bitmap bitmap = bitmapDeImagenDeReferencia(campo);
            int long1cm = Convert.ToInt32(10 / campo.ReferenceImage.XRes);
            dibujarCampo(bitmap, campo.ControlPoints[0].JawPositions.X1, campo.ControlPoints[0].JawPositions.X2, campo.ControlPoints[0].JawPositions.Y1, campo.ControlPoints[0].JawPositions.Y2, long1cm, campo.IsSetupField, campo.ControlPoints[0].CollimatorAngle, campo.ControlPoints[0].LeafPositions);
            AgregarTexto(bitmap, crearPPF.IECaVarian(campo.ControlPoints.First().GantryAngle).ToString(), campo.Id, Math.Round((campo.SSD / 10), 1).ToString());
            return bitmap;
        }

        public static Bitmap bitmapDeImagenDeReferencia(Beam campo)
        {
            VMS.TPS.Common.Model.API.Image imagen = campo.ReferenceImage;
            int[,] matriz = new int[imagen.XSize, imagen.YSize];
            double[,] matrizDouble = new double[imagen.XSize, imagen.YSize];
            Bitmap bitmap = new Bitmap(imagen.XSize, imagen.YSize);
            imagen.GetVoxels(0, matriz);
            for (int i = 0; i < imagen.XSize; i++)
            {
                for (int j = 0; j < imagen.YSize; j++)
                {
                    int valor = (matriz[i, j] * 255 / imagen.Window);
                    matrizDouble[i, j] = valor;
                    bitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(valor, valor, valor));
                }
            }
            Bitmap bitmap2 = new Bitmap(bitmap, new Size(imagen.XSize, imagen.YSize));


            return bitmap2;
        }


        public static void AgregarTexto(Bitmap bitmap, string gantry, string nombreCampo, string DFP)
        {
            string texto = nombreCampo + "\nGantry: " + gantry + "º\nDFP: " + DFP + "cm";


            using (var g = Graphics.FromImage(bitmap))
            {
                RectangleF rectf = new RectangleF(30, 30, 400, 150);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawString(texto, new System.Drawing.Font("Arial", 15), Brushes.Yellow, rectf);

                g.Flush();
            }
        }

        public static int GenerarImagenes(PlanSetup plan, string pathDestino)
        {
            int imagenes = 0;
            List<Beam> camposOrdenados = plan.Beams.OrderBy(b => !b.IsSetupField).ToList();
            foreach (Beam campo in camposOrdenados)
            {
                if (campo.IsSetupField || campo.ControlPoints.First().GantryAngle == campo.ControlPoints.Last().GantryAngle)
                {
                    if (campo.ReferenceImage != null)
                    {
                        Bitmap bitmap = dibujarCampoEnImagen(campo);
                        bitmap.Save(pathDestino + @"\" + (imagenes + 1).ToString() + "_" + campo.Id + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        imagenes++;
                    }
                }
            }
            return imagenes;
        }

        #region Dibujar

        public static Pen amarillo = new Pen(Color.Yellow, 1);
        public static int anchoLineaCorta = 5;
        public static int anchoLineaLarga = 10;
        public static void lineaHorizontal(Bitmap bitmap, int y, int xInicial, int xFinal, Pen pen)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawLine(pen, xInicial, y, xFinal, y);
            }
        }

        public static void lineaVertical(Bitmap bitmap, int x, int yInicial, int yFinal, Pen pen)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawLine(pen, x, yInicial, x, yFinal);
            }
        }

        public static void dibujarEje(Bitmap bitmap)
        {
            lineaHorizontal(bitmap, bitmap.Size.Height / 2, 0, bitmap.Size.Width, amarillo);
            lineaVertical(bitmap, bitmap.Width / 2, 0, bitmap.Height, amarillo);
        }

        public static void agregarLineaCortaHorizontal(Bitmap bitmap, int y)
        {
            lineaHorizontal(bitmap, y, bitmap.Size.Width / 2 - anchoLineaCorta / 2, bitmap.Size.Width / 2 + anchoLineaCorta / 2, amarillo);
        }

        public static void agregarLineaLargaHorizontal(Bitmap bitmap, int y)
        {
            lineaHorizontal(bitmap, y, bitmap.Size.Width / 2 - anchoLineaLarga / 2, bitmap.Size.Width / 2 + anchoLineaLarga / 2, amarillo);
        }

        public static void agregarLineaCortaVertical(Bitmap bitmap, int x)
        {
            lineaVertical(bitmap, x, bitmap.Size.Height / 2 - anchoLineaCorta / 2, bitmap.Size.Height / 2 + anchoLineaCorta / 2, amarillo);
        }

        public static void agregarLineaLargaVertical(Bitmap bitmap, int x)
        {
            lineaVertical(bitmap, x, bitmap.Size.Height / 2 - anchoLineaLarga / 2, bitmap.Size.Height / 2 + anchoLineaLarga / 2, amarillo);
        }


        public static void agregarLineasHorizontales(Bitmap bitmap, int long1cm)
        {
            int i = long1cm;
            while (i < bitmap.Size.Width / 2)
            {
                if (i / long1cm % 5 == 0)
                {
                    agregarLineaLargaHorizontal(bitmap, bitmap.Size.Width / 2 - i);
                    agregarLineaLargaHorizontal(bitmap, bitmap.Size.Width / 2 + i);
                }
                else
                {
                    agregarLineaCortaHorizontal(bitmap, bitmap.Size.Width / 2 - i);
                    agregarLineaCortaHorizontal(bitmap, bitmap.Size.Width / 2 + i);
                }
                i += long1cm;
            }
        }

        public static void agregarLineasVerticales(Bitmap bitmap, int long1cm)
        {
            int i = long1cm;
            while (i < bitmap.Size.Height / 2)
            {
                if (i / long1cm % 5 == 0)
                {
                    agregarLineaLargaVertical(bitmap, bitmap.Size.Width / 2 - i);
                    agregarLineaLargaVertical(bitmap, bitmap.Size.Width / 2 + i);
                }
                else
                {
                    agregarLineaCortaVertical(bitmap, bitmap.Size.Width / 2 - i);
                    agregarLineaCortaVertical(bitmap, bitmap.Size.Width / 2 + i);
                }
                i += long1cm;
            }
        }

        public static void bordesCampo(Bitmap bitmap, double x1, double x2, double y1, double y2, double long1cm, bool esSetup)
        {
            int height = bitmap.Height;
            int width = bitmap.Width;
            int x1p = height / 2 + Convert.ToInt32(x1 / 10 * long1cm);
            int x2p = height / 2 + Convert.ToInt32(x2 / 10 * long1cm);
            int y1p = width / 2 - Convert.ToInt32(y1 / 10 * long1cm);
            int y2p = width / 2 - Convert.ToInt32(y2 / 10 * long1cm);
            Pen pen = new Pen(Color.Yellow);
            if (esSetup)
            {
                pen.Color = Color.Cyan;
            }
            lineaHorizontal(bitmap, y2p, x1p, x2p, pen);
            lineaHorizontal(bitmap, y1p, x1p, x2p, pen);
            lineaVertical(bitmap, x1p, y1p, y2p, pen);
            lineaVertical(bitmap, x2p, y1p, y2p, pen);
        }

        public static Bitmap rotar(Bitmap bitmap, double angulo)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TranslateTransform(bitmap.Width / 2, bitmap.Height / 2);
            graphics.RotateTransform(Convert.ToSingle(angulo));
            graphics.TranslateTransform(-bitmap.Width / 2, -bitmap.Height / 2);
            graphics.DrawImage(bitmap, new Point(0, 0));
            graphics.Dispose();
            return bitmap;
        }

        public static void dibujarCampo(Bitmap bitmap, double x1, double x2, double y1, double y2, int long1cm, bool esSetup, double anguloColimador, float[,] MLCs = null)
        {
            bitmap = rotar(bitmap, anguloColimador);
            dibujarEje(bitmap);
            bordesCampo(bitmap, x1, x2, y1, y2, long1cm, esSetup);
            agregarLineasHorizontales(bitmap, long1cm);
            agregarLineasVerticales(bitmap, long1cm);
            if (!esSetup && MLCs != null)
            {
                dibujarMLC(bitmap, MLCs, x1, x2, y1, y2, long1cm);
            }
            bitmap = rotar(bitmap, -anguloColimador);
        }

        public static void marcarRef(Bitmap bitmap, int long1cm, double xRef, double yRef, double zRef, double AnguloGantry)
        {
            double desplazHorizcm = 0;
            double desplazVertcm = zRef;
            if (AnguloGantry==0)
            {
                desplazHorizcm = xRef;
            }
            else if (AnguloGantry==90)
            {
                desplazHorizcm = yRef;
            }
            else if (AnguloGantry==270)
            {
                desplazHorizcm = -yRef;
            }

        }


        public static void recortarImagen(Bitmap bitmap, double x1, double x2, double y1, double y2, int long1cm, bool esSetup, double anguloColimador)
        {

        }
        public static void dibujarMLC(Bitmap bitmap, float[,] MLCs, double x1, double x2, double y1, double y2, int long1cm)
        {
            int yFondo = bitmap.Height / 2 + Convert.ToInt32(21 * long1cm);
            int xMedio = bitmap.Width / 2;
            int yAncha = 1 * long1cm;
            int yFina = Convert.ToInt32(0.5 * long1cm);
            int AnchoLamina;
            int xLaminaIzq;
            int xLaminaSiguienteIzq;
            int xLaminaDer;
            int xLaminaSiguienteDer;
            int yLamina = yFondo;
            int yLaminaSiguiente = yLamina - yAncha;
            int height = bitmap.Height;
            int width = bitmap.Width;
            int x1p = height / 2 + Convert.ToInt32(x1 / 10 * long1cm);
            int x2p = height / 2 + Convert.ToInt32(x2 / 10 * long1cm);
            int y1p = width / 2 - Convert.ToInt32(y1 / 10 * long1cm);
            int y2p = width / 2 - Convert.ToInt32(y2 / 10 * long1cm);


            for (int i = 0; i < 59; i++)
            {

                xLaminaIzq = Convert.ToInt32(xMedio + MLCs[0, i] / 10 * long1cm);
                xLaminaSiguienteIzq = Convert.ToInt32(xMedio + MLCs[0, i + 1] / 10 * long1cm);
                xLaminaDer = Convert.ToInt32(xMedio + MLCs[1, i] / 10 * long1cm);
                xLaminaSiguienteDer = Convert.ToInt32(xMedio + MLCs[1, i + 1] / 10 * long1cm);
                yLamina = yLaminaSiguiente;
                if (i > 9 && i < 50)
                {
                    AnchoLamina = yFina;
                }
                else
                {
                    AnchoLamina = yAncha;
                }
                yLaminaSiguiente = yLamina - AnchoLamina;
                if (yLamina < y1p + AnchoLamina && yLamina > y2p && xLaminaIzq != xLaminaDer)
                //if (xLaminaDer<x2p && xLaminaIzq>x1p && yLamina< y1p + AnchoLamina && yLamina > y2p && xLaminaIzq!=xLaminaDer)
                {
                    lineaVertical(bitmap, xLaminaIzq, yLamina, yLaminaSiguiente, amarillo);
                    lineaVertical(bitmap, xLaminaDer, yLamina, yLaminaSiguiente, amarillo);
                    lineaHorizontal(bitmap, yLaminaSiguiente, xLaminaIzq, xLaminaSiguienteIzq, amarillo);
                    lineaHorizontal(bitmap, yLaminaSiguiente, xLaminaDer, xLaminaSiguienteDer, amarillo);
                }
            }
        }
        #endregion
    }
}
