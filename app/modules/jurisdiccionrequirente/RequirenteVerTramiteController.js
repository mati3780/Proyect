angular
    .module('webApp')
    .controller('requirenteVerTramiteController', ['$scope', '$routeParams', '$resource', '$http', '$uibModal', '$location', '$filter', 'validation', 'Tramite',
									'Provincia', 'TiposDocumentos', 'UnidadMedida', 'Jurisdiccion', 'Profesion', requirenteVerTramiteController]);

function requirenteVerTramiteController($scope, $routeParams, $resource, $http, $uibModal, $location, $filter, validation, Tramite, Provincia, TiposDocumentos, 
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
};