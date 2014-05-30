'use strict';
app.factory('authService', ['$http', '$q', '$rootScope', 'localStorageService', function ($http, $q, $rootScope, localStorageService) {

    //var serviceBase = 'http://localhost:26264/';
    var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
    var authServiceFactory = {};

    $rootScope.authentication = {
        isAuth: false,
        userName : ""
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName });

            $rootScope.authentication.isAuth = true;
            $rootScope.authentication.userName = loginData.userName;

            deferred.resolve(response);

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');
        $rootScope.authentication.isAuth = false;
        $rootScope.authentication.userName = "";

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData)
        {
            $rootScope.authentication.isAuth = true;
            $rootScope.authentication.userName = authData.userName;
        }

    }

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;

    return authServiceFactory;
}]);