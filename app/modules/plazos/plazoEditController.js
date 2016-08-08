angular
    .module('webApp')
    .controller('plazoEditController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'Plazo', 'Servicio', 'Jurisdiccion', 'authentication', plazoEditController]);

function plazoEditController($scope, $routeParams, $resource, $http, $location, validation, Plazo, Servicio, Jurisdiccion, authentication) {
	var vm = this;

	vm.plazoInexistente = false;
	
	vm.plazo = Plazo.get({ id: $routeParams.plazoId },
						function () {
						},
						function (error) {
							if (error.status === 404) {
								vm.plazoInexistente = true;
							}
						});

	vm.jurisdiccion = Jurisdiccion.get({ id: authentication.getCurrentLoginUser().jurisdiccion });

	var submitError = function (error) {
		switch (error.status) {
			case 404:
				vm.plazoInexistente = true;
				break;
			case 400:
				validation.validate(vm, error);
				break;
			default:
				alert('Ocurrió un error en la llamada al servidor:\n\nError: ' + error.statusText + '\nMensaje: ' + error.data.Message);
		}
	}

	vm.guardar = function () {
		$scope.$broadcast('show-errors-check-validity', 'plazoForm');
		if (vm.plazoForm.$valid) {
			vm.plazo.$update({ id: $routeParams.plazoId }, function () {
				$location.path('/plazos');
			},
	    	function (error) {
	    		submitError(error);
	    	});
		}
	};
};