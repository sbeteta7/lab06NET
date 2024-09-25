using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace lab06
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listar_clientes();
        }

        // Remove placeholder text when user focuses on TextBox
        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Compañía" || textBox.Text == "Nombre" || textBox.Text == "ID" ||
                textBox.Text == "Cargo" || textBox.Text == "Dirección" || textBox.Text == "Ciudad" ||
                textBox.Text == "Región" || textBox.Text == "Código Postal" || textBox.Text == "País" ||
                textBox.Text == "Teléfono" || textBox.Text == "Fax")
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        // Add placeholder text when user leaves TextBox empty
        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                switch (textBox.Name)
                {
                    case "txtNombreCompañia":
                        textBox.Text = "Compañía";
                        break;
                    case "txtNombreContacto":
                        textBox.Text = "Nombre";
                        break;
                    case "txtId":
                        textBox.Text = "ID";
                        break;
                    case "txtCargoContacto":
                        textBox.Text = "Cargo";
                        break;
                    case "txtDireccion":
                        textBox.Text = "Dirección";
                        break;
                    case "txtCiudad":
                        textBox.Text = "Ciudad";
                        break;
                    case "txtRegion":
                        textBox.Text = "Región";
                        break;
                    case "txtCodPostal":
                        textBox.Text = "Código Postal";
                        break;
                    case "txtPais":
                        textBox.Text = "País";
                        break;
                    case "txtTelefono":
                        textBox.Text = "Teléfono";
                        break;
                    case "txtFax":
                        textBox.Text = "Fax";
                        break;
                }
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        // Database connection method
        public SqlConnection conectar()
        {
            try
            {
                string cadena = "Data Source=DESKTOP-RS5KB90\\SQLEXPRESS2017;Initial Catalog=Neptuno;User ID=sebas;Password=tecsup123;";
                SqlConnection connection = new SqlConnection(cadena);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        // Listar clientes que tengan activo = 1
        public void listar_clientes()
        {
            try
            {
                SqlConnection conexion = conectar();
                SqlCommand command = new SqlCommand("sp_ListarClientes", conexion);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                List<Cliente> listaClientes = new List<Cliente>();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Cliente cliente = new Cliente();
                    cliente.idCliente = reader["idCliente"].ToString();
                    cliente.NombreCompañia = reader["NombreCompañia"].ToString();
                    cliente.NombreContacto = reader["NombreContacto"].ToString();
                    cliente.CargoContacto = reader["CargoContacto"].ToString();
                    cliente.Direccion = reader["Direccion"].ToString();
                    listaClientes.Add(cliente);
                }

                conexion.Close();
                dgvClientes.ItemsSource = listaClientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Insertar cliente y recargar la tabla
        private void insertar_clientes()
        {
            try
            {
                SqlConnection conexion = conectar();
                SqlCommand command = new SqlCommand("sp_InsertarCliente", conexion);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParameter1 = new SqlParameter("@idCliente", SqlDbType.VarChar);
                sqlParameter1.Value = txtId.Text;

                SqlParameter sqlParameter2 = new SqlParameter("@NombreCompañia", SqlDbType.VarChar);
                sqlParameter2.Value = txtNombreCompañia.Text;

                SqlParameter sqlParameter3 = new SqlParameter("@NombreContacto", SqlDbType.VarChar);
                sqlParameter3.Value = txtNombreContacto.Text;

                SqlParameter sqlParameter4 = new SqlParameter("@CargoContacto", SqlDbType.VarChar);
                sqlParameter4.Value = txtCargoContacto.Text;

                SqlParameter sqlParameter5 = new SqlParameter("@Direccion", SqlDbType.VarChar);
                sqlParameter5.Value = txtDireccion.Text;

                SqlParameter sqlParameter6 = new SqlParameter("@Ciudad", SqlDbType.VarChar);
                sqlParameter6.Value = txtCiudad.Text;

                SqlParameter sqlParameter7 = new SqlParameter("@Region", SqlDbType.VarChar);
                sqlParameter7.Value = txtRegion.Text;

                SqlParameter sqlParameter8 = new SqlParameter("@CodPostal", SqlDbType.VarChar);
                sqlParameter8.Value = txtCodPostal.Text;

                SqlParameter sqlParameter9 = new SqlParameter("@Pais", SqlDbType.VarChar);
                sqlParameter9.Value = txtPais.Text;

                SqlParameter sqlParameter10 = new SqlParameter("@Telefono", SqlDbType.VarChar);
                sqlParameter10.Value = txtTelefono.Text;

                SqlParameter sqlParameter11 = new SqlParameter("@Fax", SqlDbType.VarChar);
                sqlParameter11.Value = txtFax.Text;

                // Agregar los parámetros al comando
                command.Parameters.Add(sqlParameter1);
                command.Parameters.Add(sqlParameter2);
                command.Parameters.Add(sqlParameter3);
                command.Parameters.Add(sqlParameter4);
                command.Parameters.Add(sqlParameter5);
                command.Parameters.Add(sqlParameter6);
                command.Parameters.Add(sqlParameter7);
                command.Parameters.Add(sqlParameter8);
                command.Parameters.Add(sqlParameter9);
                command.Parameters.Add(sqlParameter10);
                command.Parameters.Add(sqlParameter11);

                command.ExecuteNonQuery();
                conexion.Close();

                // Recargar la lista de clientes después de la inserción
                listar_clientes();

                MessageBox.Show("Cliente insertado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Editar cliente
        private void editar_cliente()
        {
            try
            {
                SqlConnection conexion = conectar();
                SqlCommand command = new SqlCommand("sp_EditarCliente", conexion);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParameter1 = new SqlParameter("@idCliente", SqlDbType.VarChar);
                sqlParameter1.Value = txtId.Text;

                SqlParameter sqlParameter2 = new SqlParameter("@NombreCompañia", SqlDbType.VarChar);
                sqlParameter2.Value = txtNombreCompañia.Text;

                SqlParameter sqlParameter3 = new SqlParameter("@NombreContacto", SqlDbType.VarChar);
                sqlParameter3.Value = txtNombreContacto.Text;

                SqlParameter sqlParameter4 = new SqlParameter("@CargoContacto", SqlDbType.VarChar);
                sqlParameter4.Value = txtCargoContacto.Text;

                SqlParameter sqlParameter5 = new SqlParameter("@Direccion", SqlDbType.VarChar);
                sqlParameter5.Value = txtDireccion.Text;

                SqlParameter sqlParameter6 = new SqlParameter("@Ciudad", SqlDbType.VarChar);
                sqlParameter6.Value = txtCiudad.Text;

                SqlParameter sqlParameter7 = new SqlParameter("@Region", SqlDbType.VarChar);
                sqlParameter7.Value = txtRegion.Text;

                SqlParameter sqlParameter8 = new SqlParameter("@CodPostal", SqlDbType.VarChar);
                sqlParameter8.Value = txtCodPostal.Text;

                SqlParameter sqlParameter9 = new SqlParameter("@Pais", SqlDbType.VarChar);
                sqlParameter9.Value = txtPais.Text;

                SqlParameter sqlParameter10 = new SqlParameter("@Telefono", SqlDbType.VarChar);
                sqlParameter10.Value = txtTelefono.Text;

                SqlParameter sqlParameter11 = new SqlParameter("@Fax", SqlDbType.VarChar);
                sqlParameter11.Value = txtFax.Text;

                // Agregar los parámetros al comando
                command.Parameters.Add(sqlParameter1);
                command.Parameters.Add(sqlParameter2);
                command.Parameters.Add(sqlParameter3);
                command.Parameters.Add(sqlParameter4);
                command.Parameters.Add(sqlParameter5);
                command.Parameters.Add(sqlParameter6);
                command.Parameters.Add(sqlParameter7);
                command.Parameters.Add(sqlParameter8);
                command.Parameters.Add(sqlParameter9);
                command.Parameters.Add(sqlParameter10);
                command.Parameters.Add(sqlParameter11);

                command.ExecuteNonQuery();
                conexion.Close();

                // Recargar la lista de clientes después de la edición
                listar_clientes();

                MessageBox.Show("Cliente editado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Eliminar cliente (eliminación lógica, cambiar activo a 0)
        private void eliminar_cliente()
        {
            try
            {
                SqlConnection conexion = conectar();
                SqlCommand command = new SqlCommand("sp_EliminarCliente", conexion);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idCliente", txtId.Text);  // Se debe pasar el ID del cliente para eliminarlo lógicamente
                command.ExecuteNonQuery();
                conexion.Close();

                MessageBox.Show("Cliente eliminado correctamente.");
                listar_clientes();  // Recargar la tabla después de eliminar
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void limpiar_form()
        {

            try
            {
                txtCargoContacto.Text = string.Empty;
            }
            catch { }

        }

        // Evento del botón insertar
        private void Click_Insertar(object sender, RoutedEventArgs e)
        {
            insertar_clientes();
        }

        // Evento del botón editar
        private void Click_Editar(object sender, RoutedEventArgs e)
        {
            editar_cliente();
        }

        // Evento del botón eliminar
        private void Click_Eliminar(object sender, RoutedEventArgs e)
        {
            eliminar_cliente();
        }

        private void Click_Cancelar(object sender, RoutedEventArgs e)
        {

        }
    }
}
