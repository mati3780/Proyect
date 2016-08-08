angular
    .module('webApp')
    .controller('consultaTramitesController', ['$scope', '$compile', '$uibModal', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService',
					'dataTableHttpService', 'authentication', 'store', 'TiposDocumentos', 'Jurisdiccion', 'TramiteEstado', 'TipoEstadoSolicitud', consultaTramitesController]);

function consultaTramitesController($scope, $compile, $uibModal, $http, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, authentication,
									store, TiposDocumentos, Jurisdiccion, TramiteEstado, TipoEstadoSolicitud) {
    var vm = this;
    var username = authentication.getCurrentLoginUser().userName;

    //FUNCIONES
    function actionsHtml(data, type, full, meta) {
        var html = '<a ng-href="#/consultorTramites/' + data.Id + '/ver" access="ConsultorTramites" class="btn btn-info btn-sm margin-right-xs" title="Ver Trámite"><i class="fa fa-search-plus"></i></a>';
        return (html);
    }

    vm.filtro = {
        TipoPersona: null,
        CodigoBarra: null,
        TipoDocumento: null,
        NumeroDocumento: null,
        Jurisdiccion: null,
        TipoJurisdiccion: null,
        Estado: null,
        SubEstado: null,
        ApellidoDenominacion: null,
        EstadoSolicitud: null
    }

    vm.ErrorBusqueda = false;

    vm.tipoDocumentos = TiposDocumentos.query();

    vm.jurisdicciones = Jurisdiccion.query(function (data) {
        if (data.length === 1) {
            vm.filtro.Jurisdiccion = data[0].Id;
        }
    });

    vm.tiposPersonas = [
        {
            Id: true,
            Descripcion: 'Persona Humana'
        },
        {
            Id: false,
            Descripcion: 'Persona Jurídica'
        }
    ];

    vm.tipoJurisdicciones = [
        {
            Id: true,
            Descripcion: 'Jurisdicción Requirente'
        },
        {
            Id: false,
            Descripcion: 'Jurisdicción Informante'
        }
    ];
    vm.filtro.TipoJurisdiccion = vm.tipoJurisdicciones[0].Id;
	
    vm.estadosSolicitud = TipoEstadoSolicitud.query();

    vm.estados = TramiteEstado.getTipoEstados();

    vm.subEstados = {};

    vm.fillSelectSubEstados = function () {
        if (vm.filtro.Estado != null) {
            vm.subEstados = TramiteEstado.getTipoSubEstados({ tipoEstadoId: vm.filtro.Estado });
        } else {
            vm.filtro.SubEstado = null;
            vm.subEstados = {};
        }
    };

    vm.fillApellidoDenominacion = function () {
        vm.filtro.ApellidoDenominacion = null;
    }

    vm.buscar = function () {
        $scope.$broadcast('show-errors-check-validity', "consultaForm");
        if (vm.consultaForm.$valid) {
            vm.ErrorBusqueda = false;
            vm.dtInstance.reloadData();
            store.set("busquedaTramites-" + username, vm.filtro);
        } else {
            vm.ErrorBusqueda = true;
        }
    }

    vm.limpiar = function () {
        if (vm.jurisdicciones.length > 1) {
            vm.filtro.Jurisdiccion = null;
        }
        vm.filtro.CodigoBarra = null;
        vm.filtro.TipoDocumento = null;
        vm.filtro.NumeroDocumento = null;
        vm.filtro.TipoJurisdiccion = vm.tipoJurisdicciones[0].Id;
        vm.filtro.Estado = null;
        vm.filtro.EstadoSolicitud = null;
        vm.filtro.SubEstado = null;
        vm.filtro.TipoPersona = null;
        vm.filtro.ApellidoDenominacion = null;
        vm.ErrorBusqueda = false;
        store.remove("busquedaTramites-" + username);
    }

    vm.dtInstance = {};

    var deferLoading = true;
    var busquedaData = store.get("busquedaTramites-" + username);
    if (busquedaData) {
        vm.filtro = busquedaData;
        deferLoading = false;
    }

    vm.dtOptions = DTOptionsBuilder.newOptions()
                                        .withOption('ajax', function (data, callback, settings) {
                                            dataTableHttpService.post('api/Tramites/SearchAdminEnte', vm.filtro, data, callback);
                                        })
                                        .withDataProp('data')
                                        .withOption('deferLoading', deferLoading ? 0 : null)
                                        .withOption('createdRow', function (row, data, dataIndex) {
                                            $compile(angular.element(row).contents())($scope);
                                        })
                                        .withBootstrap();
    vm.dtColumns = [
                        DTColumnBuilder.newColumn('Numero').withTitle('N° de Trámite').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('FechaTramite').withTitle('Fecha Inicio').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('FechaMaxEntrega').withTitle('Fecha Máx. Entrega').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('FechaEstado').withTitle('Plazo').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('EstadoSolicitud').withTitle('Estado Solicitud').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('Estado').withTitle('Estado Trámite').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('SubEstado').withTitle('SubEstado Trámite').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('JurisdiccionRequirente').withTitle('Requirente').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('JurisdiccionInformante').withTitle('Informante').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width text-left').notSortable().renderWith(actionsHtml)
    ];
}