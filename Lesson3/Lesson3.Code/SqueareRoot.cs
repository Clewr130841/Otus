using System;
using System.Runtime.CompilerServices;

namespace Lesson3.Code
{
    public class SqueareRoot
    {
        public double[] Solve(double a, double b, double c)
        {
            if (IsInfOrNan(a))
            {
                throw new ArgumentException();
            }

            if (IsInfOrNan(b))
            {
                throw new ArgumentException();
            }

            if (IsInfOrNan(c))
            {
                throw new ArgumentException();
            }

            if (IsZero(a))
            {
                return new double[0];
            }

            var d = b * b - 4 * a * c;

            if (IsZero(d))
            {
                var result = -b / (2 * a);
                return new double[] { result, result };
            }
            else
            {
                if (d < 0)
                {
                    return new double[0];
                }
                else
                {
                    return new double[]
                    {
                        (-b + Math.Sqrt(d))/(2*a),
                        (-b - Math.Sqrt(d))/(2*a),
                    };
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsZero(double value)
        {
            return value < Double.Epsilon && value > -Double.Epsilon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsInfOrNan(double value)
        {
            return Double.IsInfinity(value) | Double.IsNaN(value);
        }
    }
}
