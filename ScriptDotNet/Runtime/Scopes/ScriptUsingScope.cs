using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ScriptNET.Runtime
{
  public class ScriptUsingScope : ScriptScope
  {
    public ScriptUsingScope(IScriptScope parent, object usingObject):
        base(parent)
    {
      if (parent == null) throw new ArgumentNullException("parent");
      if (usingObject == null) throw new ArgumentNullException("usingObject");

      Type type = usingObject as Type;
      if (type == null) type = usingObject.GetType(); ;

      IEnumerable<MethodInfo> methods = type.GetMethods(ObjectBinder.MethodFilter);
      foreach (MethodInfo method in methods)
      {
        SetItem(method.Name, new LateBoundMethod(method.Name, usingObject));
      }
    }

    public override object GetItem(string id, bool throwException)
    {
      return Parent.GetItem(id, throwException);
    }

    public override void SetItem(string id, object value)
    {
        Parent.SetItem(id, value);
    }
  }

  public class ScriptUsingScopeActivator : IScopeActivator
  {
    #region IScopeActivator Members

    public IScriptScope Create(IScriptScope parent, params object[] args)
    {
      if (args.Length == 1)
        return new ScriptUsingScope(parent, args[0]);

      throw new NotSupportedException();
    }

    #endregion
  }

}
