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
    });
}