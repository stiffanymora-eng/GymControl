namespace GymControl.Logica
{
    // Plan que ofrece el gimnasio (Mensual, Trimestral, etc.)
    public class TipoMembresia
    {
        private int idTipoMembresia;
        private string nombre = string.Empty;
        private int duracionDias;
        private decimal precio;

        public int IdTipoMembresia
        {
            get { return idTipoMembresia; }
            set { idTipoMembresia = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public int DuracionDias
        {
            get { return duracionDias; }
            set { duracionDias = value; }
        }

        public decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }
    }
}