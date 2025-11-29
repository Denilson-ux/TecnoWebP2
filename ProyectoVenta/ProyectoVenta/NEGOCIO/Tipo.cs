using System;
using System.Data;
using ProyectoVenta.DATOS;

namespace ProyectoVenta.NEGOCIO
{
    public class Tipo : Conexion
    {
        private int id_tipo;
        private string nombre;

        public Tipo()
        {
            id_tipo = 0;
            nombre = "";
        }

        // Propiedades
        public int IdTipo
        {
            get { return this.id_tipo; }
            set { this.id_tipo = value; }
        }

        public string Nombre
        {
            get { return this.nombre; }
            set { this.nombre = value; }
        }

        // Métodos CRUD

        public bool Guardar()
        {
            iniciarSP("insertarTipo");
            parametroVarchar(nombre, "p_nombre", 50);
            return ejecutarSP();
        }

        public bool Modificar()
        {
            iniciarSP("actualizarTipo");
            parametroInt(id_tipo, "p_id_tipo");
            parametroVarchar(nombre, "p_nombre", 50);
            return ejecutarSP();
        }

        public bool Eliminar()
        {
            iniciarSP("eliminarTipo");
            parametroInt(id_tipo, "p_id_tipo");
            return ejecutarSP();
        }

        public DataTable Buscar(string nombreBuscar = "")
        {
            iniciarSP("buscarTipo");
            parametroVarchar(nombreBuscar, "p_nombre", 50);
            return mostrarData();
        }

        // NUEVO: Para obtener la descripción formateada del tipo (opcional)
        public string descripcionTipo()
        {
            return "Tamaño: " + nombre;
        }

        public DataTable Listar()
        {
            return Buscar("");
        }

    }
}
