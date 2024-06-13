namespace ImportExport.Models;

public partial class TblRent
{
    public int RentId { get; set; }

    public int CusId { get; set; }

    public int MvId { get; set; }

    public DateTime RentAt { get; set; }

    public DateTime? ReturnAt { get; set; }

    public virtual TblCustomer Cus { get; set; }

    public virtual TblMovie Mv { get; set; }
}