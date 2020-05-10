using System;
using Microsoft.Extensions.Configuration;
using Runtime.URLShortener.ApplicationCore.Interfaces.Config;

namespace Runtime.URLShortener.Infrastructure.Config
{
    public class ConfigDB:IConfigDB
    {

        public int EntryTimeToLeaveDays {get;set;}
        public TimeSpan EntryTimeToLeave => new TimeSpan(EntryTimeToLeaveDays,0,0,0);


    }
}
