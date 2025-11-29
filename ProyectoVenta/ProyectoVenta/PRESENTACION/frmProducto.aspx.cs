using System;
using System.Data;
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

                // ID desde DataKeys (no desde celdas)
                int idTmp = Convert.ToInt32(gvProductos.DataKeys[row.RowIndex].Value);
                id_producto = idTmp;

                // Ahora las columnas cambian de índice porque ya no está el ID visible
                string c1 = row.Cells[0].Text; // Código
                string c2 = row.Cells[1].Text; // Producto
                string c3 = row.Cells[2].Text; // Tipo
                string c4 = row.Cells[3].Text; // Precio
                string c5 = row.Cells[4].Text; // Stock

                txtCodigo.Text = c1;
                txtNombre.Text = c2;

                string textoPrecio = c4;
                textoPrecio = textoPrecio.Replace("€", "")
                                         .Replace("Bs.", "")
                                         .Replace("Bs", "")
                                         .Trim();
                txtPrecio.Text = textoPrecio;

                txtStock.Text = c5;

                for (int i = 0; i < ddlTipo.Items.Count; i++)
                {
                    if (ddlTipo.Items[i].Text == c3)
                    {
                        ddlTipo.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al seleccionar (SelectedIndexChanged): " + ex.Message, "danger");
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
