using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public abstract class SplineInterpolation : IInterpolation
{
	private readonly double[] _x;
	private readonly Polynomial[] _polynomials;

	public SplineInterpolation((double X, double Y)[] samples)
	{
		var orderedSamples = samples.OrderBy(s => s.X);
		_x = orderedSamples.Select(s => s.X).ToArray();
		_polynomials = GetPolynomials(orderedSamples.ToArray());
	}

	public abstract string Name { get; }

	protected abstract Polynomial[] GetPolynomials((double X, double Y)[] samples);
	public double Interpolate(double x)
	{
		int j = Array.BinarySearch(_x, x);
		if (j < 0)
		{
			j = ~j - 1;
		}
		j = Math.Min(j, _polynomials.Length - 1);
		return _polynomials[j].At(x - _x[j]);
	}
}
