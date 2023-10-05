namespace Utils
{
    public interface IFileReader
    {
        /// <summary>
        /// Reads data from a line of the file
        /// </summary>
        /// <returns>An array with the components of the line</returns>
        string[] NextLine();
        
        /// <summary>
        /// Checks whether the file still has lines to read
        /// </summary>
        /// <returns>
        /// <code>true</code> if there is at least one more line in the file,
        /// <code>false</code> otherwise
        /// </returns>
        bool HasNext();
    }
}