using Ardalis.Specification;
using Domain.Client;

namespace Application.Specifications;

public class PagedClientsSpecification : Specification<Client>
{
    public PagedClientsSpecification(int pageSize, int pageNumber, string name, string lastName)
    {
        Query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        if (!string.IsNullOrWhiteSpace(name))
        {
            Query.Search(x => x.Name, $"%{name}%");
        }

         if (!string.IsNullOrWhiteSpace(lastName))
        {
            Query.Search(x => x.LastName, $"%{lastName}%");
        }
    }
}