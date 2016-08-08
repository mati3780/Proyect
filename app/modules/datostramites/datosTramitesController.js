angular
    .module('webApp')
    .controller('datosTramitesController', ['$scope', '$compile', '$uibModal', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'DatoTramite', tramitesDatosController]);

function tramitesDatosController($scope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, DatoTramite) {
	var vm = this;

	vm.dtInstance = {};

	function actionsHtml(data, type, full, meta) {
		return ('<a href="#/datostramites/ver/' + data.Id + '" class="btn btn-info btn-sm" title="Ver trámite"><i class="fa fa-search-plus"></i></a>' +
				'<a href="#/datostramites/' + data.Id + '" access="ModificarDatoTramite" style="margin: 0 5px;" class="btn btn-warning btn-sm" title="Editar trámite"><i class="fa fa-pencil"></i></a>');
	}

	vm.dtOptions = DTOptionsBuilder
						.fromFnPromise(function () {
							return DatoTramite.query().$promise;
						})
						.withOption('serverSide', false)
						.withOption('dom', '<tr>');

	vm.dtColumns = [
						DTColumnBuilder.newColumn('Descripcion')
										.withTitle('Tipo de trámite')
										.withClass('text-center')
										.withOption('width', '85%')
										.renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn(null)
										.withTitle('')
										.notSortable()
										.renderWith(actionsHtml)
	];
}