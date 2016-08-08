var feriadoServices = angular.module('feriadoServices', ['ngResource']);

feriadoServices.factory('Feriado', ['$resource',
  function ($resource) {
  	return $resource('api/feriados/:id', { id: "@id" }, {
  		query: { method: 'GET', url: 'api/feriados?start=:start&length=:length', params: { start: '@start', length: '@length' } },
		update: { method: 'PUT' },
		search: { method: 'GET', url: 'api/feriados/search/:nombre', params: { nombre: '@nombre' }, isArray: true }
  	});
  }]);