using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineLibrarySystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Published Date is required")]
        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        public string Description { get; set; }


        public string CoverImage { get; set; }

        
        public string PDFFilePath { get; set; }

        [Required(ErrorMessage = "Available Copies is required")]
        public int AvailableCopies { get; set; }

        [Required(ErrorMessage = "Total Copies is required")]
        public int TotalCopies { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        public DateTime AddedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}
