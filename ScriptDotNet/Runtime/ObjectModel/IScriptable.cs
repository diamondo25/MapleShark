using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Expose dynamic members of an Instance to the script.
  /// This require using of <cref="ScriptNET.Runtime.ObjectBinderExtended">ObjectBinderExtended</cref> class as default object binder.
  /// </summary>
  public interface IScriptable
  {
    /// <summary>
    /// Should return object wrapped by IScriptable or this
    /// </summary>
    [Bindable(false)]
    object Instance { get; }

    /// <summary>
    /// Gets a binding to an instance's member (field, property)
    /// </summary>
    [Bindable(false)]
    IMemberBind GetMember(string name, params object[] arguments);

    /// <summary>
    /// Gets a binding to an instance's method
    /// </summary>
    [Bindable(false)]
    IObjectBind GetMethod(string name, params object[] arguments);
  }
}
