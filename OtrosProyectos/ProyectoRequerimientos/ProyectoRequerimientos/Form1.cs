using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Collections;

namespace ProyectoRequerimientos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TextoTitulo(string texto, int width, int height, int top, int left)
        {
            Label destino = new Label();
            destino.Text = texto;
            destino.ForeColor = Color.Black;
            destino.Font = new Font("Century Gothic", 12 ,FontStyle.Regular);
            destino.Width = width;
            destino.Height = height;
            destino.Top = top;
            destino.Left = left;
            Paneles.Controls.Add(destino);
        }

        private void TextoSubTitulo(string texto, int width, int height, int top, int left)
        {
            Label destino = new Label();
            destino.Text = texto;
            destino.ForeColor = Color.Gray;
            destino.Font = new Font("Arial Narrow", 9, FontStyle.Regular);
            destino.Width = width;
            destino.Height = height;
            destino.Top = top;
            destino.Left = left;
            Paneles.Controls.Add(destino);
        }

        private void PanelResumen(string destino, string transporte, string cantidad, decimal adjudicado, decimal asignado, int top, int left)
        {
            int top_local, left_local;
            top_local = top;
            left_local = left;
            TextoTitulo(destino, 100, 20, top_local, 20);
            TextoTitulo(transporte, 100, 20, top_local, 125);
            TextoTitulo(cantidad, 100, 20, top_local, 270);
            TextoTitulo(adjudicado + " %", 80, 20, top_local, 385);
            TextoTitulo(asignado + " %", 80, 20, top_local, 470);
            top_local += 18;
            TextoSubTitulo("Destino", 100, 20, top_local, 20);
            TextoSubTitulo("Tranporte", 100, 20, top_local, 125);
            TextoSubTitulo("Cantidad de Transportes", 100, 20, top_local, 270);
            TextoSubTitulo("% Adjudicado", 80, 20, top_local, 385);
            TextoSubTitulo("% Asignado", 80, 20, top_local, 470);
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            CargarDatos BaseDatos = new CargarDatos();
            BaseDatos.Show();
        }

        private void DT(string pais)
        {
            decimal adj, asg, adj2, asg2;
            string conexion = "Data Source = DESKTOP-U983H9I; Initial Catalog = DB_C1; Integrated Security = True";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();
            string cadena = "select r.Destino, r.Transporte, COUNT(r.Transporte) as Cantidad, p.Adjudicado, p.Asignado from Rqms as r inner join Porcentaje as p on r.Pais like p.Pais where r.Pais like '"+ pais +"' group by r.Destino, r.Transporte, p.Adjudicado, p.Asignado";
            SqlDataAdapter adaprtador = new SqlDataAdapter(cadena, conn);
            DataSet tabla = new DataSet();
            adaprtador.Fill(tabla);

            int altura = 25;
            for (int i = 0; i < tabla.Tables[0].Rows.Count; i++)
            {
                adj = Convert.ToDecimal(tabla.Tables[0].Rows[i][3].ToString().Trim());
                asg = Convert.ToDecimal(tabla.Tables[0].Rows[i][4].ToString().Trim());
                adj2 = adj * 100;
                asg2 = asg * 100; 
                PanelResumen(tabla.Tables[0].Rows[i][0].ToString().Trim(), tabla.Tables[0].Rows[i][1].ToString().Trim(), tabla.Tables[0].Rows[i][2].ToString().Trim(), adj2, asg2, altura, 25);
                altura += 50;
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Paneles.Controls.Clear();
            Grafico(btn_pais.Text);
            DT(btn_pais.Text);
        }

        ArrayList ArrayDestino = new ArrayList();
        ArrayList Transporte = new ArrayList();
        ArrayList Cantidad = new ArrayList();
        SqlDataReader dr;
        private void Grafico(string pais)
        {
            string conexion = "Data Source = DESKTOP-U983H9I; Initial Catalog = DB_C1; Integrated Security = True";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();          
            string cadena = "select r.Destino, r.Transporte, COUNT(r.Transporte) as Cantidad, p.Adjudicado, p.Asignado from Rqms as r inner join Porcentaje as p on r.Pais like p.Pais where r.Pais like '" + pais + "' group by r.Destino, r.Transporte, p.Adjudicado, p.Asignado";
            SqlCommand cmd = new SqlCommand(cadena, conn);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Transporte.Add(dr.GetString(1));
                Cantidad.Add(dr.GetInt32(2));
     
            }
            chart1.Series[0].Points.DataBindXY(Transporte, Cantidad);
            dr.Close();
            conn.Close();
        }

        private void DataPct()
        {
            string conexion = "Data Source = DESKTOP-U983H9I; Initial Catalog = DB_C1; Integrated Security = True";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();
            string cadena = "select * from Porcentaje";
            SqlDataAdapter adaprtador = new SqlDataAdapter(cadena, conn);
            DataTable tabla = new DataTable();
            adaprtador.Fill(tabla);
            bunifuCustomDataGrid1.DataSource = tabla;
            conn.Close();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            IngresarPct();
            DataPct();
        }

        private void IngresarPct()
        {
            string conexion = "Data Source = DESKTOP-U983H9I; Initial Catalog = DB_C1; Integrated Security = True";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();
            string cadena = "insert into Porcentaje (Num, Pais, Destino, Transporte, Adjudicado, Asignado) values ('"+ txt_codigo.Text.Trim() +"','"+ txt_pais.Text.Trim() +"','"+ txt_destino.Text.Trim() + "'," +
                "'"+ txt_trans.Text.Trim() + "','"+ Convert.ToDecimal(txt_adj.Text.Trim()) + "','"+ Convert.ToDecimal(txt_asg.Text.Trim()) + "')";
            SqlCommand cmd = new SqlCommand(cadena, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void EliminarPct()
        {
            string conexion = "Data Source = DESKTOP-U983H9I; Initial Catalog = DB_C1; Integrated Security = True";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();
            string cadena = "delete from Porcentaje where Num='" + txt_codigo.Text.Trim() + "'";
            SqlCommand cmd = new SqlCommand(cadena, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            EliminarPct();
            DataPct();
        }

        private void buscarPct()
        {
            string conexion = "Data Source = DESKTOP-U983H9I; Initial Catalog = DB_C1; Integrated Security = True";
            SqlConnection conn = new SqlConnection(conexion);
            conn.Open();
            string cadena = "select * from Porcentaje where Num='" + txt_codigo.Text.Trim() + "'";
            SqlDataAdapter adaprtador = new SqlDataAdapter(cadena, conn);
            DataSet tabla = new DataSet();
            adaprtador.Fill(tabla);

            txt_pais.Text = tabla.Tables[0].Rows[0][1].ToString().Trim();
            txt_destino.Text = tabla.Tables[0].Rows[0][2].ToString().Trim();
            txt_trans.Text = tabla.Tables[0].Rows[0][3].ToString().Trim();
            txt_adj.Text = tabla.Tables[0].Rows[0][4].ToString().Trim();
            txt_asg.Text = tabla.Tables[0].Rows[0][5].ToString().Trim();
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            buscarPct();
        }
    }
}
