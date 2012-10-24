#region using
using System;
using System.Diagnostics;

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptGenericsPostfix : ScriptExpr
  {
    private ScriptTypeExprList token;

    public ScriptGenericsPostfix(AstNodeArgs args)
        : base(args)
    {
      token = ChildNodes[1] as ScriptTypeExprList;
    }

    public override void Evaluate(IScriptContext context)
    {
      token.Evaluate(context);
      Debug.Assert(context.Result != null);
    }

    public string GetGenericTypeName(string name)
    {
      return string.Format("{0}`{1}", name, token.exprList.Length);
    }
  }
}
