// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function updateCartCount() {
    $.ajax({
        url: '/Cart/GetCartCount', // Make sure the URL matches your route setup
        type: 'GET',
        success: function (response) {
            if (response !== undefined) {
                document.getElementById("cart-count").textContent = response; // Use response directly if it returns the count
            }
        },
        error: function () {
            console.log('Error updating cart count');
        }
    });
}
