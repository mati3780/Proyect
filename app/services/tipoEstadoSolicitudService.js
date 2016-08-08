var tipoEstadoSolicitudServices = angular.module('tipoEstadoSolicitudServices', ['ngResource']);

tipoEstadoSolicitudServices.factory('TipoEstadoSolicitud', ['$resource',
  function ($resource) {
  	return $resource('api/SolicitudesEstados/:id', { id: "@id" }, {
  	});
  }]);