angular
    .module('webApp')
    .controller('jurisdiccionRequirenteController', ['$scope', '$compile', '$http', '$uibModal', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'TiposDocumentos','TipoEstadoSolicitud','TramiteEstado' ,jurisdiccionRequirenteController]);

function jurisdiccionRequirenteController($scope, $compile, $http, $uibModal, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, TiposDocumentos,TipoEstadoSolicitud,TramiteEstado) {
    var vm = this;
    vm.ErrorBusqueda = false;

    vm.tipoDocumentos = TiposDocumentos.query(function () { }, function (error) {
        alert('Error al obtener los Tipos de documento.');
    });

    TipoEstadoSolicitud.query(function (data) {
        vm.estadosSolicitud = data;
    }, function (error) {
        alert("Error al obtener los estados de solicitud");
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
        var html = '<a ng-href="#/JurisdiccionRequirente/' + data.Id + '/ver" class="btn btn-info btn-sm margin-right-xs" title="Ver Trámite"><i class="fa fa-search-plus"></i></a>';

        if (data.SubTipoEstadoId === 4 || data.SubTipoEstadoId === 7 || data.SubTipoEstadoId === 10) {//ELR - VERIFICAR ESTA LISTA DE ESTADOS EN LOS QUE SE DEBERIA MOSTRAR EL BOTON RESPONDER TRAMITE
            html += '<a ng-href="#/JurisdiccionRequirente/' + data.Id + '" access="RequirenteResponderTramite" class="btn btn-primary btn-sm " title="Responder Trámite"><i class="fa fa-reply"></i></a>';
        }
        return (html);
    }

    vm.dtOptions = DTOptionsBuilder.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.post('api/Tramites/Search/Requiriente', vm.filtro, data, callback);
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
        DTColumnBuilder.newColumn('EstadoSolicitudDescripcion').withTitle('Estado Solicitud').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('EstadoDescripcion').withTitle('Estado Trámite').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('SubEstadoDescripcion').withTitle('SubEstado Trámite').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('JurisdiccionInformante').withTitle('Informante').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Numero').withTitle('Nro. de Trámite').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('ServicioDescripcion').withTitle('Trámite').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('PlazoDescripcion').withTitle('Plazo').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width-s text-left').notSortable().renderWith(actionsHtml)
    ];
}