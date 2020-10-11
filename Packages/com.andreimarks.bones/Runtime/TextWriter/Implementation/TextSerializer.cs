using System;
using System.IO;
using UnityEngine;

namespace Bones.TextWriter
{
    public class TextSerializer : ITextWriterSerializer
    {
        private const string _FOLDER = "Journal";
        private const string _FILENAME = "journal.txt";

        private StreamWriter _streamWriter;

        private string _Directory
        {
            get
            {
                return Application.persistentDataPath +
                       Path.DirectorySeparatorChar +
                       _FOLDER;
            }
        }

        private string _FullPath
        {
            get
            {
                if (!Directory.Exists(_Directory))
                {
                    Directory.CreateDirectory(_Directory);
                }

                return Application.persistentDataPath +
                       Path.DirectorySeparatorChar +
                       _FOLDER +
                       Path.DirectorySeparatorChar +
                       _FILENAME;
            }
        }

        public TextSerializer()
        {
            _streamWriter = new StreamWriter(_FullPath, append: true);
            _streamWriter.Write(DateTime.Now);
        }

        public void UpdateWithText(string text)
        {
            _streamWriter.Write(text);
            _streamWriter.Flush();
        }

        public void DeleteText(int charactersToDelete)
        {

        }
    }
}