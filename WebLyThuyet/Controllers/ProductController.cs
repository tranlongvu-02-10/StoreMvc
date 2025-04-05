using Microsoft.AspNetCore.Mvc;
using WebLyThuyet.Models;
using WebLyThuyet.Repositories;

namespace WebLyThuyet.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            var totalItems = await _productRepository.CountAsync();
            var products = await _productRepository.GetPagedAsync(page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = Math.Max(1, (int)Math.Ceiling((double)totalItems / pageSize));

            return View(products);
        }

        // Hiển thị chi tiết sản phẩm
        [HttpGet]
        [Route("Display/{id:int}")]
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
