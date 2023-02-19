namespace CompMath_Lab6
{
    public class NewtonBackwardInterpolation : NewtonInterpolation
    {
        public override string Name => "Newton backward";

        public override double Interpolate((double X, double Y)[] samples, double x)
        {
            int n = samples.Length;
            var divDiff = GetDividedDifference(samples);
            double p = divDiff[n - 1][0];
            var mul = 1.0;
            for (int i = n - 1; i > 0; i--)
            {
                mul *= x - samples[i].X;
                p += mul * divDiff[i - 1][n - i];
            }
            return p;
        }
    }
}
