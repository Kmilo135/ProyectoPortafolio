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
    public partial class WebUsuarios : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Service1 service = new Service1();
            string datos = service.ListarUsuarios();
            XmlSerializer ser = new XmlSerializer(typeof(Modelo.UsuarioCollection));
            StringReader reader = new StringReader(datos);
            
            Modelo.UsuarioCollection listaUsuario = (Modelo.UsuarioCollection)ser.Deserialize(reader);
            reader.Close();
            gvUsuario.DataSource = listaUsuario;
            gvUsuario.DataBind();
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

        protected void gvUsuario__RowEditing(object sender, EventArgs e)
        {
            Response.Redirect("../Administrador/WebEditarUsuario.aspx");
        }
        /*
        private void CargarTablaDetalle() {//Carga pestañas por linea
            PtoControl control = new PtoControl();
            List<Recorrido> collections = RecorridoCollections.ReadAll();
            //Linea 1
            Recorrido recorrido = collections.FirstOrDefault
                (
                    x => x.Nombre.Contains("1")
                );
            List<PtoControlRecorrido> ptos = PtoControlRecorridoCollections.ReadRuta(recorrido.Id).OrderBy(x => x.Sentido).ThenBy(x => x.Numero).ToList();
            BoundField newColumnName = new BoundField();

            newColumnName.DataField = "Maquina";
            newColumnName.HeaderText = "Máquina";

            gvDetalles.Columns.Add(newColumnName);

            newColumnName.DataField = "Maquina";
            newColumnName.HeaderText = "Máquina";

            gvDetalles.Columns.Add(newColumnName);
            foreach (PtoControlRecorrido p in ptos) {
                control.Id = p.IdControl;
                control.Read();
                newColumnName.DataField = p.Id.ToString() + "-" + p.Sentido + "-" + p.Numero;
                newColumnName.HeaderText = control.Nombre;

                gvDetalles.Columns.Add(newColumnName);
            }

            gvDetalles.DataSource = New List(Of String)
        }
        */
    }
}