using System.IO;
using Diffstore.IO.Filesystem;
using Xunit;

namespace Diffstore.Tests.IO.Filesystem
{
    public class FilesystemLocatorTest
    {
        private const string EntityFilename = "entity";

        [Theory]
        [InlineData("")]
        [InlineData("TestFolder")]
        public void NumericKeyWithoutPartition(string basePath)
        {
            int key = 123;
            var options = new FilesystemEntityStorageOptions()
            {
                BasePath = basePath,
                EntitiesPerDirectory = 0
            };
            var expected = Path.Combine(basePath, key.ToString(), EntityFilename);
            var actual = FilesystemLocator.LocateEntityFile(key, options);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TestFolder")]
        public void NumericKeyWithPartition(string basePath)
        {
            int key1 = 123;
            int key2 = 1123;
            var options = new FilesystemEntityStorageOptions()
            {
                BasePath = basePath,
                EntitiesPerDirectory = 1000
            };

            var expected1 = Path.Combine(basePath, "0", key1.ToString(), EntityFilename);
            var actual1 = FilesystemLocator.LocateEntityFile(key1, options);
            Assert.Equal(expected1, actual1);

            var expected2 = Path.Combine(basePath, "1", key2.ToString(), EntityFilename);
            var actual2 = FilesystemLocator.LocateEntityFile(key2, options);
            Assert.Equal(expected2, actual2);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("", 10)]
        [InlineData("TestFolder", 0)]
        [InlineData("TestFolder", 10)]
        public void StringKey(string basePath, int entitiesPerDir)
        {
            var key = "Hello World";
            var options = new FilesystemEntityStorageOptions()
            {
                BasePath = basePath,
                EntitiesPerDirectory = entitiesPerDir
            };
            var expected = Path.Combine(basePath, key.GetHashCode().ToString(), EntityFilename);
            var actual = FilesystemLocator.LocateEntityFile(key, options);
            Assert.Equal(expected, actual);
        }
    }
}