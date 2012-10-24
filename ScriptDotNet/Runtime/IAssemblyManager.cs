using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using ScriptNET.Runtime.Configuration;

namespace ScriptNET.Runtime
{
  public interface IAssemblyManager : IDisposable
  {
    void Initialize(ScriptConfiguration configuration);

    void AddAssembly(Assembly assembly);

    void RemoveAssembly(Assembly assembly);

    Type GetType(string name);

    bool HasType(string name);

    bool HasNamespace(string name);

    void AddType(string alias, Type type);

    event EventHandler<AssemblyHandlerEventArgs> BeforeAddAssembly;

    event EventHandler<AssemblyHandlerEventArgs> BeforeAddType;
  }

  public class AssemblyHandlerEventArgs : EventArgs
  {
    public bool Cancel { get; set; }

    public Assembly Assembly { get; private set; }

    public Type Type { get; private set; }

    public AssemblyHandlerEventArgs(Assembly assembly, Type type)
    {
      Cancel = false;
      Assembly = assembly;
      Type = type;
    }
  }
}
