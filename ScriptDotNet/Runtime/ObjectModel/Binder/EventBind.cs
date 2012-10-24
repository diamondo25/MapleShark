using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ScriptNET.Runtime.ObjectModel.Binder
{
  [Obsolete()]
  internal class EventBind : IMemberBind
  {
    public EventBind(object target, EventInfo @event)
    {
      if (target == null)
        throw new ArgumentNullException("target");
      if (@event == null)
        throw new ArgumentNullException("event");

      Target = target;
      TargetType = target.GetType();
      this.@event = @event;
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

    private EventInfo @event;

    public MemberInfo Member
    {
      get
      {
        return @event;
      }
    } 

    public void SetValue(object value)
    {
      throw new NotSupportedException();
    }

    public object GetValue()
    {
      throw new NotSupportedException();
    }

    public void AddHandler(object value)
    {
      EventBroker.AssignEvent(@event, Target, (IInvokable)value);
    }

    public void RemoveHandler(object value)
    {
      EventBroker.RemoveEvent(@event, Target, (IInvokable)value);      
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
