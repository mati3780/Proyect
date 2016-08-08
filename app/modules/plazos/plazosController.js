angular
    .module('webApp')
    .controller('plazosController', ['$scope', '$compile', '$uibModal', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'Plazo', plazosController]);

function plazosController($scope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, Plazo) {
    var vm = this;

    vm.dtInstance = {};

    function actionsHtml(data, type, full, meta) {
        var html = '<a ng-href="#/plazos/' + data.Id + '/detalle" style="margin: 0 5px;" class="btn btn-info btn-sm" title="Ver Plazo de Trámite"><i class="fa fa-search-plus"></i></a>';

        if (!data.VigenciaHasta) {
            html += '<a ng-href="#/plazos/' + data.Id + '" access="BorrarPlazo" class="btn btn-danger btn-sm" title="Cerrar Plazo de Trámite"><i class="fa fa-lock"></i></a>';
        }
        return (html);
    }

    vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.get('api/JurisdiccionServicioPlazos', null, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					})
					.withBootstrap();

    vm.dtColumns = [
        DTColumnBuilder.newColumn('ServicioDescripcion').withTitle('Trámite').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('PlazoDescripcion').withTitle('Plazo').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('PlazoDias').withTitle('Plazo en Días').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Costo').withTitle('Tasa Provincial').renderWith($.fn.dataTable.render.number('', '.', 2, '$ ')),
        DTColumnBuilder.newColumn('VigenciaDesde').withTitle('Vigencia Desde').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('VigenciaHasta').withTitle('Vigencia Hasta').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width').notSortable().renderWith(actionsHtml)
    ];
}