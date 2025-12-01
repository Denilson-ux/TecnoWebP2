using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoVenta.DATOS;

namespace ProyectoVenta.NEGOCIO
{
    public class DetalleVenta : Conexion
    {
        private int id_detalle;
        private int id_venta;
        private int id_producto;
        private int id_tamanio;
        private int cantidad;
        private decimal precio_unitario;
        private decimal subtotal;
        private string notas_especiales;

        public DetalleVenta()
        {
            id_detalle = 0;
            id_venta = 0;
            id_producto = 0;
            id_tamanio = 0;
            cantidad = 1;
            precio_unitario = 0;
            subtotal = 0;
            notas_especiales = "";
        }

        // Propiedades
        public int IdDetalle
        {
            get { return this.id_detalle; }
            set { this.id_detalle = value; }
        }

        public int IdVenta
        {
            get { return this.id_venta; }
            set { this.id_venta = value; }
        }

        public int IdProducto
        {
            get { return this.id_producto; }
            set { this.id_producto = value; }
        }

        public int IdTamanio
        {
            get { return this.id_tamanio; }
            set { this.id_tamanio = value; }
        }

        public int Cantidad
        {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }

        public decimal PrecioUnitario
        {
            get { return this.precio_unitario; }
            set { this.precio_unitario = value; }
        }

        public decimal Subtotal
        {
            get { return this.subtotal; }
            set { this.subtotal = value; }
        }

        public string NotasEspeciales
        {
            get { return this.notas_especiales; }
            set { this.notas_especiales = value; }
        }

        // Métodos
        public void calcularSubtotal()
        {
            this.subtotal = this.precio_unitario * this.cantidad;
        }

        public bool guardar()
        {
            iniciarSP("guardarDetalleVenta");
            parametroInt(id_venta, "id_ven");
            parametroInt(id_producto, "id_prod");

            if (id_tamanio > 0)
                parametroInt(id_tamanio, "id_tam");
            else
                parametroInt(0, "id_tam");

            parametroInt(cantidad, "cant");
            parametroDecimal(precio_unitario, "precio");
            parametroDecimal(subtotal, "sub");
            parametroVarchar(notas_especiales, "notas", 500);

            if (ejecutarSP() == true) { return true; } else { return false; }
        }

        public DataTable listarPorVenta()
        {
            iniciarSP("listarDetallesPorVenta");
            parametroInt(id_venta, "id_ven");
            return mostrarData();
        }

        public DataTable listarPorVenta(int idVenta)
        {
            iniciarSP("listarDetallesPorVenta");
            parametroInt(idVenta, "id_ven");
            return mostrarData();
        }
    }
}
