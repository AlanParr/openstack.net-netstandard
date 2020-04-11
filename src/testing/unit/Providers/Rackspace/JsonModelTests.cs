using System;
using System.Net;
using net.openstack.Core.Domain;
using net.openstack.Core.Domain.Converters;
using net.openstack.Providers.Rackspace;
using net.openstack.Providers.Rackspace.Objects.Request;
using net.openstack.Providers.Rackspace.Objects.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Encoding = System.Text.Encoding;

namespace OpenStackNet.Testing.Unit.Providers.Rackspace
{
    public class JsonModelTests
    {
        /// <seealso cref="PasswordCredential"/>
        /// <seealso href="http://docs.openstack.org/api/openstack-identity-service/2.0/content/POST_updateUserCredential_v2.0_users__userId__OS-KSADM_credentials__credential-type__.html">Update User Credentials (OpenStack Identity Service API v2.0 Reference)</seealso>
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestPasswordCredential()
        {
            string json = @"{ ""username"" : ""test_user"", ""password"" : ""mypass"" }";
            PasswordCredential credentials = JsonConvert.DeserializeObject<PasswordCredential>(json);
            Assert.NotNull(credentials);
            Assert.Equal("test_user", credentials.Username);
            Assert.Equal("mypass", credentials.Password);
        }

        /// <seealso cref="PasswordCredentialResponse"/>
        /// <seealso href="http://docs.openstack.org/api/openstack-identity-service/2.0/content/POST_updateUserCredential_v2.0_users__userId__OS-KSADM_credentials__credential-type__.html">Update User Credentials (OpenStack Identity Service API v2.0 Reference)</seealso>
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestPasswordCredentialResponse()
        {
            string json = @"{ ""passwordCredentials"" : { username : ""test_user"", password : ""mypass"" } }";
            PasswordCredentialResponse response = JsonConvert.DeserializeObject<PasswordCredentialResponse>(json);
            Assert.NotNull(response);
            Assert.NotNull(response.PasswordCredential);
            Assert.Equal("test_user", response.PasswordCredential.Username);
            Assert.Equal("mypass", response.PasswordCredential.Password);
        }

