using System;
using System.Reflection;
using System.IO;

using ScriptNET.Runtime;

namespace ScriptNET.CustomFunctions
{
  /// <summary>
  /// About function
  /// </summary>
  internal class AboutFunc : IInvokable
  {
    /// <summary>
    /// Function instance
    /// </summary>
    public static AboutFunc FunctionDefinition = new AboutFunc();
    /// <summary>
    /// Function Name
    /// </summary>
    public static string FunctionName = "About";

    private AboutFunc()
    {
    }

    #region IInvokable Members
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Always true</returns>
    public bool CanInvoke()
    {
      return true;
    }

    /// <summary>
    /// Executes function
    /// </summary>
    /// <param name="Context"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public object Invoke(IScriptContext context, object[] args)
    {
      string timeStamp = "25 March 2009";
      string message = "Script.NET with Irony Compiler, build " + Assembly.GetExecutingAssembly().GetName().Version + "\r\n" + timeStamp + " Kiev, Ukraine\r\n(c) Petro Protsyk\r\nPiter.Protsyk@gmail.com";

#if PocketPC
      System.Windows.Forms.MessageBox.Show(message);
#elif SILVERLIGHT
      
#else
      if (System.Windows.Forms.Form.ActiveForm != null)
      {
        System.Windows.Forms.MessageBox.Show(message);
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Gray;
      }
#endif
      return message;
    }

    #endregion
  }
}
