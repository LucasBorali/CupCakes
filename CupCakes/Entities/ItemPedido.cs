namespace CupCakes.Entities
{
    public class ItemPedido
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PedidoId { get; set; }

        public Pedido Pedido { get; set; } = null!;

        public Guid ProdutoId { get; set; }

        public Produto Produto { get; set; } = null!;

        public int Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }
    }
}
