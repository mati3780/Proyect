angular
    .module('webApp')
    .controller('depositosController', ['$scope', '$compile', '$uibModal', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'Deposito', 'Jurisdiccion', depositosController]);

function depositosController($scope, $compile, $uibModal, $http, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, Deposito, Jurisdiccion) {
    var vm = this;

    vm.fillJurisdicciones = function () {
	    vm.jurisdicciones = Jurisdiccion.query();
    }();

    vm.buscar = function () {
        vm.dtInstance.reloadData();
    }

    vm.dtInstance = {};

    function actionsHtml(data, type, full, meta) {
        var html = '<a ng-href="#/ActualizarDepositosAcreditaciones/' + data.Id + '/ver" access="ListarDepositosAcreditaciones" class="btn btn-info btn-sm margin-right-xs" title="Ver"><i class="fa fa-search-plus"></i></a>'
                 + '<a ng-href="#/ActualizarDepositosAcreditaciones/' + data.Id + '/imprimir" access="ListarDepositosAcreditaciones" class="btn btn-default btn-sm margin-right-xs" title="Imprimir"><i class="fa fa-print"></i></a>';

        if (data.PuedeRegistrar) {
            html += '<a ng-href="#/ActualizarDepositosAcreditaciones/' + data.Id + '" access="ActualizarDepositoAcreditacion" class="btn btn-primary btn-sm" title="Registrar"><i class="fa fa-refresh"></i></a>';
        }
        
        return html;
    }

    vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.post('api/Liquidaciones/SearchLiquidaciones', vm.filtro, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					    angular.element(row).addClass(addClassSemaforo(data.EstadoSemaforo));
					})
					.withBootstrap();

    vm.dtColumns = [
        DTColumnBuilder.newColumn('FechaLimiteDeposito').withTitle('Límite Depósito').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Movimiento').withTitle('Movimiento').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Origen').withTitle('Origen').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Destino').withTitle('Destino').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Importe').withTitle('Importe').withClass('text-right').renderWith(function (data) {
        																												return '$' + data.toFixed(2);
																													}),
        DTColumnBuilder.newColumn('FechaDeposito').withTitle('Fecha Depósito').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('FechaCompensacion').withTitle('Fecha Compensación').renderWith($.fn.dataTable.render.text()),
        //DTColumnBuilder.newColumn('ImporteCompensacion').withTitle('Importe Compensación').withClass('text-right').renderWith(function (data) {
        //																												return data ? '$ ' + data.toFixed(2) : '';
		//																											}),
        DTColumnBuilder.newColumn('ConciliacionBancaria').withTitle('Conciliación Bancaria').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width text-left').notSortable().renderWith(actionsHtml)
    ];
}