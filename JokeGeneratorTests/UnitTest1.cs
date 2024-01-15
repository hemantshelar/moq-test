using System.Security.Cryptography;
using JokeGenerator.Models;
using JokeGenerator.Services;
using Moq;
using Moq.Protected;

namespace JokeGeneratorTests;

[TestClass]
public class JokeProviderTest
{
    [TestMethod]
    public async Task GetJokeCategoriesShouldReturnListOfString()
    {
        //Arrange
        HttpResponseMessage result = new HttpResponseMessage();
        result.Content =  new StringContent($"[\"animal\",\"career\",\"celavebrity\",\"dev\",\"explicit\",\"fashion\",\"food\",\"history\",\"money\",\"movie\",\"music\",\"political\",\"religion\",\"science\",\"sport\",\"travel\"]");

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(result)
            .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object){
            BaseAddress = new Uri("https://api.chucknorris.io/jokes/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(_ => _.CreateClient(AppConstants.CNJokeGenerator)).Returns(httpClient);

        IJokeProvider jockProvider = new JokeProvider(mockHttpClientFactory.Object);

        //Act
        var r = await jockProvider.GetJokeCategories();
        

        //Assert
    }
}