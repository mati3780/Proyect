angular
    .module('webApp')
    .controller('verDatosTramiteController', ['$scope', '$routeParams', '$compile', '$uibModal', '$location', 'popupService', 'validation', 'DatoTramite', 'Servicio', 'authentication', 'Jurisdiccion', 'Plazo', verDatosTramiteController]);

function verDatosTramiteController($scope, $routeParams, $compile, $uibModal, $location, popupService, validation, DatoTramite, Servicio, authentication,
										Jurisdiccion, Plazo) {
	var vm = this;
	
	vm.datoTramite = DatoTramite.get({ id: $routeParams.servicioId },
									function () { },
									function (error) {
										if (error.status === 404) {
											vm.datoTramiteInexistente = true;
										}
									});
	
	vm.jurisdiccion = Jurisdiccion.get({ id: authentication.getCurrentLoginUser().jurisdiccion });

	vm.plazos = Plazo.plazosDisponibles({ servicioId: $routeParams.servicioId });

	vm.plazoChanged = function () {
		if (vm.plazoId) {
			vm.plazo = Plazo.disponible({
					servicioId: vm.datoTramite.ServicioId,
					plazoId: vm.plazoId
				});
		} else {
			vm.plazo = null;
		}
		
	}

}