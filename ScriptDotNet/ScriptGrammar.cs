#region Copyright
/*
 * Product: IronyScript.NET
 * Author: Petro Protsyk
 * Date: 11.02.2008
 * Time: 20:58
 * Update: June 2008
 *  
 * 
 * History:
 * 
 *  17.03.2008:  Evaluation implementation with Visitor pattern.
 *  02.04.2008:  Large refactoring to use Evaluation function instead of Visitor
 *   June 2008:  Getting things done
 */
#endregion

#region Using
using System;
using System.Globalization;
using System.Collections.Generic;

using Irony.Compiler;

using ScriptNET.Ast;
using ScriptNET.Runtime;
#endregion

namespace ScriptNET
{
  /// <summary>
  /// This class represents Irony Grammar for Script.NET
  /// </summary>
  [Bindable(false)] 
  public class ScriptdotnetGrammar : Grammar
  {
    public ScriptdotnetGrammar(bool expressionGrammar)
    {
      #region 1. Terminals
      NumberLiteral n = TerminalFactory.CreateCSharpNumber("number");
      IdentifierTerminal v = CreateScriptNetIdentifier("Identifier");
      Terminal s = CreateScriptNetString("string");

      Terminal @is = Symbol("is");
      Terminal dot = Symbol(".", "dot");
      Terminal less = Symbol("<");
      Terminal greater = Symbol(">");
      Terminal arrow = Symbol("->");
      Terminal LSb = Symbol("[");
      Terminal RSb = Symbol("]");
      Terminal LCb = Symbol("(");
      Terminal RCb = Symbol(")");
      Terminal RFb = Symbol("}");
      Terminal LFb = Symbol("{");
      Terminal LMb = Symbol("<!");
      Terminal RMb = Symbol("!>");
      Terminal LGb = Symbol("<|");
      Terminal RGb = Symbol("|>");
      Terminal comma = Symbol(",");
      Terminal semicolon = Symbol(";");
      Terminal colon = Symbol(":");
      #endregion

      #region 2. Non-terminals
      #region 2.1 Expressions
      NonTerminal Expr = new NonTerminal("Expr", typeof(ScriptExpr));
      NonTerminal ConstExpr = new NonTerminal("ConstExpr", typeof(ScriptConstExpr));
      NonTerminal BinExpr = new NonTerminal("BinExpr", typeof(ScriptBinExpr));
      NonTerminal UnaryExpr = new NonTerminal("UnaryExpr", typeof(ScriptUnaryExpr));
      NonTerminal AssignExpr = new NonTerminal("AssignExpr", typeof(ScriptAssignExpr));
      NonTerminal TypeConvertExpr = new NonTerminal("TypeConvertExpr", typeof(ScriptTypeConvertExpr));
      NonTerminal IsExpr = new NonTerminal("IsExpr", typeof(ScriptIsExpr));
      NonTerminal MetaExpr = new NonTerminal("MetaExpr", typeof(ScriptMetaExpr));
      NonTerminal FuncDefExpr = new NonTerminal("FuncDefExpr", typeof(ScriptFunctionDefinition));  //typeof(ScriptFunctionDefExpression));

      NonTerminal TypeExpr = new NonTerminal("TypeExpr", typeof(ScriptTypeExpr));
      NonTerminal TypeConstructor = new NonTerminal("TypeConstructor", typeof(ScriptTypeConstructor));
      NonTerminal FunctionCall = new NonTerminal("FunctionCall", typeof(ScriptFunctionCall));
      NonTerminal ArrayResolution = new NonTerminal("ArrayResolution", typeof(ScriptArrayResolution));

      NonTerminal BinOp = new NonTerminal("BinOp");
      NonTerminal LUnOp = new NonTerminal("LUnOp");
      NonTerminal RUnOp = new NonTerminal("RUnOp");

      NonTerminal ArrayConstructor = new NonTerminal("ArrayConstructor", typeof(ScriptArrayConstructor));
      NonTerminal MObjectConstructor = new NonTerminal("MObjectConstructor", typeof(ScriptMObject));
      NonTerminal MObjectPart = new NonTerminal("MObjectPart", typeof(ScriptMObjectPart));
      NonTerminal MObjectParts = new NonTerminal("MObjectPart", typeof(ScriptAst));

      NonTerminal TypeList = new NonTerminal("TypeList", typeof(ScriptTypeExprList));
      #endregion

      #region 2.2 QualifiedName
      //Expression List:  expr1, expr2, expr3, ..
      NonTerminal ExprList = new NonTerminal("ExprList", typeof(ScriptExprList));

      //A name in form: a.b.c().d[1,2].e ....
      NonTerminal NewStmt = new NonTerminal("NewStmt", typeof(ScriptNewStmt));
      NonTerminal NewArrStmt = new NonTerminal("NewArrStmt", typeof(ScriptNewArrStmt));
      NonTerminal QualifiedName = new NonTerminal("QualifiedName", typeof(ScriptQualifiedName));
      NonTerminal GenericsPostfix = new NonTerminal("GenericsPostfix", typeof(ScriptGenericsPostfix));

      NonTerminal GlobalList = new NonTerminal("GlobalList", typeof(ScriptGlobalList));
      #endregion

      #region 2.3 Statement
      NonTerminal Condition = new NonTerminal("Condition", typeof(ScriptCondition));
      NonTerminal Statement = new NonTerminal("Statement", typeof(ScriptStatement));

      NonTerminal IfStatement = new NonTerminal("IfStatement", typeof(ScriptIfStatement));
      NonTerminal WhileStatement = new NonTerminal("WhileStatement", typeof(ScriptWhileStatement));
      NonTerminal ForStatement = new NonTerminal("ForStatement", typeof(ScriptForStatement));
      NonTerminal ForEachStatement = new NonTerminal("ForEachStatement", typeof(ScriptForEachStatement));
      NonTerminal OptionalExpression = new NonTerminal("OptionalExpression", typeof(ScriptExpr));
      NonTerminal SwitchStatement = new NonTerminal("SwitchStatement", typeof(ScriptStatement));
      NonTerminal SwitchStatements = new NonTerminal("SwitchStatements", typeof(ScriptSwitchStatement));
      NonTerminal SwitchCaseStatement = new NonTerminal("SwitchCaseStatement", typeof(ScriptSwitchCaseStatement));
      NonTerminal SwitchDefaultStatement = new NonTerminal("SwitchDefaultStatement", typeof(ScriptSwitchDefaultStatement));
      NonTerminal UsingStatement = new NonTerminal("UsingStatement", typeof(ScriptUsingStatement));
      NonTerminal TryCatchFinallyStatement = new NonTerminal("TryCatchFinallyStatement", typeof(ScriptTryCatchFinallyStatement));
      NonTerminal FlowControlStatement = new NonTerminal("FlowControl", typeof(ScriptFlowControlStatement));
      NonTerminal ExprStatement = new NonTerminal("ExprStatement", typeof(ScriptStatement));

      //Block
      NonTerminal BlockStatement = new NonTerminal("BlockStatement", typeof(ScriptStatement));
      NonTerminal Statements = new NonTerminal("Statements(Compound)", typeof(ScriptCompoundStatement));
      #endregion

      #region 2.4 Program and Functions
      NonTerminal Prog = new NonTerminal("Prog", typeof(ScriptProg));
      NonTerminal Element = new NonTerminal("Element", typeof(ScriptAst));
      NonTerminal Elements = new NonTerminal("Elements", typeof(ScriptElements));
      NonTerminal FuncDef = new NonTerminal("FuncDef", typeof(ScriptFunctionDefinition));
      NonTerminal FuncContract = new NonTerminal("FuncContract", typeof(ScriptFuncContract));
      NonTerminal ParameterList = new NonTerminal("ParamaterList", typeof(ScriptFuncParameters));

      NonTerminal FuncContractPre = new NonTerminal("Pre Conditions", typeof(ScriptFuncContractPre));
      NonTerminal FuncContractPost = new NonTerminal("Post Conditions", typeof(ScriptFuncContractPost));
      NonTerminal FuncContractInv = new NonTerminal("Invariant Conditions", typeof(ScriptFuncContractInv));
      #endregion

      #endregion

      #region 3. BNF rules
      #region 3.1 Expressions
      ConstExpr.Rule = Symbol("true")
                      | "false"
                      | "null"
                      | s
                      | n;

      BinExpr.Rule = Expr + BinOp + Expr
                     | IsExpr;

      UnaryExpr.Rule = LUnOp + Expr;

      IsExpr.Rule = Expr + @is + TypeExpr;

      TypeConvertExpr.Rule = LCb + Expr + RCb + Expr.Q();
                        
      AssignExpr.Rule = QualifiedName + "=" + Expr
                       | QualifiedName + "++"
                       | QualifiedName + "--"
                       | QualifiedName + ":=" + Expr
                       | QualifiedName + "+=" + Expr
                       | QualifiedName + "-=" + Expr;

      //TODO: MetaFeatures;
      // <[    ] + > because of conflict a[1]>2
      MetaExpr.Rule = LMb + Elements + RMb;

      GlobalList.Rule = "global" + LCb + ParameterList + RCb;

      FuncDefExpr.Rule = "function" + LCb + ParameterList + RCb 
        + GlobalList.Q()
        + FuncContract.Q()
        + BlockStatement;

      Expr.Rule =   ConstExpr
                  | BinExpr
                  | UnaryExpr
                  | QualifiedName
                  | AssignExpr
                  | NewStmt
                  | FuncDefExpr
                  | NewArrStmt
                  | ArrayConstructor
                  | MObjectConstructor                  
                  | TypeConvertExpr
                  | MetaExpr
                  ;

      NewStmt.Rule = "new" + TypeConstructor;
      NewArrStmt.Rule = "new" + TypeExpr + ArrayResolution;
      BinOp.Rule = Symbol("+") | "-" | "*" | "/" | "%" | "^" | "&" | "|"
                  | "&&" | "||" | "==" | "!=" | greater | less 
                  | ">=" | "<=";

      LUnOp.Rule = Symbol("~") | "-" | "!" | "$";

      ArrayConstructor.Rule = LSb + ExprList + RSb;
      
      MObjectPart.Rule = v + arrow + Expr;
      MObjectParts.Rule = MakePlusRule(MObjectParts, comma, MObjectPart);
      MObjectConstructor.Rule = LSb + MObjectParts + RSb;   

      OptionalExpression.Rule = Expr.Q();
      #endregion

      #region 3.2 QualifiedName
      TypeExpr.Rule = //MakePlusRule(TypeExpr, dot, v);
          v + GenericsPostfix.Q()
          | TypeExpr + dot + (v + GenericsPostfix.Q());

      GenericsPostfix.Rule = LGb + TypeList + RGb;
      FunctionCall.Rule = LCb + ExprList.Q() + RCb;
      ArrayResolution.Rule = LSb + ExprList + RSb;
        
      QualifiedName.Rule = v + (GenericsPostfix | ArrayResolution | FunctionCall).Star()
                          | QualifiedName + dot + v + (GenericsPostfix | ArrayResolution | FunctionCall).Star();

      ExprList.Rule = MakePlusRule(ExprList, comma, Expr);
      TypeList.Rule = MakePlusRule(TypeList, comma, TypeExpr);
      TypeConstructor.Rule = TypeExpr + FunctionCall;
      #endregion

      #region 3.3 Statement
      Condition.Rule = LCb + Expr + RCb;
      IfStatement.Rule = "if" + Condition + Statement + ("else" + Statement).Q();
      WhileStatement.Rule = "while" + Condition + Statement;
      ForStatement.Rule = "for" + LCb + OptionalExpression + semicolon + OptionalExpression + semicolon + OptionalExpression + RCb + Statement;      
      ForEachStatement.Rule = "foreach" + LCb + v + "in" + Expr + RCb + Statement;
      UsingStatement.Rule = "using" + LCb + Expr + RCb + BlockStatement;
      TryCatchFinallyStatement.Rule = "try" + BlockStatement + "catch" + LCb + v + RCb + BlockStatement + "finally" + BlockStatement;
      SwitchStatement.Rule = "switch" + LCb + Expr + RCb + LFb + SwitchStatements + RFb;
      ExprStatement.Rule = Expr + semicolon;
      FlowControlStatement.Rule = "break" + semicolon
                                | "continue" + semicolon
                                | "return" + Expr + semicolon
                                | "throw" + Expr + semicolon;
                 
      Statement.Rule = semicolon
                      | IfStatement                 //1. If
                      | WhileStatement              //2. While
                      | ForStatement                //3. For
                      | ForEachStatement            //4. ForEach
                      | UsingStatement              //5. Using
                      | SwitchStatement             //6. Switch
                      | BlockStatement              //7. Block
                      | TryCatchFinallyStatement    //8. TryCatch
                      | ExprStatement               //9. Expr
                      | FlowControlStatement;       //10. FlowControl

      Statements.SetOption(TermOptions.IsList);
      Statements.Rule = Statements + Statement | Empty;
      BlockStatement.Rule = LFb + Statements + RFb;

      SwitchStatements.Rule = SwitchCaseStatement.Star() + SwitchDefaultStatement.Q();
      SwitchCaseStatement.Rule = Symbol("case") + Expr + colon + Statements;
      SwitchDefaultStatement.Rule = "default" + colon + Statements;
      #endregion

      #region 3.4 Prog
      FuncContract.Rule = LSb +
                           FuncContractPre + semicolon +
                           FuncContractPost + semicolon +
                           FuncContractInv + semicolon +
                          RSb;
      FuncContractPre.Rule = "pre" + LCb + ExprList.Q() + RCb;
      FuncContractPost.Rule = "post" + LCb + ExprList.Q() + RCb;
      FuncContractInv.Rule = "invariant" + LCb + ExprList.Q() + RCb;
      
      ParameterList.Rule = MakeStarRule(ParameterList, comma, v);
      FuncDef.Rule = "function" + v + LCb + ParameterList + RCb
        + GlobalList.Q()
        + FuncContract.Q()
        + BlockStatement;

      Element.Rule = Statement | FuncDef;
      Elements.SetOption(TermOptions.IsList);
      Elements.Rule = Elements + Element | Empty;
      
      Prog.Rule = Elements + Eof;

      Terminal Comment = new CommentTerminal("Comment", "/*", "*/");
      NonGrammarTerminals.Add(Comment);
      Terminal LineComment = new CommentTerminal("LineComment", "//", "\n");
      NonGrammarTerminals.Add(LineComment);

      #endregion
      #endregion

      #region 4. Set starting symbol
      if (!expressionGrammar)
        Root = Prog; // Set grammar root
      else
        Root = Expr;
      #endregion

      #region 5. Operators precedence
      RegisterOperators(1, "=", "+=", "-=", ":=");
      RegisterOperators(2, "|", "||");
      RegisterOperators(3, "&", "&&");      
      RegisterOperators(4, "==", "!=", ">", "<", ">=", "<=");    
      RegisterOperators(5, "is");
      RegisterOperators(6, "+", "-");
      RegisterOperators(7, "*", "/", "%");
      RegisterOperators(8, Associativity.Right, "^");
      RegisterOperators(9, "~", "!", "$", "++", "--");
      RegisterOperators(10, ".");

      //RegisterOperators(10, Associativity.Right, ".",",", ")", "(", "]", "[", "{", "}");
      //RegisterOperators(11, Associativity.Right, "else");
      #endregion

      #region 6. Punctuation symbols
      RegisterPunctuation( "(", ")", "[", "]", "{", "}", ",", ";" );
      #endregion
    }

