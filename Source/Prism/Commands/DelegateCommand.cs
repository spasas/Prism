using System;
using System.Linq.Expressions;
using Prism.Properties;
using Prism.Events;

namespace Prism.Commands
{
   /// <summary>
   /// An <see cref="ICommand"/> whose delegates do not take any parameters for <see cref="Execute"/> and <see cref="CanExecute"/>.
   /// </summary>
   /// <see cref="DelegateCommandBase"/>
   /// <see cref="DelegateCommand{T}"/>
   public sealed class DelegateCommand : DelegateCommandBase
   {
      public DelegateCommand(Action executeMethod)
       : this(executeMethod, ThreadOption.PublisherThread)
      {
      }

      public DelegateCommand(Action executeMethod, ThreadOption threadOption)
         : this(executeMethod, () => true, threadOption)
      {
      }

      public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
         : this(executeMethod, canExecuteMethod, ThreadOption.PublisherThread)
      {
      }

      public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, ThreadOption threadOption)
         : base((object o) => executeMethod(), (object o) => canExecuteMethod(), threadOption)
      {
         if (executeMethod == null || canExecuteMethod == null)
            throw new ArgumentNullException(nameof(executeMethod), Resources.DelegateCommandDelegatesCannotBeNull);
      }

      ///<summary>
      /// Executes the command.
      ///</summary>
      public void Execute()
      {
         base.Execute(null);
      }

      /// <summary>
      /// Determines if the command can be executed.
      /// </summary>
      /// <returns>Returns <see langword="true"/> if the command can execute,otherwise returns <see langword="false"/>.</returns>
      public bool CanExecute()
      {
         return base.CanExecute(null);
      }

      /// <summary>
      /// Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
      /// </summary>
      /// <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
      /// <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
      /// <returns>The current instance of DelegateCommand</returns>
      public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
      {
         ObservesPropertyInternal(propertyExpression);
         return this;
      }

      /// <summary>
      /// Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
      /// </summary>
      /// <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute((o) => PropertyName).</param>
      /// <returns>The current instance of DelegateCommand</returns>
      public DelegateCommand ObservesCanExecute(Expression<Func<object, bool>> canExecuteExpression)
      {
         ObservesCanExecuteInternal(canExecuteExpression);
         return this;
      }
   }
}
