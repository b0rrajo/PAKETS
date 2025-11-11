using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PAKETS
{
    public partial class newcreationstep2 : UserControl
    {
        // Altura uniforme para todas las filas nuevas
        private readonly float uniformRowHeight = 20f;

        // Helpers que buscan los controles por nombre en el árbol de controles generado por el diseñador.
        // Usamos búsqueda en tiempo de ejecución para evitar errores de compilación si el diseñador
        // no define campos con esos nombres en este partial.
        private TableLayoutPanel TableLayout => this.Controls.Find("tableLayoutPanel1", true).FirstOrDefault() as TableLayoutPanel;
        private Panel Panel1 => this.Controls.Find("panel1", true).FirstOrDefault() as Panel;
        private RadioButton RadioButton1 => this.Controls.Find("radioButton1", true).FirstOrDefault() as RadioButton;

        // Estado público para que el formulario padre pueda consultar y reaccionar
        public event EventHandler CompletionChanged;
        public bool AreAllCellsCompleted { get; private set; }

        public newcreationstep2()
        {
            InitializeComponent();

            var tlp = TableLayout;
            if (tlp != null)
            {
                // Asegurar que las columnas usan proporciones y que el TableLayoutPanel añade filas correctamente
                tlp.GrowStyle = TableLayoutPanelGrowStyle.AddRows;

                // Normalizar estilos de columna a porcentajes para que mantengan ancho relativo
                if (tlp.ColumnCount > 0)
                {
                    float percent = 100f / tlp.ColumnCount;
                    tlp.ColumnStyles.Clear();
                    for (int c = 0; c < tlp.ColumnCount; c++)
                    {
                        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
                    }
                }

                // Normalizar estilos de fila existentes (si ya hay filas en tiempo de diseño)
                EnsureUniformRowStyles();

                // Insertar fila cabecera con los títulos en mayúsculas si no existe
                InsertColumnHeaders();

                // Asegurar que al mostrarse la tabla haya al menos una fila de datos editable
                EnsureInitialDataRow();

                // Suscribir cambios del radiobutton para reevaluar completitud
                if (RadioButton1 != null) RadioButton1.CheckedChanged += (s, e) => EvaluateCompletion();
            }

            // Evaluar estado inicial
            EvaluateCompletion();
        }

        private void EnsureUniformRowStyles()
        {
            var tlp = TableLayout;
            if (tlp == null) return;

            // Asegurar que RowStyles tiene entradas para cada RowCount y que todas son Absolute con la misma altura
            while (tlp.RowStyles.Count < tlp.RowCount)
            {
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, uniformRowHeight));
            }

            for (int i = 0; i < tlp.RowStyles.Count; i++)
            {
                tlp.RowStyles[i].SizeType = SizeType.Absolute;
                tlp.RowStyles[i].Height = uniformRowHeight;
            }
        }

        // Inserta una fila de cabecera en la posición 0 con dos labels: TRANSPORTÍSTA y API
        private void InsertColumnHeaders()
        {
            var tlp = TableLayout;
            if (tlp == null) return;

            // Si ya existen encabezados con esos nombres, salir
            if (tlp.Controls.Find("colHeader_0", true).Any() || tlp.Controls.Find("colHeader_1", true).Any())
                return;

            tlp.SuspendLayout();

            // Asegurar que hay al menos una fila
            if (tlp.RowCount == 0)
            {
                tlp.RowCount = 1;
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, uniformRowHeight));
            }

            // Si hay controles ya en la fila 0, los movemos hacia abajo (append) para liberar la fila 0.
            var controlsInRow0 = tlp.Controls.Cast<Control>().Where(c => tlp.GetRow(c) == 0).ToArray();
            if (controlsInRow0.Length > 0)
            {
                // Aumentar RowCount (append) y añadir RowStyle para la nueva fila al final
                int newRowCount = tlp.RowCount + 1;
                tlp.RowCount = newRowCount;
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, uniformRowHeight));

                // Mover hacia abajo solo los controles inicialmente en la fila 0 (no tocar el resto)
                foreach (var c in controlsInRow0)
                {
                    tlp.SetRow(c, 1); // mover a la siguiente fila (1)
                }
            }

            // Ahora la fila 0 está libre -> añadir los labels de cabecera
            var lblLeft = new Label
            {
                Name = "colHeader_0",
                Text = "TRANSPORTÍSTA",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(FontFamily.GenericSansSerif, 9f, FontStyle.Bold),
                Margin = new Padding(2)
            };

            var lblRight = new Label
            {
                Name = "colHeader_1",
                Text = "API",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font(FontFamily.GenericSansSerif, 9f, FontStyle.Bold),
                Margin = new Padding(2)
            };

            // Asegurar RowStyles antes de añadir
            EnsureUniformRowStyles();

            // Añadir labels en la fila 0, columnas 0 y 1
            tlp.Controls.Add(lblLeft, 0, 0);
            if (tlp.ColumnCount > 1)
            {
                tlp.Controls.Add(lblRight, 1, 0);
            }

            // Asegurar estilos uniformes tras inserción
            EnsureUniformRowStyles();

            tlp.ResumeLayout();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            var rb = RadioButton1;
            var p = Panel1;
            var tlp = TableLayout;

            if (rb != null && p != null && tlp != null)
            {
                if (rb.Checked)
                {
                    p.Enabled = true;
                    tlp.BackColor = Color.White;
                    tlp.Enabled = true;
                }
                else
                {
                    p.Enabled = false;
                    tlp.Enabled = false;
                }
            }

            EvaluateCompletion();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void newcreationstep2_Load(object sender, EventArgs e)
        {
            // Asegurarse de normalizar filas al cargar y volver a insertar encabezados si es necesario
            EnsureUniformRowStyles();
            InsertColumnHeaders();
            EnsureInitialDataRow();
            EvaluateCompletion();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            AddDataRow();
        }

        // Extrae la lógica de añadir fila para poder reutilizarla (botón y creación inicial)
        private void AddDataRow()
        {
            var tlp = TableLayout;
            if (tlp == null) return;

            tlp.SuspendLayout();

            int newRow = tlp.RowCount;
            tlp.RowCount = newRow + 1;

            // Añadir RowStyle consistente (altura fija) para la fila nueva
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, uniformRowHeight));

            // Crear un control editable por cada columna con Dock = Fill
            for (int col = 0; col < tlp.ColumnCount; col++)
            {
                // Evitar duplicar si ya existe un control en esa celda
                var existing = tlp.GetControlFromPosition(col, newRow);
                if (existing != null) continue;

                var textbox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(2),
                    BorderStyle = BorderStyle.FixedSingle,
                    // Nombre único para facilitar depuración y evitar colisiones
                    Name = $"cell_{newRow}_{col}_{Guid.NewGuid().ToString("N").Substring(0, 6)}"
                };

                // Suscribir para reevaluar completitud cuando el usuario escriba
                textbox.TextChanged += (s, e) => EvaluateCompletion();

                tlp.Controls.Add(textbox, col, newRow);
            }

            // Asegurar que todas las RowStyles mantienen la altura uniforme (por si había disparidad)
            EnsureUniformRowStyles();

            tlp.ResumeLayout();

            // Reevaluar tras añadir fila
            EvaluateCompletion();
        }

        // Garantiza que al mostrarse la tabla exista al menos una fila de datos editable justo debajo de la cabecera
        private void EnsureInitialDataRow()
        {
            var tlp = TableLayout;
            if (tlp == null) return;

            // Determinar cuántas filas pertenecen a cabecera (1 si existe el header)
            int headerRows = tlp.Controls.Find("colHeader_0", true).Any() ? 1 : 0;

            // Si ya hay alguna fila de datos (filas totales > headerRows) no hacemos nada
            if (tlp.RowCount > headerRows) return;

            // Añadir una fila de datos (se añade al final; si existe header estará en la posición headerRows)
            AddDataRow();
        }

        // Reevaluar completitud y notificar si cambia
        private void EvaluateCompletion()
        {
            var tlp = TableLayout;
            if (tlp == null) return;

            bool prev = AreAllCellsCompleted;

            // Si el primer radiobutton no está marcado -> no requerimos completar las celdas
            if (RadioButton1 == null || !RadioButton1.Checked)
            {
                AreAllCellsCompleted = true;
            }
            else
            {
                // Comprobar todos los TextBox que están dentro del TableLayout y en filas de datos (>= headerRows)
                int headerRows = tlp.Controls.Find("colHeader_0", true).Any() ? 1 : 0;

                var textboxes = tlp.Controls
                    .OfType<TextBox>()
                    .Where(tb => tlp.GetRow(tb) >= headerRows)
                    .ToArray();

                // Si no hay textboxes consideramos no completado (forzamos al menos una fila existente)
                if (textboxes.Length == 0)
                {
                    AreAllCellsCompleted = false;
                }
                else
                {
                    AreAllCellsCompleted = textboxes.All(tb => !string.IsNullOrWhiteSpace(tb.Text));
                }
            }

            if (AreAllCellsCompleted != prev)
            {
                CompletionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            var tlp = TableLayout;
            if (tlp == null) return;

            // Evitar eliminar la fila de cabecera: determinar mínimo de filas
            int headerRows = tlp.Controls.Find("colHeader_0", true).Any() ? 1 : 0;
            if (tlp.RowCount <= headerRows + 1) return; // mantener al menos una fila de datos si es posible

            tlp.SuspendLayout();

            int lastRow = tlp.RowCount - 1;

            // Recolectar controles en la última fila y eliminarlos
            var toRemove = tlp.Controls
                .Cast<Control>()
                .Where(c => tlp.GetRow(c) == lastRow)
                .ToList();

            foreach (var ctrl in toRemove)
            {
                tlp.Controls.Remove(ctrl);
                ctrl.Dispose();
            }

            // Eliminar RowStyle asociado y decrementar RowCount
            if (tlp.RowStyles.Count > lastRow)
            {
                tlp.RowStyles.RemoveAt(lastRow);
            }

            tlp.RowCount = lastRow;

            // Asegurar uniformidad tras eliminación
            EnsureUniformRowStyles();

            tlp.ResumeLayout();

            // Reevaluar tras eliminar fila
            EvaluateCompletion();
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (RadioButton1.Checked)
            {
                TableLayout.BackColor = Color.White;
                TableLayout.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
            }
            else
            {
                TableLayout.BackColor = Color.LightGray;
                TableLayout.Enabled = false;
                button2.Enabled = false;
                button1.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
            }

            EvaluateCompletion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Click(sender, e);
        }

        private void tableLayoutPanel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Reuse safe deletion logic
            delete_Click(sender, e);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
