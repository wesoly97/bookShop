function AddToCart(item, itemId,userId)
{
    
    let formData = new FormData();
    formData.append("itemId", itemId)
    formData.append("userId", userId)
    $.ajax({
        async: true,
        type: 'Post',
        contentType: false,
        processData: false,
        data: formData,
        url: '/book/addToCart',
        success: function (data) {
            if (data) {
                if (data.success) {
                    if (!(typeof count == 'undefined'))
                    {
                        
                        $("#quantity"+itemId).html(data.quantity);
                        $("#price" + itemId).html(data.price + " PLN");
                        if (count + 1 <= data.quantityBook) {
                            count = count + 1;
                            $("#count").html(count);
                            sum = sum + data.priceBook;
                            let roundedString = sum.toFixed(2);
                            sum = Number(roundedString);
                            $("#sum").html(sum);
                        }
                    }
                 
                  
                }
            }
        }
   
    });
}