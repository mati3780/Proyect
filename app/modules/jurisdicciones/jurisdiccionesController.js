angular
    .module('webApp')
    .controller('jurisdiccionesController', ['$scope', '$compile', '$uibModal', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', 'popupService', 'dataTableHttpService', 'authentication', 'Jurisdiccion', 'Provincia', 'TipoEstadoAdhesion', jurisdiccionesController]);

function jurisdiccionesController($scope, $compile, $uibModal, $http, DTOptionsBuilder, DTColumnBuilder, popupService, dataTableHttpService, authentication,
									Jurisdiccion, Provincia, TipoEstadoAdhesion) {
    var vm = this;

    vm.esProyect = authentication.getCurrentLoginUser().aliases.indexOf('PROYECT') !== -1;

    //METODOS
    if (vm.esProyect) {
        vm.fillSelectProvincias = function () {
	        vm.provincias = Provincia.query(function() {}, function(error) {
				alert('Error al obtener las provincias.');
	        });
        };

        vm.fillSelectTipoEstadoAdhesiones = function () {
	        vm.tipoEstadoAdhesiones = TipoEstadoAdhesion.query(function() {}, function(error) {
		        vm.tipoEstadoAdhesiones = [];
		        alert('Error al obtener los Tipos Estados Adhesiones.');
	        });
        };
        
        vm.fillSelectProvincias();
        vm.fillSelectTipoEstadoAdhesiones();
    }
    //FIN:METODOS

    

    vm.dtInstance = {};

    function actionsHtml(data, type, full, meta) {
        return ('<a ng-click="jurisdiccionesCtrl.ver(' + data.Id + ')" class="btn btn-info btn-sm" title="Ver jurisdicción"><i class="fa fa-search-plus"></i></a>' +
				'<a ng-href="#/jurisdicciones/' + data.Id + '" access="ModificarJurisdiccion" style="margin: 0 5px;" class="btn btn-warning btn-sm" title="Editar jurisdicción"><i class="fa fa-pencil"></i></a>');
    }

    vm.dtOptions = DTOptionsBuilder
					.newOptions()
					.withOption('ajax', function (data, callback, settings) {
					    dataTableHttpService.post('api/Jurisdicciones/Search', vm.filtro, data, callback);
					})
					.withDataProp('data')
					.withOption('createdRow', function (row, data, dataIndex) {
					    $compile(angular.element(row).contents())($scope);
					})
					.withBootstrap();

    vm.dtColumns = [
						DTColumnBuilder.newColumn('Descripcion')
											.withTitle('Jurisdicción')
											.renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn('ProvinciaDescripcion')
											.withTitle('Provincia')
											.renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn('TipoEstadoAdhesionDescripcion')
											.withTitle('Estado Adhesión')
											.renderWith($.fn.dataTable.render.text()),
						DTColumnBuilder.newColumn(null)
											.withTitle('')
                                            .withClass('column-min-width-s')
											.renderWith(actionsHtml)
    ];

    vm.ver = function (jurisdiccionId) {
        $uibModal.open({
            templateUrl: 'modal.html',
            controller: 'JurisdiccionModal',
            controllerAs: 'jurisdiccionMD',
            size: 'lg',
            backdrop: 'static',
            animation: false,
            resolve: {
                jurisdiccionId: function () {
                    return jurisdiccionId;
                }
            }
        });
    };

    vm.buscar = function () {
        vm.dtInstance.reloadData();
    }
}

angular
    .module('webApp')
    .controller('JurisdiccionModal', ['$scope', '$uibModalInstance', 'Jurisdiccion', 'jurisdiccionId', function ($scope, $uibModalInstance, Jurisdiccion, jurisdiccionId) {
        var vm = this;
        vm.jurisdiccionInexistente = false;
        vm.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        vm.jurisdiccion = Jurisdiccion.get({ id: jurisdiccionId },
								function () { },
								function (error) {
								    if (error.status === 404) {
								        vm.jurisdiccionInexistente = true;
								    }
								});
    }]);