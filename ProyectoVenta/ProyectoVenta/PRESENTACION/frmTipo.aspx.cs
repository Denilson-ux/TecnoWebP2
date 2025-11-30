using System;
using System.Data;
using System.Web;
using ProyectoVenta.NEGOCIO;

namespace ProyectoVenta.PRESENTACION
{
    public partial class frmTipo : System.Web.UI.Page
    {
        Tipo objTipo = new Tipo();

        // Usar ViewState para persistir el ID entre postbacks
        public int IdTipo
        {
            get { return ViewState["id_tipo"] != null ? (int)ViewState["id_tipo"] : 0; }
            set { ViewState["id_tipo"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTipos();
            }
        }

        private void CargarTipos()
        {
            try
            {
                DataTable dt = objTipo.Buscar("");
                gvTipo.DataSource = dt;
                gvTipo.DataBind();
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al cargar tipos: " + ex.Message, "danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTipo.Text))
                {
                    mostrarMensaje("Debe ingresar el nombre del tipo", "warning");
                    return;
                }

                objTipo.Nombre = txtTipo.Text.Trim();
                if (objTipo.Guardar())
                {
                    mostrarMensaje("Tipo guardado correctamente", "success");
                    limpiarCampos();
                    CargarTipos();
                }
                else
                {
                    mostrarMensaje("Error al guardar el tipo", "danger");
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error: " + ex.Message, "danger");
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarCampos();
            CargarTipos();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (IdTipo == 0)
                {
                    mostrarMensaje("Debe seleccionar un tipo", "warning");
                    return;
                }

                objTipo.IdTipo = IdTipo;
                if (objTipo.Eliminar())
                {
                    mostrarMensaje("Tipo eliminado correctamente", "success");
                    limpiarCampos();
                    CargarTipos();
                }
                else
                {
                    mostrarMensaje("Error al eliminar el tipo", "danger");
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al eliminar: " + ex.Message, "danger");
            }
        }

        protected void gvTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Obtener el ID desde DataKeys
                IdTipo = Convert.ToInt32(gvTipo.DataKeys[gvTipo.SelectedIndex].Value);

                // Obtener datos del DataTable para evitar problemas con HTML entities
                DataTable dt = objTipo.Buscar("");
                DataRow[] rows = dt.Select("id_tipo = " + IdTipo);

                if (rows.Length > 0)
                {
                    DataRow dataRow = rows[0];
                    // Decodificar HTML entities si existen
                    string nombre = dataRow["nombre"].ToString();
                    txtTipo.Text = HttpUtility.HtmlDecode(nombre);
                }
            }
            catch (Exception ex)
            {
                mostrarMensaje("Error al seleccionar: " + ex.Message, "danger");
            }
        }

        private void limpiarCampos()
        {
            IdTipo = 0;
            txtTipo.Text = "";
            lblMensaje.Visible = false;
            gvTipo.SelectedIndex = -1;
        }

        private void mostrarMensaje(string mensaje, string tipo)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = "alert alert-" + tipo + " mt-3";
            lblMensaje.Visible = true;
        }
    }
}
