using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace GestionEstudiantes
{
    public partial class CerrarSesion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Finalizar la autenticación.
            FormsAuthentication.SignOut();

            // Limpiar los datos de sesión.
            Session.Clear();
            Session.Abandon();

            // Evitar mostrar páginas protegidas desde la caché.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            // Regresar al inicio de sesión.
            Response.Redirect("~/Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
