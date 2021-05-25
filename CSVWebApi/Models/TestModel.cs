using System;
using System.ComponentModel.DataAnnotations;

namespace CSVWebApi.Models
{
    public class TestModel
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? EditedAt { get; set; }

        public bool IsDeleted { get; set; }

        public static string[] GetColumnNames()
        {
            return new[]
            {
                nameof(Name),
                nameof(Description),
                nameof(CreatedAt),
                nameof(EditedAt)
            };
        }
    }
}
