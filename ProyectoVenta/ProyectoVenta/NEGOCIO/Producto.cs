using System;
using System.Data;
using ProyectoVenta.DATOS;

namespace ProyectoVenta.NEGOCIO
{
    public class Producto : Conexion
    {
        private int id_producto;
        private string codigo_producto;
        private string descripcion;
        private decimal precio_base;
        private int stock;
        private int id_tipo;
        private bool es_pizza;
        private string imagen;

        public Producto()
        {
            id_producto = 0;
            codigo_producto = "";
            descripcion = "";
            precio_base = 0;
            stock = 0;
            id_tipo = 0;
            es_pizza = false;
            imagen = "";
        }

        // Propiedades
        public int Idproducto
        {
            get { return this.id_producto; }
            set { this.id_producto = value; }
        }

        public string CodigoProducto
        {
            get { return this.codigo_producto; }
            set { this.codigo_producto = value; }
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

        public bool EsPizza
        {
            get { return this.es_pizza; }
            set { this.es_pizza = value; }
        }

        public string Imagen
        {
            get { return this.imagen; }
            set { this.imagen = value; }
        }

        // Métodos CRUD con nombres correctos
        public bool Guardar()
        {
            iniciarSP("insertarProducto");
            parametroVarchar(codigo_producto, "p_codigo_producto", 50);
            parametroVarchar(descripcion, "p_descripcion", 200);
            parametroDecimal(precio_base, "p_precio_base");
            parametroInt(stock, "p_stock");
            parametroInt(id_tipo, "p_id_tipo");
            parametroBool(es_pizza, "p_es_pizza");
            parametroVarchar(imagen, "p_imagen", 255);
            return ejecutarSP();
        }

        public bool Modificar()
        {
            iniciarSP("actualizarProducto");
            parametroInt(id_producto, "p_id_producto");
            parametroVarchar(codigo_producto, "p_codigo_producto", 50);
            parametroVarchar(descripcion, "p_descripcion", 200);
            parametroDecimal(precio_base, "p_precio_base");
            parametroInt(stock, "p_stock");
            parametroInt(id_tipo, "p_id_tipo");
            parametroBool(es_pizza, "p_es_pizza");
            parametroVarchar(imagen, "p_imagen", 255);
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
