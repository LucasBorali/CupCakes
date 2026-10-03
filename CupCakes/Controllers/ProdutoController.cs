using CupCakes.Data;
using CupCakes.DTOs;
using CupCakes.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupCakes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdutoResponseDto>>> GetAll()
        {
            var produto = await _context.Produtos
                .Where(x => x.Ativo)
                .Select(x => new ProdutoResponseDto
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Preco = x.Preco,
                    ImagemUrl = x.ImagemUrl,
                    Estoque = x.Estoque,
                    Ativo = x.Ativo

                })
                .ToListAsync();

            if (produto is null)
                return NotFound();

            return Ok(produto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoResponseDto>> GetById(Guid id)
        {
            var produto = await _context.Produtos
                .Where(x => x.Id == id)
                .Select(x => new ProdutoResponseDto
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Descricao = x.Descricao,
                    Preco = x.Preco,
                    ImagemUrl = x.ImagemUrl,
                    Estoque = x.Estoque,
                    Ativo = x.Ativo
                })
                .FirstOrDefaultAsync();

            if (produto is null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateProdutoDto dto)
        {
            var produto = new Produto
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                ImagemUrl = dto.ImagemUrl,
                Estoque = dto.Estoque
            };

            _context.Produtos.Add(produto);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = produto.Id },
                produto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(
    Guid id,
    UpdateProdutoDto dto)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (produto is null)
                return NotFound();

            produto.Nome = dto.Nome;
            produto.Descricao = dto.Descricao;
            produto.Preco = dto.Preco;
            produto.ImagemUrl = dto.ImagemUrl;
            produto.Estoque = dto.Estoque;
            produto.Ativo = dto.Ativo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (produto is null)
                return NotFound();

            produto.Ativo = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}

