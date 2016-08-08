angular
    .module('webApp')
    .controller('verPDFRechazadoController', ['$scope', '$routeParams', '$resource', '$http', 'Tramite', '$sce', 'FileSaver', 'Blob', verPDFRechazadoController]);

function verPDFRechazadoController($scope, $routeParams, $resource, $http, Tramite, $sce, FileSaver, Blob) {
    var vm = this;

    vm.tramite = Tramite.get({ id: $routeParams.tramiteId },
        function () {
            vm.tramiteExiste = true;
            $scope.$parent.appCtrl.subtitulo = 'Número: ' + vm.tramite.Numero;
        },
        function (error) {
            vm.tramiteInexistente = true;
            alert(error);
        });

    vm.datauri = !isIE();

    $http.get('api/Tramites/' + $routeParams.tramiteId + '/informe', { responseType: 'arraybuffer' })
           .then(function (response) {
               vm.file = new Blob([response.data], { type: 'application/pdf' });
               if (vm.datauri) {
                   var fileUrl = URL.createObjectURL(vm.file);
                   vm.pdfContent = $sce.trustAsResourceUrl(fileUrl);
               }
           }, function (errorResponse) {
               alert(errorResponse);
           });

    vm.descargarSolicitud = function () {
        FileSaver.saveAs(vm.file, 'Solicitud.pdf');
    }
};