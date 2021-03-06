﻿angular
    .module('webApp')
    .controller('verTramiteController', ['$scope', '$routeParams', '$resource', '$http', '$uibModal', '$location', '$filter', 'validation', 'Solicitud',
								'Provincia', 'TiposDocumentos', 'UnidadMedida', 'Jurisdiccion', 'Profesion', verTramiteController]);

function verTramiteController($scope, $routeParams, $resource, $http, $uibModal, $location, $filter, validation, Solicitud, Provincia, TiposDocumentos, UnidadMedida,
								Jurisdiccion, Profesion) {
    var vm = this;

    vm.tramite = Solicitud.get({ id: $routeParams.tramiteId },
        function () {
            vm.tramiteExiste = true;
            vm.permiteRectificar = false;
            initTramiteController(vm, $scope, $http, $uibModal, $filter, validation, $routeParams, $location, Provincia, TiposDocumentos, UnidadMedida, Jurisdiccion, 
								Profesion);
        },
        function (error) {
            alert(error);
        });
};