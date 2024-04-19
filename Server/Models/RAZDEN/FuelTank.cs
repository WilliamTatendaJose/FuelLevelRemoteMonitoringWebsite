using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeanRemoteMonitoringWeb.Server.Models.RAZDEN
{
    [Table("FuelTanks", Schema = "dbo")]
    public partial class FuelTank
    {
        [Key]
        [Required]
        public int Tank { get; set; }

        [Column("Fuel Left")]
        public double? FuelLeft { get; set; }

        [Column("Fuel Comsumed/Day")]
        public double? FuelComsumedDay { get; set; }

        public DateTime? Date { get; set; }

    }
}