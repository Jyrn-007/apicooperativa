using apicooperativa.Data;
using apicooperativa.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace apicooperativa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshTokensController : ControllerBase
    {
        private readonly MySqlContext _db;

        public RefreshTokensController(MySqlContext db)
        {
            _db = db;
        }

        // 🔹 LISTAR (solo para debug / admin)
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = new List<RefreshToken>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM refresh_tokens", conn);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(new RefreshToken
                {
                    Id = rd.GetInt32("id"),
                    UsuarioId = rd.GetInt32("usuario_id"),
                    Token = rd.GetString("token"),
                    Expiracion = rd.GetDateTime("expiracion"),
                    Revocado = rd.GetBoolean("revocado")
                });
            }
            return Ok(lista);
        }

        // 🔹 CREAR (cuando haces login)
        [HttpPost]
        public IActionResult Create(RefreshToken rt)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"INSERT INTO refresh_tokens
                        (usuario_id, token, expiracion, revocado)
                        VALUES (@u,@t,@e,0)";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", rt.UsuarioId);
            cmd.Parameters.AddWithValue("@t", rt.Token);
            cmd.Parameters.AddWithValue("@e", rt.Expiracion);

            cmd.ExecuteNonQuery();
            return Ok("Refresh token creado");
        }

        // 🔹 REVOCAR
        [HttpPut("revocar/{id}")]
        public IActionResult Revoke(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(
                "UPDATE refresh_tokens SET revocado=1 WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            return Ok("Refresh token revocado");
        }

        // 🔹 ELIMINAR
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(
                "DELETE FROM refresh_tokens WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            return Ok("Refresh token eliminado");
        }
    }
}
