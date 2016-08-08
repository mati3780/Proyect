var plazoServices = angular.module('plazoServices', ['ngResource']);

plazoServices.factory('Plazo', ['$resource',
  function ($resource) {
      return $resource('api/JurisdiccionServicioPlazos/:id', { id: "@id" }, {
      	query: { method: 'GET', url: 'api/JurisdiccionServicioPlazos?start=:start&length=:length', params: { start: '@start', length: '@length' } },
      	plazosNoConfigurados: { method: 'GET', url: 'api/JurisdiccionServicioPlazos/:servicioId/PlazosNoConfigurados', params: { servicioId: '@servicioId' }, isArray: true },
      	plazosDisponibles: { method: 'GET', url: 'api/JurisdiccionServicioPlazos/:servicioId/PlazosDisponibles', params: { servicioId: '@servicioId' }, isArray: true },
      	disponible: { method: 'GET', url: 'api/JurisdiccionServicioPlazos/:servicioId/Disponible/:plazoId', params: { servicioId: '@servicioId', plazoId: '@plazoId' } },
		update: { method: 'PUT' },
		search: { method: 'GET', url: 'api/JurisdiccionServicioPlazos/search/:nombre', params: { nombre: '@nombre' }, isArray: true }
  	});
  }]);