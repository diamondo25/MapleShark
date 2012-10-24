using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ScriptNET.Runtime
{
#if !PocketPC && !SILVERLIGHT
  public class ExplicitInterface : IScriptable
  {
    private InterfaceMapping Mapping;
    private object Target;

    public ExplicitInterface(object target, Type interfaceType)
    {
      if (target == null) throw new ScriptException("Null object conversion");
      if (interfaceType == null) throw new ArgumentNullException();

      Mapping = target.GetType().GetInterfaceMap(interfaceType);
      Target = target;
    }

    #region IScriptable Members

    public object Instance
    {
      get { return Target; }
    }

    [Bindable(false)]
    public IMemberBind GetMember(string name, params object[] arguments)
    {
      IObjectBind getter = null;
      MethodInfo getMethod = FindMethod("get_" + name);
      if (getMethod != null)
        getter = new LateBoundMethod(getMethod, Target);

      IObjectBind setter = null;
      MethodInfo setMethod = FindMethod("set_" + name);
      if (setMethod != null)
        setter = new LateBoundMethod(setMethod, Target);

      return new InterfaceMember(getter, setter, Target);
    }

    [Bindable(false)]
    public IObjectBind GetMethod(string name, params object[] arguments)
    {
      MethodInfo method = FindMethod(name);
      if (method == null)
        throw new ScriptMethodNotFoundException(name);

      return new LateBoundMethod(method, Target);
    }

    [Bindable(false)]
    private MethodInfo FindMethod(string name)
    {
      MethodInfo method = Mapping.InterfaceMethods.Where(i => i.Name == name).FirstOrDefault();
      return method;
    }

    #endregion

    #region Interface Property Bind
    private class InterfaceMember : IMemberBind
    {
      IObjectBind getter;
      IObjectBind setter;

      public InterfaceMember(IObjectBind getter, IObjectBind setter, object target)
      {
        this.getter = getter;
        this.setter = setter;
        Target = target;
      }

      #region IMemberBind Members

      public object Target
      {
        get;
        set;
      }

      public Type TargetType
      {
        get { throw new NotSupportedException(); }
      }

      public MemberInfo Member
      {
        get { throw new NotSupportedException(); }
      }

      public void SetValue(object value)
      {
        if (setter == null) throw new NotSupportedException();
        setter.Invoke(null, new object[] { value });
      }

      public object GetValue()
      {
        if (getter == null) throw new NotSupportedException();
        return getter.Invoke(null, new object[0]);
      }

      public void AddHandler(object value)
      {
        throw new NotSupportedException();
      }

      public void RemoveHandler(object value)
      {
        throw new NotSupportedException();
      }

      #endregion

      #region IInvokable Members

      public bool CanInvoke()
      {
        return getter != null;
      }

      public object Invoke(IScriptContext context, object[] args)
      {
        return GetValue();
      }

      #endregion
    }
    #endregion
  }
#endif
}
