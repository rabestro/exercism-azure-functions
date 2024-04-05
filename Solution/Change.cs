namespace Exercism.Solution;

public static class Change
{
    public static int[] FindFewestCoins(int[] coins, int target)
    {
        if (target < 0)
            throw new ArgumentException("Negative change not allowed");

        var changes = new Dictionary<int, List<int>> { { 0, [] } };
        var amounts = new Queue<int>();
        amounts.Enqueue(0);

        while (amounts.Count > 0)
        {
            var amount = amounts.Dequeue();
            var change = changes[amount];
            if (amount == target)
                return change.ToArray();

            foreach (var coin in coins)
            {
                var total = amount + coin;
                if (total > target)
                    break;
                if (changes.ContainsKey(total))
                    continue;
                amounts.Enqueue(total);
                changes[total] = [..change, coin];
            }
        }

        throw new ArgumentException("can't make target with given coins");
    }
}