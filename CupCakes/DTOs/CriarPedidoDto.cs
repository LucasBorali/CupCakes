namespace CupCakes.DTOs
{
    public class CriarPedidoDto
    {
        public List<CriarItemPedidoDto> Itens { get; set; } = new();
    }

    public class CriarItemPedidoDto
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
