#region using
using System.Collections.Generic;

using Irony.Compiler;
using ScriptNET.Runtime;
using System;
using System.Linq;
#endregion

namespace ScriptNET.Ast
{
  /// <summary>
  /// Qualified Name
  /// </summary>
  internal class ScriptQualifiedName : ScriptExpr
  {
    #region Members
    private string Identifier;
    private List<ScriptAst> Modifiers;
    private ScriptQualifiedName NamePart;

    private delegate void EvaluateFunction(IScriptContext context);
    private delegate void AssignFunction(object value, IScriptContext context);

    private EvaluateFunction evaluation;
    private AssignFunction assignment;
    #endregion

    #region Constructor
    public ScriptQualifiedName(AstNodeArgs args)
      : base(args)
    {
      if (ChildNodes.Count == 2 && ChildNodes[1].ChildNodes.Count == 0)
      {
        Identifier = ((Token)ChildNodes[0]).Text;

        evaluation = EvaluateIdentifier;
        assignment = AssignIdentifier;
      }
      else
        if (ChildNodes[0] is Token && ChildNodes[1].ChildNodes.Count != 0)
        {
          Identifier = (ChildNodes[0] as Token).Text;

          //NOTE: There might be two cases:
          //      1) a()[]...() 
          //      2) a<>.(NamePart)    
          Modifiers = new List<ScriptAst>();
          foreach (ScriptAst node in ChildNodes[1].ChildNodes)
            Modifiers.Add(node);

          ScriptGenericsPostfix generic = Modifiers.FirstOrDefault() as ScriptGenericsPostfix;
          if (generic != null && Modifiers.Count == 1)
          {
            //Case 2
            evaluation = EvaluateGenericType;
            assignment = null;
          }
          else
          {
            //Case 1
            evaluation = EvaluateFunctionCall;
            assignment = AssignArray;
          }
        }
        else
        {
          NamePart = ChildNodes[0] as ScriptQualifiedName;
          Identifier = ((Token)ChildNodes[2]).Text;
          if (ChildNodes.Count == 4 && ChildNodes[3].ChildNodes.Count != 0)
          {
            Modifiers = new List<ScriptAst>();
            foreach (ScriptAst node in ChildNodes[3].ChildNodes)
            {
              Modifiers.Add(node);
            }
          }
          evaluation = EvaluateNamePart;
          assignment = AssignNamePart;
        }
    }
    #endregion

    #region Public Methods
    public override void Evaluate(IScriptContext context)
    {
      evaluation(context);
    }

    public void Assign(object value, IScriptContext context)
    {
      assignment(value, context);
    }
    #endregion

    #region Identifier
    private void EvaluateIdentifier(IScriptContext context)
    {
      object result = GetIndentifierValue(context, Identifier);
      context.Result = result;
    }

    private static object GetIndentifierValue(IScriptContext context, string identifier)
    {
      object result = context.GetItem(identifier, false);
      if (result != RuntimeHost.NoVariable) return result;

      if (RuntimeHost.HasType(identifier))
        return RuntimeHost.GetType(identifier);
      else
        return NameSpaceResolver.Get(identifier);
    }

    private void AssignIdentifier(object value, IScriptContext context)
    {
      context.SetItem(Identifier, value);
    }
    #endregion

    #region Single Call
    private void EvaluateGenericType(IScriptContext context)
    {
      ScriptGenericsPostfix genericPostfix = (ScriptGenericsPostfix)Modifiers.First();

      Type genericType = GetIndentifierValue(context, genericPostfix.GetGenericTypeName(Identifier)) as Type;
      if (genericType == null || !genericType.IsGenericType)
      {
        throw new ScriptException("Given type is not generic");
      }

      genericPostfix.Evaluate(context);
      context.Result = genericType.MakeGenericType((Type[])context.Result);
    }

    private void EvaluateFunctionCall(IScriptContext context)
    {
      EvaluateIdentifier(context);

      foreach (ScriptAst node in Modifiers)
      {
        ScriptFunctionCall funcCall = node as ScriptFunctionCall;
        if (funcCall != null)
        {
          IInvokable function = context.Result as IInvokable;
          if (function == null)
            throw new ScriptException("Is not a function type");
          context.Result = CallFunction(function, funcCall, context);
          continue;
        }

        ScriptArrayResolution arrayResolution = node as ScriptArrayResolution;
        if (arrayResolution != null)
        {
          GetArrayValue(context.Result, arrayResolution, context);
          continue;
        }

        ScriptGenericsPostfix genericPostfix = node as ScriptGenericsPostfix;
        if (genericPostfix != null)
        {
          throw new NotSupportedException();
          //genericPostfix.Evaluate(Context);
          //continue;
        }

      }
    }

    private void AssignArray(object value, IScriptContext context)
    {
      object obj = context.GetItem(Identifier, true);

      foreach (ScriptAst node in Modifiers)
      {
        ScriptFunctionCall functionCall = node as ScriptFunctionCall;
        if (functionCall != null)
        {
          obj = CallFunction(context.GetFunctionDefinition(Identifier), functionCall, context);
          continue;
        }

        ScriptArrayResolution arrayResolution = node as ScriptArrayResolution;
        if (arrayResolution != null)
        {
          SetArrayValue(obj, arrayResolution, context, value);
          continue;
        }

        ScriptGenericsPostfix genericPostfix = node as ScriptGenericsPostfix;
        if (genericPostfix != null)
        {
          throw new NotSupportedException();
        }
      }
    }

