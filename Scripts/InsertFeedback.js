var starApp = angular.module('starApp', []);

starApp.controller('StarCtrl', ['$scope', '$http', '$window', function ($scope, $http,$window) {

    $scope.name = localStorage.getItem("key");
    if ($scope.name == undefined) 
        $window.location.href = 'login.html';
        
    

    $scope.rating = 0;
    $scope.ratings = [{
        current: 3,
        max: 5
    }];

    

    $scope.getSelectedRating = function (rating) {
        console.log(rating);
     //  alert("The rating is" + rating);
    }
    $scope.name = localStorage.getItem("key");
    $http.get("WebService1.asmx/GetAllUserData").then(function (response) {


       
        // $http.get("http://localhost:50236/WebService1.asmx/GetAllUserData").then(function (response) {

        $scope.WebService1class = response.data;

        //        // $scope.name = localStorage.getItem("key");
        // alert(response.data);
    });

  
    $scope.takeFeedback = function () {

        if ($scope.subject == undefined || $scope.myEmployee == undefined || $scope.pr == undefined || $scope.nr == undefined) {
            alert('Please fill up the required details')
        }
        else {
            $scope.name = localStorage.getItem("key");
          //  alert($scope.myEmployee);

            $http({
                method: 'POST',
                url: 'WebService1.asmx/InsertFeedbackDataNew',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'dataType': 'json'
                },

                data: { Subject: $scope.subject, Recepient_Email: $scope.myEmployee, Description: $scope.pr, Suggestion: $scope.nr, Email: $scope.name, Rating: $scope.ratings[0].current }

            }).then(function (response) {
                if (response.data.d > 0) {
                    alert("Provided Feedback successfully.");
                    $window.location.href = 'DisplayFeedbackdata.html';

                    $scope.pr = $scope.nr = $scope.rate1 = "";
                }
                else {
                    alert("You have provided a feedback ");

                }
            })
        }
    }

    $scope.sendRate = function () {
        alert("Thanks for your rates!\n rate: " + $scope.ratings[0].current + "/" + $scope.ratings[0].max)
    }
}]);

starApp.directive('starRating', function () {
    return {
        restrict: 'A',
        template: '<ul class="rating">' +
            '<li ng-repeat="star in stars" ng-class="star" ng-click="toggle($index)">' +
            '\u2605' +
            '</li>' +
            '</ul>',
        scope: {
            ratingValue: '=',
            max: '=',
            onRatingSelected: '&'
        },
        link: function (scope, elem, attrs) {

            var updateStars = function () {
                scope.stars = [];
                for (var i = 0; i < scope.max; i++) {
                    scope.stars.push({
                        filled: i < scope.ratingValue
                    });
                }
            };

            scope.toggle = function (index) {
                scope.ratingValue = index + 1;
                scope.onRatingSelected({
                    rating: index + 1
                });
            };

            scope.$watch('ratingValue', function (oldVal, newVal) {
                if (newVal) {
                    updateStars();
                }
            });
        }
    }
});


