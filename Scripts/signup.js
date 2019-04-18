
var app = angular.module("MyApp", []);
app.controller("MyController", function ($scope, $http, $window) {

    


    $scope.Save = function () {
        if ($scope.FName == undefined || $scope.LName == undefined || $scope.office == undefined || $scope.email == undefined || $scope.password == undefined || $scope.cpassword == undefined)
            alert('Please fill the required details')

        else if ($scope.password != $scope.cpassword)
            alert('Please Enter same password in both the feilds.')

        else {
            $http({
                method: 'POST',
                url: 'WebService1.asmx/InsertUserData',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
               
             data: { FirstName: $scope.FName, LastName: $scope.LName, Office: $scope.office, Email: $scope.email, Password: $scope.password, IsActive: false, IsDelete: false }
                
            }).then(function (response) {
                if (response.data.d > 0) {
                    alert("Registered Successfully.");
                    $window.location.href = 'VerifyAccessCode.html';
                }
                else {
                    alert("Please Login!!! The Email Address is already registered. ");
                    $scope.email = "";
                    $window.location.href = 'login.html'
                }
            })
        }
    }
});