﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Application.Models
{
    public class ResultModel
    {
        public Guid Id { get; set; }
        public required Guid AppointmentId { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recomendations { get; set; }
    }

    public class CreateResultModel
    {
        public required Guid AppointmentId { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recomendations { get; set; }
    }

    public class UpdateResultModel
    {
        public required Guid Id { get; set; }
        public required string Complaints { get; set; }
        public required string Conclusion { get; set; }
        public required string Recomendations { get; set; }
    }
}
