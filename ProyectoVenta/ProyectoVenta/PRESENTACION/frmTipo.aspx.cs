using System;
using System.Data;
using ProyectoVenta.NEGOCIO;

namespace ProyectoVenta.PRESENTACION
{
    public partial class frmTipo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)    
            {
                CargarTipos();
            }
        }

        private void CargarTipos()
        {
            Tipo tipo = new Tipo();
            gvTipo.DataSource = tipo.Buscar(""); // Trae todos los tamaños
            gvTipo.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Tipo tipo = new Tipo();
            tipo.Nombre = txtTipo.Text.Trim();
            if (tipo.Guardar())
            {
                // Puedes agregar mensajes para el usuario aquí
                CargarTipos();
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            txtTipo.Text = "";
            lblMensaje.Visible = false;
            gvTipo.SelectedIndex = -1;
            // Puedes agregar más lógica para limpiar el formulario si lo necesitas
        }


        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (gvTipo.SelectedIndex >= 0)
            {
                // Obtén el ID seleccionado
                int idTipo = Convert.ToInt32(gvTipo.SelectedRow.Cells[0].Text);
                Tipo tipo = new Tipo();
                tipo.IdTipo = idTipo;
                if (tipo.Eliminar())
                {
                    lblMensaje.Text = "Tipo eliminado correctamente";
                    lblMensaje.CssClass = "alert alert-success mt-3";
                    lblMensaje.Visible = true;
                    CargarTipos();
                    txtTipo.Text = "";
                    gvTipo.SelectedIndex = -1;
                }
                else
                {
                    lblMensaje.Text = "Error al eliminar el tipo";
                    lblMensaje.CssClass = "alert alert-danger mt-3";
                    lblMensaje.Visible = true;
                }
            }
            else
            {
                lblMensaje.Text = "Debes seleccionar un tipo";
                lblMensaje.CssClass = "alert alert-warning mt-3";
                lblMensaje.Visible = true;
            }
        }

        protected void gvTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvTipo.SelectedIndex >= 0)
            {
                // Obtener el nombre seleccionado para mostrar/editar
                txtTipo.Text = gvTipo.SelectedRow.Cells[1].Text;
            }
        }


    }
}
