#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptWhileStatement : ScriptExpr
  {
    private ScriptCondition condition;
    private ScriptStatement statement;

    public ScriptWhileStatement(AstNodeArgs args)
        : base(args)
    {
      condition = args.ChildNodes[1] as ScriptCondition;
      statement = args.ChildNodes[2] as ScriptStatement;
    }

    public override void Evaluate(IScriptContext context)
    {
      condition.Evaluate(context);
      object lastResult = RuntimeHost.NullValue;

      while ((bool)context.Result)
      {
        statement.Evaluate(context);
        lastResult = context.Result;

        if (context.IsBreak() || context.IsReturn())
        {
          context.SetBreak(false);
          break;
        }

        if (context.IsContinue())
        {
          context.SetContinue(false);
        }

        condition.Evaluate(context);
      }

      context.Result = lastResult;
    }
  }
}