# URLShortener WebApp #

This is a WebApp that shortens URLs through a website. 

You can visit it at: https://cvrnnmaskjs.westeurope.cloudapp.azure.com:5001/ 

(I am reusing an Azure machine to put this online, so the domain name is not related)

### Usage ###
Access the portal to ShortenURL: https://localhost:5001/

Use Short URL: https://localhost:5001/s/{shortURL}

### System Setup ###
On the repository root folder execute:

`> docker-compose up`

### Dependencies ###
Built with:
- Docker version 19.03.8
- Docker Compose version 1.25.5

### Main Features ###
- URLS are *shortened into 11 chars*.
- The ShortURL value of the URL is based on the *URL hash value*. Current hash size has 64 bits.
- ShortURL character are encoded in base64,which reduces the number of lenght of the URL in terms of characters, as it can encode 6 bits with just one character.
- System supports *URL deduplication*, by detecting existent URLs and retrieving the ShortURL that is already stored;
- URLs will persist in the system the number of days that we configure;
- Protection against bad usage:
    1. text that is not URL is not accepted
    2. null values
    3. URLs larger than a configured value

### Solution/Architecture Characteristics ###
![Architecture Overview](https://github.com/jpsalada/Runtime.URLShortner/blob/master/assets/images/architectur.png)

- WebApp UI implemented in **AngularJS**, v8.12, running on .net krestel web server and driven by **dotnet.core.asp**;
- Backend in **dotnet.core.sdk and asp v3.1, exposing functionality through a REST API.
- Supports **https**, with self-signed certificates for krestel web server;
- The storage database selected was **Redis** with persistence enabled. The mapping of a ShortURL to an URL matches perfectly the paradigm of Key-Value access. There are another Key-Value databases but Redis not only is a proven solution, as it also allows data persistence. This is very important, because Key-Value databases are most of the times designed for cach only purposes. A Key-Value database, allows horizontal scalling which I believe is the correct scaling solution for our problem.


### Additional Implementation ###
- Types of tests implemented:
    1. *Unit Tests*: against classes
    2. *Integration Tests*: interation between URLRepository and Redis
    3. *Functional Tests*: with the webserver up, backed by Redis, against the webservices;
    
  **Note:** Due to time restrictions tests were not extensively populated. Given the time I preferred to demonstrate the 3 types of testing, instead of just implementing Unit Tests. My objective here here was to demonstrate:
        1- The 3 types of testing are absolutely essential. We could still add other types, such as, load/stress or penetration testing.
        2- The solution architecture and decoupling allows these 3 types of testing easilly;

### Security Concerns ###
- Sensitive data, such as, https certificates, and passwords are present in the repository. Passwords should never be in configuration files. This is far away from the ideal, but I chose to put them here, so the person that will evaluate the solution will not be configuring or downloading certificates or passwords. Usually these steps are not so straightforward.

- The proposed solution to safe secrets of sensitive data for production environments is solutions like Azure Vault. For development environments there is the dotnet secrets system.

### Solution Limitations ###
Due to the fact that the system is deduplicating URLs, the limit of the URLs that we can store is limited by the capacity of our hash functions to properly generate hash values without collisions, or by a number of collisions that will not impact the system. 

The collision probability of hash functions is mainly dictated by the number of bits of the hash value. Current solution uses 64 bits hash values. While 64bits hash values, in theory allow to encode 2^64= 18446744 Tera different numbers, the ability of an hash function to generate a value without collisions is way lower, specially due to the Birthday Paradox (https://en.wikipedia.org/wiki/Birthday_problem). 

The birthday paradox states that, for instance, in a random group of 23 people, there is about a 50 % chance that two people have the same birthday, when to have 100% probability it would take 367 people. For a system like a URLShortener 50% of getting a collision is not acceptable at all, because it would be 50% of the ShortURLs would point to the wrong URL and thus defeating the whole purpose of the system. 

For instance, with a 64 bits hash function and assuming we would accept a probability of collision of 1 in a million, the maximum number of URLs that we can persist in the system at the same time is only 6.07 millions. Of course, we could increase the size of the hash function from 64 bits to 128 bits, but this would double the size of our ShortURL, having direct impact in the usability.

![Table of hash collisions](https://github.com/jpsalada/Runtime.URLShortner/blob/master/assets/images/hashcollisions.jpg)

There are many possible solutions and alternatives. One possible alternative solution, and in case we give up on URL deduplication, we could play with the maximum time of allowed URLs to remain in the system, and encode the ShortURL, based on a timestamp that is designed to only track time within the maximum time window that URLs remain in the system. 

If we do so, the possibility to get a collision only happens if requests arrive at the same time. So if URLs remain in the system for a maximum x days, and using timestamps with ms precision, our timestamp would only need to encode information of d:hh:mm.ss:ms. If we assume d = 30, then we could encode the timestamp with just 32 bits. Assuming that there could exist URLs arriving at the same timestamp (ms), we would add more 32 bits of the URL hash value to the ShortURL value:

The ShortURL would be composed of 32 bits timestamp + 32 bits hash value. With 32 bits hash value, and maintaining the probability of collision to 1 in a million, we would be limited to a maximum of 93 hash values per ms, meaning a capacity of processing requests of 93K requests per second. With this data the maximum number of URLs to hold would be 93*1000*60*60*24*30=241.056.000.000 if we persist URLs for a maximum of 30 days. **We can conclude then that not deduplicating URLs enable us to serve way more URLs in the system, using ShortURLs with the same size.**

*Note: the solution implemented used deduplication because when I asked by email which of the two approaches were better to implement, deduplication was indicated e as the preferred one.*

