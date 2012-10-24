#region using
using System.Diagnostics;

using Irony.Compiler;
using ScriptNET.Runtime;
using ScriptNET.Runtime.Operators;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptIsExpr : ScriptBinExpr
  {
    private IOperator @operator;

    public ScriptIsExpr(AstNodeArgs args)
      : base(args)
    {
      Debug.Assert(oper == "is");

      @operator = RuntimeHost.GetBinaryOperator(oper);
      if (@operator == null)
        throw new ScriptException("RuntimeHost did not initialize property. Can't find binary operators.");
    }

    public override void Evaluate(IScriptContext context)
    {
      object leftVal, rightVal;

      left.Evaluate(context);
      leftVal = context.Result;

      context.Result = RuntimeHost.NullValue;

      ScriptTypeExpr name = (ScriptTypeExpr)right;
      name.Evaluate(context);
      rightVal = context.Result;

      context.Result = @operator.Evaluate(leftVal, rightVal);
    }
  }
}
