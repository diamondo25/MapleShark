#region using
using System;
using System.Linq;

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Expression List Expression
  /// </summary>
  internal class ScriptTypeExprList : ScriptExpr
  {
    internal ScriptTypeExpr[] exprList
    {
      get
      {
        return ChildNodes.OfType<ScriptTypeExpr>().ToArray();
      }
    }
  
    public ScriptTypeExprList(AstNodeArgs args)
        : base(args)
    {
    }

    public override void Evaluate(IScriptContext context)
    {
      Type[] listObjects = new Type[exprList.Length];
      for (int i = 0; i < exprList.Length; i++)
      {
        exprList[i].Evaluate(context);
        listObjects[i] = (Type)context.Result;
      }
      context.Result = listObjects;
    }
  }
}
