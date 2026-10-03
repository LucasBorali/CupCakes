namespace CupCakes.Entities
{
    public class Produto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public string? ImagemUrl { get; set; }

        public int Estoque { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
