using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace LAB05_DAEA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SqlConnection connection = new SqlConnection(
           "Data Source=HARUMI\\SQLEXPRESS02;Initial Catalog=Neptuno;User ID=harumi;Password=123456;TrustServerCertificate=True");

        public MainWindow()
        {
            InitializeComponent();
            CargarClientes();
        }

        private void CargarClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_ListarClientesActivos", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        IdCliente = reader["idCliente"].ToString(),
                        NombreCompañia = reader["NombreCompañia"].ToString(),
                        NombreContacto = reader["NombreContacto"]?.ToString(),
                        CargoContacto = reader["CargoContacto"]?.ToString(),
                        Direccion = reader["Direccion"]?.ToString(),
                        Ciudad = reader["Ciudad"]?.ToString(),
                        Region = reader["Region"]?.ToString(),
                        CodPostal = reader["CodPostal"]?.ToString(),
                        Pais = reader["Pais"]?.ToString(),
                        Telefono = reader["Telefono"]?.ToString(),
                        Fax = reader["Fax"]?.ToString()
                    });
                }

                reader.Close();
                dgClientes.ItemsSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            string textoBuscar = txtBuscar.Text.Trim();
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_BuscarClientesPorNombre", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NombreCompañia", textoBuscar);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        IdCliente = reader["idCliente"].ToString(),
                        NombreCompañia = reader["NombreCompañia"].ToString(),
                        NombreContacto = reader["NombreContacto"]?.ToString(),
                        CargoContacto = reader["CargoContacto"]?.ToString(),
                        Direccion = reader["Direccion"]?.ToString(),
                        Ciudad = reader["Ciudad"]?.ToString(),
                        Region = reader["Region"]?.ToString(),
                        CodPostal = reader["CodPostal"]?.ToString(),
                        Pais = reader["Pais"]?.ToString(),
                        Telefono = reader["Telefono"]?.ToString(),
                        Fax = reader["Fax"]?.ToString()
                    });
                }

                reader.Close();
                dgClientes.ItemsSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar clientes: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        private void Crear_Click(object sender, RoutedEventArgs e)
        {
            CrearClienteWindow crearWindow = new CrearClienteWindow(connection);
            crearWindow.Owner = this;

            if (crearWindow.ShowDialog() == true)
            {
                CargarClientes();
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is Cliente clienteSeleccionado)
            {
                EditarClienteWindow editarWindow = new EditarClienteWindow(connection, clienteSeleccionado);
                editarWindow.Owner = this;

                if (editarWindow.ShowDialog() == true)
                {
                    CargarClientes();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para editar.");
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is Cliente cliente)
            {
                var confirmacion = MessageBox.Show(
                    $"¿Estás seguro de eliminar al cliente \"{cliente.NombreCompañia}\"?",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (confirmacion == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_EliminarCliente", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente eliminado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el cliente.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar cliente: " + ex.Message);
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }

                    CargarClientes();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para eliminar.");
            }
        }


    }
}
