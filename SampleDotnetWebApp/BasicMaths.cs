using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDotnetWebApp
{
    public class BasicMaths
    {
        public double Add(double num1, double num2)
        {
            return num1 + num2;
        }

        public double Subtract(double num1, double num2)
        {
            return num1 - num2;
        }

        public double divide(double num1, double num2)
        {
            return num1 / num2;
        }

        public double Multiply(double num1, double num2)
        {
            // to trace error while testing, writing + opertaor instead of * operator
            return num1 * num2;
        }
    }
}
