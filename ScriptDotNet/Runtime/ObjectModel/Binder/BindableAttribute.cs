using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Indicates members and classes which could participate in binding procedure
  /// during script execution.
  /// </summary>
  [AttributeUsage(AttributeTargets.All)]
  public class BindableAttribute : Attribute
  {
    /// <summary>
    /// If set to true the member will be processed by default object binder
    /// otherwise this member will be invisible from the script.
    /// </summary>
    public bool CanBind { get; set; }

    public BindableAttribute()
    {
      CanBind = true;
    }

    public BindableAttribute(bool canBind)
    {
      CanBind = canBind;
    }
  }
}
