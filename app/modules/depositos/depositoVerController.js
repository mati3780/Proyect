angular
    .module('webApp')
    .controller('depositoVerController', ['$scope', '$routeParams', '$resource', '$http', 'Liquidacion', depositoVerController]);

function depositoVerController($scope, $routeParams, $resource, $http, Liquidacion) {
    var vm = this;

    vm.liquidacion = Liquidacion.get({ id: $routeParams.liquidacionId },
        function () {
            $scope.$parent.appCtrl.titulo = "Ver " + vm.liquidacion.Movimiento;

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

    vm.totalTasaNacional = function () {
        var total = 0;
        for (var i = 0; i < vm.liquidacion.Movimientos.length; i++) {
            total += vm.liquidacion.Movimientos[i].TasaNacional;
        }
        return total;
    };

    vm.totalTasaProvincial = function () {
        var total = 0;
        for (var i = 0; i < vm.liquidacion.Movimientos.length; i++) {
            total += vm.liquidacion.Movimientos[i].TasaProvincial;
        }
        return total;
    };

    vm.totalImporteProyect = function () {
        var total = 0;
        for (var i = 0; i < vm.liquidacion.Movimientos.length; i++) {
            total += vm.liquidacion.Movimientos[i].ImporteProyect;
        }
        return total;
    };

    vm.totalDebito = function () {
        var total = 0;
        for (var i = 0; i < vm.liquidacion.Movimientos.length; i++) {
            total += vm.liquidacion.Movimientos[i].Debito;
        }
        return total;
    };

    vm.totalCredito = function () {
        var total = 0;
        for (var i = 0; i < vm.liquidacion.Movimientos.length; i++) {
            total += vm.liquidacion.Movimientos[i].Credito;
        }
        return total;
    };

};