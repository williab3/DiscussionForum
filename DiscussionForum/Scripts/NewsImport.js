var imageReader = new FileReader();
var selectedArticleRow;
imageReader.onload = function (imgArgs) {
    var newsPic = $("#articleImage > img");
    newsPic.attr("src", imgArgs.target.result).width(200);
};
imageReader.onerror = function (e) {
    console.log(e);
};

$("tr.news").click(function (args) {
    uiBusy();
    var id = $($(args.currentTarget).find("td.malID")).text();
    $.ajax({
        url: "/Admin/NewsReport",
        data: {
            malId: id,
        },
        method:"post",
        success: function (data, status, xhr) {
            console.log(status);
            //Show clicked anime related new articles grid
            $("#articleGrid").html(data);
            $("#btnArticle").click();
            uiUnlock();
            $("#headCheckbox").change(function (args) {
                console.log($(this).val());
            });
            $(".rowCheckbox").click(function (args) {
                args.stopPropagation();
            });
            //View Article details
            $(".articleRow").click(function (args) {
                selectedArticleRow = $($(args.currentTarget).closest("tr"));
                console.log(args);
                $("#articleModal-label").text($(selectedArticleRow).find(".relatedAnime").val());
                $("#articleImage > img").attr("src", $(selectedArticleRow.find(".gridRowImage")).attr("src"));
                $("#articleTitle").text($(selectedArticleRow).find("[data-article=title]").text());
                $("#articleText").text($(selectedArticleRow).find("[data-article=content]").val());
                $("#articleImage").click(function (args) {
                    $("#imgUploader").click();
                });
                $("#imgUploader").change(function (args) {
                    var imageFile = args.target.files[0];
                    if (!imageFile.type.match("image.*")) {
                        alert("The selected file is not an image. Please select an image file");
                        return;
                    } else {
                        imageReader.readAsDataURL(imageFile);
                    }
                });
                $("#titleEdit").val($(selectedArticleRow).find("[data-article=title]").text());

                $("#contentEdit").val($("#articleText").text()).hide();
                //Edit article content
                $("#articleText").click(function (args) {
                    $(this).hide();
                    $("#contentEdit").show();
                });
                //Edit article title
                $("#articleTitle").click(function (args) {
                    $(this).hide();
                    $("#editTitleContainer").show();
                });
                showArticle();
            });
            //Go back to article grid without saving
            $("#btnArticleBack").click(function (args) {
                hideArticle();
            });
            //Save article changes
            $("#btnArticleSave").click(function (args) {
                console.log(selectedArticleRow);
                $("#articleTitle").text($("#titleEdit").val());
                $("#articleText").text($("#contentEdit").val());
                selectedArticleRow.find(".gridRowImage").attr("src", $("#articleImage > img").attr("src"));
                $(selectedArticleRow.find("[data-article=title]")).text($("#titleEdit").val());
                $(selectedArticleRow).find("[data-article=content]").val($("#contentEdit").val());
                hideArticle();
            });
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
$("#showArticle").click(function (args) {
    showArticle();
});
function showArticle() {
    $("#articleContent").width(858);
    $("#saveNews").attr("disabled", "true");
}
function hideArticle() {
    $("#articleContent").width(0);
    $("#saveNews").removeAttr("disabled");
    $("#editTitleContainer").hide();
    $("#articleTitle").show();
    $("#articleText").show();
    $("#contentEdit").hide();
}



