using System;
using System.ComponentModel.DataAnnotations;

namespace _2.ExpenseManagement.Api.DTOs.Attachments
{
    public class AttachmentEditRequest
    {
        public List<AttachmentAddData>? Attachments { get; set; }
    }
}

