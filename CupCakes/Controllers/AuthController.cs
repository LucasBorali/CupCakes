using CupCakes.Data;
using CupCakes.DTOs;
using CupCakes.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupCakes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(RegistrarDto dto)
        {
            var emailExiste = await _context.Usuarios
                .AnyAsync(x => x.Email == dto.Email);

            if (emailExiste)
            {
                return BadRequest(new
                {
                    mensagem = "Este email já está cadastrado."
                });
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                Role = "Cliente"
            };

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Usuário cadastrado com sucesso."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (usuario == null)
            {
                return Unauthorized(new
                {
                    mensagem = "Email ou senha inválidos."
                });
            }

            var senhaValida = BCrypt.Net.BCrypt.Verify(
                dto.Senha,
                usuario.SenhaHash
            );

            if (!senhaValida)
            {
                return Unauthorized(new
                {
                    mensagem = "Email ou senha inválidos."
                });
            }

            return Ok(new
            {
                mensagem = "Login realizado com sucesso.",
                usuario = new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                    usuario.Role
                }
            });
        }
    }
}
