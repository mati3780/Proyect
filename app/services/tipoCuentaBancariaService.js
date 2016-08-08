var tipoCuentaBancariaServices = angular.module('tipoCuentaBancariaServices', ['ngResource']);

tipoCuentaBancariaServices.factory('TipoCuentaBancaria', ['$resource',
  function ($resource) {
  	return $resource('api/TipoCuentaBancarias/:id', { id: "@id" }, {
  	});
  }]);