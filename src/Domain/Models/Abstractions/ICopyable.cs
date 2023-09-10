namespace Journal.Domain.Models.Abstractions
{
    public interface ICopyable<T>
    {
        T Copy();
    }
}