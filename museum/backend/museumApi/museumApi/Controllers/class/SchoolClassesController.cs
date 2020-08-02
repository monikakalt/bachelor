using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using museum.Application.main;
using museum.Application.main.classes;
using museum.Application.main.classes.dto;
using museum.Application.main.users;
using System;

namespace museumApi.Controllers.schoolClass
{
    [Route("/api/[controller]")]
    [Authorize]
    public class SchoolClassesController : Controller
    {
        protected readonly IClassesService _service;
        public SchoolClassesController(IClassesService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var classes = _service.GetAllClasses();
            return Ok(classes);

        }

        [HttpPost]
        public IActionResult Create([FromBody] ClassDTO classInfo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var cl = _service.PostClass(classInfo);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClassDTO cl)
        {
            try
            {
                _service.UpdateClass(id, cl);
                var result = _service.GetClassById(id);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClass(int id)
        {
            try
            {
 
                var classInfo = _service.GetClassById(id);

                if (classInfo == null)
                    return BadRequest();
                _service.DeleteClass(id);
                return new NoContentResult();
            }
            catch (Exception )
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get Class by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetClassById(int id)
        {
            try
            {
                var cl = _service.GetClassById(id);
                if (cl != null)
                    return new ObjectResult(cl);
                return NotFound();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Source);
            }
            return NotFound();
        }
    }
}