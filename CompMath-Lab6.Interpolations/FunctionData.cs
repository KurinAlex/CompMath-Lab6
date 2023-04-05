global using FunctionData = System.Collections.Generic.IList<CompMath_Lab6.Interpolations.FunctionDataSample>;

namespace CompMath_Lab6.Interpolations;

public record struct FunctionDataSample(double X, double Y, double DY, double DDY);