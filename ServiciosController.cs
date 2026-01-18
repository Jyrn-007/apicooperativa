using apicooperativa.Data;
using apicooperativa.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace apicooperativa
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly MySqlContext _db;

        public ServiciosController(MySqlContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lista = new List<Servicio>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM servicios", conn);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(new Servicio
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Descripcion = rd.GetString("descripcion"),
                    Icono = rd.GetString("icono"),
                    Activo = rd.GetBoolean("activo")
                });
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Create(Servicio s)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"INSERT INTO servicios 
                        (nombre, descripcion, icono, activo)
                        VALUES (@n,@d,@i,@a)";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", s.Nombre);
            cmd.Parameters.AddWithValue("@d", s.Descripcion);
            cmd.Parameters.AddWithValue("@i", s.Icono);
            cmd.Parameters.AddWithValue("@a", s.Activo);

            cmd.ExecuteNonQuery();
            return Ok("Servicio creado");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Servicio s)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"UPDATE servicios SET
                        nombre=@n, descripcion=@d, icono=@i, activo=@a
                        WHERE id=@id";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", s.Nombre);
            cmd.Parameters.AddWithValue("@d", s.Descripcion);
            cmd.Parameters.AddWithValue("@i", s.Icono);
            cmd.Parameters.AddWithValue("@a", s.Activo);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            return Ok("Servicio actualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM servicios WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return Ok("Servicio eliminado");
        }
    }
}
