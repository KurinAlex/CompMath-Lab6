namespace CompMath_Lab6
{
    class Program
    {
        const int N = 5;
        const double A = 1.0;
        const double B = 9.0;
        const int TestScale = 5;
        const int Precision = 8;

        const double Width = B - A;
        const double Step = Width / (N - 1);
        const double TestStep = Step / TestScale;
        const int TestN = (int)(Width / TestStep) + 1;

        static readonly Func<double, double> f =
            (double x) => 2 * Math.Asin(2 / (3 * x + 1)) + Math.Sqrt(9 * x * x + 6 * x - 3);

        static readonly IInterpolation[] interpolations = {
            new LagrangeInterpolation(),
            new NewtonForwardInterpolation(),
            new NewtonBackwardInterpolation(),
            new SplineInterpolation(),
        };

        static void Main()
        {
            var samples = new (double, double)[N];
            for (int i = 0; i < N; i++)
            {
                double x = A + i * Step;
                samples[i] = (x, f(x));
            }

            var xData = new double[TestN];
            for (int i = 0; i < TestN; i++)
            {
                xData[i] = A + i * TestStep;
            }

            var eData = new Dictionary<string, double[]>(interpolations.Length);
            foreach (var interpolation in interpolations)
            {
                var eDataArray = new double[TestN];
                for (int i = 0; i < TestN; i++)
                {
                    double x = xData[i];
                    double y = interpolation.Interpolate(samples, x);
                    eDataArray[i] = Math.Abs(f(x) - y);
                }
                eData.Add(interpolation.Name, eDataArray);
            }

            Drawer.DrawTable(xData, eData, "e(x)", Precision);
            Console.ReadLine();
        }
    }
}