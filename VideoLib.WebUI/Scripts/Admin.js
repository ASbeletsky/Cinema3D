
            $(document).on("click", ".open-DeleteConfirmDialog", function () {
                var FilmId = $(this).data('id');
                var FilmName = $(this).data('name');
                $(".modal-body").html('Вы действительно хотите удалить фильм "');
                $(".modal-body").append(FilmName);
                $(".modal-body").append('" ?');
                $(document).on("click", "#DeleteFilm", function () {
                    $.ajax({
                        type: "POST",
                        url: "/Admin/DeleteFilm",
                        data: { Id: FilmId },
                        success: function (data) {
                            window.location = "/Admin/FilmIndex";
                        }
                    });
                });
            });

    $(document).on("click", ".ajax-link", function () {
        $("#sideNavbar").height('180vh');
    });
function UseBootstrapControls() {
    $('.combobox').combobox();
    $('#releaseDatePicker').datetimepicker({
        format: "DD/MM/YYYY"
    });
    $('#timeDurationPicker').datetimepicker({
        format: 'HH:mm'
    });
    //$.validator.unobtrusive.parse(document);
}

    $(document).ready(function () {
        $('#sidePanel').BootSideMenu({
            side: "right", // left or right
            autoClose: true // auto close when page loads
        });
        $('.combobox').combobox();
        $('.combobox-side').combobox();
    });

function OnSuccess(data) {
    if (data.OK) {
        $('#alerts').removeClass('alert-info');
        $('#alerts').addClass('alert-success');
        $('#alerts').append('<p class = "text-center">Изменения успешно сохранены!</p>');
        $('#alerts').addClass('in');
        setTimeout(
          function () {
              location.reload();
          }, 3000);

    }
    else {
        $('#alerts').removeClass('alert-info');
        $('#alerts').addClass('alert-success');
        $('#alerts').append('<p class = "text-center">Ошибка! Изменения не вступили в силу.</p>');
        $('#alerts').addClass('in');
    }
}

$(function () {
    $('#GenreDropDown').change(function () {
        var selectedValue = $("#GenreDropDown :selected").text();
        $('#GenreTextBox').val(selectedValue);
    });
});
$(function () {
    $('#CompanyDropDown').change(function () {
        var selectedValue = $("#CompanyDropDown :selected").text();
        $('#CompanyTextBox').val(selectedValue);
    });
});
$(function () {
    $('#CountryDropDown').change(function () {
        var selectedValue = $("#CountryDropDown :selected").text();
        $('#CountryTextBox').val(selectedValue);
    });
});
$(function () {
    $('#AddGenre').click(function () {
        var name = $("#GenreTextBox").val();
        this.href = this.href.replace("xxx", name);
    });
});
$(function () {
    $('#AddCompany').click(function () {
        var name = $("#CompanyTextBox").val();
        this.href = this.href.replace("xxx", name);
    });
});
$(function () {
    $('#AddCountry').click(function () {
        var name = $("#CountryTextBox").val();
        this.href = this.href.replace("xxx", name);
    });
});
$(function () {
    $('#EditCompany').click(function () {
        $('#EditCompanyForm').submit();
    });
});
$(function () {
    $('#EditCountry').click(function () {
        $('#EditCountryForm').submit();
    });
});
$(function () {
    $('#EditGenre').click(function () {
        $('#EditGenreForm').submit();
    });
});
$(function () {
    $('#DeleteGenre').click(function () {
        var id = $('#GenreDropDown').val();
        this.href = this.href.replace("99", id);
    });
});
$(function () {
    $('#DeleteCompany').click(function () {
        var id = $('#CompanyDropDown').val();
        this.href = this.href.replace("99", id);
    });
});
$(function () {
    $('#DeleteCountry').click(function () {
        var id = $('#CountryDropDown').val();
        this.href = this.href.replace("99", id);
    });
});