        /// <seealso href="http://docs.openstack.org/api/openstack-compute/2/content/ServerUpdate.html">Update Server (OpenStack Compute API v2 and Extensions Reference)</seealso>
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestUpdateServerRequest()
        {
            UpdateServerRequest request = new UpdateServerRequest("new-name", IPAddress.Parse("10.0.0.1"), IPAddress.Parse("2607:f0d0:1002:51::4"));
            string expectedJson = @"{""server"":{""name"":""new-name"",""accessIPv4"":""10.0.0.1"",""accessIPv6"":""2607:f0d0:1002:51::4""}}";
            string actual = JsonConvert.SerializeObject(request, Formatting.None);
            Assert.Equal(expectedJson, actual);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestIPAddressDetailsConverter()
        {
            IPAddressDetailsConverter converter = new IPAddressDetailsConverter();

            string json = @"{ ""version"" : 4, ""addr"" : ""10.0.0.1"" }";
            IPAddress address = JsonConvert.DeserializeObject<IPAddress>(json, converter);
            Assert.Equal(IPAddress.Parse("10.0.0.1"), address);

            json = @"{ ""version"" : 6, ""addr"" : ""::babe:4317:0A83"" }";
            address = JsonConvert.DeserializeObject<IPAddress>(json, converter);
            Assert.Equal(IPAddress.Parse("::babe:4317:0A83"), address);

            json = JsonConvert.SerializeObject(IPAddress.Parse("10.0.0.1"), converter);
            Assert.Equal(@"{""addr"":""10.0.0.1"",""version"":""4""}", json);

            json = JsonConvert.SerializeObject(IPAddress.Parse("::babe:4317:0A83"), converter);
            Assert.Equal(@"{""addr"":""::babe:4317:a83"",""version"":""6""}", json);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestIPAddressSimpleConverter()
        {
            IPAddressSimpleConverter converter = new IPAddressSimpleConverter();

            string json = @"""10.0.0.1""";
            IPAddress address = JsonConvert.DeserializeObject<IPAddress>(json, converter);
            Assert.Equal(IPAddress.Parse("10.0.0.1"), address);

            json = @"""::babe:4317:0A83""";
            address = JsonConvert.DeserializeObject<IPAddress>(json, converter);
            Assert.Equal(IPAddress.Parse("::babe:4317:0A83"), address);

            json = JsonConvert.SerializeObject(IPAddress.Parse("10.0.0.1"), converter);
            Assert.Equal(@"""10.0.0.1""", json);

            json = JsonConvert.SerializeObject(IPAddress.Parse("::babe:4317:0A83"), converter);
            Assert.Equal(@"""::babe:4317:a83""", json);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestPersonalityJsonModel()
        {
            string expectedPath = "/usr/lib/stuff";
            string expectedText = "Example text";
            Personality personality = new Personality(expectedPath, expectedText, Encoding.UTF8);
            Assert.Equal(expectedPath, personality.Path);
            Assert.Equal(expectedText, Encoding.UTF8.GetString(personality.Content));

            string json = JsonConvert.SerializeObject(personality);
            Personality personality2 = JsonConvert.DeserializeObject<Personality>(json);
            Assert.Equal(expectedPath, personality.Path);
            Assert.Equal(expectedText, Encoding.UTF8.GetString(personality.Content));

            // make sure the JSON was Base-64 encoded
            JObject personalityObject = JsonConvert.DeserializeObject<JObject>(json);
            Assert.IsType<JValue>(personalityObject["contents"]);
            byte[] encodedText = Convert.FromBase64String((string)((JValue)personalityObject["contents"]).Value);
            Assert.Equal(expectedText, Encoding.UTF8.GetString(encodedText));
            Assert.Equal(personality.Content.Length, encodedText.Length);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestImpersonationRequest()
        {
            CloudIdentityProviderWrapper provider = new CloudIdentityProviderWrapper();
            JObject requestBody = provider.BuildImpersonationRequestJsonAccessor("myUser", 27);
            Assert.Equal(@"{""RAX-AUTH:impersonation"":{""user"":{""username"":""myUser"",""expire-in-seconds"":27}}}", requestBody.ToString(Formatting.None));
        }

        protected class CloudIdentityProviderWrapper : CloudIdentityProvider
        {
            public JObject BuildImpersonationRequestJsonAccessor(string userName, int expirationInSeconds)
            {
                return BuildImpersonationRequestJson(userName, expirationInSeconds);
            }
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestDiskConfigurationConversions()
        {
            TestExtensibleEnumSerialization(DiskConfiguration.Auto, "OTHER", DiskConfiguration.FromName);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestImageState()
        {
            TestExtensibleEnumSerialization(ImageState.Active, "OTHER", ImageState.FromName);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestImageType()
        {
            TestExtensibleEnumSerialization(ImageType.Base, "OTHER", ImageType.FromName);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestRebootType()
        {
            TestExtensibleEnumSerialization(RebootType.Hard, "OTHER", RebootType.FromName);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestServerState()
        {
            TestExtensibleEnumSerialization(ServerState.Build, "OTHER", ServerState.FromName);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestSnapshotState()
        {
            TestExtensibleEnumSerialization(SnapshotState.Available, "OTHER", SnapshotState.FromName);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestVolumeState()
        {
            TestExtensibleEnumSerialization(VolumeState.Creating, "OTHER", VolumeState.FromName);
        }

        private void TestExtensibleEnumSerialization<T>(T standardItem, string nonStandardName, Func<string, T> fromName)
        {
            if (fromName == null)
                throw new ArgumentNullException("fromName");

            T obj = JsonConvert.DeserializeObject<T>("null");
            Assert.Null(obj);

            obj = JsonConvert.DeserializeObject<T>(@"""""");
            Assert.Null(obj);

            // matching case, predefined value
            obj = JsonConvert.DeserializeObject<T>('"' + standardItem.ToString() + '"');
            Assert.Equal(standardItem, obj);

            // different case, predefined value
            string caseDifference = standardItem.ToString().ToLowerInvariant();
            if (standardItem.ToString() == caseDifference)
            {
                caseDifference = standardItem.ToString().ToUpperInvariant();
                Assert.NotEqual(standardItem.ToString(), caseDifference);
            }

            obj = JsonConvert.DeserializeObject<T>('"' + caseDifference + '"');
            Assert.Equal(standardItem, obj);

            // new value
            obj = JsonConvert.DeserializeObject<T>('"' + nonStandardName + '"');
            Assert.Equal(fromName(nonStandardName), obj);

            // different case, same as value encountered before
            caseDifference = nonStandardName.ToLowerInvariant();
            if (nonStandardName == caseDifference)
            {
                caseDifference = nonStandardName.ToUpperInvariant();
                Assert.NotEqual(nonStandardName, caseDifference);
            }

            Assert.NotEqual(nonStandardName, caseDifference);
            obj = JsonConvert.DeserializeObject<T>('"' + nonStandardName.ToLowerInvariant() + '"');
            Assert.Equal(fromName(nonStandardName), obj);

            string json = JsonConvert.SerializeObject(standardItem);
            Assert.Equal('"' + standardItem.ToString() + '"', json);
        }
    }
}
