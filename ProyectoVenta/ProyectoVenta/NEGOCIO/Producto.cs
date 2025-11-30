using System;
using System.Data;
using ProyectoVenta.DATOS;

namespace ProyectoVenta.NEGOCIO
{
    public class Producto : Conexion
    {
        private int id_producto;
        private string nombre;
        private decimal precio;
        private int stock;
        private int id_tipo;

        public Producto()
        {
            id_producto = 0;
            nombre = "";
            precio = 0;
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
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        public decimal PrecioBase
        {
            get { return this.precio; }
            set { this.precio = value; }
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

        // Métodos CRUD usando los SP y parámetros reales de tu base de datos
        public bool Guardar()
        {
            iniciarSP("insertarProducto");
            parametroVarchar(nombre, "p_nombre", 100);
            parametroVarchar("", "p_descripcion", 255);
            parametroInt(id_tipo, "p_id_tipo");
            parametroDecimal(precio, "p_precio");
            parametroInt(stock, "p_stock");
            parametroVarchar("", "p_imagen", 255);
            return ejecutarSP();
        }

        public bool Modificar()
        {
            iniciarSP("actualizarProducto");
            parametroInt(id_producto, "p_id_producto");
            parametroVarchar(nombre, "p_nombre", 100);
            parametroVarchar("", "p_descripcion", 255);
            parametroInt(id_tipo, "p_id_tipo");
            parametroDecimal(precio, "p_precio");
            parametroInt(stock, "p_stock");
            parametroVarchar("", "p_imagen", 255);
            return ejecutarSP();
        }

        public bool Eliminar()
        {
            iniciarSP("eliminarProducto");
            parametroInt(id_producto, "p_id_producto");
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
