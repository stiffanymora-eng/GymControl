namespace GymControl.Logica
{
    // Entrenador también hereda de Persona
    public class Entrenador : Persona
    {
        private int idEntrenador;
        private string especialidad = string.Empty;

        public int IdEntrenador
        {
            get { return idEntrenador; }
            set { idEntrenador = value; }
        }

        public string Especialidad
        {
            get { return especialidad; }
            set { especialidad = value; }
        }
    }
}