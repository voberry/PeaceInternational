"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Username', field: 'userName' },   
    { headerName: 'PhoneNo', field: 'phoneNumber', sortable: false, filter: false },   
    {
        headerName: 'Change Password', maxWidth: 200, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-edit" id="editButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            ChangePassword(params.data);
        }
    },
    {
        headerName: 'Delete', maxWidth: 200, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-trash" id="trashButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Delete(params.data);

        }
    }
];

//Function to set the data for the grid
const setGridData = () => {

    $.ajax({
        url: 'Users/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};

//Settings for the Users grid
let gridOptions = {
    columnDefs: columnDefs,
    rowHeight: 40,
    defaultColDef: {
        sortable: true,
        filter: true
    },
    paginationAutoPageSize: true,
    pagination: true,
    accentedSort: true,
    onGridSizeChanged: (params) => {
        params.api.sizeColumnsToFit();
    }
};

//Function to clear create users form
const Clear = () => {
    removeBorderClass();
    $("#id").val('');
    $('#username').val('');   
    $('#phoneNo').val('');
    $('#password').val('');
    $('#confirmPassword').val('');
    $('#role').val('');
};


//Function to clear change password form
const ClearChangePassword = () => {
    removeBorderClass();
    $("#id").val('');
    $('#changePwdUserName').val('');
    $('#newPassword').val('');
    $('#confirmNewPassword').val('');    
};

//Function specifying rules for validating the form
const usersValidation = () => {

    $('#usersForm').validate({
        rules: {
            username: {
                required: true,
                maxlength: 100,
                isOnlyWhiteSpace: true
            },
            email: {
                required: true,
                email: true
            },
            role: {
                required: true
            },
            phoneNo: {
                required: true,
                digits: true
            },
            password: {
                required: true,
                minlength: 5
            },
            confirmPassword: {
                required: true,
                equalTo: "#password",
                minlength: 5
            }   
        }
    });
};

//Change password form validation
const changePasswordValidation = () => {
    $('#changePasswordForm').validate({
        rules: {
            newPassword: {
                required: true,
                minlength: 5
            },
            confirmNewPassword: {
                required: true,
                equalTo: "#newPassword",
                minlength: 5
            }
        }
    });
};

const ChangePassword = (data) => {


    $('#userId').val(data.id);
    $('#newPassword').val('');
    $('#confirmNewPassword').val('');
    removeBorderClass();
    $('#changePasswordForm').validate().destroy();
    changePasswordValidation();
    $('#changePasswordForm').validate().resetForm();
    $('#changePwdUserName').val(data.userName);
    $('#changePassword').modal('toggle');

};


const Save = () => {

    $('#usersForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            Id: $('#id').val(),
            Username: $('#username').val(),
            Password: $('#password').val(),
            ConfirmPassword: $('#confirmPassword').val(),
            PhoneNumber: $('#phoneNo').val(),
            Role: $('#role').val()
        };

        $.ajax({
            url: 'Users/Save',
            method: 'POST',
            data: { newUser: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createUsers').modal('toggle');
                setGridData();
            }
        });

    });
};

const SaveNewPassword = () => {

    $('#changePasswordForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            UserId: $('#userId').val(),
            NewPassword: $('#newPassword').val(),
            ConfirmNewPassword: $('#confirmNewPassword').val()            
        };

        $.ajax({
            url: 'Users/ChangePassword',
            method: 'POST',
            data: { newPassword: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#changePassword').modal('toggle');               
            }
        });

    });

};

const Delete = (data) => {

    var confirm = window.confirm("Are you sure you want to delete?");

    if (confirm) {
        $.ajax({
            url: 'Users/Delete',
            method: 'POST',
            data: { id: data.id },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });

                setGridData();
            }
        });
    }
};

$(document).ready(function () {
    var usersGrid = document.querySelector('#usersGrid');

    new agGrid.Grid(usersGrid, gridOptions);

    setGridData();

    $('#addUsersBtn').click(function () {
        console.log('Button Pressed');
        $('#usersTitle').html("Add Users");
        Clear();
        $('#usersForm').validate().destroy();
        usersValidation();
        $('#usersForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#usersForm').valid()) {
            Save();
        }
    });

    $('#btnSaveNewPassword').off('click').on('click', function () {
        if ($('#changePasswordForm').valid()) {
            SaveNewPassword();
        }
    });

    $('#searchField').on('keyup', function () {
        var filter;
        filter = {
            userName: { type: 'contains', filter: $('#searchField').val() }
        };
        gridOptions.api.setFilterModel(filter);
        gridOptions.api.onFilterChanged();
    });
});