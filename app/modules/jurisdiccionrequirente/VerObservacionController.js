angular
    .module('webApp')
    .controller('verObservacionController', ['$scope', '$uibModalInstance', 'titulo', 'observacion', verObservacionController]);

function verObservacionController($scope, $uibModalInstance, titulo, observacion) {
    var vm = this;

    vm.tituloObservacion = titulo;
    vm.Observacion = observacion;

    vm.cerrarObservar = function () {
        $uibModalInstance.dismiss('cancel');
    }
}