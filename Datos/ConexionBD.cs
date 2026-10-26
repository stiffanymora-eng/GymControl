using Microsoft.Data.SqlClient;

namespace GymControl.Datos
{
    // Se encarga únicamente de crear conexiones a SQL Server
    public static class ConexionBD
    {
        private const string cadenaConexion =
            "Server=TIFF\\SQLEXPRESS;Database=GymControl;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }

        // Prueba si se puede abrir la conexión y devuelve un mensaje claro
        public static bool ProbarConexion(out string mensaje)
        {
            try
            {
                using (SqlConnection conexion = ObtenerConexion())
                {
                    conexion.Open();
                    mensaje = "Conexión exitosa con la base de datos GymControl.";
                    return true;
                }
            }
            catch (SqlException ex)
            {
                mensaje = "No se pudo conectar con la base de datos: " + ex.Message;
                return false;
            }
        }
    }
}