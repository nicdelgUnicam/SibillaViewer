namespace Utils
{
    public interface IFileReader
    {
        string[] NextLine();
        bool HasNext();
    }
}