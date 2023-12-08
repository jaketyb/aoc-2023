using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;

Part1();
Part2();

void Part1()
{
    var result = ReadSchematic();

    var validPartNumbers = new List<int>();
    foreach (var potentialPartNumber in result.potentialPartNumbers)
    {
        if (
            potentialPartNumber.ValidSymbolPositions.Any(
                vsp =>
                    result.symbols.Select(s => s.Coords).Contains(vsp, new CoordsEqualityComparer())
            )
        )
        {
            validPartNumbers.Add(potentialPartNumber.PartNumber);
        }
    }

    Console.WriteLine(validPartNumbers.Aggregate((x, y) => x + y));
}

void Part2()
{
    var result = ReadSchematic();

    var gearRatio = 0;

    foreach (var symbol in result.symbols)
    {
        if (symbol.Symbol != '*')
            continue;

        var matchingPartNumbers = result.potentialPartNumbers
            .Where(
                pn => pn.ValidSymbolPositions.Contains(symbol.Coords, new CoordsEqualityComparer())
            )
            .ToArray();

        if (matchingPartNumbers.Length == 2)
        {
            gearRatio += matchingPartNumbers[0].PartNumber * matchingPartNumbers[1].PartNumber;
        }
    }

    Console.WriteLine(gearRatio);
}

(List<PotentialPartNumber> potentialPartNumbers, List<SchematicSymbol> symbols) ReadSchematic()
{
    var schematicLines = File.ReadAllLines("input.txt");

    var potentialPartNumbers = new List<PotentialPartNumber>();
    var symbols = new List<SchematicSymbol>();

    for (var i = 0; i < schematicLines.Length; i++)
    {
        var schematicLine = schematicLines[i];
        var buildingNumber = "";

        for (var j = 0; j < schematicLine.Length; j++)
        {
            var schematicChar = schematicLine[j];

            if (char.IsNumber(schematicChar))
            {
                // start building up a number
                buildingNumber += schematicChar;
                continue;
            }

            if (buildingNumber.Length > 0)
            {
                // store the number into an array of potential part numbers
                potentialPartNumbers.Add(BuildPotentialPartNumber(buildingNumber, j, i));

                buildingNumber = "";
            }

            if (schematicChar != '.')
            {
                symbols.Add(
                    new SchematicSymbol
                    {
                        Symbol = schematicChar,
                        Coords = new Coords { X = j, Y = i }
                    }
                );
            }
        }

        if (buildingNumber.Length > 0)
        {
            // store the number into an array of potential part numbers
            potentialPartNumbers.Add(
                BuildPotentialPartNumber(buildingNumber, schematicLine.Length, i)
            );

            buildingNumber = "";
        }
    }

    return (potentialPartNumbers, symbols);
}

PotentialPartNumber BuildPotentialPartNumber(
    string buildingNumber,
    int currentXPos,
    int currentYPos
)
{
    var buildingValidSymbolPositions = new List<Coords>();
    for (var k = 0; k < buildingNumber.Length; k++)
    {
        buildingValidSymbolPositions.AddRange(
            new[]
            {
                new Coords { X = currentXPos - k - 1, Y = currentYPos - 1 },
                new Coords { X = currentXPos - k - 1, Y = currentYPos + 1 }
            }
        );
    }

    buildingValidSymbolPositions.AddRange(
        new[]
        {
            new Coords { X = currentXPos - buildingNumber.Length - 1, Y = currentYPos - 1 },
            new Coords { X = currentXPos - buildingNumber.Length - 1, Y = currentYPos },
            new Coords { X = currentXPos - buildingNumber.Length - 1, Y = currentYPos + 1 },
            new Coords { X = currentXPos, Y = currentYPos - 1 },
            new Coords { X = currentXPos, Y = currentYPos },
            new Coords { X = currentXPos, Y = currentYPos + 1 }
        }
    );

    // store the number into an array of potential part numbers
    return new PotentialPartNumber
    {
        PartNumber = int.Parse(buildingNumber),
        ValidSymbolPositions = buildingValidSymbolPositions
    };
}

class CoordsEqualityComparer : IEqualityComparer<Coords>
{
    public bool Equals(Coords? x, Coords? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return x.X == y.X && x.Y == y.Y;
    }

    public int GetHashCode(Coords obj) => obj.X ^ obj.Y;
}

class PotentialPartNumber
{
    public int PartNumber { get; set; }
    public List<Coords> ValidSymbolPositions { get; set; } = new List<Coords>();
}

class SchematicSymbol
{
    public char Symbol { get; set; }
    public Coords Coords { get; set; }
}

class Coords
{
    public int X { get; set; }
    public int Y { get; set; }
}
