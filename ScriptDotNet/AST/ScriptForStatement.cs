#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// For statement
  /// </summary>
  internal class ScriptForStatement : ScriptStatement
  {
    private ScriptAst init;
    private ScriptAst cond;
    private ScriptAst next;
    private ScriptStatement statement;

    public ScriptForStatement(AstNodeArgs args)
        : base(args)
    {
      init = (ScriptAst)args.ChildNodes[1];
      cond = (ScriptAst)args.ChildNodes[2];
      next = (ScriptAst)args.ChildNodes[3];
      statement = (ScriptStatement)args.ChildNodes[4];
    }

    public override void Evaluate(IScriptContext context)
    {
      bool condBool;
      object result = RuntimeHost.NullValue;

      init.Evaluate(context);
      cond.Evaluate(context);
      condBool = context.Result == null ? true : (bool)context.Result;
      
      while (condBool)
      {
        statement.Evaluate(context);
        result = context.Result;

        if (context.IsBreak() || context.IsReturn())
        {
          context.SetBreak(false);
          break;
        }

        if (context.IsContinue())
        {
          context.SetContinue(false);
        }


        next.Evaluate(context);
        cond.Evaluate(context);
        condBool = context.Result == null ? true : (bool)context.Result;
      }

      context.Result = result;
    }
  }
}
