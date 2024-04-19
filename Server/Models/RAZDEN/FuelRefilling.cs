using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DeanRemoteMonitoringWeb.Server.Models.RAZDEN
{
    [Table("Fuel Refilling", Schema = "dbo")]
    public partial class FuelRefilling
    {
        [Key]
        [Required]
        public int Tank { get; set; }

        [Column("Litres Filled")]
        public decimal? LitresFilled { get; set; }

        [Column("Company Refilling")]
        public string CompanyRefilling { get; set; }

        public string Authorisation { get; set; }

        public string Supervisor { get; set; }

        public DateTime? Date { get; set; }

    }
}