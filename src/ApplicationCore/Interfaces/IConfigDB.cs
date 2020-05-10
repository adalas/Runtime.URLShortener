using System;

namespace Runtime.URLShortener.ApplicationCore.Interfaces.Config
{
    public interface IConfigDB
    {
        TimeSpan EntryTimeToLeave {get;}
    }
}