﻿"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Name', field: 'name' }, 
    { headerName: 'Code', field: 'code', maxWidth: 100 }, 
    { headerName: 'Category', field: 'category', maxWidth: 100 }, 
    { headerName: 'Address', field: 'address' },
    { headerName: 'PhoneNo', field: 'phoneNo', maxWidth: 150, sortable: false, filter: false },
    {
        headerName: 'Edit', maxWidth: 150, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-edit" id="editButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Edit(params.data);

        }
    },
    {
        headerName: 'Delete', maxWidth: 150, sortable: false, filter: false,
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
        url: 'Hotel/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Settings for the Hotel grid
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

//Function to clear form
const Clear = () => {
    removeBorderClass();
    $("#id").val('');
    $('#name').val('');
    $('#code').val('');
    $('#category').val('');
    $('#address').val('');
    $('#phoneNo').val('');
};

//Function specifying rules for validating the form
const hotelValidation = () => {

    $('#hotelForm').validate({
        rules: {
            name: {
                required: true,
                maxlength: 100,
                isOnlyWhiteSpace: true
            },
            code: {
                required: true,
                maxlength: 15,
                isOnlyWhiteSpace: true
            },
            category: {
                required: true             
            },
            address: {
                required: true,
                isOnlyWhiteSpace: true
            },
            phoneNo: {
                required: true,
                digits: true
            }
        }
    });
};

const Edit = (data) => {

    Clear();
    $('#hotelTitle').html("Edit Hotel");
    $('#id').val(data.id);
    $('#name').val(data.name);
    $('#code').val(data.code);
    $('#category').val(data.category);
    $('#address').val(data.address);
    $('#phoneNo').val(data.phoneNo);
    $('#hotelForm').validate().destroy();
    hotelValidation();
    $('#hotelForm').validate().resetForm();
    $('#createHotel').modal('toggle');
};

const Delete = (data) => {

    var confirm = window.confirm("Are you sure you want to delete?");

    if (confirm) {
        $.ajax({
            url: 'Hotel/Delete',
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


const Save = () => {

    $('#hotelForm').off('submit').on('submit', function (e) {

        e.preventDefault();       

        var record = {
            Id: $('#id').val(),
            Name: $('#name').val(),
            Code: $('#code').val(),
            Category: $('#category').val(),
            Address: $('#address').val(),
            PhoneNo: $('#phoneNo').val()
        };

        $.ajax({
            url: 'Hotel/Save',
            method: 'POST',
            data: { hotel: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createHotel').modal('toggle');
                setGridData();  
            }
        });

    });
};

$(document).ready(function () {
    var hotelGrid = document.querySelector('#hotelGrid');

    new agGrid.Grid(hotelGrid, gridOptions);

    setGridData();

    $('#addHotelBtn').click(function () {
        console.log('Button Pressed');
        $('#hotelTitle').html("Add Hotel");
        Clear();
        $('#hotelForm').validate().destroy();
        hotelValidation();
        $('#hotelForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#hotelForm').valid()) {
            Save();
        }
    });

    $('#searchField').on('keyup', function () {
        var filter;
        filter = {
            name: { type: 'contains', filter: $('#searchField').val() }
        };
        gridOptions.api.setFilterModel(filter);
        gridOptions.api.onFilterChanged();
    });
});