var tramiteServices = angular.module('tramiteServices', ['ngResource']);

tramiteServices.factory('Tramite', ['$resource',
  function ($resource) {
      return $resource('api/Tramites/:id', { id: "@id" }, {
          query: { method: 'GET', url: 'api/Tramites?start=:start&length=:length', params: { requiriente: '@requiriente', start: '@start', length: '@length' } },
          update: { method: 'PUT' },
          search: { method: 'GET', url: 'api/Tramites/search/:nombre', params: { nombre: '@nombre' }, isArray: true }
      });
  }]);