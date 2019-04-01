var myapp = angular.module('Myapp', []);
myapp.controller("Mycontroller", function ($scope, $http, $window) {
   
        $window.location.href = 'login.html';
    
});