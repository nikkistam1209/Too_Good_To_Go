using Core.Domain.Entities;
using Core.DomainServices.IServices;
using HotChocolate.Types;

namespace WebAPI.GraphQL
{
    public class GetPackagesQuery
    {
        private readonly IPackageService _packageService;

        public GetPackagesQuery(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public IEnumerable<Package> GetAvailablePackages() => _packageService.GetAvailablePackages();
    }

}
