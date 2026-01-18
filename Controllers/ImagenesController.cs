using apicooperativa.Data;
using apicooperativa.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace apicooperativa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesController : ControllerBase
    {
        private readonly MySqlContext _db;

        public ImagenesController(MySqlContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lista = new List<Imagen>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM imagenes", conn);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(new Imagen
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Url = rd.GetString("url"),
                    Tipo = rd.GetString("tipo")
                });
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Create(Imagen i)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = "INSERT INTO imagenes (nombre, url, tipo) VALUES (@n,@u,@t)";
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", i.Nombre);
            cmd.Parameters.AddWithValue("@u", i.Url);
            cmd.Parameters.AddWithValue("@t", i.Tipo);

            cmd.ExecuteNonQuery();
            return Ok("Imagen creada");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM imagenes WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return Ok("Imagen eliminada");
        }
    }
}
