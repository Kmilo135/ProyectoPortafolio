﻿using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WcfNegocio;

namespace Web.Administrador
{
    public partial class WebUsuarios : System.Web.UI.Page
    {

        private List<Usuario> users;
        private List<Modelo.Empleado> empleados;
        private List<Modelo.Cliente> clientes;
        private List<Modelo.Proveedor> proveedores;


        void Page_PreInit(object sender, EventArgs e)
        {
            if (MiSesion != null)
            {
                if (MiSesion.TIPO_USUARIO != null && MiSesion.ESTADO != null)
                {
                    if (MiSesion.TIPO_USUARIO.Equals("Administrador") &&
                    MiSesion.ESTADO.Equals("Habilitado"))
                    {
                        MasterPageFile = "~/Administrador/AdminM.Master";
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>window.alert('No Posee los permisos necesarios');window.location='../Hostal/WebLogin.aspx';</script>");
                    }
                }
                else
                {
                    Response.Write("<script language='javascript'>window.alert('Debe Iniciar Sesión Primero');window.location='../Hostal/WebLogin.aspx';</script>");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CargarGriedView();
        }

        //Creación de Sesión
        public Usuario MiSesion
        {
            get
            {
                if (Session["Usuario"] == null)
                {
                    Session["Usuario"] = new Usuario();
                }
                return (Usuario)Session["Usuario"];
            }
            set
            {
                Session["Usuario"] = value;
            }
        }


        
        private void CargarGriedView() {
            //Carga listas con datos
            users = UsuarioCollection.ListaUsuarios();
            empleados = EmpleadoCollection.ListaEmpleados();
            clientes = ClienteCollection.ListaClientes();
            proveedores = ProveedorCollection.ListaProveedores();

            Usuario user;
            
            //Creacion DataTable
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                new DataColumn("ID", typeof(int)),
                new DataColumn("Nombre", typeof(string)),
                new DataColumn("Usuario",typeof(string)),
                new DataColumn("Tipo",typeof(string)),
                new DataColumn("Estado",typeof(string))
            });

            //Carga de datos en DataTable
            foreach (Modelo.Cliente c in clientes) {
                user = new Usuario();
                var list = users.Where(x => x.ID_USUARIO == c.ID_USUARIO).ToList();
                foreach (var item in list) {
                    user.NOMBRE_USUARIO = item.NOMBRE_USUARIO;
                    user.TIPO_USUARIO = item.TIPO_USUARIO;
                    user.ESTADO = item.ESTADO;
                }
                dt.Rows.Add(c.ID_USUARIO,c.NOMBRE_CLIENTE,user.NOMBRE_USUARIO,user.TIPO_USUARIO,user.ESTADO);
            }

            foreach (Modelo.Empleado e in empleados) {
                user = new Usuario();
                var list = users.Where(x => x.ID_USUARIO == e.ID_USUARIO).ToList();
                foreach (var item in list) {
                    user.NOMBRE_USUARIO = item.NOMBRE_USUARIO;
                    user.TIPO_USUARIO = item.TIPO_USUARIO;
                    user.ESTADO = item.ESTADO;
                }
                dt.Rows.Add(e.ID_USUARIO,e.PNOMBRE_EMPLEADO+ " " + e.APP_PATERNO_EMPLEADO + " " + e.APP_MATERNO_EMPLEADO,user.NOMBRE_USUARIO,user.TIPO_USUARIO,user.ESTADO);
            }

            foreach (Modelo.Proveedor p in proveedores) {
                user = new Usuario();
                var list = users.Where(x => x.ID_USUARIO == p.ID_USUARIO).ToList();
                foreach (var item in list) {
                    user.NOMBRE_USUARIO = item.NOMBRE_USUARIO;
                    user.TIPO_USUARIO = item.TIPO_USUARIO;
                    user.ESTADO = item.ESTADO;
                }
                dt.Rows.Add(p.ID_USUARIO,p.PNOMBRE_PROVEEDOR + " " + p.APP_PATERNO_PROVEEDOR + " " + p.APP_MATERNO_PROVEEDOR,user.NOMBRE_USUARIO,user.TIPO_USUARIO,user.ESTADO);
            }

            //Carga de GriedView
            gvUsuario.DataSource = dt;
            gvUsuario.DataBind();
        }

