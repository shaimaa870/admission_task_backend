using admission_task.Dtos;
using admission_task.Repos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace admission_task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelRepository _modelRepository;
        public ModelController(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository; 

        }
        //y[Authorize]
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> GetModels()
        {
            var list = await _modelRepository.GetModelsAsync();
            return Ok(list);

        }
        [HttpGet]
        [Route("model/{id}")]
        public async Task<IActionResult> GetModel([FromRoute] string id)
        {
            var model = await _modelRepository.GetModelAsync(id);
            return Ok(model);

        }
        [HttpPost]
        [Route("add-model")]
        public async Task<IActionResult> AddModel([FromBody] CreatedModelDto model)
        {
            var res = await _modelRepository.CreateModelAsync(model);
            return Ok(res);

        }
        [HttpPut]
        [Route("update-model")]
        public async Task<IActionResult> EditModel(CreatedModelDto model)
        {
            var res = await _modelRepository.UpdateModelAsync(model);
            return Ok(res);

        }
        [HttpDelete]
        [Route("delete-model/{id}")]
        public async Task<IActionResult> RemoveModel([FromRoute] string id)
        {
            var res = await _modelRepository.DeleteModelAsync(id);
            return Ok(res);

        }
    }
}
