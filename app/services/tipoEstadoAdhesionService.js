var tipoEstadoAdhesionServices = angular.module('tipoEstadoAdhesionServices', ['ngResource']);

tipoEstadoAdhesionServices.factory('TipoEstadoAdhesion', ['$resource',
  function ($resource) {
  	return $resource('api/TipoEstadoAdhesiones/:id', { id: "@id" }, {
  	});
  }]);