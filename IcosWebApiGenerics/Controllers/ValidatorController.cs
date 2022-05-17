using IcosWebApiGenerics.Models;
using IcosWebApiGenerics.Models.BADM;
using IcosWebApiGenerics.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
//using System.Web.Mvc;

namespace IcosWebApiGenerics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidatorController<T> : Controller where T:BaseClass
    {
        private readonly IValidatorService<T> _service;
        private readonly ISaveDataService<T> _saveData;
        private readonly IMapper _mapper;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("getweather")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public ValidatorController(IValidatorService<T> service, ISaveDataService<T> saveData, IMapper mapper)
        {
            _service = service;
            _saveData = saveData;
            _mapper = mapper;
        }
        
        // POST: ValidatorController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        // public async Task<ActionResult> Post([FromBody] T model)
        public async Task<JsonResult> Post([FromBody] T model)
           // JsonResult<WebAgent>
        {
            Response response =  await _service.Validate(model);
            if (response.Code != 0)
            {
                return Json(response);
                //return Ok(response);
            }
            bool result = await _saveData.SaveItemAsync(model, model.InsertUserId, model.SiteId);
            if (!result)
            {
                response.Code = 500;
                //response.Messages = "An error occurred in saving data. Please contact info@icos-etc.eu";
            }
            else
            {
                //....????....
                //response.Message = "Data succesfully saved in db";
            }
            return Json(response);
            //return Ok(response);
            //int res = _service.ValidateModel(model);

        }

        [HttpPost("postvg")]
        public async Task<ActionResult> Post([FromBody] VarForGroup vg)
        {
            BaseClass bb = _mapper.MapToObject(vg);
            T baseObj = _mapper.MapToObject(vg) as T;
            Response response = await _service.Validate(bb as T);
            /*Response response = _service.Validate(model);
            if (response.Code != 0)
            {
                return Ok(response);
            }
            bool result = await _saveData.SaveItemAsync(model, model.InsertUserId, model.SiteId);
            if (!result)
            {
                response.Code = 100;
                response.Message = "An error occurred in saving data. Please contact info@icos-etc.eu";
            }
            else
            {
                //....????....
                response.Message = "Data succesfully saved in db";
            }*/
            return Ok();
        }


    }
}
