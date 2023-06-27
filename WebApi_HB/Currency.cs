using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Test3

{
    public class Currency
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(60)]
        public string Title { get; set; }

        [Required]
        [MaxLength(3)]
        public string Code { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public DateTime A_DATE { get; set; }
    }
}