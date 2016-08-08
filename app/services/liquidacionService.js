var liquidacionServices = angular.module('liquidacionServices', ['ngResource']);

liquidacionServices.factory('Liquidacion', ['$resource',
  function ($resource) {
      return $resource('api/liquidaciones/:id', { id: "@id" }, {
          update: { method: 'PUT' },
          registrarAcreditacionSinPDF: { method: 'PUT', url: 'api/liquidaciones/:id/RegistrarAcreditacionSinPDF', params: { id: '@id' } },
          compensacion: { method: 'GET', url: 'api/liquidaciones/compensacion/:jurisdiccionId/:liquidacionesId', params: { jurisdiccionId: '@jurisdiccionId', liquidacionesId: '@liquidacionesId' } },
      });
  }]);