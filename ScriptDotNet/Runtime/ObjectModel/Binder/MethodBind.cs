using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ScriptNET.Runtime
{
  /// <summary>
  /// Binding to the method or constructor, also used for index getters
  /// </summary>
  internal class MethodBind : IObjectBind
  {
    MethodBase Method { get; set; }
    object Target { get; set; }
    object[] Arguments { get; set; }

    /// <summary>
    /// Creates binding to the method
    /// </summary>
    /// <param name="method">method or constructor to invoke</param>
    /// <param name="target">method's owner or type if method is static</param>
    /// <param name="arguments">a list of converted arguments that will be directly passed to the method</param>
    public MethodBind(MethodBase method, object target, object[] arguments)
    {
      Method = method;
      Arguments = arguments;
      Target = target;
    }

    #region IInvokable Members

    public bool CanInvoke()
    {
      return Method != null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="args">Ignored for strongly bound objects (null should be passed)</param>
    /// <returns></returns>
    public object Invoke(IScriptContext context, object[] args)
    {
      //object result = RuntimeHost.NullValue;
      //if (args != null)
      //{
      //  if (Target == null && !Method.IsStatic)
      //    result = Method.Invoke(args.First(), args.Skip(1).ToArray());
      //  else
      //    result = Method.Invoke(Target, args);
      //}
      //else
      //{
      //  result = Method.Invoke(Target, Arguments);
      //}
      //context.Result = result;     
      //return result;      
      return Method.Invoke(Target, Arguments);
    }

    #endregion

#if !PocketPC && !SILVERLIGHT
    //TODO: Review this approach
    public static implicit operator IntPtr(MethodBind invokableMethod)
    {
      return invokableMethod.Method.MethodHandle.GetFunctionPointer();
    }
#endif
  }
}
