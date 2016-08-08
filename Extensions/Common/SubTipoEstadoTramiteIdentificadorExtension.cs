using System;
using System.Linq;
using PROYECT.Dominio.Attributes;
using PROYECT.WebAPI.Enums;

namespace PROYECT.WebAPI.Extensions.Common
{
    public static class SubTipoEstadoTramiteIdentificadorExtension
    {
        public static Type GetTramiteHandler(this SubTipoEstadoTramiteOption value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var operationHandler = type.GetField(name).GetCustomAttributes(false).OfType<TramiteHandlerAttribute>().SingleOrDefault();
            return operationHandler?.Handler;
        }
    }
}
