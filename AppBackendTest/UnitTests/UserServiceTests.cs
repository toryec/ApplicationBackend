using Application.Core.Interfaces;
using Application.Services.Types;
using Domain.Core.Types;
using Domain.DALs.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AppBackendTest.UnitTests;

[TestFixture]
public class UserServiceTests
{
    private Mock<IPasswordHasher<User>> passwordHasherMock = default!;
    private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock = default!;
    private Mock<IUnitOfWork> unitOfWorkMock;
    private UserService userService;

    [SetUp]
    public void Setup()
    {
        unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
        passwordHasherMock = new Mock<IPasswordHasher<User>>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        userService = new UserService(unitOfWorkFactoryMock.Object, passwordHasherMock.Object);
    }

    [Test]
    public async Task CreateUserAsync_UserCreated_ReturnsTaskOfBoolean()
    {
        //Arrange
        var expectedInput = new User
        {
            UserName = "Dom",
            UserDetail = default!,
            Password = "12345"
        };

        var expectedResult = new PasswordHasher<User>().HashPassword(expectedInput, expectedInput.Password);
        var userDALMock = new Mock<IUserDAL>();

        userDALMock.Setup(d => d.CreateUserAsync(It.IsAny<User>(), CancellationToken.None)).ReturnsAsync(true);
        userDALMock.Setup(x => x.CheckUserNameExistsAsync(expectedInput.UserName, CancellationToken.None)).ReturnsAsync(false);
        userDALMock.Setup(u => u.UpdateUserDetailAsync(It.IsAny<UserDetail>(), CancellationToken.None)).ReturnsAsync(true); // Test UserDetails
        userDALMock.Setup(x => x.InsertRoleAsync(It.IsAny<IEnumerable<UserRole>>(), CancellationToken.None)).ReturnsAsync(true);
        unitOfWorkMock.Setup(u => u.GetDAL<IUserDAL>()).Returns(userDALMock.Object);

        //unitOfWorkMock.Setup(x => x.Dispose());
        unitOfWorkMock.Setup(x => x.CommitAsync(CancellationToken.None));
        unitOfWorkFactoryMock.Setup(f => f.GetUnitOfWork())
            .Returns(unitOfWorkMock.Object);

        passwordHasherMock.Setup(p => p.HashPassword(expectedInput, expectedInput.Password))
            .Returns(expectedResult);

        //Act
        var actualResult = await userService.CreateUserAsync(expectedInput);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task CheckIfExistsAsync_UserSearched_ReturnsTaskOfBoolean()
    {
        // Arrange

        var expectedInput = new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc");
        //Guid actualInput = default!;
        var userDALMock = new Mock<IUserDAL>();

        userDALMock.Setup(x => x.CheckIfExistsAsync(expectedInput, CancellationToken.None))
            .ReturnsAsync(true);

        //userDALMock.Setup(x => x.CheckIfExistsAsync(expectedInput, CancellationToken.None))
        //   .Callback<Guid,CancellationToken>((d,x) => actualInput = d)
        //   .ReturnsAsync(true);

        unitOfWorkMock.Setup(u => u.GetDAL<IUserDAL>()).Returns(userDALMock.Object);

        unitOfWorkFactoryMock.Setup(f => f.GetUnitOfWork())
          .Returns(unitOfWorkMock.Object);

        // Act
        var actualResult = await userService.CheckIfExistsAsync(new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc"), CancellationToken.None);

        // Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task DeleteUserAsync_UserDeleted_ReturnsTaskOfBoolean()
    {
        //Arrange

        var expectedInput = new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc");
        var userDALMock = new Mock<IUserDAL>();

        userDALMock.Setup(x => x.DeleteUserAsync(expectedInput, CancellationToken.None))
            .ReturnsAsync(true);

        unitOfWorkMock.Setup(u => u.GetDAL<IUserDAL>()).Returns(userDALMock.Object);

        unitOfWorkFactoryMock.Setup(f => f.GetUnitOfWork())
          .Returns(unitOfWorkMock.Object);

        //Act
        var actualResult = await userService.DeleteUserAsync(new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc"), CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task GetUserAsync_UserRetured_ReturnsTaskOfUser()
    {
        //Arrange
        var expectedInput = new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc");

        var expectedUserDetailsResult = new UserDetail
        {
            UserId = new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc")
        };

        var expectedRolesResult = new List<UserRole>
        {
            new UserRole{UserId = new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc")}
        };

        var expectedUserResult = new User
        {
            Id = new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc"),
            UserName = "Dom",
            UserDetail = default!,
            UserRoles = default!,
            Password = "12345"
        };

        var userDALMock = new Mock<IUserDAL>();

        userDALMock.Setup(u => u.GetUserAsync(expectedInput, CancellationToken.None)).ReturnsAsync(expectedUserResult);
        userDALMock.Setup(r => r.GetRoleAsync(expectedInput, CancellationToken.None)).ReturnsAsync(expectedRolesResult);
        userDALMock.Setup(d => d.GetUserDetailAsync(expectedInput, CancellationToken.None)).ReturnsAsync(expectedUserDetailsResult);

        unitOfWorkMock.Setup(u => u.GetDAL<IUserDAL>()).Returns(userDALMock.Object);

        unitOfWorkFactoryMock.Setup(f => f.GetUnitOfWork())
          .Returns(unitOfWorkMock.Object);

        //Act
        var actualResult = await userService.GetUserAsync(new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc"), true, true, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.EqualTo(expectedUserResult));
    }

    [Test]
    public async Task GetUsersAsync_UsersReturned_ReturnsTaskOfIEnumerableOfUser()
    {
        //Arrange
        var name = "JohnDoe";
        IEnumerable<User> expectedResult = new List<User>
        {
            new User(),
            new User{ UserName = name}
        };

        var userDALMock = new Mock<IUserDAL>();
        userDALMock.Setup(x => x.GetUsersAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(expectedResult);

        unitOfWorkMock.Setup(u => u.GetDAL<IUserDAL>()).Returns(userDALMock.Object);

        unitOfWorkFactoryMock.Setup(f => f.GetUnitOfWork())
          .Returns(unitOfWorkMock.Object);
        //Act
        var actualResult = await userService.GetUsersAsync(cancellationToken: CancellationToken.None);

        //Assert

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task GetPaginatedUsersAsync_PagedUsersReturned_ReturnsTaskOfPaginatedListOfUser()
    {
        //Arrange
        int expectedPageIndex = 1;
        var expectedPageSize = 10;

        IEnumerable<User> expectedInput = new List<User>
        {
            new User(),
            new User(),
            new User()
        };

        PaginatedList<User> expectedResult = PaginatedList<User>.ToPaginatedList(expectedInput, expectedPageIndex, expectedPageSize);

        var userDALMock = new Mock<IUserDAL>();
        userDALMock.Setup(x => x.GetPaginatedUsersAsync(It.IsAny<Int32>(), It.IsAny<Int32>(), CancellationToken.None)).ReturnsAsync(expectedResult);

        unitOfWorkMock.Setup(u => u.GetDAL<IUserDAL>()).Returns(userDALMock.Object);

        unitOfWorkFactoryMock.Setup(f => f.GetUnitOfWork())
          .Returns(unitOfWorkMock.Object);

        //Act
        var actualResult = await userService.GetPaginatedUsersAsync(expectedPageIndex, expectedPageSize, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task UpdateUserAsync_UserUpdated_ReturnsTaskOfBoolean()
    {
        //Arrange
        var id = new Guid("f7e6c4de-2713-41ee-9f4f-19f398aa9ffc");
        var expectedInput = new User
        {
            Id = default!,
            UserName = "Dom",
            UserDetail = default!,
            UserRoles = default!,
            Password = "12345"
        };
        var userDALMock = new Mock<IUserDAL>();
        userDALMock.Setup(x => x.UpdateUserAsync(It.IsAny<User>(), CancellationToken.None)).ReturnsAsync(true);
        userDALMock.Setup(u => u.UpdateUserDetailAsync(It.IsAny<UserDetail>(), CancellationToken.None)).ReturnsAsync(true); //Test UserDetails

        unitOfWorkMock.Setup(u => u.GetDAL<IUserDAL>()).Returns(userDALMock.Object);
        unitOfWorkMock.Setup(x => x.CommitAsync(CancellationToken.None));

        unitOfWorkFactoryMock.Setup(f => f.GetUnitOfWork())
          .Returns(unitOfWorkMock.Object);

        //Act
        var actualResult = await userService.UpdateUserAsync(id, expectedInput, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    
}