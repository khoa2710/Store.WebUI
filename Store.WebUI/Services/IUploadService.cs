namespace Store.WebUI.Services
{
    public interface IUploadService
    {
        string StandardizeFileName(string oldFileName);
        bool ExtensionWhiteList(IFormFile file);
        Task<string> SaveFileAsync(IFormFile file);
        bool SizeRestriction(IFormFile file);
    }
}
