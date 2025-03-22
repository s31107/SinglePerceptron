namespace Perceptron;

public class Trainer(List<Tuple<IList<double>, string>> trainData, List<Tuple<IList<double>, string>> testData)
{
    private readonly Random _random = new();

    private void Train(string currentLabel, Perceptron perceptron)
    {
        var data = trainData.OrderBy(_ => _random.Next()).ToList();
        var arrayData = new double[data.Count][];
        var arrayResults = new bool[data.Count];
        for (var iter = 0; iter < data.Count; ++iter)
        {
            arrayData[iter] = data[iter].Item1.ToArray();
            arrayResults[iter] = data[iter].Item2 == currentLabel;
        } perceptron.Learn(arrayData, arrayResults);
    }
    
    public void RetrainPerceptron(string currentLabel, Perceptron perceptron)
    {
        long allGoodAnswers = 0;
        long classifyDataGoodAnswers = 0;
        long restDataGoodAnswers = 0;
        Train(currentLabel, perceptron);
        testData.ForEach(test =>
        {
            var perceptronReturn = perceptron.Compute(test.Item1.ToArray());
            var expectedResult = currentLabel == test.Item2;
            Console.WriteLine($"Expected: {(expectedResult ? "" : "Not ")}{currentLabel} , Actual: {
                (perceptronReturn ? "" : "Not ")}{currentLabel}");
            if (perceptronReturn != expectedResult) return;
            ++allGoodAnswers;
            if (expectedResult) ++classifyDataGoodAnswers;
            else ++restDataGoodAnswers;
        });
        long classifyLabelNumber = testData.Select(test => test.Item2 == currentLabel ? 1 : 0).Sum();
        Console.WriteLine($"acc = {(double)allGoodAnswers / testData.Count}");
        Console.WriteLine($"flower1 acc = {(double)classifyDataGoodAnswers / classifyLabelNumber}");
        Console.WriteLine($"flower2 acc = {(double)restDataGoodAnswers / (testData.Count - classifyLabelNumber)}");
    }
    
    public static void CheckPerceptron(double[] vector, string currentLabel, Perceptron perceptron)
    {
        Console.WriteLine($"{(perceptron.Compute(vector) ? "" : "Not ")}{currentLabel}");
    }
}