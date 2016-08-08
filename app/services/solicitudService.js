var solicitudServices = angular.module('solicitudServices', ['ngResource']);

solicitudServices.factory('Solicitud', ['$resource',
  function ($resource) {
      return $resource('api/solicitudes/:id', { id: "@id" }, {
          update: { method: 'PUT' }
      });
  }]);
