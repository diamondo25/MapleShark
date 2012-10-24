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
  internal class ScriptFlowControlStatement : ScriptStatement
  {
    private string operation;
    ScriptAst expression;

    public ScriptFlowControlStatement(AstNodeArgs args)
        : base(args)
    {
      Token oper = ChildNodes[0] as Token;
      operation = oper.Text;
      Debug.Assert(oper.Text == "return" || oper.Text == "break" || oper.Text == "continue" || oper.Text == "throw");

      if (operation == "return" || operation == "throw")
        expression = (ScriptExpr)ChildNodes[1];
    }

    //TODO: reorganize switch
    public override void Evaluate(IScriptContext context)
    {
      switch (operation)
      {
        case "break":
          if (context.Result == null)
            context.Result = RuntimeHost.NullValue;
          context.SetBreak(true);
          break;
        case "continue":
          if (context.Result == null)
            context.Result = RuntimeHost.NullValue;
          context.SetContinue(true);
          break;
        case "return":
          expression.Evaluate(context);
          context.SetReturn(true);
          break;
        case "throw":
          expression.Evaluate(context);
          throw (Exception)context.Result;
        default:
          throw new ScriptException("This should never happen");
      }
    }
  }
}