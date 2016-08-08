angular
    .module('webApp')
    .controller('registrarTramiteController', ['$scope', '$routeParams', '$resource', '$http', '$uibModal', '$location', '$filter', 'validation', 'Solicitud', 
										'Provincia', 'TiposDocumentos', 'UnidadMedida', 'Jurisdiccion', 'Profesion', registrarTramiteController]);

function registrarTramiteController($scope, $routeParams, $resource, $http, $uibModal, $location, $filter, validation, Solicitud, Provincia, TiposDocumentos,
										UnidadMedida, Jurisdiccion, Profesion) {
	var vm = this;

    vm.tramite = Solicitud.get({ id: $routeParams.tramiteId },
        function() {
            vm.tramiteExiste = true;
            vm.permiteRectificar = true;
            vm.permiteIngresarDatosPago = true;
            vm.solicitanteOriginal = clone(vm.tramite.Solicitante);
            vm.entidadPersonaOriginal = clone(vm.tramite.Entidad.Persona);
            vm.entidadInmuebleOriginal = clone(vm.tramite.Entidad.Inmueble);
            initTramiteController(vm, $scope, $http, $uibModal, $filter, validation, $routeParams, $location, Provincia, TiposDocumentos, UnidadMedida, 
									Jurisdiccion, Profesion);
        },
        function(error) {
            alert(error);
        });

    function validatePago() {
        var isValidGreaterThanOrEqualToFechaSolicitud = true;
        var isValidLessThanOrEqualToToday = true;

        if (vm.tramite.Pago && vm.tramite.Pago.Fecha) {
            isValidGreaterThanOrEqualToFechaSolicitud = getDate(vm.tramite.Pago.Fecha) >= getDate(vm.tramite.FechaSolicitud);
            isValidLessThanOrEqualToToday = getDate(vm.tramite.Pago.Fecha) <= new Date();
        }

        vm.tramiteForm.fecha.$setValidity('GreaterThanOrEqualToFechaSolicitud', isValidGreaterThanOrEqualToFechaSolicitud);
        vm.tramiteForm.fecha.$setValidity('LessThanOrEqualToToday', isValidLessThanOrEqualToToday);
    }

    vm.guardar = function () {
        if (confirm('¿Registrar Trámite?')) {
            if (!vm.tramite.ReparticionSolicitanteId) {
                validatePago();
            }
            vm.guardarBase("/registrartramites");
        }
    };
};