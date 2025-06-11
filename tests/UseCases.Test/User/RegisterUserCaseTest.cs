using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User;

public class RegisterUserCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        //Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();


        //Act
        var result = await useCase.Execute(request);

        //Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        //Arrange
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        //Act
        Func<Task> act = async () => await useCase.Execute(request);

        //Assert
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 
            && e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
    }
    [Fact]
    public async Task Error_Name_Empty()
    {
        //Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        //Act
        Func<Task> act = async () => await useCase.Execute(request);

        //Assert
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1
            && e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
    }

    private RegisterUserCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var writRepository = UserWriteOnlyRepositoryBuilder.Builder();
        var unitOfWork = UnitOfWorkBuilder.Builder();
        var readRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrEmpty(email) == false)
            readRepositoryBuilder.ExistActiveUserWithEmail(email);


        return new RegisterUserCase(writRepository, readRepositoryBuilder.Build(), 
            unitOfWork, mapper, passwordEncripter);
    }
}
