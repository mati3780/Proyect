angular
    .module('webApp')
    .controller('informanteVerTramiteController', ['$scope', '$routeParams', '$resource', '$http', '$uibModal', '$location', '$filter', 'validation', 'Tramite', 
									'Provincia', 'TiposDocumentos', 'UnidadMedida', 'Jurisdiccion', 'Profesion', informanteVerTramiteController]);

function informanteVerTramiteController($scope, $routeParams, $resource, $http, $uibModal, $location, $filter, validation, Tramite, Provincia, TiposDocumentos, 
										UnidadMedida, Jurisdiccion, Profesion) {
    var vm = this;

    vm.tramite = Tramite.get({ id: $routeParams.tramiteId },
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