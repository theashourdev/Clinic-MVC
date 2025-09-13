namespace Cilinc_System.Services.IServices
{
    public interface IImageManager
    {
        string SaveImage(IFormFile file, string folderName);
        bool DeleteImage(string imagePath);
    }

}
