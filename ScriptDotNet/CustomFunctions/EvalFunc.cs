using System;
using System.Collections.Generic;
using System.Text;
using Irony.Compiler;
using ScriptNET.Runtime;
using ScriptNET.Ast;

namespace ScriptNET.CustomFunctions
{
  internal class EvalFunc : IInvokable
  {
    public static EvalFunc FunctionDefinition = new EvalFunc();
    public static string FunctionName = "eval";

    private EvalFunc()
    {
    }

    #region IInvokable Members

    public bool CanInvoke()
    {
      return true;
    }

    public object Invoke(IScriptContext context, object[] args)
    {      
      string code = (String)args[0];
      ScriptAst result = null;

      LanguageCompiler compiler = (LanguageCompiler) context.GetItem("Compiler", true);
      RuntimeHost.Lock();
      
      try
      {
        result = Script.Parse(code + ";", false) as ScriptAst;
        //TODO: Create LocalOnlyScope that can't change Parent's variables
        //No, need for doing these. It is already done
        context.CreateScope();
      }
      finally
      {
        RuntimeHost.UnLock();
      }

      result.Evaluate(context);
      context.RemoveLocalScope();
      
      return context.Result;
    }

    #endregion
  }
}