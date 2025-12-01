using System;
using System.Data;
using System.Web.UI;
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
                cargarVentas();
            }
        }

        private void cargarVentas()
        {
            try
            {
                DataTable dt = objVenta.listar();
                
                // Validar si hay datos
                if (dt == null || dt.Rows.Count == 0)
                {
                    gvVentas.DataSource = null;
                    gvVentas.DataBind();
                    lblTotalGeneral.Text = "Bs. 0.00";
                    return;
                }
                
                // Filtrar por número si hay búsqueda
                if (!string.IsNullOrEmpty(txtBuscarNumero.Text))
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "numero_venta LIKE '%" + txtBuscarNumero.Text + "%'";
                    dt = dv.ToTable();
                }

                // Filtrar por fecha
                if (!string.IsNullOrEmpty(txtFechaDesde.Text) && !string.IsNullOrEmpty(txtFechaHasta.Text))
                {
                    DateTime fechaDesde = DateTime.Parse(txtFechaDesde.Text);
                    DateTime fechaHasta = DateTime.Parse(txtFechaHasta.Text).AddDays(1);
                    
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = string.Format("fecha_venta >= #{0}# AND fecha_venta < #{1}#",
                        fechaDesde.ToString("MM/dd/yyyy"),
                        fechaHasta.ToString("MM/dd/yyyy"));
                    dt = dv.ToTable();
                }

                gvVentas.DataSource = dt;
                gvVentas.DataBind();

                // Calcular total general
                decimal totalGeneral = 0;
                foreach (DataRow row in dt.Rows)
                {
                    totalGeneral += Convert.ToDecimal(row["total"]);
                }
                lblTotalGeneral.Text = "Bs. " + totalGeneral.ToString("N2");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al cargar ventas: " + ex.Message + "');</script>");
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarVentas();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscarNumero.Text = "";
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            cargarVentas();
        }

        protected string GetEstadoBadgeClass(string estado)
        {
            switch (estado.ToLower())
            {
                case "pendiente":
                    return "badge-pendiente";
                case "completado":
                case "entregado":
                    return "badge-completado";
                case "en proceso":
                    return "badge-proceso";
                case "cancelado":
                    return "badge-cancelado";
                default:
                    return "bg-secondary";
            }
        }
    }
}
