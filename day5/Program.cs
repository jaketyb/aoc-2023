using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Text.Encodings.Web;

var almanacInput = File.ReadAllLines("almanac.txt");
var seedsInput = File.ReadAllLines("seeds.txt");

var almanac = ParseAlmanac(almanacInput);
var reversedAlmanac = almanac.Reverse();

var seeds = ParseSeedsPart1(seedsInput[0]);

Part2();

// Could we function as part1, but when we look through maps keep track of the smallest different between current source and upper source bound
// Return the smallest diff along with seedLocation
// Skip the next x number of seeds, where x = smallestDiff

void Part1()
{
    long? minLocation = null;

    foreach (var seed in seeds)
    {
        // Console.WriteLine($"Seed: {seed}");
        var seedLocation = GetLocationForSeed(seed);
        // Console.WriteLine("");
        if (minLocation == null || seedLocation < minLocation)
        {
            minLocation = seedLocation;
        }
    }
    Console.WriteLine(minLocation);
}

void Part2Force()
{
    var part2Seeds = ParseSeedsPart2(seedsInput[0]);

    foreach (var seedRange in part2Seeds)
    {
        for (var i = seedRange.min; i <= seedRange.max; i++)
        {
            var location = GetLocationForSeed(i);

            if (location < 59370573)
            {
                Console.WriteLine($"Found location {location}");
            }
        }
    }
}

void Part2()
{
    var part1Seeds = ParseSeedsPart1(seedsInput[0]);
    var part2Seeds = ParseSeedsPart2(seedsInput[0]);

    var location = 0;
    var found = false;
    while (!found)
    {
        var seed = GetSeedForLocation(location);

        //Does seed exist?
        if (part2Seeds.Any(tup => tup.min <= seed && tup.max >= seed))
        {
            found = true;
        }
        else
        {
            location++;
        }
    }

    Console.WriteLine(location);
}

IEnumerable<long> ParseSeedsPart1(string seeds)
{
    return seeds.Split(' ').Select(s => long.Parse(s));
}

IEnumerable<(long min, long max)> ParseSeedsPart2(string seeds)
{
    var returnVal = new List<(long min, long max)>();

    var splitSeeds = seeds.Split(' ');
    for (var i = 0; i < splitSeeds.Length; i += 2)
    {
        var min = long.Parse(splitSeeds[i]);
        var max = min + (long.Parse(splitSeeds[i + 1]) - 1);
        returnVal.Add((min, max));
    }

    return returnVal;
}

IEnumerable<IEnumerable<MapValue>> ParseAlmanac(string[] inputLines)
{
    var almanacResult = new List<List<MapValue>>();

    var currentMap = new List<MapValue>();
    foreach (var line in inputLines)
    {
        if (line.Trim().Length == 0 && currentMap.Count > 0)
        {
            almanacResult.Add(currentMap);
            currentMap = new List<MapValue>();
            continue;
        }

        if (char.IsNumber(line[0]))
        {
            var mapValue = ParseMapValueLine(line);
            currentMap.Add(mapValue);
        }
    }

    almanacResult.Add(currentMap);

    return almanacResult;
}

MapValue ParseMapValueLine(string line)
{
    var segments = line.Split(' ');
    return new MapValue(long.Parse(segments[0]), long.Parse(segments[1]), long.Parse(segments[2]));
}

long GetSeedForLocation(long location)
{
    var source = location;
    foreach (var maps in reversedAlmanac)
    {
        source = GetSourceFromDestination(source, maps);
    }
    return source;
}

long GetSourceFromDestination(long destination, IEnumerable<MapValue> mapValues)
{
    var targetMap = mapValues.FirstOrDefault(
        mp => mp.DestinationStart <= destination && mp.Range + mp.DestinationStart >= destination
    );

    if (targetMap == null)
    {
        return destination;
    }

    return targetMap.SourceStart + (destination - targetMap.DestinationStart);
}

long GetLocationForSeed(long seed)
{
    var destination = seed;
    foreach (var maps in almanac)
    {
        destination = GetDestinationFromSource(destination, maps);
    }
    return destination;
}

long GetDestinationFromSource(long source, IEnumerable<MapValue> mapValues)
{
    var targetMap = mapValues.FirstOrDefault(
        mp => mp.SourceStart <= source && mp.Range + mp.SourceStart >= source
    );

    if (targetMap == null)
    {
        return source;
    }

    return targetMap.DestinationStart + (source - targetMap.SourceStart);
}
