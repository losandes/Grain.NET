using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using Grain.Attributes;

namespace Grain.Extensions
{
    public static partial class ClassExtensions
    {   
        /// <summary>
        /// Checks to see if a given type implements a given interface
        /// </summary>
        /// <param name="thisObjectType">Type: the type of object that might inherit the interface</param>
        /// <param name="typeOfInterface">Type: the type of interface</param>
        /// <returns>true if the given type inherits the interface</returns>
        public static bool ImplementsInterface(this Type thisObjectType, Type typeOfInterface) 
        {
            if (thisObjectType == null)
                throw new NullReferenceException("The object type is required to check if it implements an interface.");

            if (typeOfInterface == null)
                throw new NullReferenceException("The interface type is required to check if an object implements it.");

            return typeOfInterface.IsAssignableFrom(thisObjectType);
        }

        /// <summary>
        /// Checks to see if a given object implements a given interface
        /// </summary>
        /// <param name="thisObjectType">Type of T: the object that might inherit the interface</param>
        /// <param name="typeOfInterface">Type: the type of interface</param>
        /// <returns>true if the given object inherits the interface</returns>
        public static bool ImplementsInterface<T>(this T thisObject, Type typeOfInterface)
        {
            if (thisObject == null)
                throw new NullReferenceException("An object is required to check if it implements an interface.");

            if (typeOfInterface == null)
                throw new NullReferenceException("The interface type is required to check if an object implements it.");

            return thisObject.GetType().ImplementsInterface(typeOfInterface);
        }

        /// <summary>
        /// Checks to see if a given type implements a given generic interface
        /// </summary>
        /// <param name="thisObjectType">Type: the type of object that might inherit the interface</param>
        /// <param name="typeOfInterface">Type: the type of interface</param>
        /// <returns>true if the given type inherits the interface</returns>
        /// <example>foo.GetType().ImplementsGenericInterface(typeof(IComparableObject<>))</example>
        public static bool ImplementsGenericInterface(this Type thisObjectType, Type typeOfInterface)
        {
            if (thisObjectType == null)
                throw new NullReferenceException("The object type is required to check if it implements an interface.");

            if (typeOfInterface == null)
                throw new NullReferenceException("The interface type is required to check if an object implements it.");

            return thisObjectType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeOfInterface);
        }

        /// <summary>
        /// Checks to see if a given type implements a given generic interface
        /// </summary>
        /// <param name="thisObjectType">Type: the type of object that might inherit the interface</param>
        /// <param name="typeOfInterface">Type: the type of interface</param>
        /// <returns>true if the given type inherits the interface</returns>
        /// <example>foo.ImplementsGenericInterface(typeof(IComparableObject<>))</example>
        public static bool ImplementsGenericInterface<T>(this T thisObject, Type typeOfInterface)
        {
            if (thisObject == null)
                throw new NullReferenceException("An object is required to check if it implements an interface.");

            if (typeOfInterface == null)
                throw new NullReferenceException("The interface type is required to check if an object implements it.");

            return thisObject.GetType().ImplementsGenericInterface(typeOfInterface);
        }

        /// <summary>
        /// Returns all of the types within an assembly, and are derived from a given type
        /// </summary>
        /// <typeparam name="T">Type: the base type that other classes are derived from</typeparam>
        /// <param name="assembly">Assembly: the assembly to find derived types, within (i.e. Assembly.GetExecutingAssembly())</param>
        /// <returns>IEnumerable of Type: a collection of types that exist in this assembly and are derived from Type, T</returns>
        /// <example>
        /// Assembly.GetExecutingAssembly().FindDerivedTypes<IController>();
        /// </example>
        /// <remarks>
        /// Assembly
        ///        .GetExecutingAssembly()
        ///        .GetReferencedAssemblies()
        ///        .Select(name => Assembly.Load(name.FullName))
        ///        .SelectMany(assembly => assembly.GetTypes())
        ///        .Where(type => typeof(T).IsAssignableFrom(type))
        ///        .Where(type => !type.IsAbstract);
        /// </remarks>
        public static IEnumerable<Type> FindDerivedTypes<T>(this Assembly assembly)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();

