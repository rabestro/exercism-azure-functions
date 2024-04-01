namespace Exercism.Solution;

public static class ScrabbleScore
{
    public static int Score(string input) => input.ToUpper().Select(Score).Sum();

    private static int Score(char letter) =>
        letter switch
        {
            'Q' or 'Z' => 10,
            'J' or 'X' => 8,
            'K' => 5,
            'F' or 'H' or 'V' or 'W' or 'Y' => 4,
            'B' or 'C' or 'M' or 'P' => 3,
            'D' or 'G' => 2,
            _ => 1
        };
}