using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  public interface IScopeActivator
  {
    IScriptScope Create(IScriptScope parent, params object[] args);
  }
}
