var tipoTramiteServices = angular.module('servicioServices', ['ngResource']);

tipoTramiteServices.factory('Servicio', ['$resource',
  function ($resource) {
  	return $resource('api/servicios/:id', { id: "@id" }, {
  		noConfigurados: { method: 'GET', url: 'api/servicios/noconfigurados', isArray: true },
  		disponibles: { method: 'GET', url: 'api/servicios/disponibles', isArray: true },
  		condiciones: { method: 'GET', url: 'api/servicios/condiciones', isArray: true },
  		update: { method: 'PUT' }
  	});
  }]);
