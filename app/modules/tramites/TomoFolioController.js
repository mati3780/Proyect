angular
    .module('webApp')
    .controller('tomoFolioController', ['$scope', '$uibModalInstance', 'action', 'tomoFolio', tomoFolioController]);

function tomoFolioController($scope, $uibModalInstance, action, tomoFolio) {
    var vm = this;

    vm.action = action;
    vm.tomoFolio = tomoFolio;

    vm.cancelarTomoFolio = function () {
        $uibModalInstance.dismiss('cancel');
    }

    vm.aceptarTomoFolio = function () {
        $scope.$broadcast('show-errors-check-validity', 'tomoFolioForm');
        if (vm.tomoFolioForm.$valid && vm.tomoFolio && (vm.tomoFolio.Tomo || vm.tomoFolio.Folio)) {
            $uibModalInstance.close(vm.tomoFolio);
        }
    }
}