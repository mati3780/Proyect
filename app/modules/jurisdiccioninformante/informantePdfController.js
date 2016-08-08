angular
    .module('webApp')
    .controller('informantePdfController', ['$scope', '$routeParams', '$http', '$sce', 'FileSaver', 'Blob', 'Tramite', informantePdfController]);

function informantePdfController($scope, $routeParams, $http, $sce, FileSaver, Blob, Tramite) {
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
    $http.get('api/tramites/report/' + $routeParams.tramiteId, { responseType: 'arraybuffer' })
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