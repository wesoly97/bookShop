function RemoveFromCart(item, itemId, userId) {

    let formData = new FormData();
    formData.append("itemId", itemId)
    formData.append("userId", userId)
    $.ajax({
        async: true,
        type: 'Post',
        contentType: false,
        processData: false,
        data: formData,
        url: '/book/removeFromCart',
        success: function (data) {
            if (data) {
                if (data.success) {
                   
                    count = count - 1;
                    $("#count").html(count);
                    sum = sum - data.priceBook;
                    let roundedString = sum.toFixed(2);
                    sum = Number(roundedString);
                    $("#sum").html(sum);
                    alert(data.quantity)
                    if (data.quantity >0)
                    {
                        $("#quantity" + itemId).html(data.quantity);
                        $("#price" + itemId).html(data.price + " PLN");
                    }
                    else
                    {
                        $(".item" + itemId).remove();
                    }
                    if (count < 1)
                    {
                            $('.items').removeClass().addClass('items');
                            $(".items").html("<h1>Twój koszyk jest pusty</h1>");
                    }
                   
                       

                }
            }
        }

    });
}