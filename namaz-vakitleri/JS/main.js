$(document).ready(function () {
    $('.selectpicker').selectpicker({
        style: 'btn-default',
        liveSearchStyle: 'startsWith',
        width: '100%',
        size: '10'
    });

    $("#txtUlke").on('changed.bs.select', function (e) {
        var ulke = $("#txtUlke").val();
        $("#hfSehir").val("");
        $("#hfIlce").val("");

        if (ulke == "0") {
            $("#txtSehir").empty();
            $("#txtSehir").html("<option>Şehir Seçiniz...</option>").selectpicker('refresh');
        }
        else {
            SehirleriYukle(ulke);
        }
    });

    $("#txtSehir").on('changed.bs.select', function (e) {
        var ulke = $("#txtUlke").val();
        var sehir = $("#txtSehir").val();

        if (ulke == "ABD" || ulke == "KANADA" || ulke == "TÜRKİYE") {
            if (sehir == "0") {
                $("#txtIlce").empty();
                $("#txtIlce").html("<option>İlçe Seçiniz...</option>").selectpicker('refresh');
            }
            else {
                IlceleriYukle(ulke, sehir);
            }
        }
    });

    UlkeleriYukle();
    $(".loaderStore").fadeOut("slow");
});

function UlkeleriYukle() {
    $.ajax({
        type: "POST",
        url: "/namaz-vakitleri/NamazVakitleri.aspx/UlkeleriYukle",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{}',
        async: false,
        success: function (result) {
            var sonuc = JSON.parse(result.d)
            if (sonuc.Hata == "") {
                $("#txtUlke").empty();
                $("#txtUlke").append(sonuc.Ulkeler).selectpicker('render');
                var ulke = $("#hfUlke").val();
                $("#txtUlke").val(ulke).selectpicker('refresh');
                SehirleriYukle(ulke);
            }
            else {
                $("#txtUlke").empty();
                bootbox.alert(sonuc.Hata);
            }
        },
        error: function (xhr, status, error) {
            bootbox.alert(error);
        }
    });
}

function SehirleriYukle(ulke) {
    $.ajax({
        type: "POST",
        url: "/namaz-vakitleri/NamazVakitleri.aspx/SehirleriYukle",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{ulke:" + JSON.stringify(ulke) + "}",
        async: false,
        success: function (result) {
            var sonuc = JSON.parse(result.d)
            if (sonuc.Hata == "") {
                $("#txtSehir").empty();
                $("#txtSehir").append(sonuc.Sehirler).selectpicker('render');
                var sehir = $("#hfSehir").val();
                $("#txtSehir").val(sehir).selectpicker('refresh');


                if (ulke == "ABD" || ulke == "KANADA" || ulke == "TÜRKİYE") {
                    $("[data-id='txtIlce']").show();
                    $("#txtIlce").empty();
                    $("#txtIlce").append("<option>İlçe Seçiniz...</option>").selectpicker('render');
                    $("#txtIlce").val("0").selectpicker('refresh');

                    var sehirText = $("#txtSehir").val();
                    if (sehirText != "0") {
                        IlceleriYukle(ulke, sehir);
                    }
                }
                else {
                    $("[data-id='txtIlce']").hide();
                    $("#hfIlce").val("");
                    $("#txtIlce").empty();
                }


                if (sonuc.SagTarafIller != "") {
                    $("#sagTarafIller").empty();
                    $("#sagTarafIller").append(sonuc.SagTarafIller);
                }
            }
            else {
                $("#txtSehir").empty();
                bootbox.alert(sonuc.Hata);
            }
        },
        error: function (xhr, status, error) {
            bootbox.alert(error);
        }
    });
}

function IlceleriYukle(ulke, sehir) {
    $.ajax({
        type: "POST",
        url: "/namaz-vakitleri/NamazVakitleri.aspx/IlceleriYukle",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{ulke: " + JSON.stringify(ulke) + ", sehir:" + JSON.stringify(sehir) + "}",
        async: false,
        success: function (result) {
            var sonuc = JSON.parse(result.d)
            if (sonuc.Hata == "") {
                $("#txtIlce").empty();
                $("#txtIlce").append(sonuc.Ilceler).selectpicker('render');
                var ilce = $("#hfIlce").val();
                $("#txtIlce").val(ilce).selectpicker('refresh');

                $("#sagTarafIller").empty();
                $("#sagTarafIller").append(sonuc.SagTarafIller);
            }
            else {
                $("#txtIlce").empty();
                bootbox.alert(sonuc.Hata);
            }
        },
        error: function (xhr, status, error) {
            bootbox.alert(error);
        }
    });
}

function Ara() {
    var ulke = $('#txtUlke option:selected').text();
    var sehir = $('#txtSehir option:selected').text();
    var ilce = $('#txtIlce option:selected').text();

    if (ulke == "ABD" || ulke == "KANADA" || ulke == "TÜRKİYE") {
        if (ulke == "" || sehir == "" || ilce == "") {
            bootbox.alert("Ülke - Şehir - İlçe seçiniz!");
            return;
        }
    }
    else {
        if (ulke == "" || sehir == "") {
            bootbox.alert("Ülke - Şehir seçiniz!");
            return;
        }
    }
    $(".loaderStore").show();
    $.ajax({
        type: "POST",
        url: "/namaz-vakitleri/NamazVakitleri.aspx/AramaUrlDon",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{ulke:" + JSON.stringify(ulke) + ",sehir:" + JSON.stringify(sehir) + ",ilce:" + JSON.stringify(ilce) + "}",
        async: true,
        success: function (result) {
            window.location.href = result.d;
        },
        error: function (xhr, status, error) {
            bootbox.alert(error);
        }
    });
}