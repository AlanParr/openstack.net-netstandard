using System.Linq;
using net.openstack.Core.Domain;
using net.openstack.Providers.Rackspace.Exceptions;
using net.openstack.Providers.Rackspace.Objects;
using Xunit;
using BinaryFormatter = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;
using HttpStatusCode = System.Net.HttpStatusCode;
using MemoryStream = System.IO.MemoryStream;
using Stream = System.IO.Stream;

namespace OpenStackNet.Testing.Unit.Providers.Rackspace
{
    /// <summary>
    /// These tests are used to ensure that certain objects can be serialized and deserialized as expected.
    /// Many of these cases are due to objects providing additional information for custom exceptions,
    /// with exceptions being serializable.
    /// </summary>
    public class SerializationTests
    {
        /// <summary>
        /// This tests verifies the serialization behavior of <see cref="BulkDeletionResults"/>,
        /// which is included as a member of the exception class <see cref="BulkDeletionException"/>.
        /// </summary>
        [Fact]
        [Trait("Category", TestCategories.Unit)]
        public void TestBulkDeletionResultsSerializable()
        {
            var successfulObjects = new[] { "/container/object1", "/container/object2" };
            var failedObjects =
                new[]
                {
                    new BulkDeletionFailedObject("/badContainer/object3", new Status((int)HttpStatusCode.BadRequest, "invalidContainer")),
                    new BulkDeletionFailedObject("/container/badObject", new Status((int)HttpStatusCode.BadRequest, "invalidName"))
                };
            BulkDeletionResults results = new BulkDeletionResults(successfulObjects, failedObjects);
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, results);
                stream.Position = 0;
                BulkDeletionResults deserialized = (BulkDeletionResults)formatter.Deserialize(stream);
                Assert.NotNull(deserialized);

                Assert.NotNull(deserialized.SuccessfulObjects);
                Assert.Equal(successfulObjects.Length, deserialized.SuccessfulObjects.Count());
                for (int i = 0; i < successfulObjects.Length; i++)
                    Assert.Equal(successfulObjects[i], deserialized.SuccessfulObjects.ElementAt(i));

                Assert.NotNull(deserialized.FailedObjects);
                Assert.Equal(failedObjects.Length, deserialized.FailedObjects.Count());
                for (int i = 0; i < failedObjects.Length; i++)
                {
                    Assert.NotNull(deserialized.FailedObjects.ElementAt(i));
                    Assert.Equal(failedObjects[i].Object, deserialized.FailedObjects.ElementAt(i).Object);
                    Assert.NotNull(deserialized.FailedObjects.ElementAt(i).Status);
                    Assert.Equal(failedObjects[i].Status.Code, deserialized.FailedObjects.ElementAt(i).Status.Code);
                    Assert.Equal(failedObjects[i].Status.Description, deserialized.FailedObjects.ElementAt(i).Status.Description);
                }
            }
        }
    }
}
