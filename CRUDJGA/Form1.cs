using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace CRUDJGA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Métodos
        private static string cadena = ConfigurationManager.ConnectionStrings["CadenaSql"].ConnectionString;
        private SqlConnection cnx = new SqlConnection(cadena);
        public void ListarPaises()
        {
            SqlDataAdapter da = new SqlDataAdapter("usp_Pais_Listar", cnx);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbxPais.DataSource = dt;
            cbxPais.DisplayMember = "NombrePais";
            cbxPais.ValueMember = "Idpais";
        }
        public void ListarClientes()
        {
            SqlDataAdapter da = new SqlDataAdapter("usp_Clientes_ListarTodos", cnx);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvClientes.DataSource = dt;
        }
        public void InsertarClientes(string codigoCliente, string nombreCliente, string direccion, string idPais, string telefono)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_Cliente_Insertar", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@IdCliente", SqlDbType.VarChar, 5, codigoCliente);
                //cmd.Parameters.Add("@NombreCia", SqlDbType.VarChar, 40, nombreCliente);
                //cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 60, direccion);
                //cmd.Parameters.Add("@IdPais", SqlDbType.Char, 3, idPais);
                //cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 24, telefono);
                cmd.Parameters.AddWithValue("IdCliente", codigoCliente);
                cmd.Parameters.AddWithValue("NombreCia", nombreCliente);
                cmd.Parameters.AddWithValue("Direccion", direccion);
                cmd.Parameters.AddWithValue("IdPais",  idPais);
                cmd.Parameters.AddWithValue("Telefono", telefono);

                cnx.Open();

                int i = cmd.ExecuteNonQuery();
                MessageBox.Show($"Se ha agregado {i} cliente");
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally 
            {
                cnx.Close();
            }

        } //fin de insertar clientes...
        public void ActualizarClientes(string codigoCliente, string nombreCliente, string direccion, string idPais, string telefono)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_Cliente_Actualizar", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", codigoCliente);
                cmd.Parameters.AddWithValue("@NombreCia", nombreCliente);
                cmd.Parameters.AddWithValue("@Direccion", direccion);
                cmd.Parameters.AddWithValue("@IdPais", idPais);
                cmd.Parameters.AddWithValue("@Telefono", telefono);

                cnx.Open();

                int i = cmd.ExecuteNonQuery();
                MessageBox.Show($"Se ha actualizado {i} cliente");
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                cnx.Close();
            }
        }// fin de ActualizarCliente...

        public void EliminarClientes(string codigoCliente)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("usp_Cliente_Eliminar", cnx);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCliente", codigoCliente);
             
                cnx.Open();

                int i = cmd.ExecuteNonQuery();
                MessageBox.Show($"Se ha eliminado {i} cliente");
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                cnx.Close();
            }

        } //fin de EliminarClientes...
        void LimpiarFormulario() 
        {
            btnAgregar.Enabled = true;
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = true;

            txtCodigo.Text = String.Empty;
            txtNombre.Text = String.Empty;
            txtDireccion.Text = String.Empty;
            txtTelefono.Text = String.Empty;
            
            txtCodigo.ReadOnly = false;
            txtCodigo.Focus();
        } //fin de LimpiarFormulario() ...



        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            ListarPaises();
            ListarClientes();

            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            InsertarClientes(txtCodigo.Text, txtNombre.Text, txtDireccion.Text, cbxPais.SelectedValue.ToString(), txtTelefono.Text);
            ListarClientes() ;
            LimpiarFormulario();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarClientes(txtCodigo.Text, txtNombre.Text, txtDireccion.Text, cbxPais.SelectedValue.ToString(), txtTelefono.Text);
            ListarClientes();
            LimpiarFormulario();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarClientes(txtCodigo.Text);
            ListarClientes();
            LimpiarFormulario();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow fila = dgvClientes.CurrentRow;
            txtCodigo.Text = fila.Cells[0].Value.ToString();
            txtNombre.Text = fila.Cells[1].Value.ToString();
            txtDireccion.Text = fila.Cells[2].Value.ToString();
            cbxPais.SelectedValue = fila.Cells[3].Value.ToString();
            txtTelefono.Text = fila.Cells[4].Value.ToString();

            txtCodigo.ReadOnly = true;
            btnAgregar.Enabled = false;
            btnActualizar.Enabled = true;
            btnEliminar.Enabled = true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            ListarClientes();
        }
    }
}
