namespace QuanLyChuoiBanXoi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CTHD")]
    public partial class CTHD
    {
        [Required]
        [StringLength(6)]
        public string MaHD { get; set; }

        [Key]
        [StringLength(6)]
        public string MaCT { get; set; }

        [Required]
        [StringLength(6)]
        public string MaXoi { get; set; }

        [Required]
        [StringLength(15)]
        public string TenXoi { get; set; }

        public decimal Gia { get; set; }

        public int SoLuong { get; set; }

        public decimal? ThanhTien { get; set; }

        public virtual DSXoi DSXoi { get; set; }

        public virtual HoaDon HoaDon { get; set; }
    }
}
