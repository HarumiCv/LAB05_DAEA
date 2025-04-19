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
    /// Lógica de interacción para EditarClienteWindow.xaml
    /// </summary>
    public partial class EditarClienteWindow : Window
    {
        private SqlConnection connection;
        private Cliente cliente;

        public EditarClienteWindow(SqlConnection conexionExistente, Cliente clienteSeleccionado)
        {
            InitializeComponent();
            connection = conexionExistente;
            cliente = clienteSeleccionado;
            CargarDatos();
        }

        private void CargarDatos()
        {
            txtIdCliente.Text = cliente.IdCliente;
            txtNombreCompañia.Text = cliente.NombreCompañia;
            txtNombreContacto.Text = cliente.NombreContacto;
            txtCargoContacto.Text = cliente.CargoContacto;
            txtDireccion.Text = cliente.Direccion;
            txtCiudad.Text = cliente.Ciudad;
            txtRegion.Text = cliente.Region;
            txtCodPostal.Text = cliente.CodPostal;
            txtPais.Text = cliente.Pais;
            txtTelefono.Text = cliente.Telefono;
            txtFax.Text = cliente.Fax;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_ModificarCliente", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@IdCliente", txtIdCliente.Text.Trim()); // ← ¡ESTA LÍNEA ES CLAVE!
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
                MessageBox.Show("Cliente actualizado correctamente.");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar cliente: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

    }
}
