using BooksStore.Repositories.Interfaces;
using BooksStore.Services;
using BooksStoreEntities.Entities;
using Moq;

namespace BooksStoreTests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }
    
    private ApplicationUser GenerateMockUser()
    {
        var user = new ApplicationUser
            { UserName = "Illuminati", Email = "illuminati@gmail.com", Password = "324fdsa" };
        return user;
    }
    
    [Fact]
    public async Task GetUsersAsync_ReturnsListOfUsers()
    {
        var users = new List<ApplicationUser>();
        _userRepositoryMock.Setup(repo => repo.GetUsersAsync(It.IsAny<int>(), 
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        var result = await _userService.GetUsersAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count);
    }

    [Fact]
    public async Task FindAsync_ExistingUserId_ReturnsUser()
    {
        var userId = Guid.NewGuid();
        var expectedUser = GenerateMockUser();
        expectedUser.Id = userId;

        _userRepositoryMock.Setup(repo => repo.FindAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var result = await _userService.FindAsync(userId);

        Assert.NotNull(result);
        Assert.Equal(expectedUser, result);
    }

    [Fact]
    public async Task AddAsync_NewUser_ReturnsAddedUser()
    {
        var newUser = GenerateMockUser();
        
        _userRepositoryMock.Setup(repo => repo.AddAsync(newUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newUser); 

        var result = await _userService.AddAsync(newUser);

        Assert.NotNull(result);
        Assert.Equal(newUser, result); 
        _userRepositoryMock.Verify(repo => repo.AddAsync(newUser, 
            It.IsAny<CancellationToken>()), Times.Once); 
    }
    


    [Fact]
    public async Task UpdateAsync_ValidUser_UpdatesUser()
    {
        var userToUpdate = GenerateMockUser();
        _userRepositoryMock.Setup(repo => 
                repo.UpdateAsync(userToUpdate, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _userService.UpdateAsync(userToUpdate);

        _userRepositoryMock.Verify(repo => repo.UpdateAsync(userToUpdate, 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ValidUser_RemovesUser()
    {
        var userToRemove = GenerateMockUser();
        _userRepositoryMock.Setup(repo => 
                repo.RemoveAsync(userToRemove, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _userService.RemoveAsync(userToRemove);

        _userRepositoryMock.Verify(repo => repo.RemoveAsync(userToRemove, 
            It.IsAny<CancellationToken>()), Times.Once);
    }

}