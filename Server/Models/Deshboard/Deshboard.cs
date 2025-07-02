using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EskomAdmin.Server.Models.Deshboard
{
    [Table("Deshboard", Schema = "dbo")]
    public partial class Deshboard
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int TrendNumber { get; set; }

        [ConcurrencyCheck]
        public DateTime? StartDate { get; set; }

        [ConcurrencyCheck]
        public DateTime? EndDate { get; set; }

        [ConcurrencyCheck]
        public string Summary { get; set; }

        [ConcurrencyCheck]
        public string TrendDescription { get; set; }

        [ConcurrencyCheck]
        public string Address { get; set; }

        [Column(TypeName="xml")]
        [ConcurrencyCheck]
        public string FileUpload { get; set; }
    }
}