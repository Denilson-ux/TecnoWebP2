using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoVenta.NEGOCIO;

namespace ProyectoVenta.PRESENTACION
{
    public partial class frmProducto : System.Web.UI.Page
    {
        Producto objProducto = new Producto();
        Tipo objTipo = new Tipo();
        // El id_producto se maneja por ViewState para persistencia.
        public int IdProducto
        {
            get { return ViewState["id_producto"] != null ? (int)ViewState["id_producto"] : 0; }
            set { ViewState["id_producto"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarTipos();
                listarProductos();
            }
        }

        private void cargarTipos()
        {
            try
            {
                DataTable dt = objTipo.Buscar("");
                ddlTipo.DataSource = dt;
                ddlTipo.DataTextField = "nombre";
                ddlTipo.DataValueField = "id_tipo";
                ddlTipo.DataBind();
                ddlTipo.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al cargar tipos: " + ex.Message, "danger");
            }
        }

        private void listarProductos()
        {
            try
            {
                objProducto.Descripcion = "";
                DataTable dt = objProducto.Buscar("");
                gvProductos.DataSource = dt;
                gvProductos.DataBind();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al listar productos: " + ex.Message, "danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validarCampos())
                {
                    decimal precio;
                    if (!decimal.TryParse(txtPrecio.Text, out precio))
                    {
                        mostrarMensaje("Debe ingresar un precio válido", "warning");
                        return;
                    }

                    objProducto.CodigoProducto = txtCodigo.Text.Trim();
                    objProducto.Descripcion = txtNombre.Text.Trim();
                    objProducto.PrecioBase = precio;
                    objProducto.Stock = int.Parse(txtStock.Text);
                    objProducto.IdTipo = int.Parse(ddlTipo.SelectedValue);

                    if (objProducto.Guardar())
                    {
                        mostrarMensaje("Producto guardado correctamente", "success");
                        limpiarCampos();
                        listarProductos();
                    }
                    else
                    {
                        mostrarMensaje("Error al guardar el producto", "danger");
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
                if (IdProducto == 0)
                {
                    mostrarMensaje("Debe seleccionar un producto", "warning");
                    return;
                }

                if (validarCampos())
                {
                    decimal precio;
                    if (!decimal.TryParse(txtPrecio.Text, out precio))
                    {
                        mostrarMensaje("Debe ingresar un precio válido", "warning");
                        return;
                    }

                    objProducto.Idproducto = IdProducto;
                    objProducto.CodigoProducto = txtCodigo.Text.Trim();
                    objProducto.Descripcion = txtNombre.Text.Trim();
                    objProducto.PrecioBase = precio;
                    objProducto.Stock = int.Parse(txtStock.Text);
                    objProducto.IdTipo = int.Parse(ddlTipo.SelectedValue);

                    if (objProducto.Modificar())
                    {
                        mostrarMensaje("Producto modificado correctamente", "success");
                        limpiarCampos();
                        listarProductos();
                    }
                    else
                    {
                        mostrarMensaje("Error al modificar el producto", "danger");
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
                if (IdProducto == 0)
                {
                    mostrarMensaje("Debe seleccionar un producto", "warning");
                    return;
                }

                objProducto.Idproducto = IdProducto;
                if (objProducto.Eliminar())
                {
                    mostrarMensaje("Producto eliminado correctamente", "success");
                    limpiarCampos();
                    listarProductos();
                }
                else
                {
                    mostrarMensaje("Error al eliminar el producto", "danger");
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al eliminar: " + ex.Message, "danger");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                objProducto.Descripcion = txtBuscar.Text.Trim();
                DataTable dt = objProducto.Buscar(txtBuscar.Text.Trim());
                gvProductos.DataSource = dt;
                gvProductos.DataBind();
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

        protected void gvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvProductos.SelectedRow;
                IdProducto = Convert.ToInt32(gvProductos.DataKeys[row.RowIndex].Value);

                DataTable dt = objProducto.Buscar("");
                DataRow[] rows = dt.Select("id_producto = " + IdProducto);

                if (rows.Length > 0)
                {
                    DataRow dataRow = rows[0];
                    txtCodigo.Text = dataRow["codigo_producto"].ToString();
                    txtNombre.Text = dataRow.Table.Columns.Contains("nombre_producto") ? dataRow["nombre_producto"].ToString() : dataRow["descripcion"].ToString();
                    txtStock.Text = dataRow["stock"].ToString();

                    if (dataRow.Table.Columns.Contains("precio_base") && dataRow["precio_base"] != DBNull.Value)
                    {
                        decimal precio = Convert.ToDecimal(dataRow["precio_base"]);
                        txtPrecio.Text = precio.ToString("0.00");
                    }
                    else
                    {
                        txtPrecio.Text = "";
                    }

                    string nombreTipo = dataRow.Table.Columns.Contains("Nombre") ? dataRow["Nombre"].ToString().Trim() : "";
                    string tipoNormalizado = nombreTipo.ToLowerInvariant();
                    ListItem itemTipo = null;
                    foreach (ListItem item in ddlTipo.Items)
                    {
                        if (item.Text.Trim().ToLowerInvariant() == tipoNormalizado)
                        {
                            itemTipo = item;
                            break;
                        }
                    }
                    if (itemTipo != null)
                    {
                        ddlTipo.SelectedValue = itemTipo.Value;
                    }
                    else
                    {
                        ddlTipo.SelectedIndex = 0;
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
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                mostrarMensaje("Debe ingresar el código del producto", "warning");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                mostrarMensaje("Debe ingresar el nombre del producto", "warning");
                return false;
            }

            decimal precio;
            if (!decimal.TryParse(txtPrecio.Text, out precio) || precio <= 0)
            {
                mostrarMensaje("Debe ingresar un precio válido", "warning");
                return false;
            }

            if (ddlTipo.SelectedValue == "0")
            {
                mostrarMensaje("Debe seleccionar un tipo", "warning");
                return false;
            }
            return true;
        }   

        private void limpiarCampos()
        {
            IdProducto = 0;
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "0";
            ddlTipo.SelectedIndex = 0;
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
