using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime.Operators
{
  /// <summary>
  /// Implementation of and operator
  /// </summary>
  public class AndOperator : BinaryOperator
  {
    public AndOperator() :
      base("&")
    {
        RegisterEvaluator<Boolean, Boolean>((x, y) => x & y);
        RegisterEvaluator<Byte, Byte>((x, y) => x & y);
        RegisterEvaluator<Byte, Int16>((x, y) => x & y);
        RegisterEvaluator<Byte, Int32>((x, y) => x & y);
        RegisterEvaluator<Byte, Int64>((x, y) => x & y);

        RegisterEvaluator<Int16, Byte>((x, y) => x & y);
        RegisterEvaluator<Int16, Int16>((x, y) => x & y);
        RegisterEvaluator<Int16, Int32>((x, y) => x & y);
        RegisterEvaluator<Int16, Int64>((x, y) => x & y);

        RegisterEvaluator<Int32, Byte>((x, y) => x & y);
        RegisterEvaluator<Int32, Int16>((x, y) => x & y);
        RegisterEvaluator<Int32, Int32>((x, y) => x & y);
        RegisterEvaluator<Int32, Int64>((x, y) => x & y);

        RegisterEvaluator<Int64, Byte>((x, y) => x & y);
        RegisterEvaluator<Int64, Int16>((x, y) => x & y);
        RegisterEvaluator<Int64, Int32>((x, y) => x & y);
        RegisterEvaluator<Int64, Int64>((x, y) => x & y);
    }
  }

  /// <summary>
  /// Implementation of exclusive and operator
  /// </summary>
  public class And2Operator : BinaryOperator
  {
    public And2Operator() :
      base("&&")
    {
        RegisterEvaluator<Boolean, Boolean>((x, y) => x && y);
    }
  }

  /// <summary>
  /// Implementation of or operator
  /// </summary>
  public class OrOperator : BinaryOperator
  {
    public OrOperator() :
      base("|")
    {
        RegisterEvaluator<Boolean, Boolean>((x, y) => x | y);
    }
  }

  /// <summary>
  /// Implementation of exclusive or operator
  /// </summary>
  public class Or2Operator : BinaryOperator
  {
    public Or2Operator() :
      base("||")
    {
      RegisterEvaluator<Boolean, Boolean>((x, y) => x || y);
    }
  }

  /// <summary>
  /// Implementation of equals operator
  /// </summary>
  public class EqualsOperator : IOperator
  {
    public EqualsOperator()
    {
    }

    #region IOperator Members

    public string Name
    {
      get { return "=="; }
    }

    public bool Unary
    {
      get { return false; }
    }

    public object Evaluate(object value)
    {
      throw new NotImplementedException();
    }

    public object Evaluate(object left, object right)
    {
      if (left == null || right == null)
        return object.Equals(left, right);

      IObjectBind equalityMethod = null;

      equalityMethod = RuntimeHost.Binder.BindToMethod(left, "op_Equality", null, new object[] { left, right });
      if (equalityMethod != null)
        return equalityMethod.Invoke(null, new object[] { left, right });

      equalityMethod = RuntimeHost.Binder.BindToMethod(right, "op_Equality", null, new object[] { right, left });
      if (equalityMethod != null)
        return equalityMethod.Invoke(null, new object[] { right, left });

      equalityMethod = RuntimeHost.Binder.BindToMethod(left, "Equals", null, new object[] { right });
      if (equalityMethod != null)
        return equalityMethod.Invoke(null, new object[] { left, right });
      
      return object.Equals(left, right);
    }

    #endregion
  }

  /// <summary>
  /// Implementation of not equals operator
  /// </summary>
  public class NotEqualsOperator : IOperator
  {
    EqualsOperator equalsOperator = new EqualsOperator();

    public NotEqualsOperator()
    {
    }

    #region IOperator Members

    public string Name
    {
      get { return "!="; }
    }

    public bool Unary
    {
      get { return false; }
    }

    public object Evaluate(object value)
    {
      return equalsOperator.Evaluate(value);
    }

    public object Evaluate(object left, object right)
    {
      return !(bool)equalsOperator.Evaluate(left, right);
    }

    #endregion
  }
}
