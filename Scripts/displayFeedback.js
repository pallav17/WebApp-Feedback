var testApp = angular.module("mymodule10", []).controller("myController10", function ($scope, $http, $window) {

    $scope.name = localStorage.getItem("key");
   
    if ($scope.name == undefined)
        $window.location.href = 'login.html';

    $http.get("WebService1.asmx/GetFeedBackDetailNew").
        then(function (response) {
        $scope.WebService1class = response.data;
       
    });
});