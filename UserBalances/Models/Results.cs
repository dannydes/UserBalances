using System;

namespace UserBalance.Models
{
    public class Result
    {
        public string Error { get; set; }
    }

    public class ResultID : Result
    {
        public Guid ID { get; set; }
    }

    public class ResultFloat : Result
    {
        public float Value { get; set; }
    }
}