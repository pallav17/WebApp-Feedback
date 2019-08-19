var myapp = angular.module('Myapp', []);
myapp.controller("Mycontroller", function ($scope, $http, $window) {
    $scope.LoginData = function () {
        if ($scope.email == undefined || $scope.password == undefined)
            alert('Please Fill up all the required details')
        else {
           var email = $scope.email+"@schaeffler.com"
            $http({
                method: 'POST',
                url: 'WebService1.asmx/getLogin',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                //data: { Email: $scope.email, Password: $scope.password }
                data: { Email: email, Password: $scope.password }

            }).then(function (response) {
                if (response.data.d > 0) {
                   // alert("Logged in Successfully.");
                   // localStorage.setItem("key", $scope.email);
                    localStorage.setItem("key", email);
                   
                    $window.location.href = 'DisplayFeedbackdata.html';
                }
                else {
                    alert("Please,Enter Correct username & password");
                    $scope.email = $scope.password = "";
                }
            })

        }
    }


    $scope.ForgotData = function () {

        if ($scope.forgotEmail == undefined)
            alert('Please Enter your Email ID here')

        else {

            $http({

                method: 'POST',
                url: 'WebService1.asmx/ForgotPassword',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { Email: $scope.forgotEmail }

            }).then(function (response) {

                if (response.data.d > 0) {
                    localStorage.setItem("forkey", $scope.forgotEmail);
                    localStorage.setItem("forpass", $scope.forgotEmail);
                    alert("Please check your Email inbox and enter the Access code");
                    $scope.forgotEmail = "";
                    $window.location.href = 'VerifyAccessCodeForgotPass.html';
                }
                else {
                    alert("This Email is not registered in the system");
                }
            })

        }

    }

})

