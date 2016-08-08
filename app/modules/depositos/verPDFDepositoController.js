angular
    .module('webApp')
    .controller('verPDFDepositoController', ['$scope', '$routeParams', '$resource', '$http', 'Liquidacion', '$sce', 'FileSaver', 'Blob', verPDFDepositoController]);

function verPDFDepositoController($scope, $routeParams, $resource, $http, Liquidacion, $sce, FileSaver, Blob) {
    var vm = this;

    vm.liquidacion = Liquidacion.get({ id: $routeParams.liquidacionId },
        function () {
            $scope.$parent.appCtrl.titulo = "Ver PDF Depósito";

            $scope.$parent.appCtrl.subtitulo = vm.liquidacion.FechaCorteHasta;

            if (vm.liquidacion.Movimiento === 'Depósito') {
                $scope.$parent.appCtrl.subtitulo += ' - ' + vm.liquidacion.Destino;
            } else {//Acreditación 
                $scope.$parent.appCtrl.subtitulo += ' - ' + vm.liquidacion.Origen;
            }

            $scope.$parent.appCtrl.subtitulo += ' - ' + vm.liquidacion.Importe;

            vm.liquidacionExiste = true;
        },
        function (error) {
            vm.liquidacionInexistente = true;
        });

    vm.datauri = !isIE();

    $http.get('api/Liquidaciones/' + $routeParams.liquidacionId + '/ReciboDeposito', { responseType: 'arraybuffer' })
           .then(function (response) {
               vm.file = new Blob([response.data], { type: 'application/pdf' });
               if (vm.datauri) {
                   var fileUrl = URL.createObjectURL(vm.file);
                   vm.pdfContent = $sce.trustAsResourceUrl(fileUrl);
               }
           }, function (errorResponse) {
               alert(errorResponse);
           });

    vm.descargarPdf = function () {
        FileSaver.saveAs(vm.file, 'ReciboDeposito.pdf');
    }
};