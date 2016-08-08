var compensacionServices = angular.module('compensacionServices', ['ngResource']);

compensacionServices.factory('Compensacion', ['$resource',
  function ($resource) {
      return $resource('api/compensaciones/:id', { id: "@id" }, {
          detalles: { method: 'GET', url: 'api/compensaciones/:id/Detalles', params: { id: '@id' }, isArray: true }
      });
  }]);