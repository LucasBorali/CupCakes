namespace CupCakes.Entities
{
    public class Pedido
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = null!;

        public DateTime DataPedido { get; set; } = DateTime.UtcNow;

        public decimal ValorTotal { get; set; }

        public string Status { get; set; } = "Recebido";

        public List<ItemPedido> Itens { get; set; } = [];
    }
}
