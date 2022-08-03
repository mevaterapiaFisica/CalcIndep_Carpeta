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
using Extreme.Mathematics;





namespace CalcIndep_Carpeta
{




    public partial class Form_CI : Form
    {


        public ReporteCI.CampoCI campoCI = new ReporteCI.CampoCI();

        //Equipo          = new Equipo(SN, "ID", Prof. Max, Tr_MLC, PDD/100, Dose Rate (cGy), energia);
        Equipo Equipo1_6X = new Equipo(2284, "Equipo1", 15, 0.016, 0.6727, 1, 6);
        Equipo Equipo3_6X = new Equipo(1178, "Equipo3", 16, 0.018, 0.6727, 1, 6);
        Equipo Equipo3_10X = new Equipo(1178, "Equipo3", 24, 0.021, 0.7361, 1, 10);
        Equipo Equipo4_6X = new Equipo(6158, "Equipo4", 15, 0.016, 0.6727, 1, 6);
        Equipo Equipo4_6X_SRS = new Equipo(6158, "Equipo4", 14, 0.016, 0.6627, 1, 6);
        Equipo Equipo4_10X = new Equipo(6158, "Equipo4", 22, 0.018, 0.7361, 1, 10);
        Equipo Equipo2_6X = new Equipo(1181, "Equipo2", 13 , 0.0143, 0.6653, 1, 6);


        List<FieldReferencePoint> RefPoints = new List<FieldReferencePoint>();
        Beam CampoActual;
        double UM_indep;
        string PacTAC;
        double WED;
        double DFP;
        double Field_Dose;
        double PlannedFractions;
        double[,] Sp;
        double[] TC_Sp;
        double[] IC_Sp;
        double[,] TC_eff;
        double[] Jaw_Short;
        double[] Jaw_Long;
        double[,] PDD_GD_6X;
        double[,] PDD_GD_10X;
        double[,] PDD_GD_6X_SRS;
        double[] TC_PDD_GD_STD;
        double[] prof_PDD_GD_STD;
        double[] TC_PDD_GD_SRS;
        double[] prof_PDD_GD_SRS;
        double[,] Scp_GD_6X;
        double[,] Scp_GD_10X;
        double[,] Scp_GD_6X_SRS;
        double[] X_Scp_GD_6X_SRS;
        double[] Y_Scp_GD_6X_SRS;
        double[] X_Scp_GD_6X;
        double[] Y_Scp_GD_6X;
        double[] X_Scp_GD_10X;
        double[] Y_Scp_GD_10X;
        double[,] OAR_GD_6X;
        double[] Prof_OAR_6X;
        double[] Dist_OAR_6X;
        double[,] OAR_GD_10X;
        double[] Prof_OAR_10X;
        double[] Dist_OAR_10X;
        double[,] OAR_GD_6X_SRS;
        double[] Prof_OAR_6X_SRS;
        double[] Dist_OAR_6X_SRS;
        double[] GSTT_6X;
        double[] Y_GSTT_6X;
        double[] GSTT_10X;
        double[] Y_GSTT_10X;
        float[,] MLC_ones = new float[400, 400];



        Bitmap bitmap = new Bitmap(420, 420);
        Bitmap bitmapCrop;
        public static Pen pen_rojo = new Pen(Color.Red, 2);
        public static Pen pen_verde = new Pen(Color.Green, 1);
        public static Pen pen_negra = new Pen(Color.Black, 2);
        public static Pen pen_azul = new Pen(Color.Blue, 2);
        public Form_CI()
        {
            InitializeComponent();
            // Creo matriz de unos de 400x400
            MLC_ones = Fill_Matrix(MLC_ones, 1);
        }

        public Form_CI(PlanSetup plan, Beam Campo, int NumberOfFractions, string PatOr, int campoNumero, int camposTotales)
        {
            InitializeComponent();
            RefPoints = listaRefPoints_CI(Campo);
            Text += plan.Id + ", " + Campo.Id + "(" + campoNumero.ToString() + "/" + camposTotales.ToString() + ")";
            listBox_RefPoints.DataSource = RefPoints;
            listBox_RefPoints.DisplayMember = "ReferencePoint";
            CampoActual = Campo;
            PacTAC = PatOr;
            if (campoNumero == camposTotales)
            {
                button_Next.Text = "Finalizar";
            }
            else
            {
                button_Next.Text = "Próximo";
            }
            button_Next.Enabled = false;
            label_IDcampo.Text = CampoActual.Id.ToString();
            label_Equipo.Text = CampoActual.TreatmentUnit.Id.ToString();
            label_Energia.Text = CampoActual.EnergyModeDisplayName.ToString();
            label_UMeclipse.Text = CampoActual.Meterset.Value.ToString("0.00");
        }

        public List<FieldReferencePoint> listaRefPoints_CI(Beam campo)
        {
            List<FieldReferencePoint> lista = new List<FieldReferencePoint>();
            lista.Clear();
            foreach (FieldReferencePoint rp in campo.FieldReferencePoints)
            {
                lista.Add(rp);
            }
            return lista;
        }

