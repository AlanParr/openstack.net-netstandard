using System;
using Moq;
using net.openstack.Core;
using net.openstack.Core.Validators;
using net.openstack.Providers.Rackspace.Validators;
using Xunit;

namespace OpenStackNet.Testing.Unit.Providers.Rackspace
{
    public class CloudNetworksValidatorTests
    {
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Empty_Cidr()
        {
            const string cidr = "";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal("cidr cannot be empty", ex.Message);
        }


        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Null_Cidr()
        {
            const string cidr = null;
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal("Value cannot be null." + Environment.NewLine + "Parameter name: cidr", ex.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Cidr_Missing_Slash()
        {
            const string cidr = "10.0.0.0";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal(string.Format("ERROR: CIDR {0} is missing /", cidr), ex.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Cidr_Has_Two_Ranges()
        {
            const string cidr = "10.0.0.0/24/24";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal(string.Format("ERROR: CIDR {0} must have exactly one / character", cidr), ex.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Cidr_Has_Invalid_IP_Segment()
        {
            const string cidr = "10.0.0.256/24";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal(string.Format("ERROR: IP address segment ({0}) of CIDR is not a valid IP address", "10.0.0.256"), ex.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Cidr_Has_Non_Integer_Range()
        {
            const string cidr = "10.0.0.0/abc";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal(string.Format("ERROR: CIDR range segment {0} must be an integer", "abc"), ex.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Cidr_Has_Invalid_Range()
        {
            const string cidr = "10.0.0.0/33";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal(string.Format("ERROR: CIDR range segment {0} must be between 1 and 32", "33"), ex.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Fail_When_Cidr_Has_Explicit_Range_Specified()
        {
            const string cidr = "10.0.0.0 - 10.0.0.255";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var ex = Record.Exception(() =>
            {
                var cloudNetworksValidator = CloudNetworksValidator.Default;
                cloudNetworksValidator.ValidateCidr(cidr);
            });

            Assert.Equal(string.Format("ERROR: CIDR {0} is missing /", cidr), ex.Message);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Pass_When_Cidr_Has_IPV4_IP_Segment()
        {
            const string cidr = "192.0.2.0/24";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var cloudNetworksValidator = CloudNetworksValidator.Default;
            cloudNetworksValidator.ValidateCidr(cidr);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Pass_When_Cidr_Has_IPV6_IP_Segment()
        {
            const string cidr = "2001:db8::/32";
            var validatorMock = new Mock<INetworksValidator>();
            validatorMock.Setup(v => v.ValidateCidr(cidr));

            var cloudNetworksValidator = CloudNetworksValidator.Default;
            cloudNetworksValidator.ValidateCidr(cidr);
        }
    }
}
