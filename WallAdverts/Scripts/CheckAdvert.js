$(function () {
        $("#Submit").on("click", function (e) {
            var nameAdv = $("#nameAdvert").val();
            var descAdv = $("#descriptionAdvert").val();
            var fileip = document.getElementById('fileUpload');
            var files = fileip.files

            if (files.length > 0) {
                if (window.FormData != undefined) {
                    var data = new FormData();
                    data.append("advert", files[0]);

                    $.ajax({
                        type: "POST",                       
                        url: '/Home/CreateAdvert/nameAdvert=' + nameAdv + "&descriptionAdvert=" + descAdv,
                        contentType: false,
                        processData: false,
                        success: function (result) {
                            console.log(result);
                        },
                        error: function (xhr, status, p3, p4) {
                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] == "{")
                                err = JSON.parse(xhr.responseText).Message;
                            console.log(err);
                        }
                    });
                }
                else {
                    alert("This browser doesn't support HTML5 file uploads!");
                }
            }
        });
    });
