using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyModel;
using MyRecipeBook.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient1;
    public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient1 = factory.CreateClient();

    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await _httpClient1.PostAsJsonAsync("User", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var responseBory = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBory);

        responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(request.Name);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        if (_httpClient1.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient1.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient1.DefaultRequestHeaders.Add("Accept-Language", culture);

        var response = await _httpClient1.PostAsJsonAsync("User", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBory = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBory);

        var erros = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));
        erros.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
