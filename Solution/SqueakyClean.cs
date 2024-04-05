namespace Exercism.Solution;

using System.Text;

public static class Identifier
{
    public static string Clean(string identifier)
    {
        var result = new StringBuilder();
        var isAfterDash = false;

        foreach (var symbol in identifier)
        {   
            result.Append(symbol switch
            {
                >= 'α' and <= 'ω' => default,
                _ when isAfterDash => char.ToUpperInvariant(symbol),
                _ when char.IsWhiteSpace(symbol) => '_',
                _ when char.IsControl(symbol) => "CTRL",
                _ when char.IsLetter(symbol) => symbol,
                _ => default
            });
            isAfterDash = symbol.Equals('-');
        }

        return result.ToString();
    }
}
