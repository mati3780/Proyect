angular
    .module('webApp')
    .controller('contribucionController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'ContribucionProyect', contribucionController]);

function contribucionController($scope, $routeParams, $resource, $http, $location, validation, ContribucionProyect) {
    var vm = this;

    vm.contribucionsinarepiInexistente = false;

    if ($routeParams.contribucionId != undefined) {
        vm.contribucionsinarepi = ContribucionProyect.get({ id: $routeParams.contribucionId }, function () { },
		function (error) {
		    if (error.status === 404) {
		        vm.contribucionsinarepiInexistente = true;
		    }
		});
    } else {
        vm.numero = 'Nuevo';
        vm.contribucionsinarepi = new ContribucionProyect();
        vm.contribucionsinarepi.Id = 0;
    }
	
    $http.get("api/ContribucionProyect/ObtenerFechaDesde")
			.then(function (response) {
				vm.VigenciaFechaDesde = response.data;
			});

    var submitError = function (error) {
        switch (error.status) {
            case 404:
                vm.contribucionsinarepiInexistente = true;
                break;
            case 400:
                validation.validate(vm, error);
                break;
        }
    }

    function validateContribucionsinarepi() {
        var isValid = true;

        if (vm.contribucionsinarepi.FechaHasta) {
            isValid = getDate(vm.contribucionsinarepi.FechaHasta) >= getDate(vm.contribucionsinarepi.FechaDesde);
        }

        vm.contribucionsinarepiForm.FechaHasta.$setValidity('GreaterThanOrEqualTo', isValid);
    }

    vm.guardar = function () {
        $scope.$broadcast('show-errors-check-validity', 'contribucionsinarepiForm');
        if ($routeParams.contribucionId != undefined) {
            validateContribucionsinarepi();
        }
        if (vm.contribucionsinarepiForm.$valid) {
            if ($routeParams.contribucionId == undefined) {
                vm.contribucionsinarepi.$save(function () {
                    $location.path('/contribuciones');
                },
	                function (error) {
	                    submitError(error);
	                });
            } else {
                vm.contribucionsinarepi.$update({ id: $routeParams.contribucionId }, function () {
                    $location.path('/contribuciones');
                },
	                function (error) {
	                    submitError(error);
	                });
            }
        }
    };
};