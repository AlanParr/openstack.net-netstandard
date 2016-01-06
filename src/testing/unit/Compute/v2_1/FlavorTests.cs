﻿using System;
using System.Linq;
using OpenStack.Compute.v2_1.Serialization;
using OpenStack.Serialization;
using OpenStack.Synchronous;
using OpenStack.Testing;
using Xunit;

namespace OpenStack.Compute.v2_1
{
    public class FlavorTests
    {
        private readonly ComputeService _compute;

        public FlavorTests()
        {
            _compute = new ComputeService(Stubs.AuthenticationProvider, "region");
        }
        
        [Fact]
        public void GetFlavor()
        {
            using (var httpTest = new HttpTest())
            {
                const string flavorId = "1";
                httpTest.RespondWithJson(new Flavor { Id = flavorId });

                var result = _compute.GetFlavor(flavorId);

                httpTest.ShouldHaveCalled($"*/flavors/{flavorId}");
                Assert.NotNull(result);
                Assert.Equal(flavorId, result.Id);
                Assert.IsType<ComputeApiBuilder>(((IServiceResource)result).Owner);
            }
        }

        [Fact]
        public void GetFlavorExtension()
        {
            using (var httpTest = new HttpTest())
            {
                Identifier flavorId = Guid.NewGuid();
                httpTest.RespondWithJson(new FlavorReferenceCollection
                {
                    new FlavorReference {Id = flavorId}
                });
                httpTest.RespondWithJson(new Flavor { Id = flavorId });

                var results = _compute.ListFlavors();
                var flavorRef = results.First();
                var result = flavorRef.GetFlavor();

                Assert.NotNull(result);
                Assert.Equal(flavorId, result.Id);
            }
        }

        [Fact]
        public void ListFlavors()
        {
            using (var httpTest = new HttpTest())
            {
                const string flavorId = "1";
                httpTest.RespondWithJson(new FlavorReferenceCollection
                {
                    Items = { new FlavorReference { Id = flavorId } }
                });

                var results = _compute.ListFlavors();

                httpTest.ShouldHaveCalled("*/flavors");
                Assert.Equal(1, results.Count());
                var result = results.First();
                Assert.Equal(flavorId, result.Id);
                Assert.IsType<ComputeApiBuilder>(((IServiceResource)result).Owner);
            }
        }

        [Fact]
        public void ListFlavorDetails()
        {
            using (var httpTest = new HttpTest())
            {
                const string flavorId = "1";
                httpTest.RespondWithJson(new FlavorCollection
                {
                    Items = { new Flavor { Id = flavorId } }
                });
                httpTest.RespondWithJson(new Flavor { Id = flavorId });

                var results = _compute.ListFlavorDetails();

                httpTest.ShouldHaveCalled("*/flavors");
                Assert.Equal(1, results.Count());
                var result = results.First();
                Assert.Equal(flavorId, result.Id);
                Assert.IsType<ComputeApiBuilder>(((IServiceResource)result).Owner);
            }
        }
    }
}
