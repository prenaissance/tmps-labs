namespace Journal.Domain.Models.Abstractions
{
    public abstract record Entity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}