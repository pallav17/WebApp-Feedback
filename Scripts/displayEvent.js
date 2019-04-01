var mypartone = angular
    .module("mymodule1", [])
    .controller("myController1", function ($scope, $http) {
        $http.get("WebService1.asmx/GetFeedBackDetailNew")
            .then(function (response) {
                
               // $scope.name = localStorage.getItem("key");
                $scope.EventClass = response.data;
            
            });
    })


