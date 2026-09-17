using System.Windows;

namespace NeptunoWPF
{
    public partial class MainWindow : Window
    {
        private readonly NeptunoDataAccess _dataAccess;

        public MainWindow()
        {
            InitializeComponent();

            string cadenaConexion = "Server=LAB1506-19\\SQLEXPRESS01;Database=Neptuno;Trusted_Connection=True;";
            _dataAccess = new NeptunoDataAccess(cadenaConexion);
        }

        private void BtnCargarProductos_Click(object sender, RoutedEventArgs e)
        {
            dgProductos.ItemsSource = _dataAccess.ListarProductos();
        }

        private void BtnCargarCategorias_Click(object sender, RoutedEventArgs e)
        {
            dgCategorias.ItemsSource = _dataAccess.ListarCategorias();
        }

        private void BtnBuscarProveedores_Click(object sender, RoutedEventArgs e)
        {
            dgProveedores.ItemsSource = _dataAccess.BuscarProveedoresPorContactoYCiudad(txtContacto.Text, txtCiudad.Text);
        }
    }
}