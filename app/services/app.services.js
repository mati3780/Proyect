angular.module('app.services', [])
	.service('popupService', ['$window', function($window) {
		this.showPopup = function(message) {
			return $window.confirm(message);
		}
	}])
	.service('dataTableHttpService', ['$http', function($http) {
		this.get = function(url, params, data, callback) {
			$http.get(url, {
				params: $.extend(data, 
								{
									length: data.length,
									start: data.start
								},
								params),
				cache: false
			}).then(function (res) {
				// map your server's response to the DataTables format and pass it to
				// DataTables' callback
				callback({
					recordsTotal: res.data.recordsTotal,
					recordsFiltered: res.data.recordsFiltered,
					data: res.data.data
				});
			});
		}

		this.post = function (url, params, data, callback) {
			$http.post(url, 
					   $.extend(data,
								params),
						{ cache: false })
			.then(function (res) {
				// map your server's response to the DataTables format and pass it to
				// DataTables' callback
				callback({
					recordsTotal: res.data.recordsTotal,
					recordsFiltered: res.data.recordsFiltered,
					data: res.data.data
				});
			});
		}
	}]);