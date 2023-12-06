using System.Runtime.InteropServices;

var result = 1;
foreach(var race in ParseRacesPart2()) {

    var numberOfWaysToBeat = 0;
    for(var i = 0; i < race.Time; i++) {

        var totalDistance = i * (race.Time - i);

        if (totalDistance > race.Record) {
            numberOfWaysToBeat++;
        }
    }

    result *= numberOfWaysToBeat;
}

Console.WriteLine(result);



IEnumerable<Race> ParseRacesPart1(){
    // var inputLines = File.ReadAllLines("input.txt");

    // var times = new List<int>();
    // var distances = new List<int>();
    // foreach(var line in inputLines) {
    //     var segments = line.Split(' ').Where(s => s.Trim().Length > 0).ToArray();

    //     if (segments[0] == "Time:") {
    //         foreach
    //     }
    // }

    return new List<Race>(){
        new Race{Time = 42, Record = 284},
        new Race{Time = 68, Record = 1005},
        new Race{Time = 69, Record = 1122},
        new Race{Time = 85, Record = 1341}
    };
}

IEnumerable<Race> ParseRacesPart2(){
    // var inputLines = File.ReadAllLines("input.txt");

    // var times = new List<int>();
    // var distances = new List<int>();
    // foreach(var line in inputLines) {
    //     var segments = line.Split(' ').Where(s => s.Trim().Length > 0).ToArray();

    //     if (segments[0] == "Time:") {
    //         foreach
    //     }
    // }

    return new List<Race>(){
        new Race{Time = 42686985, Record = 284100511221341},
    };
}

class Race
{
    public long Time { get; set; }
    public long Record { get; set; }
}
