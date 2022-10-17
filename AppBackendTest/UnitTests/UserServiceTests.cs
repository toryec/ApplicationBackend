using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Services.Types;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AppBackendTest.UnitTests;
public class UserServiceTests
{
    private Mock<IPasswordHasher<User>> passwordHasherMock = default!;
    private Mock<IUnitOfWorkFactory> unitOfWorkFactoryMock = default!;
    private UserService userService;
    [SetUp]
    public void Setup()
    {
        unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>(MockBehavior.Strict);
        passwordHasherMock = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
    }
}
