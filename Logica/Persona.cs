namespace GymControl.Logica
{
    // Clase base: guarda los datos que comparten miembros y entrenadores
    public class Persona
    {
        private string cedula = string.Empty;
        private string nombre = string.Empty;
        private string apellidos = string.Empty;
        private string telefono = string.Empty;

        public string Cedula
        {
            get { return cedula; }
            set { cedula = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Apellidos
        {
            get { return apellidos; }
            set { apellidos = value; }
        }

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        // Valor calculado: no se guarda en la base de datos
        public string NombreCompleto
        {
            get { return nombre + " " + apellidos; }
        }
    }
}