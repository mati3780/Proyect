function initTramiteController(vm, $scope, $http, $uibModal, $filter, validation, $routeParams, $location, Provincia, TiposDocumentos, UnidadMedida, Jurisdiccion, 
								Profesion) {
	$scope.$parent.appCtrl.subtitulo = 'Número: ' + vm.tramite.Numero;

	//RECTIFICAR
	vm.rectificarSolicitante = function () {
		vm.tramite.EsRectificacionSolicitante = true;
	}
	vm.anularRectificacionSolicitante = function () {
		vm.tramite.EsRectificacionSolicitante = false;
		vm.tramite.Solicitante = clone(vm.solicitanteOriginal);
	}

	vm.rectificarPersona = function () {
		vm.tramite.EsRectificacionPersona = true;
	}
	vm.anularRectificacionPersona = function () {
		vm.tramite.EsRectificacionPersona = false;
		vm.tramite.Entidad.Persona = clone(vm.entidadPersonaOriginal);
	}

	vm.rectificarInmueble = function () {
		vm.tramite.EsRectificacionInmueble = true;
	}
	vm.anularRectificacionInmueble = function () {
		vm.tramite.EsRectificacionInmueble = false;
		vm.tramite.Entidad.Inmueble = clone(vm.entidadInmuebleOriginal);
	}
	//FIN:RECTIFICAR

	//TABS
	vm.showTabTitularPersonaJuridica = vm.showTabTitularPersonaFisica =
        vm.tramite.Entidad.Inmueble && (vm.tramite.Entidad.Inmueble.PersonasFisicas.length > 0 || vm.tramite.Entidad.Inmueble.TomosFolios.length > 0);
	//FIN:TABS

	//MODALES
	//MODAL TOMO FOLIO
	vm.abrirTomoFolio = function (action, index) {
		var tomoFolioOriginal = null;
		if (index !== undefined) {
			tomoFolioOriginal = $filter('filter')(vm.tramite.Entidad.Inmueble.TomosFolios, { Borrado: false })[index];
		}
		var modalInstanceTomoFolio = $uibModal.open({
			templateUrl: 'ModalTomoFolio.html',
			controller: 'tomoFolioController',
			controllerAs: 'tomoFolioCtrl',
			size: 'lg',
			backdrop: 'static',
			animation: false,
			resolve: {
				action: function () {
					return action;
				},
				tomoFolio: function () {
					return clone(tomoFolioOriginal);
				}
			}
		});
		modalInstanceTomoFolio.result.then(function (returnTomoFolio) {
			if (action === 'POST') {
				returnTomoFolio.Borrado = false;
				vm.tramite.Entidad.Inmueble.TomosFolios.push(returnTomoFolio);
			}
			else if (action === 'PUT') {
				tomoFolioOriginal.Tomo = returnTomoFolio.Tomo;
				tomoFolioOriginal.Folio = returnTomoFolio.Folio;
				tomoFolioOriginal.Observacion = returnTomoFolio.Observacion;
			}
			else if (action === 'DELETE') {
				tomoFolioOriginal.Observacion = returnTomoFolio.Observacion;
				tomoFolioOriginal.Borrado = true;
			}
		});
	};

	//MODAL TITULAR PERSONA JURIDICA
	vm.abrirPersonaJuridica = function (action, index) {
		var personaJuridicaOriginal = null;
		if (index !== undefined) {
			personaJuridicaOriginal = $filter('filter')(vm.tramite.Entidad.Inmueble.PersonasJuridicas, { Borrado: false })[index];
		}
		var modalInstancePersonaJuridica = $uibModal.open({
			templateUrl: 'ModalPersonaJuridica.html',
			controller: 'personaJuridicaController',
			controllerAs: 'personaJuridicaCtrl',
			size: 'lg',
			backdrop: 'static',
			animation: false,
			resolve: {
				action: function () {
					return action;
				},
				personaJuridica: function () {
					return clone(personaJuridicaOriginal);
				}
			}
		});
		modalInstancePersonaJuridica.result.then(function (returnPersonaJuridica) {
			if (action === 'POST') {
				returnPersonaJuridica.IsJuridica = false;
				returnPersonaJuridica.Borrado = false;
				vm.tramite.Entidad.Inmueble.PersonasJuridicas.push(returnPersonaJuridica);
			}
			else if (action === 'PUT') {
				personaJuridicaOriginal.IsJuridica = false;
				personaJuridicaOriginal.RazonSocial = returnPersonaJuridica.RazonSocial;
				personaJuridicaOriginal.SociedadIGJId = returnPersonaJuridica.SociedadIGJId;
				personaJuridicaOriginal.Domicilio = returnPersonaJuridica.Domicilio;
				personaJuridicaOriginal.Cuit = returnPersonaJuridica.Cuit;
				personaJuridicaOriginal.Matricula = returnPersonaJuridica.Matricula;
				personaJuridicaOriginal.Tomo = returnPersonaJuridica.Tomo;
				personaJuridicaOriginal.Folio = returnPersonaJuridica.Folio;
				personaJuridicaOriginal.Observacion = returnPersonaJuridica.Observacion;
			}
			else if (action === 'DELETE') {
				personaJuridicaOriginal.Observacion = returnPersonaJuridica.Observacion;
				personaJuridicaOriginal.Borrado = true;
			}
		});
	};

	//MODAL TITULAR PERSONA FISICA
	vm.abrirPersonaFisica = function (action, index) {
		var personaFisicaOriginal = null;
		if (index !== undefined) {
			personaFisicaOriginal = $filter('filter')(vm.tramite.Entidad.Inmueble.PersonasFisicas, { Borrado: false })[index];
		}
		var modalInstancePersonaFisica = $uibModal.open({
			templateUrl: 'ModalPersonaFisica.html',
			controller: 'personaFisicaController',
			controllerAs: 'personaFisicaCtrl',
			size: 'lg',
			backdrop: 'static',
			animation: false,
			resolve: {
				action: function () {
					return action;
				},
				personaFisica: function () {
					return clone(personaFisicaOriginal);
				}
			}
		});
		modalInstancePersonaFisica.result.then(function (returnPersonaFisica) {
			if (action === 'POST') {
				returnPersonaFisica.IsFisica = true;
				returnPersonaFisica.Borrado = false;
				vm.tramite.Entidad.Inmueble.PersonasFisicas.push(returnPersonaFisica);
			}
			else if (action === 'PUT') {
				personaFisicaOriginal.IsFisica = true;
				personaFisicaOriginal.Apellido = returnPersonaFisica.Apellido;
				personaFisicaOriginal.Nombre = returnPersonaFisica.Nombre;
				personaFisicaOriginal.TipoDocumento = returnPersonaFisica.TipoDocumento;
				personaFisicaOriginal.NumeroDocumento = returnPersonaFisica.NumeroDocumento;
				personaFisicaOriginal.Cuit = returnPersonaFisica.Cuit;
				personaFisicaOriginal.Observacion = returnPersonaFisica.Observacion;
			}
			else if (action === 'DELETE') {
				personaFisicaOriginal.Observacion = returnPersonaFisica.Observacion;
				personaFisicaOriginal.Borrado = true;
			}
		});
	};
	//FIN:MODALES

	//LOOKUPS

	vm.profesiones = Profesion.query(function() {}, function(error) {
		alert('Error al obtener las profesiones.');
	});

	vm.reparticionesSolicitantes = Jurisdiccion.solicitantes({}, function() {}, function(error) {
		alert('Error al obtener las reparticiones solicitantes.');
	});

	vm.unidadesMedida = UnidadMedida.query(function() {}, function(error) {
		alert('Error al obtener las unidades de medida.');
	});

	vm.tipoDocumentos = TiposDocumentos.query(function () { }, function (error) {
		alert('Error al obtener los tipos de documento.');
	});

	vm.provincias = Provincia.query(function() {}, function(error) {
		alert('Error al obtener las provincias.');
	});

	vm.fillSelectLocalidades = function () {
		if (vm.tramite.Solicitante.ProvinciaId) {
			vm.localidades = Provincia.localidades({ provinciaId: vm.tramite.Solicitante.ProvinciaId }, function () { }, function (error) {
				vm.localidades = [];
				alert('Error al obtener las localidades.');
			});
		} else {
			vm.localidades = [];
		}
	};
	vm.fillSelectLocalidades();
	//END:LOOKUPS

	vm.totalTasaInformantes = function () {
		var total = 0;
		for (var i = 0; i < vm.tramite.Tramites.length; i++) {
			total += vm.tramite.Tramites[i].TasaProvincial;
		}
		return total;
	}

	vm.totalTasaProyectInformantes = function () {
		var total = 0;
		for (var i = 0; i < vm.tramite.Tramites.length; i++) {
			total += vm.tramite.Tramites[i].TasaNacional;
		}
		return total;
	}

	var submitError = function (error) {
		switch (error.status) {
			case 404:
				vm.tramiteInexistente = true;
				break;
			case 400:
				validation.validate(vm, error);
				break;
			default:
				alert('Ocurrió un error en la llamada al servidor:\n\nError: ' + error.statusText + '\nMensaje: ' + error.data.Message);
		}
	};

	vm.guardarBase = function (backPath) {
		$scope.$broadcast('show-errors-check-validity', 'tramiteForm');
		if (vm.tramite.Entidad.Inmueble) {
			vm.validateInmueble();
		}
		if (vm.tramiteForm.$valid) {
			vm.tramiteFormEsInvalido = false;
			vm.tramite.$update({ id: $routeParams.tramiteId }, function () {
				$location.path(backPath);
			},
                function (error) {
                	submitError(error);
                });
		} else {
			vm.tramiteFormEsInvalido = true;
		}
	};

	vm.validateInmueble = function () {
		if (!vm.tramiteForm.matricula)
			return;

		vm.tramiteForm.matricula.$setValidity('required', true);
		vm.tramiteForm.matricula.$setValidity('exclusiveAssignment', true);

		//VALIDAR EXCLUSION MATRICULA - TOMOS FOLIOS
		if (!vm.tramiteForm.matricula.$isEmpty(vm.tramiteForm.matricula.$modelValue) && ($filter('filter')(vm.tramite.Entidad.Inmueble.TomosFolios, { Borrado: false })).length > 0) {
			vm.tramiteForm.matricula.$setValidity('exclusiveAssignment', false);
			return;
		}
		//VALIDAR MATRICULA ES REQUERIDA
		if (vm.tramiteForm.matricula.$isEmpty(vm.tramiteForm.matricula.$modelValue) && ($filter('filter')(vm.tramite.Entidad.Inmueble.TomosFolios, { Borrado: false })).length < 1) {
			vm.tramiteForm.matricula.$setValidity('required', false);
			return;
		}
	};

}