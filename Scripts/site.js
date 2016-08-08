$.extend($.fn.dataTable.defaults, {
    searching: false,
    processing: true,
    serverSide: true,
    ordering: false,
    dom: '<lp<tr>ip>',
    pagingType: 'full_numbers',
    language: {
        emptyTable: "No hay información disponible",
        loadingRecords: "Cargando...",
        processing: "Procesando...",
        lengthMenu: "Filas por página: _MENU_",
        zeroRecords: "Sin resultados",
        info: "Página _PAGE_ de _PAGES_",
        infoEmpty: "Sin resultados",
        paginate: {
            first: "Primera",
            last: "Última",
            next: "Siguiente",
            previous: "Previa"
        }
    }
});

$.fn.dataTable.ext.errMode = 'none';

function addClassSemaforo(valor) {
    var clase = "";
    switch (valor) {
        case 2:
            clase = "semaforoVerde";
            break;
        case 3:
            clase = "semaforoRojo";
            break;
        case 4:
            clase = "semaforoAmarillo";
    }
    return clase;
}

var isIE = function() {
	return typeof navigator !== "undefined" && 
                (/MSIE /.test(navigator.userAgent) || (navigator.appName === 'Netscape' && /Trident\/.*rv:([0-9]{1,}[\.0-9]{0,})/.test(navigator.userAgent)));
}

function clone(obj) {
    return JSON.parse(JSON.stringify(obj));
}

function getDate(dateString) {
    var date = dateString.split("/");
    return new Date(date[2], date[1] - 1, date[0]);
}