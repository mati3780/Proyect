angular
    .module('webApp')
    .controller('requirenteResponderTramiteController', ['$scope', '$routeParams', '$resource', '$http', '$uibModal', '$location', '$filter', 'validation', 'Tramite', 
					'Provincia', 'TiposDocumentos', 'UnidadMedida', 'Jurisdiccion', 'Profesion', requirenteResponderTramiteController]);

function requirenteResponderTramiteController($scope, $routeParams, $resource, $http, $uibModal, $location, $filter, validation, Tramite, Provincia, TiposDocumentos, 
												UnidadMedida, Jurisdiccion, Profesion) {
    var vm = this;

    vm.tramite = Tramite.get({ id: $routeParams.tramiteId },
        function () {
            vm.tramiteExiste = true;
            vm.permiteRectificar = vm.tramite.EstadoActual.TramiteSubEstadoIdentificador === 1 || vm.tramite.EstadoActual.TramiteSubEstadoIdentificador === 4;
            vm.permiteIngresarDatosPago = false;
            vm.solicitanteOriginal = clone(vm.tramite.Solicitante);
            vm.entidadPersonaOriginal = clone(vm.tramite.Entidad.Persona);
            vm.entidadInmuebleOriginal = clone(vm.tramite.Entidad.Inmueble);
            initTramiteController(vm, $scope, $http, $uibModal, $filter, validation, $routeParams, $location, Provincia, TiposDocumentos, UnidadMedida, 
									Jurisdiccion, Profesion);
        },
        function (error) {
            alert(error);
        });

    vm.guardar = function () {
        if (confirm('¿Responder Trámite?')) {
            vm.guardarBase("/jurisdiccionrequirente");
        }
    };
    
    vm.verObservacion = function () {
        $uibModal.open({
            templateUrl: 'ModalVerObservacion.html',
            controller: 'verObservacionController',
            controllerAs: 'verObservacionCtrl',
            size: 'lg',
            backdrop: 'static',
            animation: false,
            resolve: {
                titulo: function () {
                    return 'Ver Observación';
                },
                observacion: function () {
                    return vm.tramite.EstadoActual.Observacion;
                }
            }
        });
    };
    
};