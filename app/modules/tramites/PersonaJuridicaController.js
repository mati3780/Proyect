angular
    .module('webApp')
    .controller('personaJuridicaController', ['$scope', '$uibModalInstance', 'action', 'personaJuridica', personaJuridicaController]);

function personaJuridicaController($scope, $uibModalInstance, action, personaJuridica) {
    var vm = this;

    vm.action = action;
    vm.personaJuridica = personaJuridica;

    vm.cancelarPersonaJuridica = function () {
        $uibModalInstance.dismiss('cancel');
    }

    vm.aceptarPersonaJuridica = function () {
        $scope.$broadcast('show-errors-check-validity', 'personaJuridicaForm');
        if (vm.personaJuridicaForm.$valid && vm.personaJuridica && (vm.personaJuridica.RazonSocial || vm.personaJuridica.Cuit)) {
            $uibModalInstance.close(vm.personaJuridica);
        }
    }
}