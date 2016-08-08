using System;
using System.Collections.Generic;
using System.Linq;

namespace PROYECT.WebAPI.DTOs
{
    public class InmuebleDto
    {
        public InmuebleDto()
        {
            TomosFolios = new List<TomoFolioDto>();
            PersonasFisicas = new List<PersonaDto>();
            PersonasJuridicas = new List<PersonaDto>();
        }

        public Int64 Id { get; set; }
        public String UbicacionInmueble { get; set; }
        public String Matricula { get; set; }
        public String Zona { get; set; }
        public String UnidadComplementaria { get; set; }
        public String Lote { get; set; }
        public String Manzana { get; set; }
        public String Legajo { get; set; }
        public String NomenclaturaCatastralCircunscripcion { get; set; }
        public String NomenclaturaCatastralSeccion { get; set; }
        public String NomenclaturaCatastralManzana { get; set; }
        public String NomenclaturaCatastralParcela { get; set; }
        public Decimal? Superficie { get; set; }
        public Int64? UnidadMedidaId { get; set; }
        public String UnidadMedidaDescripcion { get; set; }
        public IList<TomoFolioDto> TomosFolios { get; set; }
        public IList<PersonaDto> PersonasFisicas { get; set; }
        public IList<PersonaDto> PersonasJuridicas { get; set; }
        public IList<PersonaDto> Personas {
            get
            {
                return PersonasFisicas.Union(PersonasJuridicas).ToList(); 
            } }
    }
}
