angular
    .module('webApp', ['ngSanitize', 'ngRoute', 'angular-storage', 'ngFileSaver', 'ngResource', 'ui.bootstrap', 'ngBusy', 'angularFileUpload', 'ngFileUpload', 'app.auth', 'app.validation',
			'app.services', 'datatables', 'datatables.bootstrap', 'dateTimePicker', 'smartmenus', 'validationErrors', 'cuitValidator',
			'jurisdiccionServices', 'plazoServices', 'feriadoServices', 'cuentabancariaServices', 'datoTramiteServices',
            'depositoServices', 'depositoPendienteServices', 'servicioServices', 'contribucionsinarepiServices', 'tramiteServices', 'solicitudServices', 
			'tiposDocumentosServices', 'tramiteEstadoServices', 'liquidacionServices', 'tipoCuentaBancariaServices', 'provinciaServices', 
			'tipoEstadoAdhesionServices', 'profesionesServices', 'unidadesMedidaServices','tipoEstadoSolicitudServices','compensacionServices']);

angular
    .module('webApp')
    .controller('appController', ['$rootScope', '$scope', '$route', '$location', '$uibModal', 'authentication', appController]);

angular
	.module('webApp')
	.factory('errorHandler', [ '$q', '$rootScope', function($q, $rootScope) {
		var interceptor = {
			responseError: function(response) {
				if (response.status === 500) {
					$rootScope.$broadcast('serverError.500', response);
				}
				return $q.reject(response);
			}
		}
		return interceptor;
	}]);

angular
   .module('webApp')
   .config(['$httpProvider', function ($httpProvider) {
       //initialize get if not there
       if (!$httpProvider.defaults.headers.get) {
           $httpProvider.defaults.headers.get = {};
       }
       $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
       $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

	   $httpProvider.interceptors.push('errorHandler');
   }]);

function appController($rootScope, $scope, $route, $location, $uibModal, authentication) {
    var vm = this;

    vm.isAuthenticated = authentication.isAuthenticated();
    vm.currentUser = authentication.getCurrentLoginUser();

    if (!vm.isAuthenticated) {
        $location.path('/login');
    }

    vm.logout = function () {
        authentication.logout();
    };

    $rootScope.$on('$routeChangeStart', function (event, next) {
        vm.titulo = next.titulo;
        vm.subtitulo = next.subtitulo;
    });

    $scope.$on('loginStatusChanged', function (event, isAuthenticated, userData) {
        vm.isAuthenticated = isAuthenticated;
        vm.currentUser = userData;
        if (isAuthenticated) {
            $location.path('/');
        } else {
            $location.path('/login');
        }
    });

	var modal;
    $scope.$on('busy.begin', function (event, config) {
    	if (!modal && config.url.toLowerCase().indexOf('api/') >= 0) {
    		modal = $uibModal.open({
    			animation: true,
    			templateUrl: 'busyModal.html',
    			controller: ['$scope', '$uibModalInstance', function ($scope, $uibModalInstance) {
    				$scope.$on('busy.end', function (evnt, cfg) {
    					if (cfg.remaining === 0) {
    						$uibModalInstance.dismiss('cancel');
    						modal = null;
    					}
    				});
    			}],
    			size: 'sm',
    			backdrop: 'static'
    		});
    	}
    });

	var errorModal;
	var showError = true;
	$rootScope.$on('serverError.500', function (event, errorResponse) {
		if (!errorModal) {
			errorModal = $uibModal.open({
				animation: true,
				templateUrl: 'errorModal.html',
				controller: ['$scope', '$uibModalInstance', 'errorResponse', function ($scope, $uibModalInstance, errorResponse) {
					var errorVm = this;
					errorVm.showError = showError;
					errorVm.error = showError ? errorResponse : null;
					errorVm.close = function () {
						$uibModalInstance.dismiss('cancel');
						errorModal = null;
					}
				}],
				controllerAs: 'errorMD',
				size: showError ? 'lg' : '',
				backdrop: 'static',
				resolve: {
						errorResponse: function() {
							return errorResponse;
					}
				}
			});
		}
	});
}