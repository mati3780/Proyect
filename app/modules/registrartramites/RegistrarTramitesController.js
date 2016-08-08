angular
    .module('webApp')
    .controller('registrarTramitesController', ['$scope', '$compile', '$http', '$uibModal', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'TiposDocumentos', registrarTramitesController]);

function registrarTramitesController($scope, $compile, $http, $uibModal, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, TiposDocumentos) {
    var vm = this;

    vm.ErrorBusqueda = false;

    vm.tipoDocumentos = TiposDocumentos.query(function () { }, function (error) {
		alert('Error al obtener los tipos de documento.');
	});

    vm.buscar = function () {
        $scope.$broadcast('show-errors-check-validity', "consultaForm");
        if (vm.consultaForm.$valid) {
            vm.ErrorBusqueda = false;
        vm.dtInstance.reloadData();
        } else {
            vm.ErrorBusqueda = true;
        }
    }

    vm.dtInstance = {};

    function actionsHtml(data, type, full, meta) {
        return ('<a ng-href="#/registrartramites/' + data.Id + '/ver" access="RegistrarTramites" class="btn btn-info btn-sm margin-right-xs" title="Ver Trámite"><i class="fa fa-search-plus"></i></a>' +
                '<a ng-href="#/registrartramites/' + data.Id + '/imprimir" access="RegistrarTramites" class="btn btn-default btn-sm margin-right-xs" title="Imprimir Solicitud"><i class="fa fa-print"></i></a>' +
				'<a ng-href="#/registrartramites/' + data.Id + '" access="RegistrarTramite" class="btn btn-primary btn-sm " title="Registrar el Trámite"><i class="fa fa-refresh"></i></a>');
    }

    vm.dtOptions = DTOptionsBuilder.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.post('api/Solicitudes/Search', vm.filtro, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					})
					.withBootstrap();

    vm.dtColumns = [
        DTColumnBuilder.newColumn('FechaTramite').withTitle('Fecha Solicitud').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('ServicioDescripcion').withTitle('Trámite').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Numero').withTitle('Número de Trámite').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('TasaProvincial').withTitle('Tasa Provincial').withClass('text-right').renderWith(function (data) {
        																												return '$ ' + data.toFixed(2);
																													}),
        DTColumnBuilder.newColumn('TasaNacional').withTitle('PROYECT').withClass('text-right').renderWith(function (data) {
																														return '$ ' + data.toFixed(2);
																													}),
        DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width text-left').notSortable().renderWith(actionsHtml)
    ];
}