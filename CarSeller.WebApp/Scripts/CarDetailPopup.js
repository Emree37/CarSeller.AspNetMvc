$(function () {

    $('#modal_cardetail').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        carid = btn.data("car-id");

        $("#modal_cardetail_body").load("/Car/GetCarText/" + carid);
    })

});