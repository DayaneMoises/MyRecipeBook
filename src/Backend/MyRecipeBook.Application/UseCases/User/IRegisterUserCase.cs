using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.UseCases.User;

public interface IRegisterUserCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);

}
