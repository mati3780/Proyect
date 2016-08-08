using System;

namespace PROYECT.WebAPI.DTOs
{
    public class InmuebleRectificacionDto
    {
        public String UbicacionInmueble { get; set; }
        public String Matricula { get; set; }
        public String Zona { get; set; }
        public String UnidadComplementaria { get; set; }
        public String Lote { get; set; }
        public String Manzana { get; set; }
        public String Legajo { get; set; }
        public String NomenclaturaCatrastalCiscunscripcion { get; set; }
        public String NomeclaturaCatastralSeccion { get; set; }
        public String NomenclaturaCatastralManzana { get; set; }
        public String NomenclaturaCatastralParcela { get; set; }
        public Decimal? Superficie { get; set; }
        public String UnidadMedida { get; set; }
        public Int64 InmuebleId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
