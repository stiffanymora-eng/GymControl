namespace GymControl.Logica
{
    public class Membresia
    {
        private int idMembresia;
        private int idMiembro;
        private int idTipoMembresia;
        private int? idEntrenador;   // "int?" permite NULL: el entrenador es opcional
        private DateTime fechaInicio;
        private DateTime fechaVencimiento;
        private decimal precioAcordado;
        private List<Pago> pagos = new List<Pago>();   // colección List

        public int IdMembresia
        {
            get { return idMembresia; }
            set { idMembresia = value; }
        }

        public int IdMiembro
        {
            get { return idMiembro; }
            set { idMiembro = value; }
        }

        public int IdTipoMembresia
        {
            get { return idTipoMembresia; }
            set { idTipoMembresia = value; }
        }

        public int? IdEntrenador
        {
            get { return idEntrenador; }
            set { idEntrenador = value; }
        }

        public DateTime FechaInicio
        {
            get { return fechaInicio; }
            set { fechaInicio = value; }
        }

        public DateTime FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }

        public decimal PrecioAcordado
        {
            get { return precioAcordado; }
            set { precioAcordado = value; }
        }

        public List<Pago> Pagos
        {
            get { return pagos; }
            set { pagos = value; }
        }

        // ---------- Valores calculados (no se guardan en la base de datos) ----------

        // Días que faltan para el vencimiento (negativo si ya venció)
        public int DiasRestantes
        {
            get { return (fechaVencimiento.Date - DateTime.Today).Days; }
        }

        // Suma de todos los pagos de la membresía
        public decimal TotalPagado
        {
            get
            {
                decimal total = 0;
                foreach (Pago pago in pagos)
                {
                    total += pago.Monto;
                }
                return total;
            }
        }

        // Lo que todavía se debe
        public decimal SaldoPendiente
        {
            get { return precioAcordado - TotalPagado; }
        }

        // Activa, Por vencer (7 días o menos) o Vencida
        public string Estado
        {
            get
            {
                if (DiasRestantes < 0)
                {
                    return "Vencida";
                }
                if (DiasRestantes <= 7)
                {
                    return "Por vencer";
                }
                return "Activa";
            }
        }
    }
}