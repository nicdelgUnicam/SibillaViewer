using System;
using System.IO;

namespace Utils
{
    public class CsvFileReader : IFileReader
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        private readonly StringReader _reader;
#else
        private readonly StreamReader _reader;
#endif
        
        
        /// <summary>
        /// Creates a new reader with the given file
        /// </summary>
        /// <param name="file">The file path, or the entire contents on WebGL</param>
        /// 
        /// <exception cref="ArgumentException">If the given path or file is empty</exception>
        public CsvFileReader(string file)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            _reader = new StringReader(file);
#else
            _reader = new StreamReader(file);
#endif
            if (!HasNext())
                throw new ArgumentException("File is empty");
        }

        ~CsvFileReader()
        {
            _reader.Close();
        }


        public string[] NextLine()
        {
            return _reader.ReadLine()!.Split(',');
        }

        public bool HasNext()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return _reader.Peek() != -1;
#else
            return !_reader.EndOfStream;
#endif
        }
    }
}