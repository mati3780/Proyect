angular
    .module('webApp')
    .controller('feriadoController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'Feriado', feriadoController]);

function feriadoController($scope, $routeParams, $resource, $http, $location, validation, Feriado) {
	var vm = this;

	vm.feriadoInexistente = false;

	if ($routeParams.feriadoId != undefined) {
	    vm.feriado = Feriado.get({ id: $routeParams.feriadoId }, function() {},
		function(error) {
			if (error.status === 404) {
				vm.feriadoInexistente = true;
			}
		});
	} else {
		vm.numero = 'Nuevo';
		vm.feriado = new Feriado();
		vm.feriado.Id = 0;
	}

	var submitError = function (error) {
		switch (error.status) {
			case 404:
			    vm.feriadoInexistente = true;
				break;
			case 400:
				validation.validate(vm, error);
				break;
			default:
				alert('Ocurrió un error en la llamada al servidor:\n\nError: ' + error.statusText + '\nMensaje: ' + error.data.Message);
		}
	}

	vm.guardar = function () {
		$scope.$broadcast('show-errors-check-validity', 'feriadoForm');
		if (vm.feriadoForm.$valid) {
			if ($routeParams.feriadoId == undefined) {
				vm.feriado.$save(function() {
						$location.path('/feriados');
					},
					function(error) {
						submitError(error);
					});
			} else {
				vm.feriado.$update({ id: $routeParams.feriadoId }, function() {
						$location.path('/feriados');
					},
					function(error) {
						submitError(error);
					});
			}
		}
	};
};