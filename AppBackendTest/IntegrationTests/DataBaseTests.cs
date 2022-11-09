using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace AppBackendTest.IntegrationTests;

[TestFixture]
public class DataBaseTests
{
    private DbConnection dbConnection = default!;

    [OneTimeSetUp]
    public void Setup()
    {
        dbConnection = new SqliteConnection("Data Source=InMemorySample;Mode=Memory;Cache=Shared");
        CreateTable();
    }

    [OneTimeTearDown]
    public void closeconnections()
    {
        dbConnection.DisposeAsync();
    }

    [Test]
    public async Task CreateUserAsync_InsertsUser_ReturnsRowsAffected()
    {
        //Arrange

        var insertCommand = dbConnection.CreateCommand();
        insertCommand.CommandText =
           @"
                INSERT INTO User (Id, UserName, Age, Password)
                VALUES
                    ('187f0684-d1aa-4b57-87d6-a4d462e98eaf', 'JohnDoe', 20, '124567'),
                    ('d0700f0f-f653-4b48-9e5b-dfb92bba090d', 'JaneDoe', 21, '67788')
            ";

        //Act

        var actualResult = await insertCommand.ExecuteNonQueryAsync();

        //Assert

        Assert.That(actualResult, Is.GreaterThan(0));
        //Assert.Greater(actualResult, 0);
    }

    [Test]
    public async Task GetUserCount_CountsRecordsInUserTable_ReturnsUsersCount()
    {
        //Arrange
        var selectCommand = dbConnection.CreateCommand();
        selectCommand.CommandText =
           @"
                SELECT COUNT (*) FROM User
            ";

        //Act
        await dbConnection.OpenAsync();
        var actualResult = await selectCommand.ExecuteScalarAsync();
        //await dbConnection.CloseAsync();

        //Assert
        var result = Convert.ToInt32(actualResult);
        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetUser_SelectsUserInUserTable_ReturnsUser()
    {
        //Arrange
        var actualResult = new List<UserModel>();
        var selectCommand = dbConnection.CreateCommand();
        selectCommand.CommandText =
           @"
                SELECT Id, UserName, Age
                FROM User
                WHERE Id = '7dbe1299-779c-417a-92fb-b02f62f1cf1e'
           ";

        //Act
        await dbConnection.OpenAsync();

        var reader = await selectCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            actualResult.Add(new UserModel { Id = reader.GetString(0), UserName = reader.GetString(1), Age = reader.GetInt32(2) });
        }
        await reader.CloseAsync();
        //await dbConnection.CloseAsync();

        //Assert
        Assert.That(actualResult.Count, Is.EqualTo(1));
        //Assert.That(actualResult.Any(), Is.True);
    }

    [Test]
    public async Task GetUsers_SelectsAllUsersInUserTable_ReturnsUsers()
    {
        //Arrange
        var actualResult = new List<UserModel>();
        var selectCommand = dbConnection.CreateCommand();
        selectCommand.CommandText =
           @"
                SELECT Id, UserName, Age
                FROM User
            ";

        //Act
        await dbConnection.OpenAsync();

        var reader = await selectCommand.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            actualResult.Add(new UserModel { Id = reader.GetString(0), UserName = reader.GetString(1), Age = reader.GetInt32(2) });
        }
        await reader.CloseAsync();
        //await dbConnection.CloseAsync();

        //Assert
        Assert.That(actualResult.Any(), Is.True);
        //Assert.That(actualResult.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task UpdateUser_UpdatesAUser_ReturnsRowsAffected()
    {
        //Arrange

        var updateCommand = dbConnection.CreateCommand();

        updateCommand.CommandText =
            @"
                UPDATE User
                SET Age = 28
                WHERE
                Id = '91fcb60d-a6b3-46e0-8fa0-decd4ec31197'
            ";

        //Act
        await dbConnection.OpenAsync();
        var actualResult = await updateCommand.ExecuteNonQueryAsync();

        //Assert
        Assert.That(actualResult, Is.GreaterThan(0));
    }

    [Test]
    public async Task DeleteUser_DeletesAUser_ReturnsRowsAffected()
    {
        //Arrange

        var deleteCommand = dbConnection.CreateCommand();
        deleteCommand.CommandText =
            @"
                DELETE FROM User
                WHERE Id = 'a6f20286-8f83-47ae-992e-deae16c7d83c'
            ";

        //Act
        await dbConnection.OpenAsync();
        var actualResult = await deleteCommand.ExecuteNonQueryAsync();
        //await dbConnection.CloseAsync();

        //Assert
        Assert.That(actualResult, Is.GreaterThan(0));
    }

    private void CreateTable()
    {
        dbConnection.Open();
        var command = dbConnection.CreateCommand();
        command.CommandText =
            @"
                CREATE TABLE User (
                    Id TEXT PRIMARY KEY,
                    UserName TEXT,
                    Age INTEGER,
                    Password TEXT
                )
            ";

        command.ExecuteNonQuery();

        var insertCommand = dbConnection.CreateCommand();

        insertCommand.CommandText =
           @"
                INSERT INTO User (Id, UserName, Age, Password)
                VALUES
                   ('a6f20286-8f83-47ae-992e-deae16c7d83c', 'DoeJohn', 26, '114597'),
                   ('91fcb60d-a6b3-46e0-8fa0-decd4ec31197', 'DoeJane', 29, '52299'),
                   ('7dbe1299-779c-417a-92fb-b02f62f1cf1e', 'MikeSmith', 28, '784359')

            ";

        insertCommand.ExecuteNonQuery();
    }
}