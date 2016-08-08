/*
 http://blog.yodersolutions.com/bootstrap-form-validation-done-right-in-angularjs/
 */

var module = angular.module('validationErrors', []);

module.directive('showErrors', ['$timeout', 'showErrorsConfig', function ($timeout, showErrorsConfig) {
	var getShowSuccess = function (options) {
		var showSuccess;
		showSuccess = showErrorsConfig.showSuccess;
		if (options && options.showSuccess !== null) {
			showSuccess = options.showSuccess;
		}
		return showSuccess;
	};
	var linkFn = function (scope, el, attrs, formCtrl) {
		var toggleClasses;
		var blurred = false;
		var options = scope.$eval(attrs.showErrors);
		var showSuccess = getShowSuccess(options);
		var inputElements = Array.prototype.slice.call(el[0].querySelectorAll('[name]'));
		//Si el nombre del formulario está separado por varios puntos (controller.formName o controller.parentFormName.formName), entonces toma siempre el último nombre
		var elFormName = formCtrl.$name.split('.').length > 1 ? formCtrl.$name.split('.')[formCtrl.$name.split('.').length - 1] : formCtrl.$name;
		inputElements.forEach(function (inputEl) {
			var inputNgEl = angular.element(inputEl);
			//var inputName = inputNgEl.attr('name');
			if (!inputNgEl.attr('name')) {
				throw 'show-errors element has no child input elements with a \'name\' attribute';
			}
			inputNgEl.bind('blur', function () {
				blurred = true;
				return toggleClasses(formCtrl[inputNgEl.attr('name')].$invalid);
			});
			scope.$watch(function () {
				return formCtrl[inputNgEl.attr('name')] && formCtrl[inputNgEl.attr('name')].$invalid;
			}, function (invalid) {
				if (!blurred) {
					return;
				}
				return toggleClasses(invalid);
			});

			scope.$on('show-errors-check-validity', function (event, formName) {
				if (elFormName === formName) {
					return toggleClasses(formCtrl[inputNgEl.attr('name')].$invalid);
				}
			});
		});

		scope.$on('show-errors-reset', function (formName) {
			return $timeout(function () {
				if (elFormName === formName)
					el.removeClass('has-error');
				el.removeClass('has-success');
				return blurred === false;
			}, 0, false);
		});
		return toggleClasses = function (invalid) {
			el.toggleClass('has-error', invalid);
			if (showSuccess) {
				return el.toggleClass('has-success', !invalid);
			}
		};
	};
	return {
		restrict: 'A',
		require: '^form',
		compile: function (elem, attrs) {
			if (!elem.hasClass('form-group')) {
				throw 'show-errors element does not have the \'form-group\' class';
			}
			return linkFn;
		}
	};
}]);

module.provider('showErrorsConfig', function () {
	var _showSuccess = false;
	this.showSuccess = function (showSuccess) {
		return _showSuccess === showSuccess;
	};
	this.$get = function () {
		return { showSuccess: _showSuccess };
	};
});
