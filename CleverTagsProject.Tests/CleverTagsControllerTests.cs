using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CleverTagProject.Controllers;
using CleverTagProject.DataContext;
using CleverTagProject.Models;
using CleverTagsProject.Tests;
using Microsoft.AspNetCore.Mvc;

namespace CleverTagProject.Tests
{
    public class CleverTagsControllerTests
    {


        [Fact]
        public async Task ReturnsCreatedAtAction()
        {
            var options = new DbContextOptionsBuilder<CleverTagContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new CleverTagContext(options))
            {
                var controller = new CleverTagsController(context);
                var newCleverTag = new CleverTag { Tag = "dk-1234567890-clever", Isblocked = false };

                var result = await controller.CreateCleverTag(newCleverTag);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var cleverTag = Assert.IsAssignableFrom<CleverTag>(createdAtActionResult.Value);
                Assert.Equal(newCleverTag.Tag, cleverTag.Tag);
            }
        }
    }
}
