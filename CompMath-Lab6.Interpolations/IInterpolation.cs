namespace CompMath_Lab6.Interpolations;

public interface IInterpolation
{
	string Name { get; }
	double Interpolate(double x);
}
