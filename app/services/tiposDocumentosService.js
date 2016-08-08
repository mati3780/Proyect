var tiposDocumentosServices = angular.module('tiposDocumentosServices', ['ngResource']);

tiposDocumentosServices.factory('TiposDocumentos', ['$resource',
    function ($resource) {
        return $resource('api/TiposDocumentos/:id', { id: '@id' });
    }]);