    private static void SetArrayValue(object obj, ScriptArrayResolution scriptArrayResolution, IScriptContext context, object value)
    {
      scriptArrayResolution.Evaluate(context);

      object[] indexParameters = (object[])context.Result;
      object[] setterParameters = new object[indexParameters.Length + 1];
      indexParameters.CopyTo(setterParameters, 0);
      setterParameters[indexParameters.Length] = value;

      IObjectBind setter = RuntimeHost.Binder.BindToIndex(obj, setterParameters, true);
      if (setter != null)
      {
        setter.Invoke(context, null);
        return;
      }

      throw MethodNotFoundException("setter", indexParameters);
    }

    private static void GetArrayValue(object obj, ScriptArrayResolution scriptArrayResolution, IScriptContext context)
    {
      scriptArrayResolution.Evaluate(context);
      object[] param = (object[])context.Result;
      IObjectBind indexBind = RuntimeHost.Binder.BindToIndex(obj, param, false);

      if (indexBind != null)
      {
        context.Result = indexBind.Invoke(context, null);
      }
      else
      {
        throw MethodNotFoundException("indexer[]", param);
      }
    }
    #endregion

    #region Name Part
    private void EvaluateNamePart(IScriptContext context)
    {
      NamePart.Evaluate(context);
      object obj = context.Result;

      if (Modifiers == null)
      {
        context.Result = GetMemberValue(obj, Identifier);
        return;
      }

      Type[] genericArguments = null;
      foreach (ScriptAst node in Modifiers)
      {
        //NOTE: Generic modifier should be the first among other modifiers in the list
        ScriptGenericsPostfix generic = node as ScriptGenericsPostfix;
        if (generic != null)
        {
          if (genericArguments != null)
          {
            throw new ScriptException("Wrong modifiers sequence");
          }

          generic.Execute(context);
          genericArguments = (Type[])context.Result;
          continue;
        }

        ScriptFunctionCall functionCall = node as ScriptFunctionCall;
        if (functionCall != null)
        {
          CallClassMethod(obj, Identifier, functionCall, genericArguments, context);
          continue;
        }

        ScriptArrayResolution arrayResolution = node as ScriptArrayResolution;
        if (arrayResolution != null)
        {
          GetArrayValue(GetMemberValue(obj, Identifier), arrayResolution, context);
          continue;
        }
      }
    }

    private void AssignNamePart(object value, IScriptContext context)
    {
      NamePart.Evaluate(context);
      object obj = context.Result;

      if (Modifiers == null)
      {
        SetMember(context, obj, value);
        return;
      }

      //TODO: Bug, first evaluate get member, see unit test AssignmentToArrayObject
      string localIdentifier = Identifier;
      foreach (ScriptAst node in Modifiers)
      {
        ScriptFunctionCall scriptFunctionCall = node as ScriptFunctionCall;
        if (scriptFunctionCall != null)
        {
          if (localIdentifier != null)
          {
            CallClassMethod(obj, localIdentifier, scriptFunctionCall, null, context);
            obj = context.Result;
            localIdentifier = null;
          }
          else
          {
            IInvokable funcDef = obj as IInvokable;
            if (funcDef == null) throw new ScriptException("Attempt to invoke non IInvokable object.");
            obj = CallFunction(funcDef, scriptFunctionCall, context);
          }

          continue;
        }

        ScriptArrayResolution scriptArrayResolution = node as ScriptArrayResolution;
        if (scriptArrayResolution != null)
        {
          if (localIdentifier != null)
          {
            obj = GetMemberValue(obj, localIdentifier);
            localIdentifier = null;
          }
          SetArrayValue(obj, scriptArrayResolution, context, value);
          continue;
        }
      }
    }
    #endregion

    #region Call Function
    private static object CallFunction(IInvokable functionDefinition, ScriptFunctionCall scriptFunctionCall, IScriptContext context)
    {
      scriptFunctionCall.Evaluate(context);
      return functionDefinition.Invoke(context, (object[])context.Result);
    }

    private void CallClassMethod(object obj, string memeberInfo, ScriptFunctionCall scriptFunctionCall, Type[] genericArguments, IScriptContext context)
    {
      scriptFunctionCall.Evaluate(context);
      context.Result = CallAppropriateMethod(context, obj, memeberInfo, genericArguments, (object[])context.Result);
    }

    private static object CallAppropriateMethod(IScriptContext context, object obj, string Name, Type[] genericArguments, object[] param)
    {
      IObjectBind methodBind = methodBind = RuntimeHost.Binder.BindToMethod(obj, Name, genericArguments, param);
      if (methodBind != null)
        return methodBind.Invoke(context, null);

      throw MethodNotFoundException(Name, param);
    }
    #endregion

    #region Helpers
    private void SetMember(IScriptContext context, object obj, object value)
    {
      IMemberBind bind = RuntimeHost.Binder.BindToMember(obj, Identifier, true);
      if (bind == null)
        throw new ScriptIdNotFoundException(Identifier);

      bind.SetValue(value);
      context.Result = value;
      
      //Context.Result = RuntimeHost.Binder.Set(Identifier, obj, value);
    }

    private static object GetMemberValue(object obj, string memberInfo)
    {
      IMemberBind bind = RuntimeHost.Binder.BindToMember(obj, memberInfo, true);
      if (bind == null)
        throw new ScriptIdNotFoundException(memberInfo);

      return bind.GetValue();
     
      //return RuntimeHost.Binder.Get(memberInfo, obj);
    }

    private static ScriptMethodNotFoundException MethodNotFoundException(string Name, object[] param)
    {
      string message = "";
      foreach (object t in param)
      {
        if (t != null)
        {
          if (string.IsNullOrEmpty(message)) message += t.GetType().Name;
          else message += ", " + t.GetType().Name;
        }
      }
      return new ScriptMethodNotFoundException("Semantic: There is no method with such signature: " + Name + "(" + message + ")");
    }
    #endregion
  }
    
}