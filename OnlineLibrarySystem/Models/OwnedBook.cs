using OnlineLibrarySystem.Models;

public class OwnedBook
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime PurchaseDate { get; set; }

    public virtual User User { get; set; }
    public virtual Book Book { get; set; }
}
