using System;
using System.Collections.Generic;
using System.Text;
using Irony.Compiler;
using ScriptNET.Runtime;
using ScriptNET.Ast;

namespace ScriptNET.CustomFunctions
{
  internal class CharFunc : IInvokable
  {
    public static CharFunc FunctionDefinition = new CharFunc();
    public static string FunctionName = "char";

    private CharFunc()
    {
    }

    #region IInvokable Members

    public bool CanInvoke()
    {
      return true;
    }

    public object Invoke(IScriptContext context, object[] args)
    {
      if (args == null || args.Length == 0) return '\0';

      return ((string)args[0])[0];
    }

    #endregion
  }
}