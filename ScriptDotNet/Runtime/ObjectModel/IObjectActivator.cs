using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Creates instances by given type information
  /// </summary>
  public interface IObjectActivator
  {
    object CreateInstance(Type type, object[] args);

    object CreateInstance(IScriptContext context, IObjectBind bind);
  }

  public static class ObjectActivatorExtensions
  {
    public static object CreateInstance(this IObjectActivator activator, IScriptContext context, IObjectBind bind)
    {
      return activator.CreateInstance(null, bind);
    }
    
    public static object CreateInstance(this IObjectActivator activator, Type type)
    {
      return activator.CreateInstance(type, null);
    }

    public static T CreateInstance<T>(this IObjectActivator activator) 
    {
      return (T)CreateInstance(activator, typeof(T));
    }

    public static T CreateInstance<T>(this IObjectActivator activator, object[] args)
    {
      return (T)activator.CreateInstance(typeof(T), args);
    }

  }

}
