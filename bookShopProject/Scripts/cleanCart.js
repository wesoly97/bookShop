function CleanCart() {
    $.ajax({
        async: true,
        type: 'Post',
        contentType: false,
        processData: false,
        url: '/book/cleanCart',
        success: function (data)
        {
            if (data)
            {
                $('.items').removeClass().addClass('items');
                $(".items").html("<h1>Twój koszyk jest pusty</h1>");
            }
        }    
    });
}