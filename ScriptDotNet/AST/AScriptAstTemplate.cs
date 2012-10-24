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
  internal class ScriptAstTemplate : ScriptAst
  {
    public ScriptAstTemplate(AstNodeArgs args)
      : base(args)
    {

    }

    public override void Evaluate(IScriptContext context)
    {
      throw new NotSupportedException();
    }

  }
}
