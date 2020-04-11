using System;
using net.openstack.Providers.Rackspace.Exceptions;
using net.openstack.Providers.Rackspace.Validators;
using Xunit;

namespace OpenStackNet.Testing.Unit.Providers.Rackspace
{
    public class CloudBlockStorageTests
    {
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Not_Throw_Exception_When_Size_Is_In_Range()
        {
            const int size = 900;

            var ex = Record.Exception(() =>
            {
                var cloudBlockStorageValidator = CloudBlockStorageValidator.Default;
                cloudBlockStorageValidator.ValidateVolumeSize(size);
            });

            Assert.Null(ex);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Throw_Exception_When_Size_Is_Less_Than_1()
        {
            const int size = 0;

            var ex = Record.Exception(() =>
            {
                var cloudBlockStorageValidator = CloudBlockStorageValidator.Default;
                cloudBlockStorageValidator.ValidateVolumeSize(size);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void Should_Throw_Exception_When_Size_Is_Greater_Than_1000()
        {
            const int size = 1050;

            var ex = Record.Exception(() =>
            {
                var cloudBlockStorageValidator = CloudBlockStorageValidator.Default;
                cloudBlockStorageValidator.ValidateVolumeSize(size);
            });

            Assert.NotNull(ex);
        }
    }
}
