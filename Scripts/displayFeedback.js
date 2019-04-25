var testApp = angular.module("mymodule10", []).controller("myController10", function ($scope, $http, $window, $timeout) {

    

    $scope.name = localStorage.getItem("key");

    if ($scope.name == undefined)
        $window.location.href = 'login.html';

    $http.get("WebService1.asmx/GetFeedBackDetailNew").
        then(function (response) {
            $scope.WebService1class = response.data;

        });

    //$timeout(callAtTimeout, 3*1000);
    //function callAtTimeout() {
    //    alert("Timeout occurred");
    //    $window.location.href = 'logout.html';

    //}





});


