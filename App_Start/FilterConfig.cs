using System.Web.Http;
using Helper.ExceptionLogger.Attribute;
using SimpleInjector;
using PROYECT.Helpers.Filters;
using PROYECT.Helpers.Attributes;

namespace PROYECT.WebAPI
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(HttpConfiguration config, Container container)
		{
			config.Filters.Add(new Helper.Web.WebApi.Helpers.Attributes.AuthorizeAttribute());
			config.Filters.Add(new ExceptionFilter());
			config.Filters.Add(new AuthenticationFilterDispatcher(container.GetAllInstances));
			config.Filters.Add(new ValidateModelAttribute());
		}
	}
}
