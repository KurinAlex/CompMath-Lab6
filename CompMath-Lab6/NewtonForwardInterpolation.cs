namespace CompMath_Lab6
{
    public class NewtonForwardInterpolation : NewtonInterpolation
    {
        public override string Name => "Newton forward";

        public override double Interpolate((double X, double Y)[] samples, double x)
        {
            var divDiff = GetDividedDifference(samples)[0];
            double p = divDiff[0];
            double mul = 1.0;
            for (int i = 1; i < samples.Length; i++)
            {
                mul *= x-samples[i - 1].X;
                p += mul * divDiff[i];
            }
            return p;
        }
    }
}
