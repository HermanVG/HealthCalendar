using System;
namespace HealthCalendar.DAL
{
    public enum RepoStatus
    {
        Success = 1,
        Error = -1,
        NotFound = -2,
        Unauthorized = -3
    }
}