﻿namespace ImportExport.Models;

public partial class TblCustomer
{
    public int CusId { get; set; }

    public string FullName { get; set; }

    public string Salutation { get; set; }

    public string Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<TblRent> TblRents { get; set; } = new List<TblRent>();
}