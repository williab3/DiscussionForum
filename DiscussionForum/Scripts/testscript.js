$("#gridCount").val($("#RequestVariables_PerPage").val());
//Page down click
$("#btnPageLeft").click(function (args) {
    var pageNumber = $("#pageNumber");
    console.log("The current page number is " + $("#gridPage").val());
    if (Number($("#gridPage").val()) === 1) {
        pageNumber.val(1);
        console.log("We will send " + $("#gridPage").val() + " back to the controller");
    } else {
        pageNumber.val(Number($("#gridPage").val()) - 1);
        console.log("We have subtracted 1 from the page number so the new number is " + pageNumber.val());
    }
});
//Page up click
$("#btnPageRight").click(function (args) {
    var pageNumber = $("#pageNumber");
    pageNumber.val(Number($("#gridPage").val()) + 1);
}); //TODO:Fix bottom page up click

//Event handler for changing the number of rows in the grid.
$(".rowCount").change(function (args) {
    $("#form0").submit();
    console.log("New Count: " + $(this).val());
});
//Title row check box event handler 
$("#checkAll").change(function (args) {
    if (this.checked === true) {
        $(".gridRowCheckbox").each(function (index, element) {
            if ($(element).is(":enabled")) {
                element.checked = true;
                $(element).change();
                $(element).closest("tr").addClass("checkedRow");
            }
        });
    } else {
        $(".gridRowCheckbox").each(function (index, element) {
            if ($(element).is(":enabled")) {
                element.checked = false;
                $(element).closest("tr").removeClass("checkedRow");
            }
        });
        console.log($(".gridRowCheckbox"));
        
    }
});

var checkAnime = [];
function getGenres(args) {
    var genreList = [];
    var spans = $(args.closest("td").siblings("td")[4]).find("span.genreListItem");
    spans.each(function (index, element) {
        genreList[genreList.length] = { GenreName: element.innerText };
    });
    return genreList;

}

//Individual row check box event handler
$(".gridRowCheckbox").change(function (args) {
    if (this.checked === true) {
        //Check to see if the anime is already in the data set
        var _anilistId = $(this).closest("tr").attr("data-anime-id");
        var pos = checkAnime.map(function (args) {
            return args.AnilistId;
        }).indexOf(_anilistId);
        if (pos < 0) {
            var currentCheckbox = $(this);
            var descri = document.createElement("div");
            descri.innerHTML = $(currentCheckbox.closest("td").siblings("td")[1]).find(".anime-description").val();
            checkAnime[checkAnime.length] = {
                AnilistId: currentCheckbox.closest("tr").attr("data-anime-id"),
                Title_English: currentCheckbox.closest("td").siblings("td")[2].firstElementChild.innerHTML,
                Title_Romaji: currentCheckbox.closest("td").siblings("td")[3].firstElementChild.innerHTML,
                Description: descri.textContent,
                ImageUrlLarge: $(currentCheckbox.closest("td").siblings("td")[1]).find(".anime-largeImage").val(),
                ImageUrlMedium: $(currentCheckbox.closest("td").siblings("td")[1]).find("img").attr("src"),
                Color: $(currentCheckbox.closest("td").siblings("td")[1]).find("img").css("background-color"),
                Genres: getGenres(currentCheckbox),
                LastUpdated: $(currentCheckbox.closest("td").siblings("td")[1]).find(".anime-lastUpdated").val(),
                MalId: $(currentCheckbox.closest("td").siblings("td")[1]).find(".anime-MalId").val(),
                Popularity: $(currentCheckbox.closest("td").siblings("td")[5]).text(),
                StartDate: $(currentCheckbox.closest("td").siblings("td")[1]).find(".anime-startDate").val(),
                ExistInDB: false
            };
            console.log(checkAnime);
            $(this).closest("tr").addClass("checkedRow");
        }
    } else {
        //Remove from the data set
        var anilistId = $(this).closest("tr").attr("data-anime-id");
        if (checkAnime !== undefined) {
            var deselectedIndex = checkAnime.map(function (args) {
                return args.AnilistId;
            }).indexOf(anilistId);
            if (deselectedIndex > -1) {
                checkAnime.splice(deselectedIndex, 1);
                $(this).closest("tr").removeClass("checkedRow");
            }
            console.log(checkAnime);
        }
    }
});
//Preview data summary in modal before saving it
$(".saveSelected").click(function (args) {
    var anime = [];
    var genres = [];
    var tags = [];
    $(checkAnime).each(function (index, animeModel) {
        anime[anime.length] = {
            romanji: animeModel.Title_Romaji,
            english: animeModel.Title_English
        };
        var romanjiTagInex = $.inArray(animeModel.Title_Romaji, tags);
        if (romanjiTagInex < 0 && animeModel.Title_Romaji !== "") {
            tags[tags.length] = animeModel.Title_Romaji;
        }
        var englishTagIndex = $.inArray(animeModel.Title_English, tags);
        if (englishTagIndex < 0 && animeModel.Title_English !== "") {
            tags[tags.length] = animeModel.Title_English;
        }
        $(animeModel.Genres).each(function (index, genreItem) {
            var genreIndex = $.inArray(genreItem.GenreName, genres);
            if (genreIndex < 0 && genreItem.GenreName !== "") {
                genres[genres.length] = genreItem.GenreName;
            }
            var genreTagIndex = $.inArray(genreItem.GenreName, tags);
            if (genreTagIndex < 0) {
                tags[tags.length] = genreItem.GenreName;
            }
        });
    });
    var freshANimeTable = $("#freshAnimeTable > tbody");
    $(anime).each(function (index, element) {
        var row = $("<tr>");
        var cell = $("<td>");
        cell.html(element.romanji);
        if (element.english !== null && element.english !== "") {
            var subtitle = $("<sub>").text(element.english).addClass("subscript");
            cell.append(subtitle);
        }
        row.html(cell);
        freshANimeTable.append(row);
    });
    var freshGenreTable = $("#freshGenreTable > tbody");
    $(genres).each(function (index, elementr) {
        var row = $("<tr>");
        var cell = $("<td>");
        cell.html(elementr);
        row.html(cell);
        freshGenreTable.append(row);
    });
    $("#tagCount").text(tags.length + " possible new tags");
    console.log(tags);
});

