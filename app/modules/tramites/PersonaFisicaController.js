angular
    .module('webApp')
    .controller('personaFisicaController', ['$scope', '$uibModalInstance', '$http', 'action', 'personaFisica', 'TiposDocumentos', personaFisicaController]);

function personaFisicaController($scope, $uibModalInstance, $http, action, personaFisica, TiposDocumentos) {
    var vm = this;

    vm.action = action;
    vm.personaFisica = personaFisica;
    
    vm.tipoDocumentos = TiposDocumentos.query(function () { }, function (error) {
    	alert('Error al obtener los tipos de documento.');
    });

    vm.cancelarPersonaFisica = function () {
        $uibModalInstance.dismiss('cancel');
    }

    vm.aceptarPersonaFisica = function () {
        $scope.$broadcast('show-errors-check-validity', 'personaFisicaForm');
        if (vm.personaFisicaForm.$valid && vm.personaFisica && (vm.personaFisica.Apellido || vm.personaFisica.NumeroDocumento || vm.personaFisica.Cuit)) {

            if (vm.personaFisica.TipoDocumento && vm.personaFisica.TipoDocumento.TipoDocumentoId) {
                vm.personaFisica.TipoDocumentoId = vm.personaFisica.TipoDocumento.TipoDocumentoId;
            }

            $uibModalInstance.close(vm.personaFisica);
        }
    }
}