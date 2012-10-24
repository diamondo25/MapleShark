#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// 
  /// </summary>
  internal class ScriptMObjectPart : ScriptExpr
  {
    private string name;
    private ScriptExpr expr;

    public ScriptMObjectPart(AstNodeArgs args)
        : base(args)
    {
      name = (ChildNodes[0] as Token).Text;
      expr = ChildNodes[2] as ScriptExpr;
    }

    public override void Evaluate(IScriptContext context)
    {
      expr.Evaluate(context);
      context.Result = new object[] { name, context.Result };
    }
  }
}