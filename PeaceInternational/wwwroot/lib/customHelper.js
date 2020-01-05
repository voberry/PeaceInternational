Handlebars.registerHelper('splitDate', function (date) {
    return date.split('T')[0];
});

Handlebars.registerHelper("inc", function (value, options) {
    return parseInt(value) + 1;
});

Handlebars.registerHelper("inWords", function (num, options) {
 var th = ['','Thousand','Million', 'Billion','Trillion'];
var dg = ['Zero','One','Two','Three','Four', 'Five','Six','Seven','Eight','Nine'];
 var tn = ['Ten','Eleven','Twelve','Thirteen', 'Fourteen','Fifteen','Sixteen', 'Seventeen','Eighteen','Nineteen'];
 var tw = ['Twenty','Thirty','Forty','Fifty', 'Sixty','Seventy','Eighty','Ninety'];

    var s = num;
    s = s.toString();
    s = s.replace(/[\, ]/g,'');
    if (s != parseFloat(s)) return 'not a number';
    var x = s.indexOf('.');
    if (x == -1)
        x = s.length;
    if (x > 15)
        return 'too big';
    var n = s.split(''); 
    var str = '';
    var sk = 0;
    for (var i=0;   i < x;  i++) {
        if ((x-i)%3==2) { 
            if (n[i] == '1') {
                str += tn[Number(n[i+1])] + ' ';
                i++;
                sk=1;
            } else if (n[i]!=0) {
                str += tw[n[i]-2] + ' ';
                sk=1;
            }
        } else if (n[i]!=0) { // 0235
            str += dg[n[i]] +' ';
            if ((x-i)%3==0) str += 'hundred ';
            sk=1;
        }
        if ((x-i)%3==1) {
            if (sk)
                str += th[(x-i-1)/3] + ' ';
            sk=0;
        }
    }

    if (x != s.length) {
        var y = s.length;
        str += 'point ';
        for (var i=x+1; i<y; i++)
            str += dg[n[i]] +' ';
    }
    return str.replace(/\s+/g,' ') + 'Only';
});

Handlebars.registerHelper('times', function (n, block) {
    var accum = '';
    for (var i = 0; i < n; ++i)
        accum += block.fn(i);
    return accum;
});

Handlebars.registerHelper('for', function (from, to, incr, block) {
    var accum = '';
    for (var i = from; i < to; i += incr)
        accum += block.fn(i);
    return accum;
});

Handlebars.registerHelper('distanceFixed', function (distance) {
    return distance.toFixed(2);
});

Handlebars.registerHelper('customDate', function (date) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'];
    var splittedDate = date.split('-');
    console.log(splittedDate);
    var monthName = months[splittedDate[1] - 1];
    return `${splittedDate[2]}/${monthName}/${splittedDate[0]}`;

});

Handlebars.registerHelper('chain', function () {
    var helpers = [], value;
    $.each(arguments, function (i, arg) {
        if (Handlebars.helpers[arg]) {
            helpers.push(Handlebars.helpers[arg]);
        } else {
            value = arg;
            $.each(helpers, function (j, helper) {
                value = helper(value, arguments[i + 1]);
            });
            return false;
        }
    });
    return value;
});

Handlebars.registerHelper('percentage', function (a, b) {

    return (100 - b) / 100 * a;

})

Handlebars.registerHelper('customDivision', function (a, b, c) {

    let result = (100 - b) / 100 * a;
    return Math.ceil(result/c);

})


