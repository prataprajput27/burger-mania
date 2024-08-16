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
    public class CartsApiControllerTests
    {
        private readonly BurgerManiaDBContext _context;
        private readonly CartsApiController _controller;

        public CartsApiControllerTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<BurgerManiaDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _context = new BurgerManiaDBContext(options);
            _controller = new CartsApiController(_context);
        }

        [Fact]
        public async Task GetCart_ReturnsUserCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var burger = new Burger { B_Name = "Veg Whopper", B_Image = "image_url", Price = 150.00f };
            var cartItem = new CartItem { BurgerId = burger.Id, Quantity = 2 };
            var cart = new Cart { UserId = userId, Items = new List<CartItem> { cartItem } };

            _context.Burgers.Add(burger);
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetCart(userId);

            // Assert
            //var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var userCart = Assert.IsType<Cart>(result.Value);
            Assert.Equal(userId, userCart.UserId);
            Assert.Single(userCart.Items);
        }

        [Fact]
        public async Task GetCart_NonExistingUser_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetCart(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostCart_ValidCartItem_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var burger = new Burger { B_Name = "Cheese Burger", B_Image = "image_url", Price = 200.00f };
            var cartItem = new CartItem { BurgerId = burger.Id, Quantity = 1 };

            _context.Burgers.Add(burger);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.PostCart(userId, cartItem);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedCart = Assert.IsType<Cart>(okResult.Value);
            Assert.Equal(userId, updatedCart.UserId);
            Assert.Single(updatedCart.Items);
        }

        [Fact]
        public async Task DeleteCart_ExistingCartItem_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var burger = new Burger { B_Name = "Chicken Burger", B_Image = "image_url", Price = 250.00f };
            var cartItem = new CartItem { BurgerId = burger.Id, Quantity = 1 };
            var cart = new Cart { UserId = userId, Items = new List<CartItem> { cartItem } };

            _context.Burgers.Add(burger);
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteCart(userId, cartItem.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedCart = Assert.IsType<Cart>(okResult.Value);
            Assert.Empty(updatedCart.Items);
        }

        [Fact]
        public async Task DeleteCart_NonExistingCartItem_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _controller.DeleteCart(userId, Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}