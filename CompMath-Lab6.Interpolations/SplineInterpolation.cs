using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public abstract class SplineInterpolation : IInterpolation
{
	protected readonly double[] _x;
	protected readonly Polynomial[] _polynomials;

	public SplineInterpolation(FunctionData samples)
	{
		var orderedSamples = samples.OrderBy(s => s.X);
		_x = orderedSamples.Select(s => s.X).ToArray();
		_polynomials = GetPolynomials(orderedSamples.ToArray());
	}

	public abstract string Name { get; }

	protected abstract Polynomial[] GetPolynomials(FunctionData samples);
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
