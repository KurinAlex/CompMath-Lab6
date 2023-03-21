using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public abstract class PolynomialInterpolation : IInterpolation
{
	private readonly Polynomial _polynomial;

	public PolynomialInterpolation(FunctionData samples) => _polynomial = GetPolynomial(samples);

	public abstract string Name { get; }

	protected abstract Polynomial GetPolynomial(FunctionData samples);
	public double Interpolate(double x) => _polynomial.At(x);
}
