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
    public partial class Configuracion : Form
    {
        public Configuracion()
        {
            InitializeComponent();
            TB_RutaPacientes.Text = Properties.Settings.Default.PathPacientes;
            TB_RutaIsos.Text = Properties.Settings.Default.PathIsos;
            TB_RutaCopiaIsos.Text = Properties.Settings.Default.PathCopiaIsos;
            TB_RutaPrograma.Text = Properties.Settings.Default.PathPrograma;
        }

        private void BT_SeleccionarPacientes_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Properties.Settings.Default.PathPacientes;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                TB_RutaPacientes.Text = fbd.SelectedPath;
            }
        }

        private void BT_SeleccionarIsos_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Properties.Settings.Default.PathIsos;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                TB_RutaIsos.Text = fbd.SelectedPath;
            }
        }

        private void BT_SeleccionarCopiaIsos_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Properties.Settings.Default.PathCopiaIsos;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                TB_RutaCopiaIsos.Text = fbd.SelectedPath;
            }
        }

        private void BT_SeleccionarPrograma_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Properties.Settings.Default.PathPrograma;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                TB_RutaPrograma.Text = fbd.SelectedPath;
            }
        }

        private void BT_Guardar_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PathPacientes = TB_RutaPacientes.Text;
            Properties.Settings.Default.PathIsos = TB_RutaIsos.Text;
            Properties.Settings.Default.PathCopiaIsos = TB_RutaCopiaIsos.Text;
            Properties.Settings.Default.PathPrograma = TB_RutaPrograma.Text;
            Properties.Settings.Default.Save();
            Close();
        }

        private void BT_Cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
