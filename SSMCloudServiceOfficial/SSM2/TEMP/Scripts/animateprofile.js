$(function () {

    var g1 = new JustGage({
        id: "gauge1",
        value: 6,
        min: 0,
        max: 28,
        title: "Holidays"
    });
    var g2 = new JustGage({
        id: "gauge2",
        value: 4,
        min: 0,
        max: 7,
        title: "Shifts",
        label: 'this Week'

    });

    $(window).resize(function () {
        $('#gauge1')[0].innerHTML = '';
        $('#gauge2')[0].innerHTML = '';
        var g1 = new JustGage({
            id: "gauge1",
            value: 6,
            min: 0,
            max: 28,
            title: "Holidays"
        });
        var g2 = new JustGage({
            id: "gauge2",
            value: 4,
            min: 0,
            max: 7,
            title: "Shifts",
            label: 'this Week'

        });
    });

});