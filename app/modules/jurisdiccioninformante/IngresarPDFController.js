angular
    .module('webApp')
    .controller('ingresarPDFController', ['$scope', '$location', 'authentication', 'validation', '$uibModalInstance', 'FileUploader', 'tramiteId', 
										ingresarPDFController]);

function ingresarPDFController($scope, $location, authentication, validation, $uibModalInstance, FileUploader, tramiteId) {
    var vm = this;
    //angular-file-upload
    var headers = { 'Authorization': '' };
    headers['Authorization'] = "Bearer " + authentication.getCurrentLoginUser().access_token;

    var uploader = $scope.uploader = new FileUploader({
        url: 'api/tramites/' + tramiteId + '/Informar',
        method: "PUT",
        headers: headers
    });

    //VALIDAR EXTENSION
    uploader.filters.push({
        name: 'extensionFilter',
        fn: function (item, options) {
            var filename = item.name;
            var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
            if (extension === "pdf") {
                vm.archivoFormatoIncorrecto = false;
                return true;
            }
            else {
                vm.archivoErrorRequired = false;
                vm.archivoFormatoIncorrecto = true;
                return false;
            }
        }
    });

    //VALIDAR TAMAÑO
    uploader.filters.push({
        name: 'sizeFilter',
        fn: function (item, options) {
            var fileSize = item.size;
            fileSize = parseInt(fileSize) / 1048576;
            if (fileSize <= 30) {
                vm.archivoSizeIncorrecto = false;
                return true;
            }

            else {
                vm.archivoErrorRequired = false;
                vm.archivoSizeIncorrecto = true;
                return false;
            }
        }
    });
    //FIN:angular-file-upload

    vm.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    };

    vm.aceptar = function () {
        vm.archivoErrorRequired = document.getElementById("archivo").files.length === 0;
        if (vm.archivoErrorRequired || vm.archivoFormatoIncorrecto || vm.archivoSizeIncorrecto) {
            return;
        }
        uploader.uploadAll();
    };

    uploader.onSuccessItem = function (fileItem, response, status, headers) {
        $uibModalInstance.dismiss('cancel');
        $location.path('/jurisdiccionInformante');
    };
    uploader.onErrorItem = function (fileItem, response, status, headers) {
        if (status === 400) {
            validation.validate(vm, { status: status, data: response });
        }

    };
}