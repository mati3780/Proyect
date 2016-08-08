var depositoServices = angular.module('depositoServices', ['ngResource']);

depositoServices.factory('Deposito', ['$resource',
  function ($resource) {
      return $resource('api/Depositos/:id', { id: "@id" }, {
          query: { method: 'GET', url: 'api/Depositos?start=:start&length=:length', params: { start: '@start', length: '@length' } },
          update: { method: 'PUT' },
          search: { method: 'GET', url: 'api/Depositos/search/:nombre', params: { nombre: '@nombre' }, isArray: true }
      });
  }]);