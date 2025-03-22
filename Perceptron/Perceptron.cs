namespace Perceptron;

public class Perceptron 
{
    private readonly double[] _weights;
    private readonly double _learningRate;
    private readonly int _dimension;

    public Perceptron(int dimension, double learningRate) 
    {
        var rand = new Random();
        _weights = new double[dimension + 1];
        for (var i = 0; i < dimension; i++) 
        {
            _weights[i] = rand.NextDouble();
        }
        _learningRate = learningRate;
        _dimension = dimension;
    }

    public void Learn(double[][] data, bool[] results) 
    {
        if (data.Length != results.Length) 
        {
            throw new ArgumentException("data and results must have the same length");
        }
        for (var iter = 0; iter < data.Length; iter++) 
        {
            var scalar = Convert.ToInt32(results[iter]) - Convert.ToInt32(Compute(data[iter]));
            if (scalar == 0) { continue; }
            for (var j = 0; j < data[iter].Length; j++) 
            {
                _weights[j] += scalar * data[iter][j] * _learningRate;
            } 
            _weights[^1] += scalar * -1 * _learningRate;
        } 
    }

    public bool Compute(double[] data) 
    {
        if (data.Length != _dimension) 
        {
            throw new ArgumentException($"Vectors must have length: {_dimension}");
        }
        return data.Select((num, iter) => _weights[iter] * num).Sum() >= _weights[^1];
    }
}