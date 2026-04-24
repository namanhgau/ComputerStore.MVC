using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.MVC.Controllers
{
    // "Bùa chú" phân quyền đây rồi! 
    // Chỉ những ai đăng nhập VÀ có Role là Admin hoặc Staff mới được vào cái Controller này.
    [Authorize(Roles = "Manager, Staff")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //thêm các hàm quản lý sản phẩm, đơn hàng ở đây
    }
}