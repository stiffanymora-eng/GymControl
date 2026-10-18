namespace GymControl.Logica
{
    // Registro de una entrada de un miembro al gimnasio
    public class Asistencia
    {
        private int idAsistencia;
        private int idMiembro;
        private DateTime fechaHoraEntrada;

        public int IdAsistencia
        {
            get { return idAsistencia; }
            set { idAsistencia = value; }
        }

        public int IdMiembro
        {
            get { return idMiembro; }
            set { idMiembro = value; }
        }

        public DateTime FechaHoraEntrada
        {
            get { return fechaHoraEntrada; }
            set { fechaHoraEntrada = value; }
        }
    }
}