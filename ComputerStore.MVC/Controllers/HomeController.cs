using ComputerStore.MVC.Models; // Thêm dòng này để gọi Models
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Thêm dòng này để dùng Include, ToListAsync
using System.Diagnostics;

namespace ComputerStore.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ComputerStoreDbContext _context;

        // Bơm Database vào HomeController
        public HomeController(ComputerStoreDbContext context)
        {
            _context = context;
        }

        // Thêm 2 tham số: searchString (chữ khách hàng gõ) và categoryId (danh mục khách chọn)
        // ĐỔI Ở ĐÂY: Sửa string categoryId thành int? categoryId
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }

            // ĐỔI Ở ĐÂY: Kiểm tra xem categoryId có chứa số (có giá trị) hay không
            if (categoryId != null)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            return View(await products.ToListAsync());
        }

        // GET: Home/Details/5
        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin Sản phẩm + Danh mục + Danh sách Đánh giá
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews) // Lệnh này giúp kéo theo toàn bộ bình luận của máy này
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Home/AddReview
        [HttpPost]
        public async Task<IActionResult> AddReview(int productId, string username, string comment, int rating)
        {
            // Tạo một khung bình luận mới
            var review = new Review
            {
                ProductId = productId,
                Username = string.IsNullOrEmpty(username) ? "Khách vô danh" : username,
                Comment = comment,
                Rating = rating,
                CreatedAt = DateTime.Now // Lấy giờ hiện tại
            };

            // Thêm vào Database và Lưu lại
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Lưu xong thì tự động load lại trang chi tiết của đúng cái máy tính đó
            return RedirectToAction("Details", new { id = productId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}