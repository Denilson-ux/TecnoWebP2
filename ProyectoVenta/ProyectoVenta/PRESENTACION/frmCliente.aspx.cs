using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoVenta.NEGOCIO;

namespace ProyectoVenta.PRESENTACION
{
    public partial class frmCliente : System.Web.UI.Page
    {
        Cliente objCliente = new Cliente();
        int id_cliente = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                listarClientes();
            }
        }

        private void listarClientes()
        {
            try
            {
                objCliente.Nombre = "";
                DataTable dt = objCliente.buscar();   // usa buscar() sin parámetros
                gvClientes.DataSource = dt;
                gvClientes.DataBind();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al listar clientes: " + ex.Message, "danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validarCampos())
                {
                    objCliente.Nombre = txtNombre.Text.Trim();
                    objCliente.Apellidos = txtApellidos.Text.Trim();
                    objCliente.Telefono = txtTelefono.Text.Trim();
                    objCliente.Email = txtEmail.Text.Trim();
                    objCliente.Direccion = txtDireccion.Text.Trim();
                    objCliente.Referencia = txtReferencia.Text.Trim();
                    objCliente.Zona = ddlZona.SelectedValue;

                    if (objCliente.guardar())
                    {
                        mostrarMensaje("Cliente guardado correctamente", "success");
                        limpiarCampos();
                        listarClientes();
                    }
                    else
                    {
                        mostrarMensaje("Error al guardar el cliente", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error: " + ex.Message, "danger");
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (id_cliente == 0)
                {
                    mostrarMensaje("Debe seleccionar un cliente", "warning");
                    return;
                }

                if (validarCampos())
                {
                    objCliente.Idcliente = id_cliente;
                    objCliente.Nombre = txtNombre.Text.Trim();
                    objCliente.Apellidos = txtApellidos.Text.Trim();
                    objCliente.Telefono = txtTelefono.Text.Trim();
                    objCliente.Email = txtEmail.Text.Trim();
                    objCliente.Direccion = txtDireccion.Text.Trim();
                    objCliente.Referencia = txtReferencia.Text.Trim();
                    objCliente.Zona = ddlZona.SelectedValue;

                    if (objCliente.modificar())
                    {
                        mostrarMensaje("Cliente modificado correctamente", "success");
                        limpiarCampos();
                        listarClientes();
                    }
                    else
                    {
                        mostrarMensaje("Error al modificar el cliente", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error: " + ex.Message, "danger");
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (id_cliente == 0)
                {
                    mostrarMensaje("Debe seleccionar un cliente", "warning");
                    return;
                }

                objCliente.Idcliente = id_cliente;
                if (objCliente.eliminar())
                {
                    mostrarMensaje("Cliente eliminado correctamente", "success");
                    limpiarCampos();
                    listarClientes();
                }
                else
                {
                    mostrarMensaje("Error al eliminar el cliente", "danger");
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error: " + ex.Message, "danger");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                objCliente.Nombre = txtBuscar.Text.Trim();
                DataTable dt = objCliente.buscar();   // usa buscar() que lee objCliente.Nombre
                gvClientes.DataSource = dt;
                gvClientes.DataBind();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al buscar: " + ex.Message, "danger");
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        protected void gvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvClientes.SelectedRow;

                // ID real desde DataKeys (configura DataKeyNames="id_cliente" en el GridView)
                id_cliente = Convert.ToInt32(gvClientes.DataKeys[gvClientes.SelectedIndex].Value);

                txtNombre.Text = row.Cells[1].Text;
                txtApellidos.Text = row.Cells[2].Text;
                txtTelefono.Text = row.Cells[3].Text;
                txtEmail.Text = row.Cells[4].Text;
                ddlZona.SelectedValue = row.Cells[5].Text;
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al seleccionar: " + ex.Message, "danger");
            }
        }

        private bool validarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                mostrarMensaje("Debe ingresar el nombre", "warning");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                mostrarMensaje("Debe ingresar los apellidos", "warning");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                mostrarMensaje("Debe ingresar el teléfono", "warning");
                return false;
            }
            return true;
        }

        private void limpiarCampos()
        {
            id_cliente = 0;
            txtNombre.Text = "";
            txtApellidos.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtDireccion.Text = "";
            txtReferencia.Text = "";
            ddlZona.SelectedIndex = 0;
            lblMensaje.Visible = false;
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = "alert alert-" + tipo + " mt-3";
            lblMensaje.Visible = true;
        }
    }
}
