using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAKETS
{
    public partial class newcreationstep1 : UserControl
    {
        public newcreationstep1()
        {
            InitializeComponent();

            // Hacer que los PictureBox respondan al click para importar una foto
            this.pictureBox1.Click += PictureBox_Click;
            this.pictureBox2.Click += PictureBox_Click;

            // Mejora visual: mostrar cursor de mano y ajustar modo de visualización
            this.pictureBox1.Cursor = Cursors.Hand;
            this.pictureBox2.Cursor = Cursors.Hand;
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            // Suscribir a botones "de importación" si existen: sólo los botones que tengan un PictureBox en su mismo contenedor
            foreach (var btn in GetAllControls(this).OfType<Button>())
            {
                if (FindFirstPictureBoxInContainer(btn.Parent) != null)
                {
                    btn.Cursor = Cursors.Hand;
                    btn.Click += ImportButton_Click;
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            var pb = sender as PictureBox;
            if (pb == null) return;
            LoadImageIntoPictureBoxFromDialog(pb);
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Control;
            if (btn == null) return;

            // Buscar PictureBox en el mismo contenedor (p. ej. GroupBox) que el botón
            var target = FindFirstPictureBoxInContainer(btn.Parent) ?? this.pictureBox1 ?? this.pictureBox2;
            if (target != null)
            {
                LoadImageIntoPictureBoxFromDialog(target);
            }
        }

        private void LoadImageIntoPictureBoxFromDialog(PictureBox pb)
        {
            if (pb == null) return;

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Seleccionar imagen";
                ofd.Filter = "Archivos de imagen|*.bmp;*.jpg;*.jpeg;*.png;*.gif|Todos los archivos|*.*";
                ofd.Multiselect = false;

                if (ofd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var fs = File.OpenRead(ofd.FileName))
                    using (var img = Image.FromStream(fs))
                    {
                        var clone = new Bitmap(img);
                        var previous = pb.Image;
                        pb.Image = clone;
                        previous?.Dispose();
                    }

                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Busca recursivamente el primer PictureBox dentro del contenedor dado
        private PictureBox FindFirstPictureBoxInContainer(Control container)
        {
            if (container == null) return null;
            foreach (Control c in container.Controls)
            {
                if (c is PictureBox pb) return pb;
                var nested = FindFirstPictureBoxInContainer(c);
                if (nested != null) return nested;
            }
            return null;
        }

        // Helper para recorrer recursivamente controles hijos
        private IEnumerable<Control> GetAllControls(Control parent)
        {
            if (parent == null) yield break;
            foreach (Control c in parent.Controls)
            {
                yield return c;
                foreach (var child in GetAllControls(c))
                    yield return child;
            }
        }
    }
}
