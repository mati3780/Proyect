var depositoPendienteServices = angular.module('depositoPendienteServices', ['ngResource']);

depositoPendienteServices.factory('DepositoPendiente', ['$resource',
  function ($resource) {
      return $resource('api/DepositoPendientes/:id', { id: "@id" }, {
          query: { method: 'GET', url: 'api/DepositoPendientes?start=:start&length=:length', params: { start: '@start', length: '@length' } },
		update: { method: 'PUT' },
		search: { method: 'GET', url: 'api/DepositoPendientes/search/:nombre', params: { nombre: '@nombre' }, isArray: true }
  	});
  }]);