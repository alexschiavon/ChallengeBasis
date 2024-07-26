using Arch.Domain.Adapters.Helper;

namespace Arch.Tests.Application
{
    public class MetadataTests
    {
        [Fact]
        public void TestMetadataContent()
        {
            var metadata = new Metadata<int, string>();
            metadata.Content.Add(1);
            Assert.Single(metadata.Content);
            Assert.Equal(1, metadata.Content[0]);
        }

        [Fact]
        public void TestMetadataPagination()
        {
            var metadata = new Metadata<int, string>();
            metadata.Pagination = new Pagination { CurrentPage = 1, TotalCount = 5 };
            Assert.Equal(1, metadata.Pagination.CurrentPage);
            Assert.Equal(5, metadata.Pagination.TotalCount);
        }

        [Fact]
        public void TestMetadataSortedBy()
        {
            var metadata = new Metadata<int, string>();
            metadata.SortedBy = new SortedBy { Order = "asc", Field = "Name" };
            Assert.Equal("asc", metadata.SortedBy.Order);
            Assert.Equal("Name", metadata.SortedBy.Field);
        }

        [Fact]
        public void TestMetadataWarning()
        {
            var metadata = new Metadata<int, string>();
            metadata.Warning = new List<string> { "Warning message" };
            Assert.Single(metadata.Warning);
            Assert.Equal("Warning message", metadata.Warning[0]);
        }

        [Fact]
        public void TestMetadataCustom()
        {
            var metadata = new Metadata<int, string>();
            metadata.Custom = "Custom data";
            Assert.Equal("Custom data", metadata.Custom);
        }
    }
}