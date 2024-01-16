using System.Security.Cryptography;
using JokeGenerator.Models;
using JokeGenerator.Services;
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
        IJokeProvider jockProvider = new JokeProvider(mockHttpClientFactory.Object, _options);

        //Act
        var r = await jockProvider.GetJokeCategories();

        //Assert
        Assert.IsNotNull(r);
        Assert.AreEqual(r.Count(), 16);
        handlerMock
        .Protected()
        .Verify("SendAsync", Times.Exactly(1), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());

    }
}