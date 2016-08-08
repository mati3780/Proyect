angular
    .module('webApp')
    .controller('jurisdiccionInformanteController', ['$scope', '$compile', '$uibModal', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'TiposDocumentos', 'TramiteEstado', jurisdiccionInformanteController]);

function jurisdiccionInformanteController($scope, $compile, $uibModal, $http, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, TiposDocumentos, TramiteEstado) {
    var vm = this;
    vm.ErrorBusqueda = false;
    vm.tipoDocumentos = TiposDocumentos.query(function () { }, function (error) {
        alert('Error al obtener los Tipos de Documentos.');
    });

    TramiteEstado.getTipoEstados(function (data) {
        vm.estadosTramite = data;
    }, function (error) {
        alert("Error al obtener los estados de trámite");
    });

    vm.fillSelectSubEstados = function () {
        if (vm.filtro.EstadoTramite != null) {
            vm.subEstadosTramite = TramiteEstado.getTipoSubEstados({ tipoEstadoId: vm.filtro.EstadoTramite }, function () { },
				function (error) {
				    alert("Error al obtener los SubEstados");
				});
        } else {
            vm.filtro.SubEstadoTramite = null;
            vm.subEstadosTramite = {};
        }
    };

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
        var html = '<a ng-href="#/jurisdiccionInformante/' + data.Id + '/ver" class="btn btn-info btn-sm margin-right-xs" title="Ver Trámite"><i class="fa fa-search-plus"></i></a>'
                 + '<a ng-href="#/jurisdiccionInformante/' + data.Id + '/imprimir" class="btn btn-default btn-sm margin-right-xs" title="Imprimir Trámite"><i class="fa fa-print"></i></a>';

        if (data.SubTipoEstadoId === 2 || data.SubTipoEstadoId === 3 || data.SubTipoEstadoId === 5 || data.SubTipoEstadoId === 6 || data.SubTipoEstadoId === 8 || data.SubTipoEstadoId === 9) {
            html += '<a ng-href="#/jurisdiccionInformante/' + data.Id + '" access="InformanteResponderTramite" class="btn btn-primary btn-sm " title="Responder Trámite"><i class="fa fa-reply"></i></a>';
        }
        return (html);
    }

    vm.dtOptions = DTOptionsBuilder.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.post('api/Tramites/Search/Informante', vm.filtro, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					    angular.element(row).addClass(addClassSemaforo(data.Estado));
					})
					.withBootstrap();

    vm.dtColumns = [
						DTColumnBuilder.newColumn('FechaTramite').withTitle('Fecha Tramite').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn('FechaEntrega').withTitle('Fecha Entrega').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn('EstadoDescripcion').withTitle('Estado').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('SubEstadoDescripcion').withTitle('SubEstado').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('JurisdiccionRequiriente').withTitle('Requiriente').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('Numero').withTitle('Nro. de Trámite').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('ServicioDescripcion').withTitle('Trámite').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('PlazoDescripcion').withTitle('Plazo').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width text-left').notSortable().renderWith(actionsHtml)
    ];
}