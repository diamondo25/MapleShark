using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Scriptable object that can be used to rename properties from the given instance
  /// </summary>
  public class MemberRename : IScriptable
  {
    IMemberBind oldMember = null;
    string original;
    string newName;

    public MemberRename(object instance, string original, string newName)
    {
      Instance = instance;
      oldMember = RuntimeHost.Binder.BindToMember(instance, original, true);
      if (oldMember == null)
        throw new ScriptIdNotFoundException(original);

      this.newName = newName;
      this.original = original;
    }

    #region IScriptable
    [Bindable(false)]
    public object Instance
    {
      get;
      private set;
    }

    [Bindable(false)]
    public IMemberBind GetMember(string name, params object[] arguments)
    {
      if (name == newName) return oldMember;
      return null;
    }

    [Bindable(false)]
    public IObjectBind GetMethod(string name, params object[] arguments)
    {
      return null;
    }

    #endregion
  }
}
