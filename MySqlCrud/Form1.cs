using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySqlCrud
{
    public partial class Form1 : Form
    {
        String conexion = @"Server=localhost;Database=bookdb;Uid=root;Pwd=juancho0618;";
        int bookID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(conexion))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("BookAddOrEdit", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_BookID", bookID);
                mySqlCmd.Parameters.AddWithValue("_BookName", txtNombre.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Author", txtAutor.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Description", txtDescripcion.Text.Trim());
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Agregado correctamente");
                clear();
                GridFill();
            }
        }

        void GridFill() {
            using (MySqlConnection mysqlCon = new MySqlConnection(conexion))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("BookViewAll", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblBook = new DataTable();
                sqlDa.Fill(dtblBook);
                libro.DataSource = dtblBook;
                libro.Columns[0].Visible = false;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clear(); 
            GridFill();
            btnEliminar.Enabled = false;
        }

        void clear() {
            txtAutor.Text = txtNombre.Text = txtDescripcion.Text = txtBuscar.Text = "";
            bookID = 0;
            btn_Guardar.Text = "Guardar";
        }

        private void libro_DoubleClick(object sender, EventArgs e)
        {
            if (libro.CurrentRow.Index != -1) {
                txtNombre.Text = libro.CurrentRow.Cells[1].Value.ToString();
                txtAutor.Text = libro.CurrentRow.Cells[2].Value.ToString();
                txtDescripcion.Text = libro.CurrentRow.Cells[3].Value.ToString();
                bookID = Convert.ToInt32(libro.CurrentRow.Cells[0].Value.ToString());
                btn_Guardar.Text = "Editar";
                btnEliminar.Enabled = Enabled;
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(conexion))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("BookSearchByValue", mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_SearchValue", txtBuscar.Text);
                DataTable dtblBook = new DataTable();
                sqlDa.Fill(dtblBook);
                libro.DataSource = dtblBook;
                libro.Columns[0].Visible = false;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Eliminado correctamente "+bookID);
            using (MySqlConnection mysqlCon = new MySqlConnection(conexion))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("BookDeleteByID", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_BookID", bookID);
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Eliminado correctamente");
                clear();
                GridFill();
            }
        }
    }
}
