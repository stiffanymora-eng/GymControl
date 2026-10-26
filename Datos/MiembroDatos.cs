using Microsoft.Data.SqlClient;
using GymControl.Logica;

namespace GymControl.Datos
{
    // Acceso a la tabla Miembro: aquí vive todo el SQL de los miembros
    public class MiembroDatos
    {
        private const string columnas =
            "IdMiembro, Cedula, Nombre, Apellidos, FechaNacimiento, Telefono, Correo, FechaRegistro, Activo";

        // Consulta: devuelve todos los miembros, o solo los que coinciden con el texto
        public List<Miembro> Buscar(string texto)
        {
            List<Miembro> lista = new List<Miembro>();

            string sql = "SELECT " + columnas + " FROM Miembro " +
                         "WHERE Cedula LIKE @texto OR Nombre LIKE @texto OR Apellidos LIKE @texto " +
                         "ORDER BY Apellidos, Nombre";

            using (SqlConnection conexion = ConexionBD.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@texto", "%" + texto + "%");
                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(ConvertirAMiembro(lector));
                    }
                }
            }
            return lista;
        }

        public List<Miembro> ObtenerTodos()
        {
            return Buscar("");
        }

        // Inserción: devuelve el Id que la base de datos le asignó al nuevo miembro
        public int Insertar(Miembro miembro)
        {
            string sql = "INSERT INTO Miembro (Cedula, Nombre, Apellidos, FechaNacimiento, Telefono, Correo, Activo) " +
                         "OUTPUT INSERTED.IdMiembro " +
                         "VALUES (@cedula, @nombre, @apellidos, @fechaNacimiento, @telefono, @correo, @activo)";

            using (SqlConnection conexion = ConexionBD.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                AgregarParametros(comando, miembro);
                conexion.Open();
                return (int)comando.ExecuteScalar();
            }
        }

        // Modificación: devuelve cuántas filas se actualizaron (0 si el registro ya no existe)
        public int Actualizar(Miembro miembro)
        {
            string sql = "UPDATE Miembro SET Cedula = @cedula, Nombre = @nombre, Apellidos = @apellidos, " +
                         "FechaNacimiento = @fechaNacimiento, Telefono = @telefono, Correo = @correo, " +
                         "Activo = @activo WHERE IdMiembro = @id";

            using (SqlConnection conexion = ConexionBD.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                AgregarParametros(comando, miembro);
                comando.Parameters.AddWithValue("@id", miembro.IdMiembro);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        // Eliminación: la base de datos impide borrar miembros que tienen membresías o asistencias
        public int Eliminar(int idMiembro)
        {
            string sql = "DELETE FROM Miembro WHERE IdMiembro = @id";

            try
            {
                using (SqlConnection conexion = ConexionBD.ObtenerConexion())
                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", idMiembro);
                    conexion.Open();
                    return comando.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) when (ex.Number == 547)   // 547 = violación de llave foránea
            {
                throw new InvalidOperationException(
                    "No se puede eliminar el miembro porque tiene membresías o asistencias registradas. " +
                    "Puede marcarlo como inactivo.");
            }
        }

        // Duplicados: ¿ya existe otro miembro con esa cédula?
        public bool ExisteCedula(string cedula, int idExcluir)
        {
            string sql = "SELECT COUNT(*) FROM Miembro WHERE Cedula = @cedula AND IdMiembro <> @id";

            using (SqlConnection conexion = ConexionBD.ObtenerConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@cedula", cedula);
                comando.Parameters.AddWithValue("@id", idExcluir);
                conexion.Open();
                return (int)comando.ExecuteScalar() > 0;
            }
        }

        // ---------- Métodos auxiliares (evitan código duplicado) ----------

        private void AgregarParametros(SqlCommand comando, Miembro miembro)
        {
            comando.Parameters.AddWithValue("@cedula", miembro.Cedula);
            comando.Parameters.AddWithValue("@nombre", miembro.Nombre);
            comando.Parameters.AddWithValue("@apellidos", miembro.Apellidos);
            comando.Parameters.AddWithValue("@fechaNacimiento", miembro.FechaNacimiento);
            comando.Parameters.AddWithValue("@telefono", miembro.Telefono);
            comando.Parameters.AddWithValue("@correo",
                string.IsNullOrWhiteSpace(miembro.Correo) ? (object)DBNull.Value : miembro.Correo);
            comando.Parameters.AddWithValue("@activo", miembro.Activo);
        }

        private Miembro ConvertirAMiembro(SqlDataReader lector)
        {
            Miembro miembro = new Miembro();
            miembro.IdMiembro = lector.GetInt32(lector.GetOrdinal("IdMiembro"));
            miembro.Cedula = lector.GetString(lector.GetOrdinal("Cedula"));
            miembro.Nombre = lector.GetString(lector.GetOrdinal("Nombre"));
            miembro.Apellidos = lector.GetString(lector.GetOrdinal("Apellidos"));
            miembro.FechaNacimiento = lector.GetDateTime(lector.GetOrdinal("FechaNacimiento"));
            miembro.Telefono = lector.GetString(lector.GetOrdinal("Telefono"));

            int posicionCorreo = lector.GetOrdinal("Correo");
            miembro.Correo = lector.IsDBNull(posicionCorreo) ? "" : lector.GetString(posicionCorreo);

            miembro.FechaRegistro = lector.GetDateTime(lector.GetOrdinal("FechaRegistro"));
            miembro.Activo = lector.GetBoolean(lector.GetOrdinal("Activo"));
            return miembro;
        }
    }
}