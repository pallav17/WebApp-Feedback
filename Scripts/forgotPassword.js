var myapp = angular.module('Myapp', []);
myapp.controller("Mycontroller", function ($scope, $http, $window) {
    $scope.foremail = localStorage.getItem("forpass");
    if ($scope.foremail == undefined)
        $window.location.href = 'login.html';
    $scope.forgotPassword = function () {
        if ($scope.newpassword == undefined || $scope.newcnpassword == undefined)
            alert('Please Fill up the required details')
        if ($scope.newpassword != $scope.newcnpassword)
            alert('Please enter the same password in both the fields')
        else {
           // $scope.loading = true;
            //console.log($scope.loading);
            $http({
                method: 'POST',
                url: 'WebService1.asmx/ForgotPassMain',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { Password: $scope.newpassword }

            }).then(function (response) {
                //$scope.loading = false;
                //console.log($scope.loading);
                if (response.data.d > 0) {
                    alert("Password Updated Successfully.");
                    localStorage.removeItem("forpass");
                    $window.location.href = 'login.html';
                }
                else {
                    alert("Opps, Something Went Wrong !! Password Not Updated");
                }
            })

        }
    }
});