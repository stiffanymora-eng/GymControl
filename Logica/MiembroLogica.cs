using System.Net.Mail;
using System.Text.RegularExpressions;
using GymControl.Datos;

namespace GymControl.Logica
{
    // Reglas de negocio y validaciones de los miembros
    public class MiembroLogica
    {
        private readonly MiembroDatos datos = new MiembroDatos();

        public List<Miembro> ObtenerTodos()
        {
            return datos.ObtenerTodos();
        }

        public List<Miembro> Buscar(string texto)
        {
            return datos.Buscar(texto.Trim());
        }

        // Devuelve los errores encontrados: clave = nombre del campo, valor = mensaje.
        // Si el diccionario viene vacío, los datos son válidos.
        public Dictionary<string, string> Validar(Miembro miembro)
        {
            Dictionary<string, string> errores = new Dictionary<string, string>();

            string cedula = miembro.Cedula.Trim();
            string nombre = miembro.Nombre.Trim();
            string apellidos = miembro.Apellidos.Trim();
            string telefono = miembro.Telefono.Trim();
            string correo = miembro.Correo.Trim();

            // Cédula: obligatoria, con formato y sin duplicados
            if (cedula == "")
            {
                errores["Cedula"] = "La cédula es obligatoria.";
            }
            else if (!Regex.IsMatch(cedula, @"^\d-\d{4}-\d{4}$"))
            {
                errores["Cedula"] = "Formato no válido. Ejemplo: 1-1234-5678.";
            }
            else if (datos.ExisteCedula(cedula, miembro.IdMiembro))
            {
                errores["Cedula"] = "Ya existe un miembro con esa cédula.";
            }

            // Nombre y apellidos: obligatorios y con largo máximo
            if (nombre == "")
            {
                errores["Nombre"] = "El nombre es obligatorio.";
            }
            else if (nombre.Length > 50)
            {
                errores["Nombre"] = "El nombre no puede tener más de 50 caracteres.";
            }

            if (apellidos == "")
            {
                errores["Apellidos"] = "Los apellidos son obligatorios.";
            }
            else if (apellidos.Length > 80)
            {
                errores["Apellidos"] = "Los apellidos no pueden tener más de 80 caracteres.";
            }

            // Fecha de nacimiento: no futura y edad dentro del rango permitido
            if (miembro.FechaNacimiento.Date > DateTime.Today)
            {
                errores["FechaNacimiento"] = "La fecha de nacimiento no puede ser futura.";
            }
            else if (miembro.Edad < 14 || miembro.Edad > 100)
            {
                errores["FechaNacimiento"] = "La edad debe estar entre 14 y 100 años.";
            }

            // Teléfono: obligatorio y con formato 8888-8888
            if (telefono == "")
            {
                errores["Telefono"] = "El teléfono es obligatorio.";
            }
            else if (!Regex.IsMatch(telefono, @"^\d{4}-\d{4}$"))
            {
                errores["Telefono"] = "Formato no válido. Ejemplo: 8888-8888.";
            }

            // Correo: opcional, pero si se escribe debe ser válido
            if (correo != "" && !EsCorreoValido(correo))
            {
                errores["Correo"] = "El correo electrónico no es válido.";
            }

            return errores;
        }

        // Valida y guarda. Devuelve los errores; si no hay ninguno, el miembro quedó guardado.
        public Dictionary<string, string> Guardar(Miembro miembro)
        {
            Dictionary<string, string> errores = Validar(miembro);
            if (errores.Count > 0)
            {
                return errores;
            }

            miembro.Cedula = miembro.Cedula.Trim();
            miembro.Nombre = miembro.Nombre.Trim();
            miembro.Apellidos = miembro.Apellidos.Trim();
            miembro.Telefono = miembro.Telefono.Trim();
            miembro.Correo = miembro.Correo.Trim();

            if (miembro.IdMiembro == 0)
            {
                miembro.IdMiembro = datos.Insertar(miembro);
            }
            else if (datos.Actualizar(miembro) == 0)
            {
                throw new InvalidOperationException("El miembro ya no existe en la base de datos.");
            }

            return errores;
        }

        public void Eliminar(int idMiembro)
        {
            if (datos.Eliminar(idMiembro) == 0)
            {
                throw new InvalidOperationException("El miembro ya no existe en la base de datos.");
            }
        }

        private bool EsCorreoValido(string correo)
        {
            try
            {
                MailAddress direccion = new MailAddress(correo);
                return direccion.Address == correo;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}