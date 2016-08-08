using PROYECT.Helpers.Catpcha;

namespace PROYECT.WebAPI.DTOs
{
    public class CaptchaDto : ICaptcha
    {
        public string CaptchaKey { get; set; }
        public string CaptchaText { get; set; }
    }
}
