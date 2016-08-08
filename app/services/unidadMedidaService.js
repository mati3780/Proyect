var unidadesMedidaServices = angular.module('unidadesMedidaServices', ['ngResource']);

unidadesMedidaServices.factory('UnidadMedida', ['$resource',
    function ($resource) {
    	return $resource('api/UnidadesMedida/:id', { id: '@id' });
    }]);