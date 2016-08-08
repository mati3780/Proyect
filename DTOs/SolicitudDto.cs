using System;
using System.Collections.Generic;
using PROYECT.Dominio.Enums;
using PROYECT.Helpers.Catpcha;
using PROYECT.WebAPI.DTOs.Contracts;

namespace PROYECT.WebAPI.DTOs
{
    public class SolicitudDto : IRectificacionDto, ICaptcha
    {
        public SolicitudDto()
        {
            Tramites = new List<TramiteDetalleDto>();
        }

        public bool EsRectificacionSolicitante { get; set; }
        public bool EsRectificacionPersona { get; set; }
        public bool EsRectificacionInmueble { get; set; }

        public Int64 Id { get; set; }
        public String Numero { get; set; }
        public String MotivoConsulta { get; set; }
        public String DatosAdicionales { get; set; }
        public String Observacion { get; set; }
        public String ObservacionEntidad { get; set; }
        public String ObservacionSolicitante { get; set; }
        public String FechaValidez { get; set; }
        public String FechaSolicitud { get; set; }
        public Int64 ServicioId { get; set; }
        public Int64 JurisdiccionRequirenteId { get; set; }
        public Int64? ReparticionSolicitanteId { get; set; }
        public Int64 TasaNacionalId { get; set; }
        public ServicioDto Servicio { get; set; }
        public SolicitanteDto Solicitante { get; set; }
        public PagoDto Pago { get; set; }
        public SeleccionableDto JurisdiccionRequirente { get; set; }
        public SeleccionableDto ReparticionSolicitante { get; set; }
        public SolicitudEntidadDto Entidad { get; set; }
        public IList<TramiteDetalleDto> Tramites { get; set; }
	    public string CaptchaKey { get; set; }
	    public string CaptchaText { get; set; }
    }
}
