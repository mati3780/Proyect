var enums = {
	authorised: {
		authorised: 0,
		loginRequired: 1,
		notAuthorised: 2
	},
	permissionCheckType: {
		atLeastOne: 0,
		combinationRequired: 1
	}
};

var events = {
	userLoggedIn: 'auth:user:loggedIn',
	userLoggedOut: 'auth:user:loggedOut'
};

angular
	.module('app.auth', [])
	.factory('authentication', ['$rootScope', '$http', 'store',
		function ($rootScope, $http, store) {
			var getCurrentLoginUser = function() {
				    return store.get('userData');
			    },

			    isAuthenticated = function () {
				    if (getCurrentLoginUser())
					    return true;
				    else
				    	return false;
			    },

			    login = function(username, password) {
				    return $http
							.post('token', "grant_type=password&username=" + username + "&password=" + password)
							.then(function(response) {
					    		var userData = {
									userName: response.data.userName,
									givenName: response.data.givenName,
									surname: response.data.surname,
									access_token: response.data.access_token,
									aliases: response.data.aliases.split(','),
									jurisdiccion: response.data.jurisdiccion
								}
								store.set("userData", userData);
								$rootScope.$broadcast('loginStatusChanged', true, userData);
							}, function(response) {
								if (response.status === 400 && response.data.error) {
									throw response.data.error_description;
								}
							});
			    },

				logout = function() {
					store.remove('userData');
					$rootScope.$broadcast('loginStatusChanged', false, null);
				},

			    getAuthenticationToken = function () {
			    	var userData = store.get('userData');
				    if (userData)
					    return userData.access_token;
				    else
					    return null;
			    };

			return {
				getCurrentLoginUser: getCurrentLoginUser,
				isAuthenticated: isAuthenticated,
				login: login,
				logout: logout,
				getAuthenticationToken: getAuthenticationToken
			};
		}
	])
	.factory('authorization', ['authentication',
		function (authentication) {
			var authorize = function (allowAnonymous, requiredPermissions, permissionCheckType) {
				var result = enums.authorised.authorised,
				    user = authentication.getCurrentLoginUser(),
				    loweredPermissions = [],
				    hasPermission = true,
				    permission,
				    i;

				permissionCheckType = permissionCheckType || enums.permissionCheckType.atLeastOne;
				if (allowAnonymous !== true && (user === undefined || user === null)) {
					result = enums.authorised.loginRequired;
				} else if ((allowAnonymous !== true && user !== undefined && user !== null) &&
				(requiredPermissions === undefined || requiredPermissions.length === 0)) {
					// Login is required but no specific permissions are specified.
					result = enums.authorised.authorised;
				} else if (requiredPermissions) {
					loweredPermissions = [];
					angular.forEach(user.aliases, function (alias) {
						loweredPermissions.push(alias.toLowerCase());
					});

					for (i = 0; i < requiredPermissions.length; i += 1) {
						permission = requiredPermissions[i].toLowerCase();
						if (permissionCheckType === enums.permissionCheckType.combinationRequired) {
							hasPermission = hasPermission && loweredPermissions.indexOf(permission) > -1;
							// if all the permissions are required and hasPermission is false there is no point carrying on
							if (hasPermission === false) {
								break;
							}
						} else if (permissionCheckType === enums.permissionCheckType.atLeastOne) {
							hasPermission = loweredPermissions.indexOf(permission) > -1;
							// if we only need one of the permissions and we have it there is no point carrying on
							if (hasPermission) {
								break;
							}
						}
					}
					result = hasPermission ? enums.authorised.authorised : enums.authorised.notAuthorised;
				}
				return result;
			};

			return {
				authorize: authorize
			};
		}
	])
	.directive('access', ['authorization',
		function (authorization) {
			return {
				restrict: 'A',
				link: function (scope, element, attrs) {
					var makeVisible = function () {
						element.removeClass('hidden');
					},
					    makeHidden = function () {
					    	element.addClass('hidden');
					    },
					    alias = attrs.access.split(','),
					    determineVisibility = function (resetFirst) {
					    	if (resetFirst) {
					    		makeVisible();
					    	}

					    	var result = authorization.authorize(false, alias, attrs.accessPermissionType);
					    	if (result === enums.authorised.authorised) {
					    		makeVisible();
					    	} else {
					    		makeHidden();
					    	}
					    };

					if (alias.length > 0) {
						determineVisibility(true);
					}
				}
			};
		}
	])
	.factory('authInterceptor', ['store', '$location', '$q', '$rootScope', function (store, $location, $q, $rootScope) {
		var interceptor = {
			request: function (config) {
				var userData = store.get("userData");
				if (userData) {
					config.headers['Authorization'] = "Bearer " + userData.access_token;
				}
				return config;
			},
			responseError: function(response) {
				if (response.status === 401) {
					store.remove('userData');
					$rootScope.$broadcast('loginStatusChanged', false, null);
				} else if (response.status === 403) {
					$location.path('/notauthorized').replace();
				}
				return $q.reject(response);
			}
		}

		return interceptor;
	}])
	.config(['$httpProvider', function($httpProvider) {
       $httpProvider.interceptors.push('authInterceptor');
	}])
	.run(['$rootScope', '$location', 'authorization', function ($rootScope, $location, authorization) {
		$rootScope.$on('$routeChangeStart', function (event, next) {
			var authorised;
			if (next.access !== undefined) {
				authorised = authorization.authorize(next.access.allowAnonymous, next.access.permissions, next.access.permissionCheckType);
				if (authorised === enums.authorised.notAuthorised) {
					$location.path('/notauthorized').replace();
				} else if (authorised === enums.authorised.loginRequired) {
					$location.path('/login').replace();
				}
			}
		});
	}]);