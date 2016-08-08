var cuentabancariaServices = angular.module('contribucionsinarepiServices', ['ngResource']);

cuentabancariaServices.factory('ContribucionProyect', ['$resource',
  function ($resource) {
      return $resource('api/contribucionsinarepi/:id', { id: "@id" }, {
  		query: { method: 'GET', url: 'api/contribucionsinarepi?start=:start&length=:length', params: { start: '@start', length: '@length' } },
		update: { method: 'PUT' },
		search: { method: 'GET', url: 'api/contribucionsinarepi/search/:nombre', params: { nombre: '@nombre' }, isArray: true },
		obtenerFechaDesde: { method: 'GET', url: 'api/ContribucionProyect/ObtenerFechaDesde', isArray: false }
  	});
  }]);