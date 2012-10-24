using System;
using System.Collections.Generic;
using System.Reflection;

namespace ScriptNET.Runtime
{
  internal static class NameSpaceResolver
  {
    internal static NameSpace Get(string name)
    {
      return new NameSpace(name);
    }
  }

  /// <summary>
  /// Represents a root for namespace
  /// </summary>
  internal class NameSpace : IScriptable
  {
    /// <summary>
    /// Name of the namespace
    /// </summary>
    public string Name
    {
      get;
      set;
    }

    /// <summary>
    /// Base Contructor
    /// </summary>
    /// <param name="Name"></param>
    internal NameSpace(string name)
    {
      Name = name;

      if (!RuntimeHost.AssemblyManager.HasNamespace(name))
        throw new ScriptIdNotFoundException(string.Format("Namespace {0} is not found", name));
    }

    public override string ToString()
    {
      return string.Format("ns:{0}", Name);
    }

    #region IScriptable Members
    [Bindable(false)]
    public object Instance
    {
      get { return this; }
    }

    [Bindable(false)]
    public IMemberBind GetMember(string name, params object[] arguments)
    {
      return new NameSpaceBind(string.Format("{0}.{1}", Name, name));
    }
    
    [Bindable(false)]
    public IObjectBind GetMethod(string name, params object[] arguments)
    {
      throw new NotSupportedException();
    }

    #endregion

    #region NameSpaceBind
    private class NameSpaceBind : IMemberBind
    {
      string name;

      public NameSpaceBind(string name)
      {
        this.name = name;
      }

      #region IMemberBind Members

      public object Target
      {
        get { throw new NotSupportedException(); }
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
        throw new NotSupportedException();
      }

      public object GetValue()
      {
        if (RuntimeHost.HasType(name))
        {
          return RuntimeHost.GetType(name);
        }
        else
        {
          return new NameSpace(name);
        }
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
        throw new NotImplementedException();
      }

      public object Invoke(IScriptContext context, object[] args)
      {
        throw new NotImplementedException();
      }

      #endregion
    }

    #endregion
  }
}
