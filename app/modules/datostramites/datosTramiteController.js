angular
    .module('webApp')
    .controller('datosTramiteController', ['$scope', '$routeParams', '$compile', '$uibModal', '$location', 'popupService', 'validation', 'DatoTramite', 'Servicio', 'authentication', 'Jurisdiccion', tramiteDatoController]);

function tramiteDatoController($scope, $routeParams, $compile, $uibModal, $location, popupService, validation, DatoTramite, Servicio, authentication, Jurisdiccion) {
	var vm = this;

	vm.new = false;

	if ($routeParams.servicioId != undefined) {
		vm.datoTramite = DatoTramite.get({ id: $routeParams.servicioId },
											function () { },
											function (error) {
												if (error.status === 404) {
													vm.datoTramiteInexistente = true;
												}
											});
	} else {
		vm.datoTramite = new DatoTramite();
		vm.new = true;
	}

	vm.datosCondiciones = Servicio.condiciones({}, function() {
		vm.datosCondicionesObligatorias = $.extend([], vm.datosCondiciones);
		vm.datosCondicionesObligatorias.splice(1, 1);
	});
	
	vm.jurisdiccion = Jurisdiccion.get({ id: authentication.getCurrentLoginUser().jurisdiccion });

	vm.tiposTramites = Servicio.noConfigurados();

	vm.servicioChanged = function () {
		if (vm.servicioId) {
			vm.datoTramite = DatoTramite.get({ id: vm.servicioId });
		}
	}

	vm.catastroChanged = function (objeto) {
	    vm.datoTramite.InmuebleDatos['NomenclaturaCatastralCircunscripcion'].Condicion = objeto.Condicion;
	    vm.datoTramite.InmuebleDatos['NomenclaturaCatastralSeccion'].Condicion = objeto.Condicion;
	    vm.datoTramite.InmuebleDatos['NomenclaturaCatastralManzana'].Condicion = objeto.Condicion;
	    vm.datoTramite.InmuebleDatos['NomenclaturaCatastralParcela'].Condicion = objeto.Condicion;

	    if (objeto.Condicion === 3) {
	        vm.datoTramite.InmuebleDatos['NomenclaturaCatastralCircunscripcion'].Label = '';
	        vm.datoTramite.InmuebleDatos['NomenclaturaCatastralSeccion'].Label = '';
	        vm.datoTramite.InmuebleDatos['NomenclaturaCatastralManzana'].Label = '';
	        vm.datoTramite.InmuebleDatos['NomenclaturaCatastralParcela'].Label = '';
	    }
	
	}
	
	var submitError = function (error) {
		switch (error.status) {
			case 404:
				vm.jurisdiccionInexistente = true;
				break;
			case 400:
				validation.validate(vm, error);
				break;
		}
	}

	vm.guardar = function () {
		$scope.$broadcast('show-errors-check-validity', 'datosTramiteForm');
		if (vm.datosTramiteForm.$valid) {
			if (vm.new) {
				vm.datoTramite.$save(function () {
						$location.path('/datostramites');
					},
	    			function (error) {
	    				submitError(error);
	    			});
			} else {
				vm.datoTramite.$update({ id: $routeParams.servicioId }, function () {
						$location.path('/datostramites');
					},
	    			function (error) {
	    				submitError(error);
	    			});
			}
		}
	}

    vm.clearInputs = function (objeto) {
	    if (objeto.Condicion === 3) 
	        objeto.Label = '';
	}
}