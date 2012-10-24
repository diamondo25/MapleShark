#region using
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Irony.Compiler;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// 
  /// </summary>
  internal class ScriptArrayResolution : ScriptAst
  {
    private ScriptExprList args;
    private static readonly object Empty = new object[0];

    public ScriptArrayResolution(AstNodeArgs args)
        : base(args)
    {
      if (args.ChildNodes.Count != 0)
        this.args = args.ChildNodes[0] as ScriptExprList;
    }

    public override void Evaluate(IScriptContext context)
    {
      if (args == null)
      {
        context.Result = Empty;
        return;
      }
      args.Evaluate(context);
    }
  }
}