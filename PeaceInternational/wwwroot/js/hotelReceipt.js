"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Receipt No.', field: 'receipt' },
    { headerName: 'Hotel', field: 'hotel' },
    { headerName: 'Client Name', field: 'clientName', sortable: false, filter: false },
    {
        headerName: 'Details', maxWidth: 200, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-clipboard" id="detailsButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Edit(params.data);
        }
    },
    {
        headerName: 'Edit', maxWidth: 200, sortable: false, filter: false,
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
        url: 'HotelReceipt/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Settings for the HotelReceipt grid
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
const hotelReceiptValidation = () => {

    $('#hotelReceiptForm').validate({
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
    $('#hotelReceiptTitle').html("Edit Hotel Receipt");
    $('#id').val(data.id);
    $('#name').val(data.name);
    $('#address').val(data.address);
    $('#phoneNo').val(data.phoneNo);
    $('#hotelReceiptForm').validate().destroy();
    hotelReceiptValidation();
    $('#hotelReceiptForm').validate().resetForm();
    $('#createHotelReceipt').modal('toggle');
};


const Save = () => {

    $('#hotelReceiptForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            Id: $('#id').val(),
            Name: $('#name').val(),
            Address: $('#address').val(),
            PhoneNo: $('#phoneNo').val()
        };

        $.ajax({
            url: 'HotelReceipt/Save',
            method: 'POST',
            data: { hotelReceipt: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createHotelReceipt').modal('toggle');
                setGridData();
            }
        });

    });
};

const setHotelDropdown = () => {
    $.ajax({
        url: 'Hotel/Get',
        method: 'GET',
        success: function (data) {
            let options = "";
            for (var i = 0; i < data.length; i++) {
                options += "<option value='" + data[i].id + "'>" + data[i].name + "</option>";
            }
            $('#hotel').append(options);            
        }
    });
}

$(document).ready(function () {
    var hotelReceiptGrid = document.querySelector('#hotelReceiptGrid');

    new agGrid.Grid(hotelReceiptGrid, gridOptions);

    setGridData();

    setHotelDropdown();

    $('#addHotelReceiptBtn').click(function () {
        console.log('Button Pressed');
        $('#hotelReceiptTitle').html("Add Hotel Receipt");
        Clear();
        $('#hotelReceiptForm').validate().destroy();
        hotelReceiptValidation();
        $('#hotelReceiptForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#hotelReceiptForm').valid()) {
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