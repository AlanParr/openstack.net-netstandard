using System.Threading;
using System.Threading.Tasks;
using Moq;
using OpenStack.Authentication;

namespace OpenStack
{
    /// <summary>
    /// Default stubs for unit testing
    /// </summary>
    public static class Stubs
    {
        public static readonly IAuthenticationProvider AuthenticationProvider;

        static Stubs()
        {
            var authProviderStub = CreateAuthenticationProvider();
            AuthenticationProvider = authProviderStub.Object;
        }

        public static Mock<IAuthenticationProvider> CreateAuthenticationProvider()
        {
            var stub = new Mock<IAuthenticationProvider>();

            stub.Setup(provider => provider.GetToken(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult("mock-token"));

            stub.Setup(provider => provider.GetEndpoint(It.IsAny<ServiceType>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult("http://api.com"));

            return stub;
        }
    }
}