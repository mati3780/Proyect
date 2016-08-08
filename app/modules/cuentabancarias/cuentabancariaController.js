angular
    .module('webApp')
    .controller('cuentabancariaController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'CuentaBancaria', 'authentication', 'Jurisdiccion', 'TipoCuentaBancaria', cuentabancariaController]);

function cuentabancariaController($scope, $routeParams, $resource, $http, $location, validation, CuentaBancaria, authentication, Jurisdiccion, TipoCuentaBancaria) {
	var vm = this;

	vm.cuentabancariaInexistente = false;

	if ($routeParams.cuentabancariaId != undefined) {
	    vm.cuentabancaria = CuentaBancaria.get({ id: $routeParams.cuentabancariaId }, function () { },
		function(error) {
			if (error.status === 404) {
				vm.cuentabancariaInexistente = true;
			}
		});
	} else {
		vm.numero = 'Nuevo';
		vm.cuentabancaria = new CuentaBancaria();
		vm.cuentabancaria.Id = 0;
	}

	vm.jurisdiccion = Jurisdiccion.get({ id: authentication.getCurrentLoginUser().jurisdiccion });

	vm.tipos = TipoCuentaBancaria.query();
	
	var submitError = function (error) {
		switch (error.status) {
			case 404:
			    vm.cuentabancariaInexistente = true;
				break;
			case 400:
				validation.validate(vm, error);
				break;
		}
	}

	vm.guardar = function () {
	    $scope.$broadcast('show-errors-check-validity', 'cuentabancariaForm');
	    if (vm.cuentabancariaForm.$valid) {
	        if ($routeParams.cuentabancariaId == undefined) {
	            vm.cuentabancaria.$save(function() {
	                    $location.path('/cuentabancarias');
	                },
	                function(error) {
	                    submitError(error);
	                });
	        } else {
	            vm.cuentabancaria.$update({ id: $routeParams.cuentabancariaId }, function() {
	                    $location.path('/cuentabancarias');
	                },
	                function(error) {
	                    submitError(error);
	                });
	        }
	    }
	};
};