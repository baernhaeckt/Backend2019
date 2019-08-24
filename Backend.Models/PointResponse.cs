using System;
using System.Collections.Generic;

namespace Backend.Models
{
    public class PointResponse
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public IEnumerable<MetaDataResponse> MetaData { get; set; } = new List<MetaDataResponse>();

        public String Text { get; set; }

        public int Value { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
