#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptFunctionCall : ScriptExpr
  {
    private ScriptExprList funcArgs;

    public ScriptFunctionCall(AstNodeArgs args)
      : base(args)
    {
      if (ChildNodes.Count != 0)
        funcArgs = ChildNodes[0] as ScriptExprList;
    }

    public override void Evaluate(IScriptContext context)
    {
      if (funcArgs != null)
      {
        funcArgs.Evaluate(context);
        context.Result = (object[])context.Result;
      }
      else
      {
        context.Result = new object[0];
      }
    }
  }
}
