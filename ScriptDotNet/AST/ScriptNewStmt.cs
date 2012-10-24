#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptNewStmt : ScriptExpr
  {
    ScriptTypeConstructor constrExpr;

    public ScriptNewStmt(AstNodeArgs args)
        : base(args)
    {
      constrExpr = ChildNodes[1] as ScriptTypeConstructor;      
    }

    public override void Evaluate(IScriptContext context)
    {
      constrExpr.Evaluate(context);

      IObjectBind call = (IObjectBind)context.Result;
      context.Result = RuntimeHost.Activator.CreateInstance(context, call);
    }
  }
}