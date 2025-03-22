using System.Globalization;

namespace Perceptron;

public class Ui
{
    private readonly List<string> _allLabels;
    private readonly Trainer _trainer;
    private readonly Perceptron _perceptron;

    private Ui(string trainPath, string testPath, double learningRate)
    {
        var trainData = ParseFile(trainPath);
        _allLabels = trainData.Select(key => key.Item2).Distinct().ToList();
        if (_allLabels.Count == 0)
        {
            throw new ArgumentException("File is empty");
        }
        _trainer = new Trainer(trainData, ParseFile(testPath));
        _perceptron = new Perceptron(trainData[0].Item1.Count, learningRate);
    }
    
    private static List<Tuple<IList<double>, string>> ParseFile(string path)
    {
        return File.ReadAllLines(path).Select(line =>
        {
            var splLine = line.Split(";");
            var data = new List<double>();
            for (var i = 0; i < splLine.Length - 1; ++i)
            {
                data.Add(double.Parse(splLine[i], CultureInfo.InvariantCulture));
            }
            return new Tuple<IList<double>, string>(data, splLine[^1]);
        }).ToList();
    }

    private void TrainPerceptron() => _trainer.RetrainPerceptron(_allLabels[0], _perceptron);
    
    private void CheckPerceptron(string vector) => Trainer.CheckPerceptron(vector.Split(";").Select(
        val => double.Parse(val, CultureInfo.InvariantCulture)).ToArray(), _allLabels[0], _perceptron);
    
    public static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            throw new ArgumentException("Usage: Ui <a> <trainSet.csv> <testSet.csv>");
        }
        if (!File.Exists(args[1]) || !File.Exists(args[2]))
        {
            throw new FileNotFoundException("Specified paths don't exist!");
        }
        var ui = new Ui(args[1], args[2], double.Parse(args[0], CultureInfo.InvariantCulture));
        ui.TrainPerceptron();
        while (true)
        {
            Console.Write("=> ");
            var line = Console.ReadLine();
            if (line is "" or null) break;
            switch (line.ToLower())
            {
                case "r" or "retrain":
                    ui.TrainPerceptron();
                    break;
                default:
                    try { ui.CheckPerceptron(line); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                    break;
            }
        }
    }
}