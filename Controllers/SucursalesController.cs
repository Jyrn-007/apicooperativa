using apicooperativa.Data;
using apicooperativa.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace apicooperativa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SucursalesController : ControllerBase
    {
        private readonly MySqlContext _db;

        public SucursalesController(MySqlContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var lista = new List<Sucursal>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM sucursales", conn);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(new Sucursal
                {
                    Id = rd.GetInt32("id"),
                    Nombre = rd.GetString("nombre"),
                    Direccion = rd.GetString("direccion"),
                    Telefono = rd.GetString("telefono"),
                    Activo = rd.GetBoolean("activo")
                });
            }
            return Ok(lista);
        }

        [HttpPost]
        public IActionResult Create(Sucursal s)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"INSERT INTO sucursales 
                        (nombre, direccion, telefono, activo)
                        VALUES (@n,@d,@t,@a)";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", s.Nombre);
            cmd.Parameters.AddWithValue("@d", s.Direccion);
            cmd.Parameters.AddWithValue("@t", s.Telefono);
            cmd.Parameters.AddWithValue("@a", s.Activo);

            cmd.ExecuteNonQuery();
            return Ok("Sucursal creada");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Sucursal s)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"UPDATE sucursales SET
                        nombre=@n, direccion=@d, telefono=@t, activo=@a
                        WHERE id=@id";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", s.Nombre);
            cmd.Parameters.AddWithValue("@d", s.Direccion);
            cmd.Parameters.AddWithValue("@t", s.Telefono);
            cmd.Parameters.AddWithValue("@a", s.Activo);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            return Ok("Sucursal actualizada");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM sucursales WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            return Ok("Sucursal eliminada");
        }
    }
}
