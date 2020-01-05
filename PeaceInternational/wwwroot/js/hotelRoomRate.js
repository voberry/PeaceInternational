"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Hotel', field: 'hotel.name', maxWidth: 400 },
    { headerName: 'Single Bed', field: 'singleBed', maxWidth: 120 },
    { headerName: 'Double Bed', field: 'doubleBed', maxWidth: 120 },
    { headerName: 'Extra Bed', field: 'extraBed', maxWidth: 120 },    
    { headerName: 'AP', field: 'ap', maxWidth: 120 },
    { headerName: 'MAP', field: 'map', maxWidth: 120 },
    {
        headerName: 'Edit', maxWidth: 100, sortable: false, filter: false,
        cellRenderer: function () {
            return '<i class="btn fas fa-edit" id="editButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            Edit(params.data);

        }
    },
    {
        headerName: 'Delete', maxWidth: 100, sortable: false, filter: false,
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
        url: 'HotelRoomRate/Get',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Settings for the HotelRoomRate grid
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
    $('#hotel').val('');
    $('#singleBed').val('');
    $('#doubleBed').val('');
    $('#extraBed').val('');
    $('#bb').val('');
    $('#ap').val('');
    $('#map').val('');
};

//Function specifying rules for validating the form
const hotelRoomRateValidation = () => {

    $('#hotelRoomRateForm').validate({
        rules: {
            hotel: {
                required: true,
                maxlength: 100,
                isOnlyWhiteSpace: true
            },
            singleBed: {
                required: true,
                digits: true
            },
            doubleBed: {
                required: true,
                digits: true
            },
            extraBed: {
                required: true,
                digits: true
            },            
            ap: {
                required: true,
                digits: true
            },
            map: {
                required: true,
                digits: true
            }
        }
    });
};

const Edit = (data) => {

    Clear();
    $('#hotelRoomRateTitle').html("Edit Hotel Room Rate");
    $('#id').val(data.id);
    $('#hotel').val(data.hotelId);
    $('#singleBed').val(data.singleBed);
    $('#doubleBed').val(data.doubleBed);
    $('#extraBed').val(data.extraBed);   
    $('#ap').val(data.ap);
    $('#map').val(data.map);
    $('#hotelRoomRateForm').validate().destroy();
    hotelRoomRateValidation();
    $('#hotelRoomRateForm').validate().resetForm();
    $('#createHotelRoomRate').modal('toggle');
};

const Delete = (data) => {

    var confirm = window.confirm("Are you sure you want to delete?");

    if (confirm) {
        $.ajax({
            url: 'HotelRoomRate/Delete',
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
};


const Save = () => {

    $('#hotelRoomRateForm').off('submit').on('submit', function (e) {

        e.preventDefault();

        var record = {
            Id: $('#id').val(),
            HotelId: $('#hotel').val(),
            SingleBed: $('#singleBed').val(),
            DoubleBed: $('#doubleBed').val(),
            ExtraBed: $('#extraBed').val(),         
            AP: $('#ap').val(),
            MAP: $('#map').val()
        };

        $.ajax({
            url: 'HotelRoomRate/Save',
            method: 'POST',
            data: { hotelRoomRate: record },
            success: function (data) {
                console.log(data);
                noty({
                    type: data.type,
                    text: data.message,
                    layout: 'topCenter',
                    timeout: 2000
                });
                $('#createHotelRoomRate').modal('toggle');
                setGridData();
            }
        });

    });
};

$(document).ready(function () {
    var hotelRoomRateGrid = document.querySelector('#hotelRoomRateGrid');

    new agGrid.Grid(hotelRoomRateGrid, gridOptions);

    setGridData();

    setHotelDropdown();

    $('#addHotelRoomRateBtn').click(function () {
        console.log('Button Pressed');
        $('#hotelRoomRateTitle').html("Add Hotel Room Rate");
        Clear();
        $('#hotelRoomRateForm').validate().destroy();
        hotelRoomRateValidation();
        $('#hotelRoomRateForm').validate().resetForm();
    });

    $('#btnSave').off('click').on('click', function () {
        if ($('#hotelRoomRateForm').valid()) {
            Save();
        }
    });

    $('#searchField').on('keyup', function () {
        var filter;
        filter = {
            'hotel.name': { type: 'contains', filter: $('#searchField').val() }
        };
        gridOptions.api.setFilterModel(filter);
        gridOptions.api.onFilterChanged();
    });
});