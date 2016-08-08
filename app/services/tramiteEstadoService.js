var tramiteEstadoServices = angular.module('tramiteEstadoServices', ['ngResource']);

tramiteEstadoServices.factory('TramiteEstado', ['$resource',
    function ($resource) {
        return $resource('api/TramiteEstado/:id', { id: '@id' }, {
            getTipoEstados: { method: 'GET', url: 'api/TramiteEstado/TipoEstados', isArray: true },
            getTipoSubEstados: { method: 'GET', url: 'api/TramiteEstado/TipoSubEstados/:tipoEstadoId', params: { tipoEstadoId: '@tipoEstadoId' }, isArray: true }
        });
    }]);