$("#gridSaveSummary").on("hidden.bs.hidden.bs.modal", function () {
    var freshGenreTable = $("#freshGenreTable > tbody");
    var freshANimeTable = $("#freshAnimeTable > tbody");
    freshGenreTable.empty();
    freshANimeTable.empty();
    $("#tagCount").empty();
    $("#btn-saveGridData").removeAttr("disabled");
});
//Save button event handler
$("#btn-saveGridData").click(function (args) {
    var uploadBtn = $(this);
    if (checkAnime === undefined) {
        console.log("No rows have been selected");
    } else {
        $.ajax({
            url: "/Admin/SaveNewAnime",
            data: { checkedAnime: checkAnime },
            dataType: "json",
            method: "Post",
            success: function (data, status, xhr) {
                uploadBtn.attr("disabled", "");
                var freashAnimeTable = $("#freshAnimeTable > tbody > tr").addClass("dangerTextelement");
                var freshGenreTable = $("#freshGenreTable > tbody > tr").addClass("dangerTextelement");
                var animeData = JSON.parse(data);
                console.log(animeData.NewAnimeTitles);
                $(animeData.NewAnimeTitles).each(function (index, title) {
                    let match = freashAnimeTable.find("td").filter(function (i) { return this.innerText.includes(title); });
                    if (match !== undefined) {
                        let cell = $("<td>").text(title);
                        match.closest("tr").removeClass("dangerTextelement").addClass("goodTextelement").append(cell);
                    }
                });
                $(animeData.NewGenres).each(function (index, genre) {
                    let match = freshGenreTable.find("td").filter(function (i) { return this.innerText.includes(genre); });
                    if (match !== null) {
                        let cell = $("<td>").text(genre);
                        match.closest("tr").removeClass("dangerTextelement").addClass("goodTextelement").append(cell);
                    }
                });
                var actualTag = $("<p>").text(animeData.FreshTags.length + " tags were actually created.");
                $("#summaryBody").append(actualTag);
            },
            error: function (xhr, status, err) {
                alert($(xhr.responseText).text());
            }
        });
    }
});
//Row click event handler
var selectedRow;
$(".gridRow").click(function (args) {
    if (args.target.localName !== "input") {
        $("#btn-openDetails").click();
        console.log(args);
        selectedRow = $(this).closest("tr");
        var englishTitle = selectedRow.children("td")[3].innerText;
        var romanjiTitle = selectedRow.children("td")[4].innerText;
        var startDate = $(selectedRow.children("td")[2]).find("input.anime-startDate").val();
        var premise = $(selectedRow.children("td")[2]).find("input.anime-description").val();
        $("#EnglishTitle-txtBx").val(englishTitle);
        $("#romanjiTitle-txtBx").val(romanjiTitle);
        $("#premise-txtBx").val(premise);
        if (englishTitle.length > 1 && englishTitle.length < 5) {
            let titleWarning = $("<p>").text("This title is too short and violates Discussion Title business rules. The title must be at least 5 characters long.").addClass("text-warning");
            $("#englishHeader").append(titleWarning);
        } else if (englishTitle.length < 1 && romanjiTitle.length < 5) {
            let titleWarning = $("<p>").text("This title is too short and violates Discussion Title business rules. The title must be at least 5 characters long.").addClass("text-warning");
            $("#romanjiHeader").append(titleWarning);
        } 
        if (premise.length < 20 || premise.length > 2000) {
            var descriptionWarning = $("<p>").text("The description violates the Discussion Premise business rules. The description must be at between 20 and 2000 characters long").addClass("text-warning");
            $("#details-body").append(descriptionWarning);
        }

        $("#gridItemDetails-label").text(romanjiTitle);
        $("#details-EnglishTitle").text(englishTitle);
        $("#details-startDate").text(" |Start Date: "+ startDate);
        $("#details-Img").attr("src", $(selectedRow.children("td")[2]).find("img").attr("src"));
        $("#details-RomanjiTitle").text(romanjiTitle);
        $("#details-description").text($(selectedRow.children("td")[2]).find("input.anime-description").val());
        console.log(selectedRow.children("td"));
    } else {
        console.log("You just clicked the checkbox!!");
    }
});

