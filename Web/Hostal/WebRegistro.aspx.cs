using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WcfNegocio;

namespace Web
{
    public partial class WebRegistro2 : System.Web.UI.Page
    {
        Modelo.RegionCollection coleccionRegion;
        Modelo.ComunaCollection coleccionComuna;

        protected void Page_Load(object sender, EventArgs e)
        {
            error.Text = "";
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
                usuario.ESTADO = "Habilitado";
                usuario.TIPO_USUARIO = "Cliente";

                Service1 s = new Service1();
                XmlSerializer sr = new XmlSerializer(typeof(Modelo.Usuario));
                StringWriter writer = new StringWriter();
                sr.Serialize(writer, usuario);

                if (txtPassword.Text.Equals(txtConfirmar.Text))
                {
                    if (!s.ExisteUsuario(writer.ToString()))
                    {
                        Modelo.Cliente cliente = new Modelo.Cliente();
                        Modelo.Proveedor proveedor = new Modelo.Proveedor();
                        Modelo.Empleado empleado = new Modelo.Empleado();

                        cliente.RUT_CLIENTE = int.Parse(txtRut.Text.Substring(0, txtRut.Text.Length - 2));
                        proveedor.RUT_PROVEEDOR = int.Parse(txtRut.Text.Substring(0, txtRut.Text.Length - 2));
                        empleado.RUT_EMPLEADO = int.Parse(txtRut.Text.Substring(0, txtRut.Text.Length - 2));

                        cliente.ID_COMUNA = short.Parse(ddlComuna.SelectedValue);
                        cliente.DV_CLIENTE = txtRut.Text.Substring(txtRut.Text.Length - 1);
                        cliente.DIRECCION_CLIENTE = txtDireccion.Text;
                        cliente.NOMBRE_CLIENTE = txtNombreC.Text;
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
                                Response.Write("<script language='javascript'>window.alert('Se ha registrado con éxito. pruebe iniciar sesión');window.location='../Hostal/WebLogin.aspx';</script>");
                            }
                            else
                            {
                                error.Text = "El registro ha fallado";
                                alerta.Visible = true;
                            }
                        }
                        else
                        {
                            error.Text = "El Rut ya existe";
                            alerta.Visible = true;
                        }
                    }
                    else
                    {
                        error.Text = "El Nombre de usuario ya ha sido utilizado. Intente con otro";
                        alerta.Visible = true;
                    }
                }
                else
                {
                    error.Text = "Las Contraseñas no coinciden";
                    alerta.Visible = true;
                }
            }
            catch (Exception)
            {
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
    }
}