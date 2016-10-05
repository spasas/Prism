using Prism.Commands;
using System;

namespace Prism.Tests.Mocks.Commands
{
    public class DelegateCommandMock : DelegateCommandBase
    {
        public DelegateCommandMock(Action<object> executeMethod) :
            base(executeMethod, (o) => true)
        {

        }

        public DelegateCommandMock(Action<object> executeMethod, Func<object, bool> canExecuteMethod) :
            base(executeMethod, canExecuteMethod)
        {

        }
    }
}
