using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExpensesManagment.Common.Models
{
    public class ExpenseRequest
    {
        public int Id { get; set; }

        public string Details { get; set; }

        [Required]
        public float Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime DateToLocal => Date.ToLocalTime();

        public byte[] PictureArray { get; set; }

        [Required]
        public int ExpenseTypeId { get; set; } //1) Food, 2) Lodging, 3)Transport, 4) Oil, 5) HealthCare, 6)Phone, 7)Other

        [Required]
        public int TripId { get; set; }

        public string UserEmail { get; set; }

    }
}
