using System.Data;

namespace OnlineLibrarySystem.Models 
{ 
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Role Role { get; set; }
    public ICollection<BorrowedBook> BorrowedBooks { get; set; }
}

}
