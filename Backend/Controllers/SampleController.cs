using AspNetCore.MongoDB;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    // See https://github.com/nahidhasanswe/AspNetCore.MongoDB
    [Route("api/s")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IMongoOperation<SampleModel> _operation;

        public SampleController(IMongoOperation<SampleModel> operation)
        {
            _operation = operation;
        }

        [HttpPost("Save")]
        public async Task Save(SampleModel model)
        {
            await _operation.SaveAsync(model);
        }

        [HttpPost("Update")]
        public async Task Update(SampleModel model)
        {
            await _operation.UpdateAsync(model.Id, model);
        }

        [HttpGet]
        public async Task<IEnumerable<SampleModel>> GetAll()
        {
            return await _operation.GetAllAsync();
        }

        [HttpGet("Id")]
        public async Task<SampleModel> GetById(string Id)
        {
            return await _operation.GetByIdAsync(Id);
        }

        [HttpDelete]
        public async Task<DeleteResult> RemoveOne(string id)
        {
            return await _operation.RemoveByIdAsync(id);
        }

        [HttpDelete("all")]
        public async Task<DeleteResult> RemoveAll()
        {
            return await _operation.RemoveAllAsync();
        }

    }

    public class SampleModel : IMongoEntity
    {
        public string Name { get; set; }

        public string Address { get; set; }
    }
}