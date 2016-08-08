angular
    .module('webApp')
    .controller('depositoController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'authentication', 'validation', 'Liquidacion', 'Upload', depositoController]);

function depositoController($scope, $routeParams, $resource, $http, $location, authentication, validation, Liquidacion, Upload) {
    var vm = this;

    var headers = { 'Authorization': '' };
    headers['Authorization'] = "Bearer " + authentication.getCurrentLoginUser().access_token;
    
    vm.liquidacion = Liquidacion.get({ id: $routeParams.liquidacionId },
        function () {
            //SET TITULO
            $scope.$parent.appCtrl.titulo = "Registrar " + vm.liquidacion.Movimiento;
            $scope.$parent.appCtrl.subtitulo = vm.liquidacion.FechaCorteHasta;
            if (vm.liquidacion.Movimiento === 'Depósito') {
                $scope.$parent.appCtrl.subtitulo += ' - ' + vm.liquidacion.Destino;
            } else { //Acreditación 
                $scope.$parent.appCtrl.subtitulo += ' - ' + vm.liquidacion.Origen;
            }
            $scope.$parent.appCtrl.subtitulo += ' - ' + vm.liquidacion.Importe;
            //END: SET TITULO
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

    vm.submitError = function (error) {
        switch (error.status) {
            case 400:
                validation.validate(vm, error);
                break;
            default:
                alert('Ocurrió un error en la llamada al servidor:\n\nError: ' + error.statusText + '\nMensaje: ' + error.data.Message);
        }
    };

    vm.guardarReciboDeposito = function (file) {
        $scope.$broadcast('show-errors-check-validity', 'depositoForm');
        vm.depositoForm.reciboDeposito.$setValidity('invalidPDF', file.name.toString().toLowerCase().indexOf("pdf") > -1);
        vm.depositoForm.reciboDeposito.$setValidity('invalidSize', (parseInt(file.size) / 1048576) <= 30);
        if (!vm.depositoForm.$valid) return;

        vm.upload = Upload.upload({
            url: 'api/liquidaciones/' + $routeParams.liquidacionId + '/RegistrarDeposito',
            data: {
                FechaDeposito: vm.liquidacion.FechaDeposito,
                Transaccion: vm.liquidacion.Transaccion,
                Entidad: vm.liquidacion.Entidad,
                Sucursal: vm.liquidacion.Sucursal,
                Cajero: vm.liquidacion.Cajero,
                CuentaBancariaId: vm.liquidacion.CuentaBancariaId
            },
            method: "PUT",
            headers: headers,
            file: file
        }).progress(function (evt) {
            //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
        }).success(function (data, status, headers, config) {
            $location.path('/ActualizarDepositosAcreditaciones');
        }).error(function (data, status) {
            vm.submitError({ status: status, data: data, statusText: data.Message });
        });
    };

    vm.guardarComprobanteAcreditacion = function (file) {
        $scope.$broadcast('show-errors-check-validity', 'depositoForm');
        if (file) {
            vm.depositoForm.comprobanteAcreditacion.$setValidity('invalidPDF', file.name.toString().toLowerCase().indexOf("pdf") > -1);
            vm.depositoForm.comprobanteAcreditacion.$setValidity('invalidSize', (parseInt(file.size) / 1048576) <= 30);
        }
        if (!vm.depositoForm.$valid) return;
        vm.upload = Upload.upload({
            url: 'api/liquidaciones/' + $routeParams.liquidacionId + '/RegistrarAcreditacion',
            data: {
                ConciliacionVerificada: vm.liquidacion.ConciliacionVerificada,
                ConciliacionObservacion: vm.liquidacion.ConciliacionObservacion
            },
            method: "PUT",
            headers: headers,
            file: file
        }).progress(function (evt) {
            //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
        }).success(function (data, status, headers, config) {
            $location.path('/ActualizarDepositosAcreditaciones');
        }).error(function (data, status) {
            vm.submitError({ status: status, data: data, statusText: data.Message });
        });
    };
};