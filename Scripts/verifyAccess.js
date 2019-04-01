var myapp = angular.module('Myapp', []);
myapp.controller("Mycontroller", function ($scope, $http, $window) {
    $scope.VerifyOTP = function () {
        if ($scope.otpno == undefined)
            alert('Please Enter 4 digit OTP Number')
        else {
            $http({
                method: 'POST',
                url: 'WebService1.asmx/VerifyOtpNumber',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },
                data: { OtpNo: $scope.otpno }

            }).then(function (response) {
                if (response.data.d > 0) {
                    alert("Verified Successfully.");
                    $window.location.href = 'login.html';
                }
                else {
                    alert("Please,Enter Correct OTP Number");
                    $scope.otpno = "";
                }
            })

        }
    }
})