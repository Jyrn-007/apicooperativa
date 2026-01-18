namespace apicooperativa.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
        public bool Revocado { get; set; }
    }
}
