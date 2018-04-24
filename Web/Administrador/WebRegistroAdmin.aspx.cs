using Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WcfNegocio;

namespace Web.Administrador
{
    public partial class WebRegistroAdmin : System.Web.UI.Page
    {
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
        Modelo.RegionCollection coleccionRegion;
        Modelo.ComunaCollection coleccionComuna;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            error.Text = "";
            exito.Text = "";
            alerta_exito.Visible = false;
            alerta.Visible = false;

            //ddlComuna.Enabled = false;
            //ddlRegion.Enabled = false;
            //Cargando DDL Pais
            Service1 service = new Service1();
            string paises = service.ListarPais();
            XmlSerializer ser = new XmlSerializer(typeof(Modelo.PaisCollection));
            StringReader reader = new StringReader(paises);
            Modelo.PaisCollection coleccionPais = (Modelo.PaisCollection)ser.Deserialize(reader);
            reader.Close();

            //Cargando DDL Regiones
            string regiones = service.ListarRegion();
            XmlSerializer ser1 = new XmlSerializer(typeof(Modelo.RegionCollection));
            StringReader reader1 = new StringReader(regiones);
            coleccionRegion = (Modelo.RegionCollection)ser1.Deserialize(reader1);
            reader1.Close();

            //Cargando DDL Comunas
            string comunas = service.ListarComuna();
            XmlSerializer ser2 = new XmlSerializer(typeof(Modelo.ComunaCollection));
            StringReader reader2 = new StringReader(comunas);
            coleccionComuna = (Modelo.ComunaCollection)ser2.Deserialize(reader2);
            reader2.Close();

            //Cargando DDL Tipo Proveedor
            string tipos = service.ListarTipoProveedor();
            XmlSerializer ser3 = new XmlSerializer(typeof(Modelo.TipoProveedorCollection));
            StringReader reader3 = new StringReader(tipos);
            Modelo.TipoProveedorCollection coleccionTipo = (Modelo.TipoProveedorCollection)ser3.Deserialize(reader3);
            reader.Close();

            if (!IsPostBack)
            {
                alerta.Visible = false;
                ddlPais.DataSource = coleccionPais;
                ddlPais.DataTextField = "NOMBRE_PAIS";
                ddlPais.DataValueField = "ID_PAIS";
                ddlPais.DataBind();

                ddlRegion.DataSource = coleccionRegion;
                ddlRegion.DataTextField = "Nombre";
                ddlRegion.DataValueField = "Id_Region";
                ddlRegion.DataBind();

                ddlComuna.DataSource = coleccionComuna;
                ddlComuna.DataTextField = "Nombre";
                ddlComuna.DataValueField = "Id_Comuna";
                ddlComuna.DataBind();

                ddlTipoProveedor.DataSource = coleccionTipo;
                ddlTipoProveedor.DataTextField = "NOMBRE_TIPO";
                ddlTipoProveedor.DataValueField = "ID_TIPO_PROVEEDOR";
                ddlTipoProveedor.DataBind();

                ddlTipo.Items.Add("Cliente");
                ddlTipo.Items.Add("Proveedor");
                ddlTipo.Items.Add("Empleado");

                ddlTipo.SelectedIndex = 0;
                //Deshabilitar un Atributo del Dropdown list, con esto se puede hacer el atributo de ReadOnly.
                //ddlTipo.Items[0].Attributes.Add("disabled", "disabled");
            }
            
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                Modelo.Usuario usuario = new Modelo.Usuario();
                usuario.NOMBRE_USUARIO = txtNombre.Text;
                string hashed = BCrypt.HashPassword(txtPassword.Text, BCrypt.GenerateSalt(12));
                usuario.PASSWORD = hashed;
                usuario.ESTADO = ddlEstado.SelectedValue;
                usuario.TIPO_USUARIO = ddlTipo.SelectedValue;

                Service1 s = new Service1();
                XmlSerializer sr = new XmlSerializer(typeof(Modelo.Usuario));
                StringWriter writer = new StringWriter();
                sr.Serialize(writer, usuario);

