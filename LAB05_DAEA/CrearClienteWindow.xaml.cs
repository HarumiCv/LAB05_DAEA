using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace LAB05_DAEA
{
    /// <summary>
    /// Lógica de interacción para CrearClienteWindow.xaml
    /// </summary>
    public partial class CrearClienteWindow : Window
    {

        private SqlConnection connection;

        public CrearClienteWindow(SqlConnection conexionExistente)
        {
            InitializeComponent();
            connection = conexionExistente;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_InsertarCliente", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@NombreCompañia", txtNombreCompañia.Text.Trim());
                command.Parameters.AddWithValue("@NombreContacto", txtNombreContacto.Text.Trim());
                command.Parameters.AddWithValue("@CargoContacto", txtCargoContacto.Text.Trim());
                command.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                command.Parameters.AddWithValue("@Ciudad", txtCiudad.Text.Trim());
                command.Parameters.AddWithValue("@Region", txtRegion.Text.Trim());
                command.Parameters.AddWithValue("@CodPostal", txtCodPostal.Text.Trim());
                command.Parameters.AddWithValue("@Pais", txtPais.Text.Trim());
                command.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
                command.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());

                command.ExecuteNonQuery();
                MessageBox.Show("Cliente creado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear cliente: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}

