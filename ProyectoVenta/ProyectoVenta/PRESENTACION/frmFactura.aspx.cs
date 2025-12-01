using System;
using System.Data;
using System.Web.UI;
using ProyectoVenta.NEGOCIO;

namespace ProyectoVenta.PRESENTACION
{
    public partial class frmFactura : System.Web.UI.Page
    {
        Venta objVenta = new Venta();
        DetalleVenta objDetalle = new DetalleVenta();
        Cliente objCliente = new Cliente();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int idVenta = Convert.ToInt32(Request.QueryString["id"]);
                    cargarFactura(idVenta);
                }
                else
                {
                    Response.Redirect("frmListadoVentas.aspx");
                }
            }
        }

        private void cargarFactura(int idVenta)
        {
            try
            {
                DataTable dtVenta = objVenta.listar();
                DataRow[] ventas = dtVenta.Select("id_venta = " + idVenta);

                if (ventas.Length > 0)
                {
                    DataRow venta = ventas[0];

                    lblNumeroVenta.Text = venta["numero_venta"].ToString();
                    lblFecha.Text = Convert.ToDateTime(venta["fecha_venta"]).ToString("dd/MM/yyyy HH:mm");
                    lblEstado.Text = venta["estado_venta"].ToString();
                    lblMetodoPago.Text = venta["metodo_pago"].ToString();

                    int idCliente = Convert.ToInt32(venta["id_cliente"]);
                    DataTable dtCliente = objCliente.Buscar("");
                    DataRow[] clientes = dtCliente.Select("id_cliente = " + idCliente);
                    if (clientes.Length > 0)
                    {
                        lblCliente.Text = clientes[0]["nombre"].ToString() + " " + clientes[0]["apellido"].ToString();
                        lblTelefono.Text = clientes[0]["telefono"].ToString();
                    }

                    lblDireccion.Text = venta["direccion_entrega"].ToString();
                    lblSubtotal.Text = "Bs. " + Convert.ToDecimal(venta["subtotal"]).ToString("N2");
                    lblTotal.Text = "Bs. " + Convert.ToDecimal(venta["total"]).ToString("N2");

                    string observaciones = venta["observaciones"].ToString();
                    if (!string.IsNullOrEmpty(observaciones))
                    {
                        if (observaciones.Contains("PayPal Order ID:"))
                        {
                            int startIndex = observaciones.IndexOf("PayPal Order ID:") + 16;
                            string paypalID = observaciones.Substring(startIndex).Trim();
                            lblPayPalID.Text = paypalID;
                            pnlPayPal.Visible = true;

                            string obsLimpia = observaciones.Substring(0, observaciones.IndexOf("|")).Trim();
                            if (!string.IsNullOrEmpty(obsLimpia))
                            {
                                lblObservaciones.Text = obsLimpia;
                                pnlObservaciones.Visible = true;
                            }
                        }
                        else
                        {
                            lblObservaciones.Text = observaciones;
                            pnlObservaciones.Visible = true;
                        }
                    }

                    DataTable dtDetalle = objDetalle.listarPorVenta(idVenta);
                    gvDetalle.DataSource = dtDetalle;
                    gvDetalle.DataBind();
                }
                else
                {
                    Response.Redirect("frmListadoVentas.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al cargar factura: " + ex.Message + "');</script>");
            }
        }

        protected string GetEstadoBadge()
        {
            string estado = lblEstado.Text.ToLower();
            switch (estado)
            {
                case "pendiente":
                    return "warning";
                case "en proceso":
                    return "info";
                case "completado":
                case "entregado":
                    return "success";
                case "cancelado":
                    return "danger";
                default:
                    return "secondary";
            }
        }
    }
}
