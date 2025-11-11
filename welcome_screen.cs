using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using System.Deployment.Application;

namespace PAKETS
{
    public partial class welcome_screen : Form
    {
        private ProgressBar progressBar;
        private Label label;
        private Timer timer;

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
            main mainForm = new main();
        }

        public welcome_screen()
        {
            InitializeComponent();

            // Crear y configurar el ProgressBar sin Dock para poder controlar Size/Location
            progressBar = new ProgressBar();
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 50;
            progressBar.Size = new Size(260, 23);
            progressBar.Dock = DockStyle.None;
            progressBar.Anchor = AnchorStyles.Top;
            this.Controls.Add(progressBar);

            // Label
            label = new Label();
            label.Text = "Iniciando";
            label.Dock = DockStyle.Top;
            label.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(label);

            // Timer
            timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += Timer_Tick;
            timer.Start();

            // Centrar y actualizar posición cuando el formulario se cargue o cambie de tamaño
            this.Load += Welcome_screen_Load;
            this.Resize += Welcome_screen_Resize;

            // Forzar actualización también en Shown (se dispara tras Load y asegura que se ejecute)
            this.Shown += (s, e) => UpdateBuildLabel();

            // Actualizar la etiqueta de compilación inmediatamente (por si acaso)
            UpdateBuildLabel();
        }

        private void Welcome_screen_Load(object sender, EventArgs e)
        {
            CenterProgressBar();
        }

        private void Welcome_screen_Resize(object sender, EventArgs e)
        {
            CenterProgressBar();
        }

        private void CenterProgressBar()
        {
            if (progressBar != null)
            {
                int x = 0;
                int y = 20;
                if (progressBar.Width > this.ClientSize.Width)
                {
                    progressBar.Width = Math.Max(0, this.ClientSize.Width);
                }
                progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                progressBar.Location = new Point(x, y);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        // Método reutilizable para establecer la etiqueta con versión y compilación separadas:
        // "Versión {major}.{minor}   Compilación {build}.{revision}"
        private void UpdateBuildLabel()
        {
            if (this.labelVersion == null) return;

            try
            {
                string fullVersion = null;

                // Preferir versión de publicación (ClickOnce) cuando esté desplegada
                bool isNetworkDeployed = false;
                try
                {
                    isNetworkDeployed = ApplicationDeployment.IsNetworkDeployed;
                }
                catch
                {
                    isNetworkDeployed = false;
                }

                if (isNetworkDeployed)
                {
                    try
                    {
                        var pubVer = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                        fullVersion = pubVer.ToString(); // e.g. "1.2.3.4"
                    }
                    catch
                    {
                        fullVersion = null;
                    }
                }

                // Fallback: usar FileVersion completa
                if (string.IsNullOrEmpty(fullVersion))
                {
                    try
                    {
                        fullVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                    }
                    catch
                    {
                        fullVersion = null;
                    }
                }

                if (string.IsNullOrEmpty(fullVersion))
                {
                    this.labelVersion.Text = "Versión ?";
                    return;
                }

                // Normalizar y separar en partes
                var parts = fullVersion.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                string versionPart;
                string buildPart;

                if (parts.Length >= 4)
                {
                    versionPart = $"{parts[0]}.{parts[1]}";
                    buildPart = $"{parts[2]}.{parts[3]}";
                }
                else if (parts.Length == 3)
                {
                    versionPart = $"{parts[0]}.{parts[1]}";
                    buildPart = parts[2];
                }
                else if (parts.Length == 2)
                {
                    versionPart = $"{parts[0]}.{parts[1]}";
                    buildPart = "?";
                }
                else
                {
                    // Versión no estándar -> mostrar completo en versión y dejar compilación unknown
                    versionPart = fullVersion;
                    buildPart = "?";
                }

                this.labelVersion.Text = $"Versión {versionPart}   Compilación {buildPart}";
            }
            catch
            {
                this.labelVersion.Text = "Versión ?";
            }
        }

        private void welcome_screen_Load(object sender, EventArgs e)
        {
            UpdateBuildLabel();
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void labelVersion_Click(object sender, EventArgs e) { }

        private void labelVersion_Click_1(object sender, EventArgs e)
        {

        }
    }
}