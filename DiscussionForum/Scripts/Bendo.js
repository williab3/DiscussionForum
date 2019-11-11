//Bendo Grid
$.fn.bendoGrid = function (args) {
    let defaultSettings = $.extend(true, {
        read: {
            data: {}
        },
        columns: [
            { title: "column 1", dataField: "", width: "30%" },
            { title: "column 2", dataField: "", width: "30%" },
            { title: "column 3", dataField: "", width: "30%" },
        ],
        style: {
            hover: true,
            bordered: false,
            class: ["table"],
            bootstrapTheme: {
                fileUrl: "",
                useInverseTheme: false,
            }
        },
    }, args);
    if (args.columns !== null) {
        defaultSettings.columns = args.columns;
    }
    console.log(defaultSettings);

    //Process grid arguments
    if (defaultSettings.style.hover === true) {
        defaultSettings.style.class.push("table-hover");
    }
    if (defaultSettings.style.bordered === true) {
        defaultSettings.style.class.push("table-bordered");
    }
    if ($.inArray("table", defaultSettings.style.class) < 0) {
        defaultSettings.style.class.push("table");
    }

    //Make Grid
    let grid = $("<table>").html($("<thead>").html($("<tr>")));
    let gridHead = grid.find("thead");
    let gridBody = $("<tbody>");
    gridBody.insertAfter(gridHead);
    $.each(defaultSettings.style.class, function (index, cssClass) {
        grid.addClass(cssClass);
    });
    //Make title row
    $.each(defaultSettings.columns, function (index, column) {
        grid.find("thead>tr").append($("<th>").text(column.title).attr("width", column.width));
    });
    // Set styles
    if (defaultSettings.style.bootstrapTheme.fileUrl !== "" && defaultSettings.style.bootstrapTheme.useInverseTheme === true) {
        $.get(defaultSettings.style.bootstrapTheme.fileUrl, function (data) {
            var inverseClassSearch = /navbar-inverse\s+{(\s|\n)(^.*;\n)+}/m;
            var classResult = data.match(inverseClassSearch);
            var bgColorSearch = /(background-color:)\s+(#+(\d|\w){3,10})/;
            var bgColorResult = classResult[0].match(bgColorSearch);
            gridHead.css("background-color", bgColorResult[2]);
            var colorSearch = /\.navbar-inverse\s?\.navbar-text\s?{\n(.*;\n)+}/;
            var colorClass = data.match(colorSearch);
            var textColor = colorClass[0].match(/(color:\s?)(#?(\d|\w){3,10})/);
            console.log(textColor);
            gridHead.css("color", textColor[2]);
        }, "text");
    }

    //Get the click actions
    if (defaultSettings.rowClick) {
        var rowClick = defaultSettings.rowClick;
    }
    //Make rows 
    var gridData = defaultSettings.read.data;
    for (var i = 0; i < gridData.length; i++) {
        var row = $("<tr>");
        
        $(row).attr("data-bendo-id", gridData[i][defaultSettings.read.idField]);
        //Get the data field  columns
        var columns = defaultSettings.columns;
        $(columns).each(function (index, _column) {
            //Make a <td> for each column
            var td = $("<td>").attr("data-bendo-field", _column.dataField);
            row.append(td);

            //Check if a custom template is used
            if (typeof _column.format == "undefined") {
                console.log("set up standard format row");
            }
            else {
                var ltemTemplate;
                var linkAction = false;
                //Get custom template
                var template = _column.format;
                //Bind template fields with the appropriate data items.
                var search = /(%#(\d|\w|\.)*#%)/g;
                var dataItems = template.match(search);
                $.each(dataItems, function (index, item) {
                    var subString = item.replace(/%#data./, "").replace(/#%/, "");
                    template = template.replace(new RegExp(item), gridData[i][subString]);
                });
                //Set the link to the click action
                for (var clickIndex = 0; clickIndex < rowClick.itemClick.length; clickIndex++) {
                    if (rowClick.itemClick[clickIndex].field == _column.dataField && rowClick.itemClick[clickIndex].id == gridData[i][defaultSettings.read.idField]) {
                        linkAction = rowClick.itemClick[clickIndex].action();
                    } 
                }
                //Add template to table cell
                if (linkAction) {
                    ltemTemplate = linkAction.replace(search, template);
                    td.html(ltemTemplate);
                    linkAction = false;
                } else {
                    td.html(template);
                }

                //$(rowClick.itemClick).each(function (rowIndex, dataClickItem) {
                //    if (dataClickItem.field == _column.dataField && dataClickItem.id == gridData[i][defaultSettings.read.idField]) {
                //        linkAction = dataClickItem.action();
                //        var linkTemplate = linkAction.replace(search, template);
                //        td.html(linkTemplate);
                //        return false;
                //    } else {
                //        td.html(template);
                //        return false;
                //    }
                //});
            }
            gridBody.append(row);
        });

    }
    this.append(grid);
};

//Bendo Checkbox
$.fn.bendoCheckbox = function (args) {
    console.log(this);
    var defaultSettings = $.extend({
        setCheck: "",
        height: "25",
        postUpdate: function (args) {
            $.ajax({
                method: this.postBack.method,
                url: this.postBack.url,
                data: args
            });
        },
        uncheckAction: function (args) {
            console.log("uncheck fucntion not set");
        },
        checkAction: function (args) {
            console.log("check function not set");
        },
    }, args);
    this.settings = new function () {
        return defaultSettings;
    };
    this.each(function (i, element) {
        var dataChecked = $(element).attr("data-bendo-checked");
        if (defaultSettings.setCheck === "true" || dataChecked === "true") {
            $(element).attr("data-bendo-checked", "true");
            $(element).addClass("bendoCheckbox").addClass("text-center");
            let holder = $("<div>").addClass("checkbox-holder");
            let checkedImg = $("<img>").attr("height", defaultSettings.height).attr("data-bendo-display", "show").attr("src", defaultSettings.checkedImagUrl).addClass("checkedImg");
            holder.append(checkedImg);
            let uncheckedImg = $("<img>").attr("height", defaultSettings.height).attr("data-bendo-display", "").attr("src", defaultSettings.uncheckedImageUrl).addClass("uncheckedImg");
            holder.append(uncheckedImg);
            $(element).prepend(holder);
        } else {
            $(element).addClass("bendoCheckbox").addClass("text-center");
            let holder = $("<div>").addClass("checkbox-holder");
            let checkedImg = $("<img>").attr("height", defaultSettings.height).attr("data-bendo-display", "").attr("src", defaultSettings.checkedImagUrl).addClass("checkedImg");
            holder.append(checkedImg);
            let uncheckedImg = $("<img>").attr("height", defaultSettings.height).attr("data-bendo-display", "show").attr("src", defaultSettings.uncheckedImageUrl).addClass("uncheckedImg");
            holder.append(uncheckedImg);
            $(element).prepend(holder);
        }
    });
    var allCheckboxes = this;
    this.data("bendoCheckbox", {
        toggleCheck: function (itemId) {
            var currentCheckbox = $(allCheckboxes).closest("[data-bendo-itemId='" + itemId + "']");
            var _attribute = $(currentCheckbox).attr("data-bendo-checked");
            if (_attribute == "true") {
                $(allCheckboxes).attr("data-bendo-checked", "");
                let checkedImg = $(currentCheckbox).find("img.checkedImg");
                checkedImg.attr("data-bendo-display", "");
                let uncheckedImg = $(currentCheckbox).find("img.uncheckedImg");
                uncheckedImg.attr("data-bendo-display", "show");
            } else {
                $(currentCheckbox).attr("data-bendo-checked", "true");
                let checkedImg = $(currentCheckbox).find("img.checkedImg");
                checkedImg.attr("data-bendo-display", "show");
                let uncheckedImg = $(currentCheckbox).find("img.uncheckedImg");
                uncheckedImg.attr("data-bendo-display", "");
            }
        },
        setToCheck: function (itemId) {
            var currentCheckbox = $(allCheckboxes).closest("[data-bendo-itemId='" + itemId + "']");
            $(currentCheckbox).attr("data-bendo-checked", "true");
            let checkedImg = $(currentCheckbox).find("img.checkedImg");
            checkedImg.attr("data-bendo-display", "show");
            let uncheckedImg = $(currentCheckbox).find("img.uncheckedImg");
            uncheckedImg.attr("data-bendo-display", "");
        },
        setToUnCheck: function (itemId) {
            var currentCheckbox = $(allCheckboxes).closest("[data-bendo-itemId='" + itemId + "']");
            console.log($(currentCheckbox).closest("[data-bendo-itemId='4']"));
            $(currentCheckbox).attr("data-bendo-checked", "");
            let checkedImg = $(currentCheckbox).find("img.checkedImg");
            checkedImg.attr("data-bendo-display", "");
            let uncheckedImg = $(currentCheckbox).find("img.uncheckedImg");
            uncheckedImg.attr("data-bendo-display", "show");
        }
    });
    this.click(function (e, sender) {
        var _attribute = $(this).attr("data-bendo-checked");
        console.log(this);
        var clickObj;
        if ($(this).attr("data-bendo-itemId") !== null) {
            clickObj = {
                itemId: $(this).attr("data-bendo-itemId"),
                checkbox: this
            };
        } else {
            clickObj = {
                itemId: "",
                checkbox: this
            };
        }
        if (_attribute == "true") {
            $(this).attr("data-bendo-checked", "");
            let checkedImg = $(this).find("img.checkedImg");
            checkedImg.attr("data-bendo-display", "");
            let uncheckedImg = $(this).find("img.uncheckedImg");
            uncheckedImg.attr("data-bendo-display", "show");
            defaultSettings.uncheckAction(clickObj);
        } else {
            $(this).attr("data-bendo-checked", "true");
            let checkedImg = $(this).find("img.checkedImg");
            checkedImg.attr("data-bendo-display", "show");
            let uncheckedImg = $(this).find("img.uncheckedImg");
            uncheckedImg.attr("data-bendo-display", "");
            defaultSettings.checkAction(clickObj);
            }
    });
};

//Bendo Select list
$.fn.bendoSelect = function (args) {
    let defaultSettings = $.extend(true, {
        placeholder: "Placeholder",
        direction: "down",
        width: "230px",
        id: this.attr("id"),
        style: {
            bootstrapTheme: {
                fileUrl: "",
                useInverseTheme: false,
            }
        }
    }, args);
    this.attr("id", defaultSettings.id + "-bendoSelect").width(defaultSettings.width);
    let mySelect = this;
    let selectContainer = $("<div>").addClass("bendo-select").width(defaultSettings.width);

    // Set styles
    var bgColorResult;
    var textColor;
    if (defaultSettings.style.bootstrapTheme.fileUrl !== "" && defaultSettings.style.bootstrapTheme.useInverseTheme === true) {
        $.get(defaultSettings.style.bootstrapTheme.fileUrl, function (data) {
            let inverseClassSearch = /navbar-inverse\s+{(\s|\n)(^.*;\n)+}/m;
            let classResult = data.match(inverseClassSearch);
            let bgColorSearch = /(background-color:)\s+(#+(\d|\w){3,10})/;
            bgColorResult = classResult[0].match(bgColorSearch);
            mySelect.css("background-color", bgColorResult[2]);
            let colorSearch = /\.navbar-inverse\s?\.navbar-text\s?{\n(.*;\n)+}/;
            let colorClass = data.match(colorSearch);
            textColor = colorClass[0].match(/(color:\s?)(#?(\d|\w){3,10})/);
            console.log(textColor);
            mySelect.css("color", textColor[2]);
        }, "text");
    }
    //Set custom icon
    if (defaultSettings.icon) {
        let icon = $("<span>").html(defaultSettings.icon).addClass("icon");

        selectContainer.append(icon);
    }

    let body = $("<div>").attr("id", defaultSettings.id + "-selectBody").width(defaultSettings.width).css("position", "relative");
    let selectPlacholder = $("<p>").text(defaultSettings.placeholder);
    selectPlacholder.attr("id", defaultSettings.id + "-bendoSelectedItem");
    body.prepend(selectPlacholder);
    selectContainer.append(body);
    let chevron = $("<span>").addClass("chevron").html("&#xe114;");
    selectContainer.append(chevron);
    this.append(selectContainer);

    //Add select option items
    let selectOptions = $("<div>").width(defaultSettings.width).addClass("selectOptions").css("max-height", "0px").attr("id", defaultSettings.id + "-bendoSlectList");
    let optionsContainer = $("<div>");
    let dataItems = $(defaultSettings.items.dataArray);
    dataItems.each(function (index, item) {
        //Create a selection item
        let listItem = $("<div>").attr("data-select-value", item[defaultSettings.items.valueField]).addClass("bendo-selectItem");
        //Add hover effect to selection item
        listItem.hover(function (args) {
            listItem.css("background-color", bgColorResult[2]).css("color", textColor[2]);
        }, function (args) {
            listItem.removeAttr("style");
            });
        var contents = $("<p>").text(item[defaultSettings.items.textField]);
        //Add selection click event
        listItem.click(function (args) {
            selectPlacholder.text(contents.text());
            selectPlacholder.attr("data-selected-value", listItem.attr("data-select-value"));
            //User defined click event handler
            if (defaultSettings.selectionChange !== null) {
                var friendId = selectPlacholder.attr("data-selected-value");
                defaultSettings.selectionChange(friendId);
            }
            mySelect.focusout();
        });
        listItem.html(contents);
        selectOptions.prepend(listItem);
        selectOptions.append(optionsContainer);
    });
    body.append(selectOptions);
    //Set open direction
    if (defaultSettings.direction === "down") {
        selectOptions.addClass("bendo-dropdown");
    } else {
        selectOptions.addClass("bendo-dropup");
        //body.prepend(selectOptions);
    }
    $(this).attr("tabindex", '0');

    this.click(function (args) {
        this.focus();
        selectOptions.css("max-height", "500px").css("padding", "5px 3px 0px 5px");
        chevron.css("transform", "rotate(180deg)").css("padding", "0px");
        $("#bendoSlectList").show();
    });

    this.focusout(function (args) {
        selectOptions.css("max-height", "0px").css("padding", "0px");
        chevron.css("transform", "rotate(0deg)");
        $("#bendoSlectList").hide();
    });

};

//Bendo Instant Messenger for .NET SignalR
$.fn.bendoNetMessenger = function (args) {
    let defaultSettings = $.extend(true, {
        placeholder: "Placeholder",
    }, args);

    this.addClass("messages");
    var contactList = $("<div>").addClass("message-selector").attr("id", "bendo-messengerSelect");
    let bendoMessenger = this;

    //Create the friends list
    contactList.bendoSelect({
        icon: "&#xe008;",
        direction: "up",
        width: "265px",
        placeholder: defaultSettings.placeholder,
        style: {
            bootstrapTheme: {
                fileUrl: "/Content/Darkly3.3.7.css",
                useInverseTheme: true
            }
        },
        items: {
            textField: defaultSettings.contactList.textField,
            valueField: defaultSettings.contactList.valueField,
            dataArray: defaultSettings.contactList.dataArray
        },
        selectionChange: defaultSettings.selectionChange
    });
    this.append(contactList);
    var messegeBox = $("<div>").attr("id", "bendo-messegeBox").addClass("messageBox").scrollTop(50);
    this.append(messegeBox);
    var messageInputContainer = $("<div>").attr("id", "input-container").css("display", "flex").css("position", "fixed");
    var messageTexbox = $("<input>").attr("type", "text").width("230px").attr("id", "userMessage");
    messageInputContainer.append(messageTexbox);
    var sendButton = $("<button>").addClass("btn").addClass("btn-primary").addClass("btn-sm").attr("type", "button").attr("id", "btn-sendChat");
    var icon = $("<span>").addClass("glyphicon").addClass("glyphicon-send");
    sendButton.html(icon);
    messageTexbox.keydown(function (args) {
        if (args.keyCode === 13) {
            sendButton.click();
        }
    });
    messageInputContainer.append(sendButton);
    this.append(messageInputContainer);

    //Open Chat connection
    if (defaultSettings.proxyHub !== null) {
        //Define a function for the server to call to push a message to the client
        defaultSettings.proxyHub.client.broadcastMessage = function (messege) {
            let incomingMessage = $("<p>").addClass("incoming-message").text(messege);
            messegeBox.append(incomingMessage);
        };
        $.connection.hub.start().done(function () {
            var recipient = $("#bendo-messengerSelect-bendoSelectedItem");
            sendButton.click(function (args) {
                //Call to server when the send button is clicked
                defaultSettings.proxyHub.server.send(recipient.attr("data-selected-value"), messageTexbox.val());
                var outgoingMessege = $("<p>").addClass("outgoing-message").addClass("text-primary").text(messageTexbox.val());
                messegeBox.append(outgoingMessege);
                messageTexbox.val(" ");
            });
            console.log("Connected bro!!");
        }).fail(function () {
            alert("Unable to connect to SignalR");
        });
    }

};

// Bendo Auto-complete
$.fn.bendoAutocomplete = function (args) {
    //TODO: Properly style the autocomplete
    var defaultSettings = $.extend(true, {
        icon: "&#xe003;",
        errorMessage: "Please select an item from the autocomplete list before clicking the autocomplete button.",
    }, args);
    var container = $("<div>").attr("id", "autocomplete-container-" + this.attr("id")).css("display", "flex");
    this.before(container).addClass("bendo-autocomplete");
    if (defaultSettings.placeholder !== null) {
        this.attr("placeholder", defaultSettings.placeholder);
    } 
    container.append(this);
    var bendoAutocomplete = this;

    //Make button for the autocomplete
    var autocompleteButton = $("<button>").attr("type", "button").addClass("btn-primary bendo-icon");
    var buttonSpan = $("<span>").html(defaultSettings.buttonText);
    autocompleteButton.append(buttonSpan);
    //Attach the click event to the button
    autocompleteButton.click(function (args) {
        if (bendoAutocomplete.val() != "") {
            var freshTag = $("<span>").text(bendoAutocomplete.val()).attr("data-bendo-itemId", bendoAutocomplete.attr("data-autocomplete-value")).addClass("attachedTags");
            var idValue = "";
            idValue = bendoAutocomplete.attr("data-autocomplete-value");
            if (idValue !== undefined && idValue != "") {
                var extistingTags = $('#attachedTags > span[data-bendo-itemId="' + idValue + '"]');
                if (extistingTags.length < 1) {
                    var para = {
                        text: bendoAutocomplete.val(),
                        value: idValue
                    };
                    var bar = $("<span>").text("|").addClass("bar");
                    $("#attachedTags").append(bar);
                    $("#attachedTags").append(freshTag);
                    errorMessage.empty();
                    bendoAutocomplete.val("");
                    defaultSettings.buttonClick(para);
                } else {
                    errorMessage.text(defaultSettings.errorMessage);
                }
            } else {
                errorMessage.text(defaultSettings.errorMessage);
                console.log("Please select an item from the dropdown list");
            }
        } else {
            errorMessage.text(defaultSettings.errorMessage);
            console.log("Please enter value");
        }
    });

    container.append(autocompleteButton);
    container.css("max-width", autocompleteButton.width() + this.width() + 30).css("flex-wrap", "wrap");
    var resultsHolder = $("<div>").addClass("autocomplete-results-container").attr("id", "resultsContainer-" + this.attr("id")).css("height", "auto");
    var holderWidth = bendoAutocomplete.width();
    resultsHolder.css("overflow-y", "hidden");
    container.append(resultsHolder);
    //Get data from server
    var dataSource;
    $.ajax({
        url: defaultSettings.data.url,
        dataType: defaultSettings.data.dataType,
        method: defaultSettings.data.method,
        data: function () {
            if (defaultSettings.data.searchTerm === null) {
                return null;
            } else {
                return defaultSettings.data.searchTerm;
            }
        },
        success: function (data, status, xhr) {
            console.log(data.length);
            dataSource = data;
        },
        error: function () {
            alert("Could not retrieve data for auto-complete " + bendoAutocomplete.attr("id"));
        }
    });
    var errorMessage = $("<p>").addClass("text-warning");
    container.prepend(errorMessage);
    this.keyup(function (args) {
        var resultItems = $(resultsHolder.children());
        var activeIndex = resultsHolder.children("p.active").index();
        if (args.keyCode !== 40 && args.keyCode !== 38 && args.keyCode !== 13) {
            bendoAutocomplete.attr("data-autocomplete-value", "");
            var searchTerm = $(this).val();
            resultsHolder.empty().css("overflow-y","scroll");
            errorMessage.empty();
            $.each(dataSource, function (index, ele) {
                var match = ele[defaultSettings.data.textField].match(new RegExp(searchTerm, "ig"));
                if (match != "" && match !== null) {
                    var resultItem = $("<p>").text(ele[defaultSettings.data.textField]).attr("data-autocomplete-value", ele[defaultSettings.data.valueField]).addClass("autocomplete-results btn-primary");
                    resultItem.click(function (args) {
                        var clickedItem = $(args.target);
                        bendoAutocomplete.val(clickedItem.text());
                        bendoAutocomplete.attr("data-autocomplete-value", clickedItem.attr("data-autocomplete-value"));
                        closeSelections();
                    });
                    resultsHolder.append(resultItem);
                }
            });
            console.log(resultsHolder.height());
        } else if (args.keyCode === 40) { //Down arrow event
            if (resultItems !== null && activeIndex < resultItems.length - 1) {
                resultItems.removeClass("active");
                activeIndex += 1;
                $(resultItems[activeIndex]).addClass("active");
            }
        } else if (args.keyCode === 38 && activeIndex > -1) { //Up arrow event
            resultItems.removeClass("active");
            activeIndex -= 1;
            $(resultItems[activeIndex]).addClass("active");
        } else if (args.keyCode === 13) {
            selectResultItem();
        }

        function selectResultItem() {
            if (activeIndex > -1) {
                var activeItem = $(resultItems[activeIndex])
                var selectedText = activeItem.text();
                bendoAutocomplete.val(selectedText);
                bendoAutocomplete.attr("data-autocomplete-value", activeItem.attr("data-autocomplete-value"));
                closeSelections();
                console.log(activeItem);
            }
        }
    });


    container.blur(function (args) {
        closeSelections();
    });
    bendoAutocomplete.blur(function (args) {
        //closeSelections();
    });

    function closeSelections() {
        resultsHolder.empty();
        resultsHolder.css("overflow-y","hidden");
    }
};
