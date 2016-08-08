angular
    .module('webApp')
    .controller('compensacionesController', ['$scope', '$compile', '$uibModal', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'authentication', 'Jurisdiccion',compensacionesController]);

function compensacionesController($scope, $compile, $uibModal, $http, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, authentication, Jurisdiccion) {
    var vm = this;

    vm.esJurisdisccion = authentication.getCurrentLoginUser().aliases.indexOf('JURISDICCION') !== -1;

    vm.esProyect = authentication.getCurrentLoginUser().aliases.indexOf('PROYECT') !== -1;
    alert(vm.esProyect);
    alert(vm.esJurisdisccion);

    vm.jurisdicciones = Jurisdiccion.query();
    
    vm.dtInstance = {};

    function actionsHtml(data, type, full, meta) {
        return ('<a ng-click="compensacionesCtrl.ver(' + data.Id + ')" class="btn btn-info btn-sm" title="Ver Detalles Compensación"><i class="fa fa-search-plus"></i></a>');
    }

    vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.post('api/Compensaciones/Search', vm.filtro, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					    angular.element(row).addClass(addClassSemaforo(data.EstadoSemaforo));
					})
					.withBootstrap();

    vm.dtColumns = [
        DTColumnBuilder.newColumn('Fecha').withTitle('Fecha Compensación').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Importe').withTitle('Importe Compensación').withClass('text-right').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('Jurisdiccion').withTitle('Jurisdicción').withClass('text-right').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn('FechaDeposito').withTitle('Fecha Depósito').withClass('text-right').renderWith($.fn.dataTable.render.text()),
        DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width text-left').notSortable().renderWith(actionsHtml)
    ];

    vm.buscar = function () {
        vm.dtInstance.reloadData();
    }
    vm.ver = function (compensacionId) {
        $uibModal.open({
            templateUrl: 'modal.html',
            controller: 'compensacionModal',
            controllerAs: 'compensacionMD',
            size: 'lg',
            backdrop: 'static',
            animation: false,
            resolve: {
                compensacionId: function () {
                    return compensacionId;
                }
            }
        });
    };
}

angular
    .module('webApp')
    .controller('compensacionModal', ['$scope', '$uibModalInstance', 'Compensacion', 'compensacionId', function($scope, $uibModalInstance, Compensacion, compensacionId) {
        var vm = this;
        vm.jurisdiccionInexistente = false;
        vm.close = function() {
            $uibModalInstance.dismiss('cancel');
        };
        vm.compensacion = compensacionId;
        vm.datos = Compensacion.detalles({ id: compensacionId });
    }]);