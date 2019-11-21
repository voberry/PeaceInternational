"use strict";

//Column Defination for the grid
const columnDefs = [
    { headerName: 'Receipt No.', field: 'id', maxWidth: 120 },
    {
        headerName: 'Hotel', field: 'hotel.name'        
    },
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
            console.log(data);
            gridOptions.api.setRowData(data);            
        }
    });
};


function call() {
    let filter = {
        id: { type: 'contains', filter: $('#searchFieldReceipt').val() },
        'hotel.name': { type: 'contains', filter: $('#searchFieldHotel').val() },
        clientName: { type: 'contains', filter: $('#searchFieldClientname').val() }
    };
    gridOptions.api.setFilterModel(filter);
    gridOptions.api.onFilterChanged();
}


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
    $('#date').val(new Date().toISOString().slice(0, 10));
    $('#exchangeOrderNo').val('');
    $('#fileCodeNo').val('');
    $('#hotel').val('');
    $('#clientName').val('');
    $('#pax').val('');
    $('#arrivalOn').val('');
    $('#from').val('');
    $('#arrivalFlight').val('');
    $('#departureOn').val('');
    $('#to').val('');
    $('#departureFlight').val('');
    $('#services').val('');
};

//Function specifying rules for validating the form
const hotelReceiptValidation = () => {

    $('#hotelReceiptForm').validate({
        rules: {
            exchangeOrderNo: {
                required: true,
                digits: true
            },
            fileCodeNo: {
                required: true,
                digits: true
            },
            hotel: {
                required: true
            },
            clientName: {
                required: true
            },
            pax: {
                required: true
            },
            arrivalOn: {
                required: true,
                lesserDate: '#departureOn'
            },
            from: {
                required: true
            },
            arrivalFlight: {
                required: true
            },
            departureOn: {
                required: true,
                greaterDate: '#arrivalOn'
            },
            to: {
                required: true
            },
            departureFlight: {
                required: true
            },
            services: {
                required: true
            }

        }
    });
};

const Edit = (data) => {

    Clear();
    console.log(data);
    $('#hotelReceiptTitle').html("Edit Hotel Receipt");
    $('#id').val(data.id);
    $('#exchangeOrderNo').val(data.exchangeOrderNo);
    $('#fileCodeNo').val(data.fileCodeNo);
    $('#hotel').val(data.hotelId);
    $('#clientName').val(data.clientName);
    $('#pax').val(data.pax);
    $('#arrivalOn').val(data.arrivalDate.split('T')[0]);
    $('#from').val(data.from);
    $('#arrivalFlight').val(data.arrivalFlight);
    $('#departureOn').val(data.departureDate.split('T')[0]);
    $('#to').val(data.to);
    $('#departureFlight').val(data.departureFlight);
    $('#services').val(data.services);
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
            ExchangeOrderNo: $('#exchangeOrderNo').val(),
            FileCodeNo: $('#fileCodeNo').val(),
            HotelId: $('#hotel').val(),
            ClientName: $('#clientName').val(),
            PAX: $('#pax').val(),
            ArrivalDate: $('#arrivalOn').val(),
            From: $('#from').val(),
            ArrivalFlight: $('#arrivalFlight').val(),
            DepartureDate: $('#departureOn').val(),
            To: $('#to').val(),
            DepartureFlight: $('#departureFlight').val(),
            Services: $('#services').val()
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

    $('#searchFieldReceipt').on('keyup', function () {
        call();
    });

    $('#searchFieldHotel').on('keyup', function () {
        call();
    });

    $('#searchFieldClientname').on('keyup', function () {
        call();
    });
});