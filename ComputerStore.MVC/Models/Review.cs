using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerStore.MVC.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        // Liên kết với bảng Sản phẩm (Biết bình luận này thuộc về máy tính nào)
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        // Lưu lại tên người dùng đã bình luận (Lấy từ tài khoản đăng nhập)
        public string? Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung đánh giá")]
        public string? Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // Đánh giá từ 1 đến 5 sao

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Tự động lấy giờ hiện tại
    }
}