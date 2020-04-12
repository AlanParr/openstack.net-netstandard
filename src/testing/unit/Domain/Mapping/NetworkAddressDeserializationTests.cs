using System.Linq;
using System.Net;
using System.Net.Sockets;
using net.openstack.Core.Domain;
using Newtonsoft.Json;
using Xunit;

namespace OpenStackNet.Testing.Unit.Domain.Mapping
{
    public class NetworkAddressDeserializationTests
    {
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Return_Null_When_Null_String_Is_Mapped()
        {
            string obj = "null";
            var actual = JsonConvert.DeserializeObject<ServerAddresses>(obj);

            Assert.Null(actual);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Return_Null_When_Empty_String_Is_Mapped()
        {
            string obj = "";
            var actual = JsonConvert.DeserializeObject<ServerAddresses>(obj);

            Assert.Null(actual);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Return_Null_When_Whitespace_String_Is_Mapped()
        {
            string obj = "   ";
            var actual = JsonConvert.DeserializeObject<ServerAddresses>(obj);

            Assert.Null(actual);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Return_Network_With_No_Addresses()
        {
            string obj = "{\"public\": []}";
            var actual = JsonConvert.DeserializeObject<ServerAddresses>(obj).Single();

            Assert.Empty(actual.Value);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Return_Network_With_Single_Addresses()
        {
            string obj = "{\"public\": [{\"version\": 4, \"addr\": \"166.78.156.150\"}]}";
            var actual = JsonConvert.DeserializeObject<ServerAddresses>(obj).Single();

            Assert.Single(actual.Value);
            Assert.True(actual.Value.All(x => x != null));
            Assert.Equal(AddressFamily.InterNetwork, actual.Value[0].AddressFamily);
            Assert.Equal(IPAddress.Parse("166.78.156.150"), actual.Value[0]);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Return_Network_With_2_Addresseses()
        {
            string obj = "{\"public\": [{\"version\": 4, \"addr\": \"166.78.156.150\"}, {\"version\": 6, \"addr\": \"2001:4800:7812:0514:95e4:7f4d:ff04:d1eb\"}]}";
            var actual = JsonConvert.DeserializeObject<ServerAddresses>(obj).Single();

            Assert.Equal(2, actual.Value.Count);
            Assert.True(actual.Value.All(x => x != null));
            Assert.Equal(AddressFamily.InterNetwork, actual.Value[0].AddressFamily);
            Assert.Equal(IPAddress.Parse("166.78.156.150"), actual.Value[0]);
            Assert.Equal(AddressFamily.InterNetworkV6, actual.Value[1].AddressFamily);
            Assert.Equal(IPAddress.Parse("2001:4800:7812:0514:95e4:7f4d:ff04:d1eb"), actual.Value[1]);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Return_Network_With_Both_v4_And_v6_Addresseses()
        {
            string obj = "{\"public\": [{\"version\": 4, \"addr\": \"166.78.156.150\"}, {\"version\": 6, \"addr\": \"2001:4800:7812:0514:95e4:7f4d:ff04:d1eb\"}]}";
            var actual = JsonConvert.DeserializeObject<ServerAddresses>(obj).Single();

            Assert.NotNull(actual.Value.SingleOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork));
            Assert.NotNull(actual.Value.SingleOrDefault(a => a.AddressFamily == AddressFamily.InterNetworkV6));
        }
    }
}
