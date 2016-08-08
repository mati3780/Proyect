using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROYECT.WebAPI.Enums;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteBusquedaAdminEnteDto : BusquedaDto
    {
        public String CodigoBarra { get; set; }
        public Int64? TipoDocumento { get; set; }
        public Int64? NumeroDocumento { get; set; }
        public Int64? Jurisdiccion { get; set; }
        public Boolean? TipoJurisdiccion { get; set; }
        public Boolean? TipoPersona { get; set; }
        public String ApellidoDenominacion { get; set; }
        public Int64? Estado { get; set; }
        public Int64? SubEstado { get; set; }
        public Int64? EstadoSolicitud { get; set; }
    }
}