        private void button_Next_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Calc_Click(object sender, EventArgs e)
        {

            GSTT_6X = new double[31] { 0.150691, 0.168051, 0.18722, 0.208376, 0.231707, 0.257422, 0.285748, 0.316933, 0.351245, 0.388978, 0.430453, 0.476017, 0.52605, 0.580964, 0.64121, 0.707274, 0.77969, 0.859035, 0.945937, 1.04108, 1.145206, 1.259122, 1.383704, 1.519904, 1.668756, 1.831381, 2.008999, 2.202931, 2.414611, 2.645597, 2.897577 };
            Y_GSTT_6X = new double[31] { -200, -190, -180, -170, -160, -150, -140, -130, -120, -110, -100, -90, -80, -70, -60, -50, -40, -30, -20, -10, 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            GSTT_10X = new double[31] { 0.195441, 0.214977, 0.236228, 0.259328, 0.284425, 0.311673, 0.341242, 0.373311, 0.408074, 0.445738, 0.486525, 0.530673, 0.578438, 0.630093, 0.685931, 0.746266, 0.811433, 0.881793, 0.957731, 1.039658, 1.128016, 1.223276, 1.325944, 1.436558, 1.555697, 1.683978, 1.822059, 1.970646, 2.130494, 2.302406, 2.487244 };
            Y_GSTT_10X = new double[31] { -200, -190, -180, -170, -160, -150, -140, -130, -120, -110, -100, -90, -80, -70, -60, -50, -40, -30, -20, -10, 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };



            FieldReferencePoint FieldRefPoint = (FieldReferencePoint)listBox_RefPoints.SelectedItem;
            
            if ((FieldRefPoint.RefPointLocation.x.ToString()) != "NaN")
            {
                

                // Extraigo datos



                // Coordenadas punto de calculo.
                var RefPos = FieldRefPoint.RefPointLocation;
                var Iso = CampoActual.IsocenterPosition;

                double x_calc = RefPos[0] - Iso[0];
                double y_calc = RefPos[1] - Iso[1];
                double z_calc = RefPos[2] - Iso[2];

                double ang_gtry = (Math.PI / 180) * CampoActual.ControlPoints[0].GantryAngle;
                double ang_coll = (Math.PI / 180) * CampoActual.ControlPoints[0].CollimatorAngle;
                double ang_cam = (Math.PI / 180) * CampoActual.ControlPoints[0].PatientSupportAngle;

                //Corrijo por posicion del paciente en la TAC
                double x_posTac = 1;
                double y_posTac = 1;
                double z_posTac = 1;

                switch (PacTAC)
                {
                    case "HeadFirstSupine":
                        x_posTac = 1;
                        y_posTac = 1;
                        z_posTac = 1;
                        break;
                    case "HeadFirstProne":
                        x_posTac = -1;
                        y_posTac = -1;
                        z_posTac = 1;
                        break;
                    case "FeetFirstSupine":
                        x_posTac = 1;
                        y_posTac = -1;
                        z_posTac = -1;
                        break;
                    case "FeetFirstProne":
                        x_posTac = 1;
                        y_posTac = -1;
                        z_posTac = -1;
                        break;
                }
                x_calc = x_calc * x_posTac;
                y_calc = y_calc * y_posTac;
                z_calc = z_calc * z_posTac;

                double[,] Rot_cam = new double[3, 3] { { Math.Cos(ang_cam), 0, -Math.Sin(ang_cam) }, { 0, 1, 0 }, { Math.Sin(ang_cam), 0, Math.Cos(ang_cam) } };
                double[,] Rot_Gtry = new double[3, 3] { { Math.Cos(ang_gtry), Math.Sin(ang_gtry), 0 }, { -Math.Sin(ang_gtry), Math.Cos(ang_gtry), 0 }, { 0, 0, 1 } };
                double[,] Rot_coll = new double[3, 3] { { Math.Cos(ang_coll), 0, Math.Sin(ang_coll) }, { 0, 1, 0 }, { -Math.Sin(ang_coll), 0, Math.Cos(ang_coll) } };

                var Aux1 = Cross(Rot_coll, Rot_Gtry);
                var Aux2 = Cross(Aux1, Rot_cam);
                var coords = new double[3] { x_calc, y_calc, z_calc };
                var Ccoords = CrossV(Aux2, coords);
                x_calc = Ccoords[0];
                y_calc = Ccoords[1];
                z_calc = Ccoords[2];


                if (CampoActual.EnergyModeDisplayName.ToString() == "6X" || CampoActual.EnergyModeDisplayName.ToString() == "6X-SRS" || CampoActual.EnergyModeDisplayName.ToString() == "10X")
                {
                    // Cargo la información Especifica del equipo. 
                    double Dmax = 0;
                    float tr_MLC = 0;
                    double TPR_20_10 = 0; // obtenido del Eclipse
                    double Dose = 0; //tasa de dosis para 10x10 a prof del maximo a DFP=DFI
                    switch (CampoActual.TreatmentUnit.Id.ToString())
                    {
                        case ("Equipo1"):
                            tr_MLC = (float)Equipo1_6X.tr_MLC;
                            Dmax = Equipo1_6X.Dmax;
                            TPR_20_10 = Equipo1_6X.TPR_20_10;
                            Dose = Equipo1_6X.Dose;
                            break;
                        case ("Equipo 2 6EX"):
                            tr_MLC = (float)Equipo2_6X.tr_MLC;
                            Dmax = Equipo2_6X.Dmax;
                            TPR_20_10 = Equipo2_6X.TPR_20_10;
                            Dose = Equipo2_6X.Dose;
                            break;

                        case ("2100CMLC"):
                            switch (CampoActual.EnergyModeDisplayName.ToString())
                            {
                                case "6X":
                                    tr_MLC = (float)Equipo3_6X.tr_MLC;
                                    Dmax = Equipo3_6X.Dmax;
                                    TPR_20_10 = Equipo3_6X.TPR_20_10;
                                    Dose = Equipo3_6X.Dose;

                                    break;
                                case "10X":
                                    tr_MLC = (float)Equipo3_10X.tr_MLC;
                                    Dmax = Equipo3_10X.Dmax;
                                    TPR_20_10 = Equipo3_10X.TPR_20_10;
                                    Dose = Equipo3_10X.Dose;
                                    break;

                            }
                            break;

                        case ("D-2300CD"):
                            switch (CampoActual.EnergyModeDisplayName.ToString())
                            {
                                case "6X":
                                    tr_MLC = (float)Equipo4_6X.tr_MLC;
                                    Dmax = Equipo4_6X.Dmax;
                                    TPR_20_10 = Equipo4_6X.TPR_20_10;
                                    Dose = Equipo4_6X.Dose;

                                    break;
                                case "10X":
                                    tr_MLC = (float)Equipo4_10X.tr_MLC;
                                    Dmax = Equipo4_10X.Dmax;
                                    TPR_20_10 = Equipo4_10X.TPR_20_10;
                                    Dose = Equipo4_10X.Dose;
                                    break;

                                case "6X-SRS":
                                    tr_MLC = (float)Equipo4_6X_SRS.tr_MLC;
                                    Dmax = Equipo4_6X_SRS.Dmax;
                                    TPR_20_10 = Equipo4_6X_SRS.TPR_20_10;
                                    Dose = Equipo4_6X_SRS.Dose;

                                    break;

                            }
                            break;

                    }

                    WED = FieldRefPoint.EffectiveDepth;
                    DFP = FieldRefPoint.SSD;
                    //DFP = CampoActual.SSD;
                    Field_Dose = FieldRefPoint.FieldDose.Dose;

                    var CP = CampoActual.ControlPoints;

                    double X1 = -Math.Ceiling(CP[0].JawPositions.X1);
                    double X2 = Math.Ceiling(CP[0].JawPositions.X2);
                    double Y1 = -Math.Ceiling(CP[0].JawPositions.Y1);
                    double Y2 = Math.Ceiling(CP[0].JawPositions.Y2);
                    float[,] field_tr = new float[(int)(Y1 + Y2), (int)(X1 + X2)];
                    field_tr = Fill_Matrix(field_tr, 0);

                    int CP_number = CP.Count;
                    for (int idx = 0; idx < CP_number; idx++)
                    {
                        double cum_MeterSet = CP[idx].MetersetWeight;
                        float[,] Leafs = CP[idx].LeafPositions;
                        float[,] fl_MLC = new float[400, 400];
                        fl_MLC = Fill_Matrix(fl_MLC, tr_MLC);

                        dibujarMLC(bitmap, Leafs, X1, X2, Y1, Y2,Convert.ToInt32(x_calc),Convert.ToInt32(z_calc));

                        int yFondo = bitmap.Height / 2; // siempre es 210  + Convert.ToInt32(21 * long1cm);
                        int xMedio = bitmap.Width / 2; // siempre es 210;
                        int margen = 30;


                    
                        pictureBox.Image = bitmap;

                        double curr_MeterSet = 0;
                        int y0 = 0;

                        if (idx < CP_number && idx.IsEven())
                        {
                            curr_MeterSet = (CP[idx + 1].MetersetWeight - CP[idx].MetersetWeight) / 2;
                        }
                        else if (idx < CP_number && idx.IsOdd())
                        {
                            curr_MeterSet = (CP[idx].MetersetWeight - CP[idx - 1].MetersetWeight) / 2;
                        }
                        /*if (idx > 0 && idx < CP_number)
                        {
                            curr_MeterSet = cum_MeterSet - CP[idx - 1].MetersetWeight;
                        }*/

                        for (int l = 0; l < 60; l++)
                        {
                            //int Lx2 = 200 + (int)Leafs[0, 59 - l];
                            //int Lx1 = 200 + (int)Leafs[1, 59 - l];

                            int Lx2 = Math.Max(Math.Min(200 + (int)Leafs[0, 59 - l],400),0);
                            int Lx1 = Math.Max(Math.Min(200 + (int)Leafs[1, 59 - l],400),0);

                            int width = 5;
                            if (l < 10 | l > 49)
                            {
                                width = 10;
                            }
                            for (int y = y0; y < y0 + width; y++)
                            {
                                for (int x = Lx2; x < Lx1; x++)
                                    fl_MLC.SetValue(1, y, x);
                            }
                            y0 = y0 + width;
                        }
                        var MM = Multiply_subMatrix(fl_MLC, (float)curr_MeterSet, 200 - (int)Y2 + 1, 200 + (int)Y1, 200 - (int)X1 + 1, 200 + (int)X2);

                        if (idx == 0)
                        {
                            field_tr = MM;
                        }
                        else
                        {
                            field_tr = Add_Matrix(field_tr, MM);
                        }
                        double X1Margen = Math.Min(X1 + margen, 200);
                        double X2Margen = Math.Min(X2 + margen, 200);
                        double Y1Margen = Math.Min(Y1 + margen, 200);
                        double Y2Margen = Math.Min(Y2 + margen, 200);
                        bitmapCrop = bitmap.Clone(new Rectangle(Convert.ToInt32(xMedio - X1Margen), Convert.ToInt32(yFondo - Y2Margen), Convert.ToInt32(X2Margen + X1Margen), Convert.ToInt32(Y2Margen + Y1Margen)), bitmap.PixelFormat);
                        bitmapCrop = bitmap.Clone(new Rectangle(Convert.ToInt32(xMedio - X1), Convert.ToInt32(yFondo - Y2), Convert.ToInt32(X2 + X1), Convert.ToInt32(Y2 + Y1)), bitmap.PixelFormat);
                    }

                    // Chequeo si hay cuñas en el campo de tratamiento
                    var cunia = CampoActual.Wedges;
                    if (cunia.Count() > 0)
                    {
                        string W_Id = cunia.First().Id;
                        double W_ang = (Math.PI / 180) * cunia.First().WedgeAngle;
                        double W_ori = cunia.First().Direction;
                        double h_60 = Math.Tan(W_ang) / Math.Tan(Math.PI / 3);
                        double h_0 = 1 - h_60;
                        double MaxSTT = 0;
                        float[] STT_60 = new float[(int)(Y1 + Y2)];
                        STT_60 = Fill_Array(STT_60, 1);
                        switch (W_ori)
                        {
                            case 0:  //IN
                                for (int i = 0; i < (STT_60.Length - 5); i++)
                                {
                                    double yy = -Y1 + 0.5 + i;
                                    switch (CampoActual.EnergyModeDisplayName.ToString())
                                    {
                                        case "6X":
                                            STT_60[i] = (float)Interp1D(Y_GSTT_6X, GSTT_6X, yy, 1);
                                            break;
                                        case "10X":
                                            STT_60[i] = (float)Interp1D(Y_GSTT_10X, GSTT_10X, yy, 1);
                                            break;
                                    }
                                    if (i == 0)
                                    {
                                        MaxSTT = STT_60[i];
                                    }
                                    else
                                    {
                                        if (STT_60[i] > MaxSTT)
                                            MaxSTT = STT_60[i];
                                    }
                                }
                                for (int i = (STT_60.Length - 5); i < (STT_60.Length); i++)
                                {
                                    STT_60[i] = (float)MaxSTT;
                                }
                                break;
                            case 180:  // OUT
                                for (int i = 5; i < (STT_60.Length); i++)
                                {
                                    double yy = -Y1 + 0.5 + i;
                                    switch (CampoActual.EnergyModeDisplayName.ToString())
                                    {
                                        case "6X":
                                            STT_60[i] = (float)Interp1D(Y_GSTT_6X, GSTT_6X, yy, -1);
                                            break;
                                        case "10X":
                                            STT_60[i] = (float)Interp1D(Y_GSTT_10X, GSTT_10X, yy, -1);
                                            break;
                                    }
                                    if (STT_60[i] > MaxSTT)
                                        MaxSTT = STT_60[i];
                                }
                                for (int i = 0; i < (5); i++)
                                {
                                    STT_60[i] = (float)MaxSTT;
                                }
                                break;
                        }
                        double STT_0 = 0;
                        switch (CampoActual.EnergyModeDisplayName.ToString())
                        {
                            case "6X":
                                STT_0 = (float)Interp1D(Y_GSTT_6X, GSTT_6X, 0, 1);
                                break;
                            case "10X":
                                STT_0 = (float)Interp1D(Y_GSTT_10X, GSTT_10X, 0, 1);
                                break;
                        }
                        float[] STT_aux = Multiply_Array(STT_60, (float)h_60);
                        float[] STT = Add_Array(STT_aux, (float)(STT_0 * h_0));
                        double Max_STT = 0;
                        for (int j = 0; j < STT.Length; j++)
                        {
                            if (STT[j] > Max_STT)
                                Max_STT = STT[j];
                        }
                        STT = Multiply_Array(STT, (float)(1 / Max_STT));

                        for (int i = 0; i < field_tr.GetLength(0); i++)
                        {
                            for (int j = 0; j < field_tr.GetLength(1); j++)
                            {
                                field_tr[i, j] = STT[field_tr.GetLength(0)-i-1] * field_tr[i, j];
                            }
                        }

                    }
                    //FIN  Chequeo si hay cuñas en el campo de tratamiento

                    double[] poli_TPR = new double[6] { 1, TPR_20_10, Math.Pow(TPR_20_10, 2), Math.Pow(TPR_20_10, 3), Math.Pow(TPR_20_10, 4), Math.Pow(TPR_20_10, 5) };
                    try
                    {
                        UM_indep = Calculo_Fotones(poli_TPR, CampoActual.EnergyModeDisplayName.ToString(), WED, DFP, X1, X2, Y1, Y2, field_tr, Dmax, TPR_20_10, Dose, Field_Dose, CampoActual.IsocenterPosition, FieldRefPoint.RefPointLocation, x_calc, y_calc, z_calc);
                        button_Next.Enabled = true;
                    }
                    catch(Exception err)
                    {
                        MessageBox.Show("Ups! Algo no funcionó... ");
                    }
                }
                else
                {
                    var App = CampoActual.Applicator.Name;
                    var Energia = CampoActual.EnergyModeDisplayName;
                    var SSD = CampoActual.SSD;
                    double esp_bolus;
                    double ang_inc;
                    double[] x=null;
                    double[] y=null;
                    double Rp = 0;

                    if (Math.Round(SSD) >= 995 && SSD < 1400)
                    {
                        if (CampoActual.Boluses.Count() > 0)
                        {
                            var bolus = CampoActual.Boluses.First();
                            double.TryParse(textBox_Bolus.Text,out esp_bolus);
                            SSD = SSD - esp_bolus;
                        }

                        double.TryParse(textBoxAng.Text, out ang_inc);
                        switch (App)
                        {
                            case "A06":
                                x = new double[101];
                                y = new double[101];
                                for (int i = 0; i < 101; i++)
                                {
                                    x[i] = -50 + i;
                                    y[i] = -50 + i;
                                }
                                break;
                            case "A10":
                                x = new double[141];
                                y = new double[141];
                                for (int i = 0; i < 141; i++)
                                {
                                    x[i] = -70 + i;
                                    y[i] = -70 + i;
                                }
                                break;
                            case "A15":
                                x = new double[191];
                                y = new double[191];
                                for (int i = 0; i < 191; i++)
                                {
                                    x[i] = -95 + i;
                                    y[i] = -95 + i;
                                }
                                break;
                            case "A20":
                                x = new double[241];
                                y = new double[241];
                                for (int i = 0; i < 241; i++)
                                {
                                    x[i] = -120 + i;
                                    y[i] = -120 + i;
                                }
                                break;
                            case "A25":
                                x = new double[291];
                                y = new double[291];
                                for (int i = 0; i < 291; i++)
                                {
                                    x[i] = -145 + i;
                                    y[i] = -145 + i;
                                }
                                break;
                        }

                        switch (Energia)
                        {
                            case "6E":
                                Rp = 29.9; //mm
                                break;
                            case "9E":
                                Rp = 44.9; //mm
                                break;
                            case "12E":
                                Rp = 59.9; //mm
                                break;
                            case "15E":
                                Rp = 74.9; //mm
                                break;
                            case "16E":
                                Rp = 79.8; //mm
                                break;
                            case "18E":
                                Rp = 89.8; //mm
                                break;
                            case "20E":
                                Rp = 99.8; //mm
                                break;
                            case "22E":
                                Rp = 109.8; //mm
                                break;
                        }

                        if (CampoActual.Blocks.Count() > 0)
                        {
                            var Block=CampoActual.Blocks.First();

                            //Block.Type
                            
                        }
                        else
                        {

                        }
                        /*
                        if isfield(infoRTPlan.BeamSequence.(item), 'BlockSequence') == 1 % corroboro que haya plomo dibujado
                               s = size(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData, 1);
                        xf = infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(1:2:s);
                        yf = infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(2:2:s);
                        xf(end + 1) = xf(1);
                        yf(end + 1) = yf(1);
                        xf = xf';
                               yf = yf';
                               xf = xf + round(sizex / 2); % centrado en x
                               yf = yf + round(sizey / 2); % centrado en y

                               w = strcmp(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockType, 'APERTURE');% si dice shielding me toma el plomo invertido por eso hago esto
                               if w == 1
                                   fl = poly2mask(xf, yf, sizey, sizex); % creo la matriz con 1 dentro de la superficie y 0 fuera
                               else
                                   fl = ~poly2mask(xf, yf, sizey, sizex);
                        end*/
                        try
                        {
                            MessageBox.Show("Electrones!! Estamos trabajando para Uds... Use el otro cálculo Independiente  ");
                            //UM_indep = Calculo_Electrones(CampoActual.TreatmentUnit.Id.ToString(), SSD, WED, App, x, y, Energia, Rp, x_calc, y_calc, z_calc);
                            button_Next.Enabled = false;
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("Ups! Algo no funcionó... ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("SSD fuera de los límites. Programa válido para SSD entre 100cm y 140cm");
                    }


                }

                double SesgoPerc = 100 * (CampoActual.Meterset.Value - UM_indep) / CampoActual.Meterset.Value;
                double SesgoAbs = CampoActual.Meterset.Value - UM_indep;
                //Escribo el resultado en el form

                double tol_porciento = 4; // 4 % de tolerancia en dosis por campo
                double tol_dosis = 2;     //2 cGy de tolerancia en dosis por campo

                label_UMindep.Text = UM_indep.ToString("0.00");

                if (Math.Abs(SesgoPerc) <= tol_porciento)
                {
                    label_SesgoPerc.Text = SesgoPerc.ToString("0.00");
                    label_SesgoPerc.ForeColor = System.Drawing.Color.Green;
                    label_SesgoD.Text = SesgoAbs.ToString("0.00");
                    label_SesgoD.ForeColor = System.Drawing.Color.Green;
                }
                else if (Math.Abs(SesgoPerc) > tol_porciento && Math.Abs(SesgoAbs) <= tol_dosis)
                {
                    label_SesgoPerc.Text = SesgoPerc.ToString("0.00");
                    label_SesgoPerc.ForeColor = System.Drawing.Color.Yellow;
                    label_SesgoD.Text = SesgoAbs.ToString("0.00");
                    label_SesgoD.ForeColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    label_SesgoPerc.Text = SesgoPerc.ToString("0.00");
                    label_SesgoPerc.ForeColor = System.Drawing.Color.Red;
                    label_SesgoD.Text = SesgoAbs.ToString("0.00");
                    label_SesgoD.ForeColor = System.Drawing.Color.Red;
                }

                //var sss=CampoActual.Wedges.ToString();                  
               

                //MessageBox.Show("El punto seleccionado esta OK.");
                campoCI.ID = CampoActual.Id;
                campoCI.energia = CampoActual.EnergyModeDisplayName.ToString();
                campoCI.UMeclipse = Math.Round(CampoActual.Meterset.Value,1);
                campoCI.UMindep = Math.Round(UM_indep,1);
                campoCI.sesgoAbs = Math.Round(SesgoAbs,1);
                campoCI.sesgoRel = Math.Round(SesgoPerc,1);
                campoCI.imagen = bitmapCrop;
            }
            else
            {
                MessageBox.Show("El punto seleccionado no tiene posición física.");
            }

        }

        public float[,] Add_Matrix(float[,] MA, float[,] MB)
        {
            int Lx = MA.GetLength(0);
            int Ly = MB.GetLength(1);

            for (int xi = 0; xi < Lx; xi++)
            {
                for (int yi = 0; yi < Ly; yi++)
                {
                    MA[xi, yi] = MA[xi, yi] + MB[xi, yi];
                }

            }

            return MA;

        }
        public float[] Add_Array(float[] MA, float MB)
        {
            int Lx = MA.GetLength(0);


            for (int xi = 0; xi < Lx; xi++)
            {
                MA[xi] = MA[xi] + MB;
            }

            return MA;

        }
        public float[,] Subtract_Matrix(float[,] MA, float[,] MB)
        {
            int Lx = MA.GetLength(0);
            int Ly = MB.GetLength(1);

            for (int xi = 0; xi < Lx; xi++)
            {
                for (int yi = 0; yi < Ly; yi++)
                {
                    MA[xi, yi] = MA[xi, yi] - MB[xi, yi];
                }

            }

            return MA;

        }
        public float[,] Fill_Matrix(float[,] Matriz, float numero)
        {
            int Lx = Matriz.GetLength(0);
            int Ly = Matriz.GetLength(1);

            for (int xi = 0; xi < Lx; xi++)
            {
                for (int yi = 0; yi < Ly; yi++)
                {
                    Matriz[xi, yi] = numero;
                }

            }

            return Matriz;

        }
        public float[] Fill_Array(float[] Matriz, float numero)
        {
            int Lx = Matriz.GetLength(0);


            for (int xi = 0; xi < Lx; xi++)
            {
                Matriz[xi] = numero;
            }

            return Matriz;

        }
        public float[,] Multiply_Matrix(float[,] Matriz, float numero)
        {
            int Lx = Matriz.GetLength(0);
            int Ly = Matriz.GetLength(1);

            for (int xi = 0; xi < Lx; xi++)
            {
                for (int yi = 0; yi < Ly; yi++)
                {
                    Matriz[xi, yi] = Matriz[xi, yi] * numero;
                }

            }

            return Matriz;

        }
        public double[,] Cross(double[,] MatrizA, double[,] MatrizB)
        {
            int LxA = MatrizA.GetLength(0);
            int LyA = MatrizA.GetLength(1);
            int LxB = MatrizB.GetLength(0);
            int LyB = MatrizB.GetLength(1);
            double[,] MatrizC = new double[LxA, LyB];
            if (LxA == LyB && LyA == LxB)
            {
                for (int xi = 0; xi < LxA; xi++)
                {
                    for (int yi = 0; yi < LyB; yi++)
                    {
                        for (int k = 0; k < LyA; k++)
                        {
                            MatrizC[xi, yi] += MatrizA[xi, k] * MatrizB[k, yi];
                        }

                    }

                }


            }
            return MatrizC;
        }
        public double[] CrossV(double[,] MatrizA, double[] Vector)
        {
            int LxA = MatrizA.GetLength(0);
            int LyA = MatrizA.GetLength(1);
            int LxB = Vector.GetLength(0);
            int LyB = 1;
            double[] MatrizC = new double[LxA];
            if (LyA == LxB)
            {
                for (int xi = 0; xi < LxA; xi++)
                {
                    for (int yi = 0; yi < LyB; yi++)
                    {
                        for (int k = 0; k < LyA; k++)
                        {
                            MatrizC[xi] += MatrizA[xi, k] * Vector[k];
                        }

                    }

                }


            }
            return MatrizC;
        }

        public float[,] Multiply_subMatrix(float[,] Matriz, float numero, int x1, int x2, int y1, int y2)
        {
            int Lx = Matriz.GetLength(0);
            int Ly = Matriz.GetLength(1);
            float[,] SUB = new float[x2 - x1 + 1, y2 - y1 + 1];
            for (int xi = x1; xi < x2; xi++)
            {
                for (int yi = y1; yi < y2; yi++)
                {
                    SUB[xi - x1, yi - y1] = Matriz[xi, yi] * numero;
                }

            }

            return SUB;

        }
        public float[] Multiply_Array(float[] Matriz, float numero)
        {
            int Lx = Matriz.GetLength(0);

            for (int xi = 0; xi < Lx; xi++)
            {
                Matriz[xi] = Matriz[xi] * numero;


            }

            return Matriz;

        }
        public double Calculo_Fotones(double[] poliTPR, string Energia, double WED, double DFP, double X1, double X2, double Y1, double Y2, float[,] tr_MLC, double dmax, double TPR20_10, double Dose, double FieldDose, VVector Iso, VVector RefPos, double x_calc, double y_calc, double z_calc)
        {
            double UM_Indep = 0;
            double DFI = 1000;
            // Tablas de Golden Data para Fotontes

            /* Factores Sp vs indice de calidad del haz sacado del paper "A table of phantom scatter factors of photon beams as a function
                      * of the quality index and field size" de Storchi y Van Gasteren Estos factores Sp estan medidos a 10 cm de prof 
                      * Para los TC 2 y 3 cm se extrapolo con una cubica los Sp de los TC entre 4 y 10 cm*/

            Sp = new double[24, 11] {  { 0.787,0.815,0.836,0.855,0.869,0.881,0.888,0.893,0.893,0.893,0.890},
                                     { 0.824,0.846,0.864,0.879,0.891,0.901,0.907,0.913,0.914,0.915,0.915},
                                     { 0.858,0.875,0.889,0.901,0.911,0.919,0.925,0.930,0.932,0.934,0.934},
                                     { 0.890,0.902,0.912,0.921,0.929,0.936,0.942,0.947,0.951,0.954,0.957},
                                     { 0.917,0.926,0.934,0.941,0.948,0.953,0.958,0.962,0.965,0.968,0.970},
                                     { 0.942,0.948,0.953,0.958,0.962,0.966,0.970,0.973,0.976,0.979,0.981},
                                     { 0.965,0.968,0.971,0.973,0.976,0.978,0.980,0.983,0.985,0.987,0.989},
                                     { 0.984,0.985,0.986,0.987,0.989,0.990,0.991,0.992,0.993,0.994,0.994},
                                     { 1.000,1.000,1.000,1.000,1.000,1.000,1.000,1.000,1.000,1.000,1.000},
                                     { 1.030,1.027,1.024,1.021,1.019,1.016,1.014,1.013,1.011,1.010,1.009},
                                     { 1.052,1.047,1.042,1.038,1.034,1.030,1.026,1.023,1.020,1.018,1.016},
                                     { 1.067,1.061,1.056,1.051,1.047,1.042,1.037,1.033,1.029,1.025,1.021},
                                     { 1.079,1.074,1.069,1.063,1.058,1.053,1.047,1.042,1.036,1.030,1.024},
                                     { 1.099,1.091,1.083,1.076,1.068,1.061,1.054,1.048,1.041,1.035,1.028},
                                     { 1.108,1.100,1.093,1.085,1.077,1.070,1.062,1.055,1.047,1.039,1.032},
                                     { 1.115,1.108,1.101,1.093,1.085,1.077,1.069,1.061,1.052,1.044,1.035},
                                     { 1.121,1.115,1.107,1.100,1.092,1.084,1.075,1.066,1.057,1.047,1.037},
                                     { 1.128,1.121,1.114,1.106,1.098,1.089,1.080,1.070,1.060,1.050,1.040},
                                     { 1.134,1.127,1.120,1.112,1.103,1.094,1.084,1.074,1.064,1.053,1.042},
                                     { 1.141,1.134,1.125,1.117,1.108,1.098,1.088,1.078,1.067,1.056,1.044},
                                     { 1.148,1.140,1.131,1.122,1.112,1.102,1.091,1.081,1.070,1.058,1.047},
                                     { 1.156,1.146,1.137,1.126,1.116,1.105,1.094,1.083,1.072,1.061,1.049},
                                     { 1.164,1.153,1.142,1.131,1.120,1.108,1.097,1.086,1.074,1.063,1.051},
                                     { 1.172,1.160,1.147,1.135,1.123,1.111,1.099,1.088,1.076,1.065,1.054} };
            TC_Sp = new double[24] { 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400 };
            IC_Sp = new double[] { 0.6, 0.62, 0.64, 0.66, 0.68, 0.70, 0.72, 0.74, 0.76, 0.78, 0.80 };

            // Tabla del BJR Sup 25
            TC_eff = new double[36, 35]{{ 5,7,9,10,11,11,12,12,12,12,13,13,13,13,13,13,13,13,13,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14},
                        { 7,10,14,16,17,18,19,20,21,21,22,22,22,22,23,23,23,23,23,23,23,24,24,24,24,24,24,24,24,24,24,24,24,24,24},
                        { 9,14,20,24,27,29,31,33,34,35,36,37,37,38,38,39,39,39,39,40,40,40,40,41,41,41,41,41,41,41,41,41,41,41,41},
                        { 10,16,24,30,34,38,41,43,45,46,48,49,50,51,51,52,53,53,53,54,54,55,55,55,55,56,56,56,56,56,56,56,56,56,56},
                        { 11,17,27,34,40,45,48,51,54,56,58,60,61,62,63,64,65,65,66,66,67,68,68,69,69,69,69,69,70,70,70,70,70,70,70},
                        { 11,18,29,38,45,50,55,58,62,65,67,69,71,72,74,75,76,77,78,78,79,80,81,81,82,82,82,82,82,83,83,83,83,83,83},
                        { 12,19,31,41,48,55,60,65,69,72,75,78,80,82,84,85,86,88,89,89,90,91,92,93,94,94,94,95,95,95,95,95,95,95,95},
                        { 12,20,33,43,51,58,65,70,75,79,82,85,88,91,93,95,96,98,99,100,101,102,104,105,105,106,106,107,107,107,107,107,107,108,108},
                        { 12,21,34,45,54,62,69,75,80,85,89,93,96,99,101,103,105,107,109,110,111,113,114,116,117,117,118,118,118,119,119,119,119,119,119},
                        { 12,21,35,46,56,65,72,79,85,90,95,99,103,106,109,112,114,116,118,119,121,123,125,126,127,128,129,129,130,130,130,131,131,131,131},
                        { 13,22,36,48,58,67,75,82,89,95,100,105,109,113,116,119,122,124,126,128,130,133,135,137,138,139,140,140,141,141,141,142,142,142,142},
                        { 13,22,37,49,60,69,78,85,93,99,105,110,115,119,123,126,129,132,135,137,139,142,145,147,148,150,150,151,152,152,152,153,153,153,154},
                        { 13,22,37,50,61,71,80,88,96,103,109,115,120,125,129,133,137,140,143,145,147,150,154,157,158,160,160,162,162,163,163,164,164,164,165},
                        { 13,22,38,51,62,72,82,91,99,106,113,119,125,130,135,139,143,147,150,153,155,160,163,166,168,170,171,172,173,174,174,175,175,175,176},
                        { 13,23,38,51,63,74,84,93,101,109,116,123,129,135,140,145,149,153,157,160,163,168,172,175,178,180,181,182,183,184,185,185,186,186,186},
                        { 13,23,39,52,64,75,85,95,103,112,119,126,133,139,145,150,155,159,163,167,170,176,180,184,187,189,191,192,193,194,195,196,197,197,197},
                        { 13,23,39,53,65,76,86,96,105,114,122,129,137,143,149,155,160,165,169,173,177,183,188,193,196,199,201,202,204,205,205,207,207,208,208},
                        { 13,23,39,53,65,77,88,98,107,116,124,132,140,147,153,159,165,170,175,179,183,190,196,201,205,208,210,212,213,215,215,217,218,218,218},
                        { 13,23,39,53,66,78,89,99,109,118,126,135,143,150,157,163,169,175,180,185,189,197,203,209,213,216,219,221,223,224,226,227,228,229,229},
                        { 14,23,40,54,66,78,89,100,110,119,128,137,145,153,160,167,173,179,185,190,195,203,210,216,221,225,228,231,233,234,235,237,238,239,239},
                        { 14,23,40,54,67,79,90,101,111,121,130,139,147,155,163,170,177,183,189,195,200,209,217,224,229,233,237,240,242,244,245,248,249,249,250},
                        { 14,24,40,55,68,80,91,102,113,123,133,142,150,160,168,176,183,190,197,203,209,220,229,237,244,249,254,257,260,263,264,267,269,270,270},
                        { 14,24,40,55,68,81,92,104,114,125,135,145,154,163,172,180,188,196,203,210,217,229,240,249,257,264,269,274,278,281,283,287,289,290,291},
                        { 14,24,41,55,69,81,93,105,116,126,137,147,157,166,175,184,193,201,209,216,224,237,249,260,269,277,284,290,294,298,301,306,309,310,311},
                        { 14,24,41,55,69,82,94,105,117,127,138,148,158,168,178,187,196,205,213,221,229,244,257,269,280,289,297,304,310,314,318,325,328,330,331},
                        { 14,24,41,56,69,82,94,106,117,128,139,150,160,170,180,189,199,208,216,225,233,249,264,277,289,300,309,317,324,330,335,343,347,350,351},
                        { 14,24,41,56,69,82,94,106,118,129,140,150,160,171,181,191,201,210,219,228,237,254,269,284,297,309,320,329,337,344,350,360,366,370,371},
                        { 14,24,41,56,69,82,95,107,118,129,140,151,162,172,182,192,202,212,221,231,240,257,274,290,304,317,329,340,349,357,364,377,384,389,391},
                        { 14,24,41,56,70,82,95,107,118,130,141,152,162,173,183,193,204,213,223,233,242,260,278,294,310,324,337,349,360,369,377,393,402,407,410},
                        { 14,24,41,56,70,83,95,107,119,130,141,152,163,174,184,194,205,215,224,234,244,263,281,298,314,330,344,357,369,380,389,407,412,426,430},
                        { 14,24,41,56,70,83,95,107,119,130,141,152,163,174,185,195,205,215,226,235,245,264,283,301,318,335,350,364,377,389,400,421,435,443,448},
                        { 14,24,41,56,70,83,95,107,119,131,142,153,164,175,185,196,207,217,227,237,248,267,287,306,325,343,360,377,393,407,421,450,471,485,494},
                        { 14,24,41,56,70,83,95,107,119,131,142,153,164,175,186,197,207,218,228,238,249,269,289,309,328,347,366,384,402,412,435,471,500,521,535},
                        { 14,24,41,56,70,83,95,108,119,131,142,153,164,175,186,197,208,218,229,239,249,270,290,310,330,350,370,389,407,426,443,485,521,550,571},
                        { 14,24,41,56,70,83,95,108,119,131,142,154,165,176,186,197,208,218,229,239,250,270,291,311,331,351,371,391,410,430,448,494,535,571,600},
                        { 14,24,41,56,70,83,95,108,119,131,142,154,165,176,186,197,208,219,229,240,250,271,292,312,333,353,373,394,414,434,454,505,555,605,655}};
            Jaw_Short = new double[35] { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 450, 500, 550, 600 };
            Jaw_Long = new double[36] { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 450, 500, 550, 600, 10000 };

            // Defino PDD para 6X en equipo UNIQUE, 2100C y 2300CD segun Golden Data
            PDD_GD_6X = new double[63, 12] { { 41.6,42.4,43.9,45.7,47.3,49.2,51.4,55.5,59.0,62.1,64.3,66.2},
                                            { 70.0,70.6,71.4,72.6,73.7,75.2,76.3,79.2,81.7,83.5,84.7,85.9},
                                            { 95.2,95.2,95.4,95.9,96.2,96.5,97.0,97.8,98.4,98.7,98.8,98.8},
                                            { 99.9,99.7,99.8,99.9,99.9,100.0,100.0,100.0,100.0,99.9,99.8,99.9},
                                            { 99.0,99.1,99.1,99.0,98.9,98.9,99.0,99.0,98.8,98.6,98.7,98.8},
                                            { 96.6,96.8,97.1,97.2,97.0,97.0,97.2,97.4,97.1,96.9,96.9,97.1},
                                            { 94.0,94.3,94.8,94.9,94.9,95.1,95.1,95.5,95.4,95.3,95.2,95.5},
                                            { 91.3,91.8,92.4,92.7,92.9,93.0,93.3,93.7,93.6,93.6,93.5,93.8},
                                            { 88.7,89.3,90.0,90.6,90.8,90.9,91.4,91.7,91.7,91.8,91.9,91.9},
                                            { 86.0,86.9,87.7,88.3,88.7,89.0,89.4,89.9,89.9,90.0,90.1,90.1},
                                            { 83.4,84.5,85.5,86.2,86.6,87.0,87.6,88.1,88.2,88.4,88.4,88.6},
                                            { 81.0,82.0,83.2,84.0,84.4,85.0,85.7,86.2,86.5,86.7,86.7,86.9},
                                            { 78.4,79.6,80.8,81.9,82.3,82.9,83.7,84.3,84.6,84.9,84.9,85.3},
                                            { 75.9,77.3,78.6,79.7,80.2,81.0,81.8,82.4,82.7,83.1,83.2,83.4},
                                            { 73.6,74.8,76.4,77.6,78.1,78.8,79.9,80.4,81.0,81.3,81.3,81.7},
                                            { 71.2,72.6,74.2,75.5,76.1,76.8,77.9,78.7,79.2,79.6,79.4,80.0},
                                            { 69.0,70.3,72.1,73.4,74.2,74.9,76.0,76.9,77.4,77.9,77.6,78.3},
                                            { 66.9,68.2,70.1,71.4,72.2,73.2,74.3,75.3,75.8,76.4,76.1,76.8},
                                            { 64.8,66.0,68.1,69.5,70.3,71.4,72.4,73.5,74.1,74.6,74.4,75.1},
                                            { 62.8,64.1,66.1,67.5,68.4,69.6,70.8,71.8,72.5,73.0,72.9,73.5},
                                            { 60.9,62.3,64.3,65.7,66.8,67.8,69.0,70.1,70.8,71.3,71.3,71.9},
                                            { 59.3,60.4,62.5,63.9,65.0,66.1,67.2,68.5,69.3,69.8,69.9,70.3},
                                            { 57.2,58.6,60.7,62.2,63.3,64.4,65.7,66.9,67.7,68.3,68.3,68.8},
                                            { 55.4,56.7,58.9,60.4,61.5,62.7,64.0,65.2,66.2,66.7,66.7,67.2},
                                            { 53.8,55.0,57.1,58.7,59.8,61.0,62.4,63.6,64.6,65.1,65.4,65.7},
                                            { 52.0,53.3,55.5,57.0,58.1,59.3,60.8,62.2,63.1,63.7,63.7,64.3},
                                            { 50.3,51.7,53.8,55.5,56.5,57.8,59.3,60.8,61.6,62.3,62.4,62.9},
                                            { 48.9,50.3,52.3,53.9,55.0,56.3,57.8,59.3,60.1,60.9,61.1,61.5},
                                            { 47.6,48.7,50.7,52.4,53.6,54.8,56.3,57.8,58.7,59.6,59.7,60.2},
                                            { 46.0,47.2,49.2,51.0,52.1,53.3,54.9,56.5,57.3,58.3,58.4,59.0},
                                            { 44.6,45.8,47.9,49.6,50.6,52.0,53.6,55.1,56.0,56.8,57.1,57.6},
                                            { 43.2,44.4,46.5,48.1,49.4,50.6,52.1,53.8,54.7,55.6,55.8,56.3},
                                            { 41.9,43.0,45.0,46.8,48.0,49.2,50.8,52.4,53.4,54.3,54.5,55.0},
                                            { 40.6,41.7,43.7,45.4,46.7,47.9,49.5,51.1,52.1,53.0,53.2,53.8},
                                            { 39.3,40.4,42.4,44.1,45.5,46.6,48.2,49.8,50.9,51.7,51.9,52.6},
                                            { 38.1,39.2,41.1,42.8,44.2,45.3,46.9,48.5,49.6,50.5,50.6,51.4},
                                            { 37.0,38.0,40.0,41.7,42.8,44.1,45.7,47.3,48.5,49.4,49.5,50.3},
                                            { 35.9,37.0,38.8,40.5,41.7,42.9,44.5,46.1,47.3,48.2,48.5,49.1},
                                            { 34.7,35.9,37.7,39.4,40.6,41.8,43.4,45.0,46.2,47.0,47.5,47.9},
                                            { 33.7,34.9,36.6,38.3,39.5,40.6,42.2,43.8,45.1,45.9,46.4,46.8},
                                            { 32.7,33.8,35.5,37.1,38.4,39.6,41.2,42.8,44.1,44.8,45.3,45.8},
                                            { 31.7,32.8,34.4,36.0,37.4,38.5,40.1,41.6,43.0,43.8,44.3,44.7},
                                            { 30.7,31.8,33.5,35.0,36.4,37.5,39.1,40.6,41.9,42.7,43.3,43.7},
                                            { 29.8,30.9,32.5,34.0,35.3,36.5,38.0,39.6,40.8,41.7,42.2,42.7},
                                            { 29.0,29.9,31.5,33.0,34.3,35.5,36.9,38.5,39.9,40.7,41.2,41.7},
                                            { 28.1,29.1,30.6,32.1,33.3,34.5,35.9,37.6,38.9,39.7,40.2,40.7},
                                            { 27.2,28.2,29.8,31.2,32.4,33.5,34.9,36.7,37.9,38.7,39.3,39.7},
                                            { 26.4,27.3,28.9,30.3,31.5,32.6,34.0,35.8,37.0,37.8,38.4,38.8},
                                            { 25.7,26.5,28.0,29.5,30.6,31.7,33.2,34.9,36.1,37.0,37.5,37.8},
                                            { 24.9,25.7,27.2,28.7,29.8,30.9,32.2,34.0,35.2,36.1,36.7,37.1},
                                            { 24.2,25.1,26.5,27.8,28.9,30.0,31.4,33.2,34.4,35.2,35.8,36.3},
                                            { 23.5,24.3,25.7,27.0,28.2,29.2,30.6,32.4,33.5,34.3,34.8,35.3},
                                            { 22.8,23.6,25.0,26.3,27.4,28.4,29.8,31.5,32.6,33.5,34.1,34.5},
                                            { 22.1,22.9,24.4,25.6,26.6,27.7,29.0,30.7,31.8,32.7,33.2,33.7},
                                            { 21.5,22.2,23.7,24.9,25.9,26.9,28.3,29.9,31.0,32.0,32.3,32.8},
                                            { 20.9,21.6,23.0,24.1,25.2,26.2,27.6,29.1,30.2,31.2,31.6,32.0},
                                            { 20.3,21.0,22.3,23.5,24.5,25.5,26.8,28.4,29.5,30.4,30.9,31.2},
                                            { 19.6,20.4,21.7,22.8,23.8,24.9,26.1,27.7,28.8,29.7,30.1,30.4},
                                            { 19.1,19.8,21.1,22.2,23.2,24.3,25.5,26.9,28.1,28.9,29.4,29.6},
                                            { 18.6,19.2,20.6,21.6,22.5,23.6,24.8,26.3,27.4,28.1,28.6,29.0},
                                            { 18.1,18.7,20.0,21.0,22.0,23.0,24.1,25.6,26.7,27.4,27.9,28.3},
                                            { 17.5,18.1,19.4,20.4,21.3,22.4,23.5,24.9,26.1,26.7,27.2,27.6},
                                            { 17.0,17.7,18.9,19.8,20.8,21.8,22.9,24.3,25.4,26.1,26.6,27.0}};
            PDD_GD_10X = new double[63, 12] {
                                            { 25.9,26.6,28.8,31.4,33.8,36.2,39.6,44.6,48.9,52.5,55.6,60.7},
                                            { 52.7,53.1,54.8,56.8,59.4,61.3,64.3,68.5,71.6,74.3,76.7,80.2},
                                            { 81.6,81.6,82.8,84.0,85.5,86.6,88.0,90.2,91.4,92.5,93.2,94.4},
                                            { 94.0,94.1,94.4,95.0,95.7,96.2,96.8,97.8,98.1,98.5,98.5,99.0},
                                            { 98.8,98.6,99.1,99.1,99.3,99.5,99.6,99.9,100.0,99.9,99.9,100.0},
                                            { 99.9,99.8,100.0,99.9,99.9,99.7,99.6,99.6,99.5,99.5,99.5,99.5},
                                            { 99.0,99.3,99.3,99.0,99.0,98.7,98.6,98.5,98.3,98.4,98.3,98.3},
                                            { 97.3,97.7,97.8,97.3,97.3,97.2,96.9,97.0,96.8,96.9,96.8,96.8},
                                            { 95.1,95.5,95.8,95.5,95.6,95.4,95.2,95.3,95.1,95.3,95.2,95.2},
                                            { 92.9,93.4,93.6,93.5,93.7,93.5,93.3,93.5,93.4,93.5,93.5,93.5},
                                            { 90.7,91.4,91.7,91.6,91.9,91.7,91.7,91.8,91.8,92.0,92.0,92.0},
                                            { 88.6,89.2,89.7,89.6,89.8,89.8,89.9,90.0,90.1,90.3,90.3,90.4},
                                            { 86.4,86.8,87.7,87.7,87.9,88.0,88.1,88.3,88.3,88.6,88.6,88.8},
                                            { 84.2,85.0,85.6,85.7,86.0,86.1,86.1,86.6,86.7,87.0,87.0,87.1},
                                            { 82.0,82.8,83.5,83.7,84.1,84.2,84.4,84.8,85.0,85.2,85.4,85.5},
                                            { 79.9,80.6,81.5,81.8,82.1,82.4,82.6,83.1,83.4,83.7,83.8,83.8},
                                            { 77.9,78.7,79.6,79.8,80.4,80.7,80.9,81.5,81.7,82.0,82.1,82.2},
                                            { 76.2,76.8,77.7,78.0,78.7,79.0,79.3,79.9,80.1,80.5,80.6,80.8},
                                            { 74.1,74.9,75.9,76.3,76.9,77.2,77.6,78.3,78.6,79.0,79.2,79.2},
                                            { 72.3,73.0,74.1,74.6,75.2,75.7,75.9,76.6,77.0,77.4,77.6,77.9},
                                            { 70.5,71.3,72.3,72.8,73.6,73.9,74.4,75.1,75.6,76.0,76.1,76.2},
                                            { 68.8,69.7,70.7,71.3,72.0,72.3,72.8,73.7,74.2,74.6,74.8,74.9},
                                            { 67.2,67.9,69.1,69.5,70.3,70.7,71.2,72.2,72.6,73.1,73.3,73.5},
                                            { 65.5,66.1,67.3,67.9,68.8,69.2,69.7,70.7,71.0,71.7,71.9,72.2},
                                            { 63.7,64.5,65.5,66.2,67.2,67.4,68.1,69.1,69.7,70.2,70.4,70.6},
                                            { 62.2,62.9,64.1,64.8,65.6,66.0,66.7,67.7,68.2,68.8,69.0,69.3},
                                            { 60.6,61.3,62.5,63.2,64.0,64.5,65.1,66.3,66.8,67.4,67.7,67.9},
                                            { 59.0,59.9,60.9,61.6,62.6,63.1,63.8,65.0,65.5,66.1,66.4,66.7},
                                            { 57.6,58.4,59.6,60.1,61.2,61.7,62.4,63.6,64.2,64.8,65.1,65.3},
                                            { 56.1,57.0,58.1,58.7,59.7,60.3,61.0,62.3,62.9,63.5,63.8,64.1},
                                            { 54.8,55.6,56.7,57.3,58.3,59.0,59.7,61.0,61.6,62.3,62.6,62.8},
                                            { 53.3,54.2,55.3,56.0,57.0,57.6,58.3,59.7,60.3,61.0,61.3,61.6},
                                            { 52.1,52.8,54.1,54.6,55.7,56.3,57.2,58.4,59.1,59.8,60.1,60.3},
                                            { 50.7,51.5,52.7,53.3,54.5,55.0,55.8,57.2,57.9,58.5,58.8,59.1},
                                            { 49.4,50.3,51.3,52.2,53.2,53.7,54.5,55.9,56.6,57.3,57.7,57.9},
                                            { 48.2,48.8,50.1,50.8,51.9,52.6,53.4,54.8,55.4,56.1,56.5,56.8},
                                            { 46.9,47.7,48.8,49.6,50.6,51.3,52.2,53.5,54.2,55.1,55.4,55.6},
                                            { 45.8,46.6,47.7,48.5,49.5,50.2,51.0,52.5,53.1,53.9,54.2,54.5},
                                            { 44.6,45.3,46.6,47.3,48.5,49.1,49.9,51.3,52.1,52.7,53.2,53.4},
                                            { 43.6,44.2,45.6,46.1,47.2,48.0,48.8,50.3,51.0,51.7,52.1,52.3},
                                            { 42.4,43.2,44.4,45.1,46.2,46.8,47.7,49.2,49.9,50.6,51.0,51.3},
                                            { 41.4,42.1,43.3,44.0,45.2,45.9,46.7,48.1,48.9,49.6,50.0,50.3},
                                            { 40.4,41.1,42.3,43.0,44.0,44.8,45.6,47.1,47.8,48.5,49.0,49.1},
                                            { 39.4,40.1,41.3,42.0,43.0,43.8,44.6,46.0,46.8,47.5,48.0,48.2},
                                            { 38.5,39.1,40.2,40.9,42.0,42.8,43.7,45.1,45.8,46.6,47.0,47.2},
                                            { 37.4,38.1,39.3,40.0,41.1,41.8,42.7,44.1,44.9,45.6,46.0,46.2},
                                            { 36.5,37.2,38.3,39.0,40.0,40.8,41.7,43.1,43.9,44.7,45.1,45.3},
                                            { 35.7,36.2,37.5,38.1,39.2,39.9,40.8,42.2,43.0,43.8,44.2,44.3},
                                            { 34.7,35.4,36.5,37.1,38.2,39.0,39.9,41.2,42.1,42.8,43.3,43.4},
                                            { 33.9,34.5,35.6,36.3,37.3,38.1,39.0,40.5,41.2,42.0,42.4,42.6},
                                            { 33.2,33.7,34.9,35.5,36.5,37.3,38.1,39.5,40.3,41.1,41.5,41.7},
                                            { 32.2,32.8,33.9,34.6,35.7,36.4,37.3,38.7,39.4,40.2,40.7,40.8},
                                            { 31.5,32.1,33.2,33.9,34.9,35.6,36.5,37.8,38.6,39.4,39.9,40.0},
                                            { 30.7,31.3,32.4,33.0,34.0,34.8,35.6,37.1,37.8,38.6,39.0,39.1},
                                            { 29.9,30.5,31.6,32.3,33.2,34.0,34.8,36.3,37.0,37.7,38.2,38.4},
                                            { 29.3,29.8,30.9,31.4,32.4,33.2,34.0,35.5,36.3,37.0,37.4,37.5},
                                            { 28.5,29.0,30.1,30.7,31.7,32.5,33.3,34.6,35.4,36.2,36.6,36.7},
                                            { 27.8,28.4,29.3,30.0,30.9,31.7,32.6,34.0,34.7,35.5,35.8,36.0},
                                            { 27.2,27.8,28.7,29.3,30.2,31.0,31.9,33.3,34.0,34.7,35.1,35.3},
                                            { 26.5,27.0,28.0,28.6,29.5,30.3,31.2,32.5,33.2,34.0,34.4,34.6},
                                            { 25.9,26.4,27.3,27.9,28.9,29.6,30.5,31.8,32.5,33.2,33.7,33.8},
                                            { 25.2,25.8,26.8,27.2,28.3,29.0,29.8,31.1,31.8,32.6,33.0,33.1},
                                            { 24.7,25.2,26.1,26.6,27.6,28.3,29.1,30.5,31.1,31.8,32.3,32.4 }};
            PDD_GD_6X_SRS = new double[79, 10] {
                                                { 39.2,37.6,50.2,50.8,51.6,52.4,53.9,55.7,56.9,59.1 },
                                                { 86.4,83.5,86.9,86.6,87.0,87.5,88.1,89.3,89.3,90.3 },
                                                {99.1,97.8,98.5,98.4,98.4,98.6,98.6,98.9,98.9,99.1 },
                                                {99.5,99.9,99.8,100.0,99.9,100.0,100.0,100.0,99.9,99.9 },
                                                {96.9,98.2,98.1,98.3,98.3,98.5,98.6,98.5,98.4,98.5 },
                                                {93.9,95.5,95.5,95.9,96.0,96.2,96.5,96.5,96.5,96.5 },
                                                {90.9,92.6,92.8,93.3,93.6,93.9,94.3,94.4,94.4,94.6 },
                                                {87.8,89.8,89.9,90.6,91.0,91.5,91.9,92.3,92.4,92.6 },
                                                {85.0,87.0,87.4,88.2,88.7,89.0,89.7,90.0,90.3,90.6 },
                                                {82.0,84.1,84.6,85.5,86.3,86.8,87.5,88.0,88.2,88.6 },
                                                {79.3,81.5,82.1,83.1,83.8,84.4,85.3,85.8,86.3,86.7 },
                                                {76.7,78.9,79.6,80.7,81.5,82.1,83.1,83.8,84.3,84.8 },
                                                {74.1,76.3,77.1,78.3,79.3,79.9,81.0,81.7,82.2,82.9 },
                                                {71.7,73.7,74.7,76.0,76.9,77.6,78.8,79.6,80.2,80.9 },
                                                {69.3,71.4,72.3,73.7,74.6,75.5,76.7,77.7,78.4,79.2 },
                                                {67.0,69.2,70.1,71.4,72.4,73.2,74.7,75.6,76.4,77.2 },
                                                {64.7,66.9,67.9,69.2,70.2,71.2,72.6,73.6,74.4,75.4 },
                                                {62.7,64.7,65.7,67.0,68.0,69.1,70.5,71.7,72.4,73.6 },
                                                {60.7,62.5,63.7,65.0,66.0,67.1,68.6,69.7,70.7,71.8 },
                                                {58.6,60.6,61.6,62.9,64.1,65.1,66.6,67.8,68.8,69.9 },
                                                {56.6,58.6,59.7,61.0,62.1,63.2,64.8,66.1,67.1,68.2 },
                                                {54.8,56.8,57.9,59.1,60.3,61.4,63.0,64.3,65.3,66.5 },
                                                {53.0,54.9,56.1,57.3,58.5,59.6,61.3,62.6,63.7,64.9 },
                                                {51.3,53.1,54.2,55.5,56.7,57.7,59.5,61.0,61.9,63.3 },
                                                {49.5,51.4,52.6,53.9,55.0,56.0,57.8,59.2,60.3,61.6 },
                                                {48.0,49.8,50.9,52.2,53.3,54.4,56.2,57.6,58.7,60.1 },
                                                {46.4,48.2,49.1,50.5,51.6,52.7,54.5,55.9,57.1,58.6 },
                                                {44.9,46.6,47.7,48.9,50.1,51.2,52.9,54.3,55.5,56.9 },
                                                {43.5,45.1,46.1,47.4,48.5,49.7,51.5,52.8,54.0,55.4 },
                                                {42.1,43.6,44.7,45.9,47.2,48.2,50.0,51.4,52.6,54.1 },
                                                {40.8,42.3,43.3,44.6,45.6,46.8,48.5,49.9,51.3,52.6 },
                                                {39.4,40.9,42.0,43.1,44.3,45.4,47.1,48.6,49.8,51.2 },
                                                {38.2,39.7,40.7,41.8,42.9,43.9,45.8,47.3,48.5,49.8 },
                                                {36.9,38.5,39.5,40.7,41.6,42.7,44.5,45.9,47.1,48.5},
                                                {35.7,37.2,38.3,39.3,40.4,41.3,43.2,44.6,45.9,47.3 },
                                                {34.7,36.0,37.0,38.2,39.2,40.1,41.8,43.4,44.6,46.1 },
                                                {33.6,34.9,35.8,36.9,38.0,39.0,40.8,42.1,43.3,44.8 },
                                                {32.6,33.8,34.8,35.8,36.8,37.8,39.5,40.9,42.1,43.6 },
                                                {31.5,32.7,33.6,34.8,35.8,36.7,38.3,39.8,41.0,42.4 },
                                                {30.6,31.8,32.7,33.7,34.7,35.6,37.2,38.8,39.8,41.3 },
                                                {29.7,30.8,31.6,32.7,33.6,34.6,36.2,37.6,38.8,40.3 },
                                                {28.7,29.9,30.7,31.7,32.7,33.6,35.2,36.6,37.7,39.2},
                                                {27.9,29.0,29.8,30.8,31.7,32.6,34.2,35.5,36.7,38.1 },
                                                {26.9,28.1,28.9,29.8,30.8,31.6,33.2,34.5,35.7,37.1 },
                                                {26.2,27.2,28.0,29.0,29.8,30.7,32.3,33.6,34.7,36.1 },
                                                {25.3,26.4,27.2,28.1,29.0,29.7,31.4,32.6,33.7,35.2 },
                                                {24.6,25.6,26.3,27.2,28.1,28.9,30.4,31.7,32.9,34.2 },
                                                {23.9,24.8,25.6,26.4,27.3,28.1,29.5,30.8,31.9,33.3 },
                                                {23.1,24.1,24.8,25.7,26.5,27.2,28.7,29.9,31.1,32.4 },
                                                {22.4,23.3,24.1,24.9,25.7,26.4,27.9,29.1,30.1,31.6 },
                                                {21.7,22.7,23.3,24.2,24.9,25.7,27.1,28.3,29.4,30.7 },
                                                {21.1,22.0,22.7,23.5,24.2,25.0,26.4,27.6,28.5,29.9 },
                                                {20.5,21.3,22.0,22.8,23.6,24.3,25.6,26.8,27.8,29.1 },
                                                {19.8,20.7,21.3,22.1,22.8,23.5,24.9,26.0,27.0,28.4 },
                                                {19.2,20.0,20.7,21.5,22.2,22.9,24.2,25.3,26.3,27.6 },
                                                {18.7,19.5,20.0,20.8,21.5,22.2,23.5,24.6,25.6,26.8 },
                                                {18.1,18.9,19.5,20.2,20.9,21.5,22.8,23.9,24.9,26.1 },
                                                {17.6,18.3,18.9,19.6,20.3,20.9,22.2,23.2,24.2,25.4 },
                                                {17.1,17.8,18.4,19.0,19.7,20.4,21.6,22.6,23.6,24.8 },
                                                {16.6,17.3,17.8,18.5,19.1,19.8,20.9,22.0,22.9,24.2 },
                                                {16.1,16.8,17.3,18.0,18.6,19.2,20.4,21.4,22.3,23.5 },
                                                {15.6,16.3,16.8,17.5,18.1,18.7,19.8,20.8,21.8,22.8 },
                                                {15.2,15.8,16.3,17.0,17.6,18.1,19.2,20.2,21.1,22.3 },
                                                {14.8,15.3,15.9,16.5,17.0,17.7,18.7,19.7,20.5,21.7 },
                                                {14.3,14.9,15.4,16.0,16.6,17.1,18.2,19.2,20.0,21.1 },
                                                {13.9,14.5,15.0,15.6,16.1,16.6,17.7,18.6,19.5,20.5 },
                                                {13.5,14.0,14.5,15.1,15.6,16.2,17.2,18.1,18.9,20.0 },
                                                {13.1,13.7,14.1,14.7,15.2,15.7,16.8,17.6,18.4,19.5 },
                                                {12.7,13.3,13.7,14.3,14.7,15.2,16.3,17.1,18.0,18.9 },
                                                {12.3,12.9,13.3,13.9,14.3,14.8,15.8,16.7,17.4,18.4 },
                                                {12.0,12.5,12.9,13.5,14.0,14.4,15.4,16.3,17.0,18.0 },
                                                {11.8,12.2,12.6,13.1,13.5,14.1,14.9,15.8,16.5,17.5 },
                                                {11.4,11.9,12.2,12.7,13.2,13.6,14.6,15.4,16.1,17.0 },
                                                {11.2,11.6,11.9,12.3,12.8,13.2,14.2,15.0,15.6,16.7 },
                                                {11.0,11.4,11.5,12.0,12.4,13.0,13.8,14.6,15.3,16.1 },
                                                {10.8,11.1,11.3,11.7,12.1,12.6,13.5,14.2,14.9,15.8 },
                                                {10.3,10.7,11.0,11.5,12.0,12.4,13.2,14.0,14.6,15.5 },
                                                {10.0,10.4,10.8,11.2,11.6,12.0,12.8,13.6,14.2,15.0 },
                                                {9.8,10.2,10.5,10.9,11.3,11.7,12.5,13.2,13.9,14.8 }};
            TC_PDD_GD_STD = new double[12] { 30, 40, 60, 80, 100, 120, 150, 200, 250, 300, 350, 400 };
            prof_PDD_GD_STD = new double[63] { -1.5, 3.5, 8.5, 13.5, 18.5, 23.5, 28.5, 33.5, 38.50, 43.5, 48.5, 53.5, 58.5, 63.5, 68.5, 73.5, 78.5, 83.5, 88.50, 93.50, 98.50, 103.5, 108.50, 113.500000000000, 118.500000000000, 123.500000000000, 128.500000000000, 133.500000000000, 138.500000000000, 143.500000000000, 148.500000000000, 153.500000000000, 158.500000000000, 163.500000000000, 168.500000000000, 173.500000000000, 178.500000000000, 183.500000000000, 188.500000000000, 193.500000000000, 198.500000000000, 203.500000000000, 208.500000000000, 213.500000000000, 218.500000000000, 223.500000000000, 228.5, 233.5, 238.5, 243.5, 248.5, 253.5, 258.5, 263.5, 268.5, 273.5, 278.5, 283.5, 288.5, 293.5, 298.5, 303.5, 308.5 };
            TC_PDD_GD_SRS = new double[10] { 10, 20, 30, 40, 50, 60, 80, 100, 100, 150 };
            prof_PDD_GD_SRS = new double[79] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120, 125, 130, 135, 140, 145, 150, 155, 160, 165, 170, 175, 180, 185, 190, 195, 200, 205, 210, 215, 220, 225, 230, 235, 240, 245, 250, 255, 260, 265, 270, 275, 280, 285, 290, 295, 300, 305, 310, 315, 320, 325, 330, 335, 340, 345, 350, 355, 360, 365, 370, 375, 380, 385, 390 };
            //Los factores de campo Scp estan medidos a DFP 95 cm y prof 5 cm
            Scp_GD_6X = new double[38, 19] {
                                        {0.879,0.890,0.897,0.902,0.906,0.909,0.912,0.913,0.915,0.916,0.917,0.918,0.918,0.920,0.921,0.923,0.923,0.925,0.925},
                                        {0.892,0.906,0.916,0.922,0.927,0.931,0.934,0.936,0.938,0.940,0.941,0.942,0.944,0.945,0.947,0.949,0.950,0.951,0.951 },
                                        { 0.901,0.917,0.929,0.937,0.942,0.947,0.951,0.954,0.956,0.958,0.960,0.961,0.962,0.964,0.967,0.969,0.970,0.971,0.972},
                                        {0.908,0.926,0.939,0.947,0.954,0.959,0.964,0.967,0.970,0.972,0.974,0.976,0.977,0.980,0.982,0.985,0.987,0.988,0.989},
                                        {0.913,0.932,0.946,0.956,0.963,0.969,0.974,0.978,0.981,0.983,0.986,0.988,0.989,0.992,0.995,0.998,1.000,1.001,1.003},
                                        {0.917,0.937,0.952,0.963,0.971,0.977,0.982,0.986,0.990,0.993,0.995,0.997,0.999,1.002,1.006,1.009,1.012,1.013,1.014},
                                        {0.921,0.942,0.957,0.968,0.977,0.984,0.989,0.994,0.998,1.001,1.003,1.006,1.008,1.011,1.015,1.019,1.022,1.023,1.025},
                                        {0.924,0.946,0.961,0.973,0.982,0.989,0.995,1.000,1.004,1.007,1.010,1.013,1.015,1.019,1.023,1.027,1.030,1.032,1.034},
                                        {0.926,0.949,0.965,0.977,0.987,0.994,1.000,1.005,1.010,1.013,1.016,1.019,1.021,1.025,1.029,1.034,1.038,1.040,1.042},
                                        {0.929,0.951,0.968,0.981,0.991,0.998,1.005,1.010,1.014,1.018,1.021,1.024,1.027,1.031,1.035,1.041,1.044,1.047,1.049},
                                        {0.931,0.954,0.971,0.984,0.994,1.002,1.009,1.014,1.019,1.023,1.026,1.029,1.032,1.036,1.041,1.046,1.050,1.053,1.055},
                                        {0.932,0.956,0.973,0.986,0.997,1.005,1.012,1.018,1.023,1.027,1.030,1.033,1.036,1.040,1.045,1.052,1.056,1.059,1.061},
                                        {0.934,0.958,0.975,0.989,1.000,1.008,1.015,1.021,1.026,1.030,1.034,1.037,1.040,1.044,1.050,1.056,1.060,1.064,1.066},
                                        {0.935,0.959,0.977,0.991,1.002,1.011,1.018,1.024,1.029,1.033,1.037,1.040,1.043,1.048,1.054,1.060,1.065,1.068,1.071},
                                        {0.936,0.961,0.979,0.993,1.004,1.013,1.021,1.027,1.032,1.036,1.040,1.044,1.046,1.051,1.057,1.064,1.069,1.073,1.075},
                                        {0.938,0.962,0.981,0.995,1.006,1.015,1.023,1.029,1.034,1.039,1.043,1.046,1.049,1.055,1.061,1.068,1.073,1.077,1.079},
                                        {0.939,0.964,0.982,0.996,1.008,1.017,1.025,1.031,1.037,1.041,1.045,1.049,1.052,1.057,1.064,1.071,1.076,1.080,1.083},
                                        {0.940,0.965,0.984,0.998,1.010,1.019,1.027,1.033,1.039,1.044,1.048,1.051,1.055,1.060,1.066,1.074,1.079,1.083,1.086},
                                        {0.940,0.966,0.985,0.999,1.011,1.020,1.028,1.035,1.041,1.046,1.050,1.053,1.057,1.062,1.069,1.077,1.082,1.087,1.090},
                                        {0.941,0.967,0.986,1.001,1.012,1.022,1.030,1.037,1.042,1.047,1.052,1.056,1.059,1.065,1.071,1.080,1.085,1.089,1.093},
                                        {0.942,0.968,0.987,1.002,1.014,1.023,1.032,1.038,1.044,1.049,1.054,1.057,1.061,1.067,1.074,1.082,1.088,1.092,1.095},
                                        {0.943,0.969,0.988,1.003,1.015,1.025,1.033,1.040,1.046,1.051,1.055,1.059,1.063,1.069,1.076,1.084,1.090,1.095,1.098},
                                        {0.943,0.969,0.989,1.004,1.016,1.026,1.034,1.041,1.047,1.052,1.057,1.061,1.064,1.070,1.078,1.086,1.092,1.097,1.101},
                                        {0.944,0.970,0.990,1.005,1.017,1.027,1.035,1.042,1.048,1.054,1.058,1.062,1.066,1.072,1.079,1.088,1.095,1.099,1.103},
                                        {0.945,0.971,0.990,1.006,1.018,1.028,1.036,1.044,1.050,1.055,1.060,1.064,1.068,1.074,1.081,1.090,1.097,1.101,1.105},
                                        {0.945,0.971,0.991,1.007,1.019,1.029,1.038,1.045,1.051,1.056,1.061,1.065,1.069,1.075,1.083,1.092,1.099,1.103,1.107},
                                        {0.946,0.972,0.992,1.007,1.020,1.030,1.039,1.046,1.052,1.057,1.062,1.066,1.070,1.077,1.084,1.094,1.100,1.105,1.109},
                                        {0.946,0.973,0.993,1.008,1.021,1.031,1.039,1.047,1.053,1.059,1.063,1.068,1.072,1.078,1.086,1.095,1.102,1.107,1.111},
                                        {0.947,0.973,0.993,1.009,1.021,1.032,1.040,1.048,1.054,1.060,1.064,1.069,1.073,1.079,1.087,1.097,1.104,1.109,1.113},
                                        {0.947,0.974,0.994,1.009,1.022,1.032,1.041,1.049,1.055,1.061,1.066,1.070,1.074,1.081,1.089,1.098,1.105,1.110,1.115},
                                        {0.948,0.974,0.994,1.010,1.023,1.033,1.042,1.049,1.056,1.062,1.067,1.071,1.075,1.082,1.090,1.100,1.107,1.112,1.116},
                                        {0.948,0.975,0.995,1.011,1.023,1.034,1.043,1.050,1.057,1.062,1.067,1.072,1.076,1.083,1.091,1.101,1.108,1.114,1.118},
                                        {0.948,0.975,0.996,1.011,1.024,1.034,1.043,1.051,1.058,1.063,1.068,1.073,1.077,1.084,1.092,1.102,1.110,1.115,1.119},
                                        {0.949,0.976,0.996,1.012,1.024,1.035,1.044,1.052,1.058,1.064,1.069,1.074,1.078,1.085,1.093,1.104,1.111,1.116,1.121},
                                        {0.949,0.976,0.996,1.012,1.025,1.036,1.045,1.052,1.059,1.065,1.070,1.075,1.079,1.086,1.094,1.105,1.112,1.118,1.122},
                                        {0.949,0.976,0.997,1.013,1.026,1.036,1.045,1.053,1.060,1.066,1.071,1.075,1.080,1.087,1.095,1.106,1.113,1.119,1.123},
                                        {0.950,0.977,0.997,1.013,1.026,1.037,1.046,1.054,1.060,1.066,1.072,1.076,1.080,1.088,1.096,1.107,1.115,1.120,1.125},
                                        {0.950,0.977,0.998,1.014,1.027,1.037,1.046,1.054,1.061,1.067,1.072,1.077,1.081,1.088,1.097,1.108,1.116,1.121,1.126},

            };
            X_Scp_GD_6X = new double[19] { 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 170, 200, 250, 300, 350, 400 };
            Y_Scp_GD_6X = new double[38] { 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, 310, 320, 330, 340, 350, 360, 370, 380, 390, 400 };
            Scp_GD_10X = new double[38, 19] {
                                            { 0.884,0.894,0.902,0.906,0.910,0.913,0.915,0.916,0.917,0.919,0.919,0.920,0.921,0.922,0.923,0.924,0.924,0.925,0.925},
                                            {0.897,0.911,0.920,0.927,0.931,0.934,0.937,0.939,0.941,0.942,0.943,0.944,0.945,0.946,0.947,0.949,0.950,0.951,0.951},
                                            {0.906,0.923,0.933,0.941,0.946,0.950,0.953,0.956,0.958,0.959,0.961,0.962,0.963,0.964,0.966,0.968,0.969,0.970,0.971},
                                            {0.913,0.931,0.943,0.952,0.957,0.962,0.966,0.969,0.971,0.973,0.974,0.976,0.977,0.979,0.981,0.983,0.984,0.985,0.986},
                                            {0.918,0.938,0.951,0.960,0.966,0.971,0.975,0.979,0.981,0.983,0.985,0.987,0.988,0.990,0.993,0.995,0.997,0.998,0.999},
                                            {0.923,0.943,0.957,0.967,0.974,0.979,0.983,0.987,0.990,0.992,0.994,0.996,0.997,1.000,1.002,1.005,1.007,1.009,1.010},
                                            {0.927,0.948,0.962,0.972,0.980,0.986,0.990,0.994,0.997,1.000,1.002,1.004,1.005,1.008,1.011,1.014,1.016,1.018,1.019},
                                            {0.930,0.952,0.967,0.977,0.985,0.991,0.996,1.000,1.003,1.006,1.008,1.010,1.012,1.015,1.018,1.022,1.024,1.026,1.027},
                                            {0.932,0.955,0.970,0.981,0.989,0.996,1.001,1.005,1.008,1.011,1.014,1.016,1.018,1.021,1.024,1.028,1.031,1.033,1.034},
                                            {0.935,0.958,0.973,0.985,0.993,1.000,1.005,1.010,1.013,1.016,1.019,1.021,1.023,1.026,1.030,1.034,1.037,1.039,1.040},
                                            {0.937,0.960,0.976,0.988,0.997,1.003,1.009,1.013,1.017,1.020,1.023,1.025,1.027,1.031,1.035,1.039,1.042,1.044,1.046},
                                            {0.939,0.962,0.979,0.990,0.999,1.006,1.012,1.017,1.021,1.024,1.027,1.029,1.031,1.035,1.039,1.044,1.047,1.049,1.051},
                                            {0.940,0.964,0.981,0.993,1.002,1.009,1.015,1.020,1.024,1.027,1.030,1.033,1.035,1.039,1.043,1.048,1.051,1.054,1.056},
                                            {0.942,0.966,0.983,0.995,1.004,1.012,1.018,1.023,1.027,1.030,1.033,1.036,1.038,1.042,1.047,1.052,1.055,1.058,1.060},
                                            {0.943,0.967,0.985,0.997,1.007,1.014,1.020,1.025,1.029,1.033,1.036,1.039,1.041,1.045,1.050,1.055,1.059,1.062,1.064},
                                            {0.944,0.969,0.986,0.999,1.009,1.016,1.022,1.028,1.032,1.036,1.039,1.042,1.044,1.048,1.053,1.059,1.062,1.065,1.067},
                                            {0.945,0.970,0.988,1.000,1.010,1.018,1.024,1.030,1.034,1.038,1.041,1.044,1.047,1.051,1.056,1.062,1.066,1.069,1.071},
                                            {0.946,0.971,0.989,1.002,1.012,1.020,1.026,1.032,1.036,1.040,1.043,1.046,1.049,1.053,1.058,1.064,1.069,1.072,1.074},
                                            {0.947,0.972,0.990,1.003,1.013,1.021,1.028,1.033,1.038,1.042,1.045,1.048,1.051,1.056,1.061,1.067,1.071,1.074,1.077},
                                            {0.948,0.973,0.991,1.004,1.015,1.023,1.029,1.035,1.040,1.044,1.047,1.050,1.053,1.058,1.063,1.069,1.074,1.077,1.080},
                                            {0.949,0.974,0.992,1.006,1.016,1.024,1.031,1.037,1.041,1.046,1.049,1.052,1.055,1.060,1.065,1.072,1.076,1.080,1.082},
                                            {0.950,0.975,0.993,1.007,1.017,1.025,1.032,1.038,1.043,1.047,1.051,1.054,1.057,1.061,1.067,1.074,1.078,1.082,1.085},
                                            {0.950,0.976,0.994,1.008,1.018,1.027,1.034,1.039,1.044,1.049,1.052,1.055,1.058,1.063,1.069,1.076,1.080,1.084,1.087},
                                            {0.951,0.977,0.995,1.009,1.019,1.028,1.035,1.041,1.046,1.050,1.054,1.057,1.060,1.065,1.071,1.078,1.082,1.086,1.089},
                                            {0.952,0.978,0.996,1.010,1.020,1.029,1.036,1.042,1.047,1.051,1.055,1.058,1.061,1.066,1.072,1.079,1.084,1.088,1.091},
                                            {0.952,0.978,0.997,1.010,1.021,1.030,1.037,1.043,1.048,1.052,1.056,1.060,1.063,1.068,1.074,1.081,1.086,1.090,1.093},
                                            {0.953,0.979,0.997,1.011,1.022,1.031,1.038,1.044,1.049,1.054,1.057,1.061,1.064,1.069,1.075,1.083,1.088,1.092,1.095},
                                            {0.953,0.979,0.998,1.012,1.023,1.032,1.039,1.045,1.050,1.055,1.059,1.062,1.065,1.070,1.077,1.084,1.089,1.093,1.096},
                                            {0.954,0.980,0.999,1.013,1.024,1.032,1.040,1.046,1.051,1.056,1.060,1.063,1.066,1.072,1.078,1.086,1.091,1.095,1.098},
                                            {0.954,0.981,0.999,1.013,1.024,1.033,1.040,1.047,1.052,1.057,1.061,1.064,1.067,1.073,1.079,1.087,1.092,1.096,1.100},
                                            {0.955,0.981,1.000,1.014,1.025,1.034,1.041,1.048,1.053,1.058,1.062,1.065,1.068,1.074,1.080,1.088,1.094,1.098,1.101},
                                            {0.955,0.982,1.000,1.015,1.026,1.035,1.042,1.048,1.054,1.058,1.063,1.066,1.069,1.075,1.081,1.089,1.095,1.099,1.103},
                                            {0.956,0.982,1.001,1.015,1.026,1.035,1.043,1.049,1.055,1.059,1.063,1.067,1.070,1.076,1.083,1.091,1.096,1.101,1.104},
                                            {0.956,0.982,1.001,1.016,1.027,1.036,1.043,1.050,1.055,1.060,1.064,1.068,1.071,1.077,1.084,1.092,1.098,1.102,1.105},
                                            {0.956,0.983,1.002,1.016,1.027,1.036,1.044,1.051,1.056,1.061,1.065,1.069,1.072,1.078,1.085,1.093,1.099,1.103,1.107},
                                            {0.957,0.983,1.002,1.017,1.028,1.037,1.045,1.051,1.057,1.061,1.066,1.070,1.073,1.079,1.085,1.094,1.100,1.104,1.108},
                                            {0.957,0.984,1.003,1.017,1.028,1.038,1.045,1.052,1.057,1.062,1.067,1.070,1.074,1.079,1.086,1.095,1.101,1.105,1.109},
                                            {0.957,0.984,1.003,1.018,1.029,1.038,1.046,1.052,1.058,1.063,1.067,1.071,1.074,1.080,1.087,1.096,1.102,1.107,1.110 },
            };
            X_Scp_GD_10X = new double[19] { 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 170, 200, 250, 300, 350, 400 };
            Y_Scp_GD_10X = new double[38] { 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, 310, 320, 330, 340, 350, 360, 370, 380, 390, 400 };
            Scp_GD_6X_SRS = new double[15, 15]   {
                                                {0.7896,0.8145,0.8242,0.8287,0.8316,0.8326,0.8337,0.8344,0.8353,0.8357,0.8361,0.8363,0.8365,0.8373,0.8374},
                                                {0.8193,0.8573,0.8731,0.8811,0.8860,0.8891,0.8911,0.8926,0.8940,0.8948,0.8955,0.8960,0.8965,0.8974,0.8977},
                                                {0.8308,0.8749,0.8950,0.9061,0.9130,0.9175,0.9207,0.9231,0.9252,0.9266,0.9278,0.9287,0.9295,0.9306,0.9312},
                                                {0.8365,0.8844,0.9077,0.9209,0.9295,0.9351,0.9394,0.9426,0.9452,0.9472,0.9488,0.9501,0.9513,0.9526,0.9534},
                                                {0.8399,0.8904,0.9159,0.9309,0.9408,0.9473,0.9524,0.9563,0.9594,0.9619,0.9639,0.9656,0.9670,0.9685,0.9695},
                                                {0.8420,0.8945,0.9217,0.9380,0.9490,0.9563,0.9621,0.9665,0.9702,0.9730,0.9754,0.9774,0.9791,0.9807,0.9820},
                                                {0.8435,0.8975,0.9260,0.9434,0.9552,0.9633,0.9696,0.9746,0.9786,0.9818,0.9845,0.9867,0.9887,0.9905,0.9920},
                                                {0.8446,0.8998,0.9293,0.9476,0.9601,0.9688,0.9757,0.9811,0.9854,0.9890,0.9919,0.9944,0.9966,0.9986,1.0002},
                                                {0.8454,0.9016,0.9320,0.9510,0.9642,0.9734,0.9807,0.9864,0.9911,0.9949,0.9981,1.0008,1.0032,1.0054,1.0071},
                                                {0.8460,0.9030,0.9341,0.9538,0.9675,0.9773,0.9849,0.9910,0.9959,1.0000,1.0034,1.0063,1.0088,1.0112,1.0131},
                                                {0.8465,0.9042,0.9360,0.9562,0.9703,0.9806,0.9885,0.9949,1.0001,1.0044,1.0080,1.0110,1.0137,1.0162,1.0183},
                                                {0.8470,0.9052,0.9375,0.9582,0.9727,0.9834,0.9917,0.9982,1.0037,1.0082,1.0119,1.0152,1.0180,1.0206,1.0228},
                                                {0.8473,0.9061,0.9388,0.9599,0.9748,0.9859,0.9944,1.0012,1.0069,1.0115,1.0155,1.0189,1.0218,1.0245,1.0268},
                                                {0.8476,0.9068,0.9399,0.9614,0.9766,0.9880,0.9968,1.0038,1.0097,1.0145,1.0186,1.0221,1.0252,1.0281,1.0304},
                                                {0.8478,0.9074,0.9409,0.9628,0.9782,0.9900,0.9990,1.0062,1.0122,1.0172,1.0214,1.0251,1.0282,1.0312,1.0337},
                                                };
            X_Scp_GD_6X_SRS = new double[15] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };
            Y_Scp_GD_6X_SRS = new double[15] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };

            //Perfiles diagonales para campo 40 cm x 40 cm
            OAR_GD_6X = new double[147, 5] {
                                            { 99.90, 99.90, 100.10,99.90,99.90},
                                            { 100.0,100.00,100.10,99.90,99.90},
                                            {100.10,100.00,100.20,100.0,100.10},
                                            {100.30,100.20,100.40,100.10,100.10},
                                            {100.50,100.40,100.50,100.30,100.20},
                                            {100.70,100.50,100.70,100.40,100.20},
                                            {100.90,100.70,100.70,100.60,100.30},
                                            {101.10,100.80,100.90,100.60,100.40},
                                            {101.20,100.90,101.00,100.60,100.40},
                                            {101.40,101.10,101.10,100.70,100.60},
                                            {101.50,101.20,101.20,100.70,100.60},
                                            {101.50,101.30,101.20,100.80,100.70},
                                            {101.50,101.40,101.30,100.90,100.70},
                                            {101.60,101.50,101.30,100.90,100.60},
                                            {101.80,101.60,101.30,100.90,100.70},
                                            {102.00,101.70,101.50,100.90,100.60},
                                            {102.30,101.80,101.50,100.90,100.60},
                                            {102.50,102.00,101.70,101.00,100.60},
                                            {102.70,102.20,101.90,101.00,100.60},
                                            {103.00,102.50,102.00,101.10,100.60},
                                            {103.10,102.60,102.10,101.10,100.60},
                                            {103.30,102.80,102.20,101.20,100.70},
                                            {103.40,102.90,102.40,101.30,100.80},
                                            {103.50,103.00,102.50,101.40,100.80},
                                            {103.60,103.10,102.50,101.30,100.80},
                                            {103.70,103.20,102.50,101.30,100.70},
                                            {103.70,103.20,102.50,101.30,100.70},
                                            {103.80,103.30,102.60,101.20,100.60},
                                            {103.90,103.30,102.60,101.20,100.50},
                                            {104.00,103.40,102.60,101.20,100.30},
                                            {104.10,103.40,102.50,101.20,100.30},
                                            {104.20,103.50,102.50,101.20,100.30},
                                            {104.30,103.50,102.60,101.20,100.20},
                                            {104.40,103.60,102.60,101.20,100.10},
                                            {104.50,103.60,102.70,101.00,100.10},
                                            {104.50,103.60,102.80,100.90,100.00},
                                            {104.60,103.70,102.70,100.80,99.90},
                                            {104.60,103.70,102.70,100.80,99.80},
                                            {104.70,103.70,102.60,100.80,99.60},
                                            {104.80,103.80,102.50,100.70,99.40},
                                            {104.80,103.80,102.50,100.70,99.30},
                                            {104.80,103.80,102.60,100.50,99.20},
                                            {104.90,103.90,102.60,100.40,99.10},
                                            {105.00,103.90,102.60,100.20,99.00},
                                            {105.10,104.00,102.60,100.10,98.80},
                                            {105.20,104.10,102.60,99.90 ,98.60},
                                            {105.20,104.20,102.60,99.90 ,98.50},
                                            {105.20,104.30,102.60,99.80 ,98.40},
                                            {105.20,104.30,102.60,99.70 ,98.30},
                                            {105.30,104.30,102.50,99.60 ,98.10},
                                            {105.50,104.20,102.40,99.50 ,98.00},
                                            {105.50,104.20,102.40,99.30,97.70},
                                            {105.50,104.20,102.40,99.20,97.50},
                                            {105.60,104.30,102.30,99.10,97.10},
                                            {105.70,104.30,102.30,98.90,97.00},
                                            {105.80,104.30,102.30,98.80,97.00},
                                            {105.90,104.40,102.30,98.70,96.80},
                                            {106.00,104.50,102.20,98.50,96.50},
                                            {106.10,104.50,102.20,98.40,96.30},
                                            {106.20,104.60,102.10,98.20,96.10},
                                            {106.20,104.50,102.00,98.00,95.80},
                                            {106.20,104.60,102.00,97.90,95.60},
                                            {106.30,104.60,102.00,97.70,95.30},
                                            {106.30,104.60,102.00,97.50,95.20},
                                            {106.30,104.60,101.90,97.30,95.00},
                                            {106.40,104.60,101.90,97.20,94.70},
                                            {106.40,104.70,101.70,97.10,94.50},
                                            {106.50,104.70,101.60,96.90,94.20},
                                            {106.50,104.60,101.50,96.80,94.00},
                                            {106.60,104.60,101.50,96.60,93.70},
                                            {106.60,104.50,101.40,96.40,93.50},
                                            {106.60,104.50,101.30,96.20,93.30},
                                            {106.60,104.50,101.20,95.90,92.90},
                                            {106.60,104.40,101.00,95.60,92.70},
                                            {106.60,104.40,100.90,95.40,92.30},
                                            {106.50,104.40,100.80,95.20,92.10},
                                            {106.50,104.30,100.70,95.10,91.90},
                                            {106.50,104.20,100.60,94.80,91.70},
                                            {106.50,104.10,100.40,94.60,91.50},
                                            {106.50,104.00,100.30,94.20,91.20},
                                            {106.50,103.90,100.10,93.90,90.80},
                                            {106.40,103.80,99.90,93.60,90.50},
                                            {106.40,103.70,99.70,93.30,90.20},
                                            {106.30,103.60,99.50,93.10,89.90},
                                            {106.20,103.50,99.20,92.80,89.50},
                                            {106.10,103.40,99.00,92.50,89.10},
                                            {106.00,103.30,98.70,92.20,88.70},
                                            {105.90,103.10,98.40,91.90,88.40},
                                            {105.80,102.90,98.10,91.50,88.10},
                                            {105.60,102.60,97.90,91.20,87.90},
                                            {105.30,102.40,97.60,90.80,87.40},
                                            {105.00,102.10,97.30,90.40,86.90},
                                            {104.70,101.80,96.90,90.00,86.40},
                                            {104.30,101.40,96.40,89.60,86.10},
                                            {103.90,10.90,96.00,89.20,85.70},
                                            {103.40,10.30,95.60,88.80,85.30},
                                            {103.10,99.70,95.10,88.40,84.80},
                                            {102.20,99.10,94.50,87.90,84.30},
                                            {96.50,98.30,93.90,87.40,83.90},
                                            {82.70,97.70,93.30,87.00,83.50},
                                            {62.00,96.60,92.60,86.40,83.20},
                                            {41.60,93.20,91.80,85.90,82.70},
                                            {27.90,83.50,90.90,85.30,82.20},
                                            {21.50,66.90,90.00,84.70,81.70},
                                            {19.40,48.20,89.10,84.20,81.10},
                                            {18.40,33.40,87.90,83.60,80.60},
                                            {17.50,25.50,84.30,83.00,80.10},
                                            {16.60,22.40,75.50,82.20,79.50},
                                            {15.50,20.90,61.50,81.40,79.00},
                                            {14.30,19.80,46.00,80.60,78.50},
                                            {12.80,18.70,33.90,79.80,78.00},
                                            {11.10,17.40,27.10,78.90,77.40},
                                            {9.300,16.10,24.10,77.90,76.60},
                                            {7.600,14.70,22.70,76.80,76.00},
                                            {6.100,13.20,21.60,75.80,75.30},
                                            {5.100,11.60,20.40,74.10,74.60},
                                            {4.500,9.900,19.20,70.00,74.00},
                                            {4.200,8.300,18.00,62.10,73.30},
                                            {4.000,7.000,16.70,51.50,72.50},
                                            {3.800,6.100,15.40,40.80,71.70},
                                            {3.600,5.600,13.90,32.70,70.90},
                                            {3.400,5.200,12.40,28.10,70.10},
                                            {3.200,4.900,10.80,25.60,69.20},
                                            {3.000,4.600,9.400,24.30,68.30},
                                            {2.800,4.400,8.200,23.20,67.20},
                                            {2.700,4.200,7.400,22.20,65.60},
                                            {2.500,3.900,6.900,21.20,62.00},
                                            {2.400,3.700,6.500,20.10,55.50},
                                            {2.300,3.500,6.200,19.00,46.70},
                                            {2.200,3.400,5.900,17.90,38.10},
                                            {2.100,3.200,5.600,16.70,31.60},
                                            {2.000,3.000,5.300,15.40,27.60},
                                            {1.900,2.900,5.100,14.00,25.50},
                                            {1.800,2.800,4.800,12.60,24.30},
                                            {1.800,2.600,4.600,11.30,23.40},
                                            {1.700,2.500,4.400,10.20,22.60},
                                            {1.600,2.400,4.200,9.400,21.70},
                                            {1.600,2.300,4.000,8.800,20.80},
                                            {1.500,2.200,3.800,8.400,19.80},
                                            {1.500,2.100,3.700,8.100,18.80},
                                            {1.400,2.100,3.500,7.700,17.80},
                                            {1.400,2.000,3.400,7.400,16.80},
                                            {1.300,1.900,3.200,7.100,15.70},
                                            {1.300,1.800,3.100,6.800,14.60},
                                            {1.300,1.800,3.000,6.500,13.40},
                                            {1.200,1.700,2.800,6.200,12.20},
                                            {1.200,1.700,2.700,6.000,11.20},
                                            };
            Prof_OAR_6X = new double[5] { 16, 50, 100, 200, 300 };
            Dist_OAR_6X = new double[147] { 0, 2.500, 5, 7.500, 10, 12.50, 15, 17.50, 20, 22.50, 25, 27.50, 30, 32.50, 35, 37.50, 40, 42.50, 45, 47.50, 50, 52.50, 55, 57.50, 60, 62.50, 65, 67.50, 70, 72.50, 75, 77.50, 80, 82.50, 85, 87.50, 90, 92.50, 95, 97.50, 10, 102.50, 105, 107.50, 110, 112.50, 115, 117.50, 120, 122.50, 125, 127.50, 130, 132.50, 135, 137.50, 140, 142.50, 145, 147.50, 150, 152.50, 155, 157.50, 160, 162.50, 165, 167.50, 170, 172.50, 175, 177.50, 180, 182.50, 185, 187.50, 190, 192.50, 195, 197.50, 20, 202.50, 205, 207.50, 210, 212.50, 215, 217.50, 220, 222.50, 225, 227.50, 230, 232.50, 235, 237.50, 240, 242.50, 245, 247.50, 250, 252.50, 255, 257.50, 260, 262.50, 265, 267.50, 270, 272.50, 275, 277.50, 280, 282.50, 285, 287.50, 290, 292.50, 295, 297.50, 30, 302.50, 305, 307.50, 310, 312.50, 315, 317.50, 320, 322.50, 325, 327.50, 330, 332.50, 335, 337.50, 340, 342.50, 345, 347.50, 350, 352.50, 355, 357.50, 360, 362.50, 365 };

            //Perfiles diagonales para campo 40 cm x 40 cm
            OAR_GD_10X = new double[147, 5] {
                                            { 100,100,100,99.9,100},
                                            {100,100,100.2,99.9,100.1},
                                            {100,100,100.2,99.9,100.1},
                                            {100.1,100.1,100.3,100.1,100.1},
                                            {100.3,100.2,100.4,100.2,100.3},
                                            {100.4,100.3,100.6,100.2,100.3},
                                            {100.6,100.5,100.6,100.3,100.4},
                                            {100.8,100.7,100.8,100.4,100.3},
                                            {100.9,100.7,100.9,100.5,100.4},
                                            {101.1,100.9,101,100.5,100.5},
                                            {101.2,101,101,100.5,100.6},
                                            {101.4,101.2,101,100.5,100.4},
                                            {101.6,101.3,101.2,100.5,100.4},
                                            {101.7,101.4,101.3,100.4,100.5},
                                            {101.9,101.5,101.3,100.5,100.4},
                                            {102,101.6,101.4,100.6,100.3},
                                            {102.2,101.7,101.5,100.6,100.3},
                                            {102.3,101.8,101.6,100.5,100.3},
                                            {102.5,101.9,101.7,100.6,100.4},
                                            {102.6,102,101.7,100.7,100.3},
                                            {102.7,102.1,101.7,100.7,100.2},
                                            {102.8,102.2,101.8,100.7,100.2},
                                            {102.9,102.3,101.9,100.7,100.1},
                                            {103,102.3,101.8,100.7,100},
                                            {103,102.3,101.9,100.6,99.8},
                                            {103.2,102.4,101.9,100.5,99.7},
                                            {103.2,102.5,101.8,100.4,99.6},
                                            {103.2,102.5,101.8,100.5,99.6},
                                            {103.3,102.5,101.9,100.4,99.5},
                                            {103.5,102.6,101.9,100.2,99.3},
                                            {103.6,102.6,101.9,100.1,99.3},
                                            {103.5,102.7,101.9,100.1,99.1},
                                            {103.6,102.6,101.9,99.8,99.1},
                                            {103.6,102.6,101.9,99.7,99},
                                            {103.7,102.7,101.9,99.8,98.8},
                                            {103.7,102.7,101.8,99.8,98.7},
                                            {103.8,102.7,101.8,99.7,98.5},
                                            {103.9,102.7,101.8,99.5,98.4},
                                            {103.9,102.7,101.7,99.5,98.2},
                                            {104,102.8,101.7,99.4,98},
                                            {104,102.8,101.7,99.2,97.9},
                                            {104.1,102.9,101.7,99.1,97.7},
                                            {104.2,102.9,101.7,99,97.6},
                                            {104.2,102.9,101.7,98.9,97.6},
                                            {104.2,102.9,101.7,98.8,97.5},
                                            {104.2,102.9,101.7,98.8,97.3},
                                            {104.3,102.9,101.6,98.7,97},
                                            {104.4,102.9,101.6,98.5,96.9},
                                            {104.5,102.9,101.6,98.4,96.7},
                                            {104.5,103,101.5,98.3,96.6},
                                            {104.5,103,101.5,98.1,96.4},
                                            {104.5,103,101.4,98,96.4},
                                            {104.5,102.9,101.4,97.9,96.2},
                                            {104.4,102.9,101.4,97.8,95.9},
                                            {104.6,102.9,101.3,97.7,95.6},
                                            {104.7,102.9,101.3,97.5,95.4},
                                            {104.7,103,101.2,97.3,95.2},
                                            {104.8,103,101.1,97.1,95.1},
                                            {104.9,103,101,97,94.9},
                                            {104.9,103,101,96.9,94.6},
                                            {105,103.1,101,96.6,94.5},
                                            {105.1,103.1,100.9,96.4,94.4},
                                            {105.2,103.1,100.8,96.3,94.1},
                                            {105.3,103.2,100.8,96.2,93.9},
                                            {105.3,103.3,100.9,96.1,93.7},
                                            {105.5,103.3,100.9,95.9,93.5},
                                            {105.6,103.4,100.8,95.8,93.3},
                                            {105.8,103.4,100.8,95.6,92.9},
                                            {105.9,103.4,100.8,95.6,92.7},
                                            {106.1,103.5,100.8,95.4,92.4},
                                            {106.2,103.6,100.7,95.2,92.3},
                                            {106.3,103.8,100.5,95,92.1},
                                            {106.3,103.8,100.5,94.8,91.9},
                                            {106.4,103.9,100.6,94.7,91.7},
                                            {106.4,103.9,100.6,94.5,91.4},
                                            {106.3,103.8,100.6,94.5,91.2},
                                            {106.3,103.6,100.5,94.2,90.9},
                                            {106.2,103.5,100.3,94,90.6},
                                            {106.1,103.4,100,93.9,90.5},
                                            {105.8,103.3,99.8,93.7,90.3},
                                            {105.5,103.1,99.7,93.4,89.9},
                                            {105.2,102.9,99.6,93.2,89.7},
                                            {105,102.7,99.3,93,89.6},
                                            {104.7,102.5,99.1,92.8,89.2},
                                            {104.4,102.1,98.8,92.6,88.9},
                                            {104.1,101.7,98.6,92.3,88.7},
                                            {103.8,101.5,98.2,92,88.5},
                                            {103.5,101.1,97.9,91.7,88.2},
                                            {103.1,100.7,97.4,91.5,88},
                                            {102.8,100.3,97,91.2,87.5},
                                            {102.5,100,96.6,90.8,87.2},
                                            {102.2,99.5,96.2,90.4,87},
                                            {101.8,99,95.7,90,86.7},
                                            {101.5,98.6,95.2,89.5,86.3},
                                            {101.2,98.1,94.6,88.9,85.9},
                                            {100.9,97.7,94.1,88.4,85.7},
                                            {100.7,97.4,93.6,87.9,85.1},
                                            {100.3,97,93,87.4,84.7},
                                            {99.8,96.5,92.4,86.9,84.3},
                                            {97.7,96,91.9,86.4,83.8},
                                            {91,95.4,91.3,85.8,83.3},
                                            {76.6,94.3,90.8,85.2,82.8},
                                            {58.4,91,90.2,84.6,82.4},
                                            {43.7,81.8,89.6,84.1,81.9},
                                            {36.4,66.6,89,83.6,81.4},
                                            {33.7,50.8,88.2,83,80.9},
                                            {32.2,40.1,86.9,82.3,80.4},
                                            {31,35.2,83.6,81.7,79.7},
                                            {29.8,33.2,75.6,81,79.1},
                                            {28.5,31.7,62.4,80.4,78.5},
                                            {26.8,30.4,48.9,79.7,78},
                                            {24.6,29.1,39.9,79.1,77.4},
                                            {22,27.6,35.5,78.4,76.8},
                                            {18.9,25.8,33.4,77.6,76.1},
                                            {15.3,23.5,31.9,76.7,75.5},
                                            {11.3,20.8,30.6,75.7,74.9},
                                            {7.9,17.5,29.2,74,74.3},
                                            {5.8,14,27.8,70.1,73.5},
                                            {4.8,10.5,26.1,62.1,72.8},
                                            {4.3,7.7,24.2,51.7,72.3},
                                            {3.9,6,21.9,42.6,71.6},
                                            {3.7,5.2,19.5,36.8,70.9},
                                            {3.4,4.7,16.7,33.6,70.2},
                                            {3.2,4.3,13.7,32,69.6},
                                            {3,4,10.7,30.8,68.7},
                                            {2.8,3.8,8.5,29.7,67.6},
                                            {2.6,3.6,7,28.7,65.9},
                                            {2.4,3.3,6.2,27.5,62.7},
                                            {2.3,3.1,5.7,26.3,56.6},
                                            {2.1,2.9,5.3,25,48.5},
                                            {2,2.7,5.1,23.4,40.6},
                                            {1.8,2.6,4.8,21.5,35.2},
                                            {1.7,2.4,4.5,19.5,32.5},
                                            {1.5,2.3,4.2,17.2,30.9},
                                            {1.4,2.1,4,14.5,29.6},
                                            {1.3,1.9,3.7,11.9,28.6},
                                            {1.2,1.8,3.5,9.8,27.6},
                                            {1.1,1.7,3.3,8.4,26.7},
                                            {1,1.6,3.1,7.5,25.7},
                                            {1,1.5,2.9,7.1,24.7},
                                            {0.9,1.4,2.8,6.7,23.5},
                                            {0.9,1.3,2.6,6.3,22.1},
                                            {0.8,1.2,2.5,6,20.5},
                                            {0.7,1.1,2.3,5.8,18.8},
                                            {0.7,1,2.2,5.5,17},
                                            {0.6,1,2.1,5.3,15.1},
                                            {0.6,0.9,2,5,13},



            };
            Prof_OAR_10X = new double[5] { 24, 50, 100, 200, 300 };
            Dist_OAR_10X = new double[147] { 0, 2.500, 5, 7.500, 10, 12.50, 15, 17.50, 20, 22.50, 25, 27.50, 30, 32.50, 35, 37.50, 40, 42.50, 45, 47.50, 50, 52.50, 55, 57.50, 60, 62.50, 65, 67.50, 70, 72.50, 75, 77.50, 80, 82.50, 85, 87.50, 90, 92.50, 95, 97.50, 10, 102.50, 105, 107.50, 110, 112.50, 115, 117.50, 120, 122.50, 125, 127.50, 130, 132.50, 135, 137.50, 140, 142.50, 145, 147.50, 150, 152.50, 155, 157.50, 160, 162.50, 165, 167.50, 170, 172.50, 175, 177.50, 180, 182.50, 185, 187.50, 190, 192.50, 195, 197.50, 20, 202.50, 205, 207.50, 210, 212.50, 215, 217.50, 220, 222.50, 225, 227.50, 230, 232.50, 235, 237.50, 240, 242.50, 245, 247.50, 250, 252.50, 255, 257.50, 260, 262.50, 265, 267.50, 270, 272.50, 275, 277.50, 280, 282.50, 285, 287.50, 290, 292.50, 295, 297.50, 30, 302.50, 305, 307.50, 310, 312.50, 315, 317.50, 320, 322.50, 325, 327.50, 330, 332.50, 335, 337.50, 340, 342.50, 345, 347.50, 350, 352.50, 355, 357.50, 360, 362.50, 365 };

            OAR_GD_6X_SRS = new double[181, 5];
            Prof_OAR_6X_SRS = new double[5] { 16, 60, 100, 200, 300 };
            Dist_OAR_6X_SRS = new double[181] { -225, -222.50, -220, -217.50, -215, -212.50, -210, -207.50, -205, -202.50, -200, -197.50, -195, -192.50, -190, -187.50, -185, -182.50, -180, -177.50, -175, -172.50, -170, -167.50, -165, -162.50, -160, -157.50, -155, -152.50, -150, -147.50, -145, -142.50, -140, -137.50, -135, -132.50, -130, -127.50, -125, -122.50, -120, -117.50, -115, -112.50, -110, -107.50, -105, -102.50, -100, -97.50, -95, -92.50, -90, -87.50, -85, -82.50, -80, -77.500, -75, -72.50, -70, -67.50, -65, -62.50, -60, -57.50, -55, -52.50, -50, -47.50, -45, -42.50, -40, -37.50, -35, -32.50, -30, -27.50, -25, -22.50, -20, -17.50, -15, -12.50, -10, -7.50, -5, -2.50, 0, 2.50, 5, 7.50, 10, 12.50, 15, 17.50, 20, 22.50, 25, 27.50, 30, 32.50, 35, 37.50, 40, 42.50, 45, 47.50, 50, 52.50, 55, 57.50, 60, 62.50, 65, 67.50, 70, 72.50, 75, 77.50, 80, 82.50, 85, 87.50, 90, 92.50, 95, 97.50, 100, 102.50, 105, 107.50, 110, 112.50, 115, 117.50, 120, 122.50, 125, 127.50, 130, 132.50, 135, 137.50, 140, 142.50, 145, 147.50, 150, 152.50, 155, 157.50, 160, 162.50, 165, 167.50, 170, 172.50, 175, 177.50, 180, 182.50, 185, 187.50, 190, 192.50, 195, 197.50, 200, 202.50, 205, 207.50, 210, 212.50, 215, 217.50, 220, 222.50, 225 };

            //Kernel obtenido del paper "Photon pencil kernel parameterisation based on beam quality index"
            double A_1 = 0.0128018 * poliTPR[0] - 0.0577391 * poliTPR[1] + 0.1790839 * poliTPR[2] - 0.2467955 * poliTPR[3] + 0.1328192 * poliTPR[4] - 0.0194684 * poliTPR[5];
            double A_2 = 16.7815028 * poliTPR[0] - 279.4672663 * poliTPR[1] + 839.0016549 * poliTPR[2] - 978.4915013 * poliTPR[3] + 470.5317337 * poliTPR[4] - 69.2485573 * poliTPR[5];
            double A_3 = -0.0889669 * poliTPR[0] - 0.2587584 * poliTPR[1] + 0.7069203 * poliTPR[2] - 0.3654033 * poliTPR[3] + 0.0029760 * poliTPR[4] - 0.0003786 * poliTPR[5];
            double A_4 = 0.0017089 * poliTPR[0] - 0.0169150 * poliTPR[1] + 0.0514650 * poliTPR[2] - 0.0639530 * poliTPR[3] + 0.0324490 * poliTPR[4] - 0.0049121 * poliTPR[5];
            double A_5 = 0.1431447 * poliTPR[0] - 0.2134626 * poliTPR[1] + 0.5825546 * poliTPR[2] - 0.2969273 * poliTPR[3] - 0.0011436 * poliTPR[4] + 0.0002219 * poliTPR[5];
            double B_1 = -42.7607523 * poliTPR[0] + 264.3424720 * poliTPR[1] - 633.4540368 * poliTPR[2] + 731.5311577 * poliTPR[3] - 402.5280374 * poliTPR[4] + 82.4936551 * poliTPR[5];
            double B_2 = 0.2428359 * poliTPR[0] - 2.5029336 * poliTPR[1] + 7.6128101 * poliTPR[2] - 9.5273454 * poliTPR[3] + 4.8249840 * poliTPR[4] - 0.7097852 * poliTPR[5];
            double B_3 = -0.0910420 * poliTPR[0] - 0.2621605 * poliTPR[1] + 0.7157244 * poliTPR[2] - 0.3664126 * poliTPR[3] + 0.0000930 * poliTPR[4] - 0.0000232 * poliTPR[5];
            double B_4 = 0.0017284 * poliTPR[0] - 0.0172146 * poliTPR[1] + 0.0522109 * poliTPR[2] - 0.0643946 * poliTPR[3] + 0.0322177 * poliTPR[4] - 0.0047015 * poliTPR[5];
            double B_5 = -30.4609625 * poliTPR[0] + 354.2866078 * poliTPR[1] - 1073.2952368 * poliTPR[2] + 1315.2670101 * poliTPR[3] - 656.3702845 * poliTPR[4] + 96.5983711 * poliTPR[5];
            double a_1 = -26.3337419 * poliTPR[0] + 435.6865552 * poliTPR[1] - 1359.8342546 * poliTPR[2] + 1724.6602381 * poliTPR[3] - 972.7565415 * poliTPR[4] + 200.3468023 * poliTPR[5];
            double a_2 = -0.0065985 * poliTPR[0] + 0.0242136 * poliTPR[1] - 0.0647001 * poliTPR[2] + 0.0265272 * poliTPR[3] + 0.0072169 * poliTPR[4] - 0.0020479 * poliTPR[5];
            double b_1 = -80.7027159 * poliTPR[0] + 668.1710175 * poliTPR[1] - 2173.2445309 * poliTPR[2] + 3494.2393490 * poliTPR[3] - 2784.4670834 * poliTPR[4] + 881.2276510 * poliTPR[5];
            double b_2 = 3.4685991 * poliTPR[0] - 41.2468479 * poliTPR[1] + 124.9729952 * poliTPR[2] - 153.2610078 * poliTPR[3] + 76.5242757 * poliTPR[4] - 11.2624113 * poliTPR[5];
            double b_3 = -39.6550497 * poliTPR[0] + 277.7202038 * poliTPR[1] - 777.0749505 * poliTPR[2] + 1081.5724508 * poliTPR[3] - 747.1056558 * poliTPR[4] + 204.5432666 * poliTPR[5];
            double b_4 = 0.6514859 * poliTPR[0] - 4.7179961 * poliTPR[1] + 13.6742202 * poliTPR[2] - 19.7521659 * poliTPR[3] + 14.1873606 * poliTPR[4] - 4.0478845 * poliTPR[5];
            double b_5 = 0.4695047 * poliTPR[0] - 3.6644336 * poliTPR[1] + 10.0039321 * poliTPR[2] - 5.1195905 * poliTPR[3] - 0.0007387 * poliTPR[4] + 0.0002360 * poliTPR[5];


            double prof = DFI + y_calc - DFP;

            double prof_cm = prof / 10; // WED en centimetros
            double a = a_1 + a_2 * prof_cm; // Cálculo de los parámetros con los coeficientes
            double b = b_1 * (1 - Math.Exp(b_2 * Math.Sqrt(Math.Pow(prof_cm, 2) + Math.Pow(b_5, 2)))) * Math.Exp(b_3 * prof_cm + b_4 * Math.Pow(prof_cm, 2));
            double A = A_1 * (1 - Math.Exp(A_2 * Math.Sqrt(Math.Pow(prof_cm, 2) + Math.Pow(A_5, 2)))) * Math.Exp(A_3 * prof_cm + A_4 * Math.Pow(prof_cm, 2)) * a;
            double B = B_1 * (1 - Math.Exp(B_2 * Math.Sqrt(Math.Pow(prof_cm, 2) + Math.Pow(B_5, 2)))) * Math.Exp(B_3 * prof_cm + B_4 * Math.Pow(prof_cm, 2)) * b;



            // Calculo de dosis para campo abierto simetrico
            double[,] R = new double[(int)(X1 + X2), (int)(Y1 + Y2)];
            double[,] kernel = new double[(int)(X1 + X2), (int)(Y1 + Y2)];
            double Dosis_MLCasym = 0;
            double Dosis_Ofield = 0;
            for (int i = 0; i < Math.Ceiling(X1 + X2); i++)
            {
                for (int j = 0; j < Math.Ceiling(Y1 + Y2); j++)
                {

                    double x = 0.5 - Math.Floor((X1 + X2) / 2) + i;
                    double y = 0.5 - Math.Floor((Y1 + Y2) / 2) + j;
                    R[i, j] = Math.Sqrt(x * x + y * y);
                    //Calculow del Kernel de grilla
                    kernel[i, j] = 10 * (A * Math.Exp(-a * R[i, j] / 10) + B * Math.Exp(-b * R[i, j] / 10)) / R[i, j];
                    Dosis_Ofield = Dosis_Ofield + kernel[i, j];

                }
            }

            // Calculo de dosis para campo conformado asimetrico

            double x_cal_iso = Math.Round(x_calc * DFI / (DFP + prof));
            double y_cal_iso = Math.Round(y_calc * DFI / (DFP + prof));
            double z_cal_iso = Math.Round(z_calc * DFI / (DFP + prof));

            double F_Sp = 0;
            for (int i = 0; i < Math.Ceiling(X1 + X2); i++)
            {
                for (int j = 0; j < Math.Ceiling(Y1 + Y2); j++)
                {
                    double x = (-X1 - x_cal_iso + 0.5) + i;
                    double y = (-Y2 + z_cal_iso + 0.5) + j;
                    R[i, j] = Math.Sqrt(x * x + y * y);
                    //Calculo del Kernel de grilla
                    kernel[i, j] = 10 * (A * Math.Exp(-a * R[i, j] / 10) + B * Math.Exp(-b * R[i, j] / 10)) / R[i, j];
                    Dosis_MLCasym = Dosis_MLCasym + tr_MLC[j, i] * kernel[i, j];
                    
                }
            }
            F_Sp = Dosis_MLCasym / Dosis_Ofield;

            //Calculo por correccion por OAR.
            double r_cal;
            double f_OAR;
            double dist_agua = 0;
            double magni = 0;
            double F_inh = 0;
            double Sp_ind = 0;
            double Sc_aux1 = 0;
            double Sc_aux2 = 0;
            double Sc_aux3 = 0;
            double Sc_aux4 = 0;
            double Sc_aux5 = 0;
            double Sc_aux6 = 0;
            double Sc_ind = 0;
            double DR_aux1 = 0;
            double DR_aux2 = 0;
            double DR_indep = 0;

            r_cal = Math.Sqrt(Math.Pow(x_calc, 2) + Math.Pow(z_calc, 2)) * (DFI + prof) / (DFP + prof);
            double TCeq = Interp2D(Jaw_Short, Jaw_Long, TC_eff, Math.Min(X1 + X2, Y1 + Y2), Math.Max(X1 + X2, Y1 + Y2));

            switch (Energia)
            {
                case ("6X"):

                    f_OAR = Interp2D(Dist_OAR_6X, Prof_OAR_6X, OAR_GD_6X, r_cal, prof,1) / 100;

                    // calculo correccion por inhomogeneidad y contorno

                    //+

                    dist_agua = Math.Sqrt(Math.Pow(prof, 2) * (1 + (Math.Pow(x_cal_iso, 2) + Math.Pow(z_cal_iso, 2)) / Math.Pow(DFP + prof, 2)));
                    magni = (DFP + prof) / (DFI + dmax);
                    F_inh = TMR(WED, magni * TCeq, dmax, Dose, TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, TPR20_10, TC_Sp, IC_Sp, Sp) / TMR(dist_agua, magni * TCeq, dmax, Dose, TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, TPR20_10, TC_Sp, IC_Sp, Sp);
                    Sp_ind = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, magni * TCeq) * Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, 100, 100) / Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, magni * TCeq, 100);

