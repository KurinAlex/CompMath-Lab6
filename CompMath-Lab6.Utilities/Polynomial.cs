namespace CompMath_Lab6.Utilities;

public readonly struct Polynomial
{
	private readonly double[] _coefficients;
	private readonly IEnumerable<double> _reversedCoefficients;
	private readonly int _degree;

	public Polynomial(params double[] coefficients) : this(coefficients as IEnumerable<double>)
	{
	}
	private Polynomial(IEnumerable<double> coefficients)
	{
		_reversedCoefficients = coefficients
			.Reverse()
			.SkipWhile(c => c == 0.0);
		if (!_reversedCoefficients.Any())
		{
			_reversedCoefficients = _reversedCoefficients.Append(0.0);
		}

		_coefficients = _reversedCoefficients
			.Reverse()
			.ToArray();
		_degree = _coefficients.Length;
	}

	public static readonly Polynomial Zero = new(0.0);
	public static readonly Polynomial One = new(1.0);

	public double At(double x) => _reversedCoefficients.Aggregate(0.0, (s, c) => s * x + c);

	public static Polynomial operator +(Polynomial left, Polynomial right)
	{
		int degree = Math.Max(left._degree, right._degree);
		var coefficients = new double[degree];

		left._coefficients.CopyTo(coefficients, 0);
		for (int i = 0; i < right._degree; i++)
		{
			coefficients[i] += right._coefficients[i];
		}

		return new(coefficients);
	}
	public static Polynomial operator *(Polynomial left, Polynomial right)
	{
		int degree = left._degree + right._degree - 1;
		var coefficients = new double[degree];

		for (int i = 0; i < left._degree; i++)
		{
			for (int j = 0; j < right._degree; j++)
			{
				coefficients[i + j] += left._coefficients[i] * right._coefficients[j];
			}
		}

		return new(coefficients);
	}

	public static Polynomial operator *(Polynomial polynomial, double scale)
		=> new(polynomial._coefficients.Select(c => c * scale));
	public static Polynomial operator /(Polynomial polynomial, double scale)
		=> new(polynomial._coefficients.Select(c => c / scale));
}
