using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime.Operators
{
  /// <summary>
  /// Implementation of power operator
  /// </summary>
  public class PowOperator : BinaryOperator
  {
    public PowOperator() :
      base("^")
    {
      RegisterEvaluator<Decimal, Decimal>((x, y) => Math.Pow((double)x, (double)y));
      RegisterEvaluator<Decimal, Int16>((x, y) => Math.Pow((double)x, y));
      RegisterEvaluator<Decimal, Int32>((x, y) => Math.Pow((double)x, y));
      RegisterEvaluator<Decimal, Int64>((x, y) => Math.Pow((double)x, y));
      RegisterEvaluator<Decimal, double>((x, y) => Math.Pow((double)x, y));
      RegisterEvaluator<Decimal, float>((x, y) => Math.Pow((double)x, y));


      RegisterEvaluator<Int16, Decimal>((x, y) => Math.Pow(x, (double)y));
      RegisterEvaluator<Int32, Decimal>((x, y) => Math.Pow(x, (double)y));
      RegisterEvaluator<Int64, Decimal>((x, y) => Math.Pow(x, (double)y));
      RegisterEvaluator<double, Decimal>((x, y) => Math.Pow(x, (double)y));
      RegisterEvaluator<float, Decimal>((x, y) => Math.Pow(x, (double)y));

      RegisterEvaluator<Int16, Int16>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int16, Int32>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int16, Int64>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int16, double>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int16, float>((x, y) => Math.Pow(x, y));

      RegisterEvaluator<Int32, Int16>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int32, Int32>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int32, Int64>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int32, double>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int32, float>((x, y) => Math.Pow(x, y));

      RegisterEvaluator<Int64, Int16>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int64, Int32>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int64, Int64>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int64, double>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<Int64, float>((x, y) => Math.Pow(x, y));

      RegisterEvaluator<double, Int16>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<double, Int32>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<double, Int64>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<double, double>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<double, float>((x, y) => Math.Pow(x, y));

      RegisterEvaluator<float, Int16>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<float, Int32>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<float, Int64>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<float, double>((x, y) => Math.Pow(x, y));
      RegisterEvaluator<float, float>((x, y) => Math.Pow(x, y));
    }
  }
}