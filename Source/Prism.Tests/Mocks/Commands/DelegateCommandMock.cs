using Prism.Commands;
using Prism.Events;
using System;

namespace Prism.Tests.Mocks.Commands
{
   public class DelegateCommandMock : DelegateCommandBase
   {
      public DelegateCommandMock(Action<object> executeMethod)
       : base(executeMethod, (o) => true, ThreadOption.PublisherThread)
      {
      }

      public DelegateCommandMock(Action<object> executeMethod, ThreadOption threadOption)
         : base(executeMethod, (o) => true, threadOption)
      {
      }

      public DelegateCommandMock(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
         : base(executeMethod, canExecuteMethod, ThreadOption.PublisherThread)
      {
      }

      public DelegateCommandMock(Action<object> executeMethod, Func<object, bool> canExecuteMethod, ThreadOption threadOption)
         : base(executeMethod, canExecuteMethod, threadOption)
      {
      }
   }
}
