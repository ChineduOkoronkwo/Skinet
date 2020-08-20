using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandsSpecification()
        {
            IncludeBrandsAndTypes();
        }

        public ProductWithTypesAndBrandsSpecification(int Id) 
            : base(x => x.Id == Id)
        {
            IncludeBrandsAndTypes();
        }

        private void IncludeBrandsAndTypes() 
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}