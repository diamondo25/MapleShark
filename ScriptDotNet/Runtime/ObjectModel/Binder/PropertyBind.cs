using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ScriptNET.Runtime.ObjectModel.Binder
{
  [Obsolete()]
  internal class PropertyBind : IMemberBind
  {
    public PropertyBind(object target, PropertyInfo property)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      if (property == null)
        throw new ArgumentNullException("property");

      Target = target;
      TargetType = target.GetType();
      this.property = property;
    }

    #region IMemberBind Members

    public object Target
    {
      get;
      protected set;
    }

    public Type TargetType
    {
      get;
      protected set;
    }

    private PropertyInfo property;

    public MemberInfo Member
    {
      get
      {
        return property;
      }
    } 

    public void SetValue(object value)
    {
      property.SetValue(Target, value, null);
    }

    public object GetValue()
    {
      return property.GetValue(Target, null);
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
      return Target != null && Member != null;
    }

    public object Invoke(IScriptContext context, object[] args)
    {
      return GetValue();
    }

    #endregion
  }
}
