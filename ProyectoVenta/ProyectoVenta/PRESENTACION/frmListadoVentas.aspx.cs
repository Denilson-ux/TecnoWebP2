using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoVenta.NEGOCIO;

namespace ProyectoVenta.PRESENTACION
{
    public partial class frmListadoVentas : System.Web.UI.Page
    {
        Venta objVenta = new Venta();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtFechaHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");
                cargarVentas();
            }
        }

        private void cargarVentas()
        {
            try
            {
                DataTable dt = objVenta.listar();
                
                if (!string.IsNullOrEmpty(txtBuscarNumero.Text))
                {
                    dt = dt.Select("numero_venta LIKE '%" + txtBuscarNumero.Text + "%'").CopyToDataTable();
                }

                if (!string.IsNullOrEmpty(txtFechaDesde.Text) && !string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    DateTime desde = DateTime.Parse(txtFechaDesde.Text);
                    DateTime hasta = DateTime.Parse(txtFechaHasta.Text).AddDays(1);
                    dt = dt.Select("fecha_venta >= #" + desde.ToString("MM/dd/yyyy") + "# AND fecha_venta < #" + hasta.ToString("MM/dd/yyyy") + "#").CopyToDataTable();
                }

                gvVentas.DataSource = dt;
                gvVentas.DataBind();

                if (dt.Rows.Count > 0)
                {
                    decimal totalGeneral = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        totalGeneral += Convert.ToDecimal(row["total"]);
                    }
                    lblTotal.Text = $"Total de {dt.Rows.Count} ventas: Bs. {totalGeneral:N2}";
                }
                else
                {
                    lblTotal.Text = "No hay ventas en el rango seleccionado.";
                }
            }
            catch (Exception ex)
            {
                lblTotal.Text = "Error al cargar ventas: " + ex.Message;
                lblTotal.CssClass = "text-danger fw-bold";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarVentas();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarNumero.Text = "";
            txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtFechaHasta.Text = DateTime.Now.ToString("yyyy-MM-dd");
            cargarVentas();
        }

        protected void gvVentas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                string estado = rowView["estado_venta"].ToString();

                switch (estado.ToLower())
                {
                    case "pendiente":
                        e.Row.CssClass = "estado-pendiente";
                        break;
                    case "en proceso":
                        e.Row.CssClass = "estado-proceso";
                        break;
                    case "completado":
                    case "entregado":
                        e.Row.CssClass = "estado-completado";
                        break;
                    case "cancelado":
                        e.Row.CssClass = "estado-cancelado";
                        break;
                }
            }
        }

        protected string GetEstadoColor(string estado)
        {
            switch (estado.ToLower())
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
