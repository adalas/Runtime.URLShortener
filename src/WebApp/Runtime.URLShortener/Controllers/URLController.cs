using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Runtime.URLShortener.ApplicationCore.Exceptions;
using Runtime.URLShortener.ApplicationCore.Interfaces;
using Runtime.URLShortener.Web.Models;

namespace Runtime.URLShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class URLController : ControllerBase
    {

        private readonly ILogger<URLController> _logger;
        private readonly IURLService _service;

        public URLController(ILogger<URLController> logger,IURLService service)
        {
            _logger = logger;
            _service = service;
        }

        public class GetUrlModelInput{
            public string ShortURL {get; set;}
        }

        public class GetUrlModelOutput {
            public string URL {get; set;}
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{shortURL}")]
        public async Task<ActionResult> GetURLValue(string shortURL)
        {
            try {
            string extendedURL =  await _service.GetURLAsync(shortURL);
            if (extendedURL == null)
                return BadRequest($"ShortURL '{shortURL}' does not exist in the system.");
            return Ok(new GetUrlModelOutput(){URL=extendedURL});
            } catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        public class ShortenURLModelOutput {
            public string SUrl {get; set;}
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ShortenURL(ShortenURLModel input)
        {
            
            string shortURL = await _service.CreateURLAsync(input.URL);
            if (shortURL == null)
                return BadRequest($"It was not possible to create a new URLValue.");
            return Ok(new ShortenURLModelOutput() {SUrl=shortURL});
        }
    }
}
