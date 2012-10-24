using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Objects implementing this interface will participate in strong assignment (:=) operator.
  /// </summary>
  public interface ISupportAssign
  {
    void AssignTo(object target);
  }
}
