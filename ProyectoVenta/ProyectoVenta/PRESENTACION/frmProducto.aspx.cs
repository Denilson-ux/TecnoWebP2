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
        int id_producto = 0;

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
                DataTable dt = objTipo.Buscar(""); // Busca todos los tipos
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
                DataTable dt = objProducto.Buscar(""); // Ajusta el método para permitir búsquedas si lo necesitas
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
                if (id_producto == 0)
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

                    objProducto.Idproducto = id_producto;
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
                if (id_producto == 0)
                {
                    mostrarMensaje("Debe seleccionar un producto", "warning");
                    return;
                }

                objProducto.Idproducto = id_producto;
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

                // ID desde DataKeys
                id_producto = Convert.ToInt32(gvProductos.DataKeys[row.RowIndex].Value);

                // Leer directamente desde las celdas del GridView y DECODIFICAR entidades HTML
                string codigo = HttpUtility.HtmlDecode(row.Cells[0].Text).Trim();
                string nombreProducto = HttpUtility.HtmlDecode(row.Cells[1].Text).Trim();
                string nombreTipo = HttpUtility.HtmlDecode(row.Cells[2].Text).Trim();
                string precioTexto = HttpUtility.HtmlDecode(row.Cells[3].Text).Trim();
                string stock = HttpUtility.HtmlDecode(row.Cells[4].Text).Trim();

                // Asignar valores a los campos
                txtCodigo.Text = codigo;
                txtNombre.Text = nombreProducto;
                txtStock.Text = stock;

                // Parseo robusto del precio - probar múltiples formatos
                decimal precioDecimal = 0;
                bool precioParseado = false;

                // Limpiar caracteres comunes
                string precioLimpio = precioTexto
                    .Replace("Bs.", "")
                    .Replace("Bs", "")
                    .Replace("$", "")
                    .Replace("&nbsp;", "")
                    .Replace(" ", "")
                    .Trim();

                // Intentar parsear con coma como decimal (formato español: 25,00)
                if (!precioParseado && precioLimpio.Contains(","))
                {
                    string precioConPunto = precioLimpio.Replace(".", "").Replace(",", ".");
                    precioParseado = decimal.TryParse(precioConPunto,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out precioDecimal);
                }

                // Intentar parsear con punto como decimal (formato inglés: 25.00)
                if (!precioParseado)
                {
                    precioParseado = decimal.TryParse(precioLimpio,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out precioDecimal);
                }

                // Asignar el precio parseado
                if (precioParseado && precioDecimal > 0)
                {
                    txtPrecio.Text = precioDecimal.ToString("0.00");
                }
                else
                {
                    txtPrecio.Text = "";
                }

                // Normalizar comparación del tipo (ignorar mayúsculas/minúsculas y espacios)
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
                    ddlTipo.SelectedIndex = 0; // Si no encuentra, seleccionar "-- Seleccione --"
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
            id_producto = 0;
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