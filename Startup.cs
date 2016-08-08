using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.OAuth;
using Helper.Repositorios.Contracts.Repositories;
using Newtonsoft.Json.Linq;
using PROYECT.IoC;
using Owin;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Integration.WebApi;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Helpers;
using PROYECT.Dominio.Model;
using PROYECT.WebAPI.Extensions;
using PROYECT.Helpers;

[assembly: OwinStartup(typeof(PROYECT.WebAPI.Startup))]
namespace PROYECT.WebAPI
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();

			log4net.Config.XmlConfigurator.Configure();

			var container = IoCModule.SimpleInjectorWebApiContainer();
			config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = config.DependencyResolver;

            ConfigureOAuth(app, container);

			WebApiConfig.Register(config);
			FilterConfig.RegisterGlobalFilters(config, container);
			
			app.UseWebApi(config);
		}

		public void ConfigureOAuth(IAppBuilder app, Container container)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

			var oAuthServerOptions = new OAuthAuthorizationServerOptions
			{
				AllowInsecureHttp = true,
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
				Provider = new ADOAuthProvider("angularapp", container)
			};

			// Token Generation
			app.UseOAuthAuthorizationServer(oAuthServerOptions);
		}
	}

	public class ADOAuthProvider : OAuthAuthorizationServerProvider
	{
		private readonly string _publicClientId;

		private readonly Container _container;
		public ADOAuthProvider(string publicClientId, Container container)
		{
			if (publicClientId == null)
			{
				throw new ArgumentNullException("publicClientId");
			}

			_publicClientId = publicClientId;
			_container = container;
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			ClaimsIdentity oAuthIdentity;
			using (_container.BeginExecutionContextScope())
			{
				var jurisdiccionRepositorio = _container.GetInstance<IRepositorio<Jurisdiccion>>();
				var valid = AuthenticationHelper.ValidateUserCredentials(context.UserName, context.Password, OAuthDefaults.AuthenticationType, out oAuthIdentity);

				if (!valid)
				{
					context.SetError("invalid_grant", Resources.Validacion_UsuarioIncorrecto);
					return;
				}

				if (!oAuthIdentity.HasClaim(ClaimTypes.Role, AppConfigHelper.ADAppGroup))
				{
					context.SetError("invalid_grant", Resources.Validacion_NoHabilitadoASistema);
					return;
				}

				var groups = oAuthIdentity.Claims.Select(x => x.Value).ToList();
				oAuthIdentity.AddClaims(jurisdiccionRepositorio.GetAll()
																.Where(j => groups.Contains(j.GrupoAd)).ToList()
																.Select(j => new Claim("jurisdiccion", j.Id.ToString())));
				if (oAuthIdentity.Claims.Count(c => c.Type == "jurisdiccion") != 1 && !oAuthIdentity.IsAdmin())
				{
					context.SetError("invalid_grant", Resources.Validacion_UsuarioMasDeUnaJurisdiccionAsignada);
					return;
				}

				if (!oAuthIdentity.IsAdmin())
				{
					var jurisdiccion = jurisdiccionRepositorio.Get(oAuthIdentity.GetJurisdiccionId());
					var estadosInhibidos = new[]
										   {
											   TipoEstadoAdhesionIdentificador.Bloqueado,
											   TipoEstadoAdhesionIdentificador.EnTramite,
											   TipoEstadoAdhesionIdentificador.NoAdherido
										   };
					if (estadosInhibidos.Contains(jurisdiccion.TipoEstadoAdhesion.Identificador))
					{
						context.SetError("invalid_grant", Resources.Validacion_JurisdiccionInhabilitada);
						return;
					}
				}
			}
			var properties = CreateProperties(oAuthIdentity);
			var ticket = new AuthenticationTicket(oAuthIdentity, properties);
			context.Validated(ticket);
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (var property in context.Properties.Dictionary)
				context.AdditionalResponseParameters.Add(property.Key, property.Value);

			return Task.FromResult<object>(null);
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			// Resource owner password credentials does not provide a client ID.
			if (context.ClientId == null)
				context.Validated();

			return Task.FromResult<object>(null);
		}

		public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
		{
			if (context.ClientId == _publicClientId)
			{
				var expectedRootUri = new Uri(context.Request.Uri, "/");

				if (expectedRootUri.AbsoluteUri == context.RedirectUri)
					context.Validated();
			}

			return Task.FromResult<object>(null);
		}

		public static AuthenticationProperties CreateProperties(ClaimsIdentity identity)
		{
			IDictionary<string, string> data = new Dictionary<string, string>
																			{
																				{ "userName", identity.Name },
																				{ "givenName", identity.FindFirst(ClaimTypes.GivenName).Value },
																				{ "surname", identity.FindFirst(ClaimTypes.Surname).Value },
																				{ "aliases", string.Join(",", identity.FindAll("alias").Select(c => c.Value)) },
																				{ "jurisdiccion", identity.FindFirst("jurisdiccion") != null
																									? identity.FindFirst("jurisdiccion").Value
																									: string.Empty }
																			};
			return new AuthenticationProperties(data);
		}
	}
}
