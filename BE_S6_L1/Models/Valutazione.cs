namespace BE_S6_L1.Models
{
    public class Valutazione
    {
        public int Id { get; set; }
        public int StudenteId { get; set; }
        public string Materia { get; set; }
        public decimal Voto { get; set; }
        public DateTime DataValutazione { get; set; }
        public virtual Studente Studente { get; set; }
    }
}
