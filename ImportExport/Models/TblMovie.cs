namespace ImportExport.Models;

public partial class TblMovie
{
    public int MvId { get; set; }

    public string Title { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<TblRent> TblRents { get; set; } = new List<TblRent>();
}