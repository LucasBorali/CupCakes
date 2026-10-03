namespace CupCakes.Entities
{
    public class Endereco
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = null!;

        public string Rua { get; set; } = string.Empty;

        public string Numero { get; set; } = string.Empty;

        public string Cidade { get; set; } = string.Empty;

        public string Cep { get; set; } = string.Empty;

        public string? Complemento { get; set; }
    }
}
