using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrestacionesLaborales
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

        private string Dias(DateTime FechaInicio, DateTime FechaFinal)
        {
            TimeSpan Dias =  FechaFinal - FechaInicio;
            int dy = Dias.Days;
            return dy.ToString();
        }

        private double Promedio(TextBox sueldo1, TextBox sueldo2, TextBox sueldo3,
            TextBox sueldo4, TextBox sueldo5, TextBox sueldo6)
        {
            double promedio = (Convert.ToDouble(sueldo1.Text) + Convert.ToDouble(sueldo2.Text) + Convert.ToDouble(sueldo3.Text) +
                Convert.ToDouble(sueldo4.Text) + Convert.ToDouble(sueldo5.Text) + Convert.ToDouble(sueldo6.Text))/6;
            return promedio;
        }

        private double SueldoXdia(double promedio)
        {
            double sueldoXdias = promedio / 365;
            return sueldoXdias;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBox9.Text = Dias(dateTimePicker1.Value,dateTimePicker2.Value);
            textBox8.Text = Promedio(textBox2, textBox3, textBox4, textBox5, textBox6, textBox7).ToString("N2");
            textBox10.Text = textBox9.Text;
            textBox11.Text = SueldoXdia(Convert.ToDouble(textBox8.Text)).ToString("N2");
            textBox12.Text = (Convert.ToDouble(textBox10.Text) * Convert.ToDouble(textBox11.Text)).ToString("N2");
            textBox15.Text = Dias(dateTimePicker3.Value, dateTimePicker2.Value);
            textBox14.Text = SueldoXdia(Convert.ToDouble(textBox8.Text)).ToString("N2");
            textBox13.Text = (Convert.ToDouble(textBox15.Text) * Convert.ToDouble(textBox14.Text)).ToString("N2");
            textBox18.Text = Dias(dateTimePicker4.Value, dateTimePicker2.Value);
            textBox17.Text = SueldoXdia(Convert.ToDouble(textBox8.Text)).ToString("N2");
            textBox16.Text = (Convert.ToDouble(textBox18.Text) * Convert.ToDouble(textBox17.Text)).ToString("N2");

            textBox19.Text = (Convert.ToDouble(textBox12.Text) + Convert.ToDouble(textBox13.Text) + Convert.ToDouble(textBox16.Text)).ToString("N2");
        }
    }
}
