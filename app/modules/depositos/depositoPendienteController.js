angular
    .module('webApp')
    .controller('depositoPendienteController', ['$scope', '$routeParams', '$resource', '$http', '$location', 'validation', 'DepositoPendiente', 'authentication', depositoPendienteController]);

function depositoPendienteController($scope, $routeParams, $resource, $http, $location, validation, DepositoPendiente, authentication) {
    var vm = this;

    vm.model = { CuentasBancarias: null };

    vm.model.CuentasBancarias = [
        {
            Id: 1,
            numeroCuenta: '123',
            tipocuenta: 'Sueldo',
            entidad: 'Santander',
            sucursal: 'Bs As',
            cbu: '321'
        },
        {
            Id: 2,
            numeroCuenta: '456',
            tipocuenta: 'Deposito',
            entidad: 'HSBC',
            sucursal: 'Lujan',
            cbu: '654'
        },
        {
            Id: 3,
            numeroCuenta: '789',
            tipocuenta: 'Plazo',
            entidad: 'City',
            sucursal: 'La Plata',
            cbu: '987'
        }
    ];
};