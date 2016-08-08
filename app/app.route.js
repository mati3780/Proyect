angular.module('webApp')
    .config(['$routeProvider', ConfigRouteProvider]);

function ConfigRouteProvider($routeProvider) {
    var modules = 'app/modules/';
    $routeProvider.caseInsensitiveMatch = true;
    $routeProvider
    .when('/', {
        templateUrl: modules + 'home/home.html',
        controller: 'homeController',
        titulo: 'Home'
    })
    .when('/login', {
        templateUrl: modules + 'login/login.html',
        controller: 'loginController',
		controllerAs: 'loginCtrl',
        access: {
            allowAnonymous: true
        }
    })

    //#region Auditoria

	.when('/auditoria', {
	    templateUrl: modules + 'auditoria/auditoria.html',
	    controller: 'auditoriaController',
	    controllerAs: 'auditoriaCtrl',
	    titulo: 'Auditoría',
	    subtitulo: '',
	    access: {
	        permissions: ['Auditoria']
	    }
	})

   //#endregion

    //#region Jurisdicciones
	.when('/jurisdicciones', {
	    templateUrl: modules + 'jurisdicciones/jurisdicciones.html',
	    controller: 'jurisdiccionesController',
	    controllerAs: 'jurisdiccionesCtrl',
	    titulo: 'Jurisdicciones',
	    subtitulo: '',
	    access: {
	        permissions: ['ABMJurisdicciones']
	    }
	})
	.when('/jurisdicciones/new', {
	    templateUrl: modules + 'jurisdicciones/jurisdiccion.html',
	    controller: 'jurisdiccionController',
	    controllerAs: 'controller',
	    titulo: 'Jurisdicciones',
	    subtitulo: 'Nuevo',
	    access: {
	        permissions: ['NuevaJurisdiccion']
	    }
	})
	.when('/jurisdicciones/:jurisdiccionId', {
	    templateUrl: modules + 'jurisdicciones/jurisdiccion.html',
	    controller: 'jurisdiccionController',
	    controllerAs: 'controller',
	    titulo: 'Jurisdicciones',
	    subtitulo: 'Editar',
	    access: {
	        permissions: ['ModificarJurisdiccion']
	    }
	})

   //#endregion

    //#region Plazos
	.when('/plazos', {
	    templateUrl: modules + 'plazos/plazos.html',
	    controller: 'plazosController',
	    titulo: 'Plazos de Trámites',
	    subtitulo: '',
	    access: {
	        permissions: ['ABMPlazos']
	    }
	})
	.when('/plazos/new', {
	    templateUrl: modules + 'plazos/plazo.html',
	    controller: 'plazoController',
	    controllerAs: 'controller',
	    titulo: 'Plazo de Trámite',
	    subtitulo: 'Nuevo',
	    access: {
	        permissions: ['ABMPlazos']
	    }
	})
	.when('/plazos/:plazoId', {
	    templateUrl: modules + 'plazos/plazoEdit.html',
	    controller: 'plazoEditController',
	    controllerAs: 'controller',
	    titulo: 'Plazo de Trámite',
	    subtitulo: 'Cerrar',
	    access: {
	        permissions: ['ABMPlazos']
	    }
	})
    .when('/plazos/:plazoId/detalle', {
        templateUrl: modules + 'plazos/plazoDetalle.html',
        controller: 'plazoDetalleController',
        controllerAs: 'controller',
        titulo: 'Plazo de Trámite',
        subtitulo: 'Detalle',
        access: {
            permissions: ['ABMPlazos']
        }
    })
   //#endregion

    //#region Feriados
	.when('/feriados', {
	    templateUrl: modules + 'feriados/feriados.html',
	    controller: 'feriadosController',
	    titulo: 'Feriados',
	    subtitulo: '',
	    access: {
	        permissions: ['ABMFeriados']
	    }
	})
	.when('/feriados/new', {
	    templateUrl: modules + 'feriados/feriado.html',
	    controller: 'feriadoController',
	    titulo: 'Feriados',
	    subtitulo: 'Nuevo',
	    access: {
	        permissions: ['ABMFeriados']
	    }
	})
	.when('/feriados/:feriadoId', {
	    templateUrl: modules + 'feriados/feriado.html',
	    controller: 'feriadoController',
	    titulo: 'Feriados',
	    subtitulo: 'Editar',
	    access: {
	        permissions: ['ABMFeriados']
	    }
	})

   //#endregion

        //#region Cuentas Bancarias
	.when('/cuentabancarias', {
	    templateUrl: modules + 'cuentabancarias/cuentabancarias.html',
	    controller: 'cuentabancariasController',
	    titulo: 'Cuentas Bancarias',
	    subtitulo: '',
	    access: {
	        permissions: ['ABMCuentaBancarias']
	    }
	})
	.when('/cuentabancarias/new', {
	    templateUrl: modules + 'cuentabancarias/cuentabancaria.html',
	    controller: 'cuentabancariaController',
	    controllerAs: 'controller',
	    titulo: 'Cuentas Bancarias',
	    subtitulo: 'Nuevo',
	    access: {
	        permissions: ['ABMCuentaBancarias']
	    }
	})
	.when('/cuentabancarias/:cuentabancariaId', {
	    templateUrl: modules + 'cuentabancarias/cuentabancariabaja.html',
	    controller: 'cuentabancariaController',
	    controllerAs: 'controller',
	    titulo: 'Cuentas Bancarias',
	    subtitulo: 'Eliminar',
	    access: {
	        permissions: ['ABMCuentaBancarias']
	    }
	})
   //#endregion
	//#region Datos Tramites

	.when('/datostramites', {
	    templateUrl: modules + 'datostramites/datostramites.html',
	    controller: 'datosTramitesController',
	    controllerAs: 'datosTramitesCtrl',
	    titulo: 'Datos de Trámites',
	    access: {
	        permissions: ['ABMDatosTramites']
	    }
	})
	.when('/datostramites/new', {
	    templateUrl: modules + 'datostramites/datostramite.html',
	    controller: 'datosTramiteController',
	    controllerAs: 'datosTramiteCtrl',
	    titulo: 'Datos de Trámite',
	    subtitulo: 'Nuevo',
	    access: {
	        permissions: ['ABMDatosTramites']
	    }
	})
	.when('/datostramites/:servicioId', {
	    templateUrl: modules + 'datostramites/datostramite.html',
	    controller: 'datosTramiteController',
	    controllerAs: 'datosTramiteCtrl',
	    titulo: 'Datos de Trámite',
	    subtitulo: 'Editar',
	    access: {
	        permissions: ['ABMDatosTramites']
	    }
	})
	.when('/datostramites/ver/:servicioId', {
	    templateUrl: modules + 'datostramites/verdatostramite.html',
	    controller: 'verDatosTramiteController',
	    controllerAs: 'verDatosTramiteCtrl',
	    titulo: 'Datos de Trámite',
	    subtitulo: 'Nuevo',
	    access: {
	        permissions: ['ABMDatosTramites']
	    }
	})
	//#endregion

    //#region Depositos y Acreditaciones
	.when('/ActualizarDepositosAcreditaciones', {
	    templateUrl: modules + 'depositos/depositos.html',
	    controller: 'depositosController',
	    controllerAs: 'depositosCtrl',
	    titulo: 'Actualizar Depósitos y Acreditaciones',
	    subtitulo: '',
	    access: {
	        permissions: ['ListarDepositosAcreditaciones']
	    }
	})
    .when('/ActualizarDepositosAcreditaciones/:liquidacionId', {
        templateUrl: modules + 'depositos/deposito.html',
        controller: 'depositoController',
        controllerAs: 'depositoCtrl',
        titulo: '',
        subtitulo: '',
        access: {
            permissions: ['ActualizarDepositoAcreditacion']
        }
    })
    .when('/ActualizarDepositosAcreditaciones/:liquidacionId/ver', {
        templateUrl: modules + 'depositos/depositoVer.html',
        controller: 'depositoVerController',
        controllerAs: 'depositoCtrl',
        titulo: '',
        subtitulo: '',
        access: {
            permissions: ['ListarDepositosAcreditaciones']
        }
    })
    .when('/ActualizarDepositosAcreditaciones/:liquidacionId/imprimir', {
        templateUrl: modules + 'depositos/depositoImprimir.html',
        controller: 'depositoImprimirController',
        controllerAs: 'depositoCtrl',
        titulo: '',
        subtitulo: '',
        access: {
            permissions: ['ListarDepositosAcreditaciones']
        }
    })
    .when('/ActualizarDepositosAcreditaciones/:liquidacionId/verPDFDeposito', {
        templateUrl: modules + 'depositos/verPDFDeposito.html',
        controller: 'verPDFDepositoController',
        controllerAs: 'depositoCtrl',
        titulo: '',
        subtitulo: '',
        access: {
            permissions: ['ListarDepositosAcreditaciones']
        }
    })
    .when('/ActualizarDepositosAcreditaciones/:liquidacionId/verPDFAcreditacion', {
        templateUrl: modules + 'depositos/verPDFAcreditacion.html',
        controller: 'verPDFAcreditacionController',
        controllerAs: 'depositoCtrl',
        titulo: '',
        subtitulo: '',
        access: {
            permissions: ['ListarDepositosAcreditaciones']
        }
    })
    .when('/DepositosAcreditacionesPendientes', {
        templateUrl: modules + 'depositos/depositosPendientes.html',
        controller: 'depositosPendientesController',
        controllerAs: 'depPendientesCtrl',
        titulo: 'Depósitos y Acreditaciones Pendientes',
        subtitulo: '',
        access: {
            permissions: ['ListarDepositosAcreditacionesPendientes']
        }
    })
    .when('/DepositosAcreditacionesPendientes/:jurisdiccionId/:liquidacionesId/:importe', {
            templateUrl: modules + 'depositos/compensacion.html',
            controller: 'compensacionController',
            controllerAs: 'depositoCtrl',
            titulo: '',
            subtitulo: '',
            access: {
                permissions: ['CompensarDepositoAcreditacionPendiente']
            }
    })
          .when('/Compensaciones', {
              templateUrl: modules + 'compensaciones/compensaciones.html',
              controller: 'compensacionesController',
              controllerAs: 'compensacionesCtrl',
              titulo: 'Compensaciones',
              subtitulo: '',
              access: {
                  permissions: ['ListarCompensaciones']
              }
          })
   //#endregion

    //#region Contribucion PROYECT
	.when('/contribuciones', {
	    templateUrl: modules + 'contribucionsinarepi/contribuciones.html',
	    controller: 'contribucionesController',
	    controllerAs: 'contribucionesCtrl',
	    titulo: 'Contribuciones PROYECT',
	    subtitulo: '',
	    access: {
	        permissions: ['ABMContribucionPROYECT']
	    }
	})
	.when('/contribuciones/new', {
	    templateUrl: modules + 'contribucionsinarepi/contribucion.html',
	    controller: 'contribucionController',
	    controllerAs: 'contribucionCtrl',
	    titulo: 'Contribuciones PROYECT',
	    subtitulo: 'Nuevo',
	    access: {
	        permissions: ['ABMContribucionPROYECT']
	    }
	})
	.when('/contribuciones/:contribucionId', {
	    templateUrl: modules + 'contribucionsinarepi/contribucionbaja.html',
	    controller: 'contribucionController',
	    controllerAs: 'contribucionCtrl',
	    titulo: 'Contribuciones PROYECT',
	    subtitulo: 'Eliminar',
	    access: {
	        permissions: ['ABMContribucionPROYECT']
	    }
	})
    //#endregion

    //#region Registrar Tramite
    .when('/registrartramites', {
        templateUrl: modules + 'registrartramites/RegistrarTramites.html',
        controller: 'registrarTramitesController',
        controllerAs: 'controller',
        titulo: 'Registrar Tramites',
        subtitulo: '',
        access: {
            permissions: ['RegistrarTramites']
        }
    })
    .when('/registrartramites/:tramiteId', {
        templateUrl: modules + 'registrartramites/RegistrarTramite.html',
        controller: 'registrarTramiteController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Registrar Trámite',
        subtitulo: '',
        access: {
            permissions: ['RegistrarTramite']
        }
    }).when('/registrartramites/:tramiteId/ver', {
        templateUrl: modules + 'registrartramites/VerTramite.html',
        controller: 'verTramiteController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Ver Trámite',
        subtitulo: '',
        access: {
            permissions: ['RegistrarTramites']
        }
    })
        .when('/registrartramites/:tramiteId/imprimir', {
            templateUrl: modules + 'registrartramites/RegistrarTramitePdf.html',
            controller: 'registrarTramitePdfController',
            controllerAs: 'registrarTramitePdfCtrl',
            titulo: 'Imprimir Trámite',
            subtitulo: '',
            access: {
                permissions: ['RegistrarTramites']
            }
        })
   //#endregion

   //#region Tramite Jurisdiccion Informante
	.when('/jurisdiccionInformante', {
	    templateUrl: modules + 'jurisdiccioninformante/jurisdiccionInformante.html',
	    controller: 'jurisdiccionInformanteController',
	    controllerAs: 'jurisdiccionInformanteCtrl',
	    titulo: 'Jurisdicción Informante',
	    subtitulo: '',
	    access: {
	        permissions: ['ListarTramitesJurisdiccionInformante']
	    }
	}).when('/jurisdiccionInformante/:tramiteId', {
	    templateUrl: modules + 'jurisdiccioninformante/informanteResponderTramite.html',
	    controller: 'informanteResponderTramiteController',
	    controllerAs: 'tramiteCtrl',
	    titulo: 'Informante Responder Trámite',
	    subtitulo: '',
	    access: {
	        permissions: ['InformanteResponderTramite']
	    }
	})
    .when('/jurisdiccionInformante/:tramiteId/ver', {
        templateUrl: modules + 'jurisdiccioninformante/informanteVerTramite.html',
        controller: 'informanteVerTramiteController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Ver Trámite',
        subtitulo: '',
        access: {
            permissions: ['ListarTramitesJurisdiccionInformante']
        }
    })
    .when('/jurisdiccionInformante/:tramiteId/imprimir', {
        templateUrl: modules + 'jurisdiccioninformante/informantePdf.html',
        controller: 'informantePdfController',
        controllerAs: 'informantePdfCtrl',
        titulo: 'Imprimir Trámite',
        subtitulo: '',
        access: {
            permissions: ['ListarTramitesJurisdiccionInformante']
        }
    })
    .when('/jurisdiccionInformante/:tramiteId/verificarPDF', {
        templateUrl: modules + 'jurisdiccioninformante/VerificarPDF.html',
        controller: 'verificarPDFController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Verificar PDF Trámite',
        subtitulo: '',
        access: {
            permissions: ['InformanteResponderTramite']
        }
    })
    .when('/jurisdiccionInformante/:tramiteId/verPDFRechazado', {
        templateUrl: modules + 'jurisdiccioninformante/VerPDFRechazado.html',
        controller: 'verPDFRechazadoController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Ver PDF Rechazado Trámite',
        subtitulo: '',
        access: {
            permissions: ['InformanteResponderTramite']
        }
    })
    //#endregion

    //#region Jurisdicción Requirente
	.when('/jurisdiccionrequirente', {
	    templateUrl: modules + 'jurisdiccionrequirente/JurisdiccionRequirente.html',
	    controller: 'jurisdiccionRequirenteController',
	    controllerAs: 'jurisdiccionRequirenteCtrl',
	    titulo: 'Jurisdicción Requirente',
	    subtitulo: '',
	    access: {
	        permissions: ['ListarTramitesJurisdiccionRequirente']
	    }
	})
    .when('/jurisdiccionrequirente/:tramiteId', {
        templateUrl: modules + 'jurisdiccionrequirente/RequirenteResponderTramite.html',
        controller: 'requirenteResponderTramiteController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Requirente Responder Trámite',
        subtitulo: '',
        access: {
            permissions: ['RequirenteResponderTramite']
        }
    })
    .when('/jurisdiccionrequirente/:tramiteId/ver', {
        templateUrl: modules + 'jurisdiccionrequirente/RequirenteVerTramite.html',
        controller: 'requirenteResponderTramiteController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Ver Trámite',
        subtitulo: '',
        access: {
            permissions: ['Listar', 'RequirenteResponderTramite']
        }
    })
    .when('/jurisdiccionrequirente/:tramiteId/EntregarPDF', {
        templateUrl: modules + 'jurisdiccionrequirente/RequirenteEntregarPDF.html',
        controller: 'requirenteEntregarPDFController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Entregar PDF Trámite',
        subtitulo: '',
        access: {
            permissions: ['RequirenteResponderTramite']
        }
    })
    .when('/jurisdiccionrequirente/:tramiteId/VerificarPDF', {
        templateUrl: modules + 'jurisdiccionrequirente/RequirenteVerificarPDF.html',
        controller: 'requirenteVerificarPDFController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Verificar PDF Trámite',
        subtitulo: '',
        access: {
            permissions: ['RequirenteResponderTramite']
        }
    })
    //#endregion

     //#region consultor de Tramites
     .when('/consultorTramites', {
         templateUrl: modules + 'consultorTramites/ConsultaTramites.html',
         controller: 'consultaTramitesController',
         controllerAs: 'consultaTramitesCtrl',
         titulo: 'Consultor de Trámites',
         subtitulo: '',
         access: {
             permissions: ['ConsultorTramites']
         }
     })
    .when('/consultorTramites/:tramiteId/ver', {
        templateUrl: modules + 'consultorTramites/VerTramiteConsulta.html',
        controller: 'verTramitesController',
        controllerAs: 'tramiteCtrl',
        titulo: 'Ver Trámite',
        subtitulo: '',
        access: {
            permissions: ['ConsultorTramites']
        }
    })
    //#endregion

	.when('/notauthorized', {
	    templateUrl: 'notauthorized.html'
	})
    .otherwise({
        redirectTo: '/'
    });

    //Usar HTML5 History API
    //$locationProvider.html5Mode(true);

}