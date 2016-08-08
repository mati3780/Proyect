using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PROYECT.Helpers.Catpcha;

namespace PROYECT.WebAPI.Controllers
{
	[RoutePrefix("api/Captcha")]
    public class CaptchaController : ApiController
    {
		[Route("")]
		[AllowAnonymous]
		public IHttpActionResult Get()
		{
			return Ok(CaptchaHelper.GenerateCaptcha());
		}
    }
}
