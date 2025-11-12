using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAKETS
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();

            // Suscribir el menú "Ver la ayuda" para abrir la ayuda clásica
            this.VerlaayudaToolStripMenuItem.Click += VerlaayudaToolStripMenuItem_Click;
        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new newcreation();
            childForm.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void seleccionarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AboutBox1();
            childForm.Show();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void perfilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Perfil de Pakets (*.pakets)|*.pakets";
            ofd.ShowDialog();
        }

        // Manejador para "Ver la ayuda" - intenta abrir help.chm en la carpeta de la aplicación
        private void VerlaayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var helpPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help.chm");
                if (System.IO.File.Exists(helpPath))
                {
                    Help.ShowHelp(this, helpPath);
                    return;
                }

                // Si no hay CHM, informar al usuario (puedes cambiar para abrir una URL en su lugar)
                MessageBox.Show("No se encontró el archivo de ayuda 'help.chm' junto al ejecutable.\nColoque el archivo de ayuda en la carpeta de la aplicación para abrir la ayuda clásica.", "Ayuda no disponible", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la ayuda: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
