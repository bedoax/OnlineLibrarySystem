namespace OnlineLibrarySystem.Models { 
public class BorrowedBook
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime BorrowedDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }

    public User User { get; set; }
    public Book Book { get; set; }
}

}
