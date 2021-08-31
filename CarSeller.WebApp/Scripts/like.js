$(function () {

    var carids = [];

    $("div[data-car-id]").each(function (i, e) {
        carids.push($(e).data("car-id"));
    });

    $.ajax({
        method: "POST",
        url: "/Car/GetLiked",
        data: { ids: carids }
    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedNote = $("div[data-car-id=" + id + "]");
                var btn = likedNote.find("button[data-liked]");
                var span = btn.find("span.like-star");

                btn.data("liked", true);
                span.removeClass("glyphicon-star-empty");
                span.addClass("glyphicon-star");
            }

        }

    }).fail(function () {

    });



    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var carid = btn.data("car-id");
        var spanStar = btn.find("span.like-star");
        var spanCount = btn.find("span.like-count");

        console.log(liked);
        console.log("like count : " + spanCount.text());

        $.ajax({
            method: "POST",
            url: "/Car/SetLikeState",
            data: { "carid": carid, "liked": !liked }
        }).done(function (data) {

            console.log(data);

            if (data.hasError) {
                alert(data.errorMessage);
            } else {
                liked = !liked;
                btn.data("liked", liked);
                spanCount.text(data.result);

                console.log("like count(after) : " + spanCount.text());


                spanStar.removeClass("glyphicon-star-empty");
                spanStar.removeClass("glyphicon-star");

                if (liked) {
                    spanStar.addClass("glyphicon-star");
                } else {
                    spanStar.addClass("glyphicon-star-empty");
                }

            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        })

    });
});