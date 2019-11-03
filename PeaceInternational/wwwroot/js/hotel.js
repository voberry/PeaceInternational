"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Name', field: 'name' }, 
    { headerName: 'Address', field: 'address' },
    { headerName: 'PhoneNo', field: 'phoneNo' },
    {
        headerName: 'Edit', maxWidth: 200,
        cellRenderer: function () {
            return '<i class="btn fas fa-edit" id="editButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);  
            Edit(params.data);
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
            address: {
                required: true
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
    $('#address').val(data.address);
    $('#phoneNo').val(data.phoneNo);
    $('#hotelForm').validate().destroy();
    hotelValidation();
    $('#hotelForm').validate().resetForm();
    $('#createHotel').modal('toggle');
};


const Save = () => {

    $('#hotelForm').off('submit').on('submit', function (e) {

        e.preventDefault();       

        var record = {
            Id: $('#id').val(),
            Name: $('#name').val(),
            Address: $('#address').val(),
            PhoneNo: $('#phoneNo').val()
        };

        $.ajax({
            url: 'Hotel/Save',
            method: 'POST',
            data: { hotel: record },
            success: function (data) {
                setGridData();                
                $('#createHotel').modal('toggle');
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
});