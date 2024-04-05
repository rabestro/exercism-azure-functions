namespace Exercism.Solution;

public static class Grains
{
    private const int MinSquareNumber = 1;
    private const int MaxSquareNumber = 64;

    public static ulong Square(int n)
    {
        return n is < MinSquareNumber or > MaxSquareNumber
            ? throw new ArgumentOutOfRangeException(nameof(n),
                $"Input must be within the range of {MinSquareNumber} and {MaxSquareNumber} inclusive.")
            : 1ul << --n;
    }

    public static ulong Total()
    {
        return ~0ul;
    }
}