angular
    .module('webApp')
    .controller('informanteResponderTramiteController', ['$scope', '$routeParams', '$resource', '$http', '$uibModal', '$location', '$filter', 'validation', 'Tramite',
							'Provincia', 'TiposDocumentos', 'UnidadMedida', 'Jurisdiccion', 'Profesion', informanteResponderTramiteController]);

function informanteResponderTramiteController($scope, $routeParams, $resource, $http, $uibModal, $location, $filter, validation, Tramite, Provincia, TiposDocumentos, 
												UnidadMedida, Jurisdiccion, Profesion) {
    var vm = this;

    vm.tramite = Tramite.get({ id: $routeParams.tramiteId },
        function () {
            vm.tramiteExiste = true;
            vm.permiteRectificar = false;
            initTramiteController(vm, $scope, $http, $uibModal, $filter, validation, $routeParams, $location, Provincia, TiposDocumentos, UnidadMedida, 
									Jurisdiccion, Profesion);
        },
        function (error) {
            alert(error);
        });

    vm.documentoEnTramite = function () {
        var modalInstance = $uibModal.open({
            templateUrl: 'ModalObservar.html',
            controller: 'observarController',
            controllerAs: 'observarCtrl',
            size: 'lg',
            backdrop: 'static',
            animation: false,
            resolve: {
                titulo: function () {
                    return 'Observación de documento en trámite';
                }
            }
        });
        modalInstance.result.then(function (result) {
            $http({
                url: 'api/tramites/' + vm.tramite.Id + '/Accion/DocumentacionEnTramite',
                method: "PUT",
                data: { 'TramiteId': vm.tramite.Id, 'Observacion': result.observacion }
            })
            .then(function (response) {
                $location.path('/jurisdiccionInformante');
            },
            function (response) {
                alert('Error');
            });
        });
    };

    vm.observar = function () {
        var modalInstance = $uibModal.open({
            templateUrl: 'ModalObservar.html',
            controller: 'observarController',
            controllerAs: 'observarCtrl',
            size: 'lg',
            backdrop: 'static',
            animation: false,
            resolve: {
                titulo: function () {
                    return 'Causa de la Observación';
                }
            }
        });
        modalInstance.result.then(function (result) {
            $http({
                url: 'api/tramites/' + vm.tramite.Id + '/Accion/Observado',
                method: "PUT",
                data: { 'TramiteId': vm.tramite.Id, 'Observacion': result.observacion }
            })
            .then(function (response) {
                $location.path('/jurisdiccionInformante');
            },
            function (response) {
                alert('Error');
            });
        });
    };

    vm.ingresarPdf = function () {
        $uibModal.open({
            templateUrl: 'ModalIngresarPDF.html',
            controller: 'ingresarPDFController',
            controllerAs: 'ingresarPDFCtrl',
            size: 'lg',
            backdrop: 'static',
            animation: false,
            resolve: {
                tramiteId: function () {
                    return vm.tramite.Id;
                }
            }
        });
    };
    
};