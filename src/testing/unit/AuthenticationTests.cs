using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using OpenStack.ContentDeliveryNetworks.v1;
using OpenStack.Testing;
using Xunit;

namespace OpenStack
{
    public class AuthenticationTests
    {
        [Fact]
        public async Task When401UnauthorizedIsReturned_RetryRequest()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("Your token has expired", (int)HttpStatusCode.Unauthorized);
                httpTest.RespondWithJson(new Flavor());

                var service = new ContentDeliveryNetworkService(Stubs.AuthenticationProvider, "DFW");
                var flavor = await service.GetFlavorAsync("flavor-id");
                Assert.NotNull(flavor);
            }
        }

        [Fact]
        public async Task When401AuthenticationFailsMultipleTimes_ThrowException()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("Your token has expired", (int)HttpStatusCode.Unauthorized);
                httpTest.RespondWith("Your token has expired", (int)HttpStatusCode.Unauthorized);

                var service = new ContentDeliveryNetworkService(Stubs.AuthenticationProvider, "DFW");
                await Assert.ThrowsAsync<FlurlHttpException>(() => service.GetFlavorAsync("flavor-id"));
            }
        }
    }
}
