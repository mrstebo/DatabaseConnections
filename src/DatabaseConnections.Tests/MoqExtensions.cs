using System.Collections;
using System.Data;
using Moq;

namespace DatabaseConnections.Tests
{
    internal static class MoqExtensions
    {
        public static Mock<IDbCommand> CreateIDbCommand(this MockRepository factory)
        {
            var command = factory.Create<IDbCommand>();

            command.SetupAllProperties();
            command.Setup(c => c.CreateParameter()).Returns(() => factory.CreateIDbDataParameter().Object);
            command.Setup(c => c.Parameters).Returns(factory.CreateIDataParameterCollection().Object);

            return command;
        }

        private static Mock<IDataParameterCollection> CreateIDataParameterCollection(this MockRepository factory)
        {
            var list = new ArrayList();
            var parameters = factory.Create<IDataParameterCollection>();

            parameters.Setup(p => p.Add(It.IsAny<IDataParameter>())).Returns((IDataParameter p) => list.Add(p));
            parameters.Setup(p => p[It.IsAny<int>()]).Returns((int i) => list[i]);
            parameters.Setup(p => p.Count).Returns(() => list.Count);
            parameters.Setup(p => p.GetEnumerator()).Returns(list.GetEnumerator);

            return parameters;
        }

        private static Mock<IDbDataParameter> CreateIDbDataParameter(this MockRepository factory)
        {
            var parameter = factory.Create<IDbDataParameter>();

            parameter.SetupAllProperties();

            return parameter;
        }
    }
}