angular
    .module('webApp')
    .controller('loginController', ['authentication', '$location', '$scope', loginController]);

function loginController(authentication, $location, $scope) {
	var vm = this;

	vm.username = '';
	vm.password = '';
	vm.error = '';

	if (authentication.isAuthenticated()) {
		$location.path('/');
	}
	
	vm.login = function () {
		vm.error = "";
		$scope.$broadcast('show-errors-check-validity', "form");
		if (vm.form.$valid) {
			authentication.login(vm.username, vm.password)
				.then(function () {
				}, function(errorMsg) {
					vm.error = errorMsg;
				});
		}
	}
}