#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptSwitchCaseStatement : ScriptStatement
  {
    private ScriptExpr cond;
    private ScriptStatement statement;

    public ScriptSwitchCaseStatement(AstNodeArgs args)
        : base(args)
    {
      cond = ChildNodes[1] as ScriptExpr;
      statement = ChildNodes[3] as ScriptStatement;
    }

    public override void Evaluate(IScriptContext context)
    {
      object switchValue = context.Result;     
      cond.Evaluate(context);
      if (switchValue.Equals(context.Result))
      {
        statement.Evaluate(context);
        context.SetBreak(true);
      }
      else
        context.Result = switchValue;
    }
  }
}
