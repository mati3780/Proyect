using System.IO;
using System.Web;

namespace PROYECT.WebAPI.Extensions.Common
{
    static class HttpPostedFileExtension
    {
        public static byte[] ToBytes(this HttpPostedFile value)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(value.InputStream))
            {
                fileData = binaryReader.ReadBytes(value.ContentLength);
            }

            return fileData;
        }
    }
}
