using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoVenta.NEGOCIO;
using System.Web;

namespace ProyectoVenta.PRESENTACION
{
    public partial class frmVenta : System.Web.UI.Page
    {
        Venta objVenta = new Venta();
        Cliente objCliente = new Cliente();
        Producto objProducto = new Producto();
        Tipo objTipo = new Tipo();
        DetalleVenta objDetalle = new DetalleVenta();

        List<ItemDetalle> listaDetalles = new List<ItemDetalle>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                inicializarFormulario();
            }
            else
            {
                if (ViewState["Detalles"] != null)
                    listaDetalles = (List<ItemDetalle>)ViewState["Detalles"];
            }
        }

        private void inicializarFormulario()
        {
            txtNumeroVenta.Text = objVenta.generarNumeroVenta();
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            cargarProductos();
            cargarTipo();
            listaDetalles = new List<ItemDetalle>();
            ViewState["Detalles"] = listaDetalles;
        }

        private void cargarProductos()
        {
            Producto objProducto = new Producto();
            DataTable dt = objProducto.Buscar("");
            ddlProducto.DataSource = dt;
            ddlProducto.DataTextField = "nombre_producto";
            ddlProducto.DataValueField = "id_producto";
            ddlProducto.DataBind();
            ddlProducto.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
        }

        private void cargarTipo()
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

        protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProducto.SelectedValue != "0")
            {
                try
                {
                    DataTable dt = objProducto.Buscar("");
                    DataRow[] rows = dt.Select("id_producto = " + ddlProducto.SelectedValue);
                    if (rows.Length > 0)
                    {
                        decimal precio = Convert.ToDecimal(rows[0]["precio_base"]);
                        txtPrecioUnit.Text = precio.ToString("0.00");
                        divTipo.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    mostrarMensaje("Error: " + ex.Message, "danger");
                }
            }
            else
            {
                divTipo.Visible = false;
                txtPrecioUnit.Text = "0.00";
            }
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProducto.SelectedValue != "0" && ddlTipo.SelectedValue != "0")
            {
                try
                {
                    DataTable dtProducto = objProducto.Buscar("");
                    DataRow[] rowsProducto = dtProducto.Select("id_producto = " + ddlProducto.SelectedValue);
                    DataTable dtTipos = objTipo.Buscar("");
                    DataRow[] rowsTipo = dtTipos.Select("id_tipo = " + ddlTipo.SelectedValue);
                    if (rowsProducto.Length > 0 && rowsTipo.Length > 0)
                    {
                        decimal precioBase = Convert.ToDecimal(rowsProducto[0]["precio_base"]);
                        decimal multiplicador = 1;
                        if (rowsTipo[0].Table.Columns.Contains("multiplicador_precio"))
                            multiplicador = Convert.ToDecimal(rowsTipo[0]["multiplicador_precio"]);
                        decimal precioFinal = precioBase * multiplicador;
                        txtPrecioUnit.Text = precioFinal.ToString("0.00");
                    }
                }
                catch (Exception ex)
                {
                    mostrarMensaje("Error: " + ex.Message, "danger");
                }
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (validarDetalle())
                {
                    // ========== VALIDACIÓN DE STOCK ==========
                    int idProducto = int.Parse(ddlProducto.SelectedValue);
                    int cantidadSolicitada = int.Parse(txtCantidad.Text);

                    // Obtener el stock actual del producto desde la BD
                    DataTable dtProducto = objProducto.Buscar("");
                    DataRow[] rowsProducto = dtProducto.Select("id_producto = " + idProducto);

                    if (rowsProducto.Length > 0)
                    {
                        int stockDisponible = Convert.ToInt32(rowsProducto[0]["stock"]);

                        // Calcular cuánto ya se agregó en el carrito
                        int cantidadEnCarrito = 0;
                        foreach (var detalle in listaDetalles)
                        {
                            if (detalle.IdProducto == idProducto)
                                cantidadEnCarrito += detalle.Cantidad;
                        }

                        // Validar si hay suficiente stock
                        int stockRestante = stockDisponible - cantidadEnCarrito;

                        if (cantidadSolicitada > stockRestante)
                        {
                            mostrarMensaje($"Stock insuficiente. Disponible: {stockRestante} unidades", "danger");
                            return; // Detiene la ejecución y NO agrega al carrito
                        }
                    }
                    // ========== FIN VALIDACIÓN DE STOCK ==========

                    ItemDetalle item = new ItemDetalle
                    {
                        IdProducto = int.Parse(ddlProducto.SelectedValue),
                        NombreProducto = ddlProducto.SelectedItem.Text,
                        Cantidad = int.Parse(txtCantidad.Text),
                        PrecioUnitario = decimal.Parse(txtPrecioUnit.Text),
                        NotasEspeciales = txtNotas.Text.Trim()
                    };

                    if (divTipo.Visible && ddlTipo.SelectedValue != "0")
                    {
                        item.IdTamanio = int.Parse(ddlTipo.SelectedValue);
                        item.NombreTamanio = ddlTipo.SelectedItem.Text;
                    }
                    else
                    {
                        item.IdTamanio = 0;
                        item.NombreTamanio = "-";
                    }

                    item.Subtotal = item.PrecioUnitario * item.Cantidad;
                    listaDetalles.Add(item);
                    ViewState["Detalles"] = listaDetalles;
                    actualizarGridDetalle();
                    limpiarDetalle();
                    calcularTotales(sender, e);
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al agregar: " + ex.Message, "danger");
            }
        }


        protected void gvDetalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                listaDetalles.RemoveAt(e.RowIndex);
                ViewState["Detalles"] = listaDetalles;
                actualizarGridDetalle();
                calcularTotales(sender, e);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al eliminar: " + ex.Message, "danger");
            }
        }

        private void cargarClientes(string filtro)
        {
            try
            {
                DataTable dt = objCliente.Buscar(filtro);
                
                if (!dt.Columns.Contains("nombre_completo"))
                {
                    dt.Columns.Add("nombre_completo", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        string nombre = row["nombre"].ToString();
                        string apellido = row["apellido"].ToString();
                        row["nombre_completo"] = nombre + " " + apellido;
                    }
                }
                
                gvClientes.DataSource = dt;
                gvClientes.DataBind();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al cargar clientes: " + ex.Message, "danger");
            }
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                txtBuscarCliente.Text = "";
                cargarClientes("");
                ScriptManager.RegisterStartupScript(this, GetType(), "MostrarClientes", "mostrarModalClientes();", true);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al cargar clientes: " + ex.Message, "danger");
            }
        }

        protected void btnFiltrarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtBuscarCliente.Text.Trim();
                cargarClientes(filtro);
                ScriptManager.RegisterStartupScript(this, GetType(), "MostrarClientesFiltro", "mostrarModalClientes();", true);
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al filtrar clientes: " + ex.Message, "danger");
            }
        }

        protected void gvClientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SeleccionarCliente")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvClientes.Rows[index];
                string idCliente = row.Cells[0].Text;
                string nombreCodificado = row.Cells[1].Text;
                string nombre = HttpUtility.HtmlDecode(nombreCodificado);
                hfIdCliente.Value = idCliente;
                txtCliente.Text = nombre;
                ScriptManager.RegisterStartupScript(this, GetType(), "CerrarClientes", "cerrarModalClientes();", true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (hfPagoAprobado.Value != "1")
            {
                mostrarMensaje("Debe completar el pago con PayPal antes de guardar el pedido.", "danger");
                return;
            }

            try
            {
                if (validarVenta())
                {
                    objVenta.NumeroVenta = txtNumeroVenta.Text;
                    objVenta.FechaVenta = DateTime.Parse(txtFecha.Text);
                    objVenta.IdCliente = int.Parse(hfIdCliente.Value);
                    objVenta.DireccionEntrega = txtDireccionEntrega.Text.Trim();
                    objVenta.MetodoPago = "PayPal";
                    objVenta.Observaciones = txtObservaciones.Text.Trim();
                    if (!string.IsNullOrEmpty(hfPayPalOrderID.Value))
                    {
                        objVenta.Observaciones += " | PayPal Order ID: " + hfPayPalOrderID.Value;
                    }

                    decimal subtotal = 0;
                    foreach (var item in listaDetalles)
                        subtotal += item.Subtotal;
                    objVenta.Subtotal = subtotal;
                    objVenta.Total = subtotal;
                    
                    // CAMBIO: Si el pago ya fue aprobado, el estado es "Completado" en lugar de "Pendiente"
                    objVenta.EstadoVenta = "Completado";

                    if (objVenta.guardar())
                    {
                        int idVenta = objVenta.obtenerUltimoId();
                        bool detallesOk = true;
                        foreach (var item in listaDetalles)
                        {
                            objDetalle.IdVenta = idVenta;
                            objDetalle.IdProducto = item.IdProducto;
                            objDetalle.IdTamanio = item.IdTamanio;
                            objDetalle.Cantidad = item.Cantidad;
                            objDetalle.PrecioUnitario = item.PrecioUnitario;
                            objDetalle.Subtotal = item.Subtotal;
                            objDetalle.NotasEspeciales = item.NotasEspeciales;

                            if (!objDetalle.guardar())
                            {
                                detallesOk = false;
                                break;
                            }
                        }

                        if (detallesOk)
                        {
                            string script = @"
                                Swal.fire({
                                    icon: 'success',
                                    title: '¡Venta Registrada!',
                                    html: 'Pedido Nro: " + txtNumeroVenta.Text + @"<br>Total: " + lblTotal.Text + @"<br><br>¿Desea ver la factura?',
                                    showCancelButton: true,
                                    confirmButtonText: 'Ver Factura',
                                    cancelButtonText: 'Nueva Venta'
                                }).then((result) => {
                                    if (result.isConfirmed) {
                                        window.location.href = 'frmFactura.aspx?id=" + idVenta + @"';
                                    } else {
                                        window.location.href = 'frmVenta.aspx';
                                    }
                                });
                            ";
                            ScriptManager.RegisterStartupScript(this, GetType(), "VentaGuardada", script, true);
                        }
                        else
                        {
                            mostrarMensaje("Error al guardar los detalles", "danger");
                        }
                    }
                    else
                    {
                        mostrarMensaje("Error al guardar la venta", "danger");
                    }
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error: " + ex.Message, "danger");
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarFormulario();
        }

        protected void calcularTotales(object sender, EventArgs e)
        {
            try
            {
                decimal subtotal = 0;
                foreach (var item in listaDetalles)
                    subtotal += item.Subtotal;
                lblSubtotal.Text = "Bs. " + subtotal.ToString("0.00");
                lblTotal.Text = lblSubtotal.Text;
            }
            catch { }
        }

        private void actualizarGridDetalle()
        {
            gvDetalle.DataSource = listaDetalles;
            gvDetalle.DataBind();
        }

        private bool validarDetalle()
        {
            if (ddlProducto.SelectedValue == "0")
            {
                mostrarMensaje("Debe seleccionar un producto", "warning");
                return false;
            }
            if (divTipo.Visible && ddlTipo.SelectedValue == "0")
            {
                mostrarMensaje("Debe seleccionar un tipo", "warning");
                return false;
            }
            if (string.IsNullOrEmpty(txtCantidad.Text) || int.Parse(txtCantidad.Text) <= 0)
            {
                mostrarMensaje("Debe ingresar una cantidad válida", "warning");
                return false;
            }
            if (string.IsNullOrEmpty(txtPrecioUnit.Text) || decimal.Parse(txtPrecioUnit.Text) <= 0)
            {
                mostrarMensaje("El precio no es válido", "warning");
                return false;
            }
            return true;
        }

        private bool validarVenta()
        {
            if (hfIdCliente.Value == "0")
            {
                mostrarMensaje("Debe seleccionar un cliente", "warning");
                return false;
            }
            if (listaDetalles.Count == 0)
            {
                mostrarMensaje("Debe agregar al menos un producto", "warning");
                return false;
            }
            return true;
        }

        private void limpiarDetalle()
        {
            ddlProducto.SelectedIndex = 0;
            ddlTipo.SelectedIndex = 0;
            txtCantidad.Text = "1";
            txtPrecioUnit.Text = "";
            txtNotas.Text = "";
            divTipo.Visible = false;
        }

        private void limpiarFormulario()
        {
            txtNumeroVenta.Text = objVenta.generarNumeroVenta();
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtCliente.Text = "";
            hfIdCliente.Value = "0";
            txtDireccionEntrega.Text = "";
            txtObservaciones.Text = "";
            listaDetalles.Clear();
            ViewState["Detalles"] = listaDetalles;
            actualizarGridDetalle();
            lblSubtotal.Text = "Bs. 0.00";
            lblTotal.Text = "Bs. 0.00";
            limpiarDetalle();
            lblMensaje.Visible = false;
            hfPagoAprobado.Value = "0";
            hfPayPalOrderID.Value = "";
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = "alert alert-" + tipo;
            lblMensaje.Visible = true;
        }

        [Serializable]
        public class ItemDetalle
        {
            public int IdProducto { get; set; }
            public string NombreProducto { get; set; }
            public int IdTamanio { get; set; }
            public string NombreTamanio { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Subtotal { get; set; }
            public string NotasEspeciales { get; set; }
        }
    }
}
