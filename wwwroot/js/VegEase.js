$(document).ready(function () {

    bindProductData('dealoftheday', 'deal-of-the-day');
    bindProductData('wonder', 'Wonder');
    bindProductData('everyday', 'everyday');
    bindProductData('check-this-out', 'checkthis');
    bindProductData('beat-the-heat', 'beat');
    bindProductData('ready-to-eat', 'readyToEat');
    bindProductData('citrus-burst', 'citrusBurst');

});

function bindProductData(category, targetId) {
    $.ajax({
       
        url: `http://localhost:5500/api/products/${category}`, 
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            console.log('Success:', response);

   
            var dealOftheDayArr = response; 
            let htmlContent = "";
            dealOftheDayArr.forEach(function (product) {
                htmlContent += '<div class="mainCard">';
                htmlContent += `<a href="http://localhost:5500/product-details?id=${product.id}">`;
                htmlContent += `<img class="imgCard" data-id="1" src="/images/${product.image}" alt="Card image cap">`;
                htmlContent += '</a>';
                htmlContent += '<div class="card-body cardbody1">';
                htmlContent += `<p class="cardP"> ${product.name}</p>`;
                htmlContent += `<p class="cardP">1 kg &nbsp; &#8377 ${product.price}</p>`;
                // htmlContent += `<span class="cardSpan1"> &#8377; student.price</span>`
                htmlContent += `<span class="cardSpan2">47% off</span> <br>`;
                htmlContent += `<button id="addToCard_${product.id}" class ="btnAddToCart" onclick ="funAdd_Sub(${product.id})"
                              data-id="${product.id}" 
                              data-name="${product.name}" 
                              data-price="${product.price}" 
                              data-image="${product.image}"">
                              <p class="btnP_${product.id}" id = "addP"> ADD </P> 
                            </button>`;

                htmlContent += `<div class="add-Sub" id="AddSub_${product.id}" style="display: none;">   
                              <i class="bi bi-plus plus" onclick="funAddRemoveProd(${product.id}, 'A')"></i>
                              <span id="cart_${product.id}">1</span>
                              <i class="bi bi-dash" onclick="funAddRemoveProd(${product.id}, 'D')"></i>
                            </div>`;
                htmlContent += '</div> </div>';
            });

            // Update the HTML content with the new product data
            $(`#${targetId}`).html(htmlContent);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching product data:', status, error);
        }
    });
}



$(document).ready(function () {
    $('#profileIcon').on('click', function (event) {
        event.preventDefault();
        $('#dropdownContent').toggle();
    });

    $(window).on('click', function (event) {
        if (!$(event.target).closest('#profileIcon').length) {
            $('#dropdownContent').hide();
        }
    });
});



function getProduct(category) {
    window.location.href = `/Category/Index?type=${encodeURIComponent(category)}`;
    /*    window.location.href = `/Category/CategoryTypes?type=${encodeURIComponent(category)}`; This is for the Model Calling Way*/

}

function funAdd_Sub(productId) {
    // Get product details from button attributes
    let btnAdd = $(`.btnAddToCart[data-id="${productId}"]`);
    let addSubDiv = $(`#AddSub_${productId}`);

    const product = {
        ProductId: parseInt(btnAdd.attr('data-id')),
        Name: btnAdd.attr('data-name'),
        Price: parseFloat(btnAdd.attr('data-price')),
        Image: btnAdd.attr('data-image'),
        Quantity: 1  
    };

    $.ajax({
        url: 'http://localhost:5500/api/Cart/add',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(product),
        success: function (response) {
            console.log('Product added to cart:', response);
            
        },
        error: function (xhr, status, error) {
            console.error('Error adding product to cart:', error);
            
        }
        
    });
   
    if (addSubDiv.is(':visible')) {
        addSubDiv.hide();
        btnAdd.show();
    } else {
        addSubDiv.show();
        btnAdd.hide();
    }
}





function toDefineCart() {
    var currentValue = parseInt(document.getElementById('cart-value').innerText);
    var newValue = currentValue + 1;
    document.getElementById('cart-value').innerText = newValue;
    var notification = document.getElementById('notification');
    notification.style.display = 'block';
    setTimeout(function () {
        notification.style.display = 'none';
    }, 3000);
}


$(document).on('click', '.btnAddToCart', function () {
    const product = {
        id: $(this).data('id'),
        name: $(this).data('name'),
        price: $(this).data('price'),
        image: $(this).data('image')
    };
    $(document).ready(function () {
        $('.cartItemCount').show()
    });

    console.log('Product to add:', product);
    addToCart(product);
    toDefineCart();
});
``
function addToCart(product) {
    let cart = JSON.parse(localStorage.getItem('cart'));
    cart.push(product);
    localStorage.setItem('cart', JSON.stringify(cart));
}


function funAddRemoveProd(productId, mode) {
    $.ajax({
        url: `/api/Cart/update?ProductId=${productId}&mode=${mode}`,
        method: 'PUT', // Changed from POST to PUT
        dataType: 'json',
        success: function (response) {
            console.log('Success:', response);
            let currentValue = parseInt($(`#cart_${productId}`).text());

            if (mode === 'A') {
                $(`#cart_${productId}`).text(currentValue + 1);
                updateCart(productId, currentValue + 1);
            } else if (mode === 'D' && currentValue > 1) {
                $(`#cart_${productId}`).text(currentValue - 1);
                updateCart(productId, currentValue - 1);
            } else if (mode === 'D' && currentValue === 1) {
                // If the quantity is 1 and mode is 'D', remove the product from the cart
                removeProductFromCart(productId);
                $(`#AddSub_${productId}`).hide();
                $(`#addToCard_${productId}`).show();
            }
            /*alert("Product added/removed successfully.");*/
        },
        error: function (xhr, status, error) {
            console.log('Error:', error);
            $("#addToCard_" + productId).show();
            $("#AddSub_" + productId).hide();

        }
    });
}


function updateCart(productId, quantity) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let product = cart.find(p => p.id == productId);
    if (product) {
        product.quantity = quantity;
    } else {
        cart.push({ id: productId, quantity: quantity });
    }
    localStorage.setItem('cart', JSON.stringify(cart));
}

var urlParams = new URLSearchParams(window.location.search);
var productId = urlParams.get('id');

function removeProductFromCart(productId) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    cart = cart.filter(p => p.id !== productId);
    localStorage.setItem('cart', JSON.stringify(cart));
}