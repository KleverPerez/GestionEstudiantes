
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Web.UI;

namespace GestionEstudiantes
{
    public partial class _Default : Page
    {
        private string ObtenerCadenaConexion()
        {
            return ConfigurationManager
                .ConnectionStrings["ConexionSQL"]
                .ConnectionString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEstadisticas();
                CargarActividad();
                CargarUltimosEstudiantes();
            }
        }

        // =====================================
        // ESTADÍSTICAS GENERALES
        // =====================================

        private void CargarEstadisticas()
        {
            try
            {
                using (SqlConnection conexion =
                    new SqlConnection(ObtenerCadenaConexion()))
                {
                    conexion.Open();

                    // Total de estudiantes
                    string consultaTotal =
                        "SELECT COUNT(*) FROM dbo.Estudiantes";

                    using (SqlCommand comando =
                        new SqlCommand(consultaTotal, conexion))
                    {
                        lblTotalEstudiantes.Text =
                            Convert.ToString(comando.ExecuteScalar());
                    }

                    // Registros del mes actual
                    string consultaMes =
                        @"SELECT COUNT(*)
                          FROM dbo.Estudiantes
                          WHERE FechaRegistro >=
                                DATEFROMPARTS(YEAR(GETDATE()),
                                              MONTH(GETDATE()), 1)
                          AND FechaRegistro < 
                                DATEADD(MONTH, 1,
                                    DATEFROMPARTS(YEAR(GETDATE()),
                                                  MONTH(GETDATE()), 1))";

                    using (SqlCommand comando =
                        new SqlCommand(consultaMes, conexion))
                    {
                        lblRegistrosMes.Text =
                            Convert.ToString(comando.ExecuteScalar());
                    }

                    // Registros de los últimos siete días
                    string consultaSemana =
                        @"SELECT COUNT(*)
                          FROM dbo.Estudiantes
                          WHERE FechaRegistro >=
                                DATEADD(DAY, -6, CONVERT(date, GETDATE()))
                          AND FechaRegistro <
                                DATEADD(DAY, 1, CONVERT(date, GETDATE()))";

                    using (SqlCommand comando =
                        new SqlCommand(consultaSemana, conexion))
                    {
                        lblUltimosSieteDias.Text =
                            Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text =
                    "Error al cargar las estadísticas: "
                    + Server.HtmlEncode(ex.Message);
            }
        }

        // =====================================
        // ACTIVIDAD DE LOS ÚLTIMOS SIETE DÍAS
        // =====================================

        private void CargarActividad()
        {
            try
            {
                DataTable tabla = new DataTable();

                tabla.Columns.Add("Dia", typeof(string));
                tabla.Columns.Add("Cantidad", typeof(int));
                tabla.Columns.Add("Altura", typeof(string));

                Dictionary<DateTime, int> registros =
                    new Dictionary<DateTime, int>();

                DateTime hoy = DateTime.Today;

                // Inicializar los últimos siete días en cero
                for (int i = 6; i >= 0; i--)
                {
                    DateTime fecha = hoy.AddDays(-i);
                    registros.Add(fecha, 0);
                }

                using (SqlConnection conexion =
                    new SqlConnection(ObtenerCadenaConexion()))
                {
                    string consulta =
                        @"SELECT CONVERT(date, FechaRegistro) AS Fecha,
                                 COUNT(*) AS Cantidad
                          FROM dbo.Estudiantes
                          WHERE FechaRegistro >=
                                DATEADD(DAY, -6, CONVERT(date, GETDATE()))
                          AND FechaRegistro <
                                DATEADD(DAY, 1, CONVERT(date, GETDATE()))
                          GROUP BY CONVERT(date, FechaRegistro)";

                    using (SqlCommand comando =
                        new SqlCommand(consulta, conexion))
                    {
                        conexion.Open();

                        using (SqlDataReader lector =
                            comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                DateTime fecha =
                                    Convert.ToDateTime(lector["Fecha"]);

                                int cantidad =
                                    Convert.ToInt32(lector["Cantidad"]);

                                if (registros.ContainsKey(fecha))
                                {
                                    registros[fecha] = cantidad;
                                }
                            }
                        }
                    }
                }

                int maximo = 0;

                foreach (int cantidad in registros.Values)
                {
                    if (cantidad > maximo)
                    {
                        maximo = cantidad;
                    }
                }

                // Crear los datos para el gráfico
                foreach (KeyValuePair<DateTime, int> registro
                    in registros)
                {
                    int altura = maximo == 0
                        ? 4
                        : Math.Max(4,
                            (registro.Value * 100) / maximo);

                    DataRow fila = tabla.NewRow();

                    fila["Dia"] = registro.Key.ToString(
                        "ddd dd",
                        new CultureInfo("es-ES"));

                    fila["Cantidad"] = registro.Value;
                    fila["Altura"] = altura + "%";

                    tabla.Rows.Add(fila);
                }

                rptActividad.DataSource = tabla;
                rptActividad.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text =
                    "Error al cargar el gráfico: "
                    + Server.HtmlEncode(ex.Message);
            }
        }

        // =====================================
        // ÚLTIMOS CINCO ESTUDIANTES
        // =====================================

        private void CargarUltimosEstudiantes()
        {
            try
            {
                using (SqlConnection conexion =
                    new SqlConnection(ObtenerCadenaConexion()))
                {
                    string consulta =
                        @"SELECT TOP 5
                                IdEstudiante,
                                Cedula,
                                Nombre,
                                Apellido,
                                Correo,
                                FechaRegistro
                          FROM dbo.Estudiantes
                          ORDER BY FechaRegistro DESC,
                                   IdEstudiante DESC";

                    using (SqlDataAdapter adaptador =
                        new SqlDataAdapter(consulta, conexion))
                    {
                        DataTable tabla = new DataTable();

                        adaptador.Fill(tabla);

                        gvUltimosEstudiantes.DataSource = tabla;
                        gvUltimosEstudiantes.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text =
                    "Error al cargar los últimos estudiantes: "
                    + Server.HtmlEncode(ex.Message);
            }
        }
    }
}
