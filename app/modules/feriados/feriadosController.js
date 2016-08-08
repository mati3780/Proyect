angular
    .module('webApp')
    .controller('feriadosController', ['$scope', '$http', '$compile', '$uibModal', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'Feriado',
				feriadosController]);

function feriadosController($scope, $http, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, Feriado) {
	var vm = this;

	vm.VerFeriadosNacionales = 1;

	vm.dtInstance = {};
	vm.dtInstanceJurisdiccionales = {};

	function actionsHtmlPROYECT(data, type, full, meta) {
		return ('<a ng-click="feriadosCtrl.ver(' + data.Id + ')" class="btn btn-info btn-sm" title="Ver feriado"><i class="fa fa-search-plus"></i></a>' +
				'<a ng-href="#/feriados/' + data.Id + '" access="PROYECT" style="margin: 0 5px;" class="btn btn-warning btn-sm" title="Editar feriado"><i class="fa fa-pencil"></i></a>' +
				'<a ng-click="feriadosCtrl.borrar(' + data.Id + ')" access="PROYECT" class="btn btn-danger btn-sm" title="Eliminar Feriado"><i class="fa fa-trash"></i></a>');
	}
	
	vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function(data, callback, settings) {
					    dataTableHttpService.get('api/feriados/Search/true', null, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
						$compile(angular.element(row).contents())($scope);
					})
                    .withOption('ordering', false)
					.withBootstrap();

	vm.dtColumnsPROYECT = [
						DTColumnBuilder.newColumn('Fecha').withTitle('Fecha').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn('Descripcion').withTitle('Descripción').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width').notSortable().renderWith(actionsHtmlPROYECT)
	];
    
	function actionsHtml(data, type, full, meta) {
	    return ('<a ng-click="feriadosCtrl.ver(' + data.Id + ')" class="btn btn-info btn-sm" title="Ver feriado"><i class="fa fa-search-plus"></i></a>' +
				'<a ng-href="#/feriados/' + data.Id + '" access="ModificarFeriado" style="margin: 0 5px;" class="btn btn-warning btn-sm" title="Editar feriado"><i class="fa fa-pencil"></i></a>' +
				'<a ng-click="feriadosCtrl.borrar(' + data.Id + ')" access="BorrarFeriado" class="btn btn-danger btn-sm" title="Eliminar Feriado"><i class="fa fa-trash"></i></a>');
	}

    vm.dtOptionsJurisdiccionales = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.get('api/feriados/Search/false', null, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					})
                    .withOption('ordering', false)
					.withBootstrap();

    vm.dtColumns = [
                    DTColumnBuilder.newColumn('Fecha').withTitle('Fecha').renderWith($.fn.dataTable.render.text()),
                    DTColumnBuilder.newColumn('Descripcion').withTitle('Descripción').renderWith($.fn.dataTable.render.text()),
                    DTColumnBuilder.newColumn(null).withTitle('').notSortable().renderWith(actionsHtml)
    ];

	vm.ver = function (feriadoId) {
		$uibModal.open({
			templateUrl: 'modal.html',
			controller: 'FeriadoModal',
			controllerAs: 'feriadoMD',
			size: 'lg',
			backdrop: 'static',
			animation: false,
			resolve: {
				feriadoId: function () {
					return feriadoId;
				}
			}
		});
	};

	vm.borrar = function (feriadoId) {
		if (popupService.showPopup('¿Confirma que desea eliminar el feriado seleccionado?')) {
			Feriado.delete({ id: feriadoId },
				function() {
					alert('Se ha eliminado al feriado seleccionado.');
					vm.dtInstance.reloadData(null, false);
					vm.dtInstanceJurisdiccionales.reloadData(null, false);
				},
				function(error) {
					if (error.status === 404) {
						alert('El feriado especificado no existe.');
					} else if (error.status !== 401) {
					    alert('No puede eliminar feriados anteriores a la fecha actual o dentro del plazo de días para los trámites.');
					}
				});
		};
	}
}

angular
    .module('webApp')
    .controller('FeriadoModal', ['$scope', '$uibModalInstance', 'Feriado', 'feriadoId', function ($scope, $uibModalInstance, Feriado, feriadoId) {
		var vm = this;
		vm.feriadoInexistente = false;
		vm.close = function () {
			$uibModalInstance.dismiss('cancel');
		};

		vm.feriado = Feriado.get({ id: feriadoId },
								function () { },
								function (error) {
									if (error.status === 404) {
										vm.feriadoInexistente = true;
									}
								});
	}]);
