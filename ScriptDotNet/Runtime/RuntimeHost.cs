#region Information
/*
 by Petro Protsyk
 28.11.2008, 22:16
 */
#endregion

#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

using ScriptNET.Runtime.Configuration;
using ScriptNET.Runtime.Operators;

#if SILVERLIGHT || PocketPC
using DefaultAssemblyManager = ScriptNET.Runtime.BaseAssemblyManager;
using DefaultActivator = ScriptNET.Runtime.ObjectActivator;
using DefaultBinder = ScriptNET.Runtime.ObjectBinderExtended;
#else
using DefaultAssemblyManager = ScriptNET.Runtime.AssemblyManager;
using DefaultActivator = ScriptNET.Runtime.ObjectActivator;
using DefaultBinder = ScriptNET.Runtime.ObjectBinderExtended;
#endif
#endregion

namespace ScriptNET.Runtime
{
    /// <summary>
    /// Run-time configuration manager for Script.net
    /// </summary>
    public static class RuntimeHost
    {
        #region Fields
        private static ScriptConfiguration Configuration = null;

        private static Script InitializationScript = null;

        /// <summary>
        /// Settings section
        /// </summary>
        private static readonly Dictionary<string, object> SettingsItems = new Dictionary<string, object>();

        /// <summary>
        /// Operators
        /// </summary>
        private static readonly Dictionary<string, IOperator> BinOperators = new Dictionary<string, IOperator>();

        /// <summary>
        /// Operators
        /// </summary>
        private static readonly Dictionary<string, IOperator> UnaryOperators = new Dictionary<string, IOperator>();

        /// <summary>
        /// Object used to synchronize during multi-threaded execution
        /// </summary>
        private static object syncRoot = new object();

        [Bindable(false)]
        public static IScopeFactory ScopeFactory {get; private set;}

        public static IObjectBinder Binder { get; set; }

        [Bindable(false)]
        public static IAssemblyManager AssemblyManager { get; set; }

        /// <summary>
        /// Activator which used to activate instances
        /// </summary>
        public static IObjectActivator Activator { get; set; }

        private static readonly Dictionary<string, IOperatorHandler> Handlers = new Dictionary<string, IOperatorHandler>();

        /// <summary>
        /// Should be returned by GetVariableInternal if item with given
        /// name not found in the scope hierarchy
        /// </summary>
        public static object NoVariable = new object();
      
        public static object NullValue = new object();
        #endregion

        #region Construction & Initialization
        /// <summary>
        /// Load default configuration from RuntimeConfig.xml
        /// </summary>
        [Bindable(false)]
        public static void Initialize()
        {
            Initialize(DefaultConfig);
        }

        /// <summary>
        /// Loads given configuration
        /// </summary>
        /// <param name="configuration"></param>
        [Bindable(false)]
        public static void Initialize(Stream configuration)
        {
            Lock();
            try
            {
                LoadConfiguration(configuration);
                
                if (Binder == null)
                {
                  Binder = new DefaultBinder();
                }

                if (Activator == null)
                {
                  Activator = new DefaultActivator();
                }

                InitializeSettingItems();
                RegisterOperators();

                if (ScopeFactory == null)
                {
                  ScopeFactory = Activator.CreateInstance(GetNativeType(GetSettingsItem<string>(ConfigSchema.ScopeFactoryAttribute))) as IScopeFactory;
                }
                RegisterScopes();

                if (AssemblyManager == null)
                {
                  AssemblyManager = new DefaultAssemblyManager();
                }
                OnInitializingTypes(AssemblyManager);
                AssemblyManager.Initialize(Configuration);
                
                if (!string.IsNullOrEmpty(Configuration.Initialization))
                    InitializationScript = Script.Compile(Configuration.Initialization);

                RegisterOperatorHandler("+=", new EventOperatorHandler(true));
                RegisterOperatorHandler("-=", new EventOperatorHandler(false));
                
                ScriptNET.Ast.ScriptExpr.HandleOperator += HandleOperator;
            }
            finally
            {
                UnLock();
            }
        }

        private static void RegisterOperators()
        {
          foreach (OperatorDefinition definition in Configuration.Operators)
          {
            IOperator oper = (IOperator)Activator.CreateInstance(GetNativeType(definition.Type));
            if (oper.Unary)
              UnaryOperators.Add(oper.Name, oper);
            else
              BinOperators.Add(oper.Name, oper);
          }
        }

        private static void HandleOperator(object sender, HandleOperatorArgs e)
        {
          if (Handlers.ContainsKey(e.Symbol))
          {
            e.Result = Handlers[e.Symbol].Process(e);
          }
        }

        private static void RegisterScopes()
        {
            //NOTE: Default values
            //ScopeFactory.RegisterType(ScopeTypes.Default, typeof(ScriptScope));
            //ScopeFactory.RegisterType(ScopeTypes.Contract, typeof(ScriptContractScope));
            //ScopeFactory.RegisterType(ScopeTypes.Using, typeof(ScriptUsingScope));

            foreach (ScopeDefinition definition in Configuration.Scopes)
            {
              ScopeFactory.RegisterType(definition.Id, (IScopeActivator)Activator.CreateInstance(GetNativeType(definition.Type)));
            }
        }

