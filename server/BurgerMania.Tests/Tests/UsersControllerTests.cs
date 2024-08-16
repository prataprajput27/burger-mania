using BurgerAssignmentFinal.Controllers;
using BurgerAssignmentFinal.Models;
//using BurgerAssignmentFinal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BurgerMania.Tests.Tests
{
    public class UsersControllerTests
    {
        private readonly BurgerManiaDBContext _context;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<BurgerManiaDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _context = new BurgerManiaDBContext(options);
            _controller = new UsersController(_context);
        }

        [Fact]
        public async Task GetUsers_ReturnsAllUsers()
        {
            // Arrange
            var user = new User { Username = "TestUser", Email = "test_pratap@example.com", Phone = 1234567890, Password = "Password123" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var users = Assert.IsType<List<User>>(result.Value);
            //Assert.Single(users);
            Assert.Equal("TestUser", users[0].Username);
        }

        [Fact]
        public async Task GetUser_ExistingUserId_ReturnsUser()
        {
            // Arrange
            var user = new User { Username = "TestUser", Email = "test@example.com", Phone = 1234567890, Password = "Password123", Id = new Guid() };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetUser(user.Id);

            // Assert
            var retrievedUser = Assert.IsType<User>(result.Value);
            Assert.Equal("TestUser", retrievedUser.Username);
        }

        [Fact]
        public async Task GetUser_NonExistingUserId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetUser(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostUser_ValidUser_ReturnsCreated()
        {
            // Arrange
            var user = new User { Username = "NewUser", Email = "newuser@example.com", Phone = 9876543210, Password = "Password123" };

            // Act
            var result = await _controller.PostUser(user);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdUser = Assert.IsType<User>(createdResult.Value);
            Assert.Equal("NewUser", createdUser.Username);
        }

        [Fact]
        public async Task DeleteUser_ExistingUser_ReturnsNoContent()
        {
            // Arrange
            var user = new User { Username = "ToDelete", Email = "delete@example.com", Phone = 1234567890, Password = "Password123" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteUser(user.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_NonExistingUser_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteUser(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}