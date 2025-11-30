using System;
using System.Data;
using ProyectoVenta.DATOS;

namespace ProyectoVenta.NEGOCIO
{
    public class Producto : Conexion
    {
        private int id_producto;
        private string descripcion;
        private decimal precio_base;
        private int stock;
        private int id_tipo;

        public Producto()
        {
            id_producto = 0;
            descripcion = "";
            precio_base = 0;
            stock = 0;
            id_tipo = 0;
        }

        // Propiedades
        public int Idproducto
        {
            get { return this.id_producto; }
            set { this.id_producto = value; }
        }

        public string Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }

        public decimal PrecioBase
        {
            get { return this.precio_base; }
            set { this.precio_base = value; }
        }

        public int Stock
        {
            get { return this.stock; }
            set { this.stock = value; }
        }

        public int IdTipo
        {
            get { return this.id_tipo; }
            set { this.id_tipo = value; }
        }

        // Métodos CRUD ajustados a los nombres y orden de tu base actual
        public bool Guardar()
        {
            iniciarSP("guardarProducto");
            parametroVarchar(descripcion, "descr", 30);
            parametroDecimal(precio_base, "prec");
            parametroInt(stock, "stoc");
            parametroInt(id_tipo, "id_cat");
            return ejecutarSP();
        }

        public bool Modificar()
        {
            iniciarSP("modificarProducto");
            parametroInt(id_producto, "id_prod");
            parametroVarchar(descripcion, "descr", 30);
            parametroDecimal(precio_base, "prec");
            parametroInt(stock, "stoc");
            parametroInt(id_tipo, "id_cat");
            return ejecutarSP();
        }

        public bool Eliminar()
        {
            iniciarSP("eliminarProducto");
            parametroInt(id_producto, "id_prod");
            return ejecutarSP();
        }

        public DataTable Buscar(string texto)
        {
            iniciarSP("buscarProducto");
            parametroVarchar(texto ?? string.Empty, "buscar", 100);
            return mostrarData();
        }
    }
}
