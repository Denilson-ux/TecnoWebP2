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
        
        // Usar ViewState para persistir el ID entre postbacks
        public int IdCliente
        {
            get { return ViewState["id_cliente"] != null ? (int)ViewState["id_cliente"] : 0; }
            set { ViewState["id_cliente"] = value; }
        }

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
                DataTable dt = objCliente.buscar();
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
                if (IdCliente == 0)
                {
                    mostrarMensaje("Debe seleccionar un cliente", "warning");
                    return;
                }

                if (validarCampos())
                {
                    objCliente.Idcliente = IdCliente;
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
                if (IdCliente == 0)
                {
                    mostrarMensaje("Debe seleccionar un cliente", "warning");
                    return;
                }

                objCliente.Idcliente = IdCliente;
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
                DataTable dt = objCliente.buscar();
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
            listarClientes();
        }

        protected void gvClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvClientes.SelectedRow;
                IdCliente = Convert.ToInt32(gvClientes.DataKeys[gvClientes.SelectedIndex].Value);

                // Obtener datos directamente del DataTable para evitar problemas con HTML
                DataTable dt = objCliente.buscar();
                DataRow[] rows = dt.Select("id_cliente = " + IdCliente);

                if (rows.Length > 0)
                {
                    DataRow dataRow = rows[0];
                    txtNombre.Text = dataRow["nombre"].ToString();
                    txtApellidos.Text = dataRow["apellido"].ToString();
                    txtTelefono.Text = dataRow["telefono"].ToString();
                    txtEmail.Text = dataRow["email"] != DBNull.Value ? dataRow["email"].ToString() : "";
                    txtDireccion.Text = dataRow["direccion"] != DBNull.Value ? dataRow["direccion"].ToString() : "";
                    txtReferencia.Text = dataRow["referencia"] != DBNull.Value ? dataRow["referencia"].ToString() : "";
                    
                    string zona = dataRow["zona"] != DBNull.Value ? dataRow["zona"].ToString() : "";
                    if (!string.IsNullOrWhiteSpace(zona))
                    {
                        ListItem item = ddlZona.Items.FindByValue(zona);
                        if (item != null)
                        {
                            ddlZona.SelectedValue = zona;
                        }
                        else
                        {
                            ddlZona.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ddlZona.SelectedIndex = 0;
                    }
                }
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
            IdCliente = 0;
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
