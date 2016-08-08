angular
    .module('webApp')
    .controller('cuentabancariasController', ['$scope', '$http', '$compile', '$uibModal', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'CuentaBancaria',
				cuentabancariasController]);

function cuentabancariasController($scope, $http, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, CuentaBancaria) {
	var vm = this;

	vm.dtInstance = {};

	function actionsHtml(data, type, full, meta) {
		return ('<a ng-click="cuentabancariasCtrl.ver(' + data.Id + ')" class="btn btn-info btn-sm" title="Ver Cuenta Bancaria"><i class="fa fa-search-plus"></i></a>' +
				'<a ng-href="#/cuentabancarias/' + data.Id + '" access="BorrarCuentaBancaria" style="margin: 0 5px;" class="btn btn-danger btn-sm" title="Editar Cuenta Bancaria"><i class="fa fa-trash"></i></a>')
    }
	
    vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.get('api/cuentabancarias', null, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					})
                    .withOption('ordering', false)
					.withBootstrap();

	vm.dtColumns = [
                        DTColumnBuilder.newColumn('Jurisdiccion').withTitle('Jurisdicción').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn('Entidad').withTitle('Entidad').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn('Tipo').withTitle('Tipo de Cuenta').renderWith($.fn.dataTable.render.text()),
                        DTColumnBuilder.newColumn('Numero').withTitle('Número de Cuenta').renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn(null).withTitle('').withClass('column-min-width').notSortable().renderWith(actionsHtml)
	];

	vm.ver = function (cuentabancariaId) {
	    $uibModal.open({
	        templateUrl: 'modal.html',
	        controller: 'CuentaBancariaModal',
	        controllerAs: 'cuentabancariaMD',
	        size: 'lg',
	        backdrop: 'static',
	        animation: false,
	        resolve: {
	            cuentabancariaId: function () {
	                return cuentabancariaId;
	            }
	        }
	    });
	};

	vm.borrar = function (cuentabancariaId) {
		if (popupService.showPopup('¿Confirma que desea eliminar la cuenta bancaria seleccionado?')) {
			CuentaBancaria.delete({ id: cuentabancariaId },
				function() {
					alert('Se ha eliminado la cuenta bancaria seleccionada.');
					vm.dtInstance.reloadData(null, false);
				},
				function(error) {
					if (error.status === 404) {
						alert('La cuenta bancaria especificada no existe');
					} else if (error.status !== 401) {
						alert('Ocurrió un error eliminando la cuenta bancaria');
					}
				});
		};
	}
}

angular
    .module('webApp')
    .controller('CuentaBancariaModal', ['$scope', '$uibModalInstance', 'CuentaBancaria', 'cuentabancariaId', function ($scope, $uibModalInstance, CuentaBancaria, cuentabancariaId) {
        var vm = this;
        vm.feriadoInexistente = false;
        vm.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        vm.cuentabancaria = CuentaBancaria.get({ id: cuentabancariaId },
								function () { },
								function (error) {
								    if (error.status === 404) {
								        vm.feriadoInexistente = true;
								    }
								});
    }]);