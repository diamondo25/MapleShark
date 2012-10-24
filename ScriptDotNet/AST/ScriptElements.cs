#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// 
  /// </summary>
  internal class ScriptElements : ScriptExpr
  {
    public ScriptElements(AstNodeArgs args)
      : base(args)
    {

    }

    //TODO: Implement Breaking, Returning, etc. here
    public override void Evaluate(IScriptContext context)
    {
      if (ChildNodes.Count == 0) return;

      int index = 0;
      while (index < ChildNodes.Count)
      {
        ScriptAst node = (ScriptAst)ChildNodes[index];
        node.Evaluate(context);

        if (context.IsBreak() || context.IsReturn() || context.IsContinue())
        {
          break;
        }

        index++;
      }
    }
  }
}
