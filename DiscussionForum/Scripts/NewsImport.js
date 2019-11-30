$("tr.news").click(function (args) {
    $("#overlay").show();
    $("#busyCursor").show();
    $("body").css("cursor", "none");
    var id = $($(args.currentTarget).find("td.malID")).text();
    $.ajax({
        url: "/Admin/NewsReport",
        data: {
            malId: id,
        },
        method:"post",
        success: function (data, status, xhr) {
            console.log(status);
            $("#articleGrid").html(data);
            $("#btnArticle").click();
            setTimeout(function () {
                $("#overlay").hide();
            }, 300);
            $("#busyCursor").hide();
            $("body").css("cursor", "initial");
        },
        error: function (xhr, status, error) {
            console.log("huge fail!!");
            $("#overlay").hide();
            $("#busyCursor").hide();
            $("body").css("cursor", "initial");
            alert(error);
        }

    });
});

$("#headCheckbox").change(function (args) {
    console.log($(this).val());
});

$("#testAction").click(function (args) {
    $(".secondSlide").css("width", "100px");
});
$("#showArticle").click(function (args) {
    showArticle();
});
$("#btnArticleBack").click(function (args) {
    hideArticle();
});
$("#btnArticleSave").click(function (args) {

    hideArticle();
});
function showArticle() {
    $("#articleContent").width(858);
    $("#saveNews").attr("disabled", "true");
}
function hideArticle() {
    $("#articleContent").width(0);
    $("#saveNews").removeAttr("disabled");
}