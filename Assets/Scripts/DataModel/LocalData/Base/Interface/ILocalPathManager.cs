namespace Gululu
{
    public interface ILocalPathManager
    {
         string GetStreamingFilePath(string filename);

         string GetPersistentFilePath(string filename);
    }
}