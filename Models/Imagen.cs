namespace apicooperativa.Models
{
    public class Imagen
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public string Tipo { get; set; } // Carrusel | Servicio | Sucursal | Galeria
    }
}