                if (txtPassword.Text.Equals(txtConfirmar.Text))
                {
                    if (!s.ExisteUsuario(writer.ToString()))
                    {
                        if (usuario.TIPO_USUARIO.Equals("Cliente"))
                        {
                            Modelo.Cliente cliente = new Modelo.Cliente();
                            Modelo.Proveedor proveedor = new Modelo.Proveedor();
                            Modelo.Empleado empleado = new Modelo.Empleado();

                            cliente.RUT_CLIENTE = int.Parse(txtRut.Text.Substring(0, txtRut.Text.Length - 2));
                            proveedor.RUT_PROVEEDOR = int.Parse(txtRut.Text.Substring(0, txtRut.Text.Length - 2));
                            empleado.RUT_EMPLEADO = int.Parse(txtRut.Text.Substring(0, txtRut.Text.Length - 2));

                            cliente.ID_COMUNA = short.Parse(ddlComuna.SelectedValue);
                            cliente.DV_CLIENTE = txtRut.Text.Substring(txtRut.Text.Length - 1);
                            cliente.NOMBRE_CLIENTE = txtNombreC.Text;
                            cliente.DIRECCION_CLIENTE = txtDireccion.Text;
                            if (txtEmail.Text == string.Empty)
                            {
                                cliente.CORREO_CLIENTE = "";
                            }
                            else
                            {
                                cliente.CORREO_CLIENTE = txtEmail.Text;
                            }

                            if (txtTelefono.Text == string.Empty)
                            {
                                cliente.TELEFONO_CLIENTE = 0;
                            }
                            else
                            {
                                cliente.TELEFONO_CLIENTE = int.Parse(txtTelefono.Text);
                            }

                            XmlSerializer sr2 = new XmlSerializer(typeof(Modelo.Cliente));
                            StringWriter writer2 = new StringWriter();
                            sr2.Serialize(writer2, cliente);
                            writer2.Close();

                            XmlSerializer sr3 = new XmlSerializer(typeof(Modelo.Proveedor));
                            StringWriter writer3 = new StringWriter();
                            sr3.Serialize(writer3, proveedor);
                            writer3.Close();

                            XmlSerializer sr4 = new XmlSerializer(typeof(Modelo.Empleado));
                            StringWriter writer4 = new StringWriter();
                            sr4.Serialize(writer4, empleado);
                            writer4.Close();

                            if (!s.ExisteRutC(writer2.ToString()) && !s.ExisteRutP(writer3.ToString()) && !s.ExisteRutE(writer4.ToString()))
                            {
                                if (s.RegistroUsuario(writer.ToString()) && s.RegistroCliente(writer2.ToString()))
                                {
                                    exito.Text = "El Usuario ha sido registrado con éxito";
                                    alerta_exito.Visible = true;
                                    alerta.Visible = false;
                                }
                                else
                                {
                                    error.Text = "El registro ha fallado";
                                    alerta_exito.Visible = false;
                                    alerta.Visible = true;
                                }
                            }
                            else
                            {
                                error.Text = "El Rut ya existe";
                                alerta.Visible = true;
                                alerta_exito.Visible = false;
                            }
                        }
                        else if (usuario.TIPO_USUARIO.Equals("Empleado"))
                        {
                            Modelo.Cliente cliente = new Modelo.Cliente();
                            Modelo.Proveedor proveedor = new Modelo.Proveedor();
                            Modelo.Empleado empleado = new Modelo.Empleado();

                            empleado.RUT_EMPLEADO = int.Parse(txtRut2.Text.Substring(0, txtRut2.Text.Length - 2));
                            proveedor.RUT_PROVEEDOR = int.Parse(txtRut2.Text.Substring(0, txtRut2.Text.Length - 2));
                            cliente.RUT_CLIENTE = int.Parse(txtRut2.Text.Substring(0, txtRut2.Text.Length - 2));

                            empleado.PNOMBRE_EMPLEADO = txtNombreE.Text;

                            if (txtNombre2E.Text == string.Empty)
                            {
                                empleado.SNOMBRE_EMPLEADO = "";
                            }
                            else
                            {
                                empleado.SNOMBRE_EMPLEADO = txtNombre2E.Text;
                            }
                            empleado.APP_PATERNO_EMPLEADO = txtApellidoP.Text;
                            empleado.APP_MATERNO_EMPLEADO = txtApellidoM.Text;

                            empleado.DV_EMPLEADO = txtRut2.Text.Substring(txtRut2.Text.Length - 1);

                            XmlSerializer sr2 = new XmlSerializer(typeof(Modelo.Cliente));
                            StringWriter writer2 = new StringWriter();
                            sr2.Serialize(writer2, cliente);
                            writer2.Close();

                            XmlSerializer sr3 = new XmlSerializer(typeof(Modelo.Proveedor));
                            StringWriter writer3 = new StringWriter();
                            sr3.Serialize(writer3, proveedor);
                            writer3.Close();

                            XmlSerializer sr4 = new XmlSerializer(typeof(Modelo.Empleado));
                            StringWriter writer4 = new StringWriter();
                            sr4.Serialize(writer4, empleado);
                            writer4.Close();

                            if (!s.ExisteRutC(writer2.ToString()) && !s.ExisteRutP(writer3.ToString()) && !s.ExisteRutE(writer4.ToString()))
                            {
                                if (s.RegistroUsuario(writer.ToString()) && s.RegistroEmpleado(writer4.ToString()))
                                {
                                    exito.Text = "El Empleado ha sido registrado con éxito";
                                    alerta_exito.Visible = true;
                                    alerta.Visible = false;
                                }
                                else
                                {
                                    alerta_exito.Visible = false;
                                    error.Text = "El registro ha fallado";
                                    alerta.Visible = true;
                                }
                            }
                            else
                            {
                                alerta_exito.Visible = false;
                                error.Text = "El Rut ya existe";
                                alerta.Visible = true;
                            }
                        }
                        else if (usuario.TIPO_USUARIO.Equals("Proveedor"))
                        {
                            Modelo.Cliente cliente = new Modelo.Cliente();
                            Modelo.Proveedor proveedor = new Modelo.Proveedor();
                            Modelo.Empleado empleado = new Modelo.Empleado();

                            empleado.RUT_EMPLEADO = int.Parse(txtRut3.Text.Substring(0, txtRut3.Text.Length - 2));
                            proveedor.RUT_PROVEEDOR = int.Parse(txtRut3.Text.Substring(0, txtRut3.Text.Length - 2));
                            cliente.RUT_CLIENTE = int.Parse(txtRut3.Text.Substring(0, txtRut3.Text.Length - 2));
                            proveedor.DV_PROVEEDOR = txtRut3.Text.Substring(txtRut3.Text.Length - 1);
                            proveedor.PNOMBRE_PROVEEDOR = txtNombreP.Text;

                            if (txtNombre2P.Text == string.Empty)
                            {
                                proveedor.SNOMBRE_PROVEEDOR = "";
                            }
                            else
                            {
                                proveedor.SNOMBRE_PROVEEDOR = txtNombre2P.Text;
                            }

                            proveedor.APP_PATERNO_PROVEEDOR = txtApellidoP2.Text;
                            proveedor.APP_MATERNO_PROVEEDOR = txtApellidoM2.Text;
                            proveedor.ID_TIPO_PROVEEDOR = short.Parse(ddlTipoProveedor.SelectedValue);


                            XmlSerializer sr2 = new XmlSerializer(typeof(Modelo.Cliente));
                            StringWriter writer2 = new StringWriter();
                            sr2.Serialize(writer2, cliente);
                            writer2.Close();

                            XmlSerializer sr3 = new XmlSerializer(typeof(Modelo.Proveedor));
                            StringWriter writer3 = new StringWriter();
                            sr3.Serialize(writer3, proveedor);
                            writer3.Close();

                            XmlSerializer sr4 = new XmlSerializer(typeof(Modelo.Empleado));
                            StringWriter writer4 = new StringWriter();
                            sr4.Serialize(writer4, empleado);
                            writer4.Close();

                            if (!s.ExisteRutC(writer2.ToString()) && !s.ExisteRutP(writer3.ToString()) && !s.ExisteRutE(writer4.ToString()))
                            {
                                if (s.RegistroUsuario(writer.ToString()) && s.RegistroProveedor(writer3.ToString()))
                                {
                                    exito.Text = "El Proveedor ha sido registrado con éxito";
                                    alerta_exito.Visible = true;
                                    alerta.Visible = false;
                                }
                                else
                                {
                                    alerta_exito.Visible = false;
                                    error.Text = "El registro ha fallado";
                                    alerta.Visible = true;
                                }
                            }
                            else
                            {
                                alerta_exito.Visible = false;
                                error.Text = "El Rut ya existe";
                                alerta.Visible = true;
                            }
                        }
                        else //Tipos de usuario
                        {
                            alerta_exito.Visible = false;
                            error.Text = "El Tipo ingresado no es válido";
                            alerta.Visible = true;
                        }     
                    }
                    else
                    {
                        alerta_exito.Visible = false;
                        error.Text = "El Nombre de usuario ya ha sido utilizado. Intente con otro";
                        alerta.Visible = true;
                    }
                }
                else
                {
                    alerta_exito.Visible = false;
                    error.Text = "Las Contraseñas no coinciden";
                    alerta.Visible = true;
                }
            }
            catch (Exception)
            {
                alerta_exito.Visible = false;
                error.Text = "Excepcion";
                alerta.Visible = true;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (ddlPais.SelectedValue.Equals("Chile"))
            {
                ddlRegion.DataSource = coleccionRegion.Where(x => x.Id_Pais == 1);
                ddlRegion.DataBind();
                ddlRegion.Enabled = true;
                
            }
            else
            {
                ddlComuna.Enabled = false;
                ddlRegion.Enabled = false;
            }*/
            ddlComuna.Enabled = false;
            ddlRegion.Enabled = false;
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlComuna.DataSource = coleccionComuna.Where(x => x.Id_Region == int.Parse(ddlRegion.SelectedValue));
            ddlComuna.DataBind();
            ddlComuna.Enabled = true;
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
    }
}