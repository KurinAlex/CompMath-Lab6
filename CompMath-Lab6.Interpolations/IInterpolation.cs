namespace CompMath_Lab6.Interpolations;

public interface IInterpolation
{
	string Name { get; }
	double Interpolate((double X, double Y)[] samples, double x);
}
