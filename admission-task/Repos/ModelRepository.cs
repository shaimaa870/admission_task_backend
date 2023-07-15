using admission_task.Dtos;
using admission_task.Exceptions;
using admission_task.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace admission_task.Repos.Interfaces
{
    public class ModelRepository : IModelRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHostEnvironment _appEnvironment;

        public ModelRepository(AppDbContext appDbContext,IWebHostEnvironment webHostEnvironment, IHostEnvironment appEnvironment)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _appEnvironment = appEnvironment;   
        }
        public async Task<string?> CreateModelAsync(CreatedModelDto model)
        {
          
                var newModel = new ModelCollection();
                newModel.Name = model.Name;
                newModel.Description = model.Description;
                newModel.Model = model.Model;
            await _appDbContext.ModelCollections.AddAsync(newModel);
            string photo = null;
            if (model.img != null)
            {
                model.img.Name = newModel.ID;
                photo = UploadImg(model.img);
            }
            newModel.Photo = photo;
            await  _appDbContext.SaveChangesAsync();
          
            return newModel.ID;
        }

        public async Task<string?> DeleteModelAsync(string id)
        {
            var model =await _appDbContext.ModelCollections.FirstOrDefaultAsync(m => m.ID == id);
          if(model==null) throw new NotFoundException($"model not found"); 
           _appDbContext.Remove(model);
           await _appDbContext.SaveChangesAsync();

            return model.ID;
        }
        public async Task<string?> UpdateModelAsync(CreatedModelDto model)
        {
            var updatedModel = await _appDbContext.ModelCollections.FirstOrDefaultAsync(m => m.ID == model.ID);
            if (updatedModel == null) throw new NotFoundException($"model not found");

            updatedModel.Name = model.Name;
            updatedModel.Model = model.Model;
            updatedModel.Description=updatedModel.Description;
            if (model.img != null) {
                model.img.Name = updatedModel.ID;
               updatedModel.Photo= UploadImg(model.img);
            }
            else
            {
                updatedModel.Photo= updatedModel.Photo??null;
            }
          await  _appDbContext.SaveChangesAsync();
            return updatedModel.ID;
        }

        public async Task<ModelDto> GetModelAsync(string id)
        {
            var gotModel=new ModelDto();    
            var model=await _appDbContext.ModelCollections.FirstOrDefaultAsync(m => m.ID == id);

            if(model==null) throw new NotFoundException($"model not found");
            gotModel.ID = model.ID;
            gotModel.Name = model.Name;
            gotModel.Description=model.Description; 
            gotModel.Photo = model.Photo;
            gotModel.Model = model.Model;

            return gotModel;
        }

        public async Task<List<ModelDto>> GetModelsAsync()
        {
            var models = await _appDbContext.ModelCollections.ToListAsync();
            var modelList = new List<ModelDto>();   
            foreach (var item in models)
            {
                var model = new ModelDto();
                model.ID = item.ID;
                model.Name = item.Name;
                model.Description = item.Description;
                model.Photo = item.Photo;
                model.Model = item.Model;
                modelList.Add(model);
            }
            return modelList;
        }

       

        private string UploadImg(AppFileDto file)
        {
            int startIndex = file.Base64.IndexOf("data:image/") + "data:image/".Length;
            int endIndex = file.Base64.IndexOf(";base64,", startIndex);
            var extensionAtBase64 =file.Base64.Substring(startIndex, endIndex - startIndex);
            var base64= file.Base64.Replace($"data:image/{extensionAtBase64};base64,", String.Empty);

            byte[] bytes = Convert.FromBase64String(base64);
            var fileName = $"{file.Name}.{file.Extension}";
               var filePath = Path.Combine($"{_webHostEnvironment.ContentRootPath}/wwwroot/images/{fileName}");
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            System.IO.File.WriteAllBytes(filePath, bytes);
            return fileName;
        }
    }
}

