using apicooperativa.Models;
using BCrypt.Net;
using apicooperativa.Data;
using apicooperativa.DTOs;
using Microsoft.AspNetCore.Mvc;

using MySql.Data.MySqlClient;

using Org.BouncyCastle.Crypto.Generators;

namespace LoginController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly MySqlContext _db;

        public LoginController(MySqlContext db)
        {
            _db = db;
        }

        // 🔹 LISTAR USUARIOS
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = new List<usuarios>();
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = "SELECT * FROM usuarios";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new usuarios
                {
                    Id = reader.GetInt32("id"),
                    Nombre = reader.GetString("nombre"),
                    UsuarioLogin = reader.GetString("usuario_login"),
                    PasswordHash = reader.GetString("password_hash"),
                    Rol = reader.GetString("rol"),
                    Activo = reader.GetInt32("activo")
                });
            }
            return Ok(lista);
        }

        // 🔹 OBTENER POR ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = "SELECT * FROM usuarios WHERE id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return NotFound();

            return Ok(new usuarios
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                UsuarioLogin = reader.GetString("usuario_login"),
                Rol = reader.GetString("rol"),
                Activo = reader.GetInt32("activo")
            });
        }

        // 🔹 LOGIN
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = "SELECT * FROM usuarios WHERE usuario_login=@user AND activo=1";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@user", dto.Usuario);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return Unauthorized("Usuario no existe");

            var hash = reader.GetString("password_hash");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, hash))
                return Unauthorized("Credenciales incorrectas");

            return Ok(new
            {
                mensaje = "Login correcto",
                usuario = reader.GetString("usuario_login"),
                rol = reader.GetString("rol")
            });
        }

        // 🔹 CREAR USUARIO
        [HttpPost]
        public IActionResult Create([FromBody] usuarios u)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"INSERT INTO usuarios
                        (nombre, usuario_login, password_hash, rol, activo)
                        VALUES (@n,@u,@p,@r,1)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", u.Nombre);
            cmd.Parameters.AddWithValue("@u", u.UsuarioLogin);
            cmd.Parameters.AddWithValue("@p", BCrypt.Net.BCrypt.HashPassword(u.PasswordHash));
            cmd.Parameters.AddWithValue("@r", u.Rol);

            cmd.ExecuteNonQuery();
            return Ok("Usuario creado");
        }

        // 🔹 ACTUALIZAR
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] usuarios u)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = @"UPDATE usuarios SET
                        nombre=@n,
                        rol=@r,
                        activo=@a
                        WHERE id=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", u.Nombre);
            cmd.Parameters.AddWithValue("@r", u.Rol);
            cmd.Parameters.AddWithValue("@a", u.Activo);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            return Ok("Usuario actualizado");
        }

        // 🔹 ELIMINAR
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var sql = "DELETE FROM usuarios WHERE id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
            return Ok("Usuario eliminado");
        }
    }
}
