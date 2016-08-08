angular
    .module('webApp')
    .controller('depositoImprimirController', ['$scope', '$routeParams', '$resource', '$http', '$sce', 'Liquidacion', 'FileSaver', 'Blob', depositoImprimirController]);

function depositoImprimirController($scope, $routeParams, $resource, $http, $sce, Liquidacion, FileSaver, Blob) {
    var vm = this;

    vm.liquidacion = Liquidacion.get({ id: $routeParams.liquidacionId },
        function () {
            $scope.$parent.appCtrl.titulo = "Imprimir " + vm.liquidacion.Movimiento;

            $scope.$parent.appCtrl.subtitulo = vm.liquidacion.FechaCorteHasta;

            if (vm.liquidacion.MovimientoEnumValue === 2) {
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
    $http.get('api/Liquidaciones/report/' + $routeParams.liquidacionId, { responseType: 'arraybuffer' })
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
        FileSaver.saveAs(vm.file, 'Liquidacion.pdf');
    }
};