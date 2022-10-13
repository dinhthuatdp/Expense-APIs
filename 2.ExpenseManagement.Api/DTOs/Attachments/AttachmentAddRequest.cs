using System;
using System.ComponentModel.DataAnnotations;

namespace _2.ExpenseManagement.Api.DTOs.Attachments
{
    public class AttachmentAddRequest
    {
        public List<AttachmentAddData>? Attachments { get; set; }
    }

    public class AttachmentAddData
    {
        public string? Name { get; set; }

        public string? Url { get; set; }

        public Guid ExpenseID { get; set; }
    }
}

