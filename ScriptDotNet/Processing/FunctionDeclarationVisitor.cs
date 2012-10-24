using System;
using System.Collections.Generic;
using System.Text;
using Irony.Compiler;
using ScriptNET.Runtime;
using ScriptNET.Ast;

namespace ScriptNET.Processing
{
  internal class FunctionDeclarationVisitor : IPostProcessing
  {
    private Script Script;

    public FunctionDeclarationVisitor()
    {
    }

    #region IAstVisitor Members

    public void BeginVisit(AstNode node)
    {
      ScriptFunctionDefinition definition = node as ScriptFunctionDefinition;
      if (definition != null && !string.IsNullOrEmpty(definition.Name))
        Script.Context.SetItem(definition.Name, definition);
    }

    public void EndVisit(AstNode node)
    {
    }

    #endregion

    #region IPostProcessing Members

    public void BeginProcessing(Script script)
    {
      Script = script;
    }

    public void EndProcessing(Script script)
    {
      if (Script != script) throw new InvalidOperationException();
      Script = null;
    }

    #endregion
  }
}
