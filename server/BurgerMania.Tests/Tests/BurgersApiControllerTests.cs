using BurgerAssignmentFinal.Controllers;
using BurgerAssignmentFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BurgerMania.Tests.Tests
{
    public class BurgersApiControllerTests
    {
        private readonly BurgerManiaDBContext _context;
        private readonly BurgersApiController _controller;

        public BurgersApiControllerTests()
        {
            // set up in-memory database
            var options = new DbContextOptionsBuilder<BurgerManiaDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _context = new BurgerManiaDBContext(options);
            _controller = new BurgersApiController(_context);
        }

        [Fact]
        public async Task GetBurgers_ReturnsAllBurgers()
        {
            // Arrange
            var burger = new Burger { B_Name = "Veg Whopper", B_Image = "image_url", Price = 150.00f };
            _context.Burgers.Add(burger);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBurgers();

            // Assert
            var burgers = Assert.IsType<List<Burger>>(result.Value);
            Assert.Single(burgers);
            Assert.Equal("Veg Whopper", burgers[0].B_Name);
        }

        [Fact]
        public async Task GetBurgers_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetBurgers(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostBurgers_ValidBurger_ReturnsCreated()
        {
            // Arrange
            var burger = new Burger { B_Name = "Cheese Burger", B_Image = "image_url", Price = 200.00f };

            // Act
            var result = await _controller.PostBurgers(burger);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdBurger = Assert.IsType<Burger>(createdResult.Value);
            Assert.Equal("Cheese Burger", createdBurger.B_Name);
        }

        [Fact]
        public async Task DeleteBurgers_ExistingBurger_ReturnsNoContent()
        {
            // Arrange
            var burger = new Burger { B_Name = "Chicken Burger", B_Image = "image_url", Price = 250.00f };
            _context.Burgers.Add(burger);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteBurgers(burger.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBurgers_NonExistingBurger_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteBurgers(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}