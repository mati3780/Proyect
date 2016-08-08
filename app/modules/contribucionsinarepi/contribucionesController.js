angular
    .module('webApp')
    .controller('contribucionesController', ['$scope', '$http', '$compile', '$uibModal', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'ContribucionProyect',
				contribucionesController]);

function contribucionesController($scope, $http, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, ContribucionProyect) {
	var vm = this;

	vm.dtInstance = {};
	
	vm.VigenciaFechaDesde = ContribucionProyect.obtenerFechaDesde();

	function actionsHtml(data, type, full, meta) {
	    return ('<a ng-click="contribucionesCtrl.ver(' + data.Id + ')" class="btn btn-info btn-sm" title="Ver Contribucion PROYECT"><i class="fa fa-search-plus"></i></a>'+
				(data.FechaHasta == null? '<a ng-href="#/contribuciones/' + data.Id + '" access="ModificarContribucionPROYECT" style="margin: 0 5px;" class="btn btn-danger btn-sm" title="Editar Contribucion PROYECT"><i class="fa fa-trash"></i></a>' : ''))
    }
	
    vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.get('api/contribucionsinarepi', null, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					})
                    .withOption('ordering', false)
					.withBootstrap();

	vm.dtColumns = [
        DTColumnBuilder.newColumn('Valor').withTitle('Valor PROYECT').withClass('text-center').renderWith(function (data) {
                        																			return '$ ' + data.toFixed(2);
																								}),
		DTColumnBuilder.newColumn('FechaDesde').withTitle('Fecha Vigencia Desde').withClass('text-center').renderWith($.fn.dataTable.render.text()),
		DTColumnBuilder.newColumn('FechaHasta').withTitle('Fecha Vigencia Hasta').withClass('text-center').renderWith($.fn.dataTable.render.text()),
		DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width').notSortable().renderWith(actionsHtml)
	];

	vm.ver = function (contribucionId) {
	    $uibModal.open({
	        templateUrl: 'modal.html',
	        controller: 'ContribucionProyectModal',
	        controllerAs: 'contribucionsinarepiMD',
	        size: 'lg',
	        backdrop: 'static',
	        animation: false,
	        resolve: {
	            contribucionId: function () {
	                return contribucionId;
	            }
	        }
	    });
	};

	vm.borrar = function (contribucionId) {
		if (popupService.showPopup('¿Confirma que desea eliminar la contribución PROYECT seleccionado?')) {
		    ContribucionProyect.delete({ id: contribucionId },
				function() {
				    alert('Se ha eliminado la contribución PROYECT seleccionada.');
					vm.dtInstance.reloadData(null, false);
				},
				function(error) {
					if (error.status === 404) {
					    alert('La contribución PROYECT especificada no existe');
					}
				});
		};
	}
}

angular
    .module('webApp')
    .controller('ContribucionProyectModal', ['$scope', '$uibModalInstance', 'ContribucionProyect', 'contribucionId', function ($scope, $uibModalInstance,
													ContribucionProyect, contribucionId) {
        var vm = this;
        vm.contribucionsinarepiInexistente = false;
        vm.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        vm.contribucionsinarepi = ContribucionProyect.get({ id: contribucionId },
								function () { },
								function (error) {
								    if (error.status === 404) {
								        vm.contribucionsinarepiInexistente = true;
								    }
								});
    }]);