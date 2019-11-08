
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

    $.validator.addMethod("checkRegistrationNo", function (value, element, params) {

        var isSuccess = false;

        $.ajax({
            url: '/StoreInfo/CheckRegisterNo',
            method: 'GET',
            async: false,
            data: { regNo: value, id: params },
            success: function (data) {
                isSuccess = data === 1 ? true : false;
            }
        });

        return isSuccess;
    }, "Registration Number already exists");

    $.validator.addMethod("checkUserPhoneNo", function (value, element, params) {
        var isSuccess = false;
        $.ajax({
            url: '/Admin/CheckPhoneNo',
            method: 'GET',
            async: false,
            data: { phoneNo: value, userId: params },
            success: function (data) {
                isSuccess = data === 1 ? true : false;
            }
        });
        return isSuccess;
    }, "Phone Number already exists");

    $.validator.addMethod("checkSupplierPhoneNo", function (value, element, params) {
        var isSuccess = false;
        $.ajax({
            url: '/Supplier/CheckPhoneNo',
            method: 'GET',
            async: false,
            data: { phoneNo: value, id: params },
            success: function (data) {
                isSuccess = data === 1 ? true : false;
            }
        });
        return isSuccess;
    }, "Phone Number already exists");

    $.validator.addMethod("checkCustomerPhoneNo", function (value, element, params) {
        var isSuccess = false;
        $.ajax({
            url: '/Customer/CheckPhoneNo',
            method: 'GET',
            async: false,
            data: { phoneNo: value, id: params },
            success: function (data) {
                isSuccess = data === 1 ? true : false;
            }
        });
        return isSuccess;
    }, "Phone Number already exists");

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