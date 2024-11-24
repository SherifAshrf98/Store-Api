using AutoMapper;
using Store.APIs.DTOs;
using Store.Core.Entities;

namespace Store.APIs.Helpers.Mapping
{
    public class ProductPictureURLReSolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureURLReSolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {

            return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";

        }
    }
}
