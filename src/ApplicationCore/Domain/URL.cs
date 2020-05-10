using System;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;
using Runtime.URLShortener.ApplicationCore.Interfaces;

namespace Runtime.URLShortener.ApplicationCore.Entities
{
    public class URL: IAggregateRoot
    {
        public ShortURL Id{get; private set;}
        public string Value {get; private set;}

        public URL(string extended)
        {
            Value = extended;
            Id = ShortURL.ComputeShortURLFromExtendedURL(extended);
        }

        public URL(string value, ShortURL shortURL)
        {
            Value = value;
            Id = shortURL;
        }


        public override string ToString()
        {
            return Value;
        }

    }
}