
function removeBorderClass() {

    $('input').removeClass(["is-valid","is-invalid"]);
    $('select').removeClass(["is-valid", "is-invalid"]);
    $('textarea').removeClass(["is-valid", "is-invalid"]);  
}

$(document).ready(function () {

    $.validator.setDefaults({
        errorClass: 'invalid-feedback',
        highlight: function (element) {
            $(element)
                .closest('.form-control')
                .addClass('is-invalid')
                .removeClass('is-valid');
        },
        unhighlight: function (element) {
            $(element)
                .closest('.form-control')
                .removeClass('is-invalid')
                .addClass('is-valid');
        }
    });

    $.validator.addMethod('isOnlyWhiteSpace', function (value, element) {
        let result = /^\s+/.test(value);
        return !result;
    }, "Only white spaces not allowed");

    $.validator.addMethod("checkUserName", function (value, element, params) {
        var isSuccess = false;
        $.ajax({
            url: '/Admin/CheckUsername',
            method: 'GET',
            async: false,
            data: { username: value, userId: params },
            success: function (data) {
                isSuccess = data === 1 ? true : false;
            }
        });
        return isSuccess;
    }, "Username already exists");

    $.validator.addMethod("checkFileCodeNo", function (value, element) {

        var isSuccess = false;

        $.ajax({
            url: '/Customer/CheckFileCodeNo',
            method: 'GET',
            async: false,
            data: { fileCodeNo: value },
            success: function (data) {
                isSuccess = data === 1 ? true : false;
            }
        });

        return isSuccess;
    }, "File Code Number does not exists"); 

    $.validator.addMethod("greaterDate", function (value, element, params) {
        var startDate = $(params).val();
        return value > startDate;
    }, "End date must be greater than Start date");

    $.validator.addMethod("lesserDate", function (value, element, params) {

        var endDate = $(params).val();
        if (endDate) {
            return value < endDate;
        }
        else {
            return true;
        }
    }, "Start date must be less than End date");
});