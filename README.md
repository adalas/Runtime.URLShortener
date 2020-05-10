URLShortener WebApp
This is a WebApp that shortens URLs through a website.


Usage:
ShortenURL: https://localhost:4200/
Use Short URL: https://localhost:4200/s/{shortURL}

Dependencies:
Docker & Docker Compose

Quick Start:
Docker-Compose up


Main Features:
- WebApp in Angular;
- Backend in Dotnet.Core, exposing functionality through a REST API
- Supports https;
- URLS are shortened into 11 Base64 enconded chars. Base64 allow to reduce the size of the ShortURL in terms of characters to the User as it can encode 6 bits with just one character.
- URL deduplication, through the use of the URL Hash Value of 64bits
- Leverage of the data scheme Key-Value of the problem (ShortURL->URL), by using Key-Value database Redis with data persistence. This enables horizontal scalling and one of the fastest solutions for the problem
- URLs will persist in the system the number of days that we configure
- Protection against bad usage:
    -> text that is not URL is not accepted
    -> null values
    -> URLs larger than a configured value

Additional Implementation:
- Types of tests implemented:
    -> Unit Tests: against classes
    -> Integration Tests: interation between URLRepository and Redis
    -> Functional Tests: with the webserver up, backed by Redis, against the webservices;
  Note: Due to time restrictions tests were not extensively populated, However the objective here was to prove the following:
        1- The 3 type are absolutely essential;
        2- Solution architecture and decoupling allows this 3 types of testing easilly;
        3- I had the technical capability to implement the 3 types, with the help of the almighty .net core.

Solution Limitations:
Due to the fact that the system is deduplicating URLs, the limit of the URLs that we can store is limited by the capacity of our hash functions to properly generate hash values without collisions or with a number of collisions that will not impact the system. The collision probability of hash functions is mainly dictated by the number of bits of the hash value. Current solution uses 64 bits hash values. While 64bits hash values, in theory allow to encode 18446744 Tera different numbers, the ability of an hash function to generate a value without collisions is way lower, specially due to the Birthday Paradox (https://en.wikipedia.org/wiki/Birthday_problem). This paradox states that , for instance, in a random group of 23 people, there is about a 50 % chance that two people have the same birthday, when to have 100% probability it would take 367 people. For a system like ours 50% of getting a collision is not acceptable at all, and this probability is very important for the system reliability to the user. For instance, with a 64 bits hash function and assuming we would accept a probability of collision of 1 in a million, the maximum number of URLs that we can persist in the system at the same time is only 6.07 millions. Of course, we could increase the size of the hash function from 64 bits to 128 bits, but this would double the size of our ShortURL.

If no URL deduplication  was supported, we could play with the maximum time of allowed URLs to remain in the system, and encode the short URL based on a timestamp that is designed to only track time within the maximum time windows of URLs to remain in the system. This way the possibility to get a collision only happens if requests arrive at the same time. So if URLs remain in the system for a maximum x days, and using timestamps with ms precision, our timestamp would only need to encode information of d:hh:mm.ss:ms. If we assume d = 30, then we could encode the timestamp with just 32 bits. Assuming that there could exist URLs arriving at the same timestamp (ms), we would add more 32 bits of the URL hash value to the ShortURL value. The ShortURL would be composed of 32bit timestamp+32 bits hash value. With 32 bits hash value, and maintaining the probability of collision to 1 in a million, we would be limited to a maximum of 93 hash values per ms, meaning a capacity of processing requests of 93K requests per second. With this data the maximum number of URLs to hold would be 93*1000*60*60*24*30=241.056.000.000 if d =30. We can conclude then that not deduplicating URLs enable us to serve way more URLs in the system, using ShortURLs with the same size.

Note: the solution implemented used deduplication because when I asked by email which of the two approaches were better to implement, deduplication was indicated e as the preferred one.



Main Features:


Technologies Used:
Dotnet Core 3.1
Angular 8
Redis 
Docker & Docker Compose