using Moq;
using NUnit.Framework;
using UserServiceApp;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _mockRepository;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        // Initialize the mock repository
        _mockRepository = new Mock<IUserRepository>();

        // Initialize the service with the mocked repository
        _userService = new UserService(_mockRepository.Object);
    }

    [Test]
    public void GetUserEmail_WhenUserExists_ReturnsEmail()
    {
        // Arrange
        var testUser = new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            IsActive = true
        };

        // Setup the mock to return testUser when GetUserById(1) is called
        _mockRepository.Setup(r => r.GetUserById(1)).Returns(testUser);

        // Act
        var result = _userService.GetUserEmail(1);

        // Assert
        Assert.AreEqual("john@example.com", result);
    }

    [Test]
    public void GetUserEmail_WhenUserIsNull_ReturnsNotFoundMessage()
    {
        // Arrange
        // Setup the mock to return null when GetUserById is called
        _mockRepository.Setup(r => r.GetUserById(999)).Returns((User)null);

        // Act
        var result = _userService.GetUserEmail(999);

        // Assert
        Assert.AreEqual("User not found", result);
    }

    [Test]
    public void GetUserEmail_WhenUserIsInactive_ReturnsInactiveMessage()
    {
        // Arrange
        var inactiveUser = new User
        {
            Id = 2,
            Name = "Jane Doe",
            Email = "jane@example.com",
            IsActive = false
        };

        // Setup the mock
        _mockRepository.Setup(r => r.GetUserById(2)).Returns(inactiveUser);

        // Act
        var result = _userService.GetUserEmail(2);

        // Assert the result equals "User is inactive"
        Assert.AreEqual("User is inactive", result);
    }

    [Test]
    public void ActivateUser_WhenUserExists_ActivatesAndSaves()
    {
        // Arrange
        var testUser = new User
        {
            Id = 3,
            Name = "Bob Smith",
            Email = "bob@example.com",
            IsActive = false
        };

        _mockRepository.Setup(r => r.GetUserById(3)).Returns(testUser);

        // Setup SaveUser to return true
        _mockRepository.Setup(r => r.SaveUser(testUser)).Returns(true);

        // Act
        var result = _userService.ActivateUser(3);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(testUser.IsActive);

        // Verify that SaveUser was called once
        _mockRepository.Verify(r => r.SaveUser(testUser), Times.Once);
    }

    [Test]
    public void ActivateUser_WhenUserNotFound_ReturnsFalse()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetUserById(It.IsAny<int>())).Returns((User)null);

        // Act
        var result = _userService.ActivateUser(4);  // Any ID works since mock returns null for any

        // Assert
        Assert.IsFalse(result);

        // Verify that SaveUser was NEVER called
        _mockRepository.Verify(r => r.SaveUser(It.IsAny<User>()), Times.Never);
    }
}