        /// <summary>
        /// Clears all information in the RuntimeHost
        /// </summary>
        [Bindable(false)] 
        public static void CleanUp()
        {
            Lock();
            try
            {
                ScriptNET.Ast.ScriptExpr.HandleOperator -= HandleOperator;
                Handlers.Clear();
                initializingTypes.Clear();
                Binder = null;
                Activator = null;
                ScopeFactory = null;
                if (AssemblyManager != null)
                  AssemblyManager.Dispose();
                AssemblyManager = null;
                SettingsItems.Clear();
                BinOperators.Clear();
                UnaryOperators.Clear();
                InitializationScript = null;

                Configuration = new ScriptConfiguration();
            }
            finally
            {
                UnLock();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads language configuration from stream
        /// </summary>
        /// <param name="configStream"></param>
        private static void LoadConfiguration(Stream configStream)
        {
            XmlSerializer configurationSerializer = new XmlSerializer(typeof(ScriptConfiguration));
            Configuration = configurationSerializer.Deserialize(configStream) as ScriptConfiguration;
            if (Configuration == null)
                throw new ScriptException("Configuration has wrong format or empty");
        }
        #endregion

        #region Loading
        private static void InitializeSettingItems()
        {
          foreach (SettingXml item in Configuration.SettingXml)
          {
            object rez = item.Value;
            if (!string.IsNullOrEmpty(item.Converter))
              rez = GetItemValue(item.Value, item.Converter);

            SettingsItems.Add(item.Name, rez);
          }
        }

        private static object GetItemValue(string value, string converter)
        {
          Type converterType = GetNativeType(converter);
#if PocketPC || SILVERLIGHT
          try
          {
            return Convert.ChangeType(value, converterType, System.Globalization.CultureInfo.InvariantCulture);
          }
          catch
          {
            System.Diagnostics.Debug.WriteLine("Failed to convert string to type: " + converterType.ToString());
          }
#else
          TypeConverter converterObject = Activator.CreateInstance(converterType) as TypeConverter;
          if (converterObject != null && converterObject.CanConvertFrom(typeof(string)))
          {
            return converterObject.ConvertFrom(value);
          }
#endif
          return value;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Returns setting item specified in run-time config file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object GetSettingsItem(string id)
        { 
          if (SettingsItems.ContainsKey(id))
            return SettingsItems[id];

          return null;
        }

        public static void SetSettingItem(string id, object value)
        {
            Lock();
            try
            {
                if (SettingsItems.ContainsKey(id))
                {
                    SettingsItems[id] = value;
                }
                else
                {
                    SettingsItems.Add(id, value);
                }
            }
            finally
            {
                UnLock();
            }
        }
        /// <summary>
        /// Returns setting item specified in run-time config file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetSettingsItem<T>(string id)
        {
          object result = GetSettingsItem(id);
          return result == null ? default(T) : (T)result;
        }

        [Bindable(false)]
        public static IOperator GetBinaryOperator(string id)
        {
          if (!BinOperators.ContainsKey(id))
            throw new NotSupportedException(string.Format("Given operator {0} is not found", id));

          return BinOperators[id];
        }
        
        [Bindable(false)]
        public static IOperator GetUnaryOperator(string id)
        {
          if (!UnaryOperators.ContainsKey(id))
            throw new NotSupportedException(string.Format("Given operator {0} is not found", id));

          return UnaryOperators[id];
        }

        [Bindable(false)]
        public static void InitializeScript(IScriptContext context)
        {
          if (InitializationScript == null) return;

          Lock();
            InitializationScript.Context = context;
            InitializationScript.Execute();
            InitializationScript.Context = null;
          UnLock();
        }

        internal static Type GetNativeType(string name)
        {
          return Type.GetType(name);
        }
        
        public static Type GetType(string name)
        {
          return AssemblyManager.GetType(name);
        }

        public static bool HasType(string name)
        {
          return AssemblyManager.HasType(name);
        }
        
        [Bindable(false)]
        public static void AddType(string alias, Type type)
        {
          AssemblyManager.AddType(alias, type);
        }
        
        [Bindable(false)]
        public static void RegisterOperatorHandler(string operatorSymbol, IOperatorHandler handler)
        {
          if (!Handlers.ContainsKey(operatorSymbol))
            Handlers.Add(operatorSymbol, handler);
          else
            Handlers[operatorSymbol] = handler;
        }


        /// <summary>
        /// Lock's runtime host for threading execution 
        /// </summary>
        public static void Lock()
        {
            Monitor.Enter(syncRoot);
        }

        /// <summary>
        /// Unlock's thread
        /// </summary>
        public static void UnLock()
        {
            Monitor.Exit(syncRoot);
        }

        /// <summary>
        /// This event is raised before AssemblyManager starts creating type system.
        /// It should be used to subscribe on AssemblyManager's events in order to cancel
        /// loading some assemblies and adding particular types
        /// </summary>          
        public static event EventHandler<EventArgs> InitializingTypes
        {          
          add
          {
            initializingTypes.Add(value);
          }
          remove
          {
            initializingTypes.Remove(value);
          }
        }

        private static List<EventHandler<EventArgs>> initializingTypes = new List<EventHandler<EventArgs>>();
       
        private static void OnInitializingTypes(object sender)
        {
          foreach (EventHandler<EventArgs> handler in initializingTypes)
            handler.Invoke(sender, EventArgs.Empty);
        }
        #endregion

        #region Config
        public static Stream DefaultConfig
        {
            get
            {
                Stream configStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MapleShark.ScriptDotNet.RuntimeConfig.xml");
                configStream.Seek(0, SeekOrigin.Begin);
                return configStream;
            }
        }
        #endregion
    }
}
