using apicooperativa.Data;
using apicooperativa.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace apicooperativa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarruselController : ControllerBase
    {
        private readonly MySqlContext _db;

        public CarruselController(MySqlContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lista = new List<Carrusel>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM carrusel", conn);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(new Carrusel
                {
                    Id = rd.GetInt32("id"),
                    Titulo = rd.GetString("titulo"),
                    ImagenUrl = rd.GetString("imagen_url"),
                    Activo = rd.GetBoolean("activo")
                });
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Create(Carrusel c)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = "INSERT INTO carrusel (titulo, imagen_url, activo) VALUES (@t,@i,@a)";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@t", c.Titulo);
            cmd.Parameters.AddWithValue("@i", c.ImagenUrl);
            cmd.Parameters.AddWithValue("@a", c.Activo);

            cmd.ExecuteNonQuery();
            return Ok("Carrusel creado");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Carrusel c)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"UPDATE carrusel 
                        SET titulo=@t, imagen_url=@i, activo=@a 
                        WHERE id=@id";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@t", c.Titulo);
            cmd.Parameters.AddWithValue("@i", c.ImagenUrl);
            cmd.Parameters.AddWithValue("@a", c.Activo);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            return Ok("Carrusel actualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM carrusel WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return Ok("Carrusel eliminado");
        }
    }
}
