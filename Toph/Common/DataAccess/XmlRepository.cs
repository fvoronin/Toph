using System;
using System.IO;

namespace Toph.Common.DataAccess
{
    public class XmlRepository : FileRepository
    {
        protected override void Write<T>(string path, T entity)
        {
            File.WriteAllBytes(path, entity.SerializeXml());
        }

        protected override T Read<T>(string path)
        {
            return File.ReadAllBytes(path).DeserializeXml<T>();
        }
    }
}