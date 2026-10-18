namespace GymControl.Logica
{
    // Miembro HEREDA de Persona (": Persona")
    public class Miembro : Persona
    {
        private int idMiembro;
        private DateTime fechaNacimiento;
        private string correo = string.Empty;
        private DateTime fechaRegistro;
        private bool activo = true;

        public int IdMiembro
        {
            get { return idMiembro; }
            set { idMiembro = value; }
        }

        public DateTime FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }

        public DateTime FechaRegistro
        {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        // Valor calculado: edad a partir de la fecha de nacimiento
        public int Edad
        {
            get
            {
                DateTime hoy = DateTime.Today;
                int edad = hoy.Year - fechaNacimiento.Year;
                if (fechaNacimiento.Date > hoy.AddYears(-edad))
                {
                    edad--; // todavía no ha cumplido años este año
                }
                return edad;
            }
        }
    }
}