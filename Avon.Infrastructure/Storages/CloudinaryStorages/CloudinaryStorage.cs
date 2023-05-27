using Avon.Application.Storages.CloudinaryStorages;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Avon.Infrastructure.Storages.CloudinaryStorages
{
    public class CloudinaryStorage : ICloudinaryStorages
    {
        Cloudinary _cloudinary;
        public CloudinaryStorage(IConfiguration configuration)
        {
            Account account = configuration.GetSection("CloudinarySettings").Get<Account>();
            _cloudinary = new Cloudinary(account);
        }
        public async Task<bool> DeleteAsync(string pathOrContainerName, string fileName)
        {
            var deletionParams = new DeletionParams(pathOrContainerName+fileName);
            var deletionResult = _cloudinary.DestroyAsync(deletionParams);
            return true;
        }
        public bool HasFile(string pathOrContainerName, string fileName)
        {
            var getResource = new GetResourceParams(pathOrContainerName+fileName)
            {
                ResourceType = ResourceType.Image
            };
            var info = _cloudinary.GetResource(getResource);

            if (info.Error == null)
                return true;
            else
                return false;

        }
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadRangeAsync(string pathOrContainerName, List<IFormFile> files)
        {
            var result = await _cloudinary.CreateFolderAsync(pathOrContainerName);

            List<(string fileName, string pathOrContainerName)> fileNameList = new();

            foreach (var file in files)
            {
                string fileNewName = UploadImageAction(pathOrContainerName,result.Path,file);
                fileNameList.Add(new(fileNewName, pathOrContainerName));
            }
            return fileNameList;
        }
        public async Task<(string fileName, string pathOrContainerName)> UploadAsync(string pathOrContainerName, IFormFile file)
        {
            var result = await _cloudinary.CreateFolderAsync(pathOrContainerName);
            
            string fileNewName = UploadImageAction(pathOrContainerName,result.Path,file);

            return (fileNewName, pathOrContainerName);
        }
        public new List<string> FileNames(string path, string ceoFriendlyName)
        {
            return _cloudinary.Search().Expression($"public_id:{path}/{ceoFriendlyName}*").Execute().Resources.Select(x => x.PublicId).ToList();
        }
        public string GetPublicId(string imageUrl)
        {
            int startIndex = imageUrl.LastIndexOf('/') + 1;
            int endIndex = imageUrl.LastIndexOf('.');
            int length = endIndex - startIndex;
            return imageUrl.Substring(startIndex, length);
        }
        public string UploadImageAction(string pathOrContainerName,string resultPath, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadResult = new ImageUploadResult();

            var listName = FileNames(pathOrContainerName,file.FileName);

            //todo change image name

            string fileNewName = file.FileName;
            var uploadParams = new ImageUploadParams()
            {

                File = new FileDescription(fileNewName, stream),
                Folder = resultPath,
                PublicId = fileNewName,

            };
            uploadResult = _cloudinary.Upload(uploadParams);
            Console.WriteLine(uploadResult.Url.ToString());
            return fileNewName;
        }


      
        public string GetUrl(string pathOrContainerName, string fileName)
        {
            var getResult = _cloudinary.GetResource(pathOrContainerName + fileName);
            return getResult.Url;
        }

        public (string fileName, string pathOrContainerName) Upload(string pathOrContainerName, IFormFile file)
        {
            throw new NotImplementedException();
        }


        #region FileName

        //private async Task<string> FileRenameAsync(string pathOrContainerName,string fileName, List<string> fileNames)
        //{
        //    foreach (var item in FileNameContains.Split(","))
        //    {
        //        if (fileName.Contains(item)) fileName.Replace(item, "-");
        //    }


        //    foreach (var item in fileNames)
        //    {

        //    }

        //    return "";
        //}

        //private string FileNameContains = "!,@,#,$,%,^,&,*,(,),>,<,?,',{,},|";

        #endregion

    }
}
