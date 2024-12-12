using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.ExternalApi
{
    public static class CloudinaryService 
    {
        private static Cloudinary _cloudinary;
        
        public static void Initialize(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        
        public static string UploadProfilePicture(Guid id, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = id.ToString(),
                Overwrite = true,
                Transformation = new Transformation()
                    .Quality(50),
                Folder = "profile-picture"
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }

        public static string GetDefaultProfilePicture()
        {
            return "https://res.cloudinary.com/dwundfvss/image/upload/v1732030732/defaultprofilepicture_ofpohc.png";
        }

        public static async Task<List<string>> UploadPicturesAsync(Guid id, List<IFormFile> files)
        {
            var uploadTasks = files.Select(async file =>
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = id.ToString(),
                    Overwrite = true,
                    Folder = "picture-forum",
                    Transformation = new Transformation()
                        .Quality(50),
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                return uploadResult.SecureUrl.ToString();
            });
            var uploadResults = await Task.WhenAll(uploadTasks);
            return uploadResults.ToList();
        }

        public static bool DeletePicture(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result =  _cloudinary.Destroy(deletionParams);
            return result.Result == "ok";
        }
        public static string ExtractPublicId(string url)
        {
            Uri uri = new(url);
            string path = uri.AbsolutePath;

            path = path.Replace("/image/upload/", "").Trim('/');
            int extensionIndex = path.LastIndexOf('.');
            return extensionIndex > 0 ? path[..extensionIndex] : path;
        }

    }
}