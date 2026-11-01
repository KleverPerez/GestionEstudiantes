using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.Security;
using System.Web.UI;

namespace GestionEstudiantes
{
    public partial class Login : Page
    {
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            string conexion = ConfigurationManager
                .ConnectionStrings["ConexionSQL"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(conexion))
                {
                    string consulta = @"
                        SELECT HashContrasena, Salt
                        FROM dbo.Usuarios
                        WHERE Usuario = @Usuario";

                    using (SqlCommand cmd = new SqlCommand(consulta, cn))
                    {
                        cmd.Parameters.AddWithValue("@Usuario", usuario);

                        cn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MostrarMensaje("Usuario o contraseña incorrectos.");
                                return;
                            }

                            string hashGuardado = reader["HashContrasena"].ToString();
                            string saltGuardado = reader["Salt"].ToString();

                            byte[] hashOriginal =
                                Convert.FromBase64String(hashGuardado);

                            byte[] salt =
                                Convert.FromBase64String(saltGuardado);

                            byte[] hashIngresado;

                            using (var pbkdf2 = new Rfc2898DeriveBytes(
                                contrasena,
                                salt,
                                150000,
                                HashAlgorithmName.SHA256))
                            {
                                hashIngresado = pbkdf2.GetBytes(32);
                            }

                            // Comparación de hashes en tiempo constante.
                            if (!CompararHashes(hashOriginal, hashIngresado))
                            {
                                MostrarMensaje("Usuario o contraseña incorrectos.");
                                return;
                            }
                        }
                    }
                }

                // Crear la cookie de autenticación.
                FormsAuthentication.SetAuthCookie(usuario, false);

                // Redirigir al panel principal.
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                MostrarMensaje("No se pudo iniciar sesión. Verifica la conexión.");
            }
        }

        private bool CompararHashes(byte[] original, byte[] ingresado)
        {
            if (original == null || ingresado == null ||
                original.Length != ingresado.Length)
                return false;

            int diferencia = 0;

            for (int i = 0; i < original.Length; i++)
            {
                diferencia |= original[i] ^ ingresado[i];
            }

            return diferencia == 0;
        }

        private void MostrarMensaje(string mensaje)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = System.Drawing.Color.Red;
        }
    }
}
