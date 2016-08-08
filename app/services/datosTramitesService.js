var datoTramiteServices = angular.module('datoTramiteServices', ['ngResource']);

datoTramiteServices.factory('DatoTramite', ['$resource',
  function ($resource) {
  	return $resource('api/JurisdiccionServicioDatos/:id', { id: "@id" }, {
		update: { method: 'PUT' }
  	});
  }]);