                    Sc_aux1 = Interp2D(X_Scp_GD_6X, Y_Scp_GD_6X, Scp_GD_6X, X1 + X2, Y1 + Y2);
                    Sc_aux2 = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, TCeq);
                    Sc_aux3 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, TCeq, 100);
                    Sc_aux4 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, 100, 50);
                    Sc_aux5 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, TCeq, 50);
                    Sc_aux6 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, 100, 100);
                    Sc_ind = Sc_aux1 / Sc_aux2 * Sc_aux3 * Sc_aux4 / Sc_aux5 / Sc_aux6;

                    DR_aux1 = TMR(prof, magni * TCeq, dmax, Dose, TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_6X, TPR20_10, TC_Sp, IC_Sp, Sp);
                    DR_aux2 = Sp_ind * Sc_ind * F_Sp * f_OAR * F_inh / Math.Pow(magni, 2);
                    DR_indep = Dose * DR_aux1 * DR_aux2;
                    UM_Indep = FieldDose / DR_indep;

                    break;

                case ("10X"):
                    
                    f_OAR = Interp2D(Dist_OAR_10X, Prof_OAR_10X, OAR_GD_10X, r_cal, prof,1) / 100;
                    // calculo correccion por inhomogeneidad y contorno


                    dist_agua = Math.Sqrt(Math.Pow(prof, 2) * (1 + (Math.Pow(x_cal_iso, 2) + Math.Pow(y_cal_iso, 2)) / Math.Pow(DFP + prof, 2)));
                    magni = (DFP + prof) / (DFI + dmax);
                    F_inh = TMR(WED, magni * TCeq, dmax, Dose, TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, TPR20_10, TC_Sp, IC_Sp, Sp) / TMR(dist_agua, magni * TCeq, dmax, Dose, TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, TPR20_10, TC_Sp, IC_Sp, Sp);
                    Sp_ind = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, magni * TCeq) * Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, 100, 100) / Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, magni * TCeq, 100); //Igual

                    Sc_aux1 = Interp2D(X_Scp_GD_10X, Y_Scp_GD_10X, Scp_GD_10X, X1 + X2, Y1 + Y2);
                    Sc_aux2 = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, TCeq);
                    Sc_aux3 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, TCeq, 100);
                    Sc_aux4 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, 100, 50);
                    Sc_aux5 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, TCeq, 50);
                    Sc_aux6 = Interp2D(TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, 100, 100);
                    Sc_ind = Sc_aux1 / Sc_aux2 * Sc_aux3 * Sc_aux4 / Sc_aux5 / Sc_aux6;

                    DR_aux1 = TMR(prof, magni * TCeq, dmax, Dose, TC_PDD_GD_STD, prof_PDD_GD_STD, PDD_GD_10X, TPR20_10, TC_Sp, IC_Sp, Sp);
                    DR_aux2 = Sp_ind * Sc_ind * F_Sp * f_OAR * F_inh / Math.Pow(magni, 2);
                    DR_indep = Dose * DR_aux1 * DR_aux2;
                    UM_Indep = FieldDose / DR_indep;

                    break;

                case ("SRS"):
                    
                    f_OAR = Interp2D(Dist_OAR_6X_SRS, Prof_OAR_6X_SRS, OAR_GD_6X_SRS, r_cal, prof,1) / 100;
                    // calculo correccion por inhomogeneidad y contorno


                    dist_agua = Math.Sqrt(Math.Pow(prof, 2) * (1 + (Math.Pow(x_cal_iso, 2) + Math.Pow(y_cal_iso, 2)) / Math.Pow(DFP + prof, 2)));
                    magni = (DFP + prof) / (DFI + dmax);
                    F_inh = TMR(WED, magni * TCeq, dmax, Dose, TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, TPR20_10, TC_Sp, IC_Sp, Sp) / TMR(dist_agua, magni * TCeq, dmax, Dose, TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, TPR20_10, TC_Sp, IC_Sp, Sp);
                    Sp_ind = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, magni * TCeq) * Interp2D(TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, 100, 100) / Interp2D(TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, magni * TCeq, 100);

                    Sc_aux1 = Interp2D(X_Scp_GD_6X_SRS, Y_Scp_GD_6X_SRS, Scp_GD_6X_SRS, X1 + X2, Y1 + Y2);
                    Sc_aux2 = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, TCeq);
                    Sc_aux3 = Interp2D(TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, TCeq, 100);
                    Sc_aux4 = Interp2D(TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, 100, 50);
                    Sc_aux5 = Interp2D(TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, TCeq, 50);
                    Sc_aux6 = Interp2D(TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, 100, 100);
                    Sc_ind = Sc_aux1 / Sc_aux2 * Sc_aux3 * Sc_aux4 / Sc_aux5 / Sc_aux6;

                    DR_aux1 = TMR(prof, magni * TCeq, dmax, Dose, TC_PDD_GD_SRS, prof_PDD_GD_SRS, PDD_GD_6X_SRS, TPR20_10, TC_Sp, IC_Sp, Sp);
                    DR_aux2 = Sp_ind * Sc_ind * F_Sp * f_OAR * F_inh / Math.Pow(magni, 2);
                    DR_indep = Dose * DR_aux1 * DR_aux2;
                    UM_Indep = FieldDose / DR_indep;

                    break;
            }

            return UM_Indep;
        }


        public double Calculo_Electrones(string Machine, double SSD, double WED, string App, double[] x, double[] y, string Energia, double Rp, double x_calc, double y_calc, double z_calc)
        {

            double[] prof_PDD = null; 
            double[] x_Kernel_SSD100 = new double[69] { 0, 9.50, 10.50, 11.50, 12.50, 13.50, 14.50, 15.50, 16.50, 17.50, 18.50, 19.50, 20.50, 21.50, 22.50, 23.50, 24.50, 25.50, 26.50, 27.50, 28.50, 29.50, 30.50, 31.50, 32.50, 33.50, 34.50, 35.50, 36.50, 37.50, 38.50, 39.50, 40.50, 41.50, 42.50, 43.50, 44.50, 45.50, 46.50, 47.50, 48.50, 49.50, 50.50, 51.50, 52.50, 53.50, 54.50, 55.50, 56.50, 57.50, 58.50, 59.50, 60.50, 61.50, 62.50, 63.50, 64.50, 65.50, 66.50, 67.50, 68.50, 69.50, 70.50, 71.50, 72.50, 73.50, 74.50, 75.50, 500 };
            double[] x_Kernel_SSD110 = new double[69] { 0, 9.500, 10.50, 11.50, 12.50, 13.50, 14.50, 15.50, 16.50, 17.50, 18.50, 19.50, 20.50, 21.50, 22.50, 23.50, 24.50, 25.50, 26.50, 27.50, 28.50, 29.50, 30.50, 31.50, 32.50, 33.50, 34.50, 35.50, 36.50, 37.50, 38.50, 39.50, 40.50, 41.50, 42.50, 43.50, 44.50, 45.50, 46.50, 47.50, 48.50, 49.50, 50.50, 51.50, 52.50, 53.50, 54.50, 55.50, 56.50, 57.50, 58.50, 59.50, 60.50, 61.50, 62.50, 63.50, 64.50, 65.50, 66.50, 67.50, 68.50, 69.50, 70.50, 71.50, 72.50, 73.50, 74.50, 75.50, 500 };
            double[] PDD = null;
            double[] f_Kernel_SSD100 = null;
            double[] f_Kernel_SSD110 = null;
            double[] f_SSD_ext = null;
            double DoseRef = 0;
            double F_App = 0;

            switch (Machine)
            {
                case ("2100CMLC"):
                    switch (Energia)
                    {
                        case "6E":
                            DoseRef = 0.9837;
                            prof_PDD = new double[18] { 0, 0.500000000000000, 2.90000000000000, 5.40000000000000, 7.90000000000000, 10.4000000000000, 12.9000000000000, 15.4000000000000, 17.9000000000000, 20.4000000000000, 22.9000000000000, 25.4000000000000, 27.9000000000000, 30.4000000000000, 33, 35.5000000000000, 38, 40.5000000000000 };
                            f_Kernel_SSD100 = new double[69] { 0.00229616857289160, 0.00229616857289160, 0.00107786128445593, 0.000741711291159086, 0.000514648880235850, 0.000359594986896605, 0.000252751445449457, 0.000178564502143433, 0.000126715898202455, 9.02748697137495e-05, 6.45368834830641e-05, 4.62798130717984e-05, 3.32797384239057e-05, 2.39913843168476e-05, 1.73347117478957e-05, 1.25509376420684e-05, 9.10452869104177e-06, 6.61596813114387e-06, 4.81531572784953e-06, 3.50993484295858e-06, 2.56194315908675e-06, 1.87238100278065e-06, 1.37004764965080e-06, 1.00359834293040e-06, 7.35929740029592e-07, 5.40178251681138e-07, 3.96859536147052e-07, 2.91818214459248e-07, 2.14754682578769e-07, 1.58164175411745e-07, 1.16571115303254e-07, 8.59754868813217e-08, 6.34518022859660e-08, 4.68581134597238e-08, 3.46245444873117e-08, 2.55993893735814e-08, 1.89369524173048e-08, 1.40157051733105e-08, 1.03784939886482e-08, 7.68880797257108e-09, 5.69875961288827e-09, 4.22561765004870e-09, 3.13459167790080e-09, 2.32619172869468e-09, 1.72693917846170e-09, 1.28253498016779e-09, 9.52831336180278e-10, 7.08128061175208e-10, 5.26442774928043e-10, 3.91497545627603e-10, 2.91233016688244e-10, 2.16711168045151e-10, 1.61304601581441e-10, 1.20097198168539e-10, 8.94408099662835e-11, 6.66272004515091e-11, 4.96451521260079e-11, 3.70005487546839e-11, 2.75830499157810e-11, 2.05672451491799e-11, 1.53393415235364e-11, 1.14427719749089e-11, 8.53781990007678e-12, 6.37164227496018e-12, 4.75600121351393e-12, 3.55071872124387e-12, 2.65138082268053e-12, 0, 0 };
                            f_Kernel_SSD110 = new double[69] { 0.00131326684421853, 0.00131326684421853, 0.00107550876223910, 0.000865258523255949, 0.000701910830023651, 0.000573382528292622, 0.000471177333574171, 0.000389175762490050, 0.000322880397774988, 0.000268928827928069, 0.000224770024836774, 0.000188443827188790, 0.000158427343277698, 0.000133525882988296, 0.000112794179839955, 9.54786244889187e-05, 8.09743305589817e-05, 6.87928366006158e-05, 5.85375438898245e-05, 4.98848531900508e-05, 4.25695490360893e-05, 3.63733834620842e-05, 3.11160931201533e-05, 2.66482836001098e-05, 2.28457581708017e-05, 1.96049722540625e-05, 1.68393713008687e-05, 1.44764263094059e-05, 1.24552235203518e-05, 1.07244967184416e-05, 9.24101481148259e-06, 7.96825592124164e-06, 6.87531353432841e-06, 5.93599136991982e-06, 5.12805229512980e-06, 4.43259343029285e-06, 3.83352496406641e-06, 3.31713446513124e-06, 2.87172187896074e-06, 2.48729312202785e-06, 2.15530237613438e-06, 1.86843495381135e-06, 1.62042403846486e-06, 1.40589576800268e-06, 1.22023808112187e-06, 1.05948952325347e-06, 9.20244847622028e-07, 7.99574772341235e-07, 6.94957688163326e-07, 6.04221470274032e-07, 5.25493845080879e-07, 4.57160010264753e-07, 3.97826412346704e-07, 3.46289757980597e-07, 3.01510478952892e-07, 2.62589991323931e-07, 2.28751190268042e-07, 1.99321707111934e-07, 1.73719526675074e-07, 1.51440623305482e-07, 1.32048324997902e-07, 1.15164158040538e-07, 1.00459961127181e-07, 8.76510888045884e-08, 7.64905503672732e-08, 6.67639526270581e-08, 5.82851339521715e-08, 0, 0 };
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 0.970;
                                    PDD = new double[18] { 77.5000000000000, 78.5000000000000, 83.1000000000000, 88.6000000000000, 94.3000000000000, 98.8000000000000, 100, 96, 86, 70.6000000000000, 50.9000000000000, 31.6000000000000, 15.1000000000000, 5.30000000000000, 1.50000000000000, 0.700000000000000, 0.600000000000000, 0.600000000000000 };
                                    f_SSD_ext = new double[2] { 1.6093e-4, 0.0124 };
                                    break;
                                case ("A10"):
                                    F_App = 1.002;
                                    PDD = new double[18] { 75.4000000000000, 76.4000000000000, 81.3000000000000, 87.4000000000000, 93.6000000000000, 98.5000000000000, 100, 96.1000000000000, 86.2000000000000, 70.8000000000000, 51.2000000000000, 31.3000000000000, 15.5000000000000, 5.30000000000000, 1.60000000000000, 0.800000000000000, 0.700000000000000, 0.600000000000000 };
                                    f_SSD_ext = new double[2] { 3.2190e-5, 0.0114 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[18] { 75.9000000000000, 77, 81.9000000000000, 87.8000000000000, 93.8000000000000, 98.6000000000000, 100, 96, 86.2000000000000, 70.6000000000000, 51.1000000000000, 31.6000000000000, 15.3000000000000, 5.30000000000000, 1.60000000000000, 0.800000000000000, 0.800000000000000, 0.700000000000000 };
                                    f_SSD_ext = new double[2] { 3.7179e-6, 0.0115 };
                                    break;
                                case ("A20"):
                                    F_App = 1.015;
                                    PDD = new double[18] { 77.3000000000000, 78.3000000000000, 82.9000000000000, 88.6000000000000, 94.3000000000000, 98.8000000000000, 100, 96, 86, 70.3000000000000, 51.1000000000000, 31.5000000000000, 15.2000000000000, 5.40000000000000, 1.50000000000000, 0.800000000000000, 0.700000000000000, 0.700000000000000 };
                                    f_SSD_ext = new double[2] { 4.2552e-6, 0.0109 };
                                    break;
                                case ("A25"):
                                    F_App = 1.008;
                                    PDD = new double[18] { 77.1000000000000, 78.1000000000000, 82.9000000000000, 88.6000000000000, 94.5000000000000, 98.8000000000000, 100, 96, 86.1000000000000, 70.3000000000000, 51.1000000000000, 31.7000000000000, 15.3000000000000, 5.40000000000000, 1.60000000000000, 0.800000000000000, 0.700000000000000, 0.700000000000000 };
                                    f_SSD_ext = new double[2] { 2.2866e-6, 0.0108 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9840;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;
                        case "9E":
                            DoseRef = 0.9855;
                            prof_PDD = new double[24] {0, 0.500000000000000, 2.80000000000000, 5.30000000000000, 7.80000000000000, 10.3000000000000, 12.9000000000000, 15.4000000000000, 17.9000000000000, 20.4000000000000, 22.9000000000000, 25.4000000000000, 27.9000000000000, 30.4000000000000, 32.9000000000000, 35.4000000000000, 37.9000000000000, 40.4000000000000, 42.9000000000000, 45.4000000000000, 48, 50.5000000000000, 53, 55.5000000000000};
                            f_Kernel_SSD100 = new double[69] {0.00225178975444717, 0.00225178975444717, 0.000829517059619391, 0.000619769894462617, 0.000466917073745618, 0.000354221965340039, 0.000270326514314148, 0.000207359177076400, 0.000159768948370026, 0.000123583723872967, 9.59258228413201e-05, 7.46882488273550e-05, 5.83141193982455e-05, 4.56438492351834e-05, 3.58077238557983e-05, 2.81494166976772e-05, 2.21709260719757e-05, 1.74925412372884e-05, 1.38234805767125e-05, 1.09401891299615e-05, 8.67018575878958e-06, 6.87996573560005e-06, 5.46588963101541e-06, 4.34728645928974e-06, 3.46120891382137e-06, 2.75842824719067e-06, 2.20036411642588e-06, 1.75672291392871e-06, 1.40367527205885e-06, 1.12244550590907e-06, 8.98216921396230e-07, 7.19280127349476e-07, 5.76368870653626e-07, 4.62140990608340e-07, 3.70771971974454e-07, 2.97636076634281e-07, 2.39055747546196e-07, 1.92104346860832e-07, 1.54450640501987e-07, 1.24236019128917e-07, 9.99774342244673e-08, 8.04905664816629e-08, 6.48289367908929e-08, 5.22355975164990e-08, 4.21047641416693e-08, 3.39513112847187e-08, 2.73864980394300e-08, 2.20986331107728e-08, 1.78376613147013e-08, 1.44028661457163e-08, 1.16330508425750e-08, 9.39869264641528e-09, 7.59566930479673e-09, 6.14023939028264e-09, 4.96502327144165e-09, 4.01578328745953e-09, 3.24884268853788e-09, 2.62901545938695e-09, 2.12794500843040e-09, 1.72277028729817e-09, 1.39505426892641e-09, 1.12992277879425e-09, 9.15372064028131e-10, 7.41711794455915e-10, 6.01116815185040e-10, 4.87266273767756e-10, 3.95052971294291e-10, 0, 0};
                            f_Kernel_SSD110 = new double[69] {0.00163941369487061, 0.00163941369487061, 0.000998274832349801, 0.000790146705008404, 0.000630622510118213, 0.000506824568107477, 0.000409754066163501, 0.000332974016584466, 0.000271789026257709, 0.000222716952467872, 0.000183138641351531, 0.000151059898474714, 0.000124946210846408, 0.000103605804509867, 8.61055207648045e-05, 7.17094082762122e-05, 5.98333100946903e-05, 5.00108871837727e-05, 4.18679325634985e-05, 3.51027706188256e-05, 2.94711732386703e-05, 2.47746629698526e-05, 2.08513795987125e-05, 1.75689032777287e-05, 1.48185825489489e-05, 1.25110280731125e-05, 1.05725151899819e-05, 8.94209926992751e-06, 7.56929317607403e-06, 6.41219025638038e-06, 5.43594211729735e-06, 4.61152013917370e-06, 3.91470482980126e-06, 3.32525880900652e-06, 2.82624830774160e-06, 2.40348516836915e-06, 2.04506691107487e-06, 1.74099683264370e-06, 1.48286959173165e-06, 1.26361051032185e-06, 1.07725903817668e-06, 9.18788604689581e-07, 7.83956512519994e-07, 6.69178681453877e-07, 5.71424985147426e-07, 4.88131681755094e-07, 4.17128056737186e-07, 3.56574899847546e-07, 3.04912850308663e-07, 2.60818981970869e-07, 2.23170277735611e-07, 1.91012870972290e-07, 1.63536120070196e-07, 1.40050737952201e-07, 1.19970327224647e-07, 1.02795778477287e-07, 8.81020779440879e-08, 7.55271444954060e-08, 6.47623773501247e-08, 5.55446471209074e-08, 4.76495055456214e-08, 4.08854250126542e-08, 3.50889088893968e-08, 3.01203387194316e-08, 2.58604453716353e-08, 2.22073088578974e-08, 1.90738063688681e-08, 0, 0};
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 0.990;
                                    PDD = new double[24] { 82.2000000000000, 82.8000000000000, 85.5000000000000, 88.1000000000000, 90.5000000000000, 92.9000000000000, 95.3000000000000, 97.7000000000000, 99.5000000000000, 100, 98.6000000000000, 94.8000000000000, 88.1000000000000, 78.5000000000000, 65.8000000000000, 50.8000000000000, 35.9000000000000, 21.4000000000000, 11.2000000000000, 4.70000000000000, 2, 1.30000000000000, 1.10000000000000, 1.10000000000000 };
                                    f_SSD_ext = new double[2] { 6.8673e-5, 0.0115 };
                                    break;
                                case ("A10"):
                                    F_App = 1.004;
                                    PDD = new double[24] { 79.2000000000000, 79.8000000000000, 82.7000000000000, 85.7000000000000, 88.4000000000000, 91.3000000000000, 94.2000000000000, 97, 99.3000000000000, 100, 99, 95.2000000000000, 88.8000000000000, 78.7000000000000, 66.2000000000000, 51.8000000000000, 35.4000000000000, 22.2000000000000, 11, 5, 2.20000000000000, 1.40000000000000, 1.20000000000000, 1.20000000000000 };
                                    f_SSD_ext = new double[2] { 3.3606e-12, 0.0115 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[24] {79.3000000000000, 80, 83.1000000000000, 86, 88.8000000000000, 91.6000000000000, 94.4000000000000, 97.2000000000000, 99.2000000000000, 100, 98.9000000000000, 95.2000000000000, 88.5000000000000, 78.6000000000000, 66.2000000000000, 50.9000000000000, 35.9000000000000, 22, 11, 4.90000000000000, 2.10000000000000, 1.30000000000000, 1.20000000000000, 1.20000000000000};
                                    f_SSD_ext = new double[2] { 2.2204e-14, 0.0111 };
                                    break;
                                case ("A20"):
                                    F_App = 0.99;
                                    PDD = new double[24] {80.8000000000000, 81.4000000000000, 84.1000000000000, 86.8000000000000, 89.3000000000000, 92, 94.7000000000000, 97.4000000000000, 99.4000000000000, 100, 98.8000000000000, 95, 88.3000000000000, 78.6000000000000, 66, 51.3000000000000, 36.2000000000000, 21.8000000000000, 11.1000000000000, 5, 2.10000000000000, 1.40000000000000, 1.20000000000000, 1.20000000000000};
                                    f_SSD_ext = new double[2] { 2.2205e-14, 0.0107 };
                                    break;
                                case ("A25"):
                                    F_App = 0.967;
                                    PDD = new double[24] {80.2000000000000, 80.8000000000000, 83.8000000000000, 86.5000000000000, 88.9000000000000, 91.7000000000000, 94.7000000000000, 97.2000000000000, 99.3000000000000, 100, 98.7000000000000, 95.2000000000000, 88.8000000000000, 79, 66.1000000000000, 52, 36.2000000000000, 22.1000000000000, 11.4000000000000, 4.90000000000000, 2.20000000000000, 1.30000000000000, 1.30000000000000, 1.20000000000000};
                                    f_SSD_ext = new double[2] { 3.0068e-6, 0.0107 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9230;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;
                        case "12E":
                            DoseRef = 0.9856;
                            prof_PDD = new double[32] { 0, 0.500000000000000, 2.80000000000000, 5.30000000000000, 7.80000000000000, 10.3000000000000, 12.8000000000000, 15.3000000000000, 17.8000000000000, 20.3000000000000, 22.8000000000000, 25.3000000000000, 27.8000000000000, 30.3000000000000, 32.9000000000000, 35.4000000000000, 37.9000000000000, 40.4000000000000, 42.9000000000000, 45.4000000000000, 47.9000000000000, 50.4000000000000, 52.9000000000000, 55.4000000000000, 57.9000000000000, 60.4000000000000, 62.9000000000000, 65.4000000000000, 68, 70.5000000000000, 73, 75.5000000000000};
                            f_Kernel_SSD100 = new double[69] { 0.00222360402836368, 0.00222360402836368, 0.000788844928271251, 0.000599214339247438, 0.000458962160902147, 0.000353995727491348, 0.000274660737609195, 0.000214198588018351, 0.000167791951480969, 0.000131954872492131, 0.000104132211708527, 8.24303685914726e-05, 6.54325871255881e-05, 5.20700551980576e-05, 4.15305704986670e-05, 3.31929603226028e-05, 2.65794399711396e-05, 2.13206432905450e-05, 1.71297178612185e-05, 1.37829772307396e-05, 1.11053440179342e-05, 8.95932508368418e-06, 7.23661191954040e-06, 5.85164659043912e-06, 4.73666913586729e-06, 3.83788832136644e-06, 3.11250889257817e-06, 2.52641515947631e-06, 2.05235983663881e-06, 1.66854348248350e-06, 1.35749707152981e-06, 1.10520066097769e-06, 9.00386545471612e-07, 7.33987014070905e-07, 5.98695767150031e-07, 4.88618906529709e-07, 3.98996688806087e-07, 3.25981308955613e-07, 2.66459142783961e-07, 2.17908336416987e-07, 1.78284550636867e-07, 1.45929170251999e-07, 1.19495467809957e-07, 9.78891386466180e-08, 8.02203559212044e-08, 6.57650725526024e-08, 5.39337550463962e-08, 4.42460976727299e-08, 3.63105545089731e-08, 2.98077570310161e-08, 2.44770685803235e-08, 2.01056737738133e-08, 1.65197183213943e-08, 1.35771088007119e-08, 1.11616574203696e-08, 9.17831743494838e-09, 7.54930365503413e-09, 6.21093174625307e-09, 5.11104168199209e-09, 4.20689624049245e-09, 3.46346605587891e-09, 2.85202942961373e-09, 2.34902856705346e-09, 1.93513485191003e-09, 1.59448461061352e-09, 1.31405400209076e-09, 1.08314749263959e-09, 0, 0};
                            f_Kernel_SSD110 = new double[69] { 0.00185023831888518, 0.00185023831888518, 0.000914930486119341, 0.000717564797123039, 0.000567463947186061, 0.000451899519207342, 0.000362011950000239, 0.000291491163911364, 0.000235755797682870, 0.000191425219546241, 0.000155970075022195, 0.000127475256972070, 0.000104475681396995, 8.58403785492990e-05, 7.06893436068017e-05, 5.83330258475230e-05, 4.82277287851379e-05, 3.99423629657526e-05, 3.31334093075703e-05, 2.75258931839379e-05, 2.28988073823739e-05, 1.90738607768645e-05, 1.59067356029967e-05, 1.32802525546209e-05, 1.10989976809148e-05, 9.28507700785596e-06, 7.77474661183534e-06, 6.51572621521172e-06, 5.46504927242592e-06, 4.58733619257018e-06, 3.85340279317262e-06, 3.23913544399909e-06, 2.72457918848106e-06, 2.29319655402582e-06, 1.93126361172973e-06, 1.62737673664190e-06, 1.37204891109933e-06, 1.15737865097269e-06, 9.76777979055238e-07, 8.24748519782810e-07, 6.96696896996785e-07, 5.88782298320521e-07, 4.97790416308140e-07, 4.21029057867905e-07, 3.56241584379870e-07, 3.01535048209225e-07, 2.55320460635843e-07, 2.16263088291733e-07, 1.83241050949485e-07, 1.55310799801625e-07, 1.31678305505626e-07, 1.11674989947090e-07, 9.47376034311255e-08, 8.03913867892885e-08, 6.82359711547868e-08, 5.79335615090111e-08, 4.91990270446628e-08, 4.17915849906120e-08, 3.55078171314155e-08, 3.01758017841420e-08, 2.56501801241911e-08, 2.18080057053511e-08, 1.85452509189150e-08, 1.57738648670709e-08, 1.34192943497304e-08, 1.14183940474509e-08, 9.71766397684929e-09, 0, 0};
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 0.9830;
                                    PDD = new double[32] {87.3000000000000, 87.8000000000000, 90.1000000000000, 91.8000000000000, 93.3000000000000, 94.5000000000000, 95.6000000000000, 96.6000000000000, 97.7000000000000, 98.7000000000000, 99.6000000000000, 100, 100, 99.3000000000000, 97.6000000000000, 94.3000000000000, 89.6000000000000, 83.1000000000000, 74.5000000000000, 64.7000000000000, 53.5000000000000, 41.8000000000000, 30.3000000000000, 20.3000000000000, 12.2000000000000, 6.70000000000000, 3.70000000000000, 2.40000000000000, 1.90000000000000, 1.80000000000000, 1.70000000000000, 1.80000000000000};
                                    f_SSD_ext = new double[2] { 2.2478e-5, 0.0116 };
                                    break;
                                case ("A10"):
                                    F_App = 1.007;
                                    PDD = new double[32] {84.1000000000000, 84.6000000000000, 87, 89, 90.6000000000000, 92, 93.3000000000000, 94.8000000000000, 96, 97.4000000000000, 98.7000000000000, 99.6000000000000, 100, 99.8000000000000, 98.3000000000000, 95.5000000000000, 90.9000000000000, 84.4000000000000, 76.3000000000000, 65.3000000000000, 54.4000000000000, 42.5000000000000, 31.3000000000000, 20.4000000000000, 12.5000000000000, 7.10000000000000, 3.90000000000000, 2.60000000000000, 2.10000000000000, 2, 1.90000000000000, 1.90000000000000};
                                    f_SSD_ext = new double[2] { 5.3136e-6, 0.0113 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[32] {84.1000000000000, 84.6000000000000, 87, 89, 90.6000000000000, 91.9000000000000, 93.3000000000000, 94.8000000000000, 96.2000000000000, 97.4000000000000, 98.7000000000000, 99.5000000000000, 100, 99.6000000000000, 98.3000000000000, 95.4000000000000, 90.8000000000000, 84.7000000000000, 76, 66.1000000000000, 54.2000000000000, 42.7000000000000, 30.6000000000000, 20.5000000000000, 12.8000000000000, 6.90000000000000, 4, 2.50000000000000, 2.10000000000000, 2, 2, 2};
                                    f_SSD_ext = new double[2] { 6.1756e-6, 0.0110 };
                                    break;
                                case ("A20"):
                                    F_App = 0.9860;
                                    PDD = new double[32] {86.2000000000000, 86.7000000000000, 88.9000000000000, 90.8000000000000, 92.1000000000000, 93.3000000000000, 94.6000000000000, 95.7000000000000, 96.9000000000000, 98.1000000000000, 98.9000000000000, 99.8000000000000, 100, 99.6000000000000, 98.2000000000000, 95.2000000000000, 90.7000000000000, 84.3000000000000, 75.8000000000000, 65.5000000000000, 54.1000000000000, 42.7000000000000, 31.2000000000000, 20.2000000000000, 12.3000000000000, 7.30000000000000, 4.10000000000000, 2.70000000000000, 2.30000000000000, 2.20000000000000, 2.20000000000000, 2.10000000000000};
                                    f_SSD_ext = new double[2] { 2.2205e-14, 0.0107 };
                                    break;
                                case ("A25"):
                                    F_App = 0.9590;
                                    PDD = new double[32] {85.5000000000000, 86, 88, 90.1000000000000, 91.4000000000000, 92.8000000000000, 94, 95.1000000000000, 96.4000000000000, 97.6000000000000, 98.6000000000000, 99.6000000000000, 100, 99.6000000000000, 98.2000000000000, 95.4000000000000, 91.1000000000000, 84.4000000000000, 76.5000000000000, 66.1000000000000, 55.3000000000000, 42.9000000000000, 31.9000000000000, 21.1000000000000, 12.9000000000000, 7.30000000000000, 4.20000000000000, 2.80000000000000, 2.30000000000000, 2.30000000000000, 2.20000000000000, 2.20000000000000};
                                    f_SSD_ext = new double[2] { 2.2204e-14, 0.0107 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9120;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;
                        case "16E":
                            DoseRef = 0.9785;
                            prof_PDD = new double[42] {0, 0.500000000000000, 2.70000000000000, 5.20000000000000, 7.70000000000000, 10.3000000000000, 12.8000000000000, 15.3000000000000, 17.8000000000000, 20.3000000000000, 22.8000000000000, 25.3000000000000, 27.8000000000000, 30.3000000000000, 32.8000000000000, 35.3000000000000, 37.8000000000000, 40.3000000000000, 42.8000000000000, 45.3000000000000, 47.9000000000000, 50.4000000000000, 52.9000000000000, 55.4000000000000, 57.9000000000000, 60.4000000000000, 62.9000000000000, 65.4000000000000, 67.9000000000000, 70.4000000000000, 72.9000000000000, 75.4000000000000, 77.9000000000000, 80.4000000000000, 82.9000000000000, 85.5000000000000, 88, 90.5000000000000, 93, 95.5000000000000, 98, 100.500000000000};
                            f_Kernel_SSD100 = new double[69] {0.00199477418017233, 0.00199477418017233, 0.000860872118207018, 0.000669219783676505, 0.000524569486342650, 0.000414060433500295, 0.000328777329293750, 0.000262398564078465, 0.000210356310369688, 0.000169297061540110, 0.000136725226239191, 0.000110761892868298, 8.99780938376475e-05, 7.32774425488580e-05, 5.98121937909918e-05, 4.89223590595092e-05, 4.00909917064645e-05, 3.29109824172586e-05, 2.70601575859621e-05, 2.22824386476179e-05, 1.83734734623886e-05, 1.51695994704269e-05, 1.25393109296626e-05, 1.03766231948601e-05, 8.59588459614558e-06, 7.12770027297023e-06, 5.91571531325993e-06, 4.91406563447166e-06, 4.08535038821771e-06, 3.39901359396253e-06, 2.83004827081894e-06, 2.35795572927025e-06, 1.96590748781332e-06, 1.64006864438557e-06, 1.36905030444501e-06, 1.14346547190540e-06, 9.55568111724585e-07, 7.98959243002529e-07, 6.68347182663856e-07, 5.59351632099562e-07, 4.68343335238441e-07, 3.92312653505642e-07, 3.28761691170632e-07, 2.75615633641561e-07, 2.31149785583500e-07, 1.93929457764645e-07, 1.62760384493491e-07, 1.36647783518842e-07, 1.14762517954455e-07, 9.64131015070738e-08, 8.10225169217183e-08, 6.81090035192834e-08, 5.72701211416850e-08, 4.81695214043188e-08, 4.05259580976748e-08, 3.41041512166481e-08, 2.87071867776568e-08, 2.41701901181742e-08, 2.03550560129251e-08, 1.71460564350645e-08, 1.44461777096479e-08, 1.21740642428401e-08, 1.02614670305806e-08, 8.65111249922810e-09, 7.29492155466109e-09, 6.15252060188482e-09, 5.18999609657297e-09, 0, 0};
                            f_Kernel_SSD110 = new double[69] {0.00181023748050264, 0.00181023748050264, 0.000910912033234258, 0.000718210500484208, 0.000570993528157430, 0.000457127213101735, 0.000368146256652323, 0.000298006105453381, 0.000242306148573287, 0.000197789618240868, 0.000162012269163522, 0.000133117393497662, 0.000109679738661148, 9.05951825099975e-05, 7.50014594691284e-05, 6.22203651708081e-05, 5.17150735048451e-05, 4.30582500698812e-05, 3.59079848049359e-05, 2.99894577286602e-05, 2.50808552685954e-05, 2.10024699422449e-05, 1.76082060380238e-05, 1.47789189906295e-05, 1.24171629518224e-05, 1.04430273458955e-05, 8.79082094827720e-06, 7.40641944175259e-06, 6.24513517982548e-06, 5.27000001478136e-06, 4.45037636225631e-06, 3.76083020450746e-06, 3.18021394944977e-06, 2.69091803341228e-06, 2.27825867079504e-06, 1.92997579809730e-06, 1.63582046981365e-06, 1.38721506798579e-06, 1.17697293432528e-06, 9.99066613307944e-07, 8.48435951405277e-07, 7.20828943545765e-07, 6.12669539338671e-07, 5.20947685943427e-07, 4.43127744204013e-07, 3.77072111088928e-07, 3.20977447083902e-07, 2.73321367666418e-07, 2.32817833758153e-07, 1.98379783391264e-07, 1.69087798678824e-07, 1.44163809005961e-07, 1.22949001492920e-07, 1.04885250013817e-07, 8.94994898073813e-08, 7.63905603990841e-08, 6.52181188435690e-08, 5.56932909684687e-08, 4.75707829047288e-08, 4.06422205385498e-08, 3.47305222939546e-08, 2.96851421412760e-08, 2.53780459777583e-08, 2.17003064772034e-08, 1.85592198393064e-08, 1.58758632291527e-08, 1.35830245465289e-08, 0, 0};
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 0.9960;
                                    PDD = new double[42] {91, 91.5000000000000, 93.7000000000000, 95.3000000000000, 96.4000000000000, 97.2000000000000, 97.8000000000000, 98.4000000000000, 98.9000000000000, 99.3000000000000, 99.7000000000000, 99.9000000000000, 100, 100, 99.7000000000000, 99.3000000000000, 98.4000000000000, 97.2000000000000, 95.3000000000000, 92.8000000000000, 89.5000000000000, 85.5000000000000, 80.6000000000000, 75, 68.4000000000000, 61.5000000000000, 53.4000000000000, 44.9000000000000, 36.4000000000000, 28.6000000000000, 21.2000000000000, 14.8000000000000, 10.3000000000000, 6.90000000000000, 5, 4, 3.60000000000000, 3.50000000000000, 3.40000000000000, 3.40000000000000, 3.40000000000000, 3.30000000000000};
                                    f_SSD_ext = new double[2] { 8.2710e-6, 0.0112 };
                                    break;
                                case ("A10"):
                                    F_App = 1.011;
                                    PDD = new double[42] {89.7000000000000, 90.2000000000000, 92.4000000000000, 94.1000000000000, 95.3000000000000, 96.1000000000000, 96.8000000000000, 97.4000000000000, 97.9000000000000, 98.5000000000000, 98.8000000000000, 99.1000000000000, 99.6000000000000, 99.8000000000000, 100, 99.9000000000000, 99.6000000000000, 98.9000000000000, 97.7000000000000, 95.6000000000000, 92.9000000000000, 89.3000000000000, 84.7000000000000, 79.1000000000000, 72.4000000000000, 65, 56.4000000000000, 47.6000000000000, 38.7000000000000, 29.7000000000000, 22.1000000000000, 15.5000000000000, 10.7000000000000, 7.40000000000000, 5.40000000000000, 4.30000000000000, 3.90000000000000, 3.70000000000000, 3.70000000000000, 3.70000000000000, 3.60000000000000, 3.60000000000000};
                                    f_SSD_ext = new double[2] { 9.6936e-6, 0.0109 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[42] {89.2000000000000, 89.7000000000000, 92.1000000000000, 93.8000000000000, 94.9000000000000, 95.9000000000000, 96.7000000000000, 97.4000000000000, 97.8000000000000, 98.4000000000000, 98.8000000000000, 99.3000000000000, 99.6000000000000, 99.9000000000000, 100, 100, 99.7000000000000, 98.9000000000000, 97.8000000000000, 95.8000000000000, 92.9000000000000, 89.4000000000000, 84.8000000000000, 79.4000000000000, 72.2000000000000, 64.6000000000000, 56.6000000000000, 47.7000000000000, 38.7000000000000, 29.8000000000000, 22.4000000000000, 15.9000000000000, 10.8000000000000, 7.30000000000000, 5.40000000000000, 4.40000000000000, 3.90000000000000, 3.80000000000000, 3.80000000000000, 3.70000000000000, 3.70000000000000, 3.70000000000000 };
                                    f_SSD_ext = new double[2] { 7.0751e-6, 0.0107 };
                                    break;
                                case ("A20"):
                                    F_App = 0.990;
                                    PDD = new double[42] {90.6000000000000, 91, 93.1000000000000, 94.6000000000000, 95.8000000000000, 96.7000000000000, 97.3000000000000, 97.9000000000000, 98.4000000000000, 98.8000000000000, 99.1000000000000, 99.4000000000000, 99.7000000000000, 100, 100, 99.8000000000000, 99.5000000000000, 98.7000000000000, 97.3000000000000, 95.4000000000000, 92.5000000000000, 89.1000000000000, 84.5000000000000, 79, 72.6000000000000, 64.9000000000000, 56.3000000000000, 47.9000000000000, 38.6000000000000, 30.6000000000000, 22.3000000000000, 16.1000000000000, 11.1000000000000, 7.40000000000000, 5.50000000000000, 4.50000000000000, 4, 3.90000000000000, 3.90000000000000, 3.80000000000000, 3.70000000000000, 3.80000000000000};
                                    f_SSD_ext = new double[2] { 2.0649e-6, 0.0101 };
                                    break;
                                case ("A25"):
                                    F_App = 0.9580;
                                    PDD = new double[42] {90.3000000000000, 90.8000000000000, 92.9000000000000, 94.4000000000000, 95.6000000000000, 96.4000000000000, 97.1000000000000, 97.8000000000000, 98.2000000000000, 98.6000000000000, 99, 99.4000000000000, 99.7000000000000, 99.8000000000000, 100, 99.9000000000000, 99.7000000000000, 98.9000000000000, 97.5000000000000, 95.8000000000000, 92.9000000000000, 89.6000000000000, 85.1000000000000, 79.7000000000000, 73.1000000000000, 65.1000000000000, 57.3000000000000, 48.4000000000000, 39.3000000000000, 30.8000000000000, 22.7000000000000, 16.2000000000000, 11.1000000000000, 7.70000000000000, 5.70000000000000, 4.50000000000000, 4.20000000000000, 4, 4, 3.90000000000000, 3.90000000000000, 4 };
                                    f_SSD_ext = new double[2] { 2.2205e-14, 0.0132 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9060;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;
                        case "20E":
                            DoseRef = 0.9200;
                            prof_PDD = new double[50] {0, 0.500000000000000, 2.70000000000000, 5.20000000000000, 7.70000000000000, 10.3000000000000, 12.8000000000000, 15.3000000000000, 17.8000000000000, 20.3000000000000, 22.8000000000000, 25.3000000000000, 27.8000000000000, 30.3000000000000, 32.8000000000000, 35.3000000000000, 37.8000000000000, 40.3000000000000, 42.8000000000000, 45.3000000000000, 47.9000000000000, 50.4000000000000, 52.9000000000000, 55.4000000000000, 57.9000000000000, 60.4000000000000, 62.9000000000000, 65.4000000000000, 67.9000000000000, 70.4000000000000, 72.9000000000000, 75.4000000000000, 77.9000000000000, 80.4000000000000, 83, 85.5000000000000, 88, 90.5000000000000, 93, 95.5000000000000, 98, 100.500000000000, 103, 105.500000000000, 108, 110.500000000000, 113, 115.500000000000, 118, 120.500000000000};
                            f_Kernel_SSD100 = new double[69] {0.00179474382000392, 0.00179474382000392, 0.000928313817826219, 0.000731065199048851, 0.000580525806707020, 0.000464208838644442, 0.000373407212952800, 0.000301907192992406, 0.000245187721366800, 0.000199905047393512, 0.000163551358899355, 0.000134223031057326, 0.000110459895521365, 9.11316660288042e-05, 7.53563587010466e-05, 6.24408390706007e-05, 5.18369339052322e-05, 4.31086596209636e-05, 3.59074993361027e-05, 2.99535793008733e-05, 2.50212175829462e-05, 2.09277459133345e-05, 1.75248043413620e-05, 1.46915196281385e-05, 1.23291295519650e-05, 1.03567247792564e-05, 8.70786002096416e-06, 7.32784530766283e-06, 6.17157224399205e-06, 5.20176316231450e-06, 4.38755610638350e-06, 3.70335763055072e-06, 3.12791001179759e-06, 2.64353074450352e-06, 2.23549093363762e-06, 1.89150602317302e-06, 1.60131764263129e-06, 1.35634956406730e-06, 1.14942409038437e-06, 9.74527838511171e-07, 8.26617987022381e-07, 7.01461742022670e-07, 5.95503126582727e-07, 5.05752286787267e-07, 4.29693385574274e-07, 3.65207866355777e-07, 3.10510445315228e-07, 2.64095660588466e-07, 2.24693189291594e-07, 1.91230456104840e-07, 1.62801313261378e-07, 1.38639781956428e-07, 1.18098017936342e-07, 1.00627806291931e-07, 8.57650078057114e-08, 7.31164761513151e-08, 6.23490454284188e-08, 5.31802539641669e-08, 4.53705254484001e-08, 3.87165742454984e-08, 3.30458398328475e-08, 2.82117870124285e-08, 2.40899349927528e-08, 2.05745004931766e-08, 1.75755584545195e-08, 1.50166393509572e-08, 1.28326949863595e-08, 0, 0};
                            f_Kernel_SSD110 = new double[69] {0.00173967181065150, 0.00173967181065150, 0.000914527328094953, 0.000726038163327676, 0.000581200976797317, 0.000468510875102329, 0.000379918498157550, 0.000309658255887813, 0.000253518342450333, 0.000208370337382824, 0.000171857210449298, 0.000142181178361748, 0.000117956305465611, 9.81041341992113e-05, 8.17785432777406e-05, 6.83108456897054e-05, 5.71691457995060e-05, 4.79278980946868e-05, 4.02448664751135e-05, 3.38435195886631e-05, 2.84994646032180e-05, 2.40299120747915e-05, 2.02854371821853e-05, 1.71434955753160e-05, 1.45032903727512e-05, 1.22816870672055e-05, 1.04099464943957e-05, 8.83110029208561e-06, 7.49783376587986e-06, 6.37077153757894e-06, 5.41708444259204e-06, 4.60935377361557e-06, 3.92464251956753e-06, 3.34373373051243e-06, 2.85050429362775e-06, 2.43140878354720e-06, 2.07505306486722e-06, 1.77184128566798e-06, 1.51368304376184e-06, 1.29375001158262e-06, 1.10627330868590e-06, 9.46374519055143e-07, 8.09924545998205e-07, 6.93425544648255e-07, 5.93912021087618e-07, 5.08867877470210e-07, 4.36156745349363e-07, 3.73963409506954e-07, 3.20744501563565e-07, 2.75186952274953e-07, 2.36172946256256e-07, 2.02750333065285e-07, 1.74107622260104e-07, 1.49552833857091e-07, 1.28495594870411e-07, 1.10431971691176e-07, 9.49316105043348e-08, 8.16268266350025e-08, 7.02033410303750e-08, 6.03924100057289e-08, 5.19641344467120e-08, 4.47217682413002e-08, 3.84968738856278e-08, 3.31451968441614e-08, 2.85431501298032e-08, 2.45848172911644e-08, 2.11793960866703e-08, 0, 0};
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 1.016;
                                    PDD = new double[50] {91.8000000000000, 92.3000000000000, 94.4000000000000, 96.1000000000000, 97.2000000000000, 98, 98.6000000000000, 99, 99.4000000000000, 99.7000000000000, 99.9000000000000, 100, 99.9000000000000, 99.8000000000000, 99.5000000000000, 99.2000000000000, 98.8000000000000, 98.4000000000000, 97.4000000000000, 96.6000000000000, 95.4000000000000, 94.1000000000000, 92.5000000000000, 90.5000000000000, 88.4000000000000, 85.7000000000000, 82.7000000000000, 79.4000000000000, 75.5000000000000, 71.5000000000000, 67, 62.1000000000000, 57.2000000000000, 51.9000000000000, 46.3000000000000, 40.7000000000000, 34.9000000000000, 29.5000000000000, 24.3000000000000, 19.5000000000000, 15.5000000000000, 12.1000000000000, 9.60000000000000, 7.70000000000000, 6.70000000000000, 6, 5.60000000000000, 5.50000000000000, 5.40000000000000, 5.30000000000000};
                                    f_SSD_ext = new double[2] { 5.3848e-6, 0.0109 };
                                    break;
                                case ("A10"):
                                    F_App = 1.025;
                                    PDD = new double[50] {93.1000000000000, 93.6000000000000, 95.7000000000000, 97.3000000000000, 98.2000000000000, 99, 99.2000000000000, 99.6000000000000, 99.8000000000000, 99.9000000000000, 100, 99.9000000000000, 99.8000000000000, 99.8000000000000, 99.5000000000000, 99.3000000000000, 99, 98.7000000000000, 98.1000000000000, 97.4000000000000, 96.7000000000000, 95.8000000000000, 94.8000000000000, 93.6000000000000, 91.9000000000000, 90.1000000000000, 87.7000000000000, 85, 81.7000000000000, 78.1000000000000, 73.8000000000000, 69, 63.7000000000000, 57.8000000000000, 51.8000000000000, 45.4000000000000, 39, 32.7000000000000, 26.9000000000000, 21.6000000000000, 16.9000000000000, 13.2000000000000, 10.4000000000000, 8.50000000000000, 7.20000000000000, 6.50000000000000, 6.10000000000000, 5.90000000000000, 5.80000000000000, 5.80000000000000};
                                    f_SSD_ext = new double[2] { 6.9325e-6, 0.0108 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[50] {92.4000000000000, 92.9000000000000, 94.9000000000000, 96.6000000000000, 97.7000000000000, 98.4000000000000, 98.9000000000000, 99.3000000000000, 99.6000000000000, 99.8000000000000, 99.9000000000000, 100, 100, 99.9000000000000, 99.8000000000000, 99.7000000000000, 99.5000000000000, 99.1000000000000, 98.7000000000000, 98.2000000000000, 97.6000000000000, 96.6000000000000, 95.7000000000000, 94.5000000000000, 92.8000000000000, 91, 88.6000000000000, 85.9000000000000, 82.7000000000000, 79, 74.8000000000000, 70, 64.6000000000000, 58.7000000000000, 52.7000000000000, 46.2000000000000, 39.8000000000000, 33.2000000000000, 27.5000000000000, 21.9000000000000, 17.1000000000000, 13.5000000000000, 10.6000000000000, 8.60000000000000, 7.30000000000000, 6.50000000000000, 6.10000000000000, 6, 5.90000000000000, 5.90000000000000};
                                    f_SSD_ext = new double[2] { 4.4621e-10, 0.0106 };
                                    break;
                                case ("A20"):
                                    F_App = 0.9840;
                                    PDD = new double[50] {92.5000000000000, 92.9000000000000, 94.9000000000000, 96.6000000000000, 97.8000000000000, 98.4000000000000, 99, 99.3000000000000, 99.6000000000000, 99.8000000000000, 99.9000000000000, 100, 100, 100, 99.9000000000000, 99.6000000000000, 99.4000000000000, 99, 98.5000000000000, 98, 97.3000000000000, 96.4000000000000, 95.4000000000000, 94.1000000000000, 92.6000000000000, 90.8000000000000, 88.4000000000000, 85.7000000000000, 82.5000000000000, 78.9000000000000, 74.5000000000000, 69.8000000000000, 64.6000000000000, 58.7000000000000, 52.5000000000000, 46.4000000000000, 40, 33.5000000000000, 27.7000000000000, 22.2000000000000, 17.5000000000000, 13.6000000000000, 10.8000000000000, 8.70000000000000, 7.50000000000000, 6.70000000000000, 6.40000000000000, 6.10000000000000, 6.10000000000000, 6.10000000000000};
                                    f_SSD_ext = new double[2] { 2.9832e-7, 0.0101 };
                                    break;
                                case ("A25"):
                                    F_App = 0.950;
                                    PDD = new double[50] {92.3000000000000, 92.7000000000000, 94.8000000000000, 96.5000000000000, 97.5000000000000, 98.3000000000000, 98.8000000000000, 99.2000000000000, 99.6000000000000, 99.7000000000000, 99.8000000000000, 99.9000000000000, 100, 99.9000000000000, 99.8000000000000, 99.6000000000000, 99.3000000000000, 98.9000000000000, 98.7000000000000, 98.2000000000000, 97.4000000000000, 96.8000000000000, 95.7000000000000, 94.5000000000000, 93, 91.1000000000000, 88.8000000000000, 86.3000000000000, 83, 79.4000000000000, 75.3000000000000, 70.4000000000000, 65.1000000000000, 59.2000000000000, 53.3000000000000, 46.9000000000000, 40.3000000000000, 34.2000000000000, 28, 22.4000000000000, 17.9000000000000, 13.8000000000000, 10.9000000000000, 8.90000000000000, 7.50000000000000, 6.90000000000000, 6.50000000000000, 6.30000000000000, 6.10000000000000, 6.20000000000000};
                                    f_SSD_ext = new double[2] { 9.8604e-9, 0.0103 };
                                    break;
                                case ("A40"):
                                    F_App = 0.8970;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;
                    }
                            break;
                case ("D2300-CD"):
                    switch (Energia)
                    {
                        case "6E":
                            DoseRef = 0.9866;
                            prof_PDD = new double[40] { 0, 0.50000, 2.7000, 5.3000, 7.8000, 10.300, 12.800, 15.300, 17.800, 20.300, 22.800, 25.300, 27.800, 30.300, 32.800, 35.300, 37.900, 40.400, 42.900, 45.400, 47.900, 50.400, 52.900, 55.400, 57.900, 60.400, 62.900, 65.400, 67.900, 70.400, 73, 75.500, 78, 80.500, 83, 85.5, 88, 90.5, 93, 95.5 };
                            f_Kernel_SSD100 = new double[69] { 0.00229616857289160, 0.00229616857289160, 0.00107786128445593, 0.000741711291159086, 0.000514648880235850, 0.000359594986896605, 0.000252751445449457, 0.000178564502143433, 0.000126715898202455, 9.02748697137495e-05, 6.45368834830641e-05, 4.62798130717984e-05, 3.32797384239057e-05, 2.39913843168476e-05, 1.73347117478957e-05, 1.25509376420684e-05, 9.10452869104177e-06, 6.61596813114387e-06, 4.81531572784953e-06, 3.50993484295858e-06, 2.56194315908675e-06, 1.87238100278065e-06, 1.37004764965080e-06, 1.00359834293040e-06, 7.35929740029592e-07, 5.40178251681138e-07, 3.96859536147052e-07, 2.91818214459248e-07, 2.14754682578769e-07, 1.58164175411745e-07, 1.16571115303254e-07, 8.59754868813217e-08, 6.34518022859660e-08, 4.68581134597238e-08, 3.46245444873117e-08, 2.55993893735814e-08, 1.89369524173048e-08, 1.40157051733105e-08, 1.03784939886482e-08, 7.68880797257108e-09, 5.69875961288827e-09, 4.22561765004870e-09, 3.13459167790080e-09, 2.32619172869468e-09, 1.72693917846170e-09, 1.28253498016779e-09, 9.52831336180278e-10, 7.08128061175208e-10, 5.26442774928043e-10, 3.91497545627603e-10, 2.91233016688244e-10, 2.16711168045151e-10, 1.61304601581441e-10, 1.20097198168539e-10, 8.94408099662835e-11, 6.66272004515091e-11, 4.96451521260079e-11, 3.70005487546839e-11, 2.75830499157810e-11, 2.05672451491799e-11, 1.53393415235364e-11, 1.14427719749089e-11, 8.53781990007678e-12, 6.37164227496018e-12, 4.75600121351393e-12, 3.55071872124387e-12, 2.65138082268053e-12, 0, 0 };
                            f_Kernel_SSD110 = new double[69] { 0.00131326684421853, 0.00131326684421853, 0.00107550876223910, 0.000865258523255949, 0.000701910830023651, 0.000573382528292622, 0.000471177333574171, 0.000389175762490050, 0.000322880397774988, 0.000268928827928069, 0.000224770024836774, 0.000188443827188790, 0.000158427343277698, 0.000133525882988296, 0.000112794179839955, 9.54786244889187e-05, 8.09743305589817e-05, 6.87928366006158e-05, 5.85375438898245e-05, 4.98848531900508e-05, 4.25695490360893e-05, 3.63733834620842e-05, 3.11160931201533e-05, 2.66482836001098e-05, 2.28457581708017e-05, 1.96049722540625e-05, 1.68393713008687e-05, 1.44764263094059e-05, 1.24552235203518e-05, 1.07244967184416e-05, 9.24101481148259e-06, 7.96825592124164e-06, 6.87531353432841e-06, 5.93599136991982e-06, 5.12805229512980e-06, 4.43259343029285e-06, 3.83352496406641e-06, 3.31713446513124e-06, 2.87172187896074e-06, 2.48729312202785e-06, 2.15530237613438e-06, 1.86843495381135e-06, 1.62042403846486e-06, 1.40589576800268e-06, 1.22023808112187e-06, 1.05948952325347e-06, 9.20244847622028e-07, 7.99574772341235e-07, 6.94957688163326e-07, 6.04221470274032e-07, 5.25493845080879e-07, 4.57160010264753e-07, 3.97826412346704e-07, 3.46289757980597e-07, 3.01510478952892e-07, 2.62589991323931e-07, 2.28751190268042e-07, 1.99321707111934e-07, 1.73719526675074e-07, 1.51440623305482e-07, 1.32048324997902e-07, 1.15164158040538e-07, 1.00459961127181e-07, 8.76510888045884e-08, 7.64905503672732e-08, 6.67639526270581e-08, 5.82851339521715e-08, 0, 0 };
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 0.9250;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                                                       
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                                case ("A10"):
                                    F_App = 1.001;
                                    PDD = new double[40] { 74.700, 75.600, 79.800, 85.500, 91.700, 97.100, 100, 98.300, 91, 77.400, 58.900, 38.300, 20, 7.7000, 2.2000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.50000, 0.50000, 0.50000, 0.50000, 0, 0, 0, 0, 0, 0 };
                                    f_SSD_ext = new double[2] { 3.3215e-5, 0.0113 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[40] { 75.1000000000000, 76, 80.2000000000000, 85.8000000000000, 91.9000000000000, 97.2000000000000, 100, 98.3000000000000, 90.8000000000000, 77.1000000000000, 58.5000000000000, 37.9000000000000, 19.6000000000000, 7.50000000000000, 2.10000000000000, 0.800000000000000, 0.600000000000000, 0.600000000000000, 0.600000000000000, 0.600000000000000, 0.600000000000000, 0.600000000000000, 0.600000000000000, 0.600000000000000, 0.600000000000000, 0.500000000000000, 0.600000000000000, 0.500000000000000, 0.500000000000000, 0.600000000000000, 0.600000000000000, 0.500000000000000, 0.500000000000000, 0.500000000000000, 0, 0, 0, 0, 0, 0 };
                                    f_SSD_ext = new double[2] { 1.4741e-5, 0.011 };
                                    break;
                                case ("A20"):
                                    F_App = 1.01;
                                    PDD = new double[40] { 76.2000000000000, 77.2000000000000, 81.8000000000000, 87.4000000000000, 93.3000000000000, 98.2000000000000, 100, 96.9000000000000, 87.9000000000000, 73.2000000000000, 53.8000000000000, 33.3000000000000, 16.1000000000000, 5.50000000000000, 1.40000000000000, 0.500000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.400000000000000, 0.300000000000000, 0.400000000000000, 0.300000000000000, 0.400000000000000, 0.300000000000000, 0.300000000000000, 0.300000000000000, 0.300000000000000, 0, 0, 0, 0, 0, 0 };
                                    f_SSD_ext = new double[2] { 8.8437e-6, 0.0108 };
                                    break;
                                case ("A25"):
                                    F_App = 1.006;
                                    PDD = new double[40] { 76.4000000000000, 77.3000000000000, 81.5000000000000, 86.9000000000000, 92.8000000000000, 97.8000000000000, 100, 97.5000000000000, 89.4000000000000, 75.6000000000000, 57.1000000000000, 36.5000000000000, 18.6000000000000, 7, 2, 0.800000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.700000000000000, 0.600000000000000, 0.700000000000000, 0, 0, 0, 0, 0, 0 };
                                    f_SSD_ext = new double[2] { 9.3026e-6, 0.0106 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9850;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;

                        case "9E":
                            DoseRef = 0.9736;
                            prof_PDD = new double[40] { 0, 0.50000, 2.7000, 5.3000, 7.8000, 10.300, 12.800, 15.300, 17.800, 20.300, 22.800, 25.300, 27.800, 30.300, 32.800, 35.300, 37.900, 40.400, 42.900, 45.400, 47.900, 50.400, 52.900, 55.400, 57.900, 60.400, 62.900, 65.400, 67.900, 70.400, 73, 75.500, 78, 80.500, 83, 85.5, 88, 90.5, 93, 95.5 };
                            f_Kernel_SSD100 = new double[69] { 0.00225178975444717, 0.00225178975444717, 0.000829517095524811, 0.000619769915474458, 0.000466917085194688, 0.000354221970702424, 0.000270326515870260, 0.000207359176324585, 0.000159768946291791, 0.000123583721105949, 9.59258197935751e-05, 7.46882457536363e-05, 5.83141164512796e-05, 4.56438465002892e-05, 3.58077213743159e-05, 2.81494144828166e-05, 2.21709241195075e-05, 1.74925395327019e-05, 1.38234790999708e-05, 1.09401878585956e-05, 8.67018466987746e-06, 6.87996480697861e-06, 5.46588884197634e-06, 4.34728579094291e-06, 3.46120834922543e-06, 2.75842777135345e-06, 2.20036371621232e-06, 1.75672257792582e-06, 1.40367499041190e-06, 1.12244527016025e-06, 8.98216724315763e-07, 7.19279962781143e-07, 5.76368733375585e-07, 4.62140876200599e-07, 3.70771876707586e-07, 2.97635997366969e-07, 2.39055681636831e-07, 1.92104292094429e-07, 1.54450595020823e-07, 1.24235981379567e-07, 9.99774029081516e-08, 8.04905405139845e-08, 6.48289152680794e-08, 5.22355796842584e-08, 4.21047493728180e-08, 3.39512990572530e-08, 2.73864879192454e-08, 2.20986247375610e-08, 1.78376543884755e-08, 1.44028604182379e-08, 1.16330461073871e-08, 9.39868873241630e-09, 7.59566607034932e-09, 6.14023671780563e-09, 4.96502106414810e-09, 4.01578146439027e-09, 3.24884118329620e-09, 2.62901421652404e-09, 2.12794398251411e-09, 1.72276944073262e-09, 1.39505357019181e-09, 1.12992220209464e-09, 9.15371588459479e-10, 7.41711401988408e-10, 6.01116491485069e-10, 4.87266007027173e-10, 3.95052751138779e-10, 0, 0 };
                            f_Kernel_SSD110 = new double[69] { 0.00163941369487061, 0.00163941369487061, 0.000998274994965703, 0.000790146814600699, 0.000630622582324748, 0.000506824613874826, 0.000409754093249840, 0.000332974030537954, 0.000271789031070381, 0.000222716951022244, 0.000183138635731166, 0.000151059890183431, 0.000124946200964957, 0.000103605793809057, 8.61055097878887e-05, 7.17093973993006e-05, 5.98332995712891e-05, 5.00108771777498e-05, 4.18679231735618e-05, 3.51027618967205e-05, 2.94711652027174e-05, 2.47746556150008e-05, 2.08513729039989e-05, 1.75688972117765e-05, 1.48185770740067e-05, 1.25110231479850e-05, 1.05725107721389e-05, 8.94209531698876e-06, 7.56928964683337e-06, 6.41218711148420e-06, 5.43593931966750e-06, 4.61151765424484e-06, 3.91470262562474e-06, 3.32525685625196e-06, 2.82624657964065e-06, 2.40348364060491e-06, 2.04506556165105e-06, 1.74099564172858e-06, 1.48286854150404e-06, 1.26360958480525e-06, 1.07725822308334e-06, 9.18787887267512e-07, 7.83955881409243e-07, 6.69178126549985e-07, 5.71424497476307e-07, 4.88131253357383e-07, 4.17127680560353e-07, 3.56574569650391e-07, 3.04912560573700e-07, 2.60818727823470e-07, 2.23170054873779e-07, 1.91012675600878e-07, 1.63535948845329e-07, 1.40050587928217e-07, 1.19970195807605e-07, 1.02795663386083e-07, 8.81019771724094e-08, 7.55270562790520e-08, 6.47623001400180e-08, 5.55445795562735e-08, 4.76494464317386e-08, 4.08853733007276e-08, 3.50888636596374e-08, 3.01202991655758e-08, 2.58604107862935e-08, 2.22072786206012e-08, 1.90737799363574e-08, 0, 0 };
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 0.9920;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                                                       
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                                case ("A10"):
                                    F_App = 1.003;
                                    PDD = new double[40] { 78.7000000000000, 79.3000000000000, 81.9000000000000, 84.9000000000000, 87.7000000000000, 90.4000000000000, 93.4000000000000, 96.2000000000000, 98.6000000000000, 100, 99.6000000000000, 96.6000000000000, 90.5000000000000, 81.3000000000000, 69, 54.6000000000000, 39.3000000000000, 24.9000000000000, 13.5000000000000, 6.20000000000000, 2.70000000000000, 1.40000000000000, 1.20000000000000, 1.10000000000000, 1.20000000000000, 1.10000000000000, 1.10000000000000, 1.10000000000000, 1.10000000000000, 1.10000000000000, 1, 1, 1, 1.10000000000000, 1, 1, 1, 0.900000000000000, 1, 1 };
                                    f_SSD_ext = new double[2] { 4.9559e-6, 0.0112 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[40] { 79.1000000000000, 79.6000000000000, 82.2000000000000, 85.2000000000000, 88, 90.7000000000000, 93.6000000000000, 96.4000000000000, 98.7000000000000, 100, 99.4000000000000, 96.4000000000000, 90.2000000000000, 80.8000000000000, 68.4000000000000, 53.7000000000000, 38.5000000000000, 24.2000000000000, 12.8000000000000, 5.90000000000000, 2.50000000000000, 1.30000000000000, 1.10000000000000, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.900000000000000, 0.900000000000000, 0.900000000000000, 0.900000000000000 };
                                    f_SSD_ext = new double[2] { 7.8970e-6, 0.0108 };
                                    break;
                                case ("A20"):
                                    F_App = 0.9870;
                                    PDD = new double[40] { 79.9000000000000, 80.5000000000000, 83.2000000000000, 85.9000000000000, 88.5000000000000, 91.3000000000000, 94.1000000000000, 96.8000000000000, 98.9000000000000, 100, 98.9000000000000, 95.3000000000000, 88.6000000000000, 78.6000000000000, 65.6000000000000, 51, 35.6000000000000, 21.5000000000000, 11.1000000000000, 4.80000000000000, 1.90000000000000, 1.10000000000000, 0.900000000000000, 0.900000000000000, 0.900000000000000, 0.900000000000000, 0.900000000000000, 0.900000000000000, 0.800000000000000, 0.800000000000000, 0.900000000000000, 0.900000000000000, 0.800000000000000, 0.800000000000000, 0.800000000000000, 0.800000000000000, 0.800000000000000, 0.800000000000000, 0.800000000000000, 0.700000000000000 };
                                    f_SSD_ext = new double[2] { 6.3265e-6, 0.0107 };
                                    break;
                                case ("A25"):
                                    F_App = 0.9670;
                                    PDD = new double[40] { 80, 80.6000000000000, 83.1000000000000, 85.7000000000000, 88.4000000000000, 91.1000000000000, 93.9000000000000, 96.6000000000000, 98.8000000000000, 100, 99.4000000000000, 96.2000000000000, 90.1000000000000, 80.6000000000000, 68.2000000000000, 53.6000000000000, 38.1000000000000, 23.9000000000000, 12.8000000000000, 5.90000000000000, 2.50000000000000, 1.40000000000000, 1.30000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.20000000000000, 1.10000000000000, 1.10000000000000, 1.10000000000000, 1.10000000000000, 1.10000000000000 };
                                    f_SSD_ext = new double[2] { 8.7113e-6, 0.0105 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9310;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;

                        case "12E":
                            DoseRef = 0.9380;
                            prof_PDD = new double[44] { 0, 0.500000000000000, 2.70000000000000, 5.20000000000000, 7.70000000000000, 10.3000000000000, 12.8000000000000, 15.3000000000000, 17.8000000000000, 20.3000000000000, 22.8000000000000, 25.3000000000000, 27.8000000000000, 30.3000000000000, 32.8000000000000, 35.3000000000000, 37.8000000000000, 40.3000000000000, 42.8000000000000, 45.3000000000000, 47.9000000000000, 50.4000000000000, 52.9000000000000, 55.4000000000000, 57.9000000000000, 60.4000000000000, 62.9000000000000, 65.4000000000000, 67.9000000000000, 70.4000000000000, 72.9000000000000, 75.4000000000000, 77.9000000000000, 80.5000000000000, 83, 85.5000000000000, 88, 90.5000000000000, 93, 95.5000000000000, 98, 100.400000000000, 103, 105.500000000000 };
                            f_Kernel_SSD100 = new double[69] { 0.00222360402836368, 0.00222360402836368, 0.000788844950892897, 0.000599214353049550, 0.000458962168883701, 0.000353995731649804, 0.000274660739285710, 0.000214198588117033, 0.000167791950611381, 0.000131954871063616, 0.000104132209993572, 8.24303667687488e-05, 6.54325853094737e-05, 5.20700534589816e-05, 4.15305688772307e-05, 3.31929588393665e-05, 2.65794386334373e-05, 2.13206420971909e-05, 1.71297168057718e-05, 1.37829763037211e-05, 1.11053432083385e-05, 8.95932437997610e-06, 7.23661131030515e-06, 5.85164606477836e-06, 4.73666868363659e-06, 3.83788793328790e-06, 3.11250856028452e-06, 2.52641487549620e-06, 2.05235959436301e-06, 1.66854327610037e-06, 1.35749689595947e-06, 1.10520051180061e-06, 9.00386418859031e-07, 7.33986906715548e-07, 5.98695676203837e-07, 4.88618829547958e-07, 3.98996623692262e-07, 3.25981253918274e-07, 2.66459096292165e-07, 2.17908297166461e-07, 1.78284517517501e-07, 1.45929142319733e-07, 1.19495444263251e-07, 9.78891188048766e-08, 8.02203392080012e-08, 6.57650584801482e-08, 5.39337432014478e-08, 4.42460877050643e-08, 3.63105461243621e-08, 2.98077499798566e-08, 2.44770626521230e-08, 2.01056687906448e-08, 1.65197141339585e-08, 1.35771052826940e-08, 1.11616544649617e-08, 9.17831495304994e-09, 7.54930157110820e-09, 6.21092999640431e-09, 5.11104021352892e-09, 4.20689500782972e-09, 3.46346502167894e-09, 2.85202856174740e-09, 2.34902783905983e-09, 1.93513424118380e-09, 1.59448409843624e-09, 1.31405357269167e-09, 1.08314713255878e-09, 0, 0 };
                            f_Kernel_SSD110 = new double[69] { 0.00185023831888518, 0.00185023831888518, 0.000914930786446115, 0.000717564995181275, 0.000567464074171975, 0.000451899596726842, 0.000362011993190064, 0.000291491183461236, 0.000235755801179603, 0.000191425212386084, 0.000155970061040893, 0.000127475238886212, 0.000104475661116816, 8.58403574024943e-05, 7.06893224998971e-05, 5.83330053829415e-05, 4.82277093464837e-05, 3.99423447801488e-05, 3.31333924912845e-05, 2.75258777757944e-05, 2.28987933681779e-05, 1.90738481072060e-05, 1.59067242061652e-05, 1.32802423458936e-05, 1.10989885691790e-05, 9.28506890024084e-06, 7.77473941689783e-06, 6.51571984504333e-06, 5.46504364398825e-06, 4.58733122846024e-06, 3.85339842198938e-06, 3.23913160042064e-06, 2.72457581315628e-06, 2.29319359332763e-06, 1.93126101743374e-06, 1.62737446555327e-06, 1.37204692466302e-06, 1.15737691487634e-06, 9.76776462840647e-07, 8.24747196476191e-07, 6.96695742755454e-07, 5.88781292108775e-07, 4.97789539596724e-07, 4.21028294355312e-07, 3.56240919747581e-07, 3.01534469890368e-07, 2.55319957615741e-07, 2.16262650923699e-07, 1.83240670793371e-07, 1.55310469476842e-07, 1.31678018565975e-07, 1.11674740762610e-07, 9.47373870912685e-08, 8.03911990105567e-08, 6.82358082043686e-08, 5.79334201350383e-08, 4.91989044152296e-08, 4.17914786412905e-08, 3.55077249180863e-08, 3.01757218417176e-08, 2.56501108312340e-08, 2.18079456527911e-08, 1.85451988820113e-08, 1.57738197828327e-08, 1.34192552939423e-08, 1.14183602190810e-08, 9.71763467921282e-09, 0, 0 };
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 1.003;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                                                       
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                                case ("A10"):
                                    F_App = 1.004;
                                    PDD = new double[44] { 84, 84.5000000000000, 86.6000000000000, 88.8000000000000, 90.4000000000000, 91.8000000000000, 93.1000000000000, 94.5000000000000, 95.8000000000000, 97, 98.3000000000000, 99.3000000000000, 100, 100, 99, 96.8000000000000, 93.1000000000000, 87.7000000000000, 80.3000000000000, 71.3000000000000, 61, 49.6000000000000, 37.9000000000000, 26.7000000000000, 17, 10, 5.60000000000000, 3.30000000000000, 2.40000000000000, 2.10000000000000, 2, 2, 2, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.80000000000000, 1.80000000000000, 1.80000000000000 };
                                    f_SSD_ext = new double[2] { 3.7074e-5, 0.0111 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[44] { 84.3000000000000, 84.8000000000000, 86.8000000000000, 89.1000000000000, 90.6000000000000, 92, 93.3000000000000, 94.6000000000000, 96, 97.2000000000000, 98.4000000000000, 99.4000000000000, 100, 100, 99, 96.7000000000000, 93, 87.5000000000000, 80.2000000000000, 71.3000000000000, 60.8000000000000, 49.3000000000000, 37.5000000000000, 26.3000000000000, 16.9000000000000, 9.90000000000000, 5.40000000000000, 3.20000000000000, 2.30000000000000, 2.10000000000000, 2, 2, 2, 2, 2, 2, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.80000000000000, 1.80000000000000 };
                                    f_SSD_ext = new double[2] { 1.0151e-5, 0.0106 };
                                    break;
                                case ("A20"):
                                    F_App = 0.99;
                                    PDD = new double[44] { 86.1000000000000, 86.6000000000000, 88.8000000000000, 90.6000000000000, 92, 93.2000000000000, 94.3000000000000, 95.5000000000000, 96.6000000000000, 97.8000000000000, 98.8000000000000, 99.6000000000000, 100, 99.7000000000000, 98.5000000000000, 95.8000000000000, 91.9000000000000, 85.9000000000000, 78.5000000000000, 69.1000000000000, 58.5000000000000, 46.8000000000000, 35.1000000000000, 24.3000000000000, 15.3000000000000, 8.70000000000000, 4.80000000000000, 2.90000000000000, 2.20000000000000, 2, 2, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.90000000000000, 1.80000000000000, 1.80000000000000, 1.80000000000000 };
                                    f_SSD_ext = new double[2] { 9.2470e-6, 0.0105 };
                                    break;
                                case ("A25"):
                                    F_App = 0.9630;
                                    PDD = new double[44] { 85.8000000000000, 86.2000000000000, 88.1000000000000, 90.1000000000000, 91.6000000000000, 92.8000000000000, 94, 95.15, 96.3000000000000, 97.4000000000000, 98.6000000000000, 99.5000000000000, 100, 99.8000000000000, 98.8000000000000, 96.6000000000000, 92.8000000000000, 87.3000000000000, 80, 70.9000000000000, 60.7000000000000, 49.2000000000000, 37.5000000000000, 26.4000000000000, 17.1000000000000, 10, 5.70000000000000, 3.40000000000000, 2.60000000000000, 2.40000000000000, 2.30000000000000, 2.30000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.20000000000000, 2.10000000000000, 2.10000000000000 };
                                    f_SSD_ext = new double[2] { 1.716e-5, 0.0101 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9230;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;

                        case "15E":
                            DoseRef = 0.9034;
                            prof_PDD = new double[52] { 0, 0.500000000000000, 2.70000000000000, 5.20000000000000, 7.70000000000000, 10.2000000000000, 12.8000000000000, 15.3000000000000, 17.8000000000000, 20.3000000000000, 22.8000000000000, 25.3000000000000, 27.8000000000000, 30.3000000000000, 32.8000000000000, 35.3000000000000, 37.8000000000000, 40.3000000000000, 42.8000000000000, 45.3000000000000, 47.9000000000000, 50.4000000000000, 52.9000000000000, 55.4000000000000, 57.9000000000000, 60.4000000000000, 62.9000000000000, 65.4000000000000, 67.9000000000000, 70.4000000000000, 72.9000000000000, 75.4000000000000, 77.9000000000000, 80.5000000000000, 83, 85.5000000000000, 88, 90.5000000000000, 93, 95.5000000000000, 98, 100.500000000000, 103, 105.500000000000, 108, 110.500000000000, 113, 115.500000000000, 118, 120.500000000000, 123, 125.500000000000 };
                            f_Kernel_SSD100 = new double[69] { 0.00185295678080615, 0.00185295678080615, 0.000939733750388916, 0.000733591831579840, 0.000577441872868168, 0.000457708011677431, 0.000364960765004708, 0.000292499617296510, 0.000235471812052642, 0.000190305935784240, 0.000154337370959171, 0.000125554519257056, 0.000102423190976801, 8.37628316148161e-05, 6.86578677088662e-05, 5.63932992758963e-05, 4.64073153386135e-05, 3.82560411187997e-05, 3.15870475054681e-05, 2.61192657826031e-05, 2.16276356584433e-05, 1.79312855056080e-05, 1.48843720121489e-05, 1.23689383302604e-05, 1.02893155678990e-05, 8.56771226251063e-06, 7.14072392828555e-06, 5.95655920612128e-06, 4.97282703392080e-06, 4.15476512740476e-06, 3.47381713071354e-06, 2.90648636148638e-06, 2.43340979805054e-06, 2.03860804761247e-06, 1.70887638353279e-06, 1.43328920533017e-06, 1.20279594816400e-06, 1.00989091766441e-06, 8.48343029633699e-07, 7.12974203800043e-07, 5.99477358148445e-07, 5.04266699487407e-07, 4.24354402710274e-07, 3.57248889863365e-07, 3.00870818617904e-07, 2.53483613200171e-07, 2.13635954840216e-07, 1.80114121308189e-07, 1.51902448231071e-07, 1.28150496204463e-07, 1.08145761128181e-07, 9.12909719171271e-08, 7.70851886018114e-08, 6.51080520478873e-08, 5.50066497884939e-08, 4.64845554669609e-08, 3.92926757984655e-08, 3.32216018886379e-08, 2.80952135799681e-08, 2.37653282984238e-08, 2.01072211883130e-08, 1.70158725752865e-08, 1.44028229703818e-08, 1.21935359005294e-08, 1.03251854528760e-08, 8.74479925843698e-09, 7.40769908512370e-09, 0, 0 };
                            f_Kernel_SSD110 = new double[69] { 0.00169364423294031, 0.00169364423294031, 0.000942536828457184, 0.000748407372288824, 0.000599213941710416, 0.000483116885098822, 0.000391832041103770, 0.000319425171365256, 0.000261560901905212, 0.000215018734188499, 0.000177372027542029, 0.000146769714011164, 0.000121784625967699, 0.000101306096351279, 8.44626315194118e-05, 7.05654095335101e-05, 5.90664505779659e-05, 4.95272848910042e-05, 4.15952361682871e-05, 3.49852991366000e-05, 2.94661732362484e-05, 2.48494158864769e-05, 2.09809592924087e-05, 1.77344333058594e-05, 1.50058791404791e-05, 1.27095418659786e-05, 1.07745051295515e-05, 9.14198739816965e-06, 7.76316068107618e-06, 6.59738403671162e-06, 5.61076793148796e-06, 4.77500366270368e-06, 4.06640600567067e-06, 3.46512803363926e-06, 2.95451545256562e-06, 2.52057435852619e-06, 2.15153148770531e-06, 1.83747010689330e-06, 1.57002792831931e-06, 1.34214601112022e-06, 1.14785967443134e-06, 9.82124103258949e-07, 8.40668662667190e-07, 7.19875014456099e-07, 6.16675005099228e-07, 5.28465004871083e-07, 4.53033958025166e-07, 3.88502877928842e-07, 3.33273909565551e-07, 2.85987400903292e-07, 2.45485687315640e-07, 2.10782509893743e-07, 1.81037167557597e-07, 1.55532651162763e-07, 1.33657130750568e-07, 1.14888269299320e-07, 9.87799213449299e-08, 8.49508457015780e-08, 7.30751206666666e-08, 6.28739995113984e-08, 5.41089854228912e-08, 4.65759397227937e-08, 4.01000662495529e-08, 3.45316392150663e-08, 2.97423623571596e-08, 2.56222644978406e-08, 2.20770511488812e-08, 0, 0 };

                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 1.006;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                                                       
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                                case ("A10"):
                                    F_App = 1.009;
                                    PDD = new double[52] { 88.7000000000000, 89.1000000000000, 91, 92.9000000000000, 94.2000000000000, 95.2000000000000, 96, 96.6000000000000, 97.3000000000000, 97.9000000000000, 98.5000000000000, 98.9000000000000, 99.3000000000000, 99.8000000000000, 100, 100, 99.6000000000000, 98.9000000000000, 97.5000000000000, 95.5000000000000, 92.4000000000000, 88.5000000000000, 83.3000000000000, 76.9000000000000, 69.3000000000000, 60.9000000000000, 51.6000000000000, 42.1000000000000, 33.1000000000000, 24.5000000000000, 17.3000000000000, 11.6000000000000, 7.80000000000000, 5.20000000000000, 4, 3.40000000000000, 3.20000000000000, 3.10000000000000, 3.10000000000000, 3.10000000000000, 3.10000000000000, 3.10000000000000, 3, 3, 3, 3, 3, 2.90000000000000, 2.90000000000000, 2.90000000000000, 2.80000000000000, 2.80000000000000 };
                                    f_SSD_ext = new double[2] { 4.1744e-6, 0.0109 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[52] { 88.7000000000000, 89.1000000000000, 91, 92.9000000000000, 94.2000000000000, 95.1000000000000, 96, 96.7000000000000, 97.3000000000000, 97.9000000000000, 98.5000000000000, 98.9000000000000, 99.4000000000000, 99.8000000000000, 99.9000000000000, 100, 99.6000000000000, 98.8000000000000, 97.4000000000000, 95.3000000000000, 92.3000000000000, 88.3000000000000, 83.2000000000000, 76.8000000000000, 69.3000000000000, 60.8000000000000, 51.6000000000000, 42.1000000000000, 32.9000000000000, 24.4000000000000, 17.1000000000000, 11.5000000000000, 7.60000000000000, 5.20000000000000, 3.90000000000000, 3.40000000000000, 3.20000000000000, 3.20000000000000, 3.20000000000000, 3.10000000000000, 3.10000000000000, 3.10000000000000, 3.10000000000000, 3, 3, 3, 3, 3, 2.90000000000000, 2.90000000000000, 2.80000000000000, 2.80000000000000 };
                                    f_SSD_ext = new double[2] { 1.1973e-5, 0.0104 };
                                    break;
                                case ("A20"):
                                    F_App = 0.9860;
                                    PDD = new double[52] { 89.7000000000000, 90.1000000000000, 92.1000000000000, 93.8000000000000, 95, 95.8000000000000, 96.6000000000000, 97.2000000000000, 97.8000000000000, 98.3000000000000, 98.8000000000000, 99.2000000000000, 99.6000000000000, 99.8000000000000, 100, 99.9000000000000, 99.4000000000000, 98.5000000000000, 97, 94.7000000000000, 91.6000000000000, 87.3000000000000, 81.9000000000000, 75.4000000000000, 67.8000000000000, 59.1000000000000, 49.9000000000000, 40.4000000000000, 31.4000000000000, 22.8000000000000, 15.9000000000000, 10.5000000000000, 7, 4.80000000000000, 3.80000000000000, 3.30000000000000, 3.10000000000000, 3.10000000000000, 3, 3, 3, 3, 3, 2.90000000000000, 3, 2.90000000000000, 2.90000000000000, 2.90000000000000, 2.80000000000000, 2.80000000000000, 2.80000000000000, 2.70000000000000 };
                                    f_SSD_ext = new double[2] { 1.2750e-5, 0.0102 };
                                    break;
                                case ("A25"):
                                    F_App = 0.9560;
                                    PDD = new double[52] { 89.6000000000000, 90, 91.8000000000000, 93.6000000000000, 94.7000000000000, 95.7000000000000, 96.4000000000000, 97.1000000000000, 97.6000000000000, 98.1000000000000, 98.6000000000000, 99.1000000000000, 99.5000000000000, 99.8000000000000, 100, 99.9000000000000, 99.6000000000000, 98.8000000000000, 97.3000000000000, 95.2000000000000, 92.3000000000000, 88.4000000000000, 83.2000000000000, 76.9000000000000, 69.4000000000000, 61, 51.8000000000000, 42.3000000000000, 33, 24.5000000000000, 17.2000000000000, 11.6000000000000, 7.70000000000000, 5.40000000000000, 4.20000000000000, 3.70000000000000, 3.50000000000000, 3.40000000000000, 3.40000000000000, 3.40000000000000, 3.40000000000000, 3.40000000000000, 3.40000000000000, 3.40000000000000, 3.30000000000000, 3.30000000000000, 3.30000000000000, 3.30000000000000, 3.30000000000000, 3.20000000000000, 3.20000000000000, 3.10000000000000 };
                                    f_SSD_ext = new double[2] { 1.7798e-5, 0.0099 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9110;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;

                        case "18E":
                            DoseRef = 0.8874;
                            prof_PDD = new double[58] { 0, 0.500000000000000, 2.70000000000000, 5.20000000000000, 7.70000000000000, 10.2000000000000, 12.8000000000000, 15.3000000000000, 17.8000000000000, 20.3000000000000, 22.8000000000000, 25.3000000000000, 27.8000000000000, 30.3000000000000, 32.8000000000000, 35.3000000000000, 37.8000000000000, 40.3000000000000, 42.8000000000000, 45.4000000000000, 47.9000000000000, 50.4000000000000, 52.9000000000000, 55.4000000000000, 57.9000000000000, 60.4000000000000, 62.9000000000000, 65.4000000000000, 67.9000000000000, 70.4000000000000, 72.9000000000000, 75.4000000000000, 77.9000000000000, 80.5000000000000, 82.9000000000000, 85.5000000000000, 87.9000000000000, 90.5000000000000, 93, 95.5000000000000, 98, 100.500000000000, 103, 105.500000000000, 108, 110.500000000000, 113, 115.500000000000, 118, 120.500000000000, 123, 125.500000000000, 128, 130.500000000000, 133, 135.500000000000, 138, 140.500000000000 };
                            f_Kernel_SSD100 = new double[69] { 0.00180463520422412, 0.00180463520422412, 0.000964257937712455, 0.000753959171930530, 0.000594437992983972, 0.000471945385662675, 0.000376924486772534, 0.000302578746987123, 0.000243981550987151, 0.000197503747444074, 0.000160434973890060, 0.000130726984509548, 0.000106815958019159, 8.74971953320198e-05, 7.18353211071698e-05, 5.90990056028362e-05, 4.87129083180473e-05, 4.02219001430355e-05, 3.32641580491735e-05, 2.75507490267008e-05, 2.28500121167198e-05, 1.89755232081347e-05, 1.57767590894731e-05, 1.31318116834584e-05, 1.09416710222625e-05, 9.12571670666123e-06, 7.61814609198261e-06, 6.36513267886092e-06, 5.32255668897302e-06, 4.45418615514233e-06, 3.73021429809968e-06, 3.12607982289219e-06, 2.62151272600994e-06, 2.19976048406194e-06, 1.84695899596938e-06, 1.55162003948900e-06, 1.30421277642437e-06, 1.09682137226848e-06, 9.22864367279939e-07, 7.76864261469819e-07, 6.54258019510353e-07, 5.51240989112898e-07, 4.64638155143289e-07, 3.91797797120854e-07, 3.30503538530227e-07, 2.78901518580747e-07, 2.35440016769627e-07, 1.98819346350827e-07, 1.67950227106742e-07, 1.41919168532280e-07, 1.19959655942807e-07, 1.01428145438376e-07, 8.57840482523707e-08, 7.25730280330717e-08, 6.14130520019550e-08, 5.19827333870303e-08, 4.40115819261795e-08, 3.72718446683222e-08, 3.15716732841818e-08, 2.67493987056981e-08, 2.26687308136510e-08, 1.92147314314313e-08, 1.62904342234346e-08, 1.38140060932997e-08, 1.17163621408537e-08, 9.93916074456365e-09, 8.43311739613324e-09, 0, 0 };
                            f_Kernel_SSD110 = new double[69] { 0.00171338114565850, 0.00171338114565850, 0.000910439771617752, 0.000725129565795247, 0.000582350066175408, 0.000470954661526388, 0.000383134705581067, 0.000313289144795274, 0.000257320093552604, 0.000212178726733790, 0.000175563945460310, 0.000145717366364004, 0.000121280783989886, 0.000101195167510865, 8.46278805842476e-05, 7.09194544940053e-05, 5.95441441975298e-05, 5.00803514267972e-05, 4.21882106178643e-05, 3.55924403869391e-05, 3.00691101606775e-05, 2.54353481581492e-05, 2.15412800476485e-05, 1.82636739228425e-05, 1.55009008106390e-05, 1.31689167656121e-05, 1.11980435602687e-05, 9.53037747388862e-06, 8.11769486923094e-06, 6.91975274449765e-06, 5.90290482628881e-06, 5.03897086928017e-06, 4.30430998365030e-06, 3.67905899552106e-06, 3.14650477646425e-06, 2.69256568809085e-06, 2.30536217532763e-06, 1.97486040739116e-06, 1.69257593634940e-06, 1.45132679314261e-06, 1.24502740341258e-06, 1.06851628332872e-06, 9.17411748698167e-07, 7.87990901243041e-07, 6.77087992848543e-07, 5.82008950224381e-07, 5.00459399126067e-07, 4.30483983177612e-07, 3.70415146551601e-07, 3.18829857714034e-07, 2.74513005371762e-07, 2.36426407634734e-07, 2.03682549159827e-07, 1.75522305226295e-07, 1.51296031500395e-07, 1.30447497996168e-07, 1.12500228919364e-07, 9.70458795555631e-08, 8.37343394435824e-08, 7.22652997618513e-08, 6.23810636417944e-08, 5.38604124119270e-08, 4.65133695679197e-08, 4.01767285297718e-08, 3.47102306732145e-08, 2.99932973575104e-08, 2.59222342342934e-08, 0, 0 };
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 1.019;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                                                       
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                                case ("A10"):
                                    F_App = 1.001;
                                    PDD = new double[58] { 92, 92.4000000000000, 94.2000000000000, 96.2000000000000, 97.3000000000000, 98, 98.6000000000000, 99.1000000000000, 99.4000000000000, 99.6000000000000, 99.7000000000000, 100, 100, 100, 99.9000000000000, 99.9000000000000, 99.7000000000000, 99.3000000000000, 98.9000000000000, 98.2000000000000, 97.4000000000000, 96.1000000000000, 94.4000000000000, 92.3000000000000, 89.6000000000000, 86.2000000000000, 82.2000000000000, 77.4000000000000, 72, 65.7000000000000, 59, 51.8000000000000, 44.4000000000000, 36.9000000000000, 29.7000000000000, 23.1000000000000, 17.4000000000000, 12.9000000000000, 9.50000000000000, 7.20000000000000, 5.90000000000000, 5.20000000000000, 4.90000000000000, 4.80000000000000, 4.70000000000000, 4.70000000000000, 4.60000000000000, 4.60000000000000, 4.60000000000000, 4.50000000000000, 4.50000000000000, 4.50000000000000, 4.40000000000000, 4.40000000000000, 4.30000000000000, 4.20000000000000, 4.20000000000000, 4.20000000000000 };
                                    f_SSD_ext = new double[2] { 9.4859e-6, 0.0106 };
                                    break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[58] { 91.3000000000000, 91.7000000000000, 93.5000000000000, 95.4000000000000, 96.6000000000000, 97.5000000000000, 98.2000000000000, 98.7000000000000, 99, 99.4000000000000, 99.6000000000000, 99.8000000000000, 99.9000000000000, 100, 100, 100, 99.8000000000000, 99.6000000000000, 99.1000000000000, 98.5000000000000, 97.6000000000000, 96.4000000000000, 94.8000000000000, 92.7000000000000, 90, 86.7000000000000, 82.6000000000000, 77.8000000000000, 72.3000000000000, 66.1000000000000, 59.3000000000000, 52.2000000000000, 44.6000000000000, 37.1000000000000, 29.8000000000000, 23.2000000000000, 17.4000000000000, 12.8000000000000, 9.60000000000000, 7.30000000000000, 5.90000000000000, 5.30000000000000, 5, 4.80000000000000, 4.80000000000000, 4.80000000000000, 4.70000000000000, 4.70000000000000, 4.70000000000000, 4.70000000000000, 4.60000000000000, 4.60000000000000, 4.60000000000000, 4.50000000000000, 4.50000000000000, 4.40000000000000, 4.40000000000000, 4.40000000000000 };
                                    f_SSD_ext = new double[2] { 1.5499e-5, 0.0102 };
                                    break;
                                case ("A20"):
                                    F_App = 1.01;
                                    PDD = new double[58] { 91.9000000000000, 92.4000000000000, 94.4000000000000, 96, 97.1000000000000, 97.9000000000000, 98.4000000000000, 98.8000000000000, 99.2000000000000, 99.5000000000000, 99.7000000000000, 99.8000000000000, 100, 100, 100, 99.8000000000000, 99.6000000000000, 99.3000000000000, 98.9000000000000, 98.1000000000000, 97.2000000000000, 96, 94.3000000000000, 92.1000000000000, 89.4000000000000, 85.8000000000000, 81.7000000000000, 76.8000000000000, 71.2000000000000, 64.9000000000000, 57.8000000000000, 50.7000000000000, 43.1000000000000, 35.8000000000000, 28.3000000000000, 22, 16.4000000000000, 12.1000000000000, 9, 6.90000000000000, 5.70000000000000, 5.10000000000000, 4.80000000000000, 4.70000000000000, 4.60000000000000, 4.60000000000000, 4.60000000000000, 4.60000000000000, 4.50000000000000, 4.50000000000000, 4.50000000000000, 4.50000000000000, 4.50000000000000, 4.40000000000000, 4.30000000000000, 4.30000000000000, 4.20000000000000, 4.20000000000000 };
                                    f_SSD_ext = new double[2] { 1.1298e-5, 0.0101 };
                                    break;
                                case ("A25"):
                                    F_App = 1.006;
                                    PDD = new double[58] { 91.7000000000000, 92.1000000000000, 93.9000000000000, 95.8000000000000, 96.8000000000000, 97.7000000000000, 98.3000000000000, 98.8000000000000, 99.2000000000000, 99.5000000000000, 99.6000000000000, 99.9000000000000, 99.9000000000000, 100, 100, 99.9000000000000, 99.7000000000000, 99.5000000000000, 99, 98.4000000000000, 97.5000000000000, 96.3000000000000, 94.8000000000000, 92.6000000000000, 90, 86.7000000000000, 82.6000000000000, 77.9000000000000, 72.3000000000000, 66.2000000000000, 59.4000000000000, 52.1000000000000, 44.7000000000000, 37.2000000000000, 29.9000000000000, 23.3000000000000, 17.6000000000000, 13, 9.80000000000000, 7.60000000000000, 6.20000000000000, 5.60000000000000, 5.30000000000000, 5.10000000000000, 5.10000000000000, 5.10000000000000, 5, 5, 5, 5, 4.90000000000000, 4.90000000000000, 4.90000000000000, 4.90000000000000, 4.80000000000000, 4.70000000000000, 4.70000000000000, 4.70000000000000 };
                                    f_SSD_ext = new double[2] { 1.9338e-5, 0.0098 };
                                    break;
                                case ("A40"):
                                    F_App = 0.9850;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                            }
                            break;
                        case "22E":
                            DoseRef = 0.8822;
                            prof_PDD = new double[68] { 0, 0.500000000000000, 2.70000000000000, 5.20000000000000, 7.70000000000000, 10.2000000000000, 12.8000000000000, 15.3000000000000, 17.8000000000000, 20.3000000000000, 22.8000000000000, 25.3000000000000, 27.8000000000000, 30.3000000000000, 32.8000000000000, 35.3000000000000, 37.8000000000000, 40.3000000000000, 42.8000000000000, 45.3000000000000, 47.8000000000000, 50.4000000000000, 52.9000000000000, 55.4000000000000, 57.9000000000000, 60.4000000000000, 62.9000000000000, 65.4000000000000, 67.9000000000000, 70.4000000000000, 72.9000000000000, 75.4000000000000, 77.9000000000000, 80.5000000000000, 83, 85.5000000000000, 88, 90.5000000000000, 93, 95.5000000000000, 98, 100.500000000000, 103, 105.400000000000, 108, 110.500000000000, 113, 115.500000000000, 118, 120.500000000000, 123, 125.500000000000, 128, 130.500000000000, 133, 135.500000000000, 138, 140.500000000000, 143, 145.500000000000, 148, 150.500000000000, 153, 155.500000000000, 158, 160.500000000000, 163, 165.500000000000 };
                            f_Kernel_SSD100 = new double[69] { 0.00177659144753296, 0.00177659144753296, 0.000880524391184674, 0.000700251257421270, 0.000561526851119755, 0.000453433511205105, 0.000368327480327703, 0.000300729540328209, 0.000246633779704356, 0.000203062071218176, 0.000167768499266907, 0.000139038321286057, 0.000115548234095344, 9.62673917529138e-05, 8.03861096165608e-05, 6.72637456093760e-05, 5.63900959525111e-05, 4.73564630437697e-05, 3.98337424288123e-05, 3.35556678110864e-05, 2.83058897614608e-05, 2.39079333338556e-05, 2.02173379654512e-05, 1.71154658275340e-05, 1.45045958060298e-05, 1.23040152627158e-05, 1.04468912971760e-05, 8.87775468555391e-06, 7.55046807394848e-06, 6.42657890300685e-06, 5.47397945665618e-06, 4.66581316899955e-06, 3.97957919747160e-06, 3.39639723276378e-06, 2.90040227055618e-06, 2.47824513907988e-06, 2.11867935076944e-06, 1.81221861992978e-06, 1.55085238463552e-06, 1.32780906010742e-06, 1.13735866324288e-06, 9.74647984586825e-07, 8.35562722966529e-07, 7.16612000263558e-07, 6.14831487093781e-07, 5.27702032019362e-07, 4.53081227086246e-07, 3.89145784409995e-07, 3.34342961029545e-07, 2.87349567272612e-07, 2.47037339400767e-07, 2.12443660054352e-07, 1.82746777717933e-07, 1.57244815444102e-07, 1.35337974484178e-07, 1.16513434478115e-07, 1.00332531800367e-07, 8.64198643948071e-08, 7.44540271577954e-08, 6.41597285657149e-08, 5.53010783164404e-08, 4.76758685211267e-08, 4.11106985075887e-08, 3.54568164332050e-08, 3.05865703777746e-08, 2.63903780127479e-08, 2.27741377516141e-08, 0, 0 };
                            f_Kernel_SSD110 = new double[69] { 0.00166915182463522, 0.00166915182463522, 0.000861626927677313, 0.000694138214052399, 0.000563867117855684, 0.000461247514693682, 0.000379549769325491, 0.000313924263865230, 0.000260804774107675, 0.000217523395750383, 0.000182054646096728, 0.000152841062112651, 0.000128671696893928, 0.000108595819101024, 9.18605614310614e-05, 7.78651843759069e-05, 6.61270709983546e-05, 5.62561355921460e-05, 4.79353532996863e-05, 4.09058001775425e-05, 3.49550559352928e-05, 2.99081404165516e-05, 2.56203778375312e-05, 2.19717408094792e-05, 1.88623395622471e-05, 1.62088040980897e-05, 1.39413673955897e-05, 1.20015025310473e-05, 1.03400000469389e-05, 8.91539713494509e-06, 7.69268938835697e-06, 6.64227057495513e-06, 5.73905721639664e-06, 4.96176355979811e-06, 4.29229940028090e-06, 3.71526861247987e-06, 3.21755051321311e-06, 2.78794956233825e-06, 2.41690160841575e-06, 2.09622704847029e-06, 1.81892301104690e-06, 1.57898807701949e-06, 1.37127419189770e-06, 1.19136135027896e-06, 1.03545138969697e-06, 9.00277850701843e-07, 7.83029368840390e-07, 6.81284483333058e-07, 5.92956093325516e-07, 5.16244079156589e-07, 4.49594843876448e-07, 3.91666728046583e-07, 3.41300415741697e-07, 2.97493587398242e-07, 2.59379190405172e-07, 2.26206794996635e-07, 1.97326584162356e-07, 1.72175594607727e-07, 1.50265883331351e-07, 1.31174342985074e-07, 1.14533930208962e-07, 1.00026105917801e-07, 8.73743159266143e-08, 7.63383653312157e-08, 6.67095612566708e-08, 5.83065166748278e-08, 5.09715233486882e-08, 0, 0 };
                            switch (App)
                            {
                                case ("A06"):
                                    F_App = 1.03;
                                    //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                                                       
                                    f_SSD_ext = new double[2] { 0, 0 };
                                    break;
                                case ("A10"):
                                    F_App = 1.0210;
                                    PDD = new double[68] {92.6000000000000, 93, 94.8000000000000, 96.8000000000000, 98, 98.8000000000000, 99.3000000000000, 99.7000000000000, 99.9000000000000, 100, 100, 100, 99.9000000000000, 99.7000000000000, 99.6000000000000, 99.3000000000000, 99, 98.6000000000000, 98.2000000000000, 97.6000000000000, 97, 96.1000000000000, 95.2000000000000, 94.2000000000000, 93, 91.4000000000000, 89.7000000000000, 87.7000000000000, 85.3000000000000, 82.6000000000000, 79.5000000000000, 75.9000000000000, 72, 67.5000000000000, 62.6000000000000, 57.4000000000000, 51.9000000000000, 46.2000000000000, 40.4000000000000, 34.8000000000000, 29.3000000000000, 24.3000000000000, 19.9000000000000, 16.2000000000000, 13.2000000000000, 11, 9.40000000000000, 8.50000000000000, 7.90000000000000, 7.60000000000000, 7.40000000000000, 7.30000000000000, 7.30000000000000, 7.20000000000000, 7.20000000000000, 7.10000000000000, 7.10000000000000, 7.10000000000000, 7, 7, 6.80000000000000, 6.70000000000000, 6.70000000000000, 6.60000000000000, 6.50000000000000, 6.40000000000000, 6.30000000000000, 6.20000000000000};
                                    f_SSD_ext = new double[2] { 8.7675e-6, 0.0105 };
                                     break;
                                case ("A15"):
                                    F_App = 1;
                                    PDD = new double[68] {91.7000000000000, 92.1000000000000, 94.1000000000000, 96.2000000000000, 97.4000000000000, 98.2000000000000, 98.9000000000000, 99.3000000000000, 99.6000000000000, 99.8000000000000, 99.9000000000000, 100, 100, 99.9000000000000, 99.7000000000000, 99.5000000000000, 99.2000000000000, 98.9000000000000, 98.5000000000000, 98, 97.4000000000000, 96.7000000000000, 95.8000000000000, 94.8000000000000, 93.6000000000000, 92.1000000000000, 90.3000000000000, 88.4000000000000, 86, 83.3000000000000, 80.2000000000000, 76.7000000000000, 72.6000000000000, 68.2000000000000, 63.3000000000000, 58, 52.4000000000000, 46.6000000000000, 40.8000000000000, 35.1000000000000, 29.6000000000000, 24.6000000000000, 20.1000000000000, 16.4000000000000, 13.4000000000000, 11.2000000000000, 9.60000000000000, 8.60000000000000, 8, 7.70000000000000, 7.60000000000000, 7.50000000000000, 7.50000000000000, 7.40000000000000, 7.40000000000000, 7.30000000000000, 7.30000000000000, 7.20000000000000, 7.20000000000000, 7.10000000000000, 7.10000000000000, 7, 6.90000000000000, 6.90000000000000, 6.70000000000000, 6.60000000000000, 6.60000000000000, 6.50000000000000};
                                    f_SSD_ext = new double[2] { 1.5930e-5, 0.0101 };
                                    break;
                        case ("A20"):
                            F_App = 0.9850;
                            PDD = new double[68] {92, 92.5000000000000, 94.6000000000000, 96.4000000000000, 97.7000000000000, 98.4000000000000, 98.9000000000000, 99.3000000000000, 99.7000000000000, 99.8000000000000, 100, 100, 100, 99.9000000000000, 99.7000000000000, 99.5000000000000, 99.2000000000000, 98.8000000000000, 98.4000000000000, 97.9000000000000, 97.2000000000000, 96.5000000000000, 95.6000000000000, 94.6000000000000, 93.3000000000000, 91.8000000000000, 90, 88, 85.6000000000000, 82.8000000000000, 79.6000000000000, 76, 72, 67.5000000000000, 62.5000000000000, 57.2000000000000, 51.5000000000000, 45.7000000000000, 39.8000000000000, 34.1000000000000, 28.7000000000000, 23.8000000000000, 19.4000000000000, 15.8000000000000, 12.9000000000000, 10.8000000000000, 9.40000000000000, 8.50000000000000, 7.90000000000000, 7.70000000000000, 7.50000000000000, 7.40000000000000, 7.40000000000000, 7.40000000000000, 7.30000000000000, 7.30000000000000, 7.20000000000000, 7.20000000000000, 7.20000000000000, 7.10000000000000, 7, 6.90000000000000, 6.90000000000000, 6.80000000000000, 6.60000000000000, 6.60000000000000, 6.50000000000000, 6.40000000000000 };
                            f_SSD_ext = new double[2] { 1.6160e-5, 0.0098 };
                            break;
                        case ("A25"):
                            F_App = 0.9530;
                            PDD = new double[68] {91.9000000000000, 92.3000000000000, 94.1000000000000, 96.1000000000000, 97.3000000000000, 98.3000000000000, 98.8000000000000, 99.3000000000000, 99.6000000000000, 99.7000000000000, 99.9000000000000, 100, 100, 99.8000000000000, 99.8000000000000, 99.6000000000000, 99.3000000000000, 98.9000000000000, 98.6000000000000, 98.1000000000000, 97.5000000000000, 96.8000000000000, 95.9000000000000, 94.9000000000000, 93.7000000000000, 92.3000000000000, 90.5000000000000, 88.6000000000000, 86.3000000000000, 83.6000000000000, 80.6000000000000, 77, 73, 68.7000000000000, 63.8000000000000, 58.5000000000000, 52.9000000000000, 47.1000000000000, 41.3000000000000, 35.6000000000000, 30.1000000000000, 25, 20.6000000000000, 16.8000000000000, 13.8000000000000, 11.6000000000000, 10, 9, 8.50000000000000, 8.20000000000000, 8.10000000000000, 7.90000000000000, 7.90000000000000, 7.80000000000000, 7.80000000000000, 7.80000000000000, 7.80000000000000, 7.70000000000000, 7.70000000000000, 7.60000000000000, 7.60000000000000, 7.40000000000000, 7.40000000000000, 7.30000000000000, 7.20000000000000, 7.10000000000000, 7.10000000000000, 7};
                            f_SSD_ext = new double[2] { 1.9948e-5, 0.0096};
                            break;
                        case ("A40"):
                            F_App = 0.9020;
                            //PDD = new double[34] { 75.100, 76, 80.200, 85.800, 91.900, 97.200, 100, 98.300, 90.800, 77.100, 58.500, 37.900, 19.600, 7.5000, 2.1000, 0.80000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.60000, 0.50000, 0.60000, 0.50000, 0.50000, 0.60000, 0.60000, 0.50000, 0.50000, 0.50000 };                                   
                            f_SSD_ext = new double[2] { 0, 0 };
                            break;
                    }
                    break;
                }
                break;
        }

            var PDD_WED=Interp1D(prof_PDD, PDD, WED, 1);
            var f_ext= f_SSD_ext[0] * Math.Pow((SSD/100)-100,2) + f_SSD_ext[1] * Math.Pow(((SSD / 100) - 100)+1, 2);
            var DR = DoseRef * PDD_WED/f_ext;


            // Factor de Conformacion(cutout factor (COF))
            // kernel superficial
               double cx = Math.Round(x_calc);
               double cy = Math.Round(y_calc);
               var lx = x.Length;
               var ly = y.Length;
               float[,] D = new float[lx,ly];
               float[,] Kfit100 = new float[lx, ly];
               float[,] Kfit110 = new float[lx, ly];

            for (int i=0;i<lx;i++)
            {
                for(int j=0;j<lx;j++)
                {
                    D[i, j] = (float) (Math.Pow((x[i] - cx), 2)) +(float) (Math.Pow((y[j] - cy), 2));
                    Kfit100[i, j] = (float)(Interp1D(x_Kernel_SSD100, f_Kernel_SSD100,(double)(D[i, j]),-1));
                    Kfit110[i, j] = (float)(Interp1D(x_Kernel_SSD110, f_Kernel_SSD110, (double)(D[i, j]), -1));
                }
            }

            //Fluencia


           
            /*
                      %% Codigo de calculo para electrones escrito por Fernando Lisa

                   if strcmp(equipo,'D-2300CD')==1
                       eq='Varian_Trilogy';
                   elseif strcmp(equipo,'2100CMLC')==1
                       eq=['Varian_' equipo];
                   end

                   else
                       load('data_para_electrones') %cargo la estructura con la info


                       % Reingreso la prof eff si es necesario
 
                       %% PDD
                       pdd = interp1(data_electrones.(eq).(en).PDDs.(cono)(:,1),data_electrones.(eq).(en).PDDs.(cono)(:,2),prof_eff{idx_campo});
                       DR=data_electrones.(eq).(en).DoseRate_de_referencia*(pdd/100);
                       %% Factor de Cono
                       DR=DR*data_electrones.(eq).(en).Factores_de_Cono(2,Aplicator);
                       %% Factor de SSD ext
                       DR=DR/((data_electrones.(eq).(en).SSDextendida.Coeficientes_del_ajuste.(cono)(1)*((DFP_ci{idx_campo}/10-100)^2) + data_electrones.(eq).(en).SSDextendida.Coeficientes_del_ajuste.(cono)(2)*(DFP_ci{idx_campo}/10-100) + 1)^2);
                       %% Factor de Conformacion (cutout factor (COF))
                       % kernel superficial
                       cx=round(coord_cal(1));%coordenada X del punto de calculo
                       cy=round(coord_cal(3));%coordenada Y del punto de calculo
                       [X,Y]=meshgrid(x,y);
                       D=((X-cx).^2+(Y-cy).^2).^(.5);%OJO ACA!____________________________________-------______________________________________________--------------------------------------------------------------------------
                       clearvars K_int K_int2 K_fit K_fit2
                       if DFP_ci{idx_campo}>1000 && DFP_ci{idx_campo}<1100 %si la ssd esta entre 100 y 110 calculo el COF a 100 y a 110 y despues interpolo entre esos 2
                           %% calculo el cof a 100
                           K_fit=interp1(data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_100cm(:,1),data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_100cm(:,2),D);

                           % fluencia
                           sizex=length(x);
                           sizey=length(y);
                           % saco los datos de plomo del dicom
                           clearvars fl
                           if isfield(infoRTPlan.BeamSequence.(item),'BlockSequence')==1 %corroboro que haya plomo dibujado
                               s=size(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData,1);
                               xf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(1:2:s);
                               yf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(2:2:s);
                               xf(end+1)=xf(1);
                               yf(end+1)=yf(1);
                               xf=xf';
                               yf=yf';
                               xf=xf+round(sizex/2); %centrado en x
                               yf=yf+round(sizey/2); %centrado en y

                               w=strcmp(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockType,'APERTURE');%si dice shielding me toma el plomo invertido por eso hago esto
                               if w==1
                                   fl=poly2mask(xf,yf,sizey,sizex); %creo la matriz con 1 dentro de la superficie y 0 fuera
                               else
                                   fl=~poly2mask(xf,yf,sizey,sizex);
                               end

                               %% calculo el cof a 100
                               matriz_cof_fit=fl.*K_fit;
                               COF_fit_100=sum(matriz_cof_fit(:))/sum(K_fit(:));
                           else %si no hay plomo el factor de conformacion vale 1
                               COF_fit_100=1;
                           end

                           %% calculo el cof a 110
                           K_fit=interp1(data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_110cm(:,1),data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_110cm(:,2),D);

                           % fluencia
                           sizex=length(x);
                           sizey=length(y);
                           % saco los datos de plomo del dicom
                           clearvars fl
                           if isfield(infoRTPlan.BeamSequence.(item),'BlockSequence')==1 %corroboro que haya plomo dibujado
                               s=size(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData,1);
                               xf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(1:2:s);
                               yf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(2:2:s);
                               xf(end+1)=xf(1);
                               yf(end+1)=yf(1);
                               xf=xf';
                               yf=yf';
                               % agrando el tamaño de la figura del plomo en un 10%
                               % porque a ssd110 la fluencia en superficie es mas grande que a ssd100
                               xf=xf*1.1;
                               yf=yf*1.1;

                               xf=xf+round(sizex/2); %centrado en x
                               yf=yf+round(sizey/2); %centrado en y
                               w=strcmp(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockType,'APERTURE');
                               if w==1
                                   fl=poly2mask(xf,yf,sizey,sizex); %creo la matriz con 1 dentro de la superficie y 0 fuera
                               else
                                   fl=~poly2mask(xf,yf,sizey,sizex);
                               end
                               %             figure
                               %             surf(double,'EdgeColor','none')
                               %% calculo el cof
                               matriz_cof_fit=fl.*K_fit;
                               COF_fit_110=sum(matriz_cof_fit(:))/sum(K_fit(:));
                               %         surf(x,y,K_int{1,3},'LineStyle','none')
                           else
                               fl=ones(sizey,sizex);
                               COF_fit_110=1;
                           end

                           %% interpolo entre los dos cof
                           COF_fit = COF_fit_100 + ((COF_fit_110-COF_fit_100)/(10))*(110-DFP_ci{idx_campo}/10);

                           %%  ssd 100
                       elseif DFP_ci{idx_campo}==1000
                           K_fit=interp1(data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_100cm(:,1),data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_100cm(:,2),D);

                           % fluencia
                           sizex=length(x);
                           sizey=length(y);
                           % saco los datos de plomo del dicom
                           clearvars fl

                           if isfield(infoRTPlan.BeamSequence.(item),'BlockSequence')==1
                               s=size(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData,1);
                               xf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(1:2:s);
                               yf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(2:2:s);
                               xf(end+1)=xf(1);
                               yf(end+1)=yf(1);
                               xf=xf';
                               yf=yf';
                               xf=xf+round(sizex/2); %centrado en x
                               yf=yf+round(sizey/2); %centrado en y

                               w=strcmp(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockType,'APERTURE');
                               if w==1
                                   fl=poly2mask(xf,yf,sizey,sizex); %creo la matriz con 1 dentro de la superficie y 0 fuera
                               else
                                   fl=~poly2mask(xf,yf,sizey,sizex);
                               end

                               %% calculo el cof
                               matriz_cof_fit=fl.*K_fit;
                               COF_fit=sum(matriz_cof_fit(:))/sum(K_fit(:));
                           else
                               fl=ones(sizey,sizex);
                               COF_fit=1;
                           end
                           %% ssd mayor a 110
                       else
                           K_fit=interp1(data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_110cm(:,1),data_electrones.(eq).(en).Kernels.Output_Kernel.Calculado_por_fiteo.SSD_110cm(:,2),D);
                           % fluencia
                           sizex=length(x);
                           sizey=length(y);
                           % saco los datos de plomo del dicom
                           clearvars fl

                           if isfield(infoRTPlan.BeamSequence.(item),'BlockSequence')==1 %corroboro que el campo tenga un plomo
                               s=size(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData,1);
                               xf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(1:2:s);
                               yf=infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockData(2:2:s);
                               xf(end+1)=xf(1);
                               yf(end+1)=yf(1);
                               xf=xf';
                               yf=yf';
                               % agrando el tamaño de la figura del plomo en un 10%
                               % porque a ssd110 la fluencia en superficie es mas grande que a ssd100
                               xf=xf*1.1;
                               yf=yf*1.1;
                               xf=xf+round(sizex/2); %centrado en x
                               yf=yf+round(sizey/2); %centrado en y
                               w=strcmp(infoRTPlan.BeamSequence.(item).BlockSequence.Item_1.BlockType,'APERTURE');
                               if w==1
                                   fl=poly2mask(xf,yf,sizey,sizex); %creo la matriz con 1 dentro de la superficie y 0 fuera
                               else
                                   fl=~poly2mask(xf,yf,sizey,sizex);
                               end
                               %% calculo el cof
                               matriz_cof_fit=fl.*K_fit;
                               COF_fit=sum(matriz_cof_fit(:))/sum(K_fit(:));
                           else
                               fl=ones(sizey,sizex);
                               COF_fit=1;
                           end
                       end
                       DR=DR*COF_fit;

                       %% Factor de Oblicuidad
                       prompt={'Ingrese el valor de \theta (en grados) entre 0º y 60º'};
                       name = [infoRTPlan.BeamSequence.(item).BeamName,': Factor de Oblicuidad'];
                       defaultans = {'0'};
                       options.Interpreter = 'tex';
                       theta = inputdlg(prompt,name,[1 50],defaultans,options);
                       theta = str2num(theta{:});
                       while theta<0 || theta>60
                           prompt={'Ingrese el valor de \theta (en grados) [entre 0º y 60º]'};
                           name = [infoRTPlan.BeamSequence.(item).BeamName,': Factor de Oblicuidad'];
                           defaultans = {'0'};
                           options.Interpreter = 'tex';
                           theta = inputdlg(prompt,name,[1 50],defaultans,options);
                           theta = str2num(theta{:});
                       end
                       Z=prof_eff{idx_campo}/Rp;
                       clearvars obliq_f a_of
                       if theta==0
                       elseif theta>0 && theta<30
                           a_of= data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_0 + (data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_30 - data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_0) * (theta-0)/(30-0);
                           obliq_f= interp1(a_of(2:end,1),a_of(2:end,energy),Z);
                           DR=DR*obliq_f;
                       elseif theta==30
                           obliq_f=interp1(data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_30(2:end,1),data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_30(2:end,energy),Z);
                           DR=DR*obliq_f;
                       elseif theta>30 && theta<45
                           a_of= data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_30 + (data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_45 - data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_30) * (theta-30)/(45-30);
                           obliq_f= interp1(a_of(2:end,1),a_of(2:end,energy),Z);
                           DR=DR*obliq_f;
                       elseif theta==45
                           obliq_f=interp1(data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_45(2:end,1),data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_45(2:end,energy),Z);
                           DR=DR*obliq_f;
                       elseif theta>45 && theta<60
                           a_of= data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_45 + (data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_60 - data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_45) * (theta-45)/(60-45);
                           obliq_f= interp1(a_of(2:end,1),a_of(2:end,energy),Z);
                           DR=DR*obliq_f;
                       else
                           obliq_f=interp1(data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_60(2:end,1),data_electrones.info_adicional.tabla_de_oblicuidad.Obliquity_factors_60(2:end,energy),Z);
                           DR=DR*obliq_f;
                       end
                       %% dosis al medio
                       prompt={'Ingrese la densidad del medio en el punto de cálculo (g/cm3)'};
                       name = [infoRTPlan.BeamSequence.(item).BeamName,': Dosis al medio'];
                       defaultans = {'1'};
                       options.Interpreter = 'tex';
                       rho = inputdlg(prompt,name,[1 50],defaultans,options);
                       rho = str2num(rho{:});

                       if rho<0.795
                           F_wm=1.039;
                       else
                           F_wm=rho^(-0.17);
                       end
                       DR_indep=DR*F_wm;
                       UM_indep=dosis_dia_TPS/DR_indep;
                   end

                   %% Aviso
                   if DFP_ci{idx_campo}>1200 && energia<12
                       msgbox('El cálculo puede ser limitado debido a la combinación de bajas energías y SSD extendidas','Warning','warn')
                   end
               end


                    */

            return 0;
        }


        public double TMR(double WED, double TC,double dmax, double Dose, double [] TC_PDD, double[] prof_PDD, double [,] PDD, double TPR20_10, double[] TC_Sp, double [] IC_Sp, double [,] Sp)
        {
            double DFI = 1000;

            double TC_dmax = TC * (DFI + dmax) / (DFI + WED);
            double TC_Iso = TC * DFI / (DFI + WED);
            double Sp1a = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, TC_dmax);
            double Sp1b = Interp2D(TC_PDD, prof_PDD, PDD, 100, 100);
            double Sp1c = Interp2D(TC_PDD, prof_PDD, PDD, TC_dmax, 100);
            double Sp1 = Sp1a * Sp1b / Sp1c;

            double Sp2a = Interp2D(IC_Sp, TC_Sp, Sp, TPR20_10, TC);
            double Sp2b = Interp2D(TC_PDD, prof_PDD, PDD, 100, 100);
            double Sp2c = Interp2D(TC_PDD, prof_PDD, PDD, TC, 100);
            double Sp2 = Sp2a * Sp2b / Sp2c;

            double PDD_med = Interp2D(TC_PDD, prof_PDD, PDD, TC_Iso, WED);
            double TMR_calc = (PDD_med / 100) * (Sp1 / Sp2) * Math.Pow(((DFI + WED) / (DFI + dmax)), 2);

            return TMR_calc;


        }
        public static double Interp1D(double[] X, double[] Array, double yy,double sentido)
        {
           
            int ys = 0;
            double f;
            if (sentido == 1)
            {
                while (yy >= X[ys])
                {
                    ys++;
                }
               f = ((Array[ys] - Array[ys - 1]) / (sentido * X[ys] - sentido * X[ys - 1])) * (yy - sentido * X[ys - 1]) + Array[ys - 1];
            }
            else
            {
                while (yy <= sentido * X[ys])
                {
                    ys++;
                }
                f = ((Array[ys] - Array[ys - 1]) / (sentido * X[ys] - sentido * X[ys - 1])) * (yy - sentido * X[ys - 1]) + Array[ys - 1];

            }
            return f;
        }

        public static double Interp2D(double[] X, double[] Y, double[,] Matrix, double xx, double yy,int sentido=0)
        {
            int xs = 0;
            int ys = 0;
            double f = 0;
            while (xx >= X[xs])
            {
                xs++;
            }
            while (yy >= Y[ys])
            {
                ys++;
            }
            switch (sentido)
            {
                case 0:
                    if (ys == 0)
                    {
                        f = (+(X[xs] - xx) * (yy) * Matrix[ys, xs - 1] + (xx - X[xs - 1]) * (yy) * Matrix[ys, xs]) / ((X[xs] - X[xs - 1]) * (Y[ys]));
                    }
                    else if (xs == 0)
                    {
                        f = ((xx) * (Y[ys] - yy) * Matrix[ys - 1, xs] + (xx) * (yy - Y[ys - 1]) * Matrix[ys, xs]) / ((X[xs]) * (Y[ys] - Y[ys - 1]));
                    }
                    else
                    {
                        f = ((X[xs] - xx) * (Y[ys] - yy) * Matrix[ys - 1, xs - 1] + (xx - X[xs - 1]) * (Y[ys] - yy) * Matrix[ys - 1, xs] + (X[xs] - xx) * (yy - Y[ys - 1]) * Matrix[ys, xs - 1] + (xx - X[xs - 1]) * (yy - Y[ys - 1]) * Matrix[ys, xs]) / ((X[xs] - X[xs - 1]) * (Y[ys] - Y[ys - 1]));
                    }
                    break;
                case 1:
                    if (ys == 0)
                    {
                        f = ((xx - X[xs - 1]) * (Y[ys] - yy) * Matrix[xs - 1, ys] + (xx - X[xs - 1]) * (yy) * Matrix[xs, ys]) / ((X[xs] - X[xs - 1]) * (Y[ys]));
                    }
                    else if (xs == 0)
                    {
                        f = ((X[xs] - xx) * (yy - Y[ys - 1]) * Matrix[xs, ys - 1] + (xx) * (yy - Y[ys - 1]) * Matrix[xs, ys]) / ((X[xs]) * (Y[ys] - Y[ys - 1]));
                    }
                    else
                    {
                        f = ((X[xs] - xx) * (Y[ys] - yy) * Matrix[xs - 1, ys - 1] + (xx - X[xs - 1]) * (Y[ys] - yy) * Matrix[xs - 1, ys] + (X[xs] - xx) * (yy - Y[ys - 1]) * Matrix[xs, ys - 1] + (xx - X[xs - 1]) * (yy - Y[ys - 1]) * Matrix[xs, ys]) / ((X[xs] - X[xs - 1]) * (Y[ys] - Y[ys - 1]));
                    }
                    break;
            }
            return f;
        }
        public static void dibujarMLC(Bitmap bitmap, float[,] MLCs, double x1, double x2, double y1, double y2, int x_ci,int y_ci)
        {
            int yFondo = bitmap.Height / 2; // siempre es 210  + Convert.ToInt32(21 * long1cm);
            int xMedio = bitmap.Width / 2; // siempre es 210;
            int yAncha = 10; //1 * long1cm;
            int yFina = 5; //Convert.ToInt32(0.5 * long1cm);
            int AnchoLamina;
            int xLaminaIzq;
            int xLaminaSiguienteIzq;
            int xLaminaDer;
            int xLaminaSiguienteDer;
            int yLamina = yFondo;
            int yLaminaSiguiente = yLamina - yAncha;
            int height = bitmap.Height;
            int width = bitmap.Width;
            //int x1p = height / 2 + Convert.ToInt32(x1 / 10 * long1cm);
            //int x2p = height / 2 + Convert.ToInt32(x2 / 10 * long1cm);
            //int y1p = width / 2 - Convert.ToInt32(y1 / 10 * long1cm);
            //int y2p = width / 2 - Convert.ToInt32(y2 / 10 * long1cm);

            using (var graphics = Graphics.FromImage(bitmap))
            {               
                //graphics.DrawLine(pen, x, yInicial, x, yFinal);                     
                int ycum = 410;
                for (int i = 0; i < 59; i++)
                {

                    //xLaminaIzq = Convert.ToInt32(xMedio + MLCs[0, i] / 10 * long1cm);
                    //xLaminaSiguienteIzq = Convert.ToInt32(xMedio + MLCs[0, i + 1] / 10 * long1cm);
                    //xLaminaDer = Convert.ToInt32(xMedio + MLCs[1, i] / 10 * long1cm);
                    //xLaminaSiguienteDer = Convert.ToInt32(xMedio + MLCs[1, i + 1] / 10 * long1cm);
                    //yLamina = yLaminaSiguiente;
                    xLaminaIzq = Convert.ToInt32(xMedio + MLCs[0, i] );
                    xLaminaDer = Convert.ToInt32(xMedio + MLCs[1, i] );
                    if (i > 9 && i < 50)
                    {
                        AnchoLamina = yFina;
                    }
                    else
                    {
                        AnchoLamina = yAncha;
                    }
                    
                    graphics.DrawRectangle(pen_verde, 0,ycum-AnchoLamina,xLaminaIzq,AnchoLamina);
                    graphics.DrawRectangle(pen_verde, xLaminaDer, ycum-AnchoLamina, (420-xLaminaDer), AnchoLamina);
                    ycum = ycum - AnchoLamina;
                    /*yLaminaSiguiente = yLamina - AnchoLamina;
                    if (xLaminaDer < x2p && xLaminaIzq > x1p && yLamina < y1p + AnchoLamina && yLamina > y2p && xLaminaIzq != xLaminaDer)
                    {
                        lineaVertical(bitmap, xLaminaIzq, yLamina, yLaminaSiguiente, amarillo);
                        lineaVertical(bitmap, xLaminaDer, yLamina, yLaminaSiguiente, amarillo);
                        lineaHorizontal(bitmap, yLaminaSiguiente, xLaminaIzq, xLaminaSiguienteIzq, amarillo);
                        lineaHorizontal(bitmap, yLaminaSiguiente, xLaminaDer, xLaminaSiguienteDer, amarillo);
                    }*/
                }

                graphics.DrawLine(pen_negra, 0, yFondo, 420, yFondo);
                graphics.DrawLine(pen_negra, 200, yFondo - 50, 220, yFondo - 50);
                graphics.DrawLine(pen_negra, 200, yFondo + 50, 220, yFondo + 50);
                graphics.DrawLine(pen_negra, 200, yFondo - 100, 220, yFondo - 100);
                graphics.DrawLine(pen_negra, 200, yFondo + 100, 220, yFondo + 100);
                graphics.DrawLine(pen_negra, 200, yFondo - 150, 220, yFondo - 150);
                graphics.DrawLine(pen_negra, 200, yFondo + 150, 220, yFondo + 150);

                graphics.DrawLine(pen_negra, xMedio, 0, xMedio, 420);
                graphics.DrawLine(pen_negra, xMedio - 50, 200, xMedio - 50, 220); //-x
                graphics.DrawLine(pen_negra, xMedio + 50, 200, xMedio + 50, 220);//+x
                graphics.DrawLine(pen_negra, xMedio - 100, 200, xMedio - 100, 220); //-x
                graphics.DrawLine(pen_negra, xMedio + 100, 200, xMedio + 100, 220);//+x
                graphics.DrawLine(pen_negra, xMedio - 150, 200, xMedio - 150, 220); //-x
                graphics.DrawLine(pen_negra, xMedio + 150, 200, xMedio + 150, 220);//+x


                graphics.DrawRectangle(pen_rojo, Convert.ToInt32(xMedio - x1), Convert.ToInt32(yFondo - y2), Convert.ToInt32(x2 + x1), Convert.ToInt32(y2 + y1));
                graphics.DrawEllipse(pen_azul, 210+x_ci, 210-y_ci, 5, 5);
            }
        }
        public static void lineaVertical(Bitmap bitmap, int x, int yInicial, int yFinal, Pen pen)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawLine(pen, x, yInicial, x, yFinal);
            }
        }
        public static void lineaHorizontal(Bitmap bitmap, int y, int xInicial, int xFinal, Pen pen)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawLine(pen, xInicial, y, xFinal, y);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }



    class Equipo
    {
        public int SN;
        public string Name;
        public double Dmax;
        public double tr_MLC;
        public double TPR_20_10; // obtenido del Eclipse
        public double Dose; //tasa de dosis para 10x10 a prof del maximo a DFP=DFI
        public int Energia;


        public Equipo(int serial, string nombre, double D_max, double Tr_MLC, double TPR, double Dosis, int E)
        {
            this.SN = serial;
            this.Name = nombre;
            this.Dmax = D_max;
            this.tr_MLC = Tr_MLC;
            this.TPR_20_10 = TPR;
            this.Dose = Dosis;
            this.Energia = E;
        }
    }
}
