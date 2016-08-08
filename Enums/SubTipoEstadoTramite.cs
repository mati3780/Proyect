using PROYECT.Dominio.Attributes;
using PROYECT.WebAPI.Handler;

namespace PROYECT.WebAPI.Enums
{
    public enum SubTipoEstadoTramiteOption
    {
        [TramiteHandler(typeof(TramiteDocumentacionEnTramiteHandler))]
        DocumentacionEnTramite,

        [TramiteHandler(typeof(TramiteObservacionHandler))]
        Observado,

        [TramiteHandler(typeof(TramiteInformePdfHandler))]
        SeRegistroPdf,

        [TramiteHandler(typeof(TramiteVerificacionPdfHandler))]
        Verificacion,

        [TramiteHandler(typeof(TramiteAnulacionPdfHandler))]
        Anulacion,

        [TramiteHandler(typeof(TramiteEntregaAceptacionHandler))]
        EntregaAceptacion,

        [TramiteHandler(typeof(TramiteEntregaRechazoNoCorrespondeHandler))]
        EntregaRechazoNoCorresponde,

        [TramiteHandler(typeof(TramiteEntregaRechazoVisualizacionHandler))]
        EntregaRechazoVisualizacion,

        [TramiteHandler(typeof(TramiteValidacionRechazoNoCorrespondeHandler))]
        ValidacionRechazoNoCorresponde,

        [TramiteHandler(typeof(TramiteValidacionRechazoVisualizacionHandler))]
        ValidacionRechazoVisualizacion
    }
}