namespace apicooperativa.Models
{
    public class usuarios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string UsuarioLogin { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }
        public int Activo { get; set; }
    }
}
