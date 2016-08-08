using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Results;
using Helper.Repositorios.Helpers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Model;
using PROYECT.Helpers;
using PROYECT.Helpers.Email;
using PROYECT.WebAPI.Extensions.Common;

namespace PROYECT.WebAPI.Controllers
{
    public class ReportController : ApiController
    {
        protected static async Task<IHttpActionResult> GetReportLiquidacionDeposito(long liquidacionId, long jurisdiccionId)
        {
            var reporte = await ReportHelper.GetReport(Reporte.Liquidacion.GetDescription(),
                                                        new List<KeyValuePair<string, string>>
                                                        {
                                                            new KeyValuePair<string, string>("id", liquidacionId.ToString()),
                                                            new KeyValuePair<string, string>("jurisdiccionId", jurisdiccionId.ToString())
                                                        });

            return new ResponseMessageResult(reporte.ToPdfResponseMessage("Liquidacion"));
        }

        protected static async Task<IHttpActionResult> GetReportToDownload(Solicitud solicitud, bool sendMail, DateTime? fechaValidez = null)
        {
            var reporte = await ReportHelper.GetReport(Reporte.Solicitud.GetDescription(),
                new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("id", solicitud.Id.ToString()) });

            if (sendMail)
            {
                var html = File.ReadAllText(HostingEnvironment.MapPath("~/Emails/EmailNuevaSolicitud.html"))
                                                                    .Replace("[[NroSolicitud]]", solicitud.Numero)
                                                                    .Replace("[[NombreTramite]]", solicitud.Servicio.Descripcion)
                                                                    .Replace("[[NombreJurisdiccionRequirente]]", $"{solicitud.JurisdiccionRequirente.Descripcion}, {solicitud.JurisdiccionRequirente.Direccion}, {solicitud.JurisdiccionRequirente.Localidad.Descripcion} - {solicitud.JurisdiccionRequirente.Provincia.Descripcion}")
                                                                    .Replace("[[FechaValidez]]", fechaValidez?.ToString("d") ?? string.Empty);

                MailSender.SendEmail(new[] { solicitud.Solicitante.Email1, solicitud.Solicitante.Email2 },
                                    $"PROYECT - Aviso de generación de solicitud {solicitud.Numero}",
                                    html,
                                    new[] { new Attachment($"Solicitud {solicitud.Numero}.pdf", reporte, "application/pdf") });
            }

            return new ResponseMessageResult(reporte.ToPdfResponseMessage("Solicitud"));
        }

    }
}