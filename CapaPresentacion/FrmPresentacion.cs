﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
namespace CapaPresentacion
{
    public partial class FrmPresentacion : Form
    {
        private bool IsNuevo = false;
        private bool IsEditar = false;
        public FrmPresentacion()
        {
            InitializeComponent();
            ttMensaje.SetToolTip(txtNombre, "Ingrese el nombre de la Presentacion");
        }
        private void FrmPresentacion_Load(object sender, EventArgs e)
        {
            this.Top = 0; this.Left = 0;
            Mostrar();
            Habilitar(false);
            Botones();
        }

        private void MensajeOk(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void MensajeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Limpiar()
        {
            this.txtNombre.Text = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.txtIdpresentacion.Text = string.Empty;
            this.txtBuscar.Text = string.Empty;
        }
        private void Habilitar(bool valor)
        {
            this.txtNombre.ReadOnly = !valor;
            this.txtDescripcion.ReadOnly = !valor;

        }
        private void Botones()
        {
            if (this.IsNuevo || this.IsEditar)
            {
                Habilitar(true);
                this.btnGuardar.Enabled = true;
                this.btnNuevo.Enabled = false;
                this.btnEditar.Enabled = false;
                this.btnCancelar.Enabled = true;

            }
            else
            {
                Habilitar(false);
                this.btnGuardar.Enabled = false;
                this.btnNuevo.Enabled = true;
                this.btnEditar.Enabled = true;
                this.btnCancelar.Enabled = false;
            }

        }
        private void OcultarColumnas()
        {
            this.dataListado.Columns[0].Visible = false;
            this.dataListado.Columns[1].Visible = false;
        }
        private void Mostrar()
        {
            this.dataListado.DataSource = NPresentacion.Mostrar();
            this.OcultarColumnas();
            this.lblTotal.Text = "Total de Registros " + dataListado.Rows.Count.ToString();
        }
        private void BuscarNombre()
        {
            this.dataListado.DataSource = NCategoria.BuscarNombre(this.txtBuscar.Text);
            this.OcultarColumnas();
            this.lblTotal.Text = "Total de Registros " + dataListado.Rows.Count.ToString();
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            IsNuevo = true;
            Limpiar();
            IsEditar = false;
            Botones(); Habilitar(true);
            txtNombre.Focus();
        }

        
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Validating(object sender, CancelEventArgs e)
        {

        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            BuscarNombre();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string rpta = "";
                if (txtNombre.Text == string.Empty)
                {
                    MensajeError("Faltan ingresar datos");
                    errorIcono.SetError(txtNombre, "Ingrese nombre de la Categoria");
                }
                else
                {
                    if (IsNuevo)
                    {
                        if (NPresentacion.Coincidencia(txtNombre.Text.ToUpper()))
                        {
                            MensajeError("Este registro ya existe");
                            Limpiar();
                        }
                        else
                        {
                            rpta = NPresentacion.Insertar(txtNombre.Text.Trim().ToUpper(), txtDescripcion.Text.Trim());
                            MensajeOk(rpta);
                        }
                    }
                    else
                    {
                        rpta = NPresentacion.Editar(int.Parse(txtIdpresentacion.Text), txtNombre.Text.Trim().ToUpper(), txtDescripcion.Text.Trim());
                        MensajeOk(rpta);
                    }



                    IsNuevo = false;
                    IsEditar = false;
                    Limpiar(); Botones(); Mostrar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (txtIdpresentacion.Text != "")
            {
                IsNuevo = false;
                IsEditar = true;
                Botones(); Habilitar(true);
                txtNombre.Focus();
            }
            else
            { MensajeError("Debe seleccionar primero un categoria del listado"); }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            IsNuevo = false;
            Limpiar();
            IsEditar = false;
            Botones(); Habilitar(false);
            txtNombre.Focus();
        }

        private void dataListado_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataListado.Columns[0].Visible == true)
            {
                object temp = dataListado.SelectedCells[e.ColumnIndex].Value;
                if (Convert.ToBoolean(temp))
                {
                    dataListado.SelectedCells[e.ColumnIndex].Value = false;
                }
                else
                    dataListado.SelectedCells[e.ColumnIndex].Value = true;
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked)
            {
                checkBox1.Checked = false;
                try
                {
                    DialogResult temp;
                    temp = MessageBox.Show("Eta seguro que desea eliminar el registro", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (temp == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (DataGridViewRow data in dataListado.Rows)
                        {
                            if (Convert.ToBoolean(data.Cells[0].Value))
                            {
                                string rpta = NPresentacion.Eliminar(Convert.ToInt32(data.Cells[1].Value));
                                Mostrar();
                                MensajeOk(rpta);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MensajeError("Debe seleccionar algun elemento de la lista que desee eliminar");
        
        }

        private void txtBuscar_TextChanged_1(object sender, EventArgs e)
        {
            this.BuscarNombre();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            { dataListado.Columns[0].Visible = true; }
            else { dataListado.Columns[0].Visible = false; }
        }

        private void dataListado_DoubleClick(object sender, EventArgs e)
        {
            this.txtIdpresentacion.Text = dataListado.CurrentRow.Cells["idpresentacion"].Value.ToString();
            this.txtNombre.Text = dataListado.CurrentRow.Cells["nombre"].Value.ToString();
            this.txtDescripcion.Text = dataListado.CurrentRow.Cells["descripcion"].Value.ToString();
            tabControl1.SelectedIndex = 1;
        }
    }

}

