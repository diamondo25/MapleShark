using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Represents activator for event scope
  /// </summary>
  public class EventScopeActivator: IScopeActivator
  {
    #region IScopeActivator Members

    public IScriptScope Create(IScriptScope parent, params object[] args)
    {
      return new ScriptScope(parent);
    }

    #endregion
  }
}
