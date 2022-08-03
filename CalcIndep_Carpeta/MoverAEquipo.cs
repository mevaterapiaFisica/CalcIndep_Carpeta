using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CalcIndep_Carpeta
{
    public partial class MoverAEquipo : Form
    {
        public string equipoAEnviar = "NoEnviar";
        public MoverAEquipo(bool tiene10MV)
        {
            InitializeComponent();
            if (tiene10MV)
            {
                BT_EnviarAEq2.Enabled = false;
            }
        }

        private void BT_EnviarAEq2_Click(object sender, EventArgs e)
        {
            equipoAEnviar = "Equipo 2 6EX";
            this.Close();
        }

        private void BT_EnviarAEq3_Click(object sender, EventArgs e)
        {
            equipoAEnviar = "2100CMLC";
            this.Close();
        }

        private void BT_NoEnviar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
