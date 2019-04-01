var myapp = angular.module('Myapp', []);
myapp.controller("Mycontroller", function ($scope, $http, $window) {
    $scope.name = localStorage.getItem("key");
    if ($scope.name == undefined)
        $window.location.href = 'login.html';
    localStorage.removeItem("key");
    $window.location.href = 'login.html';
});