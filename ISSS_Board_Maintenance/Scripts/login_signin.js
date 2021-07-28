$(document).ready(function () {
    $("#signinLink").click(function () {
        $("#loginSection").animate({ width: 'toggle' }, 600);
        setTimeout(function () {
            $("#signinSection").animate({ width: 'toggle' }, 600);
        }, 600);
    });
    $("#returnLink").click(function () {
        $("#signinSection").animate({ width: 'toggle' }, 600);
        setTimeout(function () {
            $("#loginSection").animate({ width: 'toggle' }, 600);
        }, 600);
    });
});