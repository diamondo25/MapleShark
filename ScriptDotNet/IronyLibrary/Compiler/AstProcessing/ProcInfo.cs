using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Irony.Compiler {
  [Flags]
  public enum ProcFlags {
    IsExternal     = 0x01,   // is interop method
    IsImported     = 0x02,   // same language but imported from another module
    IsClosure      = 0x04,   // 

    HasParamArray  = 0x100,      // The last argument is a param array
    TypeBasedDispatch   = 0x200, //uses dynamic dispatch based on types
  }
}

