angular
    .module('webApp')
    .controller('plazoDetalleController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'Plazo', 'authentication', 'Jurisdiccion', plazoDetalleController]);

function plazoDetalleController($scope, $routeParams, $resource, $http, $location, validation, Plazo, authentication, Jurisdiccion) {
    var vm = this;

    vm.currentUser = authentication.getCurrentLoginUser();

    vm.plazoInexistente = false;

	Jurisdiccion.get({ id: vm.currentUser.jurisdiccion }, function(data) {
		vm.jurisdiccionDescripcion = data.Descripcion;
	}, function(error) {
		alert('Error al obtener Jurisdicción.');
	});

    vm.plazo = Plazo.get({ id: $routeParams.plazoId },
    function () {
    },
    function (error) {
        if (error.status === 404) {
            vm.plazoInexistente = true;
        }
    });
}