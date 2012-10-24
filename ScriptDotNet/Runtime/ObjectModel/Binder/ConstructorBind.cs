using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ScriptNET.Runtime
{
  internal class ConstructorBind : IObjectBind
  {
    ConstructorInfo Method { get; set; }
    object[] Arguments { get; set; }

    public Type Type { get; private set; }

    public ConstructorBind(ConstructorInfo method, object[] arguments)
    {
      Method = method;
      Arguments = arguments;
      Type = method.DeclaringType;
    }

    public ConstructorBind(Type type, ConstructorInfo method, object[] arguments)
    {
      Method = method;
      Arguments = arguments;
      Type = type;
    }
    #region IInvokable Members

    public bool CanInvoke()
    {
      return Method != null;
    }

    public object Invoke(IScriptContext context, object[] args)
    {
      return Method.Invoke(Arguments);
    }

    #endregion
  }
}
