var jurisdiccionServices = angular.module('jurisdiccionServices', ['ngResource']);

jurisdiccionServices.factory('Jurisdiccion', ['$resource',
  function ($resource) {
      return $resource('api/jurisdicciones/:id', { id: "@id" }, {
          update: { method: 'PUT' },
          search: { method: 'GET', url: 'api/jurisdicciones/search/:nombre', params: { nombre: '@nombre' }, isArray: true },
          bloqueadas: { method: 'GET', url:'api/jurisdicciones/bloqueadas', isArray: true },
		  solicitantes: { method: 'GET', url: 'api/Jurisdicciones/Solicitantes', isArray: true },
          descripcion: { method: 'GET', url: 'api/jurisdicciones/descripcion' }
      });
  }]);