        protected void gvUsuario_RowEditing(object sender,GridViewEditEventArgs e) {
            string tipo = gvUsuario.DataKeys[e.NewEditIndex].Values["Tipo"].ToString();
            int id = int.Parse(gvUsuario.DataKeys[e.NewEditIndex].Values["ID"].ToString());
            if (tipo.Equals(Tipo_Usuario.Administrador.ToString()) || tipo.Equals(Tipo_Usuario.Empleado.ToString())) {
                Usuario user = new Usuario();
                var list = users.Where(x => x.ID_USUARIO == id).ToList();
                foreach (Usuario item in list) {
                    user.NOMBRE_USUARIO = item.NOMBRE_USUARIO;
                    user.TIPO_USUARIO = item.TIPO_USUARIO;
                    user.ESTADO = item.ESTADO;
                    user.ID_USUARIO = item.ID_USUARIO;
                }
                SesionEdit = user;
                Modelo.Empleado emp = new Modelo.Empleado();
                var empl = empleados.Where(x => x.ID_USUARIO == id).ToList();
                foreach (Modelo.Empleado item in empl) {
                    emp.APP_MATERNO_EMPLEADO = item.APP_MATERNO_EMPLEADO;
                    emp.APP_PATERNO_EMPLEADO = item.APP_PATERNO_EMPLEADO;
                    emp.DV_EMPLEADO = item.DV_EMPLEADO;
                    emp.ID_USUARIO = item.ID_USUARIO;
                    emp.PNOMBRE_EMPLEADO = item.PNOMBRE_EMPLEADO;
                    emp.RUT_EMPLEADO = item.RUT_EMPLEADO;
                    emp.SNOMBRE_EMPLEADO = item.SNOMBRE_EMPLEADO;
                }
                SesionEmp = emp;
                Response.Redirect("../Empleado/WebEditarEmpleado.aspx");
            }
            if (tipo.Equals(Tipo_Usuario.Cliente.ToString())) {
                Usuario user = new Usuario();
                var list = users.Where(x => x.ID_USUARIO == id).ToList();
                foreach (Usuario item in list) {
                    user.NOMBRE_USUARIO = item.NOMBRE_USUARIO;
                    user.TIPO_USUARIO = item.TIPO_USUARIO;
                    user.ESTADO = item.ESTADO;
                    user.ID_USUARIO = item.ID_USUARIO;
                }
                SesionEdit = user;
                Modelo.Cliente cli = new Modelo.Cliente();
                var clil = clientes.Where(x => x.ID_USUARIO == id).ToList();
                foreach (Modelo.Cliente item in clil) {
                    cli.CORREO_CLIENTE = item.CORREO_CLIENTE;
                    cli.DIRECCION_CLIENTE = item.DIRECCION_CLIENTE;
                    cli.DV_CLIENTE = item.DV_CLIENTE;
                    cli.ID_USUARIO = item.ID_USUARIO;
                    cli.ID_COMUNA = item.ID_COMUNA;
                    cli.NOMBRE_CLIENTE = item.NOMBRE_CLIENTE;
                    cli.RUT_CLIENTE = item.RUT_CLIENTE;
                    cli.TELEFONO_CLIENTE = item.TELEFONO_CLIENTE;
                }
                SesionCl = cli;
                Response.Redirect("../Cliente/WebEditarCliente.aspx");
            }
            if (tipo.Equals(Tipo_Usuario.Proveedor.ToString())) {
                Usuario user = new Usuario();
                var list = users.Where(x => x.ID_USUARIO == id).ToList();
                foreach (Usuario item in list) {
                    user.NOMBRE_USUARIO = item.NOMBRE_USUARIO;
                    user.TIPO_USUARIO = item.TIPO_USUARIO;
                    user.ESTADO = item.ESTADO;
                    user.ID_USUARIO = item.ID_USUARIO;
                }
                SesionEdit = user;
                Modelo.Proveedor prov = new Modelo.Proveedor();
                var provl = proveedores.Where(x => x.ID_USUARIO == id).ToList();
                foreach (Modelo.Proveedor item in provl) {
                    prov.APP_MATERNO_PROVEEDOR = item.APP_MATERNO_PROVEEDOR;
                    prov.APP_PATERNO_PROVEEDOR = item.APP_PATERNO_PROVEEDOR;
                    prov.DV_PROVEEDOR = item.DV_PROVEEDOR;
                    prov.ID_USUARIO = item.ID_USUARIO;
                    prov.ID_TIPO_PROVEEDOR = item.ID_TIPO_PROVEEDOR;
                    prov.PNOMBRE_PROVEEDOR = item.PNOMBRE_PROVEEDOR;
                    prov.RUT_PROVEEDOR = item.RUT_PROVEEDOR;
                    prov.SNOMBRE_PROVEEDOR = item.SNOMBRE_PROVEEDOR;
                }
                SesionPro = prov;
                Response.Redirect("../Proveedor/WebEditarProveedor.aspx");
            }
        }
        //Sesione para editar datos
        public Modelo.Cliente SesionCl {
            get {
                if (Session["Cliente"] == null) {
                    Session["Cliente"] = new Modelo.Cliente();
                }
                return (Modelo.Cliente)Session["Cliente"];
            }
            set {
                Session["Cliente"] = value;
            }
        }

        public Modelo.Proveedor SesionPro {
            get {
                if (Session["Proveedor"] == null) {
                    Session["Proveedor"] = new Modelo.Proveedor();
                }
                return (Modelo.Proveedor)Session["Proveedor"];
            }
            set {
                Session["Proveedor"] = value;
            }
        }

        public Modelo.Empleado SesionEmp {
            get {
                if (Session["Empleado"] == null) {
                    Session["Empleado"] = new Modelo.Empleado();
                }
                return (Modelo.Empleado)Session["Empleado"];
            }
            set {
                Session["Empleado"] = value;
            }
        }

        public Usuario SesionEdit {
            get {
                if (Session["UsuarioEdit"] == null) {
                    Session["UsuarioEdit"] = new Usuario();
                }
                return (Usuario)Session["UsuarioEdit"];
            }
            set {
                Session["UsuarioEdit"] = value;
            }
        }
    }
}