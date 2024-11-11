$("#newUser").click(function (e) {
    e.preventDefault();
})

function user() {
    var userForm = $("#userForm");
    userForm.show();
}

function company() {
    var userForm = $("#companyForm");
    userForm.css('display', 'flex');
}

$(document).ready(function () {
    $("#user").click(function () {
        $("#userForm").css('display', 'flex');
    });
});

$(document).ready(function () {
    $("#company").click(function () {
        $("#companyForm").css('display', 'flex');
    });
});

$(document).ready(function () {
    $("#closeCompany").click(function () {
        $("#companyForm").css('display', 'none');
    });
});

$(document).ready(function () {
    $("#closeUser").click(function () {
        $("#userForm").css('display', 'none');
    });
});