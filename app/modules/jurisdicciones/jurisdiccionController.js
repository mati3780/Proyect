angular
    .module('webApp')
    .controller('jurisdiccionController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'authentication', 'Jurisdiccion', 'Provincia', 
					'TipoEstadoAdhesion', jurisdiccionController]);

function jurisdiccionController($scope, $routeParams, $resource, $http, $location, validation, authentication, Jurisdiccion, Provincia, TipoEstadoAdhesion) {
    var vm = this;

    vm.esProyect = authentication.getCurrentLoginUser().aliases.indexOf('PROYECT') !== -1;
    vm.esJurisdisccion = authentication.getCurrentLoginUser().aliases.indexOf('JURISDICCION') !== -1;
	
    vm.fillSelectProvincias = function() {
		vm.provincias = Provincia.query(function() {}, function(error) {
			vm.provincias = [];
			alert('Error al obtener las provincias.');
		});
    };

    vm.fillSelectLocalidades = function() {
        if (vm.jurisdiccion.ProvinciaId) {
			vm.localidades = Provincia.localidades({ provinciaId: vm.jurisdiccion.ProvinciaId }, function() {}, function(error) {
				vm.localidades = [];
				alert('Error al obtener las localidades.');
			});
        } else {
            vm.localidades = [];
        }
    };

	vm.tipoEstadoAdhesiones = TipoEstadoAdhesion.query(function() {}, function(error) {
		vm.tipoEstadoAdhesiones = [];
        alert('Error al obtener los Tipos Estados Adhesiones.');
	});

    if (vm.esProyect) {
        vm.fillSelectProvincias();
    }

    vm.jurisdiccionInexistente = false;

    if ($routeParams.jurisdiccionId != undefined) {
        vm.jurisdiccion = Jurisdiccion.get({ id: $routeParams.jurisdiccionId },
        function() {
            vm.fillSelectLocalidades();
        },
		function (error) {
		    if (error.status === 403) {
		        $location.path('/notauthorized').replace();
		    }
		    if (error.status === 404) {
		        vm.jurisdiccionInexistente = true;
		    }
		});
    } else {
        vm.numero = 'Nuevo';
        vm.jurisdiccion = new Jurisdiccion();
        vm.jurisdiccion.Id = 0;
    }

    var submitError = function (error) {
        switch (error.status) {
            case 404:
                vm.jurisdiccionInexistente = true;
                break;
            case 400:
                validation.validate(vm, error);
                break;
            default:
                alert('Ocurrió un error en la llamada al servidor:\n\nError: ' + error.statusText + '\nMensaje: ' + error.data.Message);
        }
    }

    vm.guardar = function () {
        $scope.$broadcast('show-errors-check-validity', 'jurisdiccionForm');
        if (vm.jurisdiccionForm.$valid) {
            if ($routeParams.jurisdiccionId == undefined) {
                vm.jurisdiccion.$save(function () {
                    $location.path('/jurisdicciones');
                },
	    			function (error) {
	    			    submitError(error);
	    			});
            } else {
                vm.jurisdiccion.$update({ id: $routeParams.jurisdiccionId }, function () {
                    $location.path('/jurisdicciones');
                },
	    			function (error) {
	    			    submitError(error);
	    			});
            }
        }
    };
};