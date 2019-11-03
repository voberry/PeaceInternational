"use strict";

//Column defination for InvoiceInfo Grid
const invoiceColumnDefs = [

    { headername: 'InvoiceNo', field: 'invoiceNo' },
    { headername: 'Dr', field: 'dr' },
    { headername: 'Agent Name', field: 'agentName' },
    { headername: 'Client Name', field: 'clientName' },
    { headername: 'Currency', field: 'currency' },
    { headername: 'PAX', field: 'pax' },
    {
        headername: 'Details',
        cellRenderer: function () {
            return '<i class="btn fas fa-clipboard" id="detailsButton"></i>';
        },
        onCellClicked(params) {
            console.log(params.data);
            InvoiceDetails(params.data.Id);
        }
    }
];


//Column defination for InvoiceDetails Grid
const invoiceDetailColumnDefs = [

    { headername: 'Particulars', field: 'particulars' },
    { headername: 'Amount', field: 'amount' }

];

//Function to set the data for the Invoice grid
const setInvoiceGridData = () => {

    $.ajax({
        url: 'Invoice/GetInvoice',
        method: 'GET',
        success: (data) => {
            gridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};


//Function to set the data for the Invoice Detail grid
const setInvoiceDetailGridData = (invoiceId) => {

    $.ajax({
        url: 'Invoice/GetInvoiceDetail',
        method: 'GET',
        data: { invoiceId: invoiceId },
        success: (data) => {
            invoiceDetailGridOptions.api.setRowData(data);
            console.log(data);
        }
    });
};

//Settings for the Invoice grid
const gridOptions = {
    columnDefs: invoiceColumnDefs,
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

//Settings for the Invoice Detail grid
const invoiceDetailGridOptions = {
    columnDefs: invoiceDetailColumnDefs,
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


//Invoice Details
const InvoiceDetails = (invoiceId) => {

    setInvoiceDetailGridData(invoiceId);

};

const itemListColumnDefs = [

    {
        headerName: 'Particulars', field: 'particulars', width: 450,
        cellStyle: () => {
            return { 'font-weight': 'bold', 'font-size':'16px' };
        }
    },
    {
        headerName: 'Amount', field: 'amount', width: 250, 
        cellStyle: () => {
            return { 'font-size': '16px' };
        }
    },
    {
        headerName: 'Remove', width: 150,
        cellRenderer: () => {
            return `<button type='button' class='btn btn-danger btn-sm m-1 shadow w-100'><i class='fas fa-times'></i></button>`;
        },
        onCellClicked(params) {

            itemListGridOptions.api.updateRowData({ remove: [params.data] });
            calcTotal();
        }
    }
];

const itemListGridOptions = {

    columnDefs: itemListColumnDefs,
    rowHeight: 50,
    rowData: false,
    getRowNodeId: function (data) { return data.Id; }
};

function getItemList() {

    let itemList = [];
    itemListGridOptions.api.forEachNode(function (node) {
        itemList.push(node.data);
    });
    console.log(itemList);
    return itemList;
}

function clear() {

    $('#particulars').val('');  
    $('#particularAmount').val('');   
}

const addParticularsValidation = () => {

    if ($('#particulars').val()) {
        if ($('#particularAmount').val()) {
            addParticulars();
        }
        else {
            noty({
                type: 'error',
                text: 'Amount cannot be empty',
                layout: 'center',
                timeout: 1000,
                killer: true
            });
        }
    }
    else {
        noty({
            type: 'error',
            text: 'Particulars cannot be empty',
            layout: 'center',
            timeout: 1000,
            killer: true
        });
    }    
};
    
const addParticulars = () => {

    const newData = {
        particulars: $('#particulars').val(),
        amount: parseInt($('#particularAmount').val())
    };

    itemListGridOptions.api.updateRowData({ add: [newData] });
    calcTotal();
    clear();    
};

function calcTotal() {

    let itemList = getItemList();
    let amount = 0;
    let discount = $('#discount').val();
    let netAmount = 0;

    $(itemList).each(function (idx, item) {
        amount += item.amount;
    });

    netAmount = amount - discount;    
    if ($('#discount').val() > amount) {
        $('#discount').addClass('is-invalid');
    }
    else {
        $('#discount').removeClass('is-invalid');
    }

    $('#amount').val(amount);
    $('#netAmount').val(netAmount);

    return [amount, netAmount];

}

$(document).ready(function () {

    var itemListGrid = document.querySelector('#itemListGrid');
    new agGrid.Grid(itemListGrid, itemListGridOptions);

    $('#addProduct').on('click', function () {
        addParticularsValidation();
    });

    $('#discount').on('keyup', () => { calcTotal(); });

});


