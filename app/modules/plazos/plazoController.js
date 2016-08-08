angular
    .module('webApp')
    .controller('plazoController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'Plazo', 'Servicio', 'Jurisdiccion', 'authentication', plazoController]);

function plazoController($scope, $routeParams, $resource, $http, $location, validation, Plazo, Servicio, Jurisdiccion, authentication) {
    var vm = this;

    vm.plazoInexistente = false;

    if ($routeParams.plazoId != undefined) {
    	vm.plazo = Plazo.get({ id: $routeParams.plazoId },
							function () {
							},
							function (error) {
								if (error.status === 404) {
									vm.plazoInexistente = true;
								}
							});
    } else {
    	vm.numero = 'Nuevo';
    	vm.plazo = new Plazo();
    	vm.plazo.Id = 0;
    }

	vm.jurisdiccion = Jurisdiccion.get({ id: authentication.getCurrentLoginUser().jurisdiccion });

	vm.tiposTramites = Servicio.disponibles({}, function () { },
												function (error) {
            										alert(error);
												});

	vm.servicioChanged = function () {
		if (vm.plazo.ServicioId) {
			vm.plazos = Plazo.plazosNoConfigurados({ servicioId: vm.plazo.ServicioId });
		} else {
			vm.plazos = [];
		}
	}
    
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
			vm.plazo.$save(function () {
								$location.path('/plazos');
							},
	    					function (error) {
	    						submitError(error);
	    					});
        }
    };
};