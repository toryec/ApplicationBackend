using System.Data.Common;
using DapperApi.Service;
using Domain.Models;
using Infastructure.DALs;
using Microsoft.Data.Sqlite;

namespace AppBackendTest.IntegrationTests;

[TestFixture]
public class UserDALTests
{
    private DbConnection dbConnection;
    private UserDAL sut;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        dbConnection = new SqliteConnection("Data Source=InMemorySample;Mode=Memory;Cache=Shared");
        CreateTables();
        InsertDummyData();
        Configurations.AddTypes();
    }

    [SetUp]
    public void SetUp()
    {
        sut = new UserDAL(dbConnection);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await dbConnection.DisposeAsync();
    }

    [Test]
    public async Task CreateUser_UserCreated_ReturnsTaskOfBoolean()
    {
        //Arrange

        var user = new User
        {
            Password = "12342",
            UserName = "aroon@yahoo.com",
            UserDetail = default,
            UserRoles = default
        };

        //using var cancellationToken = new CancellationTokenSource();

        //Act
        var actualResult = await sut.CreateUserAsync(user, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task DeleteUserAsync_UserDeteled_ReturnsTaskOfBoolean()
    {
        //Arrange
        var id = new Guid("7c3a4019-803e-49aa-b02f-dafebbb542b4");

        //Act
        //dbConnection.Open();
        var actualResult = await sut.DeleteUserAsync(id, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task GetUsersAsync_UsersReturned_ReturnsTaskOfIEnumerableOfUser()
    {
        //Arrange
        var expectedInput = "DoeJane";

        var insertCommand = dbConnection.CreateCommand();
        insertCommand.CommandText =
           @"
                 INSERT INTO User (Id, UserName, Password)
                 VALUES
                    ('70FB1509-2378-43EB-94C3-650160B86E86', 'DoJohn', '123642')
           ";

        int result = await insertCommand.ExecuteNonQueryAsync();

        //Act
        var actualResult = await sut.GetUsersAsync(expectedInput, CancellationToken.None);

        //Assert
        Assert.That(actualResult.Any(), Is.True);
    }

    [Test]
    public async Task GetUserAsync_UserReturned_ReturnsTaskOfUser()
    {
        //Arrange
        var id = new Guid("ad9a566c-089a-471a-b5cf-e502f5bd6a44");

        //Act
        var actualResult = await sut.GetUserAsync(id, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.Not.Null);
    }

    [Test]
    public async Task UpdateUserAsync_UserUpdated_ReturnsTaskOfBoolean()
    {
        //Arrange
        var id = new Guid("ad9a566c-089a-471a-b5cf-e502f5bd6a44");
        var expectedInput = new User { Id = id, UserName = "Jamespit", Password = "1234312", UserDetail = default, UserRoles = default };

        //Act
        var actualResult = await sut.UpdateUserAsync(expectedInput, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task InsertUserDetailAsync_UserDetailCreated_ReturnsTaskOfBoolean()
    {
        //Arrange
        var id = new Guid("ad9a566c-089a-471a-b5cf-e502f5bd6a44");
        var expectedInput = new UserDetail { UserId = id, FirstName = "John", LastName = "Ark", Age = 16 };

        // Act
        var actualResult = await sut.InsertUserDetailAsync(expectedInput, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task UpdateUserDetailAsync_UserDetailUpdated_ReturnsTaskOfBoolean()
    {
        //Arrange
        var id = new Guid("f226071d-45ea-4230-93e4-52a432bd19f6");
        var expectedInput = new UserDetail { UserId = id, FirstName = "Jake", LastName = "Bagel", Age = 28 };

        //Act
        var actualResult = await sut.UpdateUserDetailAsync(expectedInput, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task GetUserDetailAsync_UserDetailReturned_ReturnsTaskOfUserDetail()
    {
        //Arrange
        var id = new Guid("c2269674-90eb-4fc9-a1b5-dce307d07083");

        // Act
        var actualResult = await sut.GetUserDetailAsync(id, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.Not.Null);
    }

    [Test]
    public async Task InsertRoleAsync_RoleCreated_ReturnsTaskOfBoolean()
    {
        //Arrange
        var id = new Guid("ad9a566c-089a-471a-b5cf-e502f5bd6a44");
        IEnumerable<UserRole> expectedInput = new List<UserRole>
        {
            new UserRole{UserId = id, RoleId = 3},
        };

        //Act
        var actualResult = await sut.InsertRoleAsync(expectedInput, CancellationToken.None);

        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task GetRoleAsync_RoleRetured_ReturnsTaskOfIEnumerableOfRole()
    {
        //Arrange
        var id = new Guid("f226071d-45ea-4230-93e4-52a432bd19f6");
        //Act
        var actualResult = await sut.GetRoleAsync(id, CancellationToken.None);
        //Assert

        Assert.That(actualResult.Any(), Is.True);
    }

    [Test]
    public async Task CheckIfExistsAsync_UserSearched_ReturnsTaskOfBoolean()
    {
        //Arrange
        var id = new Guid("c2269674-90eb-4fc9-a1b5-dce307d07083");
        //Act
        var actualResult = await sut.CheckIfExistsAsync(id, CancellationToken.None);
        //Assert
        Assert.That(actualResult, Is.True);
    }

    [Test]
    public async Task GetByUserName_UserReturned_ReturnsTaskOfUser()
    {
        //Arrange
        var expectedInput = "MikeSmith";
        //Act
        var actualResult = await sut.GetByUserName(expectedInput, CancellationToken.None);
        //Assert
        Assert.That(actualResult, Is.Not.Null);
    }

    [Test]
    public async Task CheckUserNameExsitsAsync_UserNameSearched_ReturnsTaskOfBoolean()
    {
        //Arrange
        var expectedInput = "MikeSmith";
        //Act
        var actualResult = await sut.CheckUserNameExistsAsync(expectedInput, CancellationToken.None);
        //Assert
        Assert.That(actualResult, Is.True);
    }

    private void CreateTables()
    {
        dbConnection.Open();

        var command = dbConnection.CreateCommand();
        command.CommandText =
            @"
                CREATE TABLE User (
                    Id TEXT NOT NULL PRIMARY KEY,
                    UserName TEXT UNIQUE,
                    Password TEXT NOT NULL
                )
            ";

        command.ExecuteNonQuery();

        var userDetailsCommand = dbConnection.CreateCommand();
        userDetailsCommand.CommandText =
            @"
                CREATE TABLE UserDetail (
                    UserId TEXT NOT NULL REFERENCES User(Id),
	                FirstName TEXT NOT NULL,
	                LastName TEXT NOT NULL,
	                Age INTEGER NOT NULL,
	                PRIMARY KEY (UserId),
	                CONSTRAINT UserDetail_FK FOREIGN KEY (UserId) REFERENCES User(Id) ON DELETE CASCADE ON UPDATE CASCADE
                )
            ";
        userDetailsCommand.ExecuteNonQuery();

        var roleCommand = dbConnection.CreateCommand();
        roleCommand.CommandText =
            @"
                CREATE TABLE Role (
                 Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                 RoleCode TEXT NOT NULL UNIQUE
                )
            ";

        roleCommand.ExecuteNonQuery();

        var userRoleCommand = dbConnection.CreateCommand();
        userRoleCommand.CommandText =
            @"
                CREATE TABLE UserRole (
                 RoleId INTEGER NOT NULL REFERENCES Role(Id),
                 UserId TEXT NOT NULL REFERENCES User(Id),
                 PRIMARY KEY (RoleId, UserId) ,
                 CONSTRAINT UserRole_FK FOREIGN KEY (UserId) REFERENCES User(Id) ON DELETE CASCADE ON UPDATE CASCADE
                )
            ";

        userRoleCommand.ExecuteNonQuery();
    }

    private void InsertDummyData()
    {
        //dbConnection.Open();

        var insertCommand = dbConnection.CreateCommand();
        insertCommand.CommandText =
           @"
                 INSERT INTO User (Id, UserName, Password)
                 VALUES
                    ('7C3A4019-803E-49AA-B02F-DAFEBBB542B4', 'DoeJohn', '114597'),
                    ('F226071D-45EA-4230-93E4-52A432BD19F6', 'DoeJane', '52299'),
                    ('C2269674-90EB-4FC9-A1B5-DCE307D07083', 'MikeSmith', '784359'),
                    ('AD9A566C-089A-471A-B5CF-E502F5BD6A44', 'JohnArk', '145777')

            ";

        int result = insertCommand.ExecuteNonQuery();

        var roleCommand = dbConnection.CreateCommand();
        roleCommand.CommandText =
           @"
                INSERT INTO Role (Id,RoleCode)
                VALUES
                    (1,'System Administrator'),
                    (2,'Administrator'),
                    (3,'User')
           ";

        roleCommand.ExecuteNonQuery();

        var userDetailCommand = dbConnection.CreateCommand();
        userDetailCommand.CommandText =
           @"
                INSERT INTO UserDetail (UserId, FirstName, LastName, Age)
                VALUES
                   ('7C3A4019-803E-49AA-B02F-DAFEBBB542B4', 'Doe', 'John', '19'),
                   ('F226071D-45EA-4230-93E4-52A432BD19F6', 'Doe', 'Jane', '29'),
                   ('C2269674-90EB-4FC9-A1B5-DCE307D07083', 'Mike', 'Smith', '39')
           ";
        userDetailCommand.ExecuteNonQuery();

        // ('AD9A566C-089A-471A-B5CF-E502F5BD6A44', 'John', 'Ark', '14')

        var userRoleCommand = dbConnection.CreateCommand();
        userRoleCommand.CommandText =
            @"
            INSERT INTO UserRole (RoleId,UserId)
            VALUES
                (2,'7C3A4019-803E-49AA-B02F-DAFEBBB542B4'),
                (2,'F226071D-45EA-4230-93E4-52A432BD19F6'),
                (3,'C2269674-90EB-4FC9-A1B5-DCE307D07083')

            ";
        userRoleCommand.ExecuteNonQuery();
    }
}