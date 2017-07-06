using System;
using System.Collections.Generic;
using Diffstore.Serialization;
using Diffstore.Utils;
using Xunit;

namespace Diffstore.Tests.Utils
{
    public class ReflectionUtilsTest
    {
        [Fact]
        public void GetImplementingTypes()
        {
            var assembly = typeof(Serializer).Assembly;
            var allTypes = assembly.GetTypes();
            var types = new List<Type>(allTypes.GetImplementingTypes(typeof(ITypeSerializer<byte>)));
            

            Assert.NotEqual(0, types.Count);
            Assert.NotEqual(allTypes.Length, types.Count);
        }
    }
}