namespace apicooperativa.Models
{
    public class Auditoria
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string TablaAfectada { get; set; }
        public int RegistroId { get; set; }
        public string Ip { get; set; }
        public DateTime CreadoEn { get; set; }
    }
}
