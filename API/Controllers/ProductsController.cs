using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Helper;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productrepo;
        private readonly IGenericRepository<ProductBrand> _brandrepo;
        private readonly IGenericRepository<ProductType> _typerepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productrepo,
        IGenericRepository<ProductBrand> brandrepo, IGenericRepository<ProductType>
        typerepo, IMapper mapper)
        {
            _mapper = mapper;
            _typerepo = typerepo;
            _brandrepo = brandrepo;
            _productrepo = productrepo;

        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
           [FromQuery] ProductSpecParams productparams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productparams);

            var countSpec = new ProductWithFiltersForCountSpecification(productparams);

            var totalItems = await _productrepo.CountAsync(countSpec);

            var products = await _productrepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productparams.PageIndex, productparams.PageSize, totalItems, data)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productrepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _brandrepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _typerepo.ListAllAsync());
        }


    }
}