using System.Security.Cryptography;
using Castle.Core.Logging;
using JokeGenerator.Models;
using JokeGenerator.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace JokeGeneratorTests;

[TestClass]
public class JokeProviderTest
{
    private readonly string _jokeCats = $"[\"animal\",\"career\",\"celavebrity\",\"dev\",\"explicit\",\"fashion\",\"food\",\"history\",\"money\",\"movie\",\"music\",\"political\",\"religion\",\"science\",\"sport\",\"travel\"]";
    [TestMethod]
    public async Task GetJokeCategoriesShouldReturnListOfString()
    {
        //Arrange
        HttpResponseMessage result = new HttpResponseMessage();
        result.Content = new StringContent(_jokeCats);

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(result)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.chucknorris.io/jokes/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(AppConstants.CNJokeGenerator)).Returns(httpClient);

        IOptions<JokesAPI> _options = Options.Create<JokesAPI>(new JokesAPI
        {
            CNJokeGeneratorAPIOperations = new CNJokeGeneratorAPIOperations
            {
                Categories = "categories"
            },
            EndPoint = ""

        });

        var _logger = new Mock<ILogger<JokeProvider>>();
        var _customLogger = new Mock<CustomLogger>();
        IJokeProvider jockProvider = new JokeProvider(mockHttpClientFactory.Object, _options, _logger.Object,_customLogger.Object);

        //Act
        var r = await jockProvider.GetJokeCategories();

        //Assert
        Assert.IsNotNull(r);
        Assert.AreEqual(r.Count(), 16);
        handlerMock
        .Protected()
        .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
    }

    [TestMethod]
    public void ctest()
    {
        //Arrange
        HttpResponseMessage result = new HttpResponseMessage();
        result.Content = new StringContent(_jokeCats);

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(result)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.chucknorris.io/jokes/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(AppConstants.CNJokeGenerator)).Returns(httpClient);

        IOptions<JokesAPI> _options = Options.Create<JokesAPI>(new JokesAPI
        {
            CNJokeGeneratorAPIOperations = new CNJokeGeneratorAPIOperations
            {
                Categories = "categories"
            },
            EndPoint = ""

        });

        var _logger = new Mock<ILogger<JokeProvider>>();
        var _customLogger = new Mock<CustomLogger>();
        IJokeProvider jockProvider = (new JokeProvider(mockHttpClientFactory.Object, _options, _logger.Object,_customLogger.Object));

    }

    [TestMethod]
    public async Task GetJokeCategoriesShouldInvoke_WriteLog_Exactly_Once()
    {
        //Arrange
        HttpResponseMessage result = new HttpResponseMessage();
        result.Content = new StringContent(_jokeCats);

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(result)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.chucknorris.io/jokes/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(AppConstants.CNJokeGenerator)).Returns(httpClient);

        IOptions<JokesAPI> _options = Options.Create<JokesAPI>(new JokesAPI
        {
            CNJokeGeneratorAPIOperations = new CNJokeGeneratorAPIOperations
            {
                Categories = "categories"
            },
            EndPoint = ""

        });

        var _customLogger = new Mock<ICustomLogger>();
        //_customLogger.Setup(c=> c.WriteLog("test",1)).Verifiable();
        var _logger = new Mock<ILogger<JokeProvider>>();
        IJokeProvider jockProvider = new JokeProvider(mockHttpClientFactory.Object, _options, _logger.Object,_customLogger.Object);

        var jc = await jockProvider.GetJokeCategories();

        //_customLogger.Verify( l => l.WriteLog("test",1), Times.Once);
        _customLogger.Verify( l => l.WriteLog(It.IsAny<string>(),It.IsAny<int>()), Times.Once);
    }

}