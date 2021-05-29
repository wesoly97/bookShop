
$(document).ready(function () {
    $('.form-select').change(function () {
        var SortingValue = $(this).val();
        $.ajax({
            url: '/book/changeSortingOption',
            type: 'POST',
            cache: false,
            data: { SortingValue: SortingValue },
            success: function (result)
            {
                alert(SortingValue);
                $(".Content").html(result);
            }
        });
    }); });
