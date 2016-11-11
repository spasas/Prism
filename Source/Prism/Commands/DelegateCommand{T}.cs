﻿using Prism.Events;
using Prism.Properties;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Prism.Commands
{
   /// <summary>
   /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
   /// </summary>
   /// <typeparam name="T">Parameter type.</typeparam>
   /// <remarks>
   /// The constructor deliberately prevents the use of value types.
   /// Because ICommand takes an object, having a value type for T would cause unexpected behavior when CanExecute(null) is called during XAML initialization for command bindings.
   /// Using default(T) was considered and rejected as a solution because the implementor would not be able to distinguish between a valid and defaulted values.
   /// <para/>
   /// Instead, callers should support a value type by using a nullable value type and checking the HasValue property before using the Value property.
   /// <example>
   ///     <code>
   /// public MyClass()
   /// {
   ///     this.submitCommand = new DelegateCommand&lt;int?&gt;(this.Submit, this.CanSubmit);
   /// }
   ///
   /// private bool CanSubmit(int? customerId)
   /// {
   ///     return (customerId.HasValue &amp;&amp; customers.Contains(customerId.Value));
   /// }
   ///     </code>
   /// </example>
   /// </remarks>
   public sealed class DelegateCommand<T> : DelegateCommandBase
   {
      public DelegateCommand(Action<T> executeMethod)
         : this(executeMethod, ThreadOption.PublisherThread)
      {
      }

      public DelegateCommand(Action<T> executeMethod, ThreadOption threadOption)
         : this(executeMethod, (o) => true, threadOption)
      {
      }

      public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
         : this(executeMethod, canExecuteMethod, ThreadOption.PublisherThread)
      {
      }

      public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, ThreadOption threadOption)
         : base((o) => executeMethod((T) o), (o) => canExecuteMethod((T) o), threadOption)
      {
         if (executeMethod == null || canExecuteMethod == null)
            throw new ArgumentNullException(nameof(executeMethod), Resources.DelegateCommandDelegatesCannotBeNull);

         TypeInfo genericTypeInfo = typeof(T).GetTypeInfo();

         // DelegateCommand allows object or Nullable<>.
         // note: Nullable<> is a struct so we cannot use a class constraint.
         if (genericTypeInfo.IsValueType)
         {
            if ((!genericTypeInfo.IsGenericType) || (!typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition().GetTypeInfo())))
            {
               throw new InvalidCastException(Resources.DelegateCommandInvalidGenericPayloadType);
            }
         }
      }

      ///<summary>
      ///Executes the command and invokes the <see cref="Action{T}"/> provided during construction.
      ///</summary>
      ///<param name="parameter">Data used by the command.</param>
      public void Execute(T parameter)
      {
         base.Execute(parameter);
      }

      ///<summary>
      ///Determines if the command can execute by invoked the <see cref="Func{T,Bool}"/> provided during construction.
      ///</summary>
      ///<param name="parameter">Data used by the command to determine if it can execute.</param>
      ///<returns>
      ///<see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
      ///</returns>
      public bool CanExecute(T parameter)
      {
         return base.CanExecute(parameter);
      }

      /// <summary>
      /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
      /// </summary>
      /// <typeparam name="TP">The object type containing the property specified in the expression.</typeparam>
      /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
      /// <returns>The current instance of DelegateCommand</returns>
      public DelegateCommand<T> ObservesProperty<TP>(Expression<Func<TP>> propertyExpression)
      {
         ObservesPropertyInternal(propertyExpression);
         return this;
      }

      /// <summary>
      /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
      /// </summary>
      /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute((o) => PropertyName).</param>
      /// <returns>The current instance of DelegateCommand</returns>
      public DelegateCommand<T> ObservesCanExecute(Expression<Func<object, bool>> canExecuteExpression)
      {
         ObservesCanExecuteInternal(canExecuteExpression);
         return this;
      }
   }
}
