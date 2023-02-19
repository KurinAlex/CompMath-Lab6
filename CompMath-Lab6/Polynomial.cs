using System.Text;

namespace CompMath_Lab6
{
    public class Polynomial
    {
        private readonly double[] _coefficients;
        private readonly int _degree;

        public static Polynomial Zero => new(0.0);
        public static Polynomial One => new(1.0);

        public Polynomial(IEnumerable<double> coefficients)
        {
            _coefficients = coefficients.Reverse().SkipWhile(c => c == 0.0).Reverse().ToArray();
            _degree = _coefficients.Length;
        }

        public Polynomial(params double[] coefficients) : this(coefficients as IEnumerable<double>)
        {
        }

        public double At(double x)
        {
            return _coefficients.Select((c, i) => c * Math.Pow(x, i)).Sum();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            bool isFirst = true;
            foreach ((double c, int i) in _coefficients.Select((c, i) => (c, i)).Reverse())
            {
                if (c == 0.0)
                {
                    continue;
                }

                sb.Append($"{(Math.Sign(c) == -1 ? "- " : isFirst ? "" : "+ ")}{Math.Abs(c)} ");
                isFirst = false;

                if (i == 0)
                {
                    continue;
                }

                sb.Append($"* x ");
                if (i != 1)
                {
                    sb.Append($"^ {i} ");
                }
            }
            return sb.ToString();
        }

        public static Polynomial operator +(Polynomial left, Polynomial right)
        {
            int degree = Math.Max(left._degree, right._degree);
            var coefficients = new double[degree];

            left._coefficients.CopyTo(coefficients, 0);
            for (int i = 0; i < right._degree; i++)
            {
                coefficients[i] += right._coefficients[i];
            }

            return new(coefficients);
        }

        public static Polynomial operator *(Polynomial left, Polynomial right)
        {
            int degree = left._degree + right._degree - 1;
            var coefficients = new double[degree];

            for (int i = 0; i < left._degree; i++)
            {
                for (int j = 0; j < right._degree; j++)
                {
                    coefficients[i + j] += left._coefficients[i] * right._coefficients[j];
                }
            }

            return new(coefficients);
        }

        public static Polynomial operator *(Polynomial polynomial, double scale)
        {
            return new(polynomial._coefficients.Select(c => c * scale));
        }

        public static Polynomial operator /(Polynomial polynomial, double scale)
        {
            return new(polynomial._coefficients.Select(c => c / scale));
        }
    }
}
