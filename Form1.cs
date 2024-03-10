using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secuencialesindexados
{
    public partial class Form1 : Form
    {
        private const string indiceFileName = "index.txt";
        private const string datosFileName = "datos.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter swDatos = new StreamWriter(datosFileName, true))
                using (StreamWriter swIndice = new StreamWriter(indiceFileName, true))
                {
                    long posicion = swDatos.BaseStream.Length; // Obtiene la posición del próximo registro en el archivo de datos

                    string dato = txtDato.Text;
                    swDatos.WriteLine(dato);

                    string indice = $"{posicion},{dato}"; // Formato del índice: posición,dato
                    swIndice.WriteLine(indice);
                }

                MessageBox.Show("Dato agregado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDato.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el dato: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string datoABuscar = txtBuscar.Text;

                using (StreamReader sr = new StreamReader(indiceFileName))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] partes = linea.Split(',');
                        if (partes.Length == 2 && partes[1] == datoABuscar)
                        {
                            long posicion = long.Parse(partes[0]);
                            using (StreamReader srDatos = new StreamReader(datosFileName))
                            {
                                srDatos.BaseStream.Seek(posicion, SeekOrigin.Begin);
                                string datoEncontrado = srDatos.ReadLine();
                                MessageBox.Show($"El dato '{datoABuscar}' se encontró en la posición {posicion}: {datoEncontrado}", "Resultado de la búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }

                MessageBox.Show($"El dato '{datoABuscar}' no se encontró.", "Resultado de la búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar el dato: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
