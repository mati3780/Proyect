var cuentabancariaServices = angular.module('cuentabancariaServices', ['ngResource']);

cuentabancariaServices.factory('CuentaBancaria', ['$resource',
  function ($resource) {
  	return $resource('api/cuentabancarias/:id', { id: "@id" }, {
  		query: { method: 'GET', url: 'api/cuentabancarias?start=:start&length=:length', params: { start: '@start', length: '@length' } },
		update: { method: 'PUT' },
		search: { method: 'GET', url: 'api/cuentabancarias/search/:nombre', params: { nombre: '@nombre' }, isArray: true }
  	});
  }]);