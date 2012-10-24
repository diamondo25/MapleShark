using System;
using System.Collections.Generic;
using System.Text;
using Irony.Compiler;

namespace ScriptNET.Processing
{
  /// <summary>
  /// Processing procedures for script AST
  /// </summary>
  public interface IPostProcessing : IAstVisitor
  {
    void BeginProcessing(Script script);

    void EndProcessing(Script script);
  }
}
