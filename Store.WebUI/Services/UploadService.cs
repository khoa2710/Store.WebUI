
namespace Store.WebUI.Services
{
    public class UploadService : IUploadService
    {
        public bool ExtensionWhiteList(IFormFile file)
        {
            var whiteList = new List<string>() {  ".jpg", ".jpeg", ".png", ".gif"};
            var extension = Path.GetExtension(file.FileName);
            if (!whiteList.Contains(extension.Trim().ToLower()))
            {
                return false;
            }
            return true;
        }
       public string StandardizeFileName(string oldFileName)
       {
            string uniqueId = Guid.NewGuid().ToString();
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");

            string fileExtention = Path.GetExtension(oldFileName).Trim().ToLower();

            return $"{uniqueId}_{time}{fileExtention}";

       }
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var standardizedName = StandardizeFileName(file.FileName);
            var pathToSave = Path.Combine("wwwroot", "upload", standardizedName);
            try
            {
                await using (var fileStream = new FileStream(pathToSave, FileMode.Create))

                    await file.CopyToAsync(fileStream);

                return $"/upload/{standardizedName}";
            }
            catch (Exception ex)
            {
                return $"Cannot save file to {pathToSave} due to {ex.Message}";
            }
        }
        public bool SizeRestriction(IFormFile file)
        {
            int fileMaxSize = 5 * 1024 * 1024; 

            if (file.Length > fileMaxSize)
            {
                return false;
            }
            return true;
        }
    }
}
