using System.Data;

var cache = new Dictionary<string, long>();

var records = File.ReadAllLines("input.txt");

long sum = 0;

foreach (var recordLine in records)
{
    var springs = recordLine.Substring(0, recordLine.IndexOf(' '));

    var damageGroupInfo = recordLine.Substring(recordLine.IndexOf(' ') + 1);

    for (var i = 0; i < 4; i++)
    {
        springs += "?" + recordLine.Substring(0, recordLine.IndexOf(' '));
        damageGroupInfo += "," + recordLine.Substring(recordLine.IndexOf(' ') + 1);
    }

    var damageGroups = damageGroupInfo.Split(',').Select(int.Parse).ToArray();

    sum += CountVariations(springs, damageGroups);

    Console.WriteLine($"Running total: {sum}");
}

Console.WriteLine("-----");
Console.WriteLine(sum);

long CountVariations(string record, int[] damageGroups)
{
    if (record == "")
    {
        return damageGroups.Length == 0 ? 1 : 0;
    }

    if (damageGroups.Length == 0)
    {
        return record.Contains('#') ? 0 : 1;
    }

    var cacheKey = record + string.Join(",", damageGroups.Select(dg => dg.ToString()));

    if (cache.ContainsKey(cacheKey))
    {
        return cache[cacheKey];
    }

    long result = 0;

    // Treat unknowns as damaged springs
    if (record[0] == '?' || record[0] == '#')
    {
        if (
            damageGroups[0] <= record.Length
            && !record.Substring(0, damageGroups[0]).Contains('.')
            && (damageGroups[0] == record.Length || record[damageGroups[0]] != '#')
        )
        {
            var slicedRecord =
                damageGroups[0] + 1 > record.Length ? "" : record.Substring(damageGroups[0] + 1);
            result += CountVariations(slicedRecord, damageGroups.Skip(1).ToArray());
        }
    }

    // Treat unknowns as undamaged springs
    if (record[0] == '?' || record[0] == '.')
    {
        result += CountVariations(record.Substring(1), damageGroups);
    }

    cache.Add(cacheKey, result);

    return result;
}
