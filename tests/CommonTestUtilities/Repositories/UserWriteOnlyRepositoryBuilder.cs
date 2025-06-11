using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Builder()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object; ;
    }
}
