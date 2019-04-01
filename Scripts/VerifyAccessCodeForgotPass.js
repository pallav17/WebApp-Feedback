var myapp = angular.module('Myapp', []);
myapp.controller("Mycontroller", function ($scope, $http, $window) {
    $scope.foremail = localStorage.getItem("forkey");
    if ($scope.foremail == undefined)
        $window.location.href = 'login.html';

    $scope.VerifyAccessCodeForgotPass = function () {
        if ($scope.otpno == undefined)
            alert('Please Enter 4 digit Access code')
        else {
            $http({
                method: 'POST',
                url: 'WebService1.asmx/VerifyforgotOtpNumber',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { OtpNo: $scope.otpno, Email: $scope.foremail }

            }).then(function (response) {
                if (response.data.d > 0) {
                    alert("Verified successfully.");

                    localStorage.removeItem("forKey")
                    $window.location.href = 'ForgotPassword.html';
                }
                else {
                    alert("Invalid !! Please Enter a Valid Access Code");
                    $scope.otpno = "";
                }
            })

        }
    }
})