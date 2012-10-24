using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Wraps object, allows to create new fields and access members form inner object
  /// Note: ExpandoWrapper does not support generic methods
  /// </summary>
  public class ExpandoWrapper :  Expando
  {
    object instance;

    public ExpandoWrapper(object instance)
    {
      this.instance = instance;
    }
    
    [Bindable(false)]
    public override object Instance
    {
      get
      {
        return instance;
      }
    }

    [Bindable(false)]
    public override IMemberBind GetMember(string name, params object[] arguments)
    {
      IMemberBind bind = RuntimeHost.Binder.BindToMember(instance, name, true);
      if (bind != null)
        return bind;

      return base.GetMember(name, arguments);
    }

    [Bindable(false)]
    public override IObjectBind GetMethod(string name, params object[] arguments)
    {
      IObjectBind bind = RuntimeHost.Binder.BindToMethod(instance, name, null, arguments);
      if (bind != null)
        return bind;

      return base.GetMethod(name, arguments);
    }
  }
}
