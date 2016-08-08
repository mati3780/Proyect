angular
    .module('webApp')
    .controller('depositosPendientesController', ['$scope', '$compile', '$uibModal', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'DepositoPendiente', 'Jurisdiccion', depositosPendientesController]);

function depositosPendientesController($scope, $compile, $uibModal, $http, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, 
										DepositoPendiente, Jurisdiccion) {
    var vm = this;

    vm.seleccionados = [];
    
    vm.fillJurisdicciones = function () {
        vm.jurisdicciones = Jurisdiccion.bloqueadas(function (success) { }, function (error) {
                alert('Error al obtener las Jurisdicciones.');
            });
    }();

    vm.jurisdiccionChanged = function () {
        if (vm.filtro.JurisdiccionId) {
            $http.post('api/Liquidaciones/SearchLiquidacionesPendientes', vm.filtro)
                .then(function (response) {
                    vm.datos = response.data;
                }, function (error) {
                    alert('Error al obtener los datos');
                });
        }
    }

    vm.seleccionados = function () {
        if (!vm.datos)
            return [];

        var res = $.grep(vm.datos, function (x) { return x.selected; });
        return res;
    }

    vm.ids = function () {
        if (!vm.datos)
            return [];
        return vm.seleccionados().map(function (a) { return a.Id; }).toString();
    }

    vm.calcularTotal = function () {
        var total = 0;
        var seleccionados = vm.seleccionados();
        if (seleccionados && seleccionados.length > 0) {
            for (var i = 0; i < seleccionados.length; i++) {
                if (seleccionados[i].Movimiento === "Depósito") {
                    total -= seleccionados[i].Importe;
                } else {
                    total += seleccionados[i].Importe;
                }
            }
        }
        return total;
        }

    //TODO: agregar validacion de que se seleccione minimo una acreditacion
    function checkAnyAcreditacion() {
        var seleccionados = vm.seleccionados();
        for (var i = 0; i < seleccionados.length; i++) {
            if (seleccionados[i].Movimiento === "Acreditación")
                return true;
        }
        return false;
    }

    
    vm.habilitarCompensacion = function () {
        var seleccionados = vm.seleccionados();
        if (seleccionados.length === 0 || vm.calcularTotal() > 0)
            return true;
        else
            return false;
    }

    vm.rowClicked = function (obj) {
        if(obj.selected)
            obj.selected = false;
        else
            obj.selected = true;
    };
}