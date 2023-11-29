using System.IO;
using Diffstore.Entities.Filesystem;
using SharpFileSystem;
using Xunit;

namespace Diffstore.Tests.Entities.Filesystem
{
    public class FilesystemLocatorTest
    {
        private const string EntityFilename = FilesystemLocator.EntityFilename;
        private const string KeyFilename = FilesystemLocator.KeyFilename;

        [Theory]
        [InlineData("")]
        [InlineData("TestFolder")]
        public void TestNumericKeyWithoutPartitioning(string directory)
        {
            int key = 123;
            var basePath = FileSystemPath.Root.AppendDirectory(directory);
            var options = new FilesystemStorageOptions()
            {
                BasePath = basePath,
                EntitiesPerDirectory = 0
            };
            var expected = basePath.AppendDirectory(key.ToString()).AppendFile(EntityFilename);
            var actual = FilesystemLocator.LocateEntityFile(key, options);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TestFolder")]
        public void TestNumericKeyWithPartitioning(string directory)
        {
            int key1 = 123;
            int key2 = 1123;
            var basePath = FileSystemPath.Root.AppendDirectory(directory);
            var options = new FilesystemStorageOptions()
            {
                BasePath = basePath,
                EntitiesPerDirectory = 1000
            };

            var expected1 = basePath.AppendDirectory("0")
                                    .AppendDirectory(key1.ToString())
                                    .AppendFile(EntityFilename);

            var actual1 = FilesystemLocator.LocateEntityFile(key1, options);
            Assert.Equal(expected1, actual1);

            var expected2 = basePath.AppendDirectory("1")
                                    .AppendDirectory(key2.ToString())
                                    .AppendFile(EntityFilename);

            var actual2 = FilesystemLocator.LocateEntityFile(key2, options);
            Assert.Equal(expected2, actual2);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("", 10)]
        [InlineData("TestFolder", 0)]
        [InlineData("TestFolder", 10)]
        public void TestNonPartitionableKey(string directory, int entitiesPerDir)
        {
            var key = "Hello World";
            var basePath = FileSystemPath.Root.AppendDirectory(directory);
            var options = new FilesystemStorageOptions()
            {
                BasePath = basePath,
                EntitiesPerDirectory = entitiesPerDir
            };

            var expected = basePath.AppendDirectory(key.ToString())
                                    .AppendFile(EntityFilename);

            var actual = FilesystemLocator.LocateEntityFile(key, options);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("TestFolder")]
        public void TestKeyfileLocation(string directory)
        {
            int key = 123;
            var basePath = FileSystemPath.Root.AppendDirectory(directory);
            var options = new FilesystemStorageOptions()
            {
                BasePath = basePath,
                EntitiesPerDirectory = 0
            };
            var expected = basePath.AppendDirectory(key.ToString()).AppendFile(KeyFilename);
            var actual = FilesystemLocator.LocateKeyFile(key, options);
            Assert.Equal(expected, actual);
        }
    }
}