namespace GymControl.Logica
{
    // Un abono realizado sobre una membresía
    public class Pago
    {
        private int idPago;
        private int idMembresia;
        private DateTime fechaPago;
        private decimal monto;
        private string metodoPago = string.Empty;

        public int IdPago
        {
            get { return idPago; }
            set { idPago = value; }
        }

        public int IdMembresia
        {
            get { return idMembresia; }
            set { idMembresia = value; }
        }

        public DateTime FechaPago
        {
            get { return fechaPago; }
            set { fechaPago = value; }
        }

        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        public string MetodoPago
        {
            get { return metodoPago; }
            set { metodoPago = value; }
        }
    }
}