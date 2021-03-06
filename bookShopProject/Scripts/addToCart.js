const formatter = new Intl.NumberFormat('en-US', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
});
function AddToCart(item, itemId, userId)
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
                        
                        $("#quantity" + itemId).html(data.quantity + " Egz.");
                        $("#price" + itemId).html(data.price + " PLN");
                        if (count + 1 <= data.quantityBook) {
                            count = count + 1;
                            $("#count").html(count + " Egz.");
                            sum = sum + data.priceBook;
                            sum = parseFloat(sum.toFixed(2))
                            $("#sum").html(sum+" PLN");
                        }
                    }
                 
                  
                }
            }
        }
   
    });
}