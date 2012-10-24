#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptSwitchDefaultStatement : ScriptStatement
  {
    private ScriptStatement statement;

    public ScriptSwitchDefaultStatement(AstNodeArgs args)
        : base(args)
    {
      statement = ChildNodes[2] as ScriptStatement;
    }

    public override void Evaluate(IScriptContext context)
    {      
      statement.Evaluate(context);      
    }
  }
}