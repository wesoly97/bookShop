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
                    $("#count").html(count + " Egz.");
                    sum = sum - data.priceBook;
                    let roundedString = sum.toFixed(2);
                    sum = Number(roundedString);
                    $("#sum").html(sum+ " PLN");
                    if (data.quantity >0)
                    {
                        $("#quantity" + itemId).html(data.quantity +" Egz.");
                        $("#price" + itemId).html(data.price + " PLN");
                    }
                    else
                    {
                        $(".item" + itemId).remove();
                    }
                    if (count < 1)
                    {
                            $('.items').removeClass().addClass('items');
                        $(".items").html(`<div class="card">
    <div class="row">
        <div class="col-md-8 cart">
            <div class="title">
                <div class="row">
                    <div class="col">
                        <h4><b>Brak książek w koszyku</b></h4>
                   
    <div class="back-to-shop"><a href="/book">&leftarrow;<span class="text-muted"> Powrót do sklepu</span></a></div>

                    </div>

                </div>
            </div>
        </div>
    </div>
    </div>`);
                    }

                }
            }
        }

    });
}