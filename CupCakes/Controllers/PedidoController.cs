using System.Security.Claims;
using CupCakes.Data;
using CupCakes.DTOs;
using CupCakes.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupCakes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CriarPedidoDto dto)
        {
            if (dto.Itens.Count == 0)
            {
                return BadRequest(new
                {
                    mensagem = "O pedido precisa ter pelo menos um item."
                });
            }

            var usuarioIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (!Guid.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return Unauthorized();
            }

            var pedido = new Pedido
            {
                UsuarioId = usuarioId
            };

            foreach (var itemDto in dto.Itens)
            {
                if (itemDto.Quantidade <= 0)
                {
                    return BadRequest(new
                    {
                        mensagem = "A quantidade dos produtos deve ser maior que zero."
                    });
                }

                var produto = await _context.Produtos
                    .FirstOrDefaultAsync(x =>
                        x.Id == itemDto.ProdutoId &&
                        x.Ativo);

                if (produto == null)
                {
                    return BadRequest(new
                    {
                        mensagem = "Um dos produtos não está disponível."
                    });
                }

                if (produto.Estoque < itemDto.Quantidade)
                {
                    return BadRequest(new
                    {
                        mensagem = $"Estoque insuficiente para o produto '{produto.Nome}'."
                    });
                }

                var itemPedido = new ItemPedido
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    ValorUnitario = produto.Preco
                };

                pedido.Itens.Add(itemPedido);

                produto.Estoque -= itemDto.Quantidade;

                pedido.ValorTotal +=
                    produto.Preco * itemDto.Quantidade;
            }

            _context.Pedidos.Add(pedido);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Pedido criado com sucesso.",
                pedidoId = pedido.Id,
                valorTotal = pedido.ValorTotal,
                status = pedido.Status
            });
        }
    }
}