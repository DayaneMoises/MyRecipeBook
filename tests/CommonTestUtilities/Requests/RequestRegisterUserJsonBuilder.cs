using Bogus;
using MyRecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, (faker) => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, userjson) => faker.Internet.Email(userjson.Name))
            .RuleFor(user => user.Password, (faker) => faker.Internet.Password(passwordLength));

     }
}
