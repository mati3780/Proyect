angular
    .module('webApp')
    .controller('verTramitesController', ['$scope', '$routeParams', '$resource', '$http', '$uibModal', '$location', '$filter', 'validation', '$sce', 'FileSaver', 'Blob', 'Solicitud', 'Tramite', 'Provincia', 'TiposDocumentos', 'UnidadMedida', 'Jurisdiccion', 'Profesion', verTramitesController]);

function verTramitesController($scope, $routeParams, $resource, $http, $uibModal, $location, $filter, validation, $sce, FileSaver, Blob, Solicitud, Tramite, Provincia,
								TiposDocumentos, UnidadMedida, Jurisdiccion, Profesion) {
    var vm = this;

    vm.descargarSolicitud = function () {
        FileSaver.saveAs(vm.file, 'Solicitud.pdf');
    }

    Tramite.get({ id: $routeParams.tramiteId },
        function (data) {
            vm.tramite = Solicitud.get({ id: data.SolicitudId },
                                        function () {
                                            vm.tramiteExiste = true;
                                            vm.permiteRectificar = false;

                                            vm.pdfContent = null;
                                            vm.File = null;
                                            vm.datauri = !isIE();
                                            $http.get('api/Tramites/' + $routeParams.tramiteId + '/informe', { responseType: 'arraybuffer' })
                                                           .then(function (response) {
                                                               vm.file = new Blob([response.data], { type: 'application/pdf' });
                                                               if (vm.datauri) {
                                                                   var fileUrl = URL.createObjectURL(vm.file);
                                                                   vm.pdfContent = $sce.trustAsResourceUrl(fileUrl);
                                                               }
                                                           });

                                            initTramiteController(vm, $scope, $http, $uibModal, $filter, validation, $routeParams, $location, Provincia, 
																	TiposDocumentos, UnidadMedida, Jurisdiccion, Profesion);
                                        });
        });
}