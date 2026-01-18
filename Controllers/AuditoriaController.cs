using apicooperativa.Data;
using apicooperativa.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace apicooperativa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly MySqlContext _db;

        public AuditoriaController(MySqlContext db)
        {
            _db = db;
        }

        // 🔹 LISTAR TODA LA AUDITORÍA
        [HttpGet]
        public IActionResult Get()
        {
            var lista = new List<Auditoria>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM auditoria ORDER BY creado_en DESC", conn);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new Auditoria
                {
                    Id = rd.GetInt32("id"),
                    Usuario = rd.GetString("usuario"),
                    Accion = rd.GetString("accion"),
                    TablaAfectada = rd.GetString("tabla_afectada"),
                    RegistroId = rd.GetInt32("registro_id"),
                    Ip = rd.GetString("ip"),
                    CreadoEn = rd.GetDateTime("creado_en")
                });
            }
            return Ok(lista);
        }

        // 🔹 FILTRAR POR USUARIO
        [HttpGet("usuario/{usuario}")]
        public IActionResult GetByUsuario(string usuario)
        {
            var lista = new List<Auditoria>();
            using var conn = _db.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(
                "SELECT * FROM auditoria WHERE usuario=@u", conn);
            cmd.Parameters.AddWithValue("@u", usuario);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                lista.Add(new Auditoria
                {
                    Id = rd.GetInt32("id"),
                    Usuario = rd.GetString("usuario"),
                    Accion = rd.GetString("accion"),
                    TablaAfectada = rd.GetString("tabla_afectada"),
                    RegistroId = rd.GetInt32("registro_id"),
                    Ip = rd.GetString("ip"),
                    CreadoEn = rd.GetDateTime("creado_en")
                });
            }
            return Ok(lista);
        }
    }
}
