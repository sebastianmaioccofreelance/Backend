//import { post } from "jquery";

function ProbarPost_a(){
    var data = {
        Keys: {
            "code": "1234",
            "state": "abcd"
        }
    };
    var url = "https://localhost:44366/WebApi/Actions/ObtenerSocialLogin"
    var genericRequest = {
        Keys: {
            "code": "1234",
            "state": "abcd"
        }
    };
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(genericRequest),
        contentType: "application/json; charset=utf-8",
        async: false,
        success:
            function (data, status, headers, config) {
                debugger;
            }
    });
}

    






















var BaseURL = 'https://localhost:44366/WebApi/'
var InformationSociaLogin;




$(function () {
    ObtenerInfoSocialLogin();
    $("#facebook").click(function () {
        GetAuthorizationCode("Facebook");
    });
    $("#google").click(function () {
        GetAuthorizationCode("Google");
    });
    $("#testGet").click(function () {
        TestGetMethod();
    });

    $("#testPost").click(function () {
        TestPostMethod("Actions/testPost");
    });
    $("#gettoken").click(function () {
        GetToken();
    });
    $("#ObtenerInfoSocialLogin").click(function () {
        ObtenerInfoSocialLogin();
    });
});
function GetToken() {
    var code = GetParam("code");
    var state = GetParam("state");    
    var req = {
        Values: {
            "code": code,
            "state": state
        }
    };
    var urlendpoint = "Actions/LoginWithSocialLogin"; //?code=" + code + "&state=" + state;
    TestPostMethod(urlendpoint, req)
}
function GetParam(name) {
    var queryString = window.location.search;
    var urlParams = new URLSearchParams(queryString);
    var ret = urlParams.get(name);
    return ret;

}
function ObtenerInfoSocialLogin() {
    var url = BaseURL + "Actions/ObtenerSocialLogin";
    $.get(url, function (data) {
        InformationSociaLogin = data.Data;
        ShowText(JSON.stringify(data));
    });
}
function GetAuthorizationCode(provider) {
    debugger;
    InformationSociaLogin.forEach(function (value) {
        if (value.ProviderName == provider) {
            var url = value.URLRedirect
            url += "?client_id=" + value.ClientId;
            url += "&redirect_uri=" + encodeURIComponent(location.href);
            url += "&state=" + provider + "_" + CreateGuid();
            url += "&" + value.ParamsRequest;
            location.href = url;
        }
    }
    );
}
function CreateGuid() {
    function _p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}
function ShowText(data) {
    $("#json").text(data);
}
function TestPostMethod(endpoint, data) {
    var url = BaseURL + endpoint;
    debugger;
    $.ajax({
        "async": false,
        "url": url,
        type: "POST",
        "headers": {
            "content-type": "application/json"
        },
        "success": function () {
            console.log('Se ha ejecutado la acción');
            return true;
        },
        "error": function () {
            console.log('Error al ejecutar acción');
            return false;
        },
        dataType: "application/json",
        "data": JSON.stringify(data)
    });
    //$.post(
    //    url,
    //    data,
    //    function (result)
    //    {
    //        debugger;
    //    }
    //    , "json");
    //$.post(url, data)
    //    .done(function (data) {
    //        alert("Data Loaded: " + data);
    //    });

    //$.ajax({
    //    type: "POST",
    //    url: url,
    //    dataType: 'json',
    //    contentType: 'application/json;charset=utf-8',
    //    data: data,
    //    success:
    //        function (response) {
    //            $("#results").html(response);
    //        }
    //});
}
function PostMethod(endpoint, data) {
    var url = BaseURL + endpoint;
    $.ajax({
        method: "POST",
        url: url,
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify(data),
        success:
            function (response) {
                $("#results").html(response);
            }
        , error: function (err, status) {
            debugger;
            console.log(err);
        }
    });
}
function GetMethod(endpoint) {
    var url = BaseURL + endpoint;
    var data;
    data = $("#json").val();
    $.ajax({
        type: "GET",
        url: url,
        dataType: 'json',
        //contentType: 'application/json;charset=utf-8',
        //data: data,
        success:
            function (response) {
                $("#results").html(response);
            }
        , error: function (err, status) {
            debugger;
            console.log(err);
        }
    });
}

