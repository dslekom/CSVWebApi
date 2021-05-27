using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DAL.Entities
{
    /// <summary>
    /// Тестовая модель
    /// </summary>
    public class TestModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime? EditedAt { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return $"{Name};{Description};{CreatedAt};{(EditedAt.HasValue ? EditedAt.Value.ToString() + ";" : "")}{IsDeleted}";
        }
    }
}
