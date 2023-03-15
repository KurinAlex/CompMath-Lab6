using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public abstract class PolynomialInterpolation : IInterpolation
{
	private readonly Polynomial _polynomial;

	public PolynomialInterpolation((double X, double Y)[] samples) => _polynomial = GetPolynomial(samples);

	public abstract string Name { get; }

	protected abstract Polynomial GetPolynomial((double X, double Y)[] samples);
	public double Interpolate(double x) => _polynomial.At(x);
}
