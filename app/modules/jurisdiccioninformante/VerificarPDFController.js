angular
    .module('webApp')
    .controller('verificarPDFController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'Tramite', '$sce', 'FileSaver', 'Blob', verificarPDFController]);

function verificarPDFController($scope, $routeParams, $resource, $http, $location, Tramite, $sce, FileSaver, Blob) {
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

    vm.aceptar = function () {
        $http({
            url: 'api/tramites/' + vm.tramite.Id + '/Accion/Verificacion',
            method: "PUT",
            data: { 'TramiteId': vm.tramite.Id, 'Observacion': null }
        })
        .then(function (response) {
            $location.path('/jurisdiccionInformante');
        },
        function (response) {
            alert('Error');
        });
    };

    vm.anular = function () {
        $http({
            url: 'api/tramites/' + vm.tramite.Id + '/Accion/Anulacion',
            method: "PUT",
            data: { 'TramiteId': vm.tramite.Id, 'Observacion': null }
        })
        .then(function (response) {
            $location.path('/jurisdiccionInformante');
        },
        function (response) {
            alert('Error');
        });
    };

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