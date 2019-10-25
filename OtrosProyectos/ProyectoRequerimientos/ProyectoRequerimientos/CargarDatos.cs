using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace ProyectoRequerimientos
{
    public partial class CargarDatos : Form
    {
        public CargarDatos()
        {
            InitializeComponent();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ope = new OpenFileDialog();
            ope.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (ope.ShowDialog() == DialogResult.Cancel)
                return;
            ftm_file.Text = ope.FileName;
        }

        private int verificar(string archivo, string hoja)
        {
            int verificacion;
            string con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + archivo + "; Extended Properties =\"Excel 12.0 Xml;HDR=YES\"";

            OleDbConnection origen = default(OleDbConnection);
            origen = new OleDbConnection(con);

            OleDbDataAdapter adaptador = new OleDbDataAdapter("Select * from [" + hoja + "$]", origen);
            DataTable dt = new DataTable();

            if (dt != null)
            {
                verificacion = 1;
            }
            else
            {
                verificacion = 0;
            }
            return verificacion;
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            if (verificar(ftm_file.Text, ftm_hoja.Text) == 1)
            {
                MessageBox.Show("Registros Disponibles");
            }
            else
            {
                MessageBox.Show("No hay Registros para cargar");
            }
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ftm_file.Text + "; Extended Properties =\"Excel 12.0 Xml;HDR=YES\"";

                OleDbConnection origen = default(OleDbConnection);
                origen = new OleDbConnection(con);

                OleDbDataAdapter adaptador = new OleDbDataAdapter("Select * from [" + ftm_hoja.Text + "$]", origen);
                DataTable dt = new DataTable();

                if (dt != null)
                {
                    adaptador.Fill(dt);
                    //dataGridView1.DataSource = dt;
                    MessageBox.Show("Registros Cargados Temporalmente");
                }
                else
                {
                    MessageBox.Show("No hay ningun Registro");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string con = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ftm_file.Text + "; Extended Properties =\"Excel 12.0 Xml;HDR=YES\"";
                OleDbConnection origen = default(OleDbConnection);
                origen = new OleDbConnection(con);
                OleDbDataAdapter adaptador = new OleDbDataAdapter("Select * from [" + ftm_hoja.Text + "$]", origen);
                DataTable dt = new DataTable();
                if (dt != null)
                {
                    adaptador.Fill(dt);
                    using (SqlConnection conn = new SqlConnection("Data Source = DESKTOP-U983H9I; Initial Catalog = DB_C1; Integrated Security = True"))
                    {
                        conn.Open();
                        foreach (DataRow ImportRow in dt.Rows)
                        {
                            SqlCommand cmd = new SqlCommand("insert into Rqms (Fecha, Pedido, Destino, Observacion, Analista, Transporte, Statu, Semana,Bodega,Pais)" +
                            "values(@fecha, @pedido, @destino, @observacion, @analista, @transporte, @statu, @semana, @bodega, @pais)", conn);

                            cmd.Parameters.AddWithValue("@fecha", Convert.ToDateTime(ImportRow[0]).ToString("yyyy/MM/dd"));
                            cmd.Parameters.AddWithValue("@pedido", ImportRow[1]);
                            cmd.Parameters.AddWithValue("@destino", ImportRow[2]);
                            cmd.Parameters.AddWithValue("@observacion", ImportRow[3]);
                            cmd.Parameters.AddWithValue("@analista", ImportRow[4]);
                            cmd.Parameters.AddWithValue("@transporte", ImportRow[5]);
                            cmd.Parameters.AddWithValue("@statu", ImportRow[6]);
                            cmd.Parameters.AddWithValue("@semana", ImportRow[7]);
                            cmd.Parameters.AddWithValue("@bodega", ImportRow[8]);
                            cmd.Parameters.AddWithValue("@pais", ImportRow[9]);
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    MessageBox.Show("Registros Cargados Exitosamente");
                    
                }
                else
                {
                    MessageBox.Show("No hay ningun Registro");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
