angular
	.module('app.validation', [])
	.factory('validation', function() {
		var validate = function(scope, errorResponse) {
			var data = errorResponse.data;

			scope.validationErrors = {
				formErrors: [],
				elementErrors: {}
			}

			if (errorResponse.status === 400 && data.ModelState) {
				for(var key in data.ModelState)
					if (key === "") {
						scope.validationErrors.formErrors = data.ModelState[key];
					} else {
						scope.validationErrors.elementErrors[key] = data.ModelState[key];
					}
			}
		}

		return {
			validate: validate
		};
	})
	.directive('validator', function() {
		return {
			restrict: 'AE',
			controller: ['$scope', function ($scope) {
			}],
			scope: {
				fieldname: '@',
				elementErrors: '@'
			},
			link: function (scope, element, attrs, parentCtrls) {
				//var elementController = element.controller();
				//elementController.$watch('validationErrors', function(newVal, oldVal) {
				//	elementController.
				//});
				scope.$parent.$watch('errors', function(newVal, oldVal) {
					scope.validationErrors.elementErrors = newVal.elementErrors;
				});
			},
			transclude: true,
			replace: true,
			template: '<span class="help-block has-error" ng-if="elementErrors[fieldname]">{{fieldname}}{{elementErrors[fieldname]}}</span>'
		};
	});