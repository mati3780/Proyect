var provinciaServices = angular.module('provinciaServices', ['ngResource']);

provinciaServices.factory('Provincia', ['$resource',
  function ($resource) {
  	return $resource('api/Provincias/:id', { id: "@id" }, {
		localidades: { method: 'GET', url: 'api/Provincias/:provinciaId/Localidades', params: { provinciaId: '@provinciaId' }, isArray: true }
  	});
  }]);