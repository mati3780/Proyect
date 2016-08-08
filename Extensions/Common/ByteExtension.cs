using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PROYECT.WebAPI.Extensions.Common
{
    public static class ByteExtension
    {
        public static HttpResponseMessage ToPdfResponseMessage(this byte[] value, String fileName)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(value) };
            response.Content.Headers.ContentLength = value.Length;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline") { FileName = $"{fileName}.pdf"};

            return response;
        }
    }
}