    #region Compiler
    public static readonly LanguageCompiler Compiler = new LanguageCompiler(new ScriptdotnetGrammar(false));
    public static readonly LanguageCompiler ExpressionCompiler = new LanguageCompiler(new ScriptdotnetGrammar(true));
    #endregion

    #region Helpers
    /// <summary>
    /// Creates identifier terminal for script grammar
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static IdentifierTerminal CreateScriptNetIdentifier(string name)
    {
      IdentifierTerminal id = new IdentifierTerminal(name);
      id.SetOption(TermOptions.CanStartWithEscape);
      id.AddKeywords("true", "false", "null", "if", "else",
                   "while", "for", "foreach", "in",
                   "switch", "case", "default", "break",
                   "continue", "return", "function", "is",
                   "pre", "post", "invariant", "new", "using",
                   "global");

      id.AddPrefixFlag("@", ScanFlags.IsNotKeyword | ScanFlags.DisableEscapes);
      //From spec:
      //Start char is "_" or letter-character, which is a Unicode character of classes Lu, Ll, Lt, Lm, Lo, or Nl 
      id.StartCharCategories.AddRange(new UnicodeCategory[] {
         UnicodeCategory.UppercaseLetter, //Ul
         UnicodeCategory.LowercaseLetter, //Ll
         UnicodeCategory.TitlecaseLetter, //Lt
         UnicodeCategory.ModifierLetter,  //Lm
         UnicodeCategory.OtherLetter,     //Lo
         UnicodeCategory.LetterNumber     //Nl
      });
      //Internal chars
      /* From spec:
      identifier-part-character: letter-character | decimal-digit-character | connecting-character |  combining-character |
          formatting-character
*/
      id.CharCategories.AddRange(id.StartCharCategories); //letter-character categories
      id.CharCategories.AddRange(new UnicodeCategory[] {
        UnicodeCategory.DecimalDigitNumber, //Nd
        UnicodeCategory.ConnectorPunctuation, //Pc
        UnicodeCategory.SpacingCombiningMark, //Mc
        UnicodeCategory.NonSpacingMark,       //Mn
        UnicodeCategory.Format                //Cf
      });
      //Chars to remove from final identifier
      id.CharsToRemoveCategories.Add(UnicodeCategory.Format);
      return id;
    }

    private static StringLiteral CreateScriptNetString(string name)
    {
      StringLiteral term = new StringLiteral(name, TermOptions.None);
      term.AddStartEnd("'", ScanFlags.AllowAllEscapes);
      term.AddStartEnd("\"", ScanFlags.AllowAllEscapes);
      term.AddPrefixFlag("@", ScanFlags.DisableEscapes | ScanFlags.AllowLineBreak | ScanFlags.AllowDoubledQuote);
      return term;
    }
    #endregion
  }
}
