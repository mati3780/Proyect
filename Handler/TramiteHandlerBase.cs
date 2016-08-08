using System;
using System.Collections.Generic;
using System.Net.Http;
using PROYECT.Dominio.Contracts.Dtos;
using PROYECT.Dominio.Contracts.Handlers;
using PROYECT.Dominio.Enums;
using PROYECT.Dominio.Extensions;
using PROYECT.Dominio.Model;
using PROYECT.Repositorios.Contracts;

namespace PROYECT.WebAPI.Handler
{
    public abstract class TramiteHandlerBase : ITramiteHelperBase
    {
        private readonly ISubTipoEstadoTramiteJurisdiccionRepositorio _subTipoEstadoTramiteJurisdiccionRepositorio;

        protected TramiteHandlerBase(ISubTipoEstadoTramiteJurisdiccionRepositorio subTipoEstadoTramiteJurisdiccionRepositorio)
        {
            _subTipoEstadoTramiteJurisdiccionRepositorio = subTipoEstadoTramiteJurisdiccionRepositorio;
        }

        public virtual void Set(Tramite tramite, ITramiteActionDto dto, Byte[] pdfFile)
        {
            Set(tramite, dto);
        }

        public virtual void SendEmail(Tramite tramite)
        { }

        public abstract Boolean Valid(ITramiteActionDto dto, Object modelState);
        public abstract IList<SubTipoEstadoTramiteIdentificador> GetSubTipoEstadoIdentificador(Tramite tramite);

        private void Set(Tramite tramite, ITramiteActionDto dto)
        {
            foreach (var identificador in GetSubTipoEstadoIdentificador(tramite))
            {
                var subEstadoId = _subTipoEstadoTramiteJurisdiccionRepositorio.GetIdPorIdetificador(identificador);
                tramite.AddEstadoTramite(new TramiteEstado { SubTipoId = subEstadoId, Observacion = dto.Observacion });
            }
        }
    }
}
