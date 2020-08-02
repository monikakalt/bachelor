using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using museum.Application.main.graduates;
using museum.Application.main.graduates.dto;
using System;

namespace museumApi.Controllers.graduate
{

    [Authorize]
    [Route("/api/[controller]")]
    public class GraduatesController : Controller
    {
        private readonly IGraduatesService _service;
        public GraduatesController(IGraduatesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all Graduates
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var s = _service.GetAllGraduates();
            return Ok(s);
        }

        /// <summary>
        /// Get Graduates by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetGraduateById(int id)
        {
            try
            {
                var sub = _service.GetGraduateById(id);
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
        /// Add new Graduates
        /// </summary>
        /// <param name="Graduates"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] GraduatesDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = _service.PostGraduate(request);
                return Created("Graduates/" + result.Id, result);

            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }

        /// <summary>
        /// Edit Teacher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Teacher"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] GraduatesDTO request)
        {
            try
            {
                _service.UpdateGraduate(id, request);
                var result = _service.GetGraduateById(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Delete Graduates
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteGraduate(int id)
        {
            try
            {
                var sub = _service.GetGraduateById(id);
                _service.DeleteGraduate(id);
                return NoContent();
            }
            catch (Exception err)
            {
                return BadRequest(err);
            }
        }
    }
}