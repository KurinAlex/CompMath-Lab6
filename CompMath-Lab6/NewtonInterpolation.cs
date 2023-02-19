namespace CompMath_Lab6
{
    public abstract class NewtonInterpolation : IInterpolation
    {
        public abstract string Name { get; }
        public abstract double Interpolate((double X, double Y)[] samples, double x);

        protected static double[][] GetDividedDifference((double X, double Y)[] samples)
        {
            int n = samples.Length;
            var divDiff = new double[n][];
            for (int i = 0; i < n; i++)
            {
                divDiff[i] = new double[n - i];
                divDiff[i][0] = samples[i].Y;
            }
            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < n - j; i++)
                {
                    divDiff[i][j] = (divDiff[i + 1][j - 1] - divDiff[i][j - 1]) / (samples[i + j].X - samples[i].X);
                }
            }
            return divDiff;
        }
    }
}