//Show text box for English title 
$("#details-EnglishTitle").click(function (args) {
    console.log(this);
    $("#EnglishTitle-txtBx").val($(this).text()).show();
    $(this).hide();
});

//Show text box for anime description
$("#details-description").click(function (args) {
    $("#premise-txtBx").val($(this).text()).show();
    $(this).hide();
});

//Show text box for Romanji title
$("#details-RomanjiTitle").click(function (args) {
    $("#romanjiTitle-txtBx").val($(this).text()).show();
    $(this).hide();
});

//Event handler for Romanji title text change
$("#romanjiTitle-txtBx").keyup(function (args) {
    $("#gridItemDetails-label").text($(this).val());
});

//Save changes made to anime details
$("#detailsSave-btn").click(function (args) {
    var romanjiLabel = $("#details-RomanjiTitle");
    var englishLabel = $("#details-EnglishTitle");
    var descriptionLabel = $("#details-description");
    var isEnglishTitleValid = true;
    var isRomanjiTitleValid = true;
    var isDescrioptionValid = true;
    //Save changes to romanji title
    $(selectedRow.children("td")[4]).find("h4").text($("#romanjiTitle-txtBx").val().trim());
    romanjiLabel.text($("#romanjiTitle-txtBx").val().trim());
    //Save changes to English title
    $(selectedRow.children("td")[3]).find("h4").text($("#EnglishTitle-txtBx").val().trim());
    englishLabel.val($("#EnglishTitle-txtBx").val().trim());
    //Save changes to description
    $(selectedRow.children("td")[2]).find("input.anime-description").val($("#premise-txtBx").val().trim());
    descriptionLabel.text($("#premise-txtBx").val().trim());
    //Evaluate for validity
    if (englishLabel.text().length > 0 && englishLabel.text().length < 5) {
        let titleWarning = $("<p>").text("This title is too short and violates Discussion Title business rules. The title must be at least 5 characters long.").addClass("text-warning");
        $("#englishHeader").append(titleWarning);
        isEnglishTitleValid = false;
    } else if (englishLabel.text().length > 5) {
        isEnglishTitleValid = true;
    }
    if (englishLabel.text().length < 1 && romanjiLabel.text().length < 5) {
        let titleWarning = $("<p>").text("This title is too short and violates Discussion Title business rules. The title must be at least 5 characters long.").addClass("text-warning");
        $("#romanjiHeader").append(titleWarning);
        isRomanjiTitleValid = false;
    } else if (englishLabel.text().length < 1 && romanjiLabel.text().length > 5) {
        isRomanjiTitleValid = true;
    }
    if (descriptionLabel.text().length < 20 || descriptionLabel.text().length > 2000) {
        isDescrioptionValid = false;
        var descriptionWarning = $("<p>").text("The description violates the Discussion Premise business rules. The description must be at between 20 and 2000 characters long").addClass("text-warning");
        $("#details-body").append(descriptionWarning);
    }
    if (isDescrioptionValid && isEnglishTitleValid && isRomanjiTitleValid) {
        selectedRow.removeClass("rowWarning");
        $(selectedRow.children("td")[1]).find("input").prop("disabled", false);
    } else {
        selectedRow.addClass("rowWarning");
        $(selectedRow.children("td")[1]).find("input").prop("checked", false).prop("disabled", true);
    }
    //Put everything back
    $(".field").val("").hide();
    $("#details-RomanjiTitle").show();
    $("#details-EnglishTitle").show();
    $("#details-description").show();
    console.log(selectedRow);
});
//Event handler for hiding the anime details modal
$("#gridItemDetails").on("hidden.bs.modal", function (args) {
    $(".field").val("").hide();
    $("#details-RomanjiTitle").show();
    $("#details-EnglishTitle").show();
    $("#details-description").show();
    $(".text-warning").remove();
});

