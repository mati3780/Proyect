using System;
using System.Collections.Generic;

namespace PROYECT.WebAPI.DTOs
{
    public class TramiteDatoListDto
    {
        public TramiteDatoListDto()
        {
            PersonaFisicaDatos = new Dictionary<String, TramiteDatoDto>();
            PersonaJuridicaDatos = new Dictionary<String, TramiteDatoDto>();
            InmuebleDatos = new Dictionary<String, TramiteDatoDto>();
        }

        public Int64 ServicioId { get; set; }
		public String ServicioDescripcion { get; set; }
        public Boolean Inmueble { get; set; }
        public IDictionary<String, TramiteDatoDto> PersonaFisicaDatos { get; set; }
        public IDictionary<String, TramiteDatoDto> PersonaJuridicaDatos { get; set; }
        public IDictionary<String, TramiteDatoDto> InmuebleDatos { get; set; }
    }
}
