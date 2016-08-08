var profesionesServices = angular.module('profesionesServices', ['ngResource']);

profesionesServices.factory('Profesion', ['$resource',
    function ($resource) {
    	return $resource('api/Profesiones/:id', { id: '@id' });
    }]);