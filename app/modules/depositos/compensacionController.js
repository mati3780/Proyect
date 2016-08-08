angular
    .module('webApp')
    .controller('compensacionController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'authentication', 'validation', 'Liquidacion', 'Jurisdiccion', 'Upload', compensacionController]);

function compensacionController($scope, $routeParams, $resource, $http, $location, authentication, validation, Liquidacion, Jurisdiccion, Upload) {
    var vm = this;

    var headers = { 'Authorization': '' };
    headers['Authorization'] = "Bearer " + authentication.getCurrentLoginUser().access_token;

    vm.liquidacion = Jurisdiccion.get({ id: $routeParams.jurisdiccionId },
        function() {
            $scope.$parent.appCtrl.titulo = "Depósito";
            $scope.$parent.appCtrl.subtitulo = "Compensar";
            vm.liquidacionExiste = true;
            vm.liquidacion.Importe = Math.abs($routeParams.importe);
            vm.liquidacion.liquidacionesArray = $routeParams.liquidacionesId.split(',');
            vm.liquidacion.FechaDeposito = null;
            vm.liquidacion.Transaccion = null;
            vm.liquidacion.Entidad = null;
            vm.liquidacion.Sucursal = null;
            vm.liquidacion.Cajero = null;
            vm.liquidacion.CuentaBancariaId = null;
            vm.liquidacion.JurisdiccionId = $routeParams.jurisdiccionId;
        },
        function (error) {
            if (error.status === 400) {
                validation.validate(vm, error);
            } 
            else if (error.status === 404){
                vm.liquidacionInexistente = true;    
            }
    });

    vm.submitError = function (error) {
        switch (error.status) {
            case 400:
                validation.validate(vm, error);
                break;
        }
    };
 

    vm.guardarReciboDeposito = function (file) {
        if (vm.liquidacion.Importe > 0) {
            if (!file)
                return;
            vm.depositoForm.reciboDeposito.$setValidity('invalidPDF', file.name.toString().toLowerCase().indexOf("pdf") > -1);
            vm.depositoForm.reciboDeposito.$setValidity('invalidSize', (parseInt(file.size) / 1048576) <= 30);
        }

        $scope.$broadcast('show-errors-check-validity', 'depositoForm');
    
        if (!vm.depositoForm.$valid) return;

        vm.upload = Upload.upload({
            url: 'api/liquidaciones/RegistrarCompensacion',
            data: {
                FechaDeposito: vm.liquidacion.FechaDeposito,
                Transaccion: vm.liquidacion.Transaccion,
                Entidad: vm.liquidacion.Entidad,
                Sucursal: vm.liquidacion.Sucursal,
                Cajero: vm.liquidacion.Cajero,
                CuentaBancariaId: vm.liquidacion.CuentaBancariaId,
                liquidacionesArray: $routeParams.liquidacionesId,
                jurisdiccionId : $routeParams.jurisdiccionId
            },
            method: "PUT",
            headers: headers,
            file: file
        }).progress(function (evt) {
            //console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
        }).success(function (data, status, headers, config) {
            $location.path('/DepositosAcreditacionesPendientes');
        }).error(function (data, status) {
            vm.submitError({ status: status, data: data, statusText: data.Message });
        });
    };

};