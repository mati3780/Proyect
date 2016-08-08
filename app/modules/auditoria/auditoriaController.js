angular
    .module('webApp')
    .controller('auditoriaController', ['$scope', '$http', '$compile', '$uibModal', '$location', 'DTOptionsBuilder', 'DTColumnBuilder', 'dataTableHttpService', 
										'Auditoria', auditoriaController]);

function auditoriaController($scope, $http, $compile, $uibModal, $location, DTOptionsBuilder, DTColumnBuilder, dataTableHttpService, Auditoria) {
	var vm = this;

	vm.dtInstance = {};

	vm.usuarios = Auditoria.usuarios();

	vm.urls = Auditoria.urls();

	vm.navegadores = Auditoria.navegadores();

	vm.entidades = Auditoria.entidades();

	function actionsHtml(data, type, full, meta) {
		return ('<a ng-click="auditoriaCtrl.ver(' + data.Id + ')" class="btn btn-info btn-sm" title="Ver operación"><i class="fa fa-search-plus"></i></a>');
	}

	vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function(data, callback, settings) {
						dataTableHttpService.post('api/auditoria', vm.filtro, data, callback);
					})
					.withDataProp('data')
					.withOption('deferLoading', 0)
					.withOption('createdRow', function (row, data, dataIndex) {
						$compile(angular.element(row).contents())($scope);
					})
					.withBootstrap();

	vm.dtColumns = [
						DTColumnBuilder.newColumn('Id')
											.withTitle('Id')
											.withOption('width', '15%'),
						DTColumnBuilder.newColumn('FechaOperacion')
											.withTitle('Fecha')
											.withOption('width', '15%'),
						DTColumnBuilder.newColumn('Url')
											.withTitle('Url')
											.withOption('width', '25%'),
						DTColumnBuilder.newColumn('Usuario')
											.withTitle('Usuario')
											.withOption('width', '25%'),
						DTColumnBuilder.newColumn('Navegador')
											.withTitle('Navegador')
											.withOption('width', '25%'),
						DTColumnBuilder.newColumn('Ip')
											.withTitle('Ip')
											.withOption('width', '25%'),
						DTColumnBuilder.newColumn(null)
											.withTitle('')
											.notSortable()
											.withOption('width', '25%')
											.renderWith(actionsHtml)
	];

	vm.filtrar = function() {
		vm.dtInstance.reloadData();
	}

	vm.limpiar = function() {
		vm.filtro = null;
	}

	vm.ver = function (operacionId) {
		$uibModal.open({
			templateUrl: 'auditoriaModal.html',
			controller: 'AuditoriaModal',
			controllerAs: 'auditoriaMD',
			size: 'lg',
			backdrop: 'static',
			animation: false,
			resolve: {
				operacionId: function () {
					return operacionId;
				}
			}
		});
	};

}

angular
    .module('webApp')
    .controller('AuditoriaModal', ['$scope', '$http', '$uibModalInstance', 'Auditoria', 'operacionId',
		function ($scope, $http, $uibModalInstance, Auditoria, operacionId) {
			var vm = this;
			vm.operacionInexistente = false;
			vm.close = function () {
				$uibModalInstance.dismiss('cancel');
			};
			
			vm.operacion = Auditoria.get({ id: operacionId }, function(){}, 
											function(error) {
												if (error.status === 404) {
													vm.operacionInexistente = true;
												}
											});
		}]);