            return assembly.GetReferencedAssemblies()
                    .Select(name => Assembly.Load(name.FullName))
                    .SelectMany(_assembly => _assembly.GetTypes())
                    .Where(type => typeof(T).IsAssignableFrom(type))
                    .Where(type => !type.IsAbstract);
        }

        /// <summary>
        /// Checks to see if a type is derived from another type
        /// </summary>
        /// <typeparam name="TBase">Type: the base type</typeparam>
        /// <param name="type">Type: the type that might be derived from the base type</param>
        /// <returns>bool: true if the type is derived from the base type</returns>
        public static bool TypeIsDerivedFrom<TBase>(Type type)
        {
            if (type == null)
                throw new NullReferenceException("The type is required to check if it is derived from a base type.");

            return typeof(TBase).IsAssignableFrom(type);
        }        
        
        /// <summary>
        /// Checks to see if a type is NOT derived from another type
        /// </summary>
        /// <typeparam name="TBase">Type: the base type</typeparam>
        /// <param name="type">Type: the type that might be derived from the base type</param>
        /// <returns>bool: true if the type is NOT derived from the base type</returns>
        public static bool TypeIsNotDerivedFrom<TBase>(Type type)
        {
            if (type == null)
                throw new NullReferenceException("The type is required to check if it is derived from a base type.");

            return !(typeof(TBase).IsAssignableFrom(type));
        }

        /// <summary>
        /// Get all of the Types in a given namespace
        /// </summary>
        /// <param name="assembly">Assembly: the assembly that the namespace exists in</param>
        /// <param name="nameSpace">string: the fully qualified path to the namespace</param>
        /// <returns>List of type Type: the Types in the given namespace</returns>
        [Cite(Author = "Fredrik Mörk", Link = "http://stackoverflow.com/questions/949246/how-to-get-all-classes-within-namespace", Type = CiteType.Adaptation)]
        public static IEnumerable<Type> FindTypesInNamespace(this Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
        }

        /// <summary>
        /// Returns a new instance of an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstanceOf<T>() 
        {
            //return new InstanceCreator().GetInstanceOf<T>();
            return (T)GetInstanceOf(typeof(T));
        }

        /// <summary>
        /// Returns a new instance of an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstanceOf<T>(Type[] parameterTypes, object[] parameters)
        {
            //return new InstanceCreator().GetInstanceOf<T>();
            return (T)GetInstanceOf(typeof(T), parameterTypes, parameters);
        }

        /// <summary>
        /// Returns a new instance of an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object GetInstanceOf(Type type)
        {
            //return new ObjectCreator().GetInstanceOf(type);
            return type.GetConstructor(new Type[] { }).Invoke(new object[] { });
        }

        /// <summary>
        /// Returns a new instance of an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object GetInstanceOf(Type type, Type[] parameterTypes, object[] parameters)
        {
            //return new ObjectCreator().GetInstanceOf(type);
            return type.GetConstructor(parameterTypes).Invoke(parameters);
        } 
    }


    public partial class ObjectCreator
    {
        public delegate object ObjectActivator(params object[] args);

        /// <summary>
        /// Dynamically get an instance of a class via a parameterless constructor, as fast as possible, without getting into IL.emit
        /// </summary>
        /// <param name="type">The type of object to invoke</param>
        /// <returns></returns>
        public object GetInstanceOf(Type type)
        {
            ConstructorInfo _ctor = type.GetConstructors().First();
            ObjectActivator _createdActivator = GetActivator(_ctor);
            return _createdActivator();
        }

        /// <summary>
        /// Dynamically get a class activator, as fast as possible, without getting into IL.emit
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [Cite(Author = "Roger Alsing", Link = "http://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/ ", Type = CiteType.Adaptation)]
        protected static ObjectActivator GetActivator(ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator), newExp, param);

            //compile it
            ObjectActivator compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }  

    }
    
    public partial class InstanceCreator 
    {
        public delegate T ObjectActivator<T>(params object[] args);

        /// <summary>
        /// Dynamically get an instance of a class via a parameterless constructor, as fast as possible, without getting into IL.emit
        /// </summary>
        /// <typeparam name="T">The type of object to invoke</typeparam>
        /// <returns></returns>
        public T GetInstanceOf<T>() 
        { 
            ConstructorInfo _ctor = typeof(T).GetConstructors().First();
            ObjectActivator<T> _createdActivator = GetActivator<T>(_ctor);
            return _createdActivator();
        }

        //public T GetInstanceOf<T>(object[] args)
        //{
        //    ConstructorInfo _ctor = typeof(T).GetConstructors().First();
        //    ObjectActivator<T> _createdActivator = GetActivator<T>(_ctor);
        //    return _createdActivator(args);
        //}
        
        /// <summary>
        /// Dynamically get a class activator, as fast as possible, without getting into IL.emit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [Cite(Author = "Roger Alsing", Link = "http://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/")]
        public static ObjectActivator<T> GetActivator<T>
            (ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param =
                Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp =
                new Expression[paramsInfo.Length];

            //pick each arg from the params array and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

            //compile it
            ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }  
    }

    //http://ayende.com/blog/3167/creating-objects-perf-implications
    //public partial class InstanceCreatorIL 
    //{
    //    static ConstructorInfo ctor = typeof(Created).GetConstructors()[0];
    //    delegate Created CreateCtor(int i, string s);
    //    static CreateCtor createdCtorDelegate;
    //    DynamicMethod method = new DynamicMethod("CreateIntance", typeof(Created),
    //        new Type[] { typeof(object[]) });
    //    ILGenerator gen = method.GetILGenerator();
    //    gen.Emit(OpCodes.Ldarg_0);//arr
    //    gen.Emit(OpCodes.Ldc_I4_0);
    //    gen.Emit(OpCodes.Ldelem_Ref);
    //    gen.Emit(OpCodes.Unbox_Any, typeof(int));
    //    gen.Emit(OpCodes.Ldarg_0);//arr
    //    gen.Emit(OpCodes.Ldc_I4_1);
    //    gen.Emit(OpCodes.Ldelem_Ref);
    //    gen.Emit(OpCodes.Castclass, typeof(string));
    //    gen.Emit(OpCodes.Newobj, ctor);// new Created
    //    gen.Emit(OpCodes.Ret);
    //    createdCtorDelegate = (CreateCtor)method.CreateDelegate(typeof(CreateCtor));
    //}
}
