using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    [Table("Clients")]
    public class Clients
    {
        [Key]
        [Column("client_id")]
        public int ClientId { get; set; }
        [Column("client_code")]
        public string? ClientCode { get; set; }
        [Column("client_name")]
        public string? ClientName { get; set; }
        [Column("company_name")]
        public string? CompanyName { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("phone")]
        public string? Phone { get; set; }
        [Column("address")]
        public string? Address { get; set; }
        [Column("city")]
        public string? City { get; set; }
        [Column("country")]
        public string? Country { get; set; }
        [Column("subscription_plan")]
        public string? SubscriptionPlan { get; set; }
        [Column("subscription_start_date")]
        public DateTime? SubscriptionStartDate { get; set; }
        [Column("subscription_end_date")]
        public DateTime? SubscriptionEndDate { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [Column("database_type")]
        public string? DatabaseType { get; set; }
        [Column("database_connection_string")]
        public string? DatabaseConnectionString { get; set; }
        
    }
}
