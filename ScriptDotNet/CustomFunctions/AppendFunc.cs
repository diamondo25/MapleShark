using System;
using System.Collections.Generic;
using System.Text;
using Irony.Compiler;
using ScriptNET.Runtime;
using ScriptNET.Ast;

namespace ScriptNET.CustomFunctions
{
  internal class AppendFunc : IInvokable
  {
    public static AppendFunc FunctionDefinition = new AppendFunc();
    public static string FunctionName = "AppendAst";

    private AppendFunc()
    {
    }

    #region IInvokable Members

    public bool CanInvoke()
    {
      return true;
    }

    public object Invoke(IScriptContext context, object[] args)
    {
      ScriptProg node = (ScriptProg)args[0];

      //Get Prog
      AstNode prog = node.Parent;
      while (prog != null && !(prog is ScriptProg))
        prog = prog.Parent;

      foreach (ScriptAst scriptnode in node.Elements.ChildNodes)
      {
        prog.ChildNodes[0].ChildNodes.Add(scriptnode);
      }

      return node;
    }

    #endregion
  }
}
