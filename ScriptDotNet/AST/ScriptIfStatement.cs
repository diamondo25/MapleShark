#region using

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Script Array Constructor Expression
  /// </summary>
  internal class ScriptIfStatement : ScriptStatement
  {
    private ScriptCondition condition;
    private ScriptStatement statement;
    private ScriptStatement elseStatement;

    public ScriptIfStatement(AstNodeArgs args)
        : base(args)
    {
      condition = (ScriptCondition) ChildNodes[1];
      statement = (ScriptStatement)ChildNodes[2];
      //Else exists
      if (ChildNodes.Count == 4 && ChildNodes[3].ChildNodes.Count == 2 && ChildNodes[3].ChildNodes[1] is ScriptStatement)
      {
        elseStatement = (ScriptStatement)ChildNodes[3].ChildNodes[1];
      }
    }

    public override void Evaluate(IScriptContext context)
    {
      condition.Evaluate(context);
      if ((bool)context.Result)
      {       
        statement.Evaluate(context);
      }
      else
        if (elseStatement != null)
        {
          elseStatement.Evaluate(context);
        }
    }
  }
}