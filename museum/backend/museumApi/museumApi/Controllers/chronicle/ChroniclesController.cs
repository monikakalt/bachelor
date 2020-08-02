using Firebase.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using museum.Application.main;
using museum.Application.main.chronicles;
using museum.Application.main.chronicles.dto;
using System;
using System.Globalization;
using System.IO;

namespace museumApi.Controllers.chronicle
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChroniclesController : Controller
    {
        private readonly IChroniclesService _service;

        public ChroniclesController(IChroniclesService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("upload")]
        public IActionResult Upload()
        {
            try
            {
                var httpRequest = HttpContext.Request.Form;
                string title = Request.Form["Title"].ToString();
                var dateString = Request.Form["Date"].ToString();
                var usrId = Request.Form["userId"].ToString();

                string format = "yyyy-MMM-dd";
                DateTime date = Convert.ToDateTime(dateString);
                bool validFormat = DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                var files = httpRequest.Files;

                var folderName = Path.Combine("D:\\KTU\\4 KURSAS 2 SEMESTRAS\bakalauras\\jjgmuziejus\\museum\\museum-frontend\\src\\assets");

                var chronicle = new ChronicleDTO()
                {
                    Title = title,
                    Date = Convert.ToDateTime(dateString),
                    FolderUrl = Path.GetFullPath(folderName),
                    FkUser = int.Parse(usrId)
                };

                try
                {
                    var ch = _service.PostChronicle(chronicle);
                    _service.ProcessFiles(files, folderName, ch.Id);
                    
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }
 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Get chronicle by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetChronicleById(int id)
        {
            try
            {
                var sub = _service.GetChronicleById(id);
                if (sub != null)
                    return new ObjectResult(sub);
                return NotFound();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Source);
            }
            return NotFound();

        }

        /// <summary>
        /// Gets all chronicles
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var s = _service.GetAllChronicles();
            return Ok(s);
        }

        [HttpGet("recent")]
        [AllowAnonymous]
        public IActionResult GetRecentChronicles()
        {
            var s = _service.GetRecentChronicle();
            return Ok(s);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id)
        {
            try
            {
                var httpRequest = HttpContext.Request.Form;
                string title = Request.Form["Title"].ToString();
                var dateString = Request.Form["Date"].ToString();
                var usrId = Request.Form["userId"].ToString();
                var oldChronicle = _service.GetChronicleById(id);

                var folderName = Path.Combine("D:\\KTU\\4 KURSAS 2 SEMESTRAS\bakalauras\\jjgmuziejus\\museum\\museum-frontend\\src\\assets");

                var request = new ChronicleDTO()
                {
                    Id = oldChronicle.Id,
                    Title = title,
                    Date = Convert.ToDateTime(dateString),
                    FolderUrl = Path.GetFullPath(folderName),
                    FkUser = int.Parse(usrId)
                };

                var files = httpRequest.Files;

                try
                {
                    _service.UpdateChronicle(id, request);
                    _service.DeletePhotos(request.Id);
                    _service.ProcessFiles(files, folderName, request.Id);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteChronicle(int id)
        {
            try
            {
                _service.DeleteChronicle(id);
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
