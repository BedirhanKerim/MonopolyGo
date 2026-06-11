using System.IO;
using UnityEngine;

namespace MonopolyGo
{
    public class JsonSaveStorage : ISaveStorage
    {
        private readonly string m_Path;

        public JsonSaveStorage(string fileName)
        {
            m_Path = Path.Combine(Application.persistentDataPath, fileName);
        }

        public void Save(string json)
        {
            File.WriteAllText(m_Path, json);
        }

        public string Load()
        {
            return File.ReadAllText(m_Path);
        }

        public bool HasData()
        {
            return File.Exists(m_Path);
        }
    }
}
