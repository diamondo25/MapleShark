using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime.Operators
{
  public abstract class BinaryOperator : IOperator
  {
    /// <summary>
    /// Generic evaluator
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    private delegate object EvaluatorGeneric(object left, object right);

    /// <summary>
    /// Strongly typed evaluator
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public delegate object Evaluator<T1, T2>(T1 left, T2 right);

    private Dictionary<Type, Dictionary<Type, EvaluatorGeneric>> operators = new Dictionary<Type, Dictionary<Type, EvaluatorGeneric>>();

    public BinaryOperator(string name)
    {
      Name = name;
    }

    #region IOperator Members

    public virtual string Name
    {
      get;
      private set;
    }

    public object Evaluate(object left, object right)
    {
      EvaluatorGeneric function;
      try
      {
        function = operators[left.GetType()][right.GetType()];
      }
      catch (KeyNotFoundException)
      {
          
        throw new NotSupportedException("Operator does not support given arguments. Left object: " + left.GetType().ToString() + ", right object: " + right.GetType().ToString());
      }

      return function(left, right);
    }

    private void RegisterEvaluatorGeneric(Type left, Type right, EvaluatorGeneric eval)
    {

      if (operators.ContainsKey(left))
      {
        operators[left].Add(right, eval);
      }
      else
      {
        Dictionary<Type, EvaluatorGeneric> op = new Dictionary<Type, EvaluatorGeneric>();
        op.Add(right, eval);

        operators.Add(left, op);
      }
    }

    /// <summary>
    /// Register the evaluater strongly typed version.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="eval"></param>
    protected void RegisterEvaluator<T1, T2>(Evaluator<T1, T2> evaluator)
    {
      EvaluatorGeneric evaluatorGeneric = (x, y) =>
      {
        return evaluator((T1)x, (T2)y);
      };

      RegisterEvaluatorGeneric(typeof(T1), typeof(T2), evaluatorGeneric);
    }

    #endregion

    #region Unary functions
    public bool Unary
    {
      get { return false; }
    }

    public object Evaluate(object value)
    {
      throw new NotImplementedException();
    }
    #endregion
  }
}
