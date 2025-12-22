using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Person.Application.Interfaces.Repositories;
using Person.Infrastructure.Data;

namespace Person.Infrastructure.Repositories;

public class PersonRepository : RepositoryBase<Domain.Entities.Person>, IPersonRepository
{
    private readonly PersonDbContext _context;

    public PersonRepository(PersonDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByEmailAsync(string email, Guid? excludePersonId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Persons
            .Where(p => p.Email.Value == email);

        if (excludePersonId.HasValue)
        {
            query = query.Where(p => p.Id != excludePersonId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}