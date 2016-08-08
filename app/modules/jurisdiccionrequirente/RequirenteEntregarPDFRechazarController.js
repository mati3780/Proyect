angular
    .module('webApp')
    .controller('requirenteEntregarPDFRechazarController', ['$scope', '$location', '$http', '$uibModalInstance', 'tramiteId', requirenteEntregarPDFRechazarController]);

function requirenteEntregarPDFRechazarController($scope, $location, $http, $uibModalInstance, tramiteId) {
    var vm = this;

    vm.tramiteId = tramiteId;

    vm.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };

    vm.rechazar = function () {
        $scope.$broadcast('show-errors-check-validity', 'requirenteEntregarPDFRechazarForm');
        if (vm.requirenteEntregarPDFRechazarForm.$valid) {
            var accion = vm.MotivoId === '8' ? 'EntregaRechazoNoCorresponde' : 'EntregaRechazoVisualizacion';
            $http({
                url: 'api/tramites/' + vm.tramiteId + '/Accion/' + accion,
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