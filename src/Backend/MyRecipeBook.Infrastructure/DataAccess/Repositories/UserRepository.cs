using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public UserRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;
    //escreve no banco
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
    
    //verifica se já tem usuario ativo no banco
    public async Task<bool> ExistActiveUserWithEmail(string email) => 
        await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

}
