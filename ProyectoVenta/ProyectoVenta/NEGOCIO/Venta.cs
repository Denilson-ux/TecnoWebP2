using System;
using System.Data;
using ProyectoVenta.DATOS;

namespace ProyectoVenta.NEGOCIO
{
    public class Venta : Conexion
    {
        private int id_venta;
        private string numero_venta;
        private DateTime fecha_venta;
        private int id_cliente;
        private string tipo_entrega;
        private string direccion_entrega;
        private decimal subtotal;
        private decimal descuento;
        private decimal costo_delivery;
        private decimal total;
        private string metodo_pago;
        private string estado_venta;
        private string observaciones;

        public Venta()
        {
            id_venta = 0;
            numero_venta = "";
            fecha_venta = DateTime.Now;
            id_cliente = 0;
            tipo_entrega = "Local";
            direccion_entrega = "";
            subtotal = 0;
            descuento = 0;
            costo_delivery = 0;
            total = 0;
            metodo_pago = "Efectivo";
            estado_venta = "Pendiente";
            observaciones = "";
        }

        // Propiedades
        public int IdVenta
        {
            get { return this.id_venta; }
            set { this.id_venta = value; }
        }

        public string NumeroVenta
        {
            get { return this.numero_venta; }
            set { this.numero_venta = value; }
        }

        public DateTime FechaVenta
        {
            get { return this.fecha_venta; }
            set { this.fecha_venta = value; }
        }

        public int IdCliente
        {
            get { return this.id_cliente; }
            set { this.id_cliente = value; }
        }

        public string TipoEntrega
        {
            get { return this.tipo_entrega; }
            set { this.tipo_entrega = value; }
        }

        public string DireccionEntrega
        {
            get { return this.direccion_entrega; }
            set { this.direccion_entrega = value; }
        }

        public decimal Subtotal
        {
            get { return this.subtotal; }
            set { this.subtotal = value; }
        }

        public decimal Descuento
        {
            get { return this.descuento; }
            set { this.descuento = value; }
        }

        public decimal CostoDelivery
        {
            get { return this.costo_delivery; }
            set { this.costo_delivery = value; }
        }

        public decimal Total
        {
            get { return this.total; }
            set { this.total = value; }
        }

        public string MetodoPago
        {
            get { return this.metodo_pago; }
            set { this.metodo_pago = value; }
        }

        public string EstadoVenta
        {
            get { return this.estado_venta; }
            set { this.estado_venta = value; }
        }

        public string Observaciones
        {
            get { return this.observaciones; }
            set { this.observaciones = value; }
        }

        // Métodos
        public bool guardar()
        {
            iniciarSP("guardarVenta");
            parametroVarchar(numero_venta, "num_ven", 50);
            parametroFecha(fecha_venta, "fec_ven");
            parametroInt(id_cliente, "id_cli");
            
            // NOTA: Aunque el SP recibe estos parámetros, los ignora internamente
            // Los enviamos para mantener la firma del SP pero con valores dummy
            parametroVarchar(tipo_entrega, "tip_ent", 20);
            parametroDecimal(descuento, "desc");
            parametroDecimal(costo_delivery, "del");
            
            parametroVarchar(direccion_entrega, "dir_ent", 500);
            parametroDecimal(subtotal, "sub");
            parametroDecimal(total, "tot");
            parametroVarchar(metodo_pago, "met_pag", 50);
            parametroVarchar(estado_venta, "est_ven", 20);
            parametroVarchar(observaciones, "obs", 500);
            return ejecutarSP();
        }

        public bool modificar()
        {
            iniciarSP("modificarVenta");
            parametroInt(id_venta, "id_ven");
            parametroVarchar(estado_venta, "est_ven", 20);
            parametroVarchar(observaciones, "obs", 500);
            return ejecutarSP();
        }

        public DataTable buscar()
        {
            iniciarSP("buscarVenta");
            parametroVarchar(numero_venta, "buscar", 50);
            return mostrarData();
        }

        public DataTable listar()
        {
            iniciarSP("listarVentas");
            return mostrarData();
        }

        public string generarNumeroVenta()
        {
            return "PZ-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public void calcularTotal()
        {
            decimal totalConDescuento = subtotal - descuento;
            if (tipo_entrega == "Delivery")
                total = totalConDescuento + costo_delivery;
            else
                total = totalConDescuento;
        }

        // Obtener ID de última venta insertada
        public int obtenerUltimoId()
        {
            iniciarSP("obtenerUltimaVenta");
            DataTable dt = mostrarData();
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["id_venta"]);
            return 0;
        }
    }
}
