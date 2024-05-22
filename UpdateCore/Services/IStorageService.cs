namespace UpdateCore.Services
{
    public interface IStorageService: IDisposable
    {
        void Upload(Stream data, string pathToSave);
        Stream Download(string pathToFile);
    }
}
