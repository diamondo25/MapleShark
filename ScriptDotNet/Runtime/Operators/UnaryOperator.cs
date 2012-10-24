using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime.Operators
{
  public abstract class UnaryOperator : IOperator
  {
    #region IOperator Members

    public string Name
    {
      get;
      protected set;
    }

    public bool Unary
    {
      get { return true; }
    }

    public abstract object Evaluate(object value);

    public object Evaluate(object left, object right)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
