angular
    .module('webApp')
    .controller('observarController', ['$scope', '$uibModalInstance', 'titulo', observarController]);

function observarController($scope, $uibModalInstance, titulo) {
    var vm = this;

    vm.tituloObservacion = titulo;

    vm.cancelarObservar = function () {
        $uibModalInstance.dismiss('cancel');
    }

    vm.aceptarObservar = function () {
        $scope.$broadcast('show-errors-check-validity', 'observarForm');
        if (vm.observarForm.$valid) {
            $uibModalInstance.close({ observacion: vm.Observacion });
        }
    }
}