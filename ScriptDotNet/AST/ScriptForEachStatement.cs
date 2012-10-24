#region using

using Irony.Compiler;
using System.Collections;
using ScriptNET.Runtime;
using System;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// ForEachStatement
  /// </summary>
  internal class ScriptForEachStatement : ScriptStatement
  {
    private Token name;
    private ScriptExpr expr;
    private ScriptStatement statement;

    public ScriptForEachStatement(AstNodeArgs args)
        : base(args)
    {
      name = (Token)ChildNodes[1];
      expr = (ScriptExpr)ChildNodes[3];
      statement = (ScriptStatement)ChildNodes[4];
    }

    public override void Evaluate(IScriptContext context)
    {
      expr.Evaluate(context);
      
      IEnumerable enumeration = context.Result as IEnumerable;
      IEnumerator enumerator = null;
     
      if (enumeration != null)
      {
        enumerator = enumeration.GetEnumerator();
      }
      else
      {
        IObjectBind bind = RuntimeHost.Binder.BindToMethod(context.Result, "GetEnumerator",new Type[0], new object[0]);
        if (bind != null)
          enumerator = bind.Invoke(context, null) as IEnumerator;
      }

      if (enumerator == null)
        throw new ScriptException("GetEnumerator() method did not found in object: " + context.Result.ToString());

      enumerator.Reset();
      
      while(enumerator.MoveNext())
      {
        context.SetItem(name.Text, enumerator.Current);
        statement.Evaluate(context);
        if (context.IsBreak() || context.IsReturn())
        {
          context.SetBreak(false);
          break;
        }
        if (context.IsContinue())
        {
          context.SetContinue(false);
        }
      } 

    }
  }
}
