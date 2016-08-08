angular
    .module('webApp')
    .controller('requirenteEntregarPDFAceptarController', ['$scope', '$location', '$http', '$uibModalInstance', 'tramiteId', requirenteEntregarPDFAceptarController]);

function requirenteEntregarPDFAceptarController($scope, $location, $http, $uibModalInstance, tramiteId) {
    var vm = this;

    vm.tramiteId = tramiteId;

    vm.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };

    vm.aceptar = function () {
        $scope.$broadcast('show-errors-check-validity', 'requirenteEntregarPDFAceptarForm');
        if (vm.requirenteEntregarPDFAceptarForm.$valid) {
            $http({
                url: 'api/tramites/' + vm.tramiteId + '/Accion/EntregaAceptacion',
                method: "PUT",
                data: { 'TramiteId': vm.tramiteId, 'Observacion': vm.Observacion }
            })
            .then(function (response) {
                $uibModalInstance.dismiss('cancel');
                $location.path('/jurisdiccionRequirente');
            },
            function (response) {
                alert('Error');
            });
        }
    };
}