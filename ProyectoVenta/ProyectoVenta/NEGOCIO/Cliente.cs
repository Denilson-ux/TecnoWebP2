using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoVenta.DATOS;

namespace ProyectoVenta.NEGOCIO
{
    public class Cliente : Conexion
    {
        private int id_cliente;
        private string nombre;
        private string apellidos;
        private string telefono;
        private string email;       // Email del cliente
        private string direccion;
        private string referencia;  // Referencia de la dirección
        private string zona;        // Zona de la ciudad para delivery

        public Cliente()
        {
            id_cliente = 0;
            nombre = "";
            apellidos = "";
            telefono = "";
            email = "";
            direccion = "";
            referencia = "";
            zona = "";
        }

        // Propiedades
        public int Idcliente
        {
            get { return id_cliente; }
            set { id_cliente = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Apellidos
        {
            get { return apellidos; }
            set { apellidos = value; }
        }

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        public string Referencia
        {
            get { return referencia; }
            set { referencia = value; }
        }

        public string Zona
        {
            get { return zona; }
            set { zona = value; }
        }

        // MÉTODOS CRUD
        public bool guardar()
        {
            iniciarSP("insertarCliente");
            parametroVarchar(nombre, "p_nombre", 100);
            parametroVarchar(apellidos, "p_apellido", 100);
            parametroVarchar(telefono, "p_telefono", 20);
            parametroVarchar(email, "p_email", 100);
            parametroVarchar(direccion, "p_direccion", 255);
            parametroVarchar(referencia, "p_referencia", 255);
            parametroVarchar(zona, "p_zona", 50);
            return ejecutarSP();
        }

        public bool modificar()
        {
            iniciarSP("modificarCliente");
            parametroInt(id_cliente, "id_c");
            parametroVarchar(nombre, "nom", 100);
            parametroVarchar(apellidos, "ape", 100);
            parametroVarchar(telefono, "tel", 20);
            parametroVarchar(email, "mail", 100);
            parametroVarchar(direccion, "dir", 500);
            parametroVarchar(referencia, "ref", 500);
            parametroVarchar(zona, "zon", 100);
            return ejecutarSP();
        }

        public bool eliminar()
        {
            iniciarSP("eliminarCliente");
            parametroInt(id_cliente, "id_cli");
            return ejecutarSP();
        }

        // Usado por frmCliente (listar/buscar) y frmVenta (modal clientes)
        public DataTable buscar()
        {
            iniciarSP("buscarCliente");
            parametroVarchar(Nombre ?? string.Empty, "buscar", 50);
            return mostrarData();
        }

        // Si quieres usarlo con filtro directo (opcional):
        public DataTable Buscar(string texto)
        {
            iniciarSP("buscarCliente");
            parametroVarchar(texto ?? string.Empty, "buscar", 50);
            return mostrarData();
        }


        // Nombre completo
        public string NombreCompleto()
        {
            return (nombre + " " + apellidos).Trim();
        }

        // Calcular costo de delivery según zona
        public decimal calcularCostoDelivery()
        {
            if (string.IsNullOrWhiteSpace(zona))
                return 20;

            switch (zona.ToLower())
            {
                case "centro": return 0;
                case "norte":
                case "sur": return 10;
                case "este":
                case "oeste": return 15;
                default: return 20;
            }
        }
    }
}
