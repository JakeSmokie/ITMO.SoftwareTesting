using System;

namespace ITMO.SoftwareTesting.Dates.Contracts.Exceptions
{
    public class DatesException : Exception
    {
        public DatesException(string message) : base(message)
        {
        }